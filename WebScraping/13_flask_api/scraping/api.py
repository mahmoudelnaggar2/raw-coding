from flask import Flask, request
import importlib

app = Flask(__name__)

@app.route("/")
def hello_world():
    return "<p>Hello, World!</p>"


@app.route("/scrape/<scraper>")
def scrape(scraper):
    try:
        scraper = importlib.import_module(f'scraping.{scraper}.main')
        entry_point = getattr(scraper, 'default')
        return entry_point(request.args.getlist('args'))
    except ModuleNotFoundError: 
        return f'scraper "{scraper}" not found!'
    except Exception:
        return []