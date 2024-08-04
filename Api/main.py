import asyncio
import os
from contextlib import asynccontextmanager

from dotenv import load_dotenv
from fastapi import FastAPI
from fastapi.responses import HTMLResponse
from passlib.context import CryptContext

from api.auth_api import add_auth_api
from infrastructure.auth import PasswordService
from infrastructure.database.util import init_async_sqlalchemy
from infrastructure.repositories.user_repository import UserRepository
from service.auth import AuthService

@asynccontextmanager
async def lifespan(app: FastAPI):
    SECRET_KEY = '3316e78ab1c67a55640b7b00a937d18decea2f40d643cc508675e5f9793d174f'  # todo: change
    ALGORITHM = 'HS256'
    ACCESS_TOKEN_EXPIRE_MINUTES = 30

    load_dotenv()

    DATABASE_URL = os.getenv("DATABASE_URL")
    if DATABASE_URL.startswith("postgresql://"):
        DATABASE_URL = DATABASE_URL.replace("postgresql://", "postgresql+asyncpg://", 1)

    engine, session_factory = await init_async_sqlalchemy(DATABASE_URL)

    user_repo = UserRepository(session_factory)

    crypt_context = CryptContext(schemes=['bcrypt'], deprecated='auto')
    password_service = PasswordService(crypt_context)
    auth_service = AuthService(password_service, user_repo, SECRET_KEY, ALGORITHM)

    add_auth_api(app, ACCESS_TOKEN_EXPIRE_MINUTES, auth_service)

    @app.get("/", response_class=HTMLResponse)
    async def root():
        return HTMLResponse(status_code=200, content="""
    <h1>NoBullshitTimer API. Visit</h1>
    <p>To use the app, visit <a href="https://timer.reinhardt.ai">timer.reinhardt.ai</a></p>
    <p>To visit the API documentation, visit <a href="http://localhost:8000/docs">api.timer.reinhardt.ai</a></p>
    """)

    @app.get("/hello/{name}")
    async def say_hello(name: str):
        return {"message": f"Hello {name}"}

    yield
app = FastAPI(lifespan=lifespan)
