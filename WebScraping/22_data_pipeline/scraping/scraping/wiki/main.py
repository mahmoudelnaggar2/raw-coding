from requests import get

from scraping.wiki.wiki_fighters import get_opponents, get_fighter_info, get_opponnets_with_info

def default(args):
    if len(args) == 0:
        raise Exception("missing argument ...")

    target = args[0]
    url = args[1]

    handler = None
    if target == 'ops':
        handler = get_opponents
    elif target == 'ops+info':
        handler = get_opponnets_with_info
    elif target == 'info':
        response = get(url)
        yield get_fighter_info(response.text)
        return

    response = get(url)
    yield from handler(response.text)
