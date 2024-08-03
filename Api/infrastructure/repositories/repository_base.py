from abc import ABC
from contextlib import contextmanager, asynccontextmanager
from typing import Callable, Generator, AsyncGenerator

from attr import define
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy.orm import Session


@define
class SqlAlchemyRepository(ABC):
    session_factory: Callable[[], AsyncSession]

    @asynccontextmanager
    async def get_session(self) -> AsyncGenerator[Session, None]:
        session = self.session_factory()
        try:
            yield session
            await session.commit()
        except:
            await session.rollback()
            raise
        finally:
            await session.close()
