<template>
  <div class="createContainer">
    <!-- <main v-if="idIsUndefined == false">
      <header v-if="route == 'create'">
        <h2>Créer une categori</h2>
      </header>
      <header v-if="route == 'edit'">
        <h2>Modifier la categori</h2>
    </header>-->
    <main>
      <header>creer une category</header>

      <form @submit="onSubmit($event)">
        <div class="alert alert-danger" v-if="errors.length > 0">
          <b>Les champs suivants semblent invalides :</b>

          <ul>
            <li v-for="e of errors" :key="e">{{e}}</li>
          </ul>
          <div>
            <label class="required">Nom</label>
            <br>
            <input type="text" v-model="category.categoryName" required>
          </div>

          <div>
            <label>Icon</label>
            <br>
            <input name="icon" value="Icon1" v-model="category.iconName">
            <div v-for="(icon, index) in icons" :key="index">
              <img :src="iconPath + '/'+icon.iconName+'.png'" @click="test(icon.iconName)">
            </div>
          </div>
        </div>
        
        
        <br>
        <br>
        <br>
        <br>
        <button class="btn btn-dark" @click="onSubmit">Sauvegarder</button>
      </form>
    </main>
  </div>
</template>

<script>
import {
  getDefaultIconsAsync,
  createCategoryAsync,
  getCategoryAsync,
  getCategoriesAsync,
  deleteCategoryAsync,
  updateCategoryAsync
} from "../../api/SpendingsApi/CategoryApi";
import AuthService from "../../services/AuthService";
import { state } from "../../state";
import Loading from "../../components/Utility/Loading.vue";
import Budget from "../Spendings/Budget.vue";
export default {
  components: {
    Loading,
    Budget
  },

  data() {
    return {
      errors: [],
      route: null,
      state: true,
      category: {},
      idIsUndefined: true,
      icon: "",
      icons: [],
      iconPath: "http://localhost:5000/Pictures/Icons",
      budget: {},
      categoies: null,
    };
  },

  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },

  async mounted() {
    this.collocId = this.$currColloc.collocId;
    this.icons = await getDefaultIconsAsync();
    console.log(this.icons);
    console.log(this.collocId);
    this.budget.collocId = this.collocId;
    this.categories = getCategoriesAsync(this.collocId);
    debugger;
  },

  methods: {
    async onSubmit() {
      event.preventDefault();

      console.log(this.category.iconName);
      //this.category.categoryId = [];
      var errors = [];
      if (!this.category.categoryName) errors.push("Category Name");
      if (!this.category.icon) errors.push("Icon");

      this.errors = errors;
      this.category.collocId = this.collocId;
      await createCategoryAsync(this.category);
    },
    async test(name) {
      this.category.iconName = name;
      await this.onSubmit();
    }
  }
};
</script>
