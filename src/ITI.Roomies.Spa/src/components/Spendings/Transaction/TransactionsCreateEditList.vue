<template>
  <div>
    <el-row :gutter="20">
      <el-col :xs="4" :sm="6" :md="8" :lg="9" :xl="11">
        <el-card>
          <div>
            <TransactionCreateEdit></TransactionCreateEdit>
          </div>
        </el-card>
      </el-col>

      <el-col :xs="4" :sm="6" :md="8" :lg="9" :xl="11">
        <el-card>
          <div>
            <TransactionList></TransactionList>
          </div>
        </el-card>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import AuthService from "../../../services/AuthService";
import Loading from "../../../components/Utility/Loading.vue";
import TransactionList from "../../Spendings/Transaction/TransactionList";
import TransactionCreateEdit from "../../Spendings/Transaction/TransactionsCreateEdit";
export default {
  components: {
    TransactionList,
    TransactionCreateEdit
  },

  data() {
    return {};
  },

  computed: {
    auth: () => AuthService,

    isLoading() {
      return this.state.isLoading;
    }
  }
};
</script>


<style lang="scss" scoped>
.el-col {
  border-radius: 4px;
}
.bg-purple-dark {
  background: #99a9bf;
}
.bg-purple {
  background: #d3dce6;
}
.bg-purple-light {
  background: #e5e9f2;
}
.grid-content {
  border-radius: 4px;
  min-height: 36px;
}
.text {
  font-size: 14px;
}

.item {
  padding: 18px 0;
}

.box-card {
  width: 480px;
}
</style>
