from unittest.mock import Mock

import pytest
from pytest_mock import MockFixture

from domain.user import User
from infrastructure.result import Err, Ok
from service.auth import AuthService


@pytest.fixture
def password_service_mock(mocker: MockFixture):
    return mocker.Mock()


@pytest.fixture
def user_repository_mock(mocker: MockFixture):
    return mocker.AsyncMock()


@pytest.fixture
def some_secret_key():
    return 'some_secret_key'


@pytest.fixture
def some_algorithm():
    return 'some_algorithm'


@pytest.mark.asyncio
async def test_authenticate_user_err_returns_none(
        password_service_mock,
        user_repository_mock,
        some_secret_key,
        some_algorithm
):
    user_repository_mock.get_user.return_value = Err()
    auth_service = AuthService(
        password_service_mock,
        user_repository_mock,
        some_secret_key,
        some_algorithm
    )
    actual = await auth_service.authenticate_user('some_user', 'some_password')
    assert actual is None


@pytest.mark.asyncio
async def test_authenticate_user_incorrect_password_returns_none(
        password_service_mock,
        user_repository_mock,
        some_secret_key,
        some_algorithm
):
    password_service_mock.verify_password.return_value = False
    user_repository_mock.get_user.return_value = Ok(
        User('some_user', 'some_passwordhash'))
    auth_service = AuthService(
        password_service_mock,
        user_repository_mock,
        some_secret_key,
        some_algorithm
    )
    actual = await auth_service.authenticate_user('some_user', 'some_password')
    assert actual is None


@pytest.mark.asyncio
async def test_authenticate_user_success(
        password_service_mock,
        user_repository_mock,
        some_secret_key,
        some_algorithm
):
    password_service_mock.verify_password.return_value = True
    user_repository_mock.get_user.return_value = Ok(
        User('some_user', 'some_passwordhash'))
    auth_service = AuthService(
        password_service_mock,
        user_repository_mock,
        some_secret_key,
        some_algorithm
    )
    actual = await auth_service.authenticate_user('some_user', 'some_password')
    assert actual.name == 'some_user'
    assert actual.hashed_password == 'some_passwordhash'
