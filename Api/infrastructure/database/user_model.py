from sqlalchemy import Column, Integer, String

from infrastructure.database.base import Base


class UserModel(Base):
    __tablename__ = 'users'

    id = Column(Integer, primary_key=True)
    name = Column(String, unique=True, nullable=False)
    hashed_password = Column(String, nullable=False)
