<template>
</template>


<script>
import AuthService from "../../services/AuthService";
import { state } from "../../state";
import { FindByEmail } from "../../api/RoomiesApi";
import { getCollocNameIdByRoomieIdAsync } from "../../api/CollocationApi";

export default {
  data() {
    return {};
  },

  async mounted() {
    try {
      var dataToRoute = await FindByEmail();
      try {
        // Récupère la premère collocation du Roomie
        var collocData = await getCollocNameIdByRoomieIdAsync();
        this.$currColloc.setCollocId(collocData.collocId);
        this.$currColloc.setCollocName(collocData.collocName);
      } catch (e) {
        console.log(e);
      }
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
@import "../../styles/global.scss";
</style>