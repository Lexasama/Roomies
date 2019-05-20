<template>
  <div id="app">
    <!-- Menu de navigation -->
    <!-- <div> -->
    <el-menu default-active="2" class="el-menu-vertical-demo" :collapse="isCollapse" id="navMenu">
      <!-- TO DO : Remplacer par l'image de la Colloc -->
      Name : {{$currColloc.collocName}}
      <br>
      Id : {{$currColloc.collocId}}
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
      <el-menu-item
        @click="clickRoute('/roomies/calendar')"
        :disabled="$setMenuItemDisabled.disableState"
      >
        <i class="el-icon-menu"></i>
        <span slot="title">Calendrier</span>
      </el-menu-item>

      <el-submenu index="1" :disabled="$setMenuItemDisabled.disableState">
        <template slot="title">
          <i class="el-icon-document"/>
          <span>Tâches</span>
        </template>

        <el-menu-item
          index="1-1"
          @click="clickRoute('/task/colloc')"
          :disabled="$setMenuItemDisabled.disableState"
        >Tâche Collocation active</el-menu-item>
        <el-menu-item
          index="1-2"
          @click="clickRoute('/task/roomie')"
          :disabled="$setMenuItemDisabled.disableState"
        >Tâches Roomie</el-menu-item>
        <el-menu-item
          index="1-3"
          @click="clickRoute('/task/create')"
          :disabled="$setMenuItemDisabled.disableState"
        >Ajouter tâche</el-menu-item>
      </el-submenu>

      <el-menu-item @click="clickRoute('/course')" :disabled="$setMenuItemDisabled.disableState">
        <i class="el-icon-location"></i>
        <span slot="title">GroceryList</span>
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
      <el-menu-item @click="clickRoute('/logout')">
        <i class="el-icon-circle-close"></i>
        <span slot="title">Se déconnecter</span>
      </el-menu-item>
    </el-menu>

    <!-- Affihe le chemin demandé -->
    <template v-if="isCollapse">
      <main v-if="state == true " role="main" style="padding-left: 100px;">Chargement en cours</main>
      <main v-else>
        <router-view id="pageContent" class="child" style="padding-left: 100px;"></router-view>
      </main>
    </template>
    <!-- </container> -->
    <template v-else>
      <main v-if="state == true " role="main" style="padding-left: 236px;">Chargement en cours</main>
      <main v-else>
        <router-view id="pageContent" class="child" style="padding-left: 236px;"></router-view>
      </main>
    </template>
    <!-- </container> -->
  </div>
</template>

<script>
import AuthService from "../services/AuthService";
import "../directives/requiredProviders";
import { state } from "../state";
import { inviteRoomieAsync } from "../api/RoomiesApi.js";
import { getCollocNameIdByRoomieIdAsync } from "../api/CollocationApi";

export default {
  data() {
    return {
      message: "",
      state: true,
      isCollapse: true
    };
  },
  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },
  async mounted() {
    this.state = true;
    //Cache le menu de navigation si l'utilisateur n'est pas connecté
    try {
      if (!AuthService.isConnected) {
        document.getElementById("navMenu").style.display = "none";
      } else {
        //if (this.$checkedGoogle) {
        // Récupère la premère collocation du Roomie
        var collocData = await getCollocNameIdByRoomieIdAsync();
        if (collocData != undefined) {
          this.$currColloc.setCollocId(collocData.collocId);
          this.$currColloc.setCollocName(collocData.collocName);
          this.$setMenuItemDisabled.setDisableState(false);
        }
        //} else {
        // document.getElementById("navMenu").style.display = "none";
        // this.$router.replace("/checkRoomie");
        // }
      }
    } catch (e) {
      console.log(e);
    } finally {
      this.state = false;
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
      if (this.isCollapse) {
        this.isCollapse = false;
        // document.getElementById("pageContent").style.paddingLeft = "236px";
      } else {
        this.isCollapse = true;
        // document.getElementById("pageContent").style.paddingLeft = "100px";
      }
    }
  }
};
</script>

<style lang="scss">
@import "../styles/global.scss";
</style>

<style lang="scss" scoped>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 12em;
  max-width: 12em;
  border-right: 1px solid #000000;
  float: left;
}
.el-menu-vertical-demo {
  float: left;
  min-height: 100vh;
  border-right: 1px solid #000000;
}

.el-menu {
  background-color: rgb(142, 142, 142);
}

.el-menu-item i,
.el-submenu i,
.el-menu-item.is-active,
.el-submenu-item.is-active {
  color: black;
}

.el-button {
  background: rgb(102, 102, 102);
  border: 1px solid #000000;
}
</style>
