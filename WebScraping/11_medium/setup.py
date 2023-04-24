from setuptools import setup

with open('requirements.txt', 'r', encoding='utf-8') as f:
    requirements = f.read().split('\n')

setup(
    name='mypackage',
    version='0.0.1',
    install_requires=requirements,
    py_modules=['scraping'],
    entry_points={
        'console_scripts': [
            'scrape_wiki = scraping.wiki.main:default',
            'scrape_shop = scraping.shop.main:default',
            'scrape_blog = scraping.blog.main:default',
        ]
    }
)