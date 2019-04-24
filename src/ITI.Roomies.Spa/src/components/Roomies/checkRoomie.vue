<template>
</template>


<script>
// import { DateTime } from "luxon";
import AuthService from "../../services/AuthService";
import { state } from "../../state";
import { getRoomieByEmailAsync } from "../../api/RoomiesApi";

export default {
  data() {
    return {};
  },

  async mounted() {
    try {
      var dataToRoute = await getRoomieByEmailAsync();
      // Affiche le menu de navigation
      document.getElementById("navMenu").style.display = "block";
      this.$router.replace("/roomies/" + dataToRoute.roomieId);
    } catch (e) {
      console.error(e);
      this.$router.replace("/roomies/create");
    }
  },

  methods: {}
};
</script>

<style lang="scss">
</style>

<style lang="scss" scoped>
@import "../../styles/global.scss";
</style>