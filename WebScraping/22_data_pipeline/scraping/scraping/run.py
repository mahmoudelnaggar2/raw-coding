from concurrent.futures import ThreadPoolExecutor
import importlib
from queue import Queue
from types import GeneratorType

from scraping.task import Task

class Runner:

    def __init__(self, pipeline) -> None:
        self.executor = ThreadPoolExecutor()
        self.work_queue = Queue()
        self.pipeline = pipeline

    def submit_work(self, fn, *args, **kwargs):
        def unpack(fnc, *args, **kwargs):
            result = fnc(*args, **kwargs)
            if isinstance(result, GeneratorType):
                for n in result:
                    if isinstance(n, Task):
                        self.submit_work(n.fnc, *n.args, **n.kwargs)
                    elif isinstance(n, dict):
                        self.pipeline(n)
            else:
                raise Exception("didn't return generator!")
           
        work = self.executor.submit(unpack, fn, *args, **kwargs)
        self.work_queue.put(work)

    def wait_to_complete(self):
        while not self.work_queue.empty():
            work = self.work_queue.get()
            work.result()

def run_scraper(scraper, pipeline, args):
    last = args[-1] if len(args) > 0 else []
    if last == '-p':
        return run_scraper_parallel(scraper, pipeline, args[:-1])

    try:
        scraper = importlib.import_module(f'scraping.{scraper}.main')
        entry_point = getattr(scraper, 'default')
        for item in entry_point(args):
            pipeline(item)

    except ModuleNotFoundError as e: 
        print(f'scraper "{scraper}" not found! {e}')


def run_scraper_parallel(scraper, pipeline, args):
    runner = Runner(pipeline)
    try:
        scraper = importlib.import_module(f'scraping.{scraper}.main')
        entry_point = getattr(scraper, 'default')
        runner.submit_work(entry_point, args)
        runner.wait_to_complete()
    except ModuleNotFoundError as e: 
        print(f'scraper "{scraper}" not found! {e}')