from requests import get
import json
import sys

from bs_xpath_opponents import get_opponents, get_fighter_info

if len(sys.argv) == 1:
    raise Exception("missing argument ...")

target = sys.argv[1]
url = sys.argv[2]
output = sys.argv[3]

handler = None
if target == 'ops':
    handler = get_opponents
elif target == 'info':
    handler = get_fighter_info

response = get(url)
results = handler(response.text)
results = json.dumps(results)

with open(f'{output}.json', 'w', encoding='utf-8') as f:
    f.write(results)

# with open('khabib.html', 'r', encoding='utf-8') as f:
#     contents = f.read()
#     print(contents)

