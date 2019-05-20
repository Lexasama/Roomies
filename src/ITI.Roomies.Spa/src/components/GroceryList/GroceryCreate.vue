<template>
  <!-- <div>
    <div>
      <h1 > Creer une liste  de course</h1>
    </div>
    <div>
      <form @submit="onSubmit($event)">
        <label>Nom</label>
        <input type="text" v-model="course.courseName">
        <label>Date</label>
        <input type="date" v-model="course.courseDate">
        <button type="submit">Sauvegarder</button>
      </form>
    </div>
  </div>-->

  <div id="container" v-if="!state">
    <main v-if="!idIsUndefined">
      <header v-if="route == 'create'">
        <h2>Créer une liste de course</h2>
      </header>

      <header v-if="route == 'edit'">
        <h2>Modifier la liste de course</h2>
      </header>

      <form>
        <div>
          <label class="required">Nom</label>
          <br>
          <input class="form-control" type="text" v-model="course.courseName" required>
        </div>

        <div>
          <label>Date</label>
          <br>
          <input class="form-control" type="date" v-model="course.courseDate">
        </div>

        <br>
        <button class="btn btn-dark" @click="onSubmit">Sauvegarder</button>
      </form>
    </main>
    <main v-else>Erreur</main>
  </div>
  <div id="container" v-else>Chargement en cours</div>
</template>

<script>
import {
  createGroceryListAsync,
  getGroceryListByIdAsync,
  updateAgroceryListAsync
} from "../../api/GroceriesApi";
import AuthService from "../../services/AuthService";
import { state } from "../../state";

export default {
  data() {
    return {
      errors: [],
      course: {},
      checkedRoomiesList: [],
      route: null,
      idIsUndefined: true,
      state: true,
      id: null
    };
  },

  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },
  async mounted() {
    //route
    if (this.$route.fullPath.replace("/course/", "") == "create") {
      this.route = "create";
      this.idIsUndefined = false;
    } else {
      this.route = "edit";

      if (this.$route.params.id == undefined) {
        this.idIsUndefined = true;
      } else {
        this.course.courseId = this.$route.params.id;

        try {
          this.course = await getGroceryListByIdAsync(this.course.courseId);
          this.idIsUndefined = false;
        } catch (e) {
          console.log(e);
          this.idIsUndefined = true;
        }
      }
    }
    this.state = false;
  },

  methods: {
    async onSubmit() {
      event.preventDefault();

      var errors = [];

      if (!this.course.courseName) errors.push("Nom");
      if (!this.course.courseDate) errors.push("Date");

      if (errors.length == 0) {
        try {
          if (this.route == "create") {
            this.course.collocId = this.$currColloc.collocId;
            await createGroceryListAsync(this.course);
            this.$router.push('/course');
          }
          if (this.route == "edit") {
            await updateAgroceryListAsync(this.course);
            this.$router.push('/course');
          }
        } catch (e) {
          console.error(e);
        }
      } else {
        for (var j = 0; j < errors.length; j++) {
          console.log(errors[j]);
        }
      }
    }
  }
};
</script>

<style lang="scss" scoped>
.input-border {
  border-width: 1px;
}
</style>

