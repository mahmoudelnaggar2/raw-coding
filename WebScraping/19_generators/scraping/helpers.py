import traceback


def catch(fn):
    def catch_inner(*args, **kwargs):
        try:
            return fn(*args, **kwargs)
        except:
            traceback.print_exc()
            return None

    return catch_inner


def to_dict(fn):
    def to_dict_inner(*args, **kwargs):
        return vars(fn(*args, **kwargs))

    return to_dict_inner