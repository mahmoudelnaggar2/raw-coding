import os, pathlib

def add_scraper(name, content):
    current_dir = os.path.dirname(os.path.realpath(__file__))
    spider_dir = current_dir + '/' + name
    pathlib.Path(spider_dir).mkdir(parents=True, exist_ok=True)
    pathlib.Path(spider_dir + '/__init__.py').touch()
    with open(spider_dir + '/main.py', 'w') as f:
        f.write(content)