
from scraping.bbc_food.collect_recepies import CollectRecepies
from scraping.run import Runner
from scraping.task import Task


def default(args):
    search_term = args[0]

    collector = CollectRecepies()
    yield Task(
        fnc=collector.start_collection,
        search_term=search_term
    )
