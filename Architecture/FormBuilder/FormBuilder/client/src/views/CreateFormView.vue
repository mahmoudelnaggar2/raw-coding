<script setup>
import {reactive} from "vue";
import MetadataInput from "../components/MetadataInput.vue";
import {inputTypes} from "../enums/enums";

const formSpec = reactive({
  id: 0,
  name: "",
  fields: []
})

function addField() {
  formSpec.fields.push({
    id: 0,
    inputType: inputTypes.TEXT,
    label: "",
    hint: "",
    metadata: ""
  })
}


function createFormSpec(){
  return fetch('/api/form-spec', {
    method: 'post', 
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(formSpec),
  })
}

</script>

<template>
  <div>
    <label>Form Name</label>
    <input v-model="formSpec.name"/>
  </div>
  <div>
    <button @click="addField">add field</button>
  </div>
  <div v-for="(f, i) in formSpec.fields" :key="`f-${i}`">
    <div>
      <label>Input Type</label>
      <select v-model="f.inputType">
        <option :value=inputTypes.TEXT>Text</option>
        <option :value=inputTypes.NUMBER>Number</option>
        <option :value=inputTypes.SELECTION>Selection</option>
      </select>
    </div>
    <div>
      <label>Label</label>
      <input type="text" v-model="f.label" />
    </div>
    <div>
      <label>Hint</label>
      <input type="text" v-model="f.hint" />
    </div>
    <MetadataInput 
        :inputType="f.inputType" 
        :metadata="f.metadata"
        @updated="v => f.metadata = v"
    />
  </div>
  <div>
    <button @click="createFormSpec">Create</button>
  </div>
  <div>
    <h1>Debug preview</h1>
    <div>{{formSpec}}</div>
  </div>
</template>


<style scoped>

</style>