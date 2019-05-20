<template>
  <div class="p-5 text-center">
    <i class="fa fa-refresh fa-spin fa-3x fa-fw"></i>
    <br>
    <h1>Déconnexion en cours...</h1>

    <iframe :src="logoutEndpoint" frameborder="0" width="0" height="0"></iframe>
  </div>
</template>

<script>
import AuthService from "../services/AuthService";

export default {
  mounted() {
    AuthService.registerSignedOutCallback(() => this.onSignedOut());
  },

  beforeDestroy() {
    AuthService.removeSignedOutCallback(() => this.onSignedOut());
  },

  computed: {
    logoutEndpoint: () => AuthService.logoutEndpoint
  },

  methods: {
    onSignedOut() {
      // Cache le menu de navigation avant la déconnexion
      document.getElementById("navMenu").style.display = "none";
      // Reset les variable globales
      this.$currColloc.setCollocId(-1);
      this.$currColloc.setCollocName("");
      this.$setMenuItemDisabled.setDisableState(true);
      this.$router.replace("/");
    }
  }
};
</script>

<style lang="scss">
</style>