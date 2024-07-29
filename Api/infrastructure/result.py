from abc import abstractmethod
from typing import TypeVar, Generic

from attr import define

T = TypeVar('T')
E = TypeVar('E')

@define
class InvalidResultException(BaseException):
    reason: str


@define
class Result(Generic[T, E]):
    _value: T | E = None

    @abstractmethod
    def is_ok(self) -> bool: pass

    @abstractmethod
    def is_err(self) -> bool: pass

    @abstractmethod
    def as_ok(self) -> T: pass

    @abstractmethod
    def as_err(self) -> E: pass

    def ensure_ok(self):
        if self.is_ok():
            return
        else:
            raise InvalidResultException('tried ensuring Ok on Err-result')


@define
class Err(Result, Generic[T, E]):
    def as_ok(self) -> T: raise InvalidResultException('tried to unwrap Err-result as Ok')

    def as_err(self) -> E: return self._value

    def is_err(self): return True

    def is_ok(self): return False


@define
class Ok(Result, Generic[T, E]):
    def as_ok(self) -> T: return self._value

    def as_err(self) -> E: raise InvalidResultException('tried to unwrap Ok-result as Err')

    def is_err(self): return False

    def is_ok(self): return True


@define
class ExceptionErr(Err[T, BaseException]):
    def ensure_ok(self):
        raise self._value
