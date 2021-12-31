<template>
<form @submit.prevent="submit" class="p-2">
<b-alert variant="danger" :show="regErrors !== null" dismissible
@dismissed="regErrors = null">
<!-- <p class="error" v-if="!$v.password.required">This field is required</p> -->

<div class="error" v-for="(error, index) in regErrors" :key="index">
{{ error[0] }}
</div>
<p class="error" v-if="!$v.password.containsUppercase">Password need at least one Uppercase.</p>
<p class="error" v-if="!$v.password.containsNumber">Password need at least one number.</p>
<p class="error" v-if="!$v.password.containsSpecial">Password need at least one special key:#?!@$%^&*-.</p>
<!-- <p class="error" v-if="!$v.confirmPassword.sameAsPassword">Password and Confirm Password should match.</p> -->
</b-alert>
<!-- <b-form-group id="label" label="First Name"> -->
<b-form-group>
<label id="label">First Name</label>
<b-form-input id="input" v-model.trim="firstName" required/>
</b-form-group>
<b-form-group>
  <label id="label">Last Name</label>
<b-form-input id="input" v-model.trim="lastName" required/>
</b-form-group>
<b-form-group>
  <label id="label">E-mail</label>
<b-form-input id="input" v-model.trim="email" required/>
</b-form-group>
<b-form-group>
  <label id="label">Password</label>
<b-form-input id="input" v-model.trim="password" type="password" v-model="$v.password.$model" 
required pattern="(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,16}"
title="Password must contain at least 8 characters, including UPPER/lowercase,
 numbers and special character"/>
</b-form-group>
<b-form-group>
<label id="label">Confirm Password</label>
<b-form-input id="input" v-model.trim="confirmPassword" required
type="password" />
</b-form-group>

<b-form-group>
<b-button variant="primary" type="submit"
:disabled="false">Register</b-button>
<b-button variant="default" @click="close"
:disabled="false">Cancel</b-button>
</b-form-group>
</form>
</template>
<script>
import { required, minLength, containsUppercase, containsLowercase, containsNumber, containsSpecial} from 'vuelidate/lib/validators'
export default {
name: "register-form",
data() {
    return {
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    confirmPassword: "",
    regErrors: null
    };
},
validations: {
  password: {
    required,
     minLength: minLength(8),  // I assume you'd want something like this too
    containsUppercase: function(value) {
      return /[A-Z]/.test(value)
    },
    containsLowercase: function(value) {
      return /[a-z]/.test(value)
    },
    containsNumber: function(value) {
      return /[0-9]/.test(value)
    },
    containsSpecial: function(value) {
      return /[#?!@$%^&*-]/.test(value)
    }
  },
},
computed: {
    loading() {
     return this.$store.state.loading;
    }
},
methods: {
    submit() {
    const payload = {
        firstName: this.firstName,
        lastName: this.lastName,
        email: this.email,
        password: this.password,
        confirmPassword: this.confirmPassword
    };
    this.$store
    .dispatch("register", payload)
    .then(response => {
        this.regErrors = null;
        this.firstName = "";
        this.lastName = "";
        this.email = "";
        this.password = "";
        this.confirmPassword = "";
        this.$emit("success");
    })
    .catch(error => {
		if (typeof error.data === "string" || error.data instanceof String) {
			this.regErrors = { error: [error.data] };
		} else {
			this.regErrors = error.data;
		}
    });
    },

    close() {
        this.regErrors = null;
        this.$emit("close");
    }
    
}
};
</script>
<style scoped>
.error {
  color: brown;
}
p { margin:0 }
#label:after {
  content:" *";
  color:red;
}
#input:invalid {
  background-color: #ffdddd;
}

</style>