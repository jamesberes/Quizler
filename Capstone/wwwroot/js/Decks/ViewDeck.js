const cards = document.querySelectorAll('div.card');

cards.forEach(card => {
    card.addEventListener('click', event => {
        let clickedCard = event.path[1];
        if (event.target.className == 'card') {
            clickedCard = event.target;
        }

        clickedCard.children[0].classList.toggle('hidden'); //TODO: Can this be refactored/reduced?
        clickedCard.children[1].classList.toggle('hidden');
        clickedCard.children[2].classList.toggle('hidden');
        clickedCard.children[3].classList.toggle('hidden');
        clickedCard.children[4].classList.toggle('hidden');
    });
});