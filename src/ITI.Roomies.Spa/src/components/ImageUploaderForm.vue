<template>

<div>

 <form enctype="multipart/form-data"  id="uploader" name="fileinfo">
  <label>Import your image</label>
  <input type="file" name="file"  />
  <button   @click="upload">Upload</button>
</form>

</div>


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

        this.image = document.getElementById("uploader");
        console.log(this.image);
        console.log(this.id);
        
         //var formData = new FormData(this.image);
        var formData = new FormData();
        console.log(formData);
        
      //  if (this.image.files.length > 0) {
         
      //      for (var i = 0; i < this.image.files.length; i++) {
      //          formData.append('file-' + i, this.image.files[i]);
      //      }
      //  }
       console.log(formData);

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