<template>
<b-container fluid class="page">
<b-row>
    <b-col cols="3">
    <filters v-if="filters.brands.length" :filters="filters" />
    </b-col>

    <b-col cols="9">
        <div class="mt-4 flex">
        <search-bar class="search" />
        <product-sort class="ml-4" />
        </div>
    <product-list v-if="products.length" :products="sortedProducts" />
    </b-col>
</b-row>
<!-- <test-vue/> -->
</b-container>
</template>

<script>
import TestVue from "../components/Catalogue/test.vue";
import ProductList from "../components/Catalogue/ProductList.vue";
import axios from "axios";
import Filters from "../components/Catalogue/Filters.vue";
import ProductSort from "../components/Catalogue/ProductSort.vue";
import SearchBar from "../components/Catalogue/SearchBar.vue";
export default {
    name: "catalogue",
    data() {
        return {
            products: [],
            filters: {
                brands: [],
                storage: [],
                colours: [],
                os: [],
                features: []
            }
        };
    },
    components: {
        ProductList,Filters,TestVue,ProductSort,SearchBar
    },

    computed: {
        sort() {
        return this.$route.query.sort || 0;
        },
        sortedProducts() {
            switch (this.sort) {
                case 1:
                    return this.products.sort((a, b) =>{
                    return b.price - a.price});
                case 2:
                    return this.products.sort((a, b) => {
                    return (a.name > b.name)?1:-1;
                });
                case 3:
                    return this.products.sort((a, b) => {
                    return (b.name > a.name)?1:-1;
                });
                default:
                    return this.products.sort((a, b) => {
                    return a.price - b.price;
                });
            }
        }
    },
    methods: {
        setData(products,filters) {
            this.products = products;
            this.filters = filters;
        }
    },
    beforeRouteEnter(to, from, next) {
        axios
        .all([
            axios.get("/api/products", { params: to.query }),
            axios.get("/api/filters")
        ])
        .then(
            axios.spread((products, filters) => {
                next(vm => vm.setData(products.data, filters.data));
            })
        );
    },

    beforeRouteUpdate(to, from, next) {
        axios.get("/api/products", { params: to.query }).then(response => {
            this.products = response.data;
            next();
        });
    }
};
</script>

<style lang="scss" scoped>
.flex {
    display: flex;
    flex-direction: row;
    .search {
    flex: 1;
    }
}
</style>