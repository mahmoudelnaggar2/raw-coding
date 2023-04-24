<script setup>
const {spec} = defineProps(['spec'])
const emit = defineEmits(['updated'])
emit('updated', "")
</script>

<template>
  <label>{{ spec.label }}:</label>
  <input @input="v => emit('updated', v.target.value)" :placeholder="spec.hint">
</template>

<style scoped>

</style>