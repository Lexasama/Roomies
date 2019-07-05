<template>
  <div>
    <header>
      <h2>Tâches</h2>
    </header>
    
    <br />

    <button
      class="btn btn-dark"
      data-toggle="collapse"
      data-target="#historique"
      aria-expanded="false"
      aria-controls="collapseExample"
      style="
    max-width: 8rem;
"
      @click="cacheCache('nouvelleTache')"
    >Historique</button>
    <button
      class="btn btn-dark"
      data-toggle="collapse"
      data-target="#nouvelleTache"
      aria-expanded="false"
      aria-controls="collapseExample"
      style="margin-left: 4rem;"
      @click="cacheCache('historique')"
    >Nouvelle tâche</button>

    <br />
    <br />
    <main class="card mainCard" v-if="taskHistoriqueData[0]">
      <br />
      <div class="collapse" id="historique">
        <div v-if="taskHistoriqueData !='Nada'">
          <table class="tableTask">
            <tbody>
              <tr v-for="task of taskHistoriqueData" :key="task.taskId">
                <td>
                  <div class="input-group mb-1">
                    <div v-if="task.state" class="input-group-text formCheckbox formtrue"></div>
                    <div v-else class="input-group-text formCheckbox formfalse">
                      <el-tooltip content="Valider" placement="top">
                        <button class="btn btn-dark" @click="updateState(task.taskId, true)">✓</button>
                      </el-tooltip>
                    </div>
                    <label
                      v-if="task.state"
                      class="form-control formName formtrue"
                    >{{ task.taskName}}</label>
                    <label v-else class="form-control formName formfalse">{{ task.taskName}}</label>
                    <label
                      v-if="task.state"
                      class="form-control formDate formtrue"
                    >{{ task.taskDate }}</label>
                    <label v-else class="form-control formDate formfalse">{{ task.taskDate }}</label>
                    <label
                      v-if="task.state"
                      :id="'formFirstName' + task.taskId"
                      class="form-control formFirstName formtrue"
                    >{{ task.firstName}}</label>
                    <label
                      v-else
                      :id="'formFirstName' + task.taskId"
                      class="form-control formFirstName formfalse"
                    >{{ task.firstName}}</label>
                    <label
                      v-if="task.state"
                      class="form-control formDesc formtrue"
                    >{{ task.taskDes }}</label>
                    <label v-else class="form-control formDesc formfalse">{{ task.taskDes }}</label>
                  </div>
                </td>
                <td style="padding-left: 1rem;">
                  <el-tooltip content="Modifier" placement="top">
                    <button class="btn btn-dark" @click="modifierTâche(task.taskId)">⚙</button>
                  </el-tooltip>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
        <div v-else>Aucune tâche à afficher</div>
      </div>

      <div class="collapse" id="nouvelleTache">
        <main>
          <form @submit="onSubmit($event)">
            <br />
            <table class="taskCreateTable">
              <tr>
                <td>
                  <label class="required">Nom</label>
                </td>
                <td style="padding-left: 2rem;">
                  <label class="required">Echéance</label>
                </td>
              </tr>
              <tr>
                <td>
                  <input class="form-control" type="text" v-model="item.TaskName" required />
                </td>
                <td style="padding-left: 2rem;">
                  <el-date-picker
                    style="width: 10rem;"
                    v-model="item.TaskDate"
                    format="dd/MM"
                    type="datetime"
                    placeholder="Date"
                    :default-value="currentDateToMidi"
                  ></el-date-picker>

                  <el-time-picker
                    style="width: 10rem;"
                    v-model="item.TaskHour"
                    :picker-options="{
                      selectableRange: '00:00:00 - 23:55:00'
                    }"
                    format="HH:mm"
                    :default-value="currentDateToMidi"
                  ></el-time-picker>
                </td>
              </tr>
            </table>
            <br />

            <label>Description</label>
            <textarea class="form-control textarea_width" v-model="item.TaskDes" />
            <br />
            <br />
            <tr v-for="roomie of roomiesList" :key="roomie.roomieId">
              <td>
                <input type="checkbox" :id="'roomie' + roomie.roomieId" />
                &nbsp;
                {{ roomie.firstName }} {{ roomie.lastName }}
              </td>
            </tr>
            <br />
            <br />&nbsp;
            &nbsp;
            <button
              class="btn btn-dark"
              @click="onSubmit"
              style="
    max-width: 8rem;
"
            >Sauvegarder</button>
          </form>
        </main>
      </div>
    </main>
    <main v-else>
      <loading />
    </main>
    <main v-if="taskData[0]" class="card mainCard">
      <h3 style="margin: 1.5rem;">{{$currColloc.collocName}}</h3>

      <div v-if="taskData !='Nada'">
        <table class="tableTask">
          <thead>
            <th>
              <div class="input-group mb-4">
                <div class="input-group-text formCheckbox"></div>
                <label class="form-control formName">Nom</label>
                <label class="form-control formDate">Date</label>
                <label class="form-control formFirstName">Roomie</label>
                <label class="form-control formDesc">Description</label>
              </div>
            </th>
            <!-- <th>Nom</th> -->
            <!-- <th>Echéance</th> -->
            <!-- <th>Description</th> -->
            <th style="padding-left: 1rem;"></th>
          </thead>
          <tbody>
            <tr v-for="task of taskData" :key="task.taskId">
              <td>
                <div class="input-group mb-1">
                  <div class="input-group-text formCheckbox">
                    <el-tooltip content="Valider" placement="top">
                      <button class="btn btn-dark" @click="updateState(task.taskId, true)">✓</button>
                    </el-tooltip>
                  </div>
                  <label class="form-control formName">{{ task.taskName }}</label>
                  <label class="form-control formDate">{{ task.taskDate }}</label>
                  <label
                    :id="'formFirstName' + task.taskId"
                    class="form-control formFirstName"
                  >{{ task.firstName}}</label>
                  <label class="form-control formDesc">{{ task.taskDes }}</label>
                </div>
              </td>
              <td style="padding-left: 1rem;">
                <!-- <label class="form-control formBtn"> -->
                <el-tooltip content="Modifier" placement="top">
                  <button class="btn btn-dark" @click="modifierTâche(task.taskId)">⚙</button>
                </el-tooltip>&nbsp;
                <el-tooltip content="Supprimer" placement="top">
                  <button class="btn btn-dark" @click="deleteTask(task.taskId)">X</button>
                </el-tooltip>
                <!-- </label> -->
              </td>
              <!-- </div> -->
            </tr>
          </tbody>
        </table>
      </div>
      <div v-else>Aucune tâche à afficher</div>
    </main>
    <main v-else>
      <loading />
    </main>
  </div>
  <!-- </div> -->
</template>

<script>
import { DateTime } from "luxon";
import AuthService from "../../services/AuthService";
import {
  UpdateTaskStateAsync,
  getTasksByCollocIdAsync,
  DeleteTaskByIdAsync,
  createTaskAsync
} from "../../api/TaskApi.js";
import { GetRoomiesIdNamesByCollocIdAsync } from "../../api/CollocationApi.js";
import Loading from "../../components/Utility/Loading.vue";

export default {
  components: {
    Loading
  },
  data() {
    return {
      errors: [],
      taskData: [],
      taskHistoriqueData: [],
      monthList: null,
      item: {},
      roomiesList: [],
      currentDate: []
    };
  },
  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },
  async mounted() {
    this.monthList = require("../../components/Utility/month.js");
    this.refreshList();

    // Date par défaut pour le date-picker
    this.currentDateToMidi = new Date();
    this.currentDateToMidi.setHours(12);
    this.currentDateToMidi.setMinutes(30);
    this.currentDateToMidi.setSeconds(0);
    this.currentDateToMidi.setMilliseconds(0);
    // Date par défaut pour le time-picker
    // this.item.TaskHour = new Date();
    // this.item.TaskHour.setHours(12);
    // this.item.TaskHour.setMinutes(30);
    // this.item.TaskHour.setSeconds(0);
    // this.item.TaskHour.setMilliseconds(0);

    this.roomiesList = await GetRoomiesIdNamesByCollocIdAsync(
      this.$currColloc.collocId
    );
  },

  methods: {
    clickRoute(pathToRoute) {
      this.$router.push(pathToRoute);
    },

    // Change une date renvoyée par SQL en date JS
    sqlToJsDate(sqlDate) {
      sqlDate = sqlDate.replace("T", " ");

      //sqlDate in SQL DATETIME format ("yyyy-mm-dd hh:mm:ss.ms")
      var sqlDateArr1 = sqlDate.split("-");
      //format of sqlDateArr1[] = ['yyyy','mm','dd hh:mm:ms']
      var sYear = sqlDateArr1[0];
      var sMonth = (Number(sqlDateArr1[1]) - 1).toString();
      var sqlDateArr2 = sqlDateArr1[2].split(" ");
      //format of sqlDateArr2[] = ['dd', 'hh:mm:ss.ms']
      var sDay = sqlDateArr2[0];
      var sqlDateArr3 = sqlDateArr2[1].split(":");
      //format of sqlDateArr3[] = ['hh','mm','ss.ms']
      var sHour = sqlDateArr3[0];
      var sMinute = sqlDateArr3[1];
      // var sqlDateArr4 = sqlDateArr3[2].split(".");
      //format of sqlDateArr4[] = ['ss','ms']
      var sSecond = 0;
      var sMillisecond = 0;

      return new Date(
        sYear,
        sMonth,
        sDay,
        sHour,
        sMinute,
        sSecond,
        sMillisecond
      );
    },

    // Change une date JS en string français
    dateToFrDisplay(laDate) {
      let dayToDisplay =
        laDate.getDate().toString().length == 1
          ? "0" + laDate.getDate().toString()
          : laDate.getDate();
      let monthToDisplay = this.monthList.monthFr[laDate.getMonth()];
      let hourToDisplay =
        laDate.getHours().toString().length == 1
          ? "0" + laDate.getHours().toString()
          : laDate.getHours();
      let minutesToDisplay =
        laDate.getMinutes().toString().length == 1
          ? "0" + laDate.getMinutes().toString()
          : laDate.getMinutes();
      return (
        dayToDisplay +
        " " +
        monthToDisplay +
        " " +
        hourToDisplay +
        "h" +
        minutesToDisplay
      );
    },

    async updateState(taskIdToUpdate, taskNewState) {
      await UpdateTaskStateAsync(taskIdToUpdate, taskNewState);
      this.refreshList();
    },

    async deleteTask(taskId) {
      await DeleteTaskByIdAsync(taskId);
      this.refreshList();
    },

    async modifierTâche(taskId) {
      this.$router.push("/task/edit/" + taskId);
    },

    // Mise à jour des tableaux
    async refreshList() {
      try {
        this.futureTaskData = await getTasksByCollocIdAsync(
          this.$currColloc.collocId
        );
        this.taskData = [];
        this.taskHistoriqueData = [];
        if (this.taskData.length == this.futureTaskData) {
          this.taskData = "Nada";
          this.taskHistoriqueData = "Nada";
        } else {
          // TO DO : LE FAIRE EN SQL BORDEL
          // Prend la valeur de la première tâche
          var currTaskDataTaskId = this.futureTaskData[0].taskId;
          var tempArray = [];
          var tempArrayHistorique = [];
          var tempRoomieList = [];
          var currDate = new Date(new Date().getTime());
          currDate.setMilliseconds(0);
          currDate.setSeconds(0);
          currDate.setMinutes(0);
          currDate.setHours(0);

          // Pour chaque ligne
          for (var task in this.futureTaskData) {
            // Si la tâche est différente de la précédente
            if (this.futureTaskData[task].taskId != currTaskDataTaskId) {
              // Ajoute la tâche précédente à un tableau temporaire possédant toutes les tâches de la collocation
              this.futureTaskData[task - 1].firstName = tempRoomieList.join(
                ", "
              );

              // Formatage state
              this.futureTaskData[task - 1].formState =
                "form" + this.futureTaskData[task - 1].state;

              console.log(this.futureTaskData[task - 1].formState);
              // Formatage de la date au format objet Date()
              this.futureTaskData[task - 1].taskDate = this.sqlToJsDate(
                this.futureTaskData[task - 1].taskDate
              );

              // Si la tâche est validée ou que sa date est aujourd'hui ou plus tard, l'ajoute au premier tableau
              if (
                !this.futureTaskData[task - 1].state &&
                this.futureTaskData[task - 1].taskDate > currDate
              ) {
                this.futureTaskData[task - 1].taskDate = this.dateToFrDisplay(
                  this.futureTaskData[task - 1].taskDate
                );
                tempArray.push(this.futureTaskData[task - 1]);
              } else {
                this.futureTaskData[task - 1].taskDate = this.dateToFrDisplay(
                  this.futureTaskData[task - 1].taskDate
                );
                tempArrayHistorique.push(this.futureTaskData[task - 1]);
              }
              tempRoomieList = [];
              // Change la tâche précédente
              currTaskDataTaskId = this.futureTaskData[task].taskId;
            }

            tempRoomieList.push(this.futureTaskData[task].firstName);
          }

          // Formatage de la date au format objet Date()
          this.futureTaskData[task].taskDate = this.sqlToJsDate(
            this.futureTaskData[task].taskDate
          );

          this.futureTaskData[task].firstName = tempRoomieList.join(", ");

          this.futureTaskData[task].formState =
            "form" + this.futureTaskData[task].state;

          // Si la tâche est validée ou que sa date est aujourd'hui ou plus tard, l'ajoute au premier tableau
          if (
            !this.futureTaskData[task].state &&
            this.futureTaskData[task].taskDate > currDate
          ) {
            this.futureTaskData[task].taskDate = this.dateToFrDisplay(
              this.futureTaskData[task].taskDate
            );
            tempArray.push(this.futureTaskData[task]);
          } else {
            this.futureTaskData[task].taskDate = this.dateToFrDisplay(
              this.futureTaskData[task].taskDate
            );
            tempArrayHistorique.push(this.futureTaskData[task]);
          }

          if (tempArray != 0) this.taskData = tempArray;
          else this.taskData = "Nada";

          if (tempArrayHistorique.length != 0)
            this.taskHistoriqueData = tempArrayHistorique;
          else this.taskHistoriqueData = "Nada";
        }
      } catch (e) {
        console.log(e);
      }
    },

    // Supprime la classe "show" du collapse non nécessaire
    async cacheCache(classCacher) {
      document.getElementById(classCacher).classList.remove("show");
    },

    // Création de tâche
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

      console.log(typeof this.item.TaskDate);
      console.log(this.item.TaskDate);
      console.log(this.item.TaskHour);
      this.item.TaskDate = new Date(
        this.item.TaskDate.getTime() +
          this.item.TaskHour.getMinutes() * 60 * 1000 +
          this.item.TaskHour.getHours() * 60 * 60 * 1000
      );
      console.log(this.item.TaskDate < new Date().getTime());

      if (!this.item.TaskName) errors.push("Le nom est invalide");
      if (!this.item.TaskDate) errors.push("L'éhéance est invalide");
      if (this.item.TaskDate < new Date().getTime())
        errors.push("La date sélectionnée est déjà passée.");
      if (!this.item.roomiesId[0])
        errors.push("Aucun roomie selectionné pour la tâche.");

      this.errors = errors;

      if (errors.length == 0) {
        try {
          this.item.collocId = this.$currColloc.collocId;
          await createTaskAsync(this.item);
          // this.item= {};
          this.refreshList();
        } catch (e) {
          console.error(e);
        }
      } else {
        for (var j = 0; j < errors.length; j++) {
          console.log(errors[j]);
          window.alert(errors[j]);
        }
      }
    }
  }
};
</script>

<style scoped>
.input-group {
  text-align: center;
}
.formName {
  max-width: 10rem;
  width: 10rem;
  height: auto;
  /* border-top: 0px !important; */
  /* border-bottom: 0px !important; */
  border-right: 0px !important;
  /* border-left: 0px !important; */
}
.formCheckbox {
  width: 2.75rem;
  padding: 0.1rem;
  border-top-right-radius: 0px;
  border-bottom-right-radius: 0px;
  /* border-top: 0px !important; */
  /* border-bottom: 0px !important; */
  border-right: 0px !important;
  /* border-left: 0px !important; */
  border: 2px black solid;
  /* height: auto; */
}
.formDesc {
  max-width: 30rem;
  width: 30rem;
  overflow: hidden;
  word-break: break-all;
  height: auto;
  text-align: left;
  /* border-top: 0px !important; */
  /* border-bottom: 0px !important; */
  /* border-right: 0px !important; */
  border-left: 0px !important;
}
.formBtn {
  max-width: 15rem;
  width: 15rem;
  height: auto;
  border-top: 0px !important;
  border-bottom: 0px !important;
  border-right: 0px !important;
  border-left: 0px !important;
}
.formDate {
  max-width: 6rem;
  width: 6rem;
  height: auto;
  /* border-top: 0px !important; */
  /* border-bottom: 0px !important; */
  border-right: 0px !important;
  border-left: 0px !important;
}
.formFirstName {
  max-width: 10rem;
  width: 10rem;
  /* word-break: break-all; */
  height: auto;
  /* border-top: 0px !important; */
  /* border-bottom: 0px !important;  */
  border-right: 0px !important;
  border-left: 0px !important;
}

tr > td {
  padding-bottom: 1;
}

.tableTask {
  width: 100%;
}
.formfalse {
  background-color: #fd4d5f;
}

.formtrue {
  background-color: #82c560;
}
/* input[type="checkbox"] {
  transform: scale(1.5);
} */

.el-date-editor,
el-time-picker {
  width: 8rem;
}
</style>