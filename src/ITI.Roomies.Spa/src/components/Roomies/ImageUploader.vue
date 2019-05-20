<template>
    <div>
      <div>
        <br>

        <form enctype="multipart/form-data">
            <input type="file" name="image" accept="image/x-png,image/jpg,image/jpeg" @change="handleImageUpload($event.target.files)"/>
        <el-button type="button" @click="submitImage()">Importer une/des photo(s)</el-button>
        </form>

      </div>

      <div>
        <img :src="this.env+'/'+this.picPath"  width="30em" height="30vh">

      </div>

    </div>
</template>

<script>

import axios from "axios"
import AuthService from '../../services/AuthService'
import {getRoomiePicAsync, getRoomieByIdAsync} from "../../api/RoomiesApi"



export default {
  // width em
  // height vh
    data(){
        return {

            file : new FormData(),
            env : process.env.VUE_APP_BACKEND,
            roomieId: null,
            picPath: null,            
        }
    },

    async mounted(){
      this.roomieId = this.$route.params.id;
      this.picPath =  await getRoomiePicAsync();
    },

    methods:{
        async submitImage() {
          
            const endpoint = process.env.VUE_APP_BACKEND + "/api/image";
           
            const file = this.file;
            file.append("roomieId", parseInt(this.roomieId));
            
            let data = await axios.post(`${endpoint}/uploadImage`, this.file,
            {
                headers: {
                            'Content-type' : "multipart/form-data",
                            'Authorization': `Bearer ${AuthService.accessToken}`
                        },
                        responseType: 'application/json'
            });
        },

        handleImageUpload(files){
          console.log(files)
          this.file.append("file", files[0], files[0].name);
          console.log(this.file)
        }
    },
    
}
</script>