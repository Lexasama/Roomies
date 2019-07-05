import Vue from 'vue';
import VueI18n from 'vue-i18n';

Vue.use(VueI18n);

const messages = {
    'fr': {
        //A
        all: 'Tous',
        //B
        Bday: 'Date de naissance',
        //C
        cal: 'Calendrier',
        category: "Categories",
        colloc: 'Colocation',
        collocCreate: ' Créer une colocation puis inviter vos Roomies :',
        collocInvite: 'Rejoigner une colocation à partir d\'un code reçu par email :',
        collocInvite2: 'Inviter de nouveaux Roomies à votre colocation :',
        connectvia:  "Se connecter via Google",
        connectvia2: "Se connecter via Roomies",
        create: 'Création',
        //D
        deco: "Se déconnecter",
        description: 'Description',
        //E
        erreur: 'ERREUR',
        //F
        //G
        greeting: "Bienvenue Roomie",
        groceryL: "Liste de courses",
        //H
        //I
        //J
        join :'Rejoindre',
        //K
        //L
        lang: 'Change to English',
        //M
        mouth: 'mois',
        modifier: 'Modifier',
        manageFlat: "Gestion de colocation",
        //N
        nullDesc: "Vous n'avez pas de description",
        number: 'Téléphone',
        Nom: 'Nom',
        //O
        //p
        pic: 'Photo :',
        Prenom: 'Prénom',
        Profil: 'Votre Profil',
        //Q
        //R
        //S
        spendings: "Dépenses",
        settings: "Paramètres",
        //T
        task: 'Tâches',
        testMsg: 'Bienvenue sur votre application Vus.js',
        thèmes: "Thèmes",
        //U
        //V
        //WXYZ
        week: "Heddomadaire",
        Welcome: 'Bienvenue sur',
        year: "Annuelle",
    },

    'en': {
        //A\\
        all: "All",

        //B
        Bday: 'Birthday',

        //C
        cal: 'Calendar',
        category: "Categories",
        colloc: 'Flatsharing',
        collocCreate: 'Create a flatsharing to invite your Roomies',
        collocInvite: 'Join a flatsharing using the code revieved by email : ',
        collocInvite2: 'Invite new Roomies :',
        connectvia: "Connect via Google",
        connectvia2: "Connect via Roomies",
        create: 'Create',

        //D
        day: 'Day',
        deco: 'Disconnect',
        description: 'About you',

        //E
        erreur: 'ERROR',

        //G
        greeting: "Welcome Rommie",
        groceryL: "Grorecy List",
//J
join :'Join',
        //L
        lang: 'Changer de langue',

        //M
        modifier: 'Modifier',
        mouth: 'mouth',
        manageFlat: "Manage flatsharing",
        number: 'Phone number',

        //N
        nullDesc: "You don't have a description",
        Nom: 'Name',

        //P
        Prenom: 'Firstname',
        Profil: 'Your Profile',
        pic: 'Profile picture',
        //S
        spendings: 'Spendings',
        settings: "Settings",

        //T
        task: "Tasks",
        testMsg: 'Welcome to your Vue.js app',
        thèmes: "Themes",

        //WXYZ
        Welcome: 'Welcome to',
        week: 'Week',
        year: 'year',

    }
};

const i18n = new VueI18n({
    locale: 'fr',
    fallbackLocale: 'en',
    messages,
});
export default i18n;