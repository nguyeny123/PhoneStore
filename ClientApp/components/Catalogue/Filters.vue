<template>
<div class="filters mb-4">
<h5 class="mt-4">Filters</h5>
<b-btn variant="outline-secondary" @click.prevent="reset">
<i class="fas fa-sync mr-2"></i> Reset</b-btn>
<b-list-group class="mt-4">

<filter-accordion>
<span slot="header">Brand</span>
<multi-select-filter slot="body" queryKey="brands" :items="filters.brands" />
</filter-accordion>

<filter-accordion>
<span slot="header">Capacity</span>
<multi-select-filter slot="body" query-key="capacity" :items="filters.storage" />

</filter-accordion>

<filter-accordion>
<span slot="header">Colour</span>
<multi-select-filter slot="body" query-key="colours" :items="filters.colours" />
</filter-accordion>


<filter-accordion>
<span slot="header">Operating system</span>
<multi-select-filter slot="body" query-key="os" :items="filters.os" />
</filter-accordion>

<filter-accordion>
<span slot="header">Features</span>
<multi-select-filter slot="body" query-key="features" :items="filters.features" />
</filter-accordion>
</b-list-group>
</div>
</template>

<script>
import FilterAccordion from "./FilterAccordion.vue";
import MultiSelectFilter from "./MultiSelectFilter.vue";
import RangeFilter from "./RangeFilter.vue";
//import vueSlider from "vue-slider-component";
export default {
    data () {
		return {
		Pricess: 10
		}
  	},
	name: "filters",
	props: {
		filters: {
			type: Object,
			required: true
		}
	},
	components: {
		FilterAccordion,MultiSelectFilter,RangeFilter
	},
	methods: {
		reset() {
			this.$router.push({ query: {} });
		}
	}
};
</script>

