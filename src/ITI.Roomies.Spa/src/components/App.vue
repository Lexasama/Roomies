<template>
  <div id="app">
    <!-- Menu de navigation -->
    <!-- <div> -->
    <el-menu
    id="navMenu"
      default-active="2"
      class="el-menu-vertical-demo"
      :collapse="isCollapse"
      v-bind:style="actualTheme.style"
    >
      <!-- TO DO : Remplacer par l'image de la Colloc -->
      Name : {{$currColloc.collocName}}
      <br>
      <!-- Id : {{$currColloc.collocId}} -->
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
        <span slot="title">Gestion collocation</span>
      </el-menu-item>
      <el-menu-item @click="clickRoute('/roomies/profile')">
        <i class="el-icon-s-custom"></i>
        <span slot="title">Profil</span>
      </el-menu-item>
      <el-menu-item
        @click="clickRoute('/roomies/calendar')"
        :disabled="$setMenuItemDisabled.disableState"
      >
        <i class="el-icon-menu"></i>
        <span slot="title">Calendrier</span>
      </el-menu-item>

      <el-submenu index="2">
        <template slot="title">
          <i class="el-icon-document"/>
          <span>Themes</span>
        </template>

        <el-menu-item
          class="el-submenu-item"
          index="2-1"
          v-for="(i, idx) in styles"
          :key="i.name"
          @click="setTheme(idx)"
        >{{i.name}}</el-menu-item>
      </el-submenu>

      <el-submenu index="1" :disabled="$setMenuItemDisabled.disableState">
        <template slot="title">
          <i class="el-icon-document"/>
          <span>Tâches</span>
        </template>

        <el-menu-item
          class="el-submenu-item"
          index="1-1"
          @click="clickRoute('/task/colloc')"
          :disabled="$setMenuItemDisabled.disableState"
        >Tâche de la Collocation</el-menu-item>
        <el-menu-item
          class="el-submenu-item"
          index="1-2"
          @click="clickRoute('/task/roomie')"
          :disabled="$setMenuItemDisabled.disableState"
        >Vos tâches</el-menu-item>
        <el-menu-item
          class="el-submenu-item"
          index="1-3"
          @click="clickRoute('/task/create')"
          :disabled="$setMenuItemDisabled.disableState"
        >Ajouter une tâche</el-menu-item>
      </el-submenu>

      <el-menu-item @click="clickRoute('/course')" :disabled="$setMenuItemDisabled.disableState">
        <i class="el-icon-location"></i>
        <span slot="title">Listes de courses</span>
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
      <el-menu-item @click="clickRoute('/logout')">
        <i class="el-icon-circle-close"></i>
        <span slot="title">Se déconnecter</span>
      </el-menu-item>
    </el-menu>

    <!-- Affihe le chemin demandé -->
    <!-- <template v-if="isCollapse"> -->
    <template>
      <main v-if="state == true " role="main">
        <loading/>
      </main>
      <main v-else>
        <router-view id="pageContent" class="child"></router-view>
      </main>
    </template>

    <!-- <template v-else>
      <main v-if="state == true " role="main" style="padding-left: 236px;">
        <loading />
      </main>
      <main v-else>
        <router-view id="pageContent" class="child" style="padding-left: 236px;"></router-view>
      </main>
    </template>-->
    <!-- Le footer -->
    <footer id="footer" class="font-small mdb-color lighten-3">
      <div class="container">
        <language/>
      </div>
      <!-- <div class="footer-copyright text-center">
        © 2019 Copyright:
        <a href>Roomie.com</a>
      </div>-->
    </footer>
  </div>
</template>

<script>
import AuthService from "../services/AuthService";
import "../directives/requiredProviders";
import { state } from "../state";
import { inviteRoomieAsync } from "../api/RoomiesApi.js";
import { getCollocNameIdByRoomieIdAsync } from "../api/CollocationApi";
import Language from "../components/Utility/Language.vue";
import Loading from "../components/Utility/Loading.vue";

export default {
  components: {
    Language,
    Loading
  },

  data() {
    return {
      styles: [
        {
          name: "Style1",
          style: "background-color: white;"
        },
        {
          name: "Style2",
          style: "background-color: rgb(142, 142, 142);"
        }
      ],
      themeIdx: 0,
      message: "",
      state: true,
      isCollapse: true
    };
  },
  computed: {
    auth: () => AuthService,
    actualTheme() {
      return this.styles[this.themeIdx];
    },
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

    if (this.$cookies.get("themeIdx") != undefined) {
      this.themeIdx = this.$cookies.get("themeIdx");
    } else {
      this.setTheme(0);
      this.themeIdx = this.$cookies.get("themeIdx");
    }
  },

  methods: {
    setTheme(themeIdx) {
      this.$cookies.set("themeIdx", themeIdx);
      this.themeIdx = themeIdx;
    },
    async invite() {
      await inviteRoomieAsync(this.message);
    },
    clickRoute(pathToRoute) {
      if (!this.isCollapse) this.expand_collapse();
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
  max-height: 100vh;
  border-right: 1px solid #000000;
  position: fixed;
}

.el-menu-item i,
.el-submenu i,
.el-menu-item.is-active,
.el-submenu-item.is-active {
  color: black;
}
.el-submenu-item,
.el-submenu-item.is-active {
  min-width: 12em !important;
}
.el-button {
  background: rgb(102, 102, 102);
  border: 1px solid #000000;
  color: #000000;
}

#footer {
  bottom: 0;
  width: 100%;
  height: 5.5rem; /* Footer height */
  // padding-top: 2.5rem;
  top: 2.5rem;
  padding-left: 12rem;
}
</style>
