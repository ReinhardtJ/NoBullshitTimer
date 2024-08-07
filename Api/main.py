import os
from contextlib import asynccontextmanager

from dotenv import load_dotenv
from fastapi import FastAPI
from passlib.context import CryptContext

from api.auth_api import add_auth_api
from api.common_api import add_index
from infrastructure.auth import PasswordService
from infrastructure.database.util import init_async_sqlalchemy
from infrastructure.repositories.user_repository import UserRepository
from service.auth import AuthService
from setup_test_environment import add_test_users


class Config:
    database_url: str
    environment: str


config = Config()


@asynccontextmanager
async def lifespan(api: FastAPI):
    SECRET_KEY = '3316e78ab1c67a55640b7b00a937d18decea2f40d643cc508675e5f9793d174f'  # todo: change
    ALGORITHM = 'HS256'
    ACCESS_TOKEN_EXPIRE_MINUTES = 30

    load_dotenv()

    global config
    config.database_url = os.getenv("DATABASE_URL")
    config.environment = os.getenv("ENVIRONMENT")

    engine, session_factory = await init_async_sqlalchemy(
        config.database_url, echo=config.environment == 'dev'
    )

    user_repo = UserRepository(session_factory)

    crypt_context = CryptContext(schemes=['bcrypt'], deprecated='auto')
    password_service = PasswordService(crypt_context)
    auth_service = AuthService(password_service, user_repo, SECRET_KEY, ALGORITHM)

    add_index(api)
    add_auth_api(api, ACCESS_TOKEN_EXPIRE_MINUTES, auth_service)

    if config.environment == 'dev':
        await add_test_users(user_repo, password_service)
    yield


app = FastAPI(lifespan=lifespan)
