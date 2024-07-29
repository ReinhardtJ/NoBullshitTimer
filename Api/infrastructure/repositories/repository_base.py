from abc import ABC
from contextlib import contextmanager
from typing import ContextManager, Callable

from attr import define
from sqlalchemy.orm import Session

from infrastructure.result import Ok, Result, Err


@define
class SqlAlchemyRepository(ABC):
    session_factory: Callable[[], Session]

    @contextmanager
    def get_session(self) -> ContextManager[Session, Result]:
        session = self.session_factory()
        result = Ok()
        try:
            yield session, result
            session.commit()
        except BaseException as e:
            session.rollback()
            result = Err(e)
        finally:
            session.close()
