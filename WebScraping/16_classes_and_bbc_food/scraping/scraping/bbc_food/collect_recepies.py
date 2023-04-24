import requests
import json
from parsel import Selector
from scraping.bbc_food.models.recipe import Recipe

class CollectRecepies:

    def start_collection(self, search_term):
        result = requests.get(f'https://www.bbcgoodfood.com/search?q={search_term}')
        html = Selector(result.text)

        json_string = html.xpath("//script[@class='js-search-data']/text()").get()
        data = json.loads(json_string)

        results = []
        for node in data["results"]:
            recipe = self.create_recepie(node)
            if recipe:
                results.append(vars(recipe))

        return results
            
    def create_recepie(self, recipe_data):
        diets = []
        if 'diet' in recipe_data['taxonomies']:
            diets = list(map(lambda x: x['name'],recipe_data['taxonomies']['diet']))

        return Recipe(
            title=recipe_data['title'],
            description=recipe_data['description'],
            time=recipe_data['totalTimeArray']['totalTimeHumanReadable'],
            difficulty=recipe_data['taxonomies']['difficulty'][0]['name'],
            vegan="Vegan" in diets,
            gluten_free="Gluten-free" in diets
        )
