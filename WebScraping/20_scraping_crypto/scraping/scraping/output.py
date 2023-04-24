import json

def dump_to_file(results):
    with open('output.json', 'w', encoding='utf-8') as f:
        json.dump(results, f)