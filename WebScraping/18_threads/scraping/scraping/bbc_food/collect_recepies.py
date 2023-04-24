import requests
import json
from parsel import Selector
from scraping.bbc_food.models.recipe import Recipe
from scraping.helpers import catch, to_dict
from scraping.run import Runner

class CollectRecepies:

    def __init__(self, runner: Runner):
        self.runner = runner

    def start_collection(self, search_term):
        result = requests.get(f'https://www.bbcgoodfood.com/search?q={search_term}')
        html = Selector(result.text)

        json_string = html.xpath("//script[@class='js-search-data']/text()").get()
        data = json.loads(json_string)

        for node in data["results"]:
            recipe = self.create_recepie(node)

            if recipe:
                self.runner.submit_result(recipe)

                if url := recipe.get('url'):
                    self.runner.submit_work(self.next_request, url)
            
    def next_request(self, url):
        result = requests.get(f'https://www.bbcgoodfood.com{url}')
        html = Selector(result.text)
        title = html.xpath("//title/text()").get()
        self.runner.submit_result({
            'title': title,
            'url': f'https://www.bbcgoodfood.com{url}'
        })

    @catch
    @to_dict
    def create_recepie(self, recipe_data):
        diets = []
        if 'diet' in recipe_data['taxonomies']:
            diets = list(map(lambda x: x['name'],recipe_data['taxonomies']['diet']))

        return Recipe(
            url=recipe_data['url'],
            title=recipe_data['title'],
            description=recipe_data['description'],
            time=recipe_data['totalTimeArray']['totalTimeHumanReadable'],
            difficulty=recipe_data['taxonomies']['difficulty'][0]['name'],
            vegan="Vegan" in diets,
            gluten_free="Gluten-free" in diets
        )
