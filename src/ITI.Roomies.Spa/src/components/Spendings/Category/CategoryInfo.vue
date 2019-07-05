<template>
  <div>
   
    <div>
    <table>
      <tr>
        <th width=80px>{{Options.categoryName}}</th>
        <th width=100px>
          <tr>Budget : {{budget.amount}}</tr>
          <tr>Depense: {{sum}}</tr>
          <div v-if="reste<0"><tr bgcolor="#FF0000">Reste : {{reste}}</tr></div>
          <div v-if="reste>0"> <tr bgcolor="#4ef542">Reste : {{reste}}</tr></div>
          <div v-else><tr>Reste : {{reste}}</tr></div>
          
        </th>
      </tr>
    </table>
    </div>


  </div>
</template>

<script>

import { getCategoryAsync } from '../../../api/SpendingsApi/CategoryApi';
import { getCurentBudgetOfCategory } from '../../../api/SpendingsApi/BudgetApi';
import { getAllExpenses } from '../../../api/SpendingsApi/TransactionsApi/TBudgetApi'

export default {
  props:{
    Options:{},
  },
  data() {
    return {
      budget: {},
      depense: 0,
      categoryId: null,
      expensesList:[],
      sum: 0,
      reste: null,
    };
  },

  async mounted() {
    console.log("options=");
    console.log(this.Options);
    this.categoryId =this.Options.categoryId;
    await this.refresh();
  },
  
  methods: {
    async refresh(){
      this.budget= await getCurentBudgetOfCategory(this.categoryId);
      var id = this.budget.budgetId;
      this.expensesList = await getAllExpenses(id);
     
      var sum = 0;
      for(var i=0; i<this.expensesList.length; i++){
        this.sum = this.expensesList[i].price + this.sum;
      }
      this.reste = this.budget.amount - this.sum;
     
    }
    
  }
};
</script>



<style lang="scss" scoped>
.el-row {
  margin-bottom: 20px;
  &:last-child {
    margin-bottom: 0;
  }
}
.el-col {
  border-radius: 4px;
}
.bg-purple-dark {
  background: #99a9bf;
}
.bg-purple {
  background: #d3dce6;
}
.bg-purple-light {
  background: #e5e9f2;
}
.grid-content {
  border-radius: 4px;
  min-height: 36px;
}
.row-bg {
  padding: 10px 0;
  background-color: #f9fafc;
}
</style>
