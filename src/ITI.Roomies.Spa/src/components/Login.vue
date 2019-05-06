<template>
    <el-container>
            <el-header><h2>Bienvenue sur Roomies</h2></el-header>

            <el-button  @click="login('Google')" class="btn btn-lg btn-block btn-primary">
              <br>
                  <i class="fa fa-google" aria-hidden="true"></i> Se connecter via Google</el-button>
              <br/>
            <el-button  @click="login('Base')" class="btn btn-lg btn-block btn-default">Se connecter via Roomies</el-button>
    </el-container>
</template>

<script>
import AuthService from '../services/AuthService'
import Vue from 'vue'
/*import onepagescroll from '../styles/onepagescroll'

onepagescroll(".main", {
     sectionContainer: "section",
     loop: true,
     responsiveFallback: false
   });*/

export default {
    data() {
        return {
            endpoint: null
        }
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
            this.$router.replace('/checkRoomie');
        }
    }
}
</script>

<style lang="scss">
iframe {
  width: 100%;
  height: 600px;
}
</style>