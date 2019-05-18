<template>

<div>
  <div>
    <h1> {{$t("lItem")}} {{courseName}}({{courseId}})</h1>
    <div>
      <router-link :to="`item/create`"><i>{{$t('add')}}</i></router-link>
    </div>
  </div>
  
  <table>

    <div v-if="itemList == 0">
        <tr>
          <td>{{$t('nullItem')}} {{courseName}}</td>
        </tr>
      </div>

    <thead>
      <tr>
        <th>{{$t('Nom')}}</th>
        <th>{{$t('price')}}</th>
        <th>Roomie</th>
        <th>Options</th>
      </tr>
    </thead>
    <tbody>
      
      <div>
      <tr v-for="i of itemList" :key="i.itemId">
        <td>{{i.itemName}}</td>
        <td>{{i.itemPrice}}</td>
        <td>{{i.roomieId}}</td>
        <td>
          <router-link :to="`item/edit/${i.itemId}`"><i>{{$t('edit')}}</i></router-link>
          <a href="#" @click="deleteItem(i.itemId)">
            <i>{{$t('supp')}}</i>
          </a>
        </td>
      </tr>
      </div>
    </tbody>
  </table>
 
</div>
</template>

<script>

import { getItemListAsync, deleteItemAsync} from "../../api/ItemApi";
import {getGroceryListByIdAsync} from "../../api/GroceriesApi";
import { getRoomieByIdAsync} from "../../api/RoomiesApi.js";


export default {
  data() {
    return { 
      itemList: [],
      courseName: '', 
      courseId: null,
      item: {},
      roomieName: "",

    }
  },

  async mounted(){
    this.courseId = this.$route.params.id;
    //console.log("the course Id is:" +this.courseId);
    await this.refreshList();
    
  },

  methods:{
    async refreshList(){
      console.log("the course Id is:" +this.courseId);
      debugger;
      this.itemList = await getItemListAsync(this.courseId);
    
      this.courseName = await getGroceryListByIdAsync(this.courseId).courseName;
      
      console.log("this is the course id");
      console.log(await getGroceryListByIdAsync(this.courseId));
      debugger;
     
    },

    async deleteItem(itemId){
      try{
      await deleteItemAsync(itemId);
      await refreshList();
      }catch(e){
        console.log(e);
      }finally{
        await refreshList();
      }
    },
    
  },
}
</script>
