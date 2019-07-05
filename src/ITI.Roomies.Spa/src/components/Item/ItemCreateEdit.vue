<template>
  <div  v-if="state == false">
    <main v-if="idIsUndefined == false">
      <header v-if="route == 'create'">
        <h2>Ajouter un Item a la liste de course {{item.courseId}}</h2>
      </header>
      <header v-if="route == 'edit'">
        <h2>Modifier l'item {{itemNameToShow}}</h2>
      </header>

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

        <div>
          <label>Nombre</label>
          <input class="form-control" type="text" v-model="item.itemQuantite">
        </div>

        <!-- /!\ TO DO : faire tune liste deffilantes avex le nom des liste/!\-->
        <!-- <div v-if="route =='edit'">
        <label class="required">Liste</label>
        <input class="input_border" type="text" v-model="item.courseId" required>
        </div>-->
        <!-- /!\ TO DO : faire une liste defilante avec le nom des roomies /!\ -->
        <!-- <div>
        <label class="required">Roomie</label>
        <input class="input_border" type="text" v-model="item.roomieId" required>
        </div>-->
        <br>
        <button class="btn btn-dark" @click="onSubmit">Sauvegarder</button>
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
  createItem,
  getItemByItemIdAsync,
  updateItemAsync
} from "../../api/ItemApi.js";
import Loading from "../../components/Utility/Loading.vue";

export default {
  components: {
    Loading
  },
  data() {
    return {
      errors: [],
      item: {},
      idIsUndefined: true,
      state: true,
      itemNameToShow: "",
      route: null,
      collocId: null,
    };
  },
  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },

  async mounted() {
    this.item.courseId = this.$route.params.id;
    this.collocId = this.$currColloc.collocId;

    if (
      this.$route.fullPath.replace(
        "/course/info/" + this.item.courseId + "/item/",
        ""
      ) == "create"
    ) {
      this.route = "create";
      this.idIsUndefined = false;
    } else {
      this.route = "edit";

      if (this.$route.params.itemId == undefined) {
        this.idIsUndefined = true;
      } else {
        try {
          this.item = (await getItemByItemIdAsync(
            this.$route.params.itemId
          )).content;
          this.itemNameToShow = this.item.itemName;
          this.idIsUndefined = false;
        } catch (e) {
          console.error(e);
          this.idIsUndefined = true;
        }
      }
    }
    this.state = false;
  },

  methods: {
    async onSubmit() {
      event.preventDefault();

      var errors = [];

      if (!this.item.itemName) errors.push("Nom");
      if (!this.item.itemPrice) errors.push("Prix");

      if (errors.length == 0) {
        try {
          this.item.collocId = this.collocId;
          if (this.route == "create") {
            this.item.collocId = this.collocId;
            await createItem(this.item);
            this.$router.push("/course/info/" + this.item.courseId);
          }
          if (this.route == "edit") {
            this.item.itemId = parseInt(this.$route.params.itemId);
            await updateItemAsync(this.item);
            this.$router.push("/course/info/" + this.item.courseId);
          }
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
