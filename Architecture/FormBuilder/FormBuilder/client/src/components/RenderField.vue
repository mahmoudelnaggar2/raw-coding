<script setup>
import {inputTypes} from "../enums/enums";
import TextField from "./fields/TextField.vue";
import NumberField from "./fields/NumberField.vue";
import SelectField from "./fields/SelectField.vue";

const props = defineProps(['spec'])
const emit = defineEmits(['updated'])
const componentMap = {
  [inputTypes.TEXT]: TextField,
  [inputTypes.NUMBER]: NumberField,
  [inputTypes.SELECTION]: SelectField,
}

const component = componentMap[props.spec.inputType]

</script>

<template>
  <div>
    <component
        :is="component"
        :spec="props.spec"
        @updated="v => emit('updated', v)"
    />
  </div>
</template>


<style scoped>

</style>