<template>
    <el-container>
    <el-header>
      <h2>S'inscrire</h2>
    </el-header>
    <el-main>
    <el-form @submit="onSubmit($event)">
      <div class="alert alert-danger" v-if="errors.length > 0">
        <b>Les champs suivants semblent invalides :</b>

        <ul>
          <li v-for="e of errors">{{e}}</li>
        </ul>
      </div>

      <div>
        <label class="required">Nom</label>
        <el-input type="text" v-model="item.lastName" required />
      </div>

      <div>
        <label class="required">Prénom</label>
        <el-input type="text" v-model="item.firstName" required />
      </div>

      <div>
        <label class="required">Date de naissance</label>
        <el-input type="date" v-model="item.birthDate" required />
      </div>

      <div>
        <label class="required">Phone</label>
        <el-input type="text" v-model="item.phone" required />
      </div>
      <br>
      <br>
      <el-button @click="onSubmit">Sauvegarder</el-button>
    </el-form>
    </el-main>
    </el-container>  
</template>

<script>
import { createRoomieAsync } from "../../api/RoomiesApi";
import { DateTime } from "luxon";
import AuthService from '../../services/AuthService'

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
  },

  methods: {
    async onSubmit() {
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
          this.$router.replace("/roomies/" + idRoomie);
        } catch (e) {
          console.error(e);
        }
      }
    }
  }
};
</script>

<style lang="scss">
@import "../../styles/global.scss";
</style>