const cards = document.querySelectorAll('div.card');
const apiUrl = `http://localhost:${location.port}/API/`;
const deckId = document.querySelector('.btn-login').id;
let deck;

cards.forEach(card => {

    card.addEventListener('click', event => {
        let clickedCard = event.path[1];

        if (event.target.className == 'card') {
            clickedCard = event.target.children[0];
        }

        clickedCard.children[0].classList.toggle('hidden');
        clickedCard.children[1].classList.toggle('hidden');
    });
});