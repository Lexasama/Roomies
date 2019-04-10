// Vue router setup
import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

import requireAuth from './helpers/requireAuth';

// Components
import Home from   './components/Home.vue'
import Login from  './components/Login.vue'
import Logout from './components/Logout.vue'

import RoomiesCreate from './components/Roomies/RoomiesCreate.vue'
import RoomieProfil from './components/Roomies/Roomie.vue'

const routes = [
    { path: '', component: Home, beforeEnter: requireAuth },
    
    { path: '/login', component: Login },
    { path: '/logout', component: Logout, beforeEnter: requireAuth },


    { path: '/createRoomies', component: RoomiesCreate, beforeEnter: requireAuth },
    { path: '/roomies', component: RoomieProfil, beforeEnter: requireAuth },
    
];

export default new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});