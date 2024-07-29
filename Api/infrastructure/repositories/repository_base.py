from abc import ABC
from contextlib import contextmanager
from typing import ContextManager, Callable, Generator

from attr import define
from sqlalchemy.orm import Session

from infrastructure.container import Container
from infrastructure.result import Ok, Result, Err


@define
class SqlAlchemyRepository(ABC):
    session_factory: Callable[[], Session]

    @contextmanager
    def get_session(self) -> Generator[tuple[Session, Container[Result]], None, None]:
        session = self.session_factory()
        result = Container(Ok())
        try:
            yield session, result
            session.commit()
        except BaseException as e:
            session.rollback()
            result.value = Err(e)
        finally:
            session.close()
