<template>
  <div>
<!-- profile picture will be displayed here -->
    <div class="centerBox">
        <img class="profilePicture" :src="this.env+'/'+this.roomiePic">
    </div>
    <!-- roomie information here -->
    <div></div>
  
  </div>
</template>

<script>

import { getProfileAsync, getRoomiePicAsync} from "../../api/RoomiesApi"
import AuthService from "../../services/AuthService";

export default {

  data(){
    return {
      env : process.env.VUE_APP_BACKEND,
      roomieId: null,
      roomie: { },
      roomiePic: '',
    }
  },

  async mounted() {
    this.roomieId = this.$route.params.id;
    this.roomie =  getProfileAsync();
    this.roomiePic =  await getRoomiePicAsync();
  },

  methods:{
    


  }
}
</script>
<style scoped>

.centerBox {
  margin: auto;
  border-radius: 15px;
  background-color: aliceblue;
  text-align: center;
}
.profilePicture  {
  vertical-align: middle;
  width: 50px;
  height: 50px;
  border-radius: 50%;
 
  border-radius: 50%;
}
</style>

