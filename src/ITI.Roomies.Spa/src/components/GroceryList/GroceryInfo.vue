<template>
  <div>
      
    <div>
      <h1>{{$t('cListe')}} {{courseName}}</h1>
    </div>
    <div>
      <item :courseId="courseId"></item>
    </div>

  </div>
</template>

<script>

import item from "../Item/Item.vue"

export default {
  props: {
    courseId: {
      type: Number,
      required: true,
      // default: null
    }
  },
  components:{
    item,
  },

  data(){
    return{

    }
  },

  async created(){

  },
  async mounted(){

  },
  methods:{

 },
  
}
</script>
