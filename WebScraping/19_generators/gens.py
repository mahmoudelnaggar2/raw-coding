def list_n(n):
    result = []
    i = 0

    while i <= n:
        result.append(i)
        i += 1

    return result


class Gen:

    def __init__(self, limit):
        self.limit = limit
        self.current = None

    def next(self) -> bool:
        if self.current == None:
            self.current = 0
            return True

        if self.current < self.limit:
            self.current += 1
            return True
        return False

def gen_n(n):
    i = 0

    while i <= n:
        yield i
        i += 1

print(list_n(5))
gen = Gen(5)
while gen.next():
    print(gen.current)

print("-----")
# for r in gen_n(5):
#     print(r)

my_gen = gen_n(5)
print(next(my_gen))
print(next(my_gen))
print(next(my_gen))
print(next(my_gen))
print(next(my_gen))
print(next(my_gen))
# print(next(my_gen))

def gen_n_nested(n):
    yield from gen_n(n)


print("-----")
for i in gen_n_nested(5):
    print(i)