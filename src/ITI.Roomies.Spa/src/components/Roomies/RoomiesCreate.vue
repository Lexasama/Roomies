<template>
  <div class="container">
    <div>
      <h1>S'inscrire</h1>
    </div>

    <form @submit="onSubmit($event)">
      <div class="alert alert-danger" v-if="errors.length > 0">
        <b>Les champs suivants semblent invalides :</b>

      </div>

      <div class="form-group">
        <label class="required">Nom</label>
        <input type="text" v-model="item.lastName" class="form-control" required>
      </div>

      <div class="form-group">
        <label class="required">Prénom</label>
        <input type="text" v-model="item.firstName" class="form-control" required>
      </div>

      <div class="form-group">
        <label class="required">Date de naissance</label>
        <input type="date" v-model="item.birthDate" class="form-control" required>
      </div>

      <div class="form-group">
        <label class="required">Phone</label>
        <input type="text" v-model="item.phone" class="form-control" required>
      </div>

      <!-- <div class="form-group">
        <input v-model="item.email" v-bind:value="AuthService.email">
      </div>-->

      <el-button native-type="submit" class="btn btn-primary">Sauvegarder</el-button>
    </form>
  </div>
</template>

<script>
import { createRoomieAsync } from "../../api/RoomiesApi";
import { DateTime } from "luxon";
import AuthService from "../../services/AuthService";

export default {
  data() {
    return {
      item: {},
      id: null,
      errors: []
    };
  },

  async mounted() {
    this.id = this.$route.params.id;
    //this.item.email = AuthService.email;
  },

  methods: {
    async onSubmit(event) {
      event.preventDefault();

      var errors = [];

      if (!this.item.lastName) errors.push("Nom");
      if (!this.item.firstName) errors.push("Prénom");
      if (!this.item.birthDate) errors.push("Date de naissance");
      if (!this.item.phone) errors.push("Téléphone");

      this.errors = errors;

      if (errors.length == 0) {
        try {
          var idRoomie = await createRoomieAsync(this.item);
          // Affiche le menu de navigation
          document.getElementById("navMenu").style.display = "block";
          // Reroute le tableau de bord à partir de l'id
          this.$router.replace("/roomies/" + idRoomie);
        } catch (e) {
          console.error(e);
          //  window.alert(e);
        }
      }
    }
  }
};
</script>

<style lang="scss" scoped>
@import "../../styles/global.scss";
</style>