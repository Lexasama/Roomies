<template>
  <div>
    <div>
      <h1 v-if="mode == 'create'">Créer un Budget pour une catégorie</h1>
      <h1 v-else>Editer le Budget</h1>
    </div>

    <div>
      <form @submit="onSubmit($event)">
        <div class="alert alert-danger" v-if="errors.length > 0 ">
          <b>Les Champs suivants semblent invalides :</b>
          <ul>
            <li v-for="e of errors" :key="e">{{e}}</li>
          </ul>
        </div>

        <div>
          <label class="label" for="">Choississez une Category</label>
          <el-select 
            v-model="budget.categoryId"
            placeholder="Select the category"
            @change="getOffDates"
            required
           >
            <el-option
              v-for="category in categoriesList"
              :key="category.categoryId"
              :label="category.categoryName"
              :value="category.categoryId"
            ></el-option>
          </el-select>
        </div>
        <div>
          <div v-if="radio1 != null && radio1 != 'Custom' ">
            <p>ends {{endDate()}}</p>
          </div>
          <label v-if="radio1 == 'Monthly'" for="Amount">Monthly Amount</label>
          <label v-if="radio1 === 'Yearly'" for="Amount">Yearly Amount</label>

          <label v-if="radio1 ==='Custom'" for="Amount">Amount</label>

          <label class="label">Amount</label>
          <el-input-number v-model="budget.amount" controls-position="right" :min="1"></el-input-number>
          <el-radio-group v-model="radio1">
            <el-radio-button label="Monthly"></el-radio-button>
            <el-radio-button label="Yearly"></el-radio-button>
            <el-radio-button label="Custom"></el-radio-button>
          </el-radio-group>
        </div>

        <div class="block" v-if="radio1 == 'Custom'">
          <span class="demonstration">choisissez une date</span>
          <el-date-picker class="D1"
            v-model="budget.date1"
            type="date"
            placeholder="Date de debut"
            required
            :picker-options="datePickerOptions"
          ></el-date-picker>
          <el-date-picker v-model="budget.date2"
            type="date" 
            placeholder="Date de fin" 
            required 
            :picker-options="datePickerOptions" >
          </el-date-picker>
        </div>
      </form>

      <button class="btn btn-dark" type="primary"  @click="onSubmit">Sauvegarder</button>
    </div>


  </div>
</template>

<script>
import {
  createBudgetAsync,
  getAllBudgetAsync,
  getAllBudgetOfCategoryAsync,
  getCategoryOffDates
} from "./../../../api/SpendingsApi/BudgetApi";
import { getCategoriesAsync, } from "./../../../api/SpendingsApi/CategoryApi";

export default {
  data() {
    return {
      errors: [],
      categoriesList: [],
      budget: {},
      offDatesList:[],
      mode: null,
      collocId: null,
      budgets: [],
      radio1: null,
      datePickerOptions: { },
    };
  },

  async mounted() {
    this.mode = this.$route.params.mode;
    this.collocId = this.$currColloc.collocId;
    this.categoriesList = await getCategoriesAsync(this.collocId);

    if (this.mode == "edit") {
      try {
      } catch (errors) {
        console.error(errors);
      } finally {
        await this.refreshList();
      }
    }
  },

  computed: {
    disabledDates(date){
      return !this.offDatesList.includes(date)
    }
  },

  methods: {
    async refreshList() {
      try {
      } catch (e) {
        console.error(errors);
      }
    },

endDate(radio1) {
      var startDate = new Date();
      this.budget.date1 = startDate;

      if (this.radio1 == "Monthly") {
        var endDate = new Date(startDate.getFullYear(), startDate.getMonth() + 1, 0);
        this.budget.date2 = endDate;

        return endDate;
      }
      if (this.radio1 == "Yearly") {
        var endDate = new Date(startDate.getFullYear(), 11, 31);
        this.budget.date2 = endDate;

        return endDate;
      }
    },

    async getOffDates(){
      console.log(' !! offDates !! ');
      this.offDatesList = await getCategoryOffDates(this.budget.categoryId);
      this.datePickerOptions.disabledDate = this.disabledDates;
    },

    async onSubmit() {
      event.preventDefault();

      var errors = [];
      if (!this.budget.amount) errors.push("amount");
      if (this.budget.amount < 0) errors.push("amount should be positive");
      if(!this.budget.categoryId) errors.push("Category");
      if (!this.budget.date1) errors.push("Start date");
      if (!this.budget.date2) errors.push("End date");
      if (this.budget.date1 > this.budget.date2) {
        var date = this.budget.date1;
        this.budget.date1 = this.budget.date2;
        this.budget.date2 = date;
      }

      this.errors = errors;

      if (errors.length == 0) {
        try {
          if (this.mode == "create") {
            await createBudgetAsync(this.budget);
          }
        } catch (errors) {
          console.error(errors);
        }
      }
    } //End onSubmit
  } //end Methods
}; //end Export
</script>

<style lang="scss" scoped>
.label{
margin-right: 100px;
}
</style>

