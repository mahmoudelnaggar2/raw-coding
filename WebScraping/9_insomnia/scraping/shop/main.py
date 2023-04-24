import requests
import json

query = '{"requests":[{"indexName":"production_ocuk_rating_desc","params":"hitsPerPage=60&snippetEllipsisText=%E2%80%A6&removeWordsIfNoResults=allOptional&maxValuesPerFacet=100&query=nvidia&highlightPreTag=__ais-highlight__&highlightPostTag=__%2Fais-highlight__&page=0&facets=%5B%22availability_status%22%2C%22attributes.manufacturer%22%2C%22attributes.price_decimal%22%2C%22attributes.price_gross_decimal%22%2C%22attributes.rating%22%2C%22attributes.rating_group%22%2C%22label_keys%22%2C%22price_gross_decimal%22%2C%22rating_rounded%22%2C%22all_category_names.level0%22%2C%22all_category_names.level1%22%2C%22all_category_names.level2%22%5D&tagFilters=&facetFilters=%5B%5B%22all_category_names.level1%3APC%20Components%20%2F%2F%2F%20Graphics%20Cards%22%5D%5D"}]}'
url = 'https://78jqob38q3-dsn.algolia.net/1/indexes/*/queries?x-algolia-agent=Algolia%20for%20JavaScript%20(4.12.1)%3B%20Browser%20(lite)%3B%20instantsearch.js%20(4.38.0)%3B%20JS%20Helper%20(3.7.0)&x-algolia-api-key=f5c8d3eaa2f0d51e934864344a1efa60&x-algolia-application-id=78JQOB38Q3'

def default():
    response = requests.post(url, data=query)

    json_data = response.json()

    products = json_data['results'][0]['hits']

    results = []
    for product in products:
        results.append(map_product(product))

    with open('output.json', 'w', encoding='utf-8') as f:
        json.dump(results, f)

        

def map_product(product_json):
    return {
        'name': product_json['abstract_name'],
        'url': f"https://www.overclockers.co.uk{product_json['url']}",
        'price': product_json['price'],
        'stock': product_json['availability'],
        'stock_type': product_json['availability_status'],
    }
