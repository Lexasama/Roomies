import Vue from 'vue';
import VueI18n from 'vue-i18n';

Vue.use(VueI18n);

const messages ={
    'fr':{
        aItem:"Ajouter un article a la liste",
        testMsg:'Bienvenue sur votre application Vus.js',
        greeting:"Bienvenue Roomie",
        Nom:'Nom',
        Prenom: 'Prénom',
        Bday: 'Date de naissance',
        cCourse:'Ajouter une liste de course',
        eCourse: 'Modifier la liste',
        eItem: "Modifier l'article",
        date: "Date",
        description: 'Description',
        edit: 'Editer',
        modifier: 'Modifier',
        lang: 'Change to English',
        liste: 'Liste',
        lItem: "Article dans la liste",
        number:'Téléphone',
        Profil: 'Votre Profil',
        Welcome: 'Bienvenue sur',
        price: 'Prix',
        pic: 'Votre photo de profil',
        erreur: 'ERREUR',
        nullDesc: "Vous n'avez pas de description",
        nullCourse:"Il n'y a pas de liste de course",
        nullItem: "Il n'y a aucun article dans la liste",
        nullPic: "Vpus n'avez pas de photo de profil",
        cListe: "Liste de Course",
        add: "Ajouter",
        save: 'Sauvegarder',
        supp: 'Supprimer',
        loading:'Chargement en cours',
        wlc: 'Bienvenue Roomie',
    },
    
    'en':{
        aItem:"Add an Item to list",
        testMsg:'Welcome to your Vue.js app',
        greeting:"Welcome Rommie",
        Nom:'Name',
        Prenom: 'Firstname',
        Bday: 'Birthday',
        cCourse: "Add a grocery list",
        Date: "Date",
        description: 'About you',
        edit: 'Edit',
        eCourse: 'Edit list',
        eItem: " Edit item",
        lang: 'Changer de langue',
        liste:'List',
        number: 'Phone number',
        Profil: 'Your Profil',
        Welcome: 'Welcome to',
        pic: 'Your profile picture',
        price: 'Price',
        erreur: 'ERROR',
        nullCourse: "You don't have any Grocery list",
        nullDesc: "You don't have a description",
        nullItem: "There no intem in the list",
        nullPic: "You don't have a profile picture",
        cListe:"Grocery list",
        add:'Add',
        save:'Save',
        supp: 'Delete',
        loading:'Loading',
        wlc: "Welcome Roomie"
    }
};

const i18n = new VueI18n({
    locale:'fr',
    fallbackLocale:'en',
    messages,
});
export default i18n;