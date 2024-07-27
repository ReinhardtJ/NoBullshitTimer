from datetime import timedelta
from typing import Annotated

from fastapi import FastAPI, HTTPException, Depends, status
from fastapi.security import OAuth2PasswordRequestForm
from pydantic import BaseModel


class TokenDto(BaseModel):
    access_token: str
    token_type: str


class UserDto(BaseModel):
    username: str


def add_auth_api(
        app: FastAPI,
        access_token_expiration_time_mins: int,
        get_current_user,
        create_encoded_jwt_access_token,
        authenticate_user
):
    @app.get('/users/me')
    async def read_users_me(
            current_user: Annotated[UserDto, Depends(get_current_user)]) -> UserDto:
        return current_user

    @app.post('/login')
    async def login(form_data: Annotated[OAuth2PasswordRequestForm, Depends()]) -> TokenDto:
        user = authenticate_user(form_data.username, form_data.password)
        if not user:
            raise HTTPException(
                status_code=status.HTTP_401_UNAUTHORIZED,
                detail='Invalid authentication credentials',
                headers={'WWW-Authenticate': 'Bearer'}
            )
        access_token_expires = timedelta(minutes=access_token_expiration_time_mins)
        access_token = create_encoded_jwt_access_token(
            data={'sub': user.name},
            expires_delta=access_token_expires
        )
        return TokenDto(access_token=access_token, token_type='bearer')
