from enum import Enum

from attr import define
from sqlalchemy import select
from sqlalchemy.exc import IntegrityError
from sqlalchemy.ext.asyncio import AsyncSession

from domain.user import User
from infrastructure.database.user_model import UserModel
from infrastructure.repositories.repository_base import SqlAlchemyRepository
from infrastructure.result import Result, Ok, Err

fake_users_db = {
    "johndoe": {
        "username": "johndoe",
        "hashed_password": "$2b$12$EixZaYVK1fsbw1ZfbX3OXePaWxn96p36WQoeG6Lruj3vjPGga31lW",
    },
}


class UserRepositoryError(Enum):
    UserNotFound = 0
    UserAlreadyExists = 1


@define
class UserRepository(SqlAlchemyRepository):
    async def get_user(self, name: str) -> Result[User, UserRepositoryError]:
        async with self.get_session() as session:
            session: AsyncSession
            statement = select(UserModel).where(UserModel.name == name)
            result = await session.execute(statement)
            db_user: UserModel | None = result.scalars().first()
            if db_user:
                return Ok(User(db_user.name, db_user.hashed_password))
            else:
                return Err(UserRepositoryError.UserNotFound)

    async def add_user(self, user: User) -> Result[None, UserRepositoryError]:
        try:
            async with self.get_session() as session:
                session: AsyncSession
                session.add(UserModel(name=user.name, hashed_password=user.hashed_password))
            return Ok()
        except IntegrityError:
            return Err(UserRepositoryError.UserAlreadyExists)

    async def delete_user(self, name: str):
        async with self.get_session() as session:
            session: AsyncSession
            statement = select(UserModel).where(UserModel.name == name)
            result = await session.execute(statement)
            user: UserModel | None = result.scalar_one_or_none()
            if user:
                await session.delete(user)
