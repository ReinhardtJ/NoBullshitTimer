from attr import define


@define
class User:
    name: str
    hashed_password: str
