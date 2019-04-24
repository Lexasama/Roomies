<template>
  <div id="app">
    <!-- Menu de navigation -->
    <div>
      <el-menu default-active="2" class="el-menu-vertical-demo" :collapse="isCollapse" id="navMenu">
        <!-- <el-menu default-active="2" class="el-menu-vertical-demo" @open="handleOpen" @close="handleClose" :collapse="isCollapse"> -->
        <el-menu-item @click="clickRoute('/')">
          <i class="el-icon-star-on">
            <span slot="title">Accueil</span>
          </i>
        </el-menu-item>
        <br>
        <br>
        <el-button @click.native="expand_collapse">
          <span class="navbar-toggler-icon el-icon-more"></span>
        </el-button>

        <el-submenu index="2" disabled>
          <template slot="title">
            <i class="el-icon-location"></i>
            <span slot="title">Navigator One</span>
          </template>
          <el-menu-item-group disabled>
            <span slot="title">Group One</span>
            <el-menu-item index="2-1">item one</el-menu-item>
            <el-menu-item index="2-2">item two</el-menu-item>
          </el-menu-item-group>
          <el-menu-item-group title="Group Two" disabled>
            <el-menu-item index="2-3">item three</el-menu-item>
          </el-menu-item-group>
          <el-submenu index="2-4" disabled>
            <span slot="title">item four</span>
            <el-menu-item index="2-4-1">item one</el-menu-item>
          </el-submenu>
        </el-submenu>
        <el-menu-item index="3" disabled>
          <i class="el-icon-menu"></i>
          <span slot="title">Navigator Two</span>
        </el-menu-item>
        <el-menu-item index="4" disabled>
          <i class="el-icon-document"></i>
          <span slot="title">Navigator Three</span>
        </el-menu-item>
        <el-menu-item @click="clickRoute('/roomies')">
          <i class="el-icon-setting"></i>
          <span slot="title">/Roomies</span>
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
    </div>
    <!-- Affihe le chemin demandé -->
    <main role="main">
      <router-view class="child"></router-view>
    </main>
  </div>
</template>

<script>
import AuthService from "../services/AuthService";
import "../directives/requiredProviders";
import { state } from "../state";
import {inviteRoomieAsync} from "../api/RoomiesApi.js"

export default {
  data() {
    return {
      message : "",
      state,
      isCollapse: true
    };
  },
  mounted() {
    // Cache le menu de navigation si l'utilisatuer n'est pas connecté
    if (!AuthService.isConnected)
      document.getElementById("navMenu").style.display = "none";
  },

  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },
  methods: {

    async invite() {
      await inviteRoomieAsync(this.message);

    },
    clickRoute(pathToRoute) {
      this.$router.push(pathToRoute);
    },
    // handleOpen(key, keyPath) {
    //   console.log(key, keyPath);
    // },
    // handleClose(key, keyPath) {
    //   console.log(key, keyPath);
    // },
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
  float: left;
  min-height: 100vh;
}
.el-menu-vertical-demo {
  float: left;
  min-height: 100vh;
}
</style>

<style lang="scss" scoped>
@import "../styles/global.scss";
</style>