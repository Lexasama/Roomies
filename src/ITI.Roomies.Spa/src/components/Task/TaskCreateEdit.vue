<template>
  <div id="container" v-if="state == false">
    <main v-if="idIsUndefined == false">
      <header v-if="route == 'create'">
        <h2>Créer une tâche</h2>
      </header>
      <header v-if="route == 'edit'">
        <h2>Modifier la tâche</h2>
      </header>

      <form @submit="onSubmit($event)">
        <!-- <div class="alert alert-danger" v-if="errors.length > 0">
        <b>Les champs suivants semblent invalides :</b>

        <ul>
          <li v-for="e of errors">{{e}}</li>
        </ul>
        </div>-->

        <div>
          <label class="required">Nom</label> <br>
          <input class="form-control" type="text" v-model="item.TaskName" required/>
        </div>

        <div>
          <label>Description</label>
          <!-- <el-input type="textarea" v-model="item.TaskDes"/> -->
          <br>
          <textarea  class="form-control textarea_width" v-model="item.TaskDes"/>
        </div>

        <div>
          <label class="required">Echéance</label>
          <!-- <el-input type="datetime-local" id="echeance" v-model="item.TaskDate" required/> -->
          <br>
          <input  class="form-control" type="datetime-local" id="echeance" v-model="item.TaskDate" required/>
        </div>
        <br>
        <br>
        <tr v-for="roomie of roomiesList" :key="roomie.roomieId">
          <td>
            <input v-if="route == 'edit'" type="checkbox" :id="'roomie' + roomie.roomieId" :checked="roomie.checked">
            <input v-else type="checkbox" :id="'roomie' + roomie.roomieId" checked>
            <label :for="'roomie' + roomie.roomieId">{{ roomie.firstName }} {{ roomie.lastName }}</label>
          </td>
        </tr>
        <br>
        <br>
        <button class="btn btn-dark" @click="onSubmit">Sauvegarder</button>
      </form>
    </main>
    <main v-else>Erreur</main>
  </div>
  <div id="container" v-else>Chargement en cours</div>
</template>

<script>
import { DateTime } from "luxon";
import AuthService from "../../services/AuthService";
import { state } from "../../state";
import { GetRoomiesIdNamesByCollocIdAsync } from "../../api/CollocationApi.js";
import { createTaskAsync, GetTaskByTaskIdAsync, UpdateTaskAsync } from "../../api/TaskApi.js";

export default {
  data() {
    return {
      errors: [],
      item: {},
      roomiesList: [],
      checkedRoomiesList: [],
      route: null,
      idIsUndefined: true,
      state: true,
      id: null
    };
  },
  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },
  async mounted() {

    // Route
    if (this.$route.fullPath.replace("/task/", "") == "create") {
      this.route = "create";
      this.idIsUndefined = false;
    } else {
      this.route = "edit";
      if (this.$route.params.id == undefined) {
        this.idIsUndefined = true;
      } else {
        this.id = this.$route.params.id;
        try {
          var getTask = await GetTaskByTaskIdAsync(this.id);
          this.item.TaskName = getTask[0].taskName;
          this.item.TaskDes = getTask[0].taskDes;
          this.item.TaskDate = getTask[0].taskDate;
          for(var task in getTask){
            this.checkedRoomiesList.push(getTask[task].roomieId);
          }
          
          this.idIsUndefined = false;
        } catch (e) {
          console.log(e);
          this.idIsUndefined = true;
        }
      }
    }

    // Liste des roomies
    try {
      this.roomiesList = await GetRoomiesIdNamesByCollocIdAsync(
        this.$currColloc.collocId
      );
      for(var roomie in this.roomiesList){
        if(this.checkedRoomiesList.indexOf(this.roomiesList[roomie].roomieId) != -1) this.roomiesList[roomie].checked = true;
        else this.roomiesList[roomie].checked = false;
      }
    } catch (e) {
      console.log(e);
    }
    this.state = false;
  },

  methods: {
    async onSubmit() {
      event.preventDefault();

      this.item.roomiesId = [];

      for (var i = 0; i < this.roomiesList.length; i++) {
        if (
          document.getElementById("roomie" + this.roomiesList[i].roomieId)
            .checked
        )
          this.item.roomiesId.push(this.roomiesList[i].roomieId);
      }

      var errors = [];

      if (!this.item.TaskName) errors.push("Nom");
      if (!this.item.TaskDate) errors.push("Echéance");
      if (!this.item.roomiesId[0])
        errors.push("Aucun roomie selectionné pour la tâche.");

      this.errors = errors;

      if (errors.length == 0) {
        try {
          this.item.collocId = this.$currColloc.collocId;
          if(this.route == 'create') await createTaskAsync(this.item);
          else await UpdateTaskAsync(this.id, this.item);
          this.$router.replace("/task/colloc");
        } catch (e) {
          console.error(e);
        }
      } else {
        for (var j = 0; j < errors.length; j++) {
          console.log(errors[j]);
        }
      }
    }
  }
};
</script>