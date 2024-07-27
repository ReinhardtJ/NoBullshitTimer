import attr

from domain.user import User

fake_users_db = {
    "johndoe": {
        "username": "johndoe",
        "hashed_password": "$2b$12$EixZaYVK1fsbw1ZfbX3OXePaWxn96p36WQoeG6Lruj3vjPGga31lW",
    },
}

@attr.s
class UserRepository:
    def get_user(self, name: str) -> User | None:
        user_data = fake_users_db.get(name)
        if not user_data:
            return None
        return User(name=user_data['username'], hashed_password=user_data['hashed_password'])

