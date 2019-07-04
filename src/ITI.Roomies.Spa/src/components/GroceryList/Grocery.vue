<template>
  <div id="app">
    <header>
      <h2>Listes de Courses</h2>
    </header>
    <main style="padding-top: 2rem;" class="card mainCard">
      <table class="tableTask">
        <tbody>
          <tr v-if="groceryList.length == 0 ">
            <td>Il n'y a pas de liste de courses</td>
          </tr>

          <tr v-else v-for="g of groceryList" :key="g.courseId">
            <td>
              <!-- <div class="input-group"> -->
              <router-link
                class="input-group"
                :to="`course/info/${g.courseId}`"
                onmouseover="style='text-decoration:none'"
              >
                <label class="form-control formName">{{ g.courseName }}</label>
                <label class="form-control formDate">{{ g.courseDate}}</label>
                <label class="form-control formPrice">{{ g.coursePrice}} €</label>
                <!-- </div> -->
              </router-link>
            </td>
            <td>
              <router-link
                :to="`course/edit/${g.courseId}`"
                onmouseover="style='text-decoration:none'"
              >
                <el-tooltip content="Modifier" placement="top">
                  <button class="btn btn-dark">⚙</button>
                </el-tooltip>
              </router-link>&nbsp;
              <button class="btn btn-dark" @click="deleteList(g.courseId)">X</button>
            </td>
          </tr>
        </tbody>
      </table>
    </main>
    <br />
    <router-link :to="`course/create`">
      <button class="btn btn-dark">Nouvelle liste</button>
    </router-link>

    <button
      class="btn btn-dark"
      data-toggle="collapse"
      data-target="#listItem"
      aria-expanded="false"
      aria-controls="collapseExample"
      style="
    max-width: 12rem;
"
      @click="refreshItemList()"
    >Objets enregistrés</button>

    <button
      class="btn btn-dark"
      data-toggle="collapse"
      data-target="#createItem"
      aria-expanded="false"
      aria-controls="collapseExample"
      style="
    max-width: 12rem;
"
    >Enregistrer un objet</button>

    <main class="card mainCard">
      <br />
      <div class="collapse" id="createItem">
        <div v-if="state == false">
          <main v-if="idIsUndefined == false">
            <form>
              <div>
                <label class="required">Nom</label>
                <br />
                <input class="form-control" type="text" v-model="item.itemName" required />
              </div>

              <div>
                <label>Prix</label>
                <input class="form-control" type="number" v-model="item.itemPrice" />
              </div>
              <br />
              <button class="btn btn-dark" @click="createSavedItem">Enregistrer</button>
            </form>
          </main>
          <main v-else>Erreur</main>
        </div>
        <div v-else>
          <loading />
        </div>
      </div>
      <br />
      <div class="collapse" id="listItem">
        <table class="table table-dark">
          <div v-if="savedItemList == 0">
            <tr>
              <td>Il n'y as pas d'objets enregistrés.</td>
            </tr>
          </div>

          <thead>
            <tr>
              <th>Name</th>
              <th>Price</th>
              <th>Options</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="i of savedItemList" :key="i.itemId">
              <td>{{i.itemName}}</td>
              <td>{{i.itemPrice / 100}} €</td>
              <td>
                <button class="btn btn-dark" @click="deleteItem(i.itemId)">Supprimer</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </main>
  </div>
</template>

<script>
import {
  getGroceryListByIdAsync,
  getAllListsAsync,
  deleteListAsync
} from "../../api/GroceriesApi";
import {
  createItem,
  getSavedItemListFromCollocAsync,
  deleteItemAsync,
  deleteSavedItemAsync
} from "../../api/ItemApi.js";
import createItemForm from "../../components/Item/createItemForm.vue";
import Loading from "../../components/Utility/Loading.vue";
// import monthFr from "../../components/Utility/month.js";

export default {
  components: {
    Loading
  },
  props: [],
  data() {
    return {
      groceryList: [],
      templateList: [],
      savedItemList: [],
      errors: [],
      item: {},
      idIsUndefined: true,
      state: true
    };
  },

  async mounted() {
    this.monthList = require("../../components/Utility/month.js");
    await this.refreshList();
    if (
      this.$currColloc.collocId != this.$currColloc.collocId.isNullOrUndefined
    )
      this.idIsUndefined = false;
    this.state = false;
  },

  methods: {
    async refreshList() {
      try {
        this.groceryList = await getAllListsAsync(this.$currColloc.collocId);
        // console.log(this.groceryList);
        if (this.groceryList.length != 0) {
          for (var grocery in this.groceryList) {
            this.groceryList[grocery].courseDate = this.dateToFrDisplay(
              this.sqlToJsDate(this.groceryList[grocery].courseDate)
            );
          }
        }
        // console.log(this.$currColloc.collocId);
        await this.refreshItemList();
        // console.log(this.savedItemList);
      } catch (e) {
        console.log(e);
      }
    },

    async refreshItemList() {
      this.state = true;
      try {
        this.savedItemList = await getSavedItemListFromCollocAsync(
          this.$currColloc.collocId
        );
      } catch (e) {
        console.log(e);
      }
      this.state = false;
    },

    async deleteList(courseId) {
      try {
        await deleteListAsync(courseId);
        await this.refreshItemList();
      } catch (e) {
        console.log(e);
      }
    },

    async deleteTemp(courseId) {
      try {
        await deleteTemplateAsync(courseId);
        await this.refreshList();
      } catch (e) {
        console.log(e);
      }
    },

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

    dateToFrDisplay(laDate) {
      let dayToDisplay =
        laDate.getDate().toString().length == 1
          ? "0" + laDate.getDate().toString()
          : laDate.getDate();
      let listeDay = [
        "Lundi",
        "Mardi",
        "Mercredi",
        "Jeudi",
        "Vendredi",
        "Samedi",
        "Dimanche"
      ];
      dayToDisplay = listeDay[laDate.getDay()] + " " + dayToDisplay;

      let monthToDisplay = this.monthList.monthFr[laDate.getMonth()];

      return dayToDisplay + " " + monthToDisplay;
    },
    async deleteItem(itemId) {
      try {
        await deleteSavedItemAsync(itemId);
        await this.refreshList();
      } catch (e) {
        console.log(e);
      } finally {
        await this.refreshList();
      }
    },
    async createSavedItem() {
      event.preventDefault();

      var errors = [];

      if (!this.item.itemName) errors.push("Nom");

      if (errors.length == 0) {
        try {
          this.item.collocId = this.$currColloc.collocId;
          this.item.itemSaved = true;
          this.item.itemPrice = this.item.itemPrice * 100;
          await createItem(this.item);

          this.item.itemPrice = null;
          this.item.itemName = null;
          // window.alert("Objet enregistré");
        } catch (e) {
          console.error(e);
        } finally {
          this.refreshItemList();
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

<style scoped>
.input-group {
  text-align: center;
  width: 34rem;
}
.formName {
  max-width: 10rem;
  width: 10rem;
  height: auto;
  border-right: 0px !important;
  /* border-left: 1px !important; */
  cursor: pointer;
}

.formBtn {
  max-width: 15rem;
  width: 15rem;
  height: auto;
  border-right: 0px !important;
  border-left: 0px !important;
}
.formDate {
  max-width: 14rem;
  width: 14rem;
  height: auto;
  border-right: 0px !important;
  border-left: 0px !important;
  cursor: pointer;
}
.formPrice {
  max-width: 10rem;
  width: 10rem;
  /* word-break: break-all; */
  height: auto;
  /* border-right: 1px !important; */
  border-left: 0px !important;
  cursor: pointer;
}

tr > td {
  padding-bottom: 1;
}

.tableTask {
  min-width: 40rem;
  max-width: 55rem;
}
.formfalse {
  background-color: #fd4d5f;
}

.formtrue {
  background-color: #82c560;
}
</style>