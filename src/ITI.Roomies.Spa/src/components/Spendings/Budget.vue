<template>
  <div class="createContainer">
    <div>
      <H1>Attribuer un budget à la categorie</H1>
    </div>

    <div>
      <form @submit="onSubmit($event)">
        <div class="alert alert-alert" v-if="errors.length > 0">
          <b>Les champs suivants semblent invalides:</b>
          <ul>
            <li v-for="e of errors" :key="e">{{e}}</li>
          </ul>
        </div>

        <label for="amount" class="required">Amount</label>
        <br>
        <input type="text" name="amount" v-model="budget.amount" required>
        <!-- <div>
          <label for="date1" class="required">Debut</label>
          <br>
          <input type="date" v-model="budget.date1" required>
        </div>
        <div>
          <label for="date2" class="required">Fin</label>
          <br>
          <input type="date" v-model="budget.date2" required>
        </div>-->
        <div class="block">
          <span class="demonstration">choisissez une date</span>
          <el-date-picker v-model="budget.date1" type="date" placeholder="Date de debut"></el-date-picker>
          <el-date-picker v-model="budget.date2" type="date" placeholder="Date de fin"></el-date-picker>
        </div>
        <br>
        <div>
          <el-select v-model="budget.categoryId" placeholder="Select the category">
            <el-option
              v-for="category in categories"
              :key="category.categoryName"
              :label="category.categoryName"
              :value="category.categoryId"
            ></el-option>
          </el-select>
        </div>
      </form>
      <el-button type="primary" round @click="onSubmit">Sauvegarder</el-button>
    </div>
  </div>
</template>

<script>
import {
  getAllBudgetAsync,
  getBudgetAsync,
  createBudgetAsync
} from "../../api/SpendingsApi/BudgetApi";
import { getCategoriesAsync } from "../../api/SpendingsApi/CategoryApi";
import AuthService from "../../services/AuthService";
import { state } from "../../state";
import Loading from "../../components/Utility/Loading.vue";

export default {
  data() {
    return {
      errors: [],
      state: true,
      idIsUndefined: true,
      bugets: null,
      budget: {},
      categories: null
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
    this.categories = await getCategoriesAsync(this.collocId);
    //this.budgets = await getAllBudgetAsync(this.collocId);
  },

  methods: {
    async onSubmit() {
      event.preventDefault();
      console.log(this.budget.categoryiId);

      var errors = [];
      if (!this.budget.amount) errors.push("Amount");
      if (!this.budget.date1) errors.push("Date1");
      if (!this.budget.date2) errors.push("Date2");
      if (!this.budget.categoryId) errors.push("Category");

      this.errors = errors;
      this.budget.collocId = this.collocId;
      await createBudgetAsync(this.budget);
    }
  }
};
</script>

