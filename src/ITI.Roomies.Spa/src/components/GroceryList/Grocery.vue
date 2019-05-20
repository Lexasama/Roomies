<template>
  <div id="container">
    <header>
      <h2>Liste de Courses</h2>
    </header>
    <br>
    <table class="table table-dark">
      <thead>
        <tr>
          <th>Name</th>
          <th>Date</th>
          <th>Price</th>
          <th>Options</th>
          <th></th>
        </tr>
      </thead>

      <tbody>
        <tr v-if="groceryList.length == 0 ">
          <td>Il n'y a pas de liste de courses</td>
        </tr>

        <tr v-else v-for="g of groceryList" :key="g.courseId">
          <td>{{ g.courseName }}</td>
          <td>{{ g.courseDate}}</td>
          <td>{{g.coursePrice}}</td>
          <td>
            <router-link :to="`course/info/${g.courseId}`">
              <button class="btn btn-dark">info</button>
            </router-link>
          </td>
          <td>
            <router-link :to="`course/edit/${g.courseId}`">
              <button class="btn btn-dark">edit</button>
            </router-link>
            <button class="btn btn-dark" @click="deleteList(g.courseId)"> Supprimer </button>
            
          </td>
        </tr>
      </tbody>
    </table>
    <br>
    <router-link :to="`course/create`">
      <button class="btn btn-dark">Nouvelle liste</button>
    </router-link>
  </div>
</template>

<script>
import {
  getAllAsync,
  getGroceryListByIdAsync,
  deleteAGroceryListAsync
} from "../../api/GroceriesApi";

export default {
  props: [],
  data() {
    return {
      groceryList: []
    };
  },

  async mounted() {
    await this.refreshList();
  },

  methods: {
    async refreshList() {
      try {
        this.groceryList = await getAllAsync(this.$currColloc.collocId);
      } catch (e) {
        console.log(e);
      }
    },

    async deleteList(courseId) {
      try {
        await deleteAGroceryListAsync(courseId);
        await this.refreshList();
      } catch (e) {
        console.log(e);
      }
    }
  }
};
</script>
