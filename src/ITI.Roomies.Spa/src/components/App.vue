<template>
  <div id="app"  v-bind:style="actualTheme.appStyle">
    <!-- Menu de navigation -->
    <el-menu
      id="navMenu"
      default-active="2"
      class="el-menu-vertical-demo"
      :collapse="isCollapse"
      v-bind:style="actualTheme.style"
    >
      <br />
      <button>
        <img
          class="form-control"
          @click="clickRoute('/roomies')"
          src="../../public/logo_800_300.png"
          style="width: 11rem;height: 4rem;"
        />
      </button>

      <!-- TO DO : Remplacer par l'image de la Colloc -->
      {{$t("colloc")}} :
      <br />

      {{$currColloc.collocName}}
      <br />
      <br />
      <!-- <el-button @click.native="expand_collapse">
        <span class="navbar-toggler-icon el-icon-more"></span>
      </el-button>-->
      <el-menu-item @click="clickRoute('/roomies/collocation')">
        <i class="el-icon-menu"></i>
        <span slot="title">{{$t("manageFlat")}}</span>
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
        <span slot="title">{{$t("cal")}}</span>
      </el-menu-item>
      <!-- 
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
      </el-submenu>-->

      <el-menu-item
        @click="clickRoute('/task/colloc')"
        :disabled="$setMenuItemDisabled.disableState"
      >
        <i class="el-icon-document"></i>
        <span slot="title">{{$t("task")}}</span>
      </el-menu-item>

      <el-menu-item @click="clickRoute('/course')" :disabled="$setMenuItemDisabled.disableState">
        <i class="el-icon-location"></i>
        <span slot="title">{{$t("groceryL")}}</span>
      </el-menu-item>
      <!-- <el-menu-item @click="clickRoute('/spendings')">
        <i class="el-icon-pie-chart"></i>
        <span slot="title">Dépense</span>
      </el-menu-item>-->

      <el-submenu index="2" :disabled="$setMenuItemDisabled.disableState">
        <template slot="title">
          <i class="el-icon-pie-chart" />
          <span>{{$t("spendings")}}</span>
        </template>
        <el-menu-item
          class="el-submenu-item"
          index="2-1"
          @click="clickRoute('/budget/create')"
          :disabled="$setMenuItemDisabled.disableState"
        >Budget</el-menu-item>
        <el-menu-item
          class="el-submenu-item"
          index="2-2"
          @click="clickRoute('/transaction/create')"
          :disabled="$setMenuItemDisabled.disableState"
        >Transaction</el-menu-item>
        <el-menu-item
          class="el-submenu-item"
          index="2-3"
          @click="clickRoute('/Category/create')"
          :disabled="$setMenuItemDisabled.disableState"
        >{{$t("category")}}</el-menu-item>
      </el-submenu>

      <el-submenu index="3">
          <template slot="title">
            <i class="el-icon-setting"></i>
            <span slot="title">{{$t("settings")}}</span>
          </template>

          <el-submenu index="3-1">
            <template slot="title">{{$t("thèmes")}}</template>
            <el-menu-item
              index="3-1-1"
              v-for="(i, idx) in styles"
              :key="i.name"
              @click="setTheme(idx),menustyle()"
            >{{i.name}}</el-menu-item>
          </el-submenu>
        </el-submenu>
      <br />
      <br />
      <br />
      <el-menu-item @click="clickRoute('/logout')">
        <i class="el-icon-circle-close"></i>
        <span slot="title">{{$t("deco")}}</span>
      </el-menu-item>
    </el-menu>
    <!-- <nav class="navbar navbar-expand-lg navbar-dark bg-dark" style='height: 3rem;'>
  
    </nav>-->

    <template>
      <div id="globalContainer">
        <main v-if="state == true " role="main">
          <loading />
        </main>
        <main v-else class="card containerCard">
          <router-view id="pageContent" class="child"></router-view>
        </main>
      </div>
    </template>
  </div>
</template>

<script>
import AuthService from "../services/AuthService";
import "../directives/requiredProviders";
import { inviteRoomieAsync, getCollocPicAsync } from "../api/RoomiesApi.js";
import { getCollocNameIdByRoomieIdAsync } from "../api/CollocationApi";
import Loading from "../components/Utility/Loading.vue";
import aodaod from "../../public/logo_800_300.png";

export default {
  components: {
    Loading
  },

  data() {
    return {
      env: process.env.VUE_APP_BACKEND,
      picPath: null,
      defaultPic: null,
      styles: [
        {
          name: "Light",
          style: "background-color: white !important;",
          appStyle: "background-color: white!important"
        },
        {
          name: "Dark",
          style: "background-color: #8f9196 !important;",
          appStyle: "background-color: #8f9196 !important;"
        },
        {
          name: "Azur Lane",
          style:
            "background: #7F7FD5!important; background: -webkit-linear-gradient(to right, #91EAE4, #86A8E7, #7F7FD5)!important;background: linear-gradient(to right, #91EAE4, #86A8E7, #7F7FD5)!important;",
          appStyle:
            "body, #app, content *{background: #7F7FD5!important; background: -webkit-linear-gradient(to right, #91EAE4, #86A8E7, #7F7FD5)!important;background: linear-gradient(to right, #91EAE4, #86A8E7, #7F7FD5)!important;}"
        },
        {
          name: "Royal Blue Petrol",
          style:
            "background: #BBD2C5 !important;background: -webkit-linear-gradient(to right, #292E49, #536976, #BBD2C5)!important;background: linear-gradient(to right, #292E49, #536976, #BBD2C5)!important;",
          appStyle:
            "body, #app, content *{background: #BBD2C5 !important;background: -webkit-linear-gradient(to right, #292E49, #536976, #BBD2C5)!important;background: linear-gradient(to right, #292E49, #536976, #BBD2C5)!important;background-color: #fafafa;}"
        },
        {
          name: "Lizo",
          style:
            "background: #654ea3;background: -webkit-linear-gradient(to right, #eaafc8, #654ea3);background: linear-gradient(to right, #eaafc8, #654ea3);",
          appStyle:
            "body, #app, content *{background: #654ea3;background: -webkit-linear-gradient(to right, #eaafc8, #654ea3);background: linear-gradient(to right, #eaafc8, #654ea3);}"
        }

      ],
      themeIdx: 0,
      message: "",
      state: true,
      isCollapse: false,
      aodaod: "../../public/logo_800_300.png"
    };
  },
  computed: {
    auth: () => AuthService,
    menustyle() {
      return this.styles[this.themeIdx];
    },
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
        // Récupère la premère collocation du Roomie
        var collocData = await getCollocNameIdByRoomieIdAsync();
        if (collocData != undefined) {
          this.$currColloc.setCollocId(collocData.collocId);
          this.$currColloc.setCollocName(collocData.collocName);
          this.$setMenuItemDisabled.setDisableState(false);
        }
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
      // if (!this.isCollapse) this.expand_collapse();
      this.$router.push(pathToRoute);
    }
    // expand_collapse() {
    //   if (this.isCollapse) {
    //     this.isCollapse = false;
    //   } else {
    //     this.isCollapse = true;
    //   }
    // }
  }
};
</script>

<style lang="scss">
@import "../styles/global.scss";
</style>

<style lang="scss" scoped>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 12rem;
  max-width: 12rem;
  border-right: 1px solid #000000;
  float: left;
  margin-top: 0;
}
.el-menu-vertical-demo {
  float: left;
  min-height: 100vh;
  max-height: 100vh;
  border-right: 1px solid #000000;
  position: fixed;
  margin-top: 0;
}

.el-menu-item i,
.el-submenu i,
.el-menu-item.is-active,
.el-submenu-item.is-active {
  color: black;
}
.el-submenu-item,
.el-submenu-item.is-active {
  min-width: 12rem !important;
}
.el-button {
  background: rgb(102, 102, 102);
  border: 1px solid #000000;
  color: #000000;
}

#footer {
  bottom: 0;
  width: 100%;
  height: 8.5rem; /* Footer height */

  top: 2.5rem;
  padding-left: 12rem;
}

.form-control {
  padding: 0;
  margin-left: 0.5rem;
}
</style>
