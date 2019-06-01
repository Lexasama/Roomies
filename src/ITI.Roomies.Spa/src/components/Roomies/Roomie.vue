<template>
  <div class="container">
    <div v-if="this.collocName==''">
      <!-- <el-aside width="200px">Menu</el-aside> -->
      <header>
        <h2>Bienvenue Roomie</h2>
      </header>

      <v-calendar
        :columns="$screens({ default: 1, lg: 2 })"
        :rows="$screens({ default: 1, lg: 2 })"
        :is-expanded="$screens({ default: true, lg: false })"
      />
    </div>
    <div v-if="this.collocName!=''">
      <h1>Collocation : {{collocName}}</h1>
      <td>
        <tr>Membres</tr>
        <tr v-for="collocInfo in collocInfo">{{collocInfo.roomiesName}}</tr>
      </td>
    </div>
  </div>
</template>

<script>
import ElementUI from "element-ui";
import VueI18n from "vue-i18n";
import VCalendar from "v-calendar";
import {
  getCollocInformation,
  getCollocNameIdByRoomieIdAsync
} from "../../api/CollocationApi";

export default {
  data() {
    return {
      collocInfo: [],
      collocName: "",
      idColloc: -1
    };
  },
  async mounted() {
    var collocData = await getCollocNameIdByRoomieIdAsync();
    if (collocData != undefined) {
      this.$currColloc.setCollocId(collocData.collocId);
      this.$currColloc.setCollocName(collocData.collocName);

      this.idColloc = collocData.collocId;
      this.collocName = collocData.collocName;
      this.collocInfo = await getCollocInformation(collocData.collocId);
    }
  },
  methods: {}
};
</script>