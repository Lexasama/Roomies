// Vue router setup
import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

import requireAuth from './helpers/requireAuth';

// Components
import Home from   './components/Home.vue'
import Login from  './components/Login.vue'
import Logout from './components/Logout.vue'
import Calendar from './components/Calendar.vue'

import Collocation from './components/Collocation.vue'

import ImageUploader from './components/Roomies/ImageUploader.vue'

import RoomiesCreate from './components/Roomies/RoomiesCreate.vue'
import RoomieProfile from './components/Roomies/RoomieProfile.vue'
import RoomieProfil from './components/Roomies/Roomie.vue'
import checkRoomie from './components/Roomies/checkRoomie.vue'

import TaskColloc from './components/Task/TaskColloc.vue'
import TaskRoomie from './components/Task/TaskRoomie.vue'
import TaskCreate from './components/Task/TaskCreateEdit.vue'

import GroceryList from './components/GroceryList/Grocery.vue'
import GroceryCreate from './components/GroceryList/GroceryCreate.vue'

import Item from './components/Item/Item.vue'
import ItemCreate from './components/Item/ItemCreateEdit.vue'
import RItemCreate from './components/Item/RItem.vue'

import Category from './components/Spendings/Category/Category.vue'
import CategoryInfo from './components/Spendings/Category/CategoryInfo.vue'

import Budget from './components/Spendings/Transaction/BudgetCreate.vue'

import TransactionsCreateEditList from './components/Spendings/Transaction/TransactionsCreateEditList';


const routes = [
    { path: '/', component: Home, beforeEnter: requireAuth },
    
    { path: '/login', component: Login },
    { path: '/logout', component: Logout, beforeEnter: requireAuth },

    {path: '/roomies/calendar', component: Calendar, beforeEnter: requireAuth},

    //{path: '/roomies/upload/:id/:isRoomie', component: ImageUploader, beforeEnter: requireAuth},
    // {path: '/roomies/upload/roomie/create', component: ImageUploader, beforeEnter: requireAuth},
    // {path: '/roomies/upload/colloc/edit/:id', component: ImageUploader, beforeEnter: requireAuth},
    // {path: '/rommies/upload/colloc/create/:id', component: ImageUploader, beforeEnter: requireAuth},
    { path: '/upload/:object(colloc|roomie)/:mode(create|edit)/:id?', component: ImageUploader, beforeEnter: requireAuth},

    {path: '/roomies/collocation', component: Collocation, beforeEnter: requireAuth},
    {path: '/roomies/collocation/:id?', component: Collocation, beforeEnter: requireAuth},


    { path: '/roomies/create', component: RoomiesCreate, beforeEnter: requireAuth },
    { path: '/roomies', component: RoomieProfil, beforeEnter: requireAuth },
    { path: '/roomies/profile', component: RoomieProfile, beforeEnter: requireAuth },
    { path: '/checkRoomie', component: checkRoomie, beforeEnter: requireAuth },

    { path: '/task/colloc', component: TaskColloc, beforeEnter: requireAuth },
    { path: '/task/roomie', component: TaskRoomie, beforeEnter: requireAuth },
    { path: '/task/create', component: TaskCreate, beforeEnter: requireAuth },
    { path: '/task/edit/:id?', component: TaskCreate, beforeEnter: requireAuth },
  
    { path: '/Ritem', component: RItemCreate, beforeEnter: requireAuth },

    { path: '/course', component: GroceryList, beforeEnter: requireAuth},
    { path: '/course/edit/:id?', component: GroceryCreate, beforeEnter: requireAuth},
    { path: '/course/info/:id?', component: Item, beforeEnter: requireAuth},
    { path: '/course/info/:id?/item/create', component: ItemCreate, beforeEnter: requireAuth},
    { path: '/course/info/:id?/item/edit/:itemId?', component: ItemCreate, beforeEnter: requireAuth},
    { path: '/course/create', component: GroceryCreate, beforeEnter: requireAuth},
    { path: '/budget/:mode([create|edit]+)/:id?', component: Budget, beforeEnter: requireAuth},
    
    { path: '/category/:mode(create|edit)/:id?', component: Category, beforeEnter: requireAuth},
    { path: '/category/info', component: CategoryInfo, beforeEnter: requireAuth },
    
    { path: '/transaction/:mode(create|edit)/:id?', component: TransactionsCreateEditList, beforeEnter: requireAuth },
];

export default new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
});