
from scraping.bbc_food.collect_recepies import CollectRecepies
from scraping.run import Runner


def default(args, runner: Runner):
    search_term = args[0]

    collector = CollectRecepies(
        runner=runner
    )
    runner.submit_work(collector.start_collection, search_term)
