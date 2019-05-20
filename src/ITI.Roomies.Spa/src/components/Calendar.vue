<template>
  <div id="container">
    <v-calendar color="red" is-dark is-expanded :rows="3" :columns="4"></v-calendar>
  </div>
</template>


<script>
import Vue from "vue";
import VCalendar from "v-calendar";

// Use v-calendar & v-date-picker components
Vue.use(VCalendar, {
  componentPrefix: "v" // Use <vc-calendar /> instead of <v-calendar />
  // ...other defaults
});

export default {
  data() {
    return {
      attrs: [
        {
          key: "today",
          highlight: {
            backgroundColor: "#ff8080"
            // Other properties are available too, like `height` & `borderRadius`
          },
          rows: 3,
          columns: 4
          // dates: new Date(),
        }
      ]
    };
  }
};
</script>

