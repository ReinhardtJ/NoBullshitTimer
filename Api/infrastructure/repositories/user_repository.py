from enum import Enum

from attr import define
from sqlalchemy import select
from sqlalchemy.exc import IntegrityError
from sqlalchemy.orm import Session

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
    def get_user(self, name: str) -> Result[User, UserRepositoryError]:
        with self.get_session() as session:
            statement = select(UserModel).where(UserModel.name == name)
            db_user: UserModel | None = session.execute(statement).scalars().first()
            if db_user:
                return Ok(User(db_user.name, db_user.hashed_password))
            else:
                return Err(UserRepositoryError.UserNotFound)

    def add_user(self, user: User) -> Result[None, UserRepositoryError]:
        try:
            with self.get_session() as session:
                session.add(UserModel(name=user.name, hashed_password=user.hashed_password))
            return Ok()
        except IntegrityError:
            return Err(UserRepositoryError.UserAlreadyExists)

    def delete_user(self, name: str):
        with self.get_session() as session:
            session: Session
            statement = select(UserModel).where(UserModel.name == name)
            user: UserModel | None = session.execute(statement).scalar_one_or_none()
            if user:
                session.delete(user)
