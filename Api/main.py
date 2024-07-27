from fastapi import FastAPI
from fastapi.security import OAuth2PasswordBearer
from passlib.context import CryptContext

from api.auth_api import add_auth_api
from infrastructure.auth.auth_logic import init_authenticator
from infrastructure.repositories.user_repository import UserRepository

SECRET_KEY = '3316e78ab1c67a55640b7b00a937d18decea2f40d643cc508675e5f9793d174f'  # todo: change
ALGORITHM = 'HS256'
ACCESS_TOKEN_EXPIRE_MINUTES = 30

app = FastAPI()

pwd_context = CryptContext(schemes=['bcrypt'], deprecated='auto')
oauth2_scheme = OAuth2PasswordBearer(tokenUrl='login')
user_repo = UserRepository()

(get_current_user,
 create_encoded_jwt_access_token,
 authenticate_user) = init_authenticator(
    oauth2_scheme, pwd_context, user_repo, SECRET_KEY, ALGORITHM
)

add_auth_api(
    app,
    ACCESS_TOKEN_EXPIRE_MINUTES,
    get_current_user,
    create_encoded_jwt_access_token,
    authenticate_user
)

@app.get("/")
async def root():
    return {"message": "Hello World"}


@app.get("/hello/{name}")
async def say_hello(name: str):
    return {"message": f"Hello {name}"}
