from domain.user import User
from infrastructure.auth import PasswordService
from infrastructure.repositories.user_repository import UserRepository


async def add_test_users(repo: UserRepository, pwd_service: PasswordService):
    print('adding test users...')
    await repo.add_user(User('johndoe', hashed_password=pwd_service.get_password_hash('secret')))
    await repo.add_user(User('admin', hashed_password=pwd_service.get_password_hash('admin')))
