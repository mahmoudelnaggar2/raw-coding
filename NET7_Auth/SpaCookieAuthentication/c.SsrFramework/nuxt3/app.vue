<template>
  <div>
    <button @click="login">Login</button>
    <button @click="test">Test</button>
  </div>
</template>

<script setup>
const login = () => fetch('https://localhost:5111/api/login', {method: 'post', credentials: 'include'})
const test = () => fetch('https://localhost:5111/api/test', {credentials: 'include'})
</script>
