<script setup>
const {spec} = defineProps(['spec'])
const emit = defineEmits(['updated'])

const options = JSON.parse(spec.metadata)
emit('updated', options[0])
</script>

<template>
  <label>{{ spec.label }}:</label>
  <select @input="v => emit('updated', v.target.value)">
    <option v-for="o in options" :value="o">{{ o }}</option>
  </select>
</template>

<style scoped>

</style>