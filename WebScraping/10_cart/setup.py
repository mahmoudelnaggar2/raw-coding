from setuptools import setup

with open('requirements.txt', 'r', encoding='utf-8') as f:
    requirements = f.read().split('\n')

setup(
    name='mypackage',
    version='0.0.1',
    install_requires=requirements,
    entry_points={
        'console_scripts': [
            'scrape_wiki = scraping.wiki.main:default',
            'scrape_shop = scraping.shop.main:default',
        ]
    }
)