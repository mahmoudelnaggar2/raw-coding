import asyncio
from concurrent.futures import ThreadPoolExecutor
from datetime import datetime
from queue import Empty, Queue
import websockets
import json

URL = 'wss://stream.coinmarketcap.com/price/latest'
ID_COIN_MAP = {
    1: 'BTC'
}

async def start_ws_connection(queue: Queue):
    async with websockets.connect(URL) as websocket:
        payload = {
            'data': {
                'cryptoIds': [1],
                'index': 'detail'
            },
            'id': 'price',
            'method': 'subscribe'
        }
        
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
            queue.put(coin_data)
            i = i + 1

        

def default(args):
    with ThreadPoolExecutor() as executor:
        queue = Queue()
        future = executor.submit(lambda:asyncio.run(start_ws_connection(queue)))
        while not future.done() or not queue.empty():
            try:
                item = queue.get(timeout=10)
                yield item
                queue.task_done()
            except Empty:
                continue

        future.result()
        