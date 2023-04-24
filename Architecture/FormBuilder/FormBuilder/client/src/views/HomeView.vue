<script setup>
import {reactive} from "vue";

const state = reactive({
  specs: [],
  forms: []
})

fetch('/api/form-spec')
    .then(r => r.json())
    .then(specs => state.specs = specs)

fetch('/api/form')
    .then(r => r.json())
    .then(forms => state.forms = forms)

</script>

<template>
  <h1>home</h1>
  <RouterLink :to="`/form/${s.id}`" v-for="s in state.specs" :key="s.id">
    {{ s.name }}
  </RouterLink>
  <div>
    <h3>submissions</h3>
    <div v-for="s in state.forms">
      {{s}}
    </div>
  </div>
</template>
