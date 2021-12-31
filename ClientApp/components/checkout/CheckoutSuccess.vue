<template>
<b-alert :variant="variant" show>
<h3>Order placed successfully.</h3>
<p>
Your order number is <strong>{{ order.orderId }}</strong>.
We will contact you to confirm your order soon.
</p>
<p>
Click <router-link to="/account">here</router-link>
to see your orders.
</p>
</b-alert>
</template>
<script>
export default {
name: "checkout-success",
props: {
    order: {
        type: Object,
        required: true
    }
},
computed: {
variant() {
return this.order.paymentStatus === "Paid" ?
"success" : "warning";
}
}
};
</script>