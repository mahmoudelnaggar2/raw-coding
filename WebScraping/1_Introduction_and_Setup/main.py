import requests

response = requests.get('https://en.wikipedia.org/wiki/Khabib_Nurmagomedov')
print(response.status_code)
print(response.text)

with open('khabib_2.html', 'w', encoding='utf-8') as f:
    f.write(response.text)

# with open('khabib.html', 'r', encoding='utf-8') as f:
#     contents = f.read()
#     print(contents)

