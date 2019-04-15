const decksDiv = document.querySelector('div.decks');
const preloader = document.querySelector('img.preload');

const url = `http://localhost:${location.port}/API/LazyLoadPublicDecks`;

LazyLoad(url, decksDiv);