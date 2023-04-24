from requests import get
import json
import sys

from scraping.wiki_fighters import get_opponents, get_fighter_info, get_opponnets_with_info

def default(args):
    if len(args) == 0:
        raise Exception("missing argument ...")

    target = args[0]
    url = args[1]
    output = args[2]

    handler = None
    if target == 'ops':
        handler = get_opponents
    elif target == 'ops+info':
        handler = get_opponnets_with_info
    elif target == 'info':
        handler = get_fighter_info

    response = get(url)
    results = handler(response.text)
    # json_payload = json.dumps(results, ensure_ascii=False)
    json_payload = json.dumps(results)

    with open(f'{output}.json', 'w', encoding='utf-8') as f:
        # f.write(json_payload.encode('ascii', 'ignore').decode('utf-8'))
        f.write(json_payload)
