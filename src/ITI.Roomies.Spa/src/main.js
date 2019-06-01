import './main.auth'
import './main.vendors'
import AuthService from './services/AuthService'
import Vue from 'vue'
import App from './components/App.vue'
import router from './main.router'
import i18n from './plugins/i18n'
import ElementUI from 'element-ui';
import { Button, Select } from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import locale from 'element-ui/lib/locale/lang/en'
import VCalendar from 'v-calendar';
import VueGlobalVariable from 'vue-global-var';
import 'bootstrap/dist/css/bootstrap.css';
import VueCookies from 'vue-cookies'
Vue.config.productionTip = false;

Vue.component(Button.name, Button);
Vue.component(Select.name, Select);
Vue.use(VueCookies)
Vue.use(ElementUI, {locale});
Vue.use(VCalendar);

class currentColloc {

  constructor(collocId, collocName) {
    this.collocId = collocId;
    this.collocName = collocName;
  }

  setCollocId(newId){
    this.collocId = newId;
  }

  setCollocName(newName){
    this.collocName = newName;
  }
}

class MenuItem{
  constructor(stateMenuItem){
    this.disableState = stateMenuItem
  }

  setDisableState(newState){
    this.disableState = newState
  }
}
Vue.use(VueGlobalVariable, {
  globals: {
  $currColloc: new currentColloc(-1,''),
  $setMenuItemDisabled: new MenuItem(true)
  //$checkedGoogle : false
  },
  });

const main = async() => {
  await AuthService.init();

  new Vue({
    i18n,
    router,
    el:'#app',
    render: h => h(App)
  }).$mount('#app')
};

main();