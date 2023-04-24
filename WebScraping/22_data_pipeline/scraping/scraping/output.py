import json

class FilePipeline:

    def __init__(self, file_name='output.json') -> None:
        self.file_name = file_name
        self.file = None
        self.first = True

    def __enter__(self):
        self.file = open(self.file_name, 'a', encoding='utf-8', buffering=1)
        self.file.write('[\n')
        return self

    def process(self, item):
        if self.first:
            self.first = False
        else:
            self.file.write(',\n')

        json.dump(item, self.file)

    def __exit__(self, *args, **kwargs):
        self.file.write('\n]')
        self.file.close()