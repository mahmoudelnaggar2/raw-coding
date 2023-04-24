<script setup>
import {reactive} from "vue";
import {useRoute} from "vue-router";
import {inputTypes} from "../enums/enums";
import RenderField from "../components/RenderField.vue";

const route = useRoute()

const state = reactive({
  spec: null,
  form: {}
})

fetch(`/api/form-spec/${route.params.id}`)
    .then(r => r.json())
    .then(spec => {
      state.spec = spec
      state.spec.fields.forEach(f => state.form[f.label] = null)
    })

function submitForm() {
  fetch(
      '/api/form?specId=' + state.spec.id,
      {
        method: 'post',
        body: JSON.stringify(state.form)
      }
  )
}

</script>

<template>
  <div v-if="state.spec === null">Loading</div>
  <div v-else>
    <h3>{{ state.spec.name }}</h3>
    <RenderField
        v-for="(f, i) in state.spec.fields"
        :key="`field-${i}`"
        :spec="f"
        @updated="v => state.form[f.label] = v"
    />
    <div>
      <button @click="submitForm">Submit</button>
    </div>
  </div>
  <div>
    <h3>Debug View</h3>
    <div>
      {{ state.form }}
    </div>
  </div>
</template>
