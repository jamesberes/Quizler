const cards = document.querySelectorAll('div.card');
let hiddenInputs = document.querySelectorAll(".submit-the-card-id");

hiddenInputs.forEach(input => {
    input.value = input.parentElement.parentElement.children[0].children[0].value;
})


cards.forEach(card => {
    card.addEventListener('click', event => {
        let clickedCard = event.path[1];
        if (event.target.className == 'card') {
            clickedCard = event.target;
        }

        clickedCard.children[1].classList.toggle('hidden');
        clickedCard.children[2].classList.toggle('hidden');
    });
});