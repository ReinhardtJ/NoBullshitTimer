import os

from dotenv import load_dotenv
from fastapi import FastAPI
from fastapi.security import OAuth2PasswordBearer
from passlib.context import CryptContext
from sqlalchemy import create_engine
from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession
from sqlalchemy.orm import sessionmaker

from api.auth_api import add_auth_api
from service.auth import AuthService
from infrastructure.auth import PasswordService
from infrastructure.repositories.user_repository import UserRepository

SECRET_KEY = '3316e78ab1c67a55640b7b00a937d18decea2f40d643cc508675e5f9793d174f'  # todo: change
ALGORITHM = 'HS256'
ACCESS_TOKEN_EXPIRE_MINUTES = 30

app = FastAPI()



load_dotenv()

DATABASE_URL = os.getenv("DATABASE_URL")
if DATABASE_URL.startswith("postgresql://"):
    DATABASE_URL = DATABASE_URL.replace("postgresql://", "postgresql+asyncpg://", 1)

engine = create_async_engine(DATABASE_URL, echo=True)
AsyncSessionLocal = sessionmaker(bind=engine, class_=AsyncSession, expire_on_commit=False)

user_repo = UserRepository(session_factory=AsyncSessionLocal)

crypt_context = CryptContext(schemes=['bcrypt'], deprecated='auto')
password_service = PasswordService(crypt_context)
auth_service = AuthService(password_service, user_repo, SECRET_KEY, ALGORITHM)

add_auth_api(app, ACCESS_TOKEN_EXPIRE_MINUTES, auth_service)

@app.get("/")
async def root():
    return {"message": "Hello World"}


@app.get("/hello/{name}")
async def say_hello(name: str):
    return {"message": f"Hello {name}"}
