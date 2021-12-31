<template>
<div>
<h3 class="mt-4">Orders</h3>
<table class="table table-striped table-hover">
<thead style="display: block;text-align:center">
<tr>
<th style="display: block">Order</th>
<th v-if="isAdmin" style="width:110px">Customer</th>
<th style="width:80px">Placed</th>
<th>Items</th>
<th>Total</th>
<th>Payment status</th>
</tr>
</thead>
<tbody style="display: block;text-align:center">
    <div v-if="orders && orders.length > 0">
        <tr
        v-for="order in orders" :key="order.id">
        <td style="width:60px">{{ order.id }}</td>
        <td v-if="isAdmin">{{ order.customer }}</td>
        <td>{{ order.placed | date }}</td>
        <td>{{ order.items }}</td>
        <td style="width:100px">{{ order.total | currency }}</td>
        <td>{{ order.paymentStatus }}</td>
        </tr>
    </div>
    <div v-else>
        <td v-if="isAdmin" colspan="6">There are no orders to display.</td>
        <td v-else colspan="5">You haven't placed any orders yet!</td>
     </div>
</tbody>
</table>
</div>
</template>
<script>
export default {
    name: "order-list",
    props: {
        orders: {
            type: Array,
            required: false
        }
    },
    computed: {
        isAdmin() {
            return this.$store.getters.isInRole("Admin");
        }
    }
};
</script>
