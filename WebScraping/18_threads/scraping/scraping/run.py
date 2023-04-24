from concurrent.futures import ThreadPoolExecutor
import importlib
from queue import Queue

class Runner:

    def __init__(self) -> None:
        self.executor = ThreadPoolExecutor()
        self.work_queue = Queue()
        self.results_queue = Queue()

    def submit_work(self, fn, *args, **kwargs):
        work = self.executor.submit(fn, *args, **kwargs)
        self.work_queue.put(work)

    def submit_result(self, result):
        self.results_queue.put(result)

    def get_results(self):
        while not self.work_queue.empty():
            work = self.work_queue.get()
            work.result()

        results = []
        
        while not self.results_queue.empty():
            item = self.results_queue.get()
            results.append(item)

        return results

def run_scraper(scraper, args):
    last = args[-1]
    if last == '-p':
        return run_scraper_parallel(scraper, args[:-1])

    try:
        scraper = importlib.import_module(f'scraping.{scraper}.main')
        entry_point = getattr(scraper, 'default')
        return entry_point(args)
    except ModuleNotFoundError as e: 
        print(f'scraper "{scraper}" not found! {e}')


def run_scraper_parallel(scraper, args):
    runner = Runner()
    try:
        scraper = importlib.import_module(f'scraping.{scraper}.main')
        entry_point = getattr(scraper, 'default')
        runner.submit_work(entry_point, args, runner=runner)
        return runner.get_results()
    except ModuleNotFoundError as e: 
        print(f'scraper "{scraper}" not found! {e}')