import asyncio
from datetime import datetime
import websockets
import json

URL = 'wss://stream.coinmarketcap.com/price/latest'
ID_COIN_MAP = {
    1: 'BTC'
}

async def hello():
    async with websockets.connect(URL) as websocket:
        payload = {
            'data': {
                'cryptoIds': [1],
                'index': 'detail'
            },
            'id': 'price',
            'method': 'subscribe'
        }
        results = []
        await websocket.send(json.dumps(payload))
        i = 0
        while i < 20:
            msg = await websocket.recv()
            data = json.loads(msg)
            unix_timestamp = int(data['d']['t'])
            coin_data = {
                'name': ID_COIN_MAP[data['d']['cr']['id']],
                'current_price': data['d']['cr']['p'],
                'time': datetime.fromtimestamp(unix_timestamp / 1000).isoformat()
            }
            results.append(coin_data)
            i = i + 1

        return results

def default(args):
    return asyncio.run(hello())