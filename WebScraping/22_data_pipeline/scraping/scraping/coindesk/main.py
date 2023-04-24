from requests import get
from datetime import datetime
import time
URL = 'https://production.api.coindesk.com/v2/tb/price/ticker?assets='

def default(args):
    if len(args) == 0:
        raise Exception("missing argument ...")

    symbols = args[0]
    print("scraping", symbols)
    final_url = URL + symbols
    individual_symbols = symbols.split(',')

    i = 0
    while i < 10:
        result = get(final_url)
        json_data = result.json()


        for s in individual_symbols:
            coin_data = json_data['data'][s]
            yield {
                'iso': coin_data['iso'],
                'name': coin_data['name'],
                'current_price': coin_data['ohlc']['c'],
                'time': datetime.fromtimestamp(int(coin_data['ts'] / 1000)).isoformat()
            }

        time.sleep(10)
        i = i + 1
