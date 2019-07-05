<template class='profile'>
  <div>
    <div class="card">
      <h2 style="text-align:center">{{$t('Profil')}}</h2>
      <div v-if="roomiePic == null">
        <img :src="this.env+'/Pictures/Icons/Default.png'" alt style="width:100%" />
      </div>
      <div v-else>
        <img width="200px" height="200px" class="profilePicture" :src="this.env+'/'+this.roomiePic" />
      </div>

      <h1>{{roomie.firstName}} {{roomie.lastName}}</h1>
      <p>{{roomie.birthDate}}</p>
      <p> <a :href="'mailto:'+roomie.email">{{roomie.email}}</a></p>
      <p><a :href="'tel:'+roomie.phone">{{roomie.phone}}</a></p>
    </div>
  </div>
</template>

<script>
import i18n from "../../plugins/i18n";
import { getProfileAsync, getRoomiePicAsync } from "../../api/RoomiesApi";
import AuthService from "../../services/AuthService";
import Loading from "../../components/Utility/Loading.vue";
import monthFr from "../../components/Utility/month.js";

export default {
  components: {
    Loading
  },

  data() {
    return {
      env: process.env.VUE_APP_BACKEND,
      roomieId: null,
      roomie: null,
      roomiePic: null,
      defaultPic: null,
      state: true
    };
  },

  async mounted() {
    this.state = true;
    this.monthList = require("../../components/Utility/month.js");
    try {
      this.roomie = await getProfileAsync();
      this.roomie.birthDate = this.dateToFrDisplay(
        this.sqlToJsDate(this.roomie.birthDate)
      );
      this.roomiePic = await getRoomiePicAsync();
    } catch (e) {
      console.error(e);
    } finally {
      this.state = false;
    }
  },

  methods: {
    async editPic() {},
    async editProfile() {
      this.$router.replace("/roomie/");
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

      let monthToDisplay = this.monthList.monthFr[laDate.getMonth()];
      let yearToDisplay = laDate.getFullYear();
      return dayToDisplay + " " + monthToDisplay + " " + yearToDisplay;
    }
  }
};
</script>

<style scoped>
.card {
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  max-width: 300px;
  margin: auto;
  text-align: center;
  font-family: arial;
}
a {
  text-decoration: none;
  font-size: 22px;
  color: black;
}
a:hover {
  opacity: 0.7;
}
</style>

