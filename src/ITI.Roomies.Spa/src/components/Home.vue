<template>
    <section class="jumbotron text-center">
        <div class="container">
            <h1 class="jumbotron-heading">Bienvenue sur Roomies</h1>
            <button @click="changeLocale('fr')">fr</button>
            <button @click="changeLocale('en')">en</button>
            <p class="lead text-muted">{{$t('testMsg')}}</p>
        </div>
    </section>
</template>

<script>
import i18n from '../plugins/i18n';

export default {

    methods: {
            changeLocale(locale) {
                   i18n.locale = locale;
            }
    }
}

</script>

<style lang="scss">

</style>