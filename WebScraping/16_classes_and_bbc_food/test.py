

class Phone:
    field = "one"

    def __init__(self, model = "", version = None):
        self.model = model
        self.version = version

    def get_version(self):
        return int(self.version)

phone1 = Phone("iphone")
phone2 = Phone("android", version="12")

print(vars(phone1))
print(vars(phone2))
print(phone2.get_version())