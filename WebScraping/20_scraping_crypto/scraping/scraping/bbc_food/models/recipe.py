
class Recipe:
    def __init__(
        self,
        url="",
        title="",
        description="",
        time=0,
        difficulty="",
        vegan=False,
        gluten_free=False
    ):
        self.url =url
        self.title = title
        self.description = description
        self.time = time
        self.difficulty = difficulty
        self.vegan = vegan
        self.gluten_free = gluten_free
