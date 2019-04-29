<template>

<!-- <form  enctype="multipart/form-data" asp-controller="UploadFiles" asp-action="Index"> -->
<div>
  
    <div class="form-group">
        <div class="col-md-10">
            <p>Importer votre photo {{id}}:</p>
            <input id="uploader" type="file" name="files" multiple>

            <button @click="upload" >submit</button>
        </div>
    </div>
</div>
<!-- </form> -->

</template>



<script>
  import {uploadRoomieImageAsync} from "../api/RoomiesApi.js"
  
  export default {
    data() {
      return {
       image: null,
       id: null,
      };
    },
    mounted() {
      this.id = this.$route.params.id;
    },

    methods: {


      async upload(){
        //console.log(document.getElementById("uploader");
        this.image = document.getElementById("uploader");
        console.log(this.image);
        console.log(this.id);

         var formData = new FormData();

       if (image.files.length > 0) {
           for (var i = 0; i < image.files.length; i++) {
               formData.append('file-' + i, image.files[i]);
           }
       }
       console.log(this.formData);
        debugger;
        await uploadRoomieImageAsync(this.id,this.formData);
        
      }

    }
  }
</script>

<style scoped>
  .avatar-uploader .el-upload {
    border: 1px dashed #d9d9d9;
    border-radius: 6px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
  }
  .avatar-uploader .el-upload:hover {
    border-color: #409EFF;
  }
  .avatar-uploader-icon {
    font-size: 28px;
    color: #8c939d;
    width: 178px;
    height: 178px;
    line-height: 178px;
    text-align: center;
  }
  .avatar {
    width: 178px;
    height: 178px;
    display: block;
  }
</style>