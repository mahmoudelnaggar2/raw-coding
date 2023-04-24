import sys

from scraping import run, output, importer

def default():
    module = sys.argv[1]
    results = run.run_scraper(module, sys.argv[2:])
    output.dump_to_file(results)

def import_scraper():
    scraper_name = sys.argv[1]
    scraper_path = sys.argv[2]

    with open(scraper_path, 'r') as f:
        scraper_contents = f.read()
        importer.add_scraper(scraper_name, scraper_contents)
