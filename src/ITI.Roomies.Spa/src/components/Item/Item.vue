<template>
  <div id="app">
    <br>
    <table class="mainTable" vertical-align="top">
      <tr>
        <td valign="left">
          <main class="card mainCard cardCurrentItem">
            <header>
              <h2>Objet dans {{courseName}}</h2>
            </header>
            <table class="table table-dark">
              <div v-if="itemList == 0">
                <tr>
                  <td>Il n'y as pas d'article dans {{courseName}}</td>
                </tr>
              </div>

              <thead>
                <tr>
                  <th>Nom</th>
                  <th>Prix</th>
                  <th>Quantité</th>
                  <th>Roomie</th>
                  <th>Options</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="i of itemList" :key="i.itemId">
                  <td>{{i.itemName}}</td>
                  <td>{{i.itemPrice / 100}}</td>
                  <td>{{i.itemQuantite}}</td>
                  <td>{{i.firstName}}</td>
                  <td>
                    <button
                      class="btn btn-dark"
                      @click="deleteItem(i.itemId, i.courseId, i.roomieId, i.itemSaved)"
                    >X</button>
                  </td>
                </tr>
              </tbody>
            </table>
          </main>
        </td>
        <td>
          <main class="card mainCard cardItemToAdd">
            <header>
              <h3>Ajouter un objet</h3>
            </header>
            <br>
            <table class="table table-dark">
              <div v-if="savedItemList == 0">
                <tr>
                  <td>Il n'y as pas d'articles enregistrés.</td>
                </tr>
              </div>

              <thead>
                <tr>
                  <th></th>
                  <th>Nom</th>
                  <th>Prix</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="i of savedItemList" :key="i.itemId">
                  <td>
                    <button class="btn btn-dark" @click="addItemToList(i.itemId, 0, 1)">+</button>
                  </td>
                  <td>{{i.itemName}}</td>
                  <td>{{i.itemPrice / 100}} €</td>
                </tr>
              </tbody>
            </table>
          </main>
        </td>
      </tr>
    </table>
    <div>
      <form>
        <header>
          <h2>Ajouter un nouvel article</h2>
        </header>
        <div>
          <label class="required">Nom</label>
          <br>
          <input class="form-control" type="text" v-model="item.itemName" required>
        </div>

        <div>
          <label>Prix</label>
          <input class="form-control" type="number" v-model="item.itemPrice">
        </div>

        <div>
          <label>Quantité</label>
          <br>
          <input class="form-control" type="text" v-model="item.itemQuantite">
        </div>
        <br>
        <div class="form-check" v-for="roomie of roomiesList" :key="roomie.roomieId">
          <input
            v-if="roomie.roomieId != 0"
            class="form-check-input"
            type="radio"
            name="roomies"
            :id="'roomie' + roomie.roomieId"
          >
          <input
            v-else
            class="form-check-input"
            type="radio"
            name="roomies"
            :id="'roomie' + roomie.roomieId"
            checked
          >&nbsp;
          <label :for="'roomie' + roomie.roomieId">{{ roomie.firstName }} {{ roomie.lastName }}</label>
        </div>
        <br>
        <button class="btn btn-dark" @click="createItem('ajouter')">Ajouter</button> &nbsp;
        <button class="btn btn-dark" @click="createItem('autre')">Ajouter et enregistrer</button>
      </form>
    </div>
  </div>
</template>

<script>
import {
  getSavedItemListFromCollocAsync,
  deleteItemFromListAsync,
  deleteSavedItemAsync,
  getItemListAsync,
  createItemInListAsync,
  createItem
} from "../../api/ItemApi.js";
import { getGroceryListByIdAsync } from "../../api/GroceriesApi";
import { getRoomieByIdAsync } from "../../api/RoomiesApi.js";
import { GetRoomiesIdNamesByCollocIdAsync } from "../../api/CollocationApi.js";

export default {
  data() {
    return {
      itemList: [],
      savedItemList: [],
      courseName: "",
      courseId: null,
      item: {},
      roomieName: "",
      roomiesList: []
    };
  },

  async mounted() {
    this.courseId = this.$route.params.id;
    this.roomiesList = await GetRoomiesIdNamesByCollocIdAsync(
      this.$currColloc.collocId
    );
    this.roomiesList.splice(0, 0, { roomieId: 0, firstName: "Aucun" });
    console.log(this.roomiesList);
    await this.refreshList();
  },

  methods: {
    async refreshList() {
      try {
        this.savedItemList = await getSavedItemListFromCollocAsync(
          this.$currColloc.collocId
        );
        // console.log(this.savedItemList);
        this.course = await getGroceryListByIdAsync(this.courseId);
        this.courseName = this.course.courseName;

        this.itemList = await getItemListAsync(this.courseId);
        console.log(this.itemList);
      } catch (e) {
        console.log(e);
      }
    },

    async updateBought(itemIdToUpdate, newBoughtState) {
      await UpdateItemBoughtAsync(itemIdToUpdate, newBoughtState);
      this.refreshList();
    },

    async deleteItem(itemId, courseId, roomieId, itemSaved) {
      try {
        await deleteItemFromListAsync(itemId, courseId, roomieId, itemSaved);
        await this.refreshList();
      } catch (e) {
        console.log(e);
      } finally {
        await this.refreshList();
      }
    },

    async addItemToList(itemId, roomieId, itemQuantite) {
      try {
        await createItemInListAsync(
          itemId,
          this.courseId,
          roomieId,
          itemQuantite
        );
      } catch (e) {
        console.log(e);
      } finally {
        await this.refreshList();
      }
    },

    async createItem(mode) {
      event.preventDefault();

      var errors = [];

      if (!this.item.itemName) errors.push("Nom invalide");

      if (errors.length == 0) {
        try {
          this.item.collocId = this.$currColloc.collocId;
          if (mode != "ajouter") this.item.itemSaved = true;
          else this.item.itemSaved = false;

          this.item.itemPrice = this.item.itemPrice * 100;
          let newItemId = await createItem(this.item);
          // console.log(newItemId);
          // this.item.roomieId = 1;

          for (var i = 0; i < this.roomiesList.length; i++) {
            if (
              document.getElementById("roomie" + this.roomiesList[i].roomieId)
                .checked
            )
              this.item.roomieId = this.roomiesList[i].roomieId;
          }
          console.log(this.item.roomieId);
          console.log(this.item.itemQuantite);
          await this.addItemToList(
            newItemId,
            this.item.roomieId,
            this.item.itemQuantite
          );

          window.alert("Objet ajouté");

          this.item.itemName = '';
          this.item.roomieId = 0;
          this.item.itemQuantite = null;
          await this.refreshList();
        } catch (e) {
          console.error(e);
        }
      } else {
        for (var j = 0; j < errors.length; j++) {
          console.log(errors[j]);
        }
      }
    },

    clickRoute(pathToRoute) {
      this.$router.push(pathToRoute);
    }
  }
};
</script>
<style scoped>
.cardCurrentItem {
  max-width: 90%;
  min-height: 20rem;
  flex: left;
}
.cardItemToAdd {
  max-width: 90%;
  min-height: 20rem;
}

.mainTable > tr {
  vertical-align: top !important;
}
</style>