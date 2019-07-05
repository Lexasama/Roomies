<template>
  <div>
    <div>
      <h1>Liste des transactions</h1>
    </div>

    <div>
      <div>
        <el-table :data="getTBudgetPage" :page-sizes="[5, 10]">
          <el-table-column label="Description" property="description"></el-table-column>
          <el-table-column label="Price" property="price"></el-table-column>
          <el-table-column label="Date"  property="date"></el-table-column>
          <el-table-column label="RoomieId" property="roomieId"></el-table-column>
          <el-table-column label="Actions">
            <template slot-scope="scope">
              <router-link :to="`edit/${getTBudgetPage[scope.$index].tBudgetId}`">
                <el-button size="mini">Edit</el-button>
              </router-link>
              <el-button size="mini" @click="deleteTBudget(getTBudgetPage[scope.$index].tBudgetId)">Delete</el-button>
            </template>
          </el-table-column>
        </el-table>
        <el-pagination
          background
          layout="prev, pager, next"
          :total="transacBudgetList.length"
          @current-change="changePage"
        ></el-pagination>
      </div>

      <div>
        <el-table :data="getTDepensePage">
          <el-table-column label="Description" property="desc"></el-table-column>
          <el-table-column label="Prix" property="price"></el-table-column>
          <el-table-column label="Date" property="date"></el-table-column>
          <el-table-column label="Sender" property="sRoomieId"></el-table-column>
          <el-table-column label="Receiver" property="rRoomieId"></el-table-column>
          <el-table-column label="Options" property>
            <template slot-scope="scope1">
            
          <router-link :to="`edit/${getTDepensePage[scope1.$index].tDepenseId}`"><el-button size="mini" >Edit</el-button></router-link>
            <el-button size="mini" @click="deleteTDepense(getTDepensePage[scope1.$index].tDepenseId)">Delete</el-button>
            </template>
          </el-table-column>
        </el-table>
         <el-pagination
          background
          layout="prev, pager, next"
          :total="transacDepenseList.length"
          @current-change="changePage"
        ></el-pagination>
      </div>
    </div>
  </div>
</template>
<script>

import { getAllTransacBudgetAsync, getTransacBudgetAsync, deleteTransacBudgetAsync, updateTransacBudgetAsync } from "../../../api/SpendingsApi/TransactionsApi/TBudgetApi";
import { getAllTransacDepenseAsync, getTransacDepenseAsync, deleteTDepenseAsync } from "../../../api/SpendingsApi/TransactionsApi/TDepenseApi";
import { getRoomieByIdAsync, } from "../../../api/RoomiesApi";

export default {
  data() {
    return {
      transacDepenseList: [],
      errors: [],
      transacBudgetList: [],
      page: 0,
    };
  },
  computed: {
    getTDepensePage() {
      return this.transacDepenseList.slice(this.page * 10, this.page * 10 + 10);
    },
    getTBudgetPage(){
      return this.transacBudgetList.slice(this.page * 10, this.page * 10 + 10);
    }
  },
  async mounted() {
    this.$root.$on("update", async () => {
      await this.refreshList();
    });
    await this.refreshList();
  },
  methods: {
    async refreshList() {
      try {
        this.transacBudgetList = await getAllTransacBudgetAsync();
        this.transacDepenseList = await getAllTransacDepenseAsync();
      } catch (e) {
        console.error(e);
      }
    },

    changePage(newPage) {
      console.log(newPage); 
      this.page = newPage - 1;
    },

    async deleteTDepense(TDepenseId) {
      try {
        await deleteTDepenseAsync(TDepenseId);
      } catch (e) {
        console.error(e);
      } finally {
        await this.refreshList();
      }
    },
    async deleteTBudget(TBudgetId) {
      try {
        await deleteTransacBudgetAsync(TBudgetId);
      } catch (e) {
        console.error(e);
      } finally {
        await this.refreshList();
      }
    },
    async getRoomieName(roomieId){
      var roomie = await getRoomieByIdAsync(roomieId);
      return roomie.firstname;
    }
  }
};
</script>

<style lang="scss" scoped>
label{
margin-right: 100px;
}
</style>

