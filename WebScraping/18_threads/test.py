from queue import Queue
from threading import Thread
from requests import get
from concurrent.futures import ThreadPoolExecutor
import time

results_queue = Queue()
work_queue = Queue()

executor = ThreadPoolExecutor(max_workers=10)

def submit_work(fn):
    work = executor.submit(fn)
    work_queue.put(work)

def do_collection():
    result = get('https://en.wikipedia.org/wiki/Microsoft_Docs')
    results_queue.put(result.status_code)

    # collect more
    for i in range(1, 5):
        submit_work(next)

def next():
    result = get('https://en.wikipedia.org/wiki/Microsoft_Docs')
    results_queue.put(result.status_code)


start = time.perf_counter()

for i in range(0, 5):
    submit_work(do_collection)

while not work_queue.empty():
    work = work_queue.get()
    work.result()
    work_queue.task_done()

end = time.perf_counter()
print(end - start, results_queue.qsize())