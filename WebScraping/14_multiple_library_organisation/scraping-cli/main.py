import sys

from scraping import run, output

def default():
    module = sys.argv[1]
    results = run.run_scraper(module, sys.argv[2:])
    output.dump_to_file(results)
