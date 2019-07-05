<template>
<div>

 <!-- TDepense create and edit  to ROOMIE -->
  <div >
      <form @submit="onSubmit($event)">
        <div class="alert alert-alert" v-if="errors.length > 0">
          <b>Les champs suivants semblent invalides</b>
          <ul>
            <li v-for="e of errors" :key="e">{{e}}</li>
          </ul>
        </div>

        <div>
          <label for="Price">Price</label>
          <el-input-number
          controls-position="right"
            type="number"
            name="Price"
            v-model="TDepense.price"
            required>
          </el-input-number>
        </div>

        <div>
          <label for="Date">Date</label>
          <el-date-picker
            v-model="TDepense.date"
            type="date"
            placeholder="Date de la transaction"
            required
          ></el-date-picker>
        </div>

        <div>
          <label for="Rommie">Roomie</label>
          <el-select v-model="TDepense.rRoomieId" placeholder="Roomie">
            <el-option
              v-for="roomie in roomies"
              :key="roomie.roomieId"
              :label="roomie.firstName"
              :value="roomie.roomieId"
            ></el-option>
          </el-select>
        </div>
        <br><br>
        <el-button class="primary" type="primary" @click="onSubmit">Sauvegarder</el-button>
      </form>
    </div>

</div>
</template>

<script>
import {createTransacDepenseAsync, updateTransacDepenseAsync, getTransacDepenseAsync} from "../../../api/SpendingsApi/TransactionsApi/TDepenseApi";
import {GetRoomiesIdNamesByCollocIdAsync} from "../../../api/CollocationApi";
export default {
  data() {
    return {
      errors: [],
      TDepense: {},
      collocId: null,
      roomies: [],
      mode:null,
      TDepenseId: null,
    }
  },
 
  async mounted() {
    this.collocId = this.$currColloc.collocId;
    this.roomies = await GetRoomiesIdNamesByCollocIdAsync(this.collocId);
    await this.refreshList();
   
    if(this.mode == 'edit'){
  
      try{
        const tdepense = await getTransacDepenseAsync(this.TDepense);
        this.TDepense = tdepense;

        this.errors.push(this.TDepense.errorMessage);
      }catch(e){
        console.error(e);
      }finally{
        await this.refreshList();
      }
    }
    
    
  },
  methods: {
    show(){
      this.$message({
          showClose: true,
          message: 'La Transaction  a bien été ajoutée !',
          type: 'success'
        });
    },
    async refreshList(){
      this.mode = this.$route.params.mode;
      this.TDepenseId = this.$route.params.id;
    },
    async onSubmit() {
      event.preventDefault();
      if(!this.TDepense.price) this.errors.push("Price");
      if(this.TDepense.price < 0) this.errors.push("Price must be positive");
      if (!this.TDepense.date) this.errors.push("Date");
      if(!this.TDepense.rRoomieId) this.errors.push('Roomie');

      if(this.errors.length==0){
      try {
        if(this.mode == 'create' ){
          await createTransacDepenseAsync(this.TDepense);
        }
        else{
          await updateTransacDepenseAsync(this.TDepense);
        }
        
      } catch(e) {
        console.error(e);
      }
      finally {
        await this.refreshList();
        this.$root.$emit('update');
        this.TDepense = {};
        this.$router.replace("/transaction/create");
        this.show();
      }
      }
    },
  }
  
}
</script>
<style lang="scss" scoped>
label{
margin-right: 100px;
}
</style>



