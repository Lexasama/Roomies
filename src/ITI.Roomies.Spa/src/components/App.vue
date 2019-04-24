<template>
  <div id="app">
    <!-- Menu de navigation -->
    <el-container>
      <el-menu default-active="2" class="el-menu-vertical-demo" :collapse="isCollapse" id="navMenu">

        <!-- TO DO : A styliser -->
        Colloc {{$currColloc.collocName}}
        <el-menu-item @click="clickRoute('/roomies')">
          <i class="el-icon-star-on">
            <span slot="title">Accueil</span>
          </i>
        </el-menu-item>
        <br>
        <br>
        <el-button @click.native="expand_collapse">
          <span class="navbar-toggler-icon el-icon-more"></span>
        </el-button>
        <el-menu-item @click="clickRoute('/roomies/collocation')">
          <i class="el-icon-menu"></i>
          <span slot="title">Create a collocation</span>
        </el-menu-item>
        <el-menu-item  @click="clickRoute('/roomies/calendar')" >
          <i class="el-icon-menu"></i>
          <span slot="title">Calendrier</span>
        </el-menu-item>
        <el-menu-item @click="clickRoute('/task')">
          <i class="el-icon-document"></i>
          <span slot="title">Tâches</span>
        </el-menu-item>
        <el-menu-item @click="clickRoute('/')" disabled>
          <i class="el-icon-setting"></i>
          <span slot="title">Dépense</span>
        </el-menu-item>
        <el-menu-item @click="clickRoute('/')" disabled>
          <i class="el-icon-setting"></i>
          <span slot="title">Paramètres</span>
        </el-menu-item>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <br>
        <el-menu-item @click="clickRoute('/logout')">
          <i class="el-icon-circle-close"></i>
          <span slot="title">Se déconnecter</span>
        </el-menu-item>
      </el-menu>

      <!-- Affihe le chemin demandé -->
      <main role="main" style="padding-left: 50px;">
        <router-view class="child"></router-view>
      </main>
    </el-container>
  </div>
</template>

<script>
import AuthService from "../services/AuthService";
import "../directives/requiredProviders";
import { state } from "../state";
import { inviteRoomieAsync } from "../api/RoomiesApi.js";

export default {
  data() {
    return {
      message: "",
      state,
      isCollapse: true
    };
  },
  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },
  mounted() {
    //Cache le menu de navigation si l'utilisateur n'est pas connecté
    if(!AuthService.isConnected){
      document.getElementById("navMenu").style.display = "none";
    }

  },

  methods: {
    async invite() {
      await inviteRoomieAsync(this.message);
    },
    clickRoute(pathToRoute) {
      this.$router.push(pathToRoute);
    },
    expand_collapse() {
      if (this.isCollapse) this.isCollapse = false;
      else this.isCollapse = true;
    }
  }
};
</script>


<style lang="scss" scoped>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 200px;
  max-width: 200px;
  float: left;
}
.el-menu-vertical-demo {
  float: left;
  min-height: 100vh;
}
</style>

<style lang="scss" scoped>
@import "../styles/global.scss";
</style>