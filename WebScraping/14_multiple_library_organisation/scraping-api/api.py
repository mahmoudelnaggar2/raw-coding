from flask import Flask, request
from scraping import run

app = Flask(__name__)

@app.route("/")
def hello_world():
    return "<p>Hello, World!</p>"


@app.route("/scrape/<scraper>")
def scrape(scraper):
    return run.run_scraper(scraper, request.args.getlist('args'))
    
def start():
    app.run()