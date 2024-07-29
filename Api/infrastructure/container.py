from typing import TypeVar, Generic

from attr import define

T = TypeVar('T')


@define
class Container(Generic[T]):
    value: T
