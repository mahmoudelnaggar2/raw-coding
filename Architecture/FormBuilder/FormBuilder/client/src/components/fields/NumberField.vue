<script setup>
const {spec} = defineProps(['spec'])
const emit = defineEmits(['updated'])
emit('updated', 0)
</script>

<template>
  <label>{{ spec.label }}:</label>
  <input @input="v => emit('updated', parseInt(v.target.value))" :placeholder="spec.hint" type="number">
</template>

<style scoped>

</style>