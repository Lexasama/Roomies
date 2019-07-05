<template>
  <div>
    <div>
      <h1>Ajouter une Transaction</h1>
    </div>

    <div>
      <el-tabs type="card">
        
      <el-tab-pane label="Roomie">
         <TDepenseCreateEdit :key="force"></TDepenseCreateEdit>
      </el-tab-pane>

      <el-tab-pane label="Category"> 
        <TBudgetCreateEdit :key="force"></TBudgetCreateEdit>
      </el-tab-pane>

      </el-tabs>
    </div>
    
  </div>
</template>

<script>

import TDepenseCreateEdit from "../Transaction/TDepenseCreateEdit.vue";
import TBudgetCreateEdit  from "../Transaction/TBudgetCreateEdit.vue";

export default {
  components: {
    TBudgetCreateEdit, 
    TDepenseCreateEdit,
  },
  data(){
    return{
      force: 0,
    }
  },
  methods: {
    forceRendering(){
      this.force +=1; 
    }
  } 
};
</script>


