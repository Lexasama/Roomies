<template>
    <section class="jumbotron text-center">
        <div>
            <h1 class="jumbotron-heading">Bienvenue sur Roomies</h1>
            <el-button @click="changeLocale('fr')">fr</el-button>
            <el-button @click="changeLocale('en')">en</el-button>
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