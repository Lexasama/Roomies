<template>
  <div class="createContainer">
    <div>
      <h1>Transaction</h1>
    </div>

    <div>
      <h2>Transaction table</h2>
    </div>
    <div>
      <table class="table table-dark">
        <thead>
          <tr>
            <th>Description</th>
            <th>Price</th>
            <th>Date</th>
            <th>BudgetId</th>
            <th>RoomieId</th>
          </tr>
        </thead>

        <tbody>
          <tr v-if="transacBudgetList.length == 0">
            <td>Il n'a y aucune transaction</td>
          </tr>

          <tr v-else v-for="t in transacBudgetList" :key="t.transacBudgetId">
            <td>{{t.description}}</td>
            <td>{{t.price}}</td>
            <td>{{t.date}}</td>
            <td>{{t.budgetId}}</td>
            <td>{{t.roomieId}}</td>
          </tr>
        </tbody>
      </table>

      <div>
        <table class="table table-dark">
          <thead>
            <tr>
              <th>Description</th>
              <th>Price</th>
              <th>Date</th>
              <th>from</th>
              <th>to</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="transacDepenseList.length == 0">
              <td>Il n'a pas de depenses</td>
            </tr>
            <tr v-else v-for="t in transacDepenseList" :key="t.transacBudgetId">
              <td>{{t.desc}}</td>
              <td>{{t.price}}</td>
              <td>{{t.date}}</td>
              <td>{{t.sRoomieId}}</td>
              <td>{{t.rRoomieId}}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div>
      <h2>Ajouter une transaction</h2>
    </div>
    <div>
      <form @submit="onSubmit($event)">
        <div class="alert alert-alert" v-if="errors.length > 0">
          <b>Les champs suivants semblent invalides</b>
          <ul>
            <li v-for="e of errors" :key="e">{{e}}</li>
          </ul>
        </div>

        <div>
          <label for="Price">Amount</label>
          <br>
          <input type="number" name="Price" v-model="transaction.price" required>
        </div>

        <div>
          <label for="Date">Date</label>
          <br>
          <input type="Date" name="Date" v-model="transaction.date" required>
        </div>

        <div>
          <label>Chosir la category</label>
        </div>
        <div>
          <el-select v-model="transaction.categoryId" clearable placeholder="Categories">
            <el-option
              v-for="category in categories"
              :key="category.categoryId"
              :label="category.categoryName"
              :value="category.categoryId"
            ></el-option>
          </el-select>
          <el-select v-model="transaction.roomieId" placeholder="Roomie">
            <el-option
              v-for="roomie in roomies"
              :key="roomie.roomieId"
              :label="roomie.firstName"
              :value="roomie.roomieId"
            ></el-option>
          </el-select>
        </div>

        <el-button type="primary" round @click="onSubmit">Sauvegarder</el-button>
      </form>
    </div>
  </div>
</template>

<script>
import {
  getAllTransacBudgetAsync,
  getTransacBudgetAsync,
  createTransactionAsync,
  deleteTransacBudgetAsync,
  updateTransacBudgetAsync,
  getAllTransacDepenseAsync
} from "../../../api/SpendingsApi/TransactionApi";
import { GetRoomiesIdNamesByCollocIdAsync } from "../../../api/CollocationApi";
import { getAllBudgetAsync } from "../../../api/SpendingsApi/BudgetApi";
import { getCategoriesAsync } from "../../../api/SpendingsApi/CategoryApi";
import AuthService from "../../../services/AuthService";
import { state } from "../../../state";
import Loading from "../../../components/Utility/Loading.vue";

export default {
  data() {
    return {
      errors: [],
      roomies: [],
      budgets: [],
      transacBudgetList: [],
      transacDepenseList: [],
      categories: [],
      strate: true,
      idIsUndefined: true,
      collocId: null,
      transaction: {}
    };
  },

  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  },

  async mounted() {
    await this.refreshList();
  },

  methods: {
    async refreshList() {
      try {
        this.collocId = this.$currColloc.collocId;
        this.transacBudgetList = await getAllTransacBudgetAsync();
        this.roomies = await GetRoomiesIdNamesByCollocIdAsync(this.collocId);
        this.budgets = await getAllBudgetAsync(this.collocId);
        this.categories = await getCategoriesAsync(this.collocId);
        this.transacDepenseList = await getAllTransacDepenseAsync();
        debugger;
      } catch (e) {
        console.log(e);
      }
    },

    async onSubmit() {
      event.preventDefault();

      this.transaction.collocId = this.$currColloc.collocId;

      if (!this.transaction.price) this.errors.push("Price");
      if (!this.transaction.date) this.errors.push("Date");
      if (!this.transaction.collocId) this.errors.push("CollocId");
      if (this.transaction.roomieId && this.transaction.categoryId)
        this.errors.push("can't have category and a Roomie");
      if (
        this.transaction.roomieId != null &&
        this.transaction.categoryId != null
      )
        this.errors.push("can't have category and a Roomie");

      debugger;
      await createTransactionAsync(this.transaction);
    }
  }
};
</script>
