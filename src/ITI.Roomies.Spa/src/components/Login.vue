<template>
  <div id="centered_container">
    <header class="centered">
      <h2>Bienvenue sur Roomies</h2>
    </header>

    <button id="centered_container" @click="login('Google')" class="btn btn-lg btn-block btn-primary">
      <i class="fa fa-google" aria-hidden="true"></i> Se connecter via Google
    </button>
    <br>
    <button id="centered_container"
      @click="login('Base')"
      class="btn btn-block btn-lg btn-dark"
    >Se connecter via Roomies</button>
  </div>
</template>

<script>
import AuthService from "../services/AuthService";
import Vue from "vue";

export default {
  data() {
    return {
      endpoint: null
    };
  },

  mounted() {
    AuthService.registerAuthenticatedCallback(() => this.onAuthenticated());
  },

  beforeDestroy() {
    AuthService.removeAuthenticatedCallback(() => this.onAuthenticated());
  },

  methods: {
    login(provider) {
      AuthService.login(provider);
    },

    onAuthenticated() {
      // Envoie sur la page de vérification du profil Roomie
      this.$router.replace("/checkRoomie");
    }
  }
};
</script>

<style lang="scss" scoped>
iframe {
  width: 50%;
  height: 600px;
}
#centered_container{
    margin-left: auto;
    margin-right: auto;
    max-width: 40em;
}
.centered{
   text-align:center;
}
.btn-block{
  max-width: 30em;
}


</style>