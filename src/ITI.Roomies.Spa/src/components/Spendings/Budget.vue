<template>
  <div></div>
</template>

<script>
import {
  getAllBudgetAsync,
  getBudgetAsync,
  createBudgetAsync
} from "../../api/SpendingsApi/BudgetApi";
import { getCategoriesAsync } from "../../api/SpendingsApi/CategoryApi";
import AuthService from "../../services/AuthService";
import Loading from "../../components/Utility/Loading.vue";

export default {
  data() {
    return {
      errors: [],
      state: true,
      idIsUndefined: true,
      bugetsList: [],
      budget: {},
      categoriesList: []
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
  },

  methods: {
    async onSubmit() {
      event.preventDefault();
      console.log(this.budget.categoryId);

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

