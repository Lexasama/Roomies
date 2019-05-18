<template>

 <el-container v-if="state == false">
    <el-main v-if="idIsUndefined == false">
      <el-header v-if="route == 'create'">
      <h2>A{{$t('aItem')}}{{courseName}}</h2>
    </el-header>
    <el-header v-if="route == 'edit'">
      <h2>{{$t('eItem')}} {{itemName}}</h2>
    </el-header>

    <el-form @submit="onSubmit($event)">

      <div>
        <label class="required">{{$t('Nom')}}</label> <br>
        <input class="input_border" type="text" v-model="item.itemName" required>
      </div>

      <div>
        <label class="required">{{$t('price')}}</label>
        <input class="input_border" type="text" v-model="item.itemPrice" required>
      </div>

<!-- /!\ revinir dessus faire tune liste deffilantes avex le nom des liste/!\-->
      <div v-if="route =='edit'">
        <label class="required">{{$t('list')}}</label>
        <input class="input_border" type="text" v-model="item.courseId" required>
      </div>
<!-- /!\ faire une liste defilante avec le nom des roomies /!\ -->
      <div>
        <label class="required">Roomie</label>
        <input class="input_border" type="text" v-model="item.roomieId" required>
      </div>

      <el-button @click="onSubmit">{{$t('save')}}</el-button>
    </el-form>
  </el-main>
  <el-main v-else>{{$t('erreur')}}</el-main>
</el-container>
<el-container v-else>{{$t('loading')}}</el-container>
  
</template>

<script>

import AuthService from "../../services/AuthService";
import {state} from "../../state";
import {getItemListAsync, createItemAsync} from "../../api/ItemApi.js";

export default {
  // props: ,
  data() {
    return{
      errors:[],
      item: {},
      roomieList: [],
      courseList:[],
      route: null,
      idIsUndefined: true,
      state: true,
      id: null,
    }
  },
  computed:{
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },

  async mounted() {
    this.id = this.$route.params.id;

    if(this.$route.fullPath.replace("/item/","") == "create") {
      this.route = "create";
      this.idIsUndefined = false;
    }else {
      this.route = "edit";

      if( this.$route.params.id == undefined) {
        this.idIsUndefined = true;

      }else{

        this.id = this.$route.params.id;

        try{
          
          
           
        } catch(e) {
          console.log(e);
          this.idIsUndefined = true;

        }
      }
    }
    this.state = false;

  },

  methods: {
    async onSubmit(event) {
      try{
        await create
                
      }
      finally{

      }

    },

  },
}
</script>

<style lang="scss" scoped>
.input_border {
  border-width: 1px;
  }
</style>
