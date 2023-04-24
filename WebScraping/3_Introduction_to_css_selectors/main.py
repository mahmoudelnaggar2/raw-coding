from requests import get
import json

# from bs_opponents import get_opponents
from bs_select_opponents import get_opponents

response = get('https://en.wikipedia.org/wiki/Khabib_Nurmagomedov')

opponents = get_opponents(response.text)

print(opponents)

opponents_json = json.dumps(opponents)
# print(opponents_json)

with open('khabib_opponents.json', 'w', encoding='utf-8') as f:
    f.write(opponents_json)

# with open('khabib.html', 'r', encoding='utf-8') as f:
#     contents = f.read()
#     print(contents)

