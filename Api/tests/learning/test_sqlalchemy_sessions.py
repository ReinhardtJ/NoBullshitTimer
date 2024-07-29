from contextlib import contextmanager
from typing import Callable, ContextManager, Generator

import pytest
from sqlalchemy import create_engine
from sqlalchemy.exc import IntegrityError
from sqlalchemy.orm import sessionmaker, Session

from infrastructure.container import Container
from infrastructure.database.base import Base
from infrastructure.database.user_model import UserModel
from infrastructure.result import Result


@pytest.fixture
def get_session() -> Callable[[], ContextManager[Session]]:
    db_url = 'sqlite:///:memory:'
    engine = create_engine(db_url, echo=True)
    Base.metadata.create_all(engine)
    session_factory = sessionmaker(bind=engine)

    # recommended usage from docs
    @contextmanager
    def session_contextmanager() -> Generator[Session, None, None]:
        session = session_factory()
        try:
            yield session
            session.commit()
        except:
            session.rollback()
            raise
        finally:
            session.close()

    yield session_contextmanager
    Base.metadata.drop_all(engine)


# the following snippets run, because we are trying to insert the identical
# object into the database. SqlAlchemy keeps an identitiy map of the objects
# stored in a session and therefore handles the attempts to save duplicates
# gracefully


def test_add_same_user_twice_manually_and_flush(get_session):
    with get_session() as session:
        session: Session
        user_model = UserModel(name='some_user', hashed_password='some_hashedpassword')
        session.add(user_model)
        session.add(user_model)
        session.flush()


def test_add_same_user_twice_manually_and_commit(get_session):
    with get_session() as session:
        session: Session
        user_model = UserModel(name='some_user', hashed_password='some_hashedpassword')
        session.add(user_model)
        session.add(user_model)
        session.commit()


def test_add_same_user_twice_manually_and_flush_in_between(get_session):
    with get_session() as session:
        session: Session
        user_model = UserModel(name='some_user', hashed_password='some_hashedpassword')
        session.add(user_model)
        session.flush()
        session.add(user_model)
        session.flush()


def test_add_same_user_twice_manually_and_commit_in_between(get_session):
    with get_session() as session:
        session: Session
        user_model = UserModel(name='some_user', hashed_password='some_hashedpassword')
        session.add(user_model)
        session.commit()
        session.add(user_model)
        session.commit()


def test_add_copied_user_twice(get_session):
    user_model = UserModel(name='some_user', hashed_password='some_hashedpassword')
    user_model_copy = UserModel(name='some_user', hashed_password='some_hashedpassword')

    try:
        with get_session() as session:
            session.add(user_model)
            session.add(user_model_copy)
            session.flush()
            pytest.fail()
    except IntegrityError:
        pass


