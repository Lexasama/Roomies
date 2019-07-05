<template>
  <div>
    <header>
      <h2>{{$t('colloc')}}</h2>
    </header>
    <span v-if="$setMenuItemDisabled.disableState">
      <p>
        {{$t('collocCreate')}}
        <br />
        <br />
        <button
          class="btn btn-dark"
          @click="changeCreate()"
          :disabled="!$setMenuItemDisabled.disableState"
        >{{$t('create')}}</button>
      </p>
      <br />
      <p>
        {{$t('collocInvite')}}
        <br />
        <br />
        <button
          class="btn btn-dark"
          @click="changeJoin()"
          :disabled="!$setMenuItemDisabled.disableState"
        >{{$t('join')}}</button>
      </p>
    </span>
    <span v-else>
      <p>
        {{$t('collocInvite2')}}
        <br />
        <br />
        <button
          class="btn btn-dark"
          @click="changeInvite()"
          :disabled="$setMenuItemDisabled.disableState"
        >Inviter</button>
      </p>
    </span>
    <div v-if="show1">
      <form @submit="onSubmit($event)">
        <div class="alert alert-danger" v-if="errors.length > 0">
          <b>Les champs suivants semblent invalides :</b>

          <ul>
            <li v-for="e of errors">{{e}}</li>
          </ul>
        </div>

        <div class="form-group">
          <label class="required">Nom de collocation</label>
          <input class="form-control" type="text" v-model="item.CollocName" required />
        </div>

        <br />
        <button class="btn btn-dark" native-type="submit" v-if="this.collocName==''">Sauvegarder</button>
        <p v-if="this.collocName!='' ">Vous avez déjà une collocation.</p>
      </form>
    </div>
    <div v-if="show3">
      <form @submit="onSubmitJoin($event)">
        <div class="form-group">
          <label class="required">Code :</label>
          <input class="form-control" type="text" v-model="item.InviteKey" required />
        </div>

        <br />
        <button class="btn btn-dark" native-type="submit" v-if="this.collocName==''">Rejoindre</button>
        <p v-if="this.collocName!=''">Vous avez déjà une collocation.</p>
        <p v-if="this.checkjoin==0">Le code que vous avez rentré n'est pas valide.</p>
      </form>
    </div>

    <div v-if="show2">
      <form @submit="onSubmitInvite($event)">
        <div class="alert alert-danger" v-if="errors.length > 0">
          <b>Les champs suivants semblent invalides :</b>

          <ul>
            <li v-for="e of errors">{{e}}</li>
          </ul>
        </div>

        <div class="form-group">
          <label class="required">Mail</label>
          <input class="form-control" type="text" v-model="item.mail" required />
        </div>

        <button class="btn btn-dark" native-type="submit" v-if="this.collocName!=''">Envoyer</button>
        <p
          v-if="this.collocName==''"
        >Veuillez d'abord créer une collocation avant d'inviter des personnes.</p>
        <p v-if="this.checkInvite==0">Le mail que vous avez rentré ne correspond à aucun roomie.</p>
        <p v-if="this.checkInvite==1">L'invitation a été envoyée</p>
      </form>
    </div>

    <div v-if="this.collocName!='' && show4">
      <span v-if="!$setMenuItemDisabled.disableState">
        <br />
        <br />
        <br />
        <button
          class="btn btn-dark"
          @click="onSubmitQuit($event)"
          native-type="submit"
        >Quitter la collocation</button>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <template
          v-if="Admin==1 && !$setMenuItemDisabled.disableState"
        >
          <button class="btn btn-dark" @click="DestroyColloc()">Suppprimer la collocation</button>
        </template>
      </span>
    </div>
  </div>
</template>


<script>
import {
  createCollocAsync,
  quitCollocAsync,
  InviteAsync,
  JoinAsync,
  DestroyCollocAsync,
  IsAdminAsync
} from "../api/CollocationApi";
import { error } from "util";
export default {
  data() {
    return {
      item: {},
      errors: [],
      show1: false,
      show2: false,
      show3: false,
      show4: false,
      collocName: "",
      collocid: "",
      mail: "",
      InviteKey: "",
      checkInvite: 2,
      checkjoin: 2,
      Admin: 0
    };
  },

  async mounted() {
    this.idColloc = this.$currColloc.collocId;
    this.collocName = this.$currColloc.collocName;
    this.show4 = !this.$setMenuItemDisabled.disableState;
    if (this.$currColloc.collocName != "") {
      this.Admin = await IsAdminAsync(this.$currColloc.collocId);
    }
  },

  methods: {
    changeCreate() {
      this.show1 = true;
      this.show2 = false;
      this.show3 = false;
      // this.show4 = true;
      this.checkInvite = 2;
    },
    changeInvite() {
      this.show1 = false;
      this.show2 = true;
      this.show3 = false;
      // this.show4 = true;
    },
    changeJoin() {
      this.show1 = false;
      this.show2 = false;
      this.show3 = true;
      // this.show4 = true;
      this.checkInvite = 2;
    },

    async onSubmit(event) {
      event.preventDefault();

      var errors = [];

      var idColloc = null;
      if (!this.item.CollocName) errors.push("CollocName");

      this.errors = errors;
      if (errors.length == 0) {
        try {
          idColloc = await createCollocAsync(this.item);

          // Ajout à la colloc en utilisation
          this.$currColloc.setCollocId(idColloc);
          this.$currColloc.setCollocName(this.item.CollocName);
          // Active les boutons du menu
          this.$setMenuItemDisabled.setDisableState(false);

          document.getElementById("navMenu").style.display = "block";
          this.$router.push("/roomies");
        } catch (e) {
          console.error(e);
        }
      }
    },

    async onSubmitQuit(event) {
      try {
        await quitCollocAsync(this.idColloc);
        this.$currColloc.setCollocId(-1);
        this.$currColloc.setCollocName("");
        // Désactive les boutons du menu
        this.$setMenuItemDisabled.setDisableState(true);
        this.$router.replace("/roomies");
      } catch (e) {
        console.error(e);
      }
    },

    async onSubmitInvite(event) {
      event.preventDefault();

      var errors = [];
      if (!this.item.mail) error.push("mail");
      if (errors.length == 0) {
        try {
          this.checkInvite = await InviteAsync(this.item.mail, this.idColloc);
        } catch (e) {
          console.error(e);
        }
      }
    },

    async onSubmitJoin(event) {
      event.preventDefault();
      if (!this.item.InviteKey) error.push("InviteKey");
      try {
        this.checkJoin = await JoinAsync(this.item.InviteKey);
        if (this.checkJoin == 1) {
          this.$setMenuItemDisabled.setDisableState(false);
          this.$router.replace("/roomies");
        }
      } catch (e) {
        console.error(e);
      }
    },

    async DestroyColloc() {
      try {
        DestroyCollocAsync(this.$currColloc.collocId);
        this.$currColloc.setCollocId(-1);
        this.$currColloc.setCollocName("");
        // Désactive les boutons du menu
        this.$setMenuItemDisabled.setDisableState(true);
        this.$router.replace("/roomies");
      } catch (e) {
        console.log(e);
      }
    }
  }
};
</script>

<style scoped>
.form-control {
  max-width: 30rem;
}
</style>
