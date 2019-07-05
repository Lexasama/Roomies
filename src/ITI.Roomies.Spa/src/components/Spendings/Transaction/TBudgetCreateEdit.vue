<template>
  <div>
    <div>
      <form @submit="onSubmit($event)">
        <div class="alert alert-alert" v-if="errors.length > 0">
          <b>Les champs suivants semblent invalides : </b>
          <ul>
            <li v-for="e of errors" :key="e">{{e}}</li>
          </ul>
        </div>
        <div>
          <label for="Price">Price </label>
         <el-input-number
         controls-position="right"
            type="number"
            name="Price"
            v-model="TBudget.price"
            
            required>
          </el-input-number>
        </div>
        <div>
          <label for="Date">Date</label>
          <el-date-picker
          name='Date'
            v-model="TBudget.date"
            type="date"
            placeholder="Date de la transaction"
            required            
          ></el-date-picker>
        </div>
        <div>
          <label for="category">Categorie</label>
          <el-select v-model="TBudget.categoryId" id='category' clearable placeholder="Categories">
            <el-option
              v-for="category in categories"
              :key="category.categoryId"
              :label="category.categoryName"
              :value="category.categoryId"
            ></el-option>
          </el-select>
        </div> 
        <br><br>
       <el-button type="primary" @click="onSubmit">Sauvegarder</el-button>
      </form>
    </div>

  </div>
</template>

<script>
import { createTransacBugetAsync, updateTransacBudgetAsync, getTransacBudgetAsync } from '../../../api/SpendingsApi/TransactionsApi/TBudgetApi';
import { getCategoriesAsync } from "../../../api/SpendingsApi/CategoryApi";

export default {
data(){
    return {
      errors: [],
      TBudget: {},
      collocId: null,
      categories: [],
      TBudgetId: null,
      mode: null,
    }
  },
  watch:{
    "$route.params.mode": async function(mode) {
      console.log("my watcher");
      await this.refreshList();
      await this.edition();
    }
  },

  async mounted() {
    this.collocId = this.$currColloc.collocId;
    this.categories = await getCategoriesAsync(this.collocId);
    await this.refreshList();
    
    await this.edition();
    
  },


  methods: {
    async refreshList(){
      this.mode = this.$route.params.mode;
      this.TBudgetId = this.$route.params.id;
    },

    async edition(){
      if( this.mode == 'edit'){
      try{
        const tbudget = await getTransacBudgetAsync(this.TBudgetId);
        this.TBudget = tbudget;
        this.errors.push(this.TBudget.errorMessage);
      } catch(e) {
        console.error(e);
      }finally{
        await this.refreshList();
      }
   }  
    },
       show(){
      this.$message({
          showClose: true,
          message: 'La Transaction  a bien été ajoutée !',
          type: 'success'
        });
    },

    async onSubmit(){
      event.preventDefault();

      if(!this.TBudget.price) this.errors.push("Price");
      if(!this.TBudget.price) this.errors.push("Price must be positive");
      if(!this.TBudget.date) this.errors.push("Date");
      if(!this.TBudget.categoryId) this.errors.push("Category");

      if(this.errors.length == 0){
      try {
        if (this.mode == 'create' ){
          await createTransacBugetAsync(this.TBudget);
        }
        else{
          await updateTransacBudgetAsync(this.TBudget);
        }
      } catch(e) {
        console.error(e);
      } finally {
        await this.refreshList();
        this.$root.$emit('update');
        this.TBudget = {};
        this.$router.replace("/transaction/create");
        this.show();
      }
      }
    },
  }
}
</script>
<style lang="scss" scoped>
label{
margin-right: 100px;
}
</style>
