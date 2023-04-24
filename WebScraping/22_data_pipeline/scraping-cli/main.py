import sys

from scraping.output import FilePipeline
from scraping import run, importer

def default():
    module = sys.argv[1]
    with FilePipeline() as pipeline:
        run.run_scraper(module, pipeline.process, sys.argv[2:])

def import_scraper():
    scraper_name = sys.argv[1]
    scraper_path = sys.argv[2]

    with open(scraper_path, 'r') as f:
        scraper_contents = f.read()
        importer.add_scraper(scraper_name, scraper_contents)
