<template>
  <div v-if="state == false">
    <main v-if="idIsUndefined == false">
      <form>
        <div>
          <label class="required">Nom</label>
          <br>
          <input class="form-control" type="text" v-model="item.itemName" required>
        </div>

        <div>
          <label>Prix</label>
          <input class="form-control" type="number" v-model="item.itemPrice">
        </div>
        <br>
        <button class="btn btn-dark" @click="onSubmit">Enregistrer</button>
      </form>
    </main>
    <main v-else>Erreur</main>
  </div>
  <div v-else>
    <loading/>
  </div>
</template>

<script>
import AuthService from "../../services/AuthService";
import {
  createItem
} from "../../api/ItemApi.js";
import Loading from "../../components/Utility/Loading.vue";
import { isNullOrUndefined } from "util";

export default {
  components: {
    Loading
  },
  data() {
    return {
      errors: [],
      item: {},
      idIsUndefined: true,
      state: true
    };
  },
  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },

  async mounted() {
    if (
      this.$currColloc.collocId != this.$currColloc.collocId.isNullOrUndefined
    )
      this.idIsUndefined = false;
    this.state = false;
  },

  methods: {
    async onSubmit() {
      event.preventDefault();

      var errors = [];

      if (!this.item.itemName) errors.push("Nom");

      if (errors.length == 0) {
        try {
          this.item.collocId = this.$currColloc.collocId;
          this.item.itemSaved = true;
          this.item.itemPrice = this.item.itemPrice * 100;
          await createItem(this.item);

          window.alert("Objet enregistré");
        } catch (e) {
          console.error(e);
        }
      } else {
        for (var j = 0; j < errors.length; j++) {
          console.log(errors[j]);
        }
      }
    }
  }
};
</script>

<style lang="scss" scoped>
.input_border {
  border-width: 1px;
}
</style>
