<script setup>
import {reactive} from 'vue';

const state = reactive({msg: ''})

const loginJson = () => fetch('https://api.company.local/login', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
    },
    credentials: "include",
    body: JSON.stringify({
        Username: 'foo',
        Password: 'pw',
    })
})

const secret = () => fetch('https://api.company.local/protected', {
    credentials: "include",
})
    .then(x => x.text())
    .then(t => state.msg = t)

const get = () => fetch('https://api.company.local/')
    .then(x => x.text())
    .then(t => state.msg = t)

</script>

<template>
    <h1>App</h1>
    <div>
        {{ state.msg }}
    </div>
    <div>
        <button @click="get">Get</button>
    </div>
    <div>
        <button @click="secret">Fetch Secret</button>
    </div>
    <div>
        <button @click="loginJson">Login Json</button>
    </div>
</template>

<style scoped>
</style>
