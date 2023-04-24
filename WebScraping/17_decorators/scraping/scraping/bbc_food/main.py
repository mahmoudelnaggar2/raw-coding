
from scraping.bbc_food.collect_recepies import CollectRecepies


def default(args):
    search_term = args[0]

    return CollectRecepies().start_collection(search_term)