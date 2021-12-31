import Vue from 'vue';
import VueRouter from 'vue-router';
import BootstrapVue from "bootstrap-vue";
import NProgress from "nprogress";
import store from "./store";
import axios from "axios";
import Vuelidate from "vuelidate";
import VeeValidate from "vee-validate";
import VueToastr from "vue-toastr";

//plugins
import "./helpers/validation";
// use plugin
Vue.use(VueToastr,{
    defaultTimeout:1000
});
Vue.use(Vuelidate);
Vue.use(VeeValidate);
Vue.use(VueRouter);
Vue.use(BootstrapVue);
import { currency, date } from "./filters";
Vue.filter("currency", currency);
Vue.filter("date", date);

const initialStore = localStorage.getItem("store");
if (initialStore) {
    store.commit("initialise", JSON.parse(initialStore));
    if (store.getters.isAuthenticated) {
        axios.defaults.headers.common["Authorization"] = `Bearer ${store.state.auth.access_token}`;
    }
}

import Catalogue from "./pages/Catalogue.vue";
import Product from "./pages/Product.vue";
import Cart from "./pages/Cart.vue";
import Checkout from "./pages/Checkout.vue";
import Account from "./pages/Account.vue";

//import admin pages
import AdminIndex from "./pages/admin/Index.vue";
import AdminOrders from "./pages/admin/Orders.vue";
import AdminProducts from "./pages/admin/Products.vue";
import AdminCreateProduct from "./pages/admin/CreateProduct.vue";

const routes = [
    { path: "/", component: Catalogue },
    { path: "/products/:slug", component: Product },
    { path: "/cart", component: Cart, meta: { role: "Customer" } },
    { path: "/checkout", component: Checkout},
    { path: "/account", component: Account },

    {
        path: "/admin",
        component: AdminIndex,
        meta: { requiresAuth: true, role: "Admin" },
        redirect: "/admin/products",
        children: [
            {
                path: "orders",
                component: AdminOrders
            },
            {
                path: "products",
                component: AdminProducts
            },
            {
                path: "products/create",
                component: AdminCreateProduct
            }
        ]
        
    },

    { path: "*", redirect: "/" }
];
const router = new VueRouter({ mode: "history", routes: routes });
router.beforeEach((to, from, next) => {
    NProgress.start();
    if (to.matched.some(route => route.meta.requiresAuth)) {
        if (!store.getters.isAuthenticated) {
            store.commit("showAuthModal");
            next({ path: from.path, query: { redirect: to.path } });
        } else {
            if (
                    to.matched.some(
                    route => route.meta.role && store.getters.isInRole(route.meta.role)
                    )) 
                {
                    next();
                } 
            else if (!to.matched.some(route => route.meta.role)) {
                    next();
                } else {
                    next({ path: "/" });
                }
            next();
        }

    } else {
        if (
                to.matched.some(
                route =>
                route.meta.role &&
                (!store.getters.isAuthenticated ||
                store.getters.isInRole(route.meta.role))
                )
            ) {
                next();
            } 
        else {
            if (to.matched.some(route => route.meta.role)) {
                next({ path: "/" });
            }
            next();
            }
        }
});

router.afterEach((to, from) => {
NProgress.done();
});
new Vue({
    el: '#app-root',
    // router: new VueRouter({ mode: 'history', routes: routes }),
    router: router,
    store,
    render: h => h(require('./components/App.vue'))
});