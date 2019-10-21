import "./main.auth";
import "./main.vendors";
import Vue from "vue";
import App from "./App.vue";
import store from "./store";
import router from "./main.router";
import AuthService from "./services/AuthService";
import i18n from "./plugins/i18n";

Vue.config.productionTip = false;

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount("#app");
