<template>
  <div>
    <div>
      <h1>{{$t("cListe")}}</h1>
      <div>
        <router-link :to="`course/create`"> <i> {{$t("add")}} </i> </router-link>
      </div>
    </div>
    <table>
      <thead>
        <tr>
          <th>{{$t('Nom')}}</th>
          <th>{{$t('Date')}}</th>
          <th>{{$t('Price')}}</th>
          <th>{{$t('Options')}}</th>
        </tr>
      </thead>

      <tbody>
        <tr v-if="groceryList.length == 0 ">
          <td>{{$t("nullCourse")}}</td>
        </tr>

        <tr v-else v-for="g of groceryList" :key="g.courseId">
          <td> {{ g.courseName }} </td>
          <td> {{ g.courseDate}} </td>
          <td> {{g.coursePrice}} </td>
          <td>
                        <router-link :to="`course/edit/${g.courseId}`"><i>{{$t('edit')}}</i> </router-link>
                        <router-link :to="`course/info/${g.courseId}`"><i>Info</i></router-link>
                        <a href="#" @click="deleteList(g.courseId)">
                            <i class="fa fa-trash"></i>
                        </a>
                    </td>
        </tr>
      </tbody>
    </table>
    <div>
  </div>
</div>
  
</template>

<script>

import { getAllAsync, getGroceryListByIdAsync, deleteAGroceryListAsync} from "../../api/GroceriesApi";


export default {
  props: [],
  data() {
    return {
      groceryList:[],
    }
  },

  async mounted() {
    await this.refreshList();
    this.$currColloc.collocId;

  },

  methods: {
    async refreshList() {
      this.groceryList = await getAllAsync(this.$currColloc.collocId);
    },

    async deleteList(courseId) {
      await deleteAGroceryListAsync(courseId);
    },
  },
}
</script>
