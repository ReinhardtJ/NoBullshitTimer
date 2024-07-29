from datetime import datetime, timezone, timedelta
from typing import Annotated

import jwt
from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2
from jwt import InvalidTokenError
from passlib.context import CryptContext

from domain.user import User
from infrastructure.repositories.user_repository import UserRepository


def init_authenticator(
        oauth2_scheme: OAuth2,
        crypt_context: CryptContext,
        user_repository: UserRepository,
        secret_key: str,
        algorithm: str,
):
    def verify_password(plain_password, hashed_password):
        return crypt_context.verify(plain_password, hashed_password)

    def get_password_hash(password):
        return crypt_context.hash(password)

    async def authenticate_user(username: str, password: str) -> User | None:
        user = user_repository.get_user(username)
        if user.is_err():
            return None
        user = user.as_ok()
        if not verify_password(password, user.hashed_password):
            return None
        return user

    def create_encoded_jwt_access_token(data: dict, expires_delta: timedelta | None = None) -> str:
        to_encode = data.copy()
        if expires_delta:
            expire = datetime.now(timezone.utc) + expires_delta
        else:
            expire = datetime.now(timezone.utc) + timedelta(minutes=15)
        to_encode.update({'exp': expire})
        encoded_jwt = jwt.encode(to_encode, secret_key, algorithm=algorithm)
        return encoded_jwt

    async def get_current_user(token: Annotated[str, Depends(oauth2_scheme)]) -> User | None:
        credentials_exception = HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail='Invalid authentication credentials',
            headers={'WWW-Authenticate': 'Bearer'}
        )
        try:
            payload = jwt.decode(token, secret_key, algorithms=[algorithm])
            username: str = payload.get('sub')
            if username is None:
                raise credentials_exception
        except InvalidTokenError:
            raise credentials_exception
        user = user_repository.get_user(username)
        if user.is_err():
            raise credentials_exception
        return user.as_ok()

    return get_current_user, create_encoded_jwt_access_token, authenticate_user
