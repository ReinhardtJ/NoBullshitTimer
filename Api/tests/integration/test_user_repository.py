import pytest
import pytest_asyncio

from domain.user import User
from infrastructure.database.util import init_async_sqlalchemy, delete_async_sqlalchemy
from infrastructure.repositories.user_repository import UserRepository, \
    UserRepositoryError


@pytest_asyncio.fixture
async def user_repo():
    engine, session_factory = await init_async_sqlalchemy('sqlite+aiosqlite:///:memory:', True)
    repo = UserRepository(session_factory)
    yield repo
    await delete_async_sqlalchemy(engine)
    await engine.dispose()


@pytest.mark.asyncio
async def test_add_and_get_user(user_repo: UserRepository):
    user = User(name='testuser', hashed_password='hashedpassword')
    add_result = await user_repo.add_user(user)
    add_result.ensure_ok()

    fetch_result = await user_repo.get_user(user.name)
    assert fetch_result.is_ok()


@pytest.mark.asyncio
async def test_delete_user(user_repo: UserRepository):
    user = User(name='testuser', hashed_password='hashedpassword')
    add_result = await user_repo.add_user(user)
    add_result.ensure_ok()

    await user_repo.delete_user(user.name)


@pytest.mark.asyncio
async def test_delete_non_existing_user_is_okay(user_repo: UserRepository):
    await user_repo.delete_user('testuser')


@pytest.mark.asyncio
async def test_add_existing_user(user_repo: UserRepository):
    user = User(name='testuser', hashed_password='hashedpassword')
    add_result = await user_repo.add_user(user)
    add_result.ensure_ok()
    add_result = await user_repo.add_user(user)
    assert add_result.is_err()
    assert add_result.as_err() == UserRepositoryError.UserAlreadyExists


@pytest.mark.asyncio
async def test_get_non_existing_user(user_repo: UserRepository):
    get_result = await user_repo.get_user('testuser')
    assert get_result.is_err()
    assert get_result.as_err() == UserRepositoryError.UserNotFound
