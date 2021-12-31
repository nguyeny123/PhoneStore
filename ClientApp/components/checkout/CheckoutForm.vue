<template>
<div>
hehe
<form @submit.prevent="submit">
    <h4 class="mb-4">Delivery address</h4>
    <b-row>
        <b-col>
            <b-form-group>
                <label>First name</label>
                <b-form-input v-model="firstName" data-vv-name="first name"
                v-validate="'required|min:3'" :state="state('first name')" />
                <b-form-invalid-feedback>
                {{ errors.first('first name') }}
                </b-form-invalid-feedback>
            </b-form-group>
        </b-col>
        <b-col>
            <b-form-group>
                <label>Last name</label>
                <b-form-input v-model="lastName" data-vv-name="last name"
                v-validate="'required|min:3'" :state="state('last name')" />
                <b-form-invalid-feedback>
                {{ errors.first('last name') }}
                </b-form-invalid-feedback>
            </b-form-group>
        </b-col>
        <b-col>
            <b-form-group>
                <label>Phone number</label>
                <b-form-input v-model="phoneNumber" data-vv-name="phone"
                v-validate="'required'" :state="state('phone')" />
                <b-form-invalid-feedback>
                {{ errors.first('phone') }}
                </b-form-invalid-feedback>
            </b-form-group>
        </b-col>
    </b-row>
        <b-form-group>
            <label>Address</label>
            <b-form-input v-model="address" data-vv-name="address" v-validate="'required'" :state="state('address')" />
            <b-form-invalid-feedback>
            {{ errors.first('address') }}
            </b-form-invalid-feedback>
        </b-form-group>
        <b-form-group>
            <label>Address 2 <span class="text-muted">(Optional)</span></label>
            <b-form-input v-model="address2" data-vv-name="address 2"
            :state="state('address 2')" />
        </b-form-group>
        <b-form-group>
            <label>Town / city</label>
            <b-form-input v-model="townCity" data-vv-name="town / city" vvalidate="'required'" :state="state('town / city')" />
            <b-form-invalid-feedback>
            {{ errors.first('town / city') }}
            </b-form-invalid-feedback>
        </b-form-group>
        <b-form-group>
            <label>County</label>
            <b-form-input v-model="county" data-vv-name="county"
            v-validate="'required'" :state="state('county')" />
            <b-form-invalid-feedback>
            {{ errors.first('county') }}
            </b-form-invalid-feedback>
        </b-form-group>
        <b-form-group>
            <label>Postcode</label>
            <b-form-input v-model="postcode" data-vv-name="postcode" vvalidate="'required'" :state="state('postcode')" />
            <b-form-invalid-feedback>
            {{ errors.first('postcode') }}
            </b-form-invalid-feedback>
        </b-form-group>
        <b-button :disabled="false" type="submit" variant="primary"
        size="lg" block class="mt-4 mb-4">
        Checkout
        <span v-if="loading" class="fas fa-spinner fa-spin"></span>
        </b-button>
</form>
</div>
</template>
<script>
import axios from "axios";
let stripe = Stripe(`pk_test_51J03AoJRtbFchhYU5gryO8RaG0n6fdBaM8aiIvyp1c1azxbBc16UYHkxdCjg3cDaoFv7jv2gyfZ2mNcmZwlUIhBJ00Xx4HrMOP`),
elements = stripe.elements(),
card = null,
style = {
base: {
lineHeight: "24px"
}
};
export default {
name: "checkout-form",
data() {
    return {
        nameOnCard: "",
        firstName: "",
        lastName: "",
        phoneNumber: "",
        address: "",
        address2: "",
        townCity: "",
        county: "",
        postcode: "",
        loading: false
    };
},
methods: {
    state(field) {
        if (this.errors.has(field)) {
        return false;
        } else {
        return this.fields[field] && this.fields[field].dirty ?
        true : null;
        }
    },
    submit() {
        if (this.$store.getters.isAuthenticated == 0)
            this.$store.commit("showAuthModal");
        this.$validator.validateAll().then(result => {
            if (result) {
            this.loading = true;
            const details = {
            name: this.nameOnCard
            };
                const order = {
                            firstName: this.firstName,
                            lastName: this.lastName,
                            phoneNumber: this.phoneNumber,
                            address1: this.address,
                            address2: this.address2,
                            townCity: this.townCity,
                            county: this.county,
                            postcode: this.postcode,
                            items: this.$store.state.cart.map(item => {
                                return {
                                    productId: item.productId,
                                    colourId: item.colourId,
                                    storageId: item.storageId,
                                    quantity: item.quantity
                                };
                            })
                        };
                        axios
                        .post("/api/orders", order)
                        .then(response => {
                        this.$store.commit("clearCartItems");
                        this.$emit("success", response.data);
                        this.loading = false;
                        })
                        .catch(error => {
                        //process server errors
                        console.log(error.response);
                        this.loading = false;
                        });
            }
        })
    }
},
mounted() {
    card = elements.create("card", { style: style });
    card.mount(this.$refs.card);
    this.firstName = this.$store.state.auth.firstName;
    this.lastName = this.$store.state.auth.lastName;
},
beforeDestroy() {
    card.destroy();
}
}
</script>
<style lang="scss" scoped>
.StripeElement--focus {
border-color: #80bdff;
outline: 0;
box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
transition: border-color 0.15s ease-in-out,
box-shadow 0.15s ease-in-out;
}
</style>