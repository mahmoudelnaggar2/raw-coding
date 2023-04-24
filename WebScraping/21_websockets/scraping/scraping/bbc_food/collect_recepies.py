import requests
import json
from parsel import Selector
from scraping.bbc_food.models.recipe import Recipe
from scraping.helpers import catch, to_dict
from scraping.task import Task

class CollectRecepies:

    def start_collection(self, search_term):
        result = requests.get(f'https://www.bbcgoodfood.com/search?q={search_term}')
        html = Selector(result.text)

        json_string = html.xpath("//script[@id='__NEXT_DATA__']/text()").get()
        data = json.loads(json_string)

        for node in data['props']['pageProps']["searchResults"]['items']:
            recipe = self.create_recepie(node)

            if recipe:
                yield recipe

                if url := recipe.get('url'):
                    yield Task(
                        fnc=self.next_request,
                        url=url
                    )
            
    def next_request(self, url):
        result = requests.get(f'https://www.bbcgoodfood.com{url}')
        html = Selector(result.text)
        title = html.xpath("//title/text()").get()
        yield {
            'title': title,
            'url': f'https://www.bbcgoodfood.com{url}'
        }

    @catch
    @to_dict
    def create_recepie(self, recipe_data):
        # diets = []
        # if 'diet' in recipe_data['taxonomies']:
        #     diets = list(map(lambda x: x['name'],recipe_data['taxonomies']['diet']))

        return Recipe(
            url=recipe_data['url'],
            title=recipe_data['title'],
            description=recipe_data['description'],
            # time=recipe_data['totalTimeArray']['totalTimeHumanReadable'],
            # difficulty=recipe_data['taxonomies']['difficulty'][0]['name'],
            # vegan="Vegan" in diets,
            # gluten_free="Gluten-free" in diets
        )
