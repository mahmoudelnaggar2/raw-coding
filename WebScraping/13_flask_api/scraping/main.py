import importlib
import sys

from scraping.output import dump_to_file

def default():
    print(sys.argv)
    module = sys.argv[1]
    try:
        scraper = importlib.import_module(f'scraping.{module}.main')
        entry_point = getattr(scraper, 'default')
        results = entry_point(sys.argv[2:])
        dump_to_file(results)
    except ModuleNotFoundError: 
        print(f'mo`dule "{module}" not found!')
