<template>
  <div id="app">
    <div id="centered_container">
      <h2 class="centered">Bienvenue</h2>

      <div>
        <button
          style="margin-top:16px"
          id="centered_container"
          @click="login('Google')"
          class="btn btn-lg btn-block btn-primary"
        >
          <i class="fa fa-google" aria-hidden="true"></i>
          connection
        </button>
        <br />
        <button
          id="centered_container"
          @click="login('Base')"
          class="btn btn-block btn-lg btn-dark"
        >connection</button>
      </div>
    </div>
    <div style="padding-top: 3rem;">
      <language />
    </div>
  </div>
</template>

<script>
import AuthService from "../services/AuthService";
import Vue from "vue";

export default {
  components: {},
  data() {
    return {
      endpoint: null
    };
  },

  mounted() {
    AuthService.registerAuthenticatedCallback(() => this.onAuthenticated());
    document.getElementById("globalContainer").style.marginRight = "12.5rem";
  },

  beforeDestroy() {
    AuthService.removeAuthenticatedCallback(() => this.onAuthenticated());
  },

  methods: {
    login(provider) {
      AuthService.login(provider);
    },

    onAuthenticated() {
      document.getElementById("globalContainer").style.marginRight = "0rem";
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
#centered_container {
  margin-left: auto;
  margin-right: auto;
  max-width: 40em;
}
.centered {
  text-align: center;
  margin-top: 1.25rem;
  margin-bottom: 1.25rem;
}
.btn-block {
  max-width: 30em;
}
</style>