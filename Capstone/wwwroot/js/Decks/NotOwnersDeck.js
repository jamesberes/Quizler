const cards = document.querySelectorAll('div.card');
let hiddenInputs = document.querySelectorAll(".submit-the-card-id");

hiddenInputs.forEach(input => {
    input.value = input.parentElement.parentElement.children[0].children[0].value;
})


cards.forEach(card => {
    card.addEventListener('click', event => {

        let clickedCard = event.path[0];
        if (event.target.className == 'front' || event.target.className == 'back' || event.target.tagName == 'IMG') {
            clickedCard = event.path[2];
        } else if (event.target.className == 'card-info') {
            clickedCard = event.path[1];
        } 

        clickedCard.children[0].children[0].classList.toggle('hidden');
        clickedCard.children[0].children[1].classList.toggle('hidden');
    });
});