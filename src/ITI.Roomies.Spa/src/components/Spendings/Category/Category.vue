<template>
  <div>
 
  <div>
   <table class="category">
     <th><td align="center"></td>
     <td align="center" ></td></th>
     <tr>
       <td align="center" class="card tdcreatecategory"><CategoryCreateEdit :key="force"></CategoryCreateEdit></td>
       <td align="center" class="card tdcreatecategory1"><CategoryList></CategoryList></td>
     </tr>
   </table>
    </div>

  </div>
</template>

<script>
import CategoryList from "../../Spendings/Category/CategoryList";
import CategoryCreateEdit from "../../Spendings/Category/CategoryCreateEdit";

export default {
  components: {
    CategoryList,
    CategoryCreateEdit
  },
  data() {
    return {
      force: 0,
    };

  },
  methods: {
    forceRendering(){
    this.force +=1;
    },
  }


};
</script>


<style lang="scss" scoped>
.category{
  width: 82vw;
}
.tdcreatecategory{
  max-width: 45%;
  float: left;
}
.tdcreatecategory1{
  max-width: 45%;
 // height: 30vh;
  float: right;
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
.text {
  font-size: 14px;
}
.item {
  padding: 18px 0;
}

.box-card {
  width: 480px;
}
</style>

