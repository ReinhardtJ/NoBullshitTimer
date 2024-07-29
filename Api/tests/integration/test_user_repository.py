import pytest
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

from domain.user import User
from infrastructure.database.base import Base
from infrastructure.repositories.user_repository import UserRepository, UserRepositoryError


@pytest.fixture
def user_repo():
    db_url = 'sqlite:///:memory:'
    engine = create_engine(db_url, echo=True)
    Base.metadata.create_all(engine)
    session_factory = sessionmaker(bind=engine)
    repo = UserRepository(session_factory)
    yield repo
    Base.metadata.drop_all(engine)


def test_add_and_get_user(user_repo: UserRepository):
    user = User(name='testuser', hashed_password='hashedpassword')
    user_repo.add_user(user).ensure_ok()

    fetch_result = user_repo.get_user(user.name)
    assert fetch_result.is_ok()


def test_delete_user(user_repo: UserRepository):
    user = User(name='testuser', hashed_password='hashedpassword')
    user_repo.add_user(user).ensure_ok()

    user_repo.delete_user(user.name)


def test_delete_non_existing_user_is_okay(user_repo: UserRepository):
    user_repo.delete_user('testuser')


def test_add_existing_user(user_repo: UserRepository):
    user = User(name='testuser', hashed_password='hashedpassword')
    user_repo.add_user(user).ensure_ok()
    add_result = user_repo.add_user(user)
    assert add_result == UserRepositoryError.UserAlreadyExists


def test_get_non_existing_user(user_repo: UserRepository):
    get_result = user_repo.get_user('testuser')
    assert get_result == UserRepositoryError.UserNotFound
