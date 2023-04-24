<script setup>
import {ref} from "vue";

const props = defineProps(['metadata'])
const emit = defineEmits(['updated'])


const options = props.metadata ? JSON.parse(props.metadata) : []
const option = ref("")

function addOption() {
  options.push(option.value)
  emit('updated', JSON.stringify(options))
}
</script>

<template>
  <input v-model="option">
  <button @click="addOption">Add</button>
  <ul>
    <li v-for="(o, i) in options" :key="`o-${o}-${i}`">{{ o }}</li>
  </ul>
</template>


<style scoped>

</style>