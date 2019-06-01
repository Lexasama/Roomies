<template>
    <div>
        <notification :hidden="true" ref="notification"></notification>
 
<br>
        <Erreur :errors="errors"/>
        <form enctype="multipart/form-data">
            <input type="file" name="file" accept="image/x-png,image/jpg,image/jpeg" @change="handleFileUpload($event.target.files)" multiple/>
        <el-button type="button" @click="submitFile()">Importer une/des photo(s)</el-button>
        </form>
        <div v-if="allPictures != null">
            <div v-if="allPictures.length >0">
        <el-button icon="el-icon-download" @click="DownloadAllFiles()">Telecharger toutes les photos</el-button>
            </div>
        </div>

    <div >

        <div class="card--in">
        <el-row>
            <el-col :span="8" v-for="(o, index) in allPictures" :key="o" :offset="index > 0 ? allPictures : 0" :body-style="{ padding: '0px' }">
                
                <!-- <el-card :body-style="{ padding: '0px' }"> -->
                    <div class="img-marg">
                        <img :src="env + o" class="imagePhoto">
<!-- </el-card> --></div>
            </el-col>
        </el-row>0
        </div>
    </div>
    </div>
</template>

<script>
import { GetAllPictures } from '../api/photoApi.js'
import axios from "axios"
import Erreur from './Erreur.vue';
import { serverBus } from '../eventbus.js'
import Notification from './Notification.vue'


export default {
     components:{
        Erreur,
        Notification
    },
data() {
        return {
            files: new FormData(),
            file : null,
            model : {},
            idGroup :  this.$route.params.groupId,
            allPictures : null,
            env : process.env.VUE_APP_BACKEND,
            fileListToServer : [],
            sizeMax : 3000000,
            errors : [],
            connection : null,
            connectionNotifyHub : null
        }
    },

    async created(){
         await serverBus.$on('ConnectionHubSurvey', (connection) => {
            this.connection = connection;
                this.connection.on("AddPhoto", () => {
                   this.GetPictures();
                });
        }); 

        await  serverBus.$on('ConnectionNotifyHub', (connectionNotify) => {
                this.connectionNotifyHub = connectionNotify;

         });
        this.allPictures = await GetAllPictures(this.idGroup);
    },

    methods:{
        async submitFile(){
            const files = this.files;
            this.errors = [];
                let CountFiles = Array.from(this.files).length;
            if(CountFiles >0){
                files.append("idGroup",parseInt(this.idGroup));
                const endpoint = process.env.VUE_APP_BACKEND + "/api/photo"
            let data = await axios.post(`${endpoint}/uploadImage`, files,
                    {
                        headers: {
                            'Content-type' : "multipart/form-data",
                        },
                        responseType: 'application/json'

                    });

                let array = data.data;

                if(array == ""){
                    this.$notify({
                            title: "Les Photos ont bien été upload !",
                            type: 'success'
                    });

                    await this.connection.invoke("AddPhoto", this.idGroup);
                    await this.connectionNotifyHub.invoke("NewNotify",this.idGroup,"Photos");

                    let notif = 'a ajouté de nouvelle photos';
                    this.$refs.notification.AddNotify(notif,"Photos","/groups/"+this.idGroup);

                }else if(array.length == CountFiles){

                    this.$notify.error({
                            title: "Aucune photos n'a été upload",
                    });
                    
                    for (let index = 0; index < array.length; index++) {
                         this.errors.push(array[index])
                    }

                }else if(array.length != CountFiles){

                     this.$notify({
                            title: "Photos",
                            message : "L'upload s'est bien passé mais certains fichiers n'ont pas été upload",
                            type: 'warning'
                    });
                    await this.connection.invoke("AddPhoto", this.idGroup);
                    await this.connectionNotifyHub.invoke("NewNotify",this.idGroup,"Photos");

                    let notif = 'a ajouté de nouvelle photos';
                    this.$refs.notification.AddNotify(notif,"Photos","IDK");


                    for (let index = 0; index < array.length; index++) {
                         this.errors.push(array[index])
                    }

                }
                
            }else{
                this.errors.push("Il n'y a pas de fichier ! ")
            }
        },

        async GetPictures(){
           this.allPictures = await GetAllPictures(this.idGroup);
        },

        handleFileUpload(fileList){

            let fileListError =[];
            this.files = new FormData();

           for (let index = 0; index < fileList.length; index++) {
                if(fileList[index].size < this.sizeMax){
                    this.files.append("file", fileList[index], fileList[index].name);
                }else{
                    fileListError.push(fileList[index].name + " est trop volumineux et ne sera pas upload")
                }
           }
           if(fileListError.length >0) this.errors = fileListError;

        },
   
        async DownloadAllFiles(){
          const endpoint = process.env.VUE_APP_BACKEND + "/api/photo"
            try{
               var response =  await axios.get(`${endpoint}/DownloadAllFiles?idGroup=${this.idGroup}`, 
                {
                    headers: {
                    },
                    responseType: 'arraybuffer'
                    
                });
                    //First Method 
                    let blob = new Blob([response.data], { type: 'application/zip' })
                    let link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = 'Photos.zip';
                    link.click();
                    
                    //Second Method
                // .then(function(response) {
                //     console.log(response);
                //     const url = window.URL.createObjectURL(new Blob([response.data]));
                //     const link = document.createElement('a');
                //     link.href = url;
                //     link.setAttribute('download','Photos.zip'); 
                //     document.body.appendChild(link);
                //     link.click();
                // });
            }catch(e){
                 this.errors.push(e);
                 window.console.error(e)
            }

            
        }

    }
}
</script>

<style>
.image_photo {
    width: auto;
    height: 500px;
    display: block;
    margin-left: auto;
    margin-right: auto;
}

.el-carousel__item h3 {
    background :white;
  }



 
  
  /* .bottom {
    margin-top: 13px;
    line-height: 12px;
  } */

  .button {
    padding: 0;
    float: right;
  }

  .imagePhoto {
    width: 600px;
    height: 600px;
    margin-left: 3%;
    padding: 13%;
    
  }

  /* .clearfix:before,
  .clearfix:after {
      display: table;
      content: "";
  } */
  
  .clearfix:after {
      clear: both
  }

  .card-block{
  }

  .card--in{
    display: inline-block;
  }

  .img-marg{
  }

</style>
