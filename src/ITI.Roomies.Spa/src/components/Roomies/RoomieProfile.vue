<template class='profile'>
  <div>
    <el-container>
      <el-header>
        <h1 class="text-center">{{$t('Welcome')}} {{$t('Profil')}}</h1>
      </el-header>

      <el-main>
        <el-container>
   
          <el-row>
            <el-col>
              <div class="centerBox">
                <h2>{{$t('pic')}}</h2>
                <img class="profilePicture" :src="this.env+'/'+this.roomiePic" alt={}>
                <br>
               </div>
            </el-col>
   
            <el-col>
              <div v-if="roomie.description == null || ''">
                <tr>{{$t("nullDesc")}}</tr>
              </div>
              <div v-else>
              <tr>
                <th>Description</th>
                <td>{{roomie.description}}</td>
              </tr>
              </div>
            </el-col>
          </el-row>
   
          <el-row>
            <el-col> 

          <table>
        <div v-if="roomie == null">
          <tr>
            <h1> {{$t('erreur')}}</h1>
          </tr>
        </div>

        <div v-else>
        <br><br>
           <tr>
             <th>{{$t('Nom')}}</th>
             <td>{{roomie.lastName}}</td>
           </tr>
           <tr>
             <th>{{$t('Prenom')}}</th>
             <td>{{roomie.firstName}}</td>
           </tr>
           <tr>
             <th>{{$t('Bday')}}</th>
             <td>{{roomie.birthDate}}</td>
           </tr>
           <tr>
             <th>Email</th>
             <td>{{roomie.email}}</td>
           </tr>
           <tr>
             <th>{{$t("number")}}</th>
             <td>{{roomie.phone}}</td>
           </tr>
           </div>
           </table>
            </el-col>
          </el-row>
        </el-container>
        
        <el-container>

        </el-container>
      </el-main>
    </el-container>
    <el-container  class="Footer">
      <el-footer class="Footer">
        <Language></Language>
      </el-footer>
    </el-container>
    
  </div>
</template>

<script>

import i18n from "../../plugins/i18n"
import { getProfileAsync, getRoomiePicAsync} from "../../api/RoomiesApi"
import AuthService from "../../services/AuthService";
import Language from "../Language.vue";

export default {
  components:{
    Language,
  },

  data(){
    return {
      env : process.env.VUE_APP_BACKEND,
      roomieId: null,
      roomie: null,
      roomiePic: '',
    }
  },

  async mounted() {
    this.roomieId = this.$route.params.id;
    this.roomie =  await getProfileAsync();
    this.roomiePic =  await getRoomiePicAsync();
   
  },

  methods:{
    async editPic(){

    },
    async editProfile() {
      this.$router.replace("/roomie/");

    },

  }
}
</script>
<style scoped>

/* .centerBox {
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
} */
.Footer {
  clear: both;
  position: relative;
  height: 40px;
  margin-top: -40px
}
</style>

