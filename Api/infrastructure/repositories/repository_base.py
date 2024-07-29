from abc import ABC
from contextlib import contextmanager
from typing import Callable, Generator

from attr import define
from sqlalchemy.orm import Session


@define
class SqlAlchemyRepository(ABC):
    session_factory: Callable[[], Session]

    @contextmanager
    def get_session(self) -> Generator[Session, None, None]:
        session = self.session_factory()
        try:
            yield session
            session.commit()
        except:
            session.rollback()
            raise
        finally:
            session.close()
