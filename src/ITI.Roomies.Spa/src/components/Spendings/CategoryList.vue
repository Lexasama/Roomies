<template>
  <div>
    <div>
      <div>
        <span>Category</span>
      </div>

      <table>
        <div v-if="categoriesList == 0">
          <tr>
            <td>Il n'y as pas category dans la liste</td>
          </tr>
        </div>

        <thead>
          <tr>
            <th>Icon</th>
            <th>Name</th>
            <th>Options</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="category in categoriesList" :key="category.categoryId" class="text item">
            <td>
              <img :src="iconPath + category.iconName +'.png'" height="50" width="50">
              </td>
            <td>{{category.categoryName}}</td>
            <td>
              <button
                class="btn btn-dark"
                @click="clickRoute('/category/edit/'+ category.categoryId)"
              >edit</button>
            </td>
            <td>
              <button @click="deleteCategory(category.categoryId)">Supprimer</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import {
  getCategoriesAsync,
  deleteCategoryAsync
} from "../../api/SpendingsApi/CategoryApi";

export default {
  data() {
    return {
      errors: [],
      categoriesList: [],
      iconPath: 'http://localhost:5000/Pictures/Icons/',
      
    };
  },

  async mounted() {
    this.collocId = this.$currColloc.collocId;
    await this.refreshList();
  },

  methods: {
    async refreshList() {
      try {
        this.categoriesList = await getCategoriesAsync(this.collocId);
        console.log(categoriesList);
      } catch (e) {
        console.log(e);
      }
    },
    async deleteCategory(categoryId) {
      try {
        await deleteCategoryAsync(categoryId);
        await this.refreshList();
      } catch (e) {
        console.log(e);
      } finally {
        await this.refreshList();
      }
    }
  }
};
</script>
<style lang="scss" scoped>


</style>

