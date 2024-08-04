from attr import define
from fastapi.security import OAuth2PasswordBearer
from passlib.context import CryptContext

oauth2_scheme = OAuth2PasswordBearer(tokenUrl='login')


@define
class PasswordService:
    crypt_context: CryptContext

    def verify_password(self, plain_password, hashed_password) -> bool:
        return self.crypt_context.verify(plain_password, hashed_password)

    def get_password_hash(self, password) -> str:
        return self.crypt_context.hash(password)
