<template>
  <div id="app">
    <template v-if="create">
      <el-steps class="el-steps_header" space="50%" :active="1">
        <el-step title="Informations" icon="el-icon-edit"></el-step>
        <el-step title="Photo" icon="el-icon-picture"></el-step>
        <el-step class="flex_basis_0" title="Collocation"></el-step>
      </el-steps>
    </template>
    <header class="header_steps">
      <h2>Ajouter une photo</h2>
    </header>
    <br />

    <main v-if="picPath != null">
      <template v-if="!defaultPic">
        <!-- <div v-if="route == 'colloc'"> -->
        <form enctype="multipart/form-data">
          <img :src="env+'/'+this.picPath" width="200px" height="200px" />
          <br />
          <input
            type="file"
            class="btn"
            name="image"
            accept="image/x-png, image/jpg, image/jpeg"
            @change="handleImageUpload($event.target.files)"
          />
          <br />
          <br />
          <br />
          <button
            class="btn btn-dark"
            @click="submitImage()"
            :disabled="uploadButtonDisabled"
          >Mettre à jour</button>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          <button
            class="btn btn-dark align-bottom-right"
            @click="clickRoute('/roomies/profile')"
          >Retour au profil</button>
        </form>
      </template>
      <template v-else>
        <form enctype="multipart/form-data">
          <input
            type="file"
            name="image"
            accept="image/x-png, image/jpg, image/jpeg"
            @change="handleImageUpload($event.target.files)"
          />
          <br />
          <br />Image actuelle :
          <br />
          <img src="../../../public/default_profile_pic.png" width="200" height="200" />
          <br />
          <br />
          <button
            class="btn btn-dark"
            @click="submitImage()"
            :disabled="uploadButtonDisabled"
          >Importer</button>
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          <button
            v-if="create"
            class="btn btn-dark"
            @click="clickRoute('/roomies/collocation')"
          >Valider</button>
          <button
            v-else
            class="btn btn-dark align-bottom-right"
            @click="clickRoute('/roomies/profile')"
          >Retour au profil</button>
        </form>
        <!-- </div> -->
      </template>
    </main>
    <main v-else>
      <loading />
    </main>
  </div>
</template>

<script>
import axios from "axios";
import AuthService from "../../services/AuthService";
import {
  getRoomiePicAsync,
  getRoomieByIdAsync,
  getCollocPicAsync
} from "../../api/RoomiesApi";
import default_pic from "../../../public/default_profile_pic.png";
import Loading from "../../components/Utility/Loading.vue";

// document.getElementById("navMenu").style.display = "block";
export default {
  components: {
    Loading
  },
  data() {
    return {
      file: new FormData(),
      env: process.env.VUE_APP_BACKEND,
      roomieId: null,
      picPath: null,
      defaultPic: null,
      uploadButtonDisabled: true,
      create: null,
      route: null,
      mode: null,
      object: null,
      collocId: null,
      isRoomie: null,
      id: null
    };
  },

  async mounted() {
    await this.refresh();

    if (this.mode == "edit") {
      this.create = false;
    } else {
       this.create = true;
      if (this.object == "roomie") {
        this.isRoomie = true;
      } else {
        this.isRoomie = false;
        this.id = this.collocId;
      }
      try {
        if (this.object == "roomie") {
          this.picPath = await getRoomiePicAsync();
        } else {
          this.picPath = await getCollocPicAsync(this.collocId);
        }

        this.defaultPic = false;
      } catch (e) {
        console.error(e);
        if (e.message == "ERROR 404 (Not Found): Roomie has no pictures") {
          this.picPath = "../default_profile_pic.png";
          // console.log(this.env + "api/Roomies/0/default_profile_pic.png");
        }
        this.defaultPic = true;
      }
    }

    // if (this.$route.path.includes("create")) this.create = true;
    // else this.create = false;
    // console.log(this.create);
    // try {
    //   this.picPath = await getRoomiePicAsync();
    //   this.defaultPic = false;
    // } catch (e) {
    //   console.log(e);
    //   if (e.message == "ERROR 404 (Not Found): Roomie has no pictures") {
    //     this.picPath = "../default_profile_pic.png";
    //     // console.log(this.env + "api/Roomies/0/default_profile_pic.png");
    //   }
    //   this.defaultPic = true;
    // }
    // console.log(this.env+'/'+this.picPath);
  },

  methods: {
    async refresh() {
      this.mode = this.$route.params.mode;
      this.object = this.$route.params.object;
      this.roomieId = this.$route.params.id;
      
      this.collocId = this.$currColloc.collocId;
    },

    async submitImage() {
      event.preventDefault();
      const endpoint = process.env.VUE_APP_BACKEND + "/api/image";

      const file = this.file;
      file.append("roomieId", parseInt(this.roomieId));
      
      var id = this.roomieId;
      var isRoomie = this.isRoomie;
      console.log(this.isRoomie);
      console.log(this.id);
      let data = await axios.post(
        `${endpoint}/uploadImage/${id}/${isRoomie}`,
        this.file,
        {
          headers: {
            "Content-type": "multipart/form-data",
            Authorization: `Bearer ${AuthService.accessToken}`
          },
          responseType: "application/json"
        }
      );
     
        
      
      this.$router.replace("../../../roomies/collocation");
      
    },

    handleImageUpload(files) {
      console.log(files);
      this.file.append("file", files[0], files[0].name);
      console.log(this.file);
      this.uploadButtonDisabled = false;
    },
    clickRoute(pathToRoute) {
      if (this.create) {
        document.getElementById("navMenu").style.display = "block";
      }
      this.$router.push(pathToRoute);
    }
  }
};
</script>