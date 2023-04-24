from setuptools import setup, find_packages

requirements_files = [
    '../scraping/requirements.txt',
    'requirements.txt'
]
requirements = []

for req in requirements_files:
    with open(req, 'r', encoding='utf-8') as f:
        requirements.extend(f.read().split('\n'))

packages = find_packages('../scraping')
package_dir = {}
for p in packages:
    package_dir[p] = '../scraping/' + p.replace('.', '/')

setup(
    name='scraping-api',
    version='0.0.1',
    install_requires=requirements,
    py_modules=['api'],
    packages=packages,
    package_dir=package_dir,
    entry_points={
        'console_scripts': [
            'scrape-api = api:start',
        ]
    }
)