<template>
<form @submit.prevent="login" class="p-2">
<b-alert variant="warning" :show="error !== null" dismissible
@dismissed="error = null">
{{ error }}
</b-alert>
<b-alert variant="success" :show="registered &&
error === null">
Registration successful. Please login to continue.
</b-alert>
<p>Login with your e-mail address and password.</p>
<b-form-group>
<label id="label">E-mail</label>
<b-form-input v-model.trim="email" />
</b-form-group>

<b-form-group>
    <label id="label">Password</label>
<b-form-input v-model.trim="password" type="password" />
</b-form-group>
<b-form-group>
<b-button variant="primary" type="submit"
:disabled="false">Login</b-button>
<b-button variant="default" @click="close"
:disabled="false">Cancel</b-button>
</b-form-group>
</form>
</template>
<script>
export default {
name: "login-form",
props: {
    registered: {
        type: Boolean,
        required: false
    }
},
data() {
    return {
        email: "admin@gmail.com",
        password: "Password1*",
        error: null
    };
},
computed: {
    loading() {
    return this.$store.state.loading;
}
},
methods: {
    login() {
        const payload = {
            email: this.email,
            password: this.password
        };
        this.$store
        .dispatch("login", payload)
        .then(response => {
        this.error = null;
        this.email = "";
        this.password = "";
        if (this.$route.query.redirect) {
            this.$router.push(this.$route.query.redirect);
        }
        })
        .catch(error => {
        this.error = error.data;
        });
    },
    close() {
        this.$emit("close");
    }
}
}
</script>
<style scoped>
#label:after {
  content:" *";
  color:red;
}
</style>