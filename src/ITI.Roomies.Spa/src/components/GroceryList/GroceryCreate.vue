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
  </div> -->

  <el-container v-if="state == false">
    <el-main v-if="idIsUndefined == false">
      <el-hearder v-if="route == 'create'">
        <h2>{{$t("cCourse")}}</h2>
      </el-hearder>

      <el-header v-if="route == 'edit'">
        <h2>{{$t('eCourse')}}</h2>
      </el-header>

      <el-form @submit="onSubmit($event)">

        <div>
          <label class="required">{{$t("Nom")}}</label> <br>
          <input class="input_border" type="text" v-model="course.courseName" required>
        </div>

        <div>
          <label> {{$t('Date')}} </label> <br>
          <input class="input_border" type="date" v-model="course.courseDate">
        </div>

        <el-button @click="onSubmit">{{$t('save')}}</el-button>
      </el-form>
    </el-main>
    <el-main v-else>{{$t('erreur')}}</el-main>
  </el-container>
  <el-container v-else>{{$t('loading')}}</el-container>
</template>

<script>

import {createGroceryListAsync, getGroceryListByIdAsync, updateAgroceryListAsync} from "../../api/GroceriesApi"
import AuthService from "../../services/AuthService";
import {state} from "../../state";

export default {
  data() {
    return{
      errors: [],
      course: {},
      checkedRoomiesList:[],
      route: null,
      idIsUndefined: true,
      state: true,
      id: null,
    }
  },

  computed: {
    auth: () =>AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },
  async mounted() {

      //route
    if(this.$route.fullPath.replace("/course/","") == "create") {
      this.route = "create";
      this.idIsUndefined = false;
    }else {
      this.route = "edit";

      if( this.$route.params.id == undefined) {
        this.idIsUndefined = true;

      }else{

        this.id = this.$route.params.id;

        try{
          this.course = await getGroceryListByIdAsync(this.id);
          
           
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
      event.preventDefault();

      var errors = [];

      if(!this.course.courseName) errors.push("Nom");
      if(!this.course.courseDate) errors.push("Date");

      if(errors.length == 0){
        try{

          if(this.route == "create"){
            await createGroceryListAsync(this.course);
          }
        }catch(e){
          console.error(e);
        }
      }else {
        for (var j = 0; j<errors.length; j++) {
          console.log(errors[j]);
        }
      }
    },

  },
}
</script>

<style lang="scss" scoped>
.input-border {
  border-width: 1px;
}
</style>

