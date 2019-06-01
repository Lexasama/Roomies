import Vue from 'vue';
import VueI18n from 'vue-i18n';

Vue.use(VueI18n);

const messages ={
    'fr':{
        testMsg:'Bienvenue sur votre application Vus.js',
        greeting:"Bienvenue Roomie",
        Nom:'Nom',
        Prenom: 'Prénom',
        Bday: 'Date de naissance',
        description: 'Description',
        modifier: 'Modifier',
        lang: 'Change to English',
        number:'Téléphone',
        Profil: 'Votre Profil',
        Welcome: 'Bienvenue sur',
        pic: 'Votre photo de profil',
        erreur: 'ERREUR',
        nullDesc: "Vous n'avez pas de description",
    },
    
    'en':{
        testMsg:'Welcome to your Vue.js app',
        greeting:"Welcome Rommie",
        Nom:'Name',
        Prenom: 'Firstname',
        Bday: 'Birthday',
        description: 'About you',
        modifier: 'Modifier',
        lang: 'Changer de langue',
        number: 'Phone number',
        Profil: 'Your Profil',
        Welcome: 'Welcome to',
        pic: 'Your profile picture',
        erreur: 'ERROR',
        nullDesc: "You don't have a description",
    }
};

const i18n = new VueI18n({
    locale:'fr',
    fallbackLocale:'en',
    messages,
});
export default i18n;