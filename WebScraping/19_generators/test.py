from queue import Queue
from threading import Thread
from types import GeneratorType
from requests import get
from concurrent.futures import ThreadPoolExecutor
import time

results_queue = Queue()
work_queue = Queue()

def start_collection():
    print("hello")
    for i in range(0, 5):
        yield do_collection

def do_collection():
    result = get('https://en.wikipedia.org/wiki/Microsoft_Docs')
    results_queue.put(result.status_code)

    # collect more
    for i in range(1, 5):
        yield next

def next():
    result = get('https://en.wikipedia.org/wiki/Microsoft_Docs')
    results_queue.put(result.status_code)


executor = ThreadPoolExecutor(max_workers=10)

def submit_work(fn):
    def unpack(fnc):
        result = fnc()
        if isinstance(result, GeneratorType):
            for n in result:
                submit_work(n)
        else:
            return result

    work = executor.submit(unpack, fn)
    work_queue.put(work)

start = time.perf_counter()

submit_work(start_collection)
while not work_queue.empty():
    work = work_queue.get()
    work.result()
    work_queue.task_done()

end = time.perf_counter()
print(end - start, results_queue.qsize())