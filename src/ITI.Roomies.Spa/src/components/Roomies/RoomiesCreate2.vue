<template>
<div class="container">
<header>
  <H2></H2>
</header>

<main>
  

  <form>
    <div class="alert alert-danger" v-if="errors.length > 0">

    </div>
    <button class="btn btn-dark" @click="onSubmit">{{$t('save')}}</button>
  </form>
</main>

</div>
</template>

<script>
export default {
  data() {
    return {

    }
  },
  async mounted() {

  },

  methods: {

  }
  
}
</script>
<style  lang="scss"scoped>
.container{
  margin-left: 0;
  // margin-left: 0;
  margin-right: 0;
}
</style>
