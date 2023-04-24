import {defineNuxtPlugin, useRequestHeaders} from "nuxt/app";

export default defineNuxtPlugin(async nuxtApp => {
    if (process.server) {
        console.log(nuxtApp.ssrContext.event.req)
        const headers = useRequestHeaders(['cookie'])
        console.log(headers)
        if (headers['cookie']) {
            const data = await $fetch('http://localhost:5110/api/test', {
                headers
            })
            console.log(data)
        }
    }
})