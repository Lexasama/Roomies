import Vue from 'vue';
import VueI18n from 'vue-i18n';

Vue.use(VueI18n);

const messages ={
    'fr':{
        testMsg:'Bienvenue sur votre application Vus.js',
        greeting:"Bienvenue Roomie",
    },
    'en':{
        testMsg:'Welcome to your Vue.js app',
        greeting:"Welcome Rommie",
    }
};

const i18n = new VueI18n({
    locale:'fr',
    fallbackLocale:'en',
    messages,
});
export default i18n;