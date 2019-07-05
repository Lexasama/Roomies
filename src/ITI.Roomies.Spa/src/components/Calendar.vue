<template>
  <div class="mainContainer">
    <FullCalendar defaultView="dayGridWeek"
      :header="{
        left: 'prev,next today',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
      }" 
      :events="eventData" :plugins="calendarPlugins" :editable="true"   />
  </div>
</template>


<script>
import Vue from "vue";
import {getTasksByCollocIdAsync} from "../api/TaskApi";
import TaskCollocVue from './Task/TaskColloc.vue';

import FullCalendar from "@fullcalendar/vue";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from '@fullcalendar/timegrid'
import interactionPlugin from "@fullcalendar/interaction";
import bootstrapPlugin from '@fullcalendar/bootstrap';

Vue.component('FullCalendar', FullCalendar)


export default {
  data() {
    return {
      eventData:[{title: "All Day Event",start: "2019-06-30"}],
      calendarPlugins: [ // plugins must be defined in the JS
        dayGridPlugin,
        timeGridPlugin,
        interactionPlugin // needed for dateClick
      ],
      
    };
  },    
  methods:{

    sqlToJsDate(sqlDate) {
      sqlDate = sqlDate.replace("T", " ");
      return sqlDate;
    }
  },
  async mounted(){
    var Task = await getTasksByCollocIdAsync(this.$currColloc.collocId);
    var eventData=[];
    var taskData={title:"",start:""};
    
    for (var i=0; i<Task.length; i++){
      taskData.start =this.sqlToJsDate( Task[i].taskDate);
      taskData.title = 
        Task[i].firstName+' '+Task[i].lastName+' : '+Task[i].taskDes;
      eventData[i] = taskData ;
      console.log(taskData);
    }
    this.eventData= eventData;
  },
}

</script>
<style lang="scss" scoped>
@import"~@fullcalendar/core/main.css";
@import"~@fullcalendar/daygrid/main.css";
@import "~@fullcalendar/timegrid/main.css";
</style>