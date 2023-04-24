

class Task:

    def __init__(self, fnc, *args, **kwargs):
        self.fnc = fnc
        self.args = args
        self.kwargs = kwargs