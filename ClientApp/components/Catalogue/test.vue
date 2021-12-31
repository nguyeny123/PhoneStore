<template>
<div>
    <h1>hehe</h1>
  <vue-slider  v-model="range"
     :min="0"
    :max="200" />
  </div>
</template>

<script>
import VueSlider from 'vue-slider-component'
import 'vue-slider-component/theme/antd.css'

export default {
  components: {
    VueSlider
  },
  data () {
    return {
      range: [20,1000]
    }
  }
}
</script>