from datetime import timedelta, datetime, timezone
from typing import Annotated

import jwt
from attr import define
from fastapi import Depends, HTTPException
from jwt import InvalidTokenError
from passlib.context import CryptContext
from starlette import status

from domain.user import User
from infrastructure.auth import oauth2_scheme
from infrastructure.repositories.user_repository import UserRepository


@define
class AuthService:
    crypt_context: CryptContext
    user_repository: UserRepository
    secret_key: str
    algorithm: str

    def verify_password(self, plain_password, hashed_password):
        return self.crypt_context.verify(plain_password, hashed_password)

    def get_password_hash(self, password):
        return self.crypt_context.hash(password)

    async def authenticate_user(self, username: str, password: str) -> User | None:
        user = self.user_repository.get_user(username)
        if user.is_err():
            return None
        user = user.as_ok()
        if not self.verify_password(password, user.hashed_password):
            return None
        return user

    def create_encoded_jwt_access_token(self, data: dict,
                                        expires_delta: timedelta | None = None) -> str:
        to_encode = data.copy()
        if expires_delta:
            expire = datetime.now(timezone.utc) + expires_delta
        else:
            expire = datetime.now(timezone.utc) + timedelta(minutes=15)
        to_encode.update({'exp': expire})
        encoded_jwt = jwt.encode(to_encode, self.secret_key, algorithm=self.algorithm)
        return encoded_jwt

    async def get_current_user(self, token: Annotated[str, Depends(oauth2_scheme)]) -> User | None:
        credentials_exception = HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail='Invalid authentication credentials',
            headers={'WWW-Authenticate': 'Bearer'}
        )
        try:
            payload = jwt.decode(token, self.secret_key, algorithms=[self.algorithm])
            username: str = payload.get('sub')
            if username is None:
                raise credentials_exception
        except InvalidTokenError:
            raise credentials_exception
        user = self.user_repository.get_user(username)
        if user.is_err():
            raise credentials_exception
        return user.as_ok()
