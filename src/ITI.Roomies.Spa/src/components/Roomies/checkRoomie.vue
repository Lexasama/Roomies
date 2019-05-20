<template></template>


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
      // Vérifie si le roomie existe
      var dataToRoute = await FindByEmail();
      if (dataToRoute.firstName == null) {
        this.$router.replace("/roomies/create");
      } else {
        try {
          // Récupère la premère collocation du Roomie
          var collocData = await getCollocNameIdByRoomieIdAsync();
          this.$currColloc.setCollocId(collocData.collocId);
          this.$currColloc.setCollocName(collocData.collocName);
          this.$setMenuItemDisabled.setDisableState(false);
          
        } catch (e) {
          console.log(e);
        }
        finally{
          // A voir si ça reste.
        // this.$checked = true;
        // Affiche le menu de navigation
        document.getElementById("navMenu").style.display = "block";
        this.$router.replace("/roomies");
        }

      }
    } catch (e) {
      console.error(e);
      this.$router.replace("/roomies/create");
    }
  },

  methods: {}
};
</script>