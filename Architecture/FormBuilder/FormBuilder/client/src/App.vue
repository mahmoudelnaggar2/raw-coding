<script setup>
import {RouterLink, RouterView} from 'vue-router'
</script>

<template>
  <header>
    <div class="wrapper">
      <nav>
        <RouterLink to="/form-spec">Create Form Spec</RouterLink>
        <RouterLink to="/">Specs</RouterLink>
      </nav>
    </div>
  </header>

  <div style="margin-top: 50px;">
    <RouterView/>
  </div>
</template>

<style scoped>
</style>
