import {defineConfig} from 'vite';
import solidPlugin from 'vite-plugin-solid';
import dns from 'dns'
import {readFileSync} from 'fs'
import {resolve} from 'path'

dns.setDefaultResultOrder('verbatim')

export default defineConfig({
    plugins: [solidPlugin()],
    server: {
        port: 3000,
        https: {
            key: readFileSync(resolve('localhost-key.pem')),
            cert: readFileSync(resolve('localhost.pem'))
        }
    },
    build: {
        target: 'esnext',
    },
});
