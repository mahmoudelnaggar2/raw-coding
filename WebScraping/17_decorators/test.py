def my_func(p):
    print(p)
    return p

my_func("hello world")
print("---")

def my_before_after(fnc, p):
    print("before")
    result = fnc(p)
    print("after")
    return result

my_before_after(my_func, "hello world")
print("---")

def my_decorator(fnc):
    print("before local")

    def local(*args, **kwargs):
        print("before")
        result = fnc(*args, **kwargs)
        print("after")
        return result

    print("after local")
    return local

my_before_after_decorated = my_decorator(my_func)
my_before_after_decorated("hello world")
print("---")

my_decorator(my_func)("hello world")
print("---")

@my_decorator
def my_func_2(p):
    print(p)
    return p

my_func_2("hello world")
print("---")

@my_decorator
class Person:

    def __init__(self):
        print("init")
        self.name = "bob"


    def __call__(self, *args, **kwds):
        return "test test"
    

p = Person()
print(vars(p))
print(p())