<script setup>
import {computed} from "vue";
import None from "./metadataInput/None.vue";
import Selection from "./metadataInput/Selection.vue";

const props = defineProps(['inputType', 'metadata'])
const emit = defineEmits(['updated'])

const component = computed(() => {
  if (props.inputType in [0, 1]) return None
  if (props.inputType === 2) return Selection
  return null;
})
</script>

<template>
  <component
      v-if="component"
      :is="component"
      :metadata="props.metadata"
      @updated="v => emit('updated', v)"
  />
</template>

<style scoped>

</style>