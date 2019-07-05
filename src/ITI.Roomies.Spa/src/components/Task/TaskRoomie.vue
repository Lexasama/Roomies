<template>
  <div>
    <header>
      <h2>Tâches</h2>
    </header>

    <main v-if="taskData[0]">
      <div v-if="taskData !='Nada'">
        <div v-for="taskColloc of taskData" :key="taskColloc.collocId">
          <h2>{{taskColloc.collocId}}</h2>

          <table class="table table-dark" v-if="taskData !='Nada'">
            <tr>
              <td>Nom</td>
              <td>Description</td>
              <td>Echéance</td>
              <td>Etat</td>
              <td>Participant(s)</td>
              <td></td>
            </tr>
            <tr v-for="task of taskColloc.taskArray" :key="task.taskId">
              <td>{{ task.taskName }}</td>
              <td>{{ task.taskDes }}</td>
              <td>{{ task.taskDate }}</td>
              <td v-if="!task.state">
                <button
                  class="btn btn-dark"
                  @click="updateState(task.taskId, true)"
                >{{ task.state }}</button>
              </td>
              <td v-else>
                <button
                  class="btn btn-dark"
                  @click="updateState(task.taskId, false)"
                >{{ task.state }}</button>
              </td>
              <td>{{task.firstName}}</td>
              <td>
                <button class="btn btn-dark" @click="modifierTâche(task.taskId)">Modifier</button>
              </td>
              <td>
                <button class="btn btn-dark" @click="deleteTask(task.taskId)">X</button>
              </td>
            </tr>
          </table>
        </div>
      </div>
      <div v-else>Aucune tâche à afficher</div>
    </main>
    <main v-else>
      <loading/>
    </main>

    <header>
      <h2>Historique</h2>
    </header>
    <main v-if="taskHistoriqueData[0]">
      <table class="table table-dark" v-if="taskHistoriqueData !='Nada'">
        <tr>
          <td>Nom</td>
          <td>Description</td>
          <td>Echéance</td>
          <td>Etat</td>
          <td>Participant(s)</td>
          <td></td>
        </tr>
        <tr v-for="task of taskHistoriqueData" :key="task.taskId">
          <td>{{ task.taskName }}</td>
          <td>{{ task.taskDes }}</td>
          <td>{{ task.taskDate }}</td>
          <td v-if="!task.state">
            <button class="btn btn-dark" @click="updateState(task.taskId, true)">{{ task.state }}</button>
          </td>
          <td v-else>
            <button class="btn btn-dark" @click="updateState(task.taskId, false)">{{ task.state }}</button>
          </td>
          <td>{{task.firstName}}</td>
          <td>
            <button class="btn btn-dark" @click="modifierTâche(task.taskId)">Modifier</button>
          </td>
          <td>
            <button class="btn btn-dark" @click="deleteTask(task.taskId)">X</button>
          </td>
        </tr>
      </table>
      <div v-else>Aune tâche à afficher</div>
    </main>
    <main v-else>
      <loading/>
    </main>
  </div>
</template>

<script>
import { DateTime } from "luxon";
import AuthService from "../../services/AuthService";
import {
  GetTasksByRoomieIdAsync,
  UpdateTaskStateAsync,
  DeleteTaskByIdAsync
} from "../../api/TaskApi.js";
import Loading from "../../components/Utility/Loading.vue";

export default {
  components: {
    Loading
  },
  data() {
    return {
      errors: [],
      taskData: [],
      taskHistoriqueData: []
    };
  },
  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },

  async mounted() {
    this.getTasks();
  },

  methods: {
    async updateState(taskIdToUpdate, taskNewState) {
      await UpdateTaskStateAsync(taskIdToUpdate, taskNewState);
      this.getTasks();
    },

    async deleteTask(taskId) {
      await DeleteTaskByIdAsync(taskId);
      this.getTasks();
    },

    async modifierTâche(taskId) {
      this.$router.push("/task/edit/" + taskId);
    },

    async getTasks() {
      try {
        this.futureTaskData = await GetTasksByRoomieIdAsync();
        this.taskData = [];
        this.taskHistoriqueData = [];

        if (this.taskData.length == this.futureTaskData) {
          this.taskData = "Nada";
          this.taskHistoriqueData = "Nada";
        } else {
          // TO DO : LE FAIRE EN SQL BORDEL
          // Prend la valeur de la première colloc
          var currTaskDataColloc = this.futureTaskData[0].collocId;
          // Prend la valeur de la première tâche
          var currTaskDataTaskId = this.futureTaskData[0].taskId;
          var tempArray = [];
          var tempArray2 = [];
          var tempRoomieList = [];
          // Pour chaque ligne
          for (var task in this.futureTaskData) {
            // Si la tâche est différente de la précédente
            if (this.futureTaskData[task].taskId != currTaskDataTaskId) {
              // Ajoute la tâche précédente à un tableau temporaire possédant toutes les tâches de la collocation
              this.futureTaskData[task - 1].firstName = tempRoomieList.join(
                ", "
              );
              // tempArray.push(this.futureTaskData[task-1]);
              if (!this.futureTaskData[task - 1].state)
                tempArray.push(this.futureTaskData[task - 1]);
              else tempArray2.push(this.futureTaskData[task - 1]);

              tempRoomieList = [];
              // Change la tâche précédente
              currTaskDataTaskId = this.futureTaskData[task].taskId;
            }

            tempRoomieList.push(this.futureTaskData[task].firstName);

            // Si la tâche a le même collocId que la tâche précédente
            if (this.futureTaskData[task].collocId != currTaskDataColloc) {
              // Ajoute un objet avec l'id de la collocatione et ses tâches au tableau qui sera affiché
              this.taskData.push({
                collocId: currTaskDataColloc,
                taskArray: tempArray
              });
              tempArray = [];
              // Change la collocation précédente
              currTaskDataColloc = this.futureTaskData[task].collocId;
            }
          }
          this.futureTaskData[task].firstName = tempRoomieList.join(", ");

          if (!this.futureTaskData[task].state)
            tempArray.push(this.futureTaskData[task]);
          else tempArray2.push(this.futureTaskData[task]);

          // Ajoute la dernière collocation
          if (tempArray != 0)
            this.taskData.push({
              collocId: this.futureTaskData[task].collocId,
              taskArray: tempArray
            });
          else this.taskData = "Nada";

          if (tempArray2.length != 0) this.taskHistoriqueData = tempArray2;
          else this.taskHistoriqueData = "Nada";
        }
      } catch (e) {
        console.log(e);
      }
    }
  }
};
</script>