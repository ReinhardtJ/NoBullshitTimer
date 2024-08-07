from typing import Callable

from sqlalchemy.ext.asyncio import AsyncSession, create_async_engine, async_sessionmaker, \
    AsyncEngine, AsyncConnection

from infrastructure.database.base import Base

AsyncSessionFactory = Callable[[], AsyncSession]


async def init_async_sqlalchemy(db_url: str, echo: bool) -> tuple[AsyncEngine, AsyncSessionFactory]:
    engine = create_async_engine(db_url, echo=echo)
    async with engine.begin() as conn:
        conn: AsyncConnection
        await conn.run_sync(Base.metadata.create_all)

    session_factory = async_sessionmaker(bind=engine)
    return engine, session_factory


async def delete_async_sqlalchemy(engine: AsyncEngine):
    async with engine.begin() as conn:
        conn: AsyncConnection
        await conn.run_sync(Base.metadata.drop_all)
