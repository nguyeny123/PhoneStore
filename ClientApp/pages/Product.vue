<template>
<div class="page">
    <product-details v-if="product" :product="product" />
</div>
</template>

<script>
import ProductDetails from "../components/product/Details.vue";
import axios from "axios";
export default {
    name: "product",
    components: {
        ProductDetails
    },
    data() {
        return {
            product: null          
        };
    },
    methods: {
        setData(product) {
            this.product = product;
        }
    },
    beforeRouteEnter(to, from, next) {
        axios.get(`/api/products/${to.params.slug}`).then(response => {
            next(vm => vm.setData(response.data));
        });
    }
};
</script>