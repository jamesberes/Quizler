const cards = document.querySelectorAll('div.card');
const denyButton = document.querySelector(".btn-deny");
const approveButton = document.querySelector(".btn-approve");
const apiUrl = `http://localhost:${location.port}/API/`;
const deckId = document.querySelector('.btn-deny').id;
let deck;
console.log(deckId);

function GetDeck() {
    fetch(`${apiUrl}GetDeck?Id=${deckId}`)
        .then(response => {
            response.json()
                .then(data => {
                    deck = data;
                })
        })
}

GetDeck();

cards.forEach(card => {
    card.addEventListener('click', event => {
        let clickedCard = event.path[1];
        if (event.target.className == 'card') {
            clickedCard = event.target;
        }

        clickedCard.children[0].classList.toggle('hidden');
        clickedCard.children[1].classList.toggle('hidden');
        clickedCard.children[2].children[0].classList.toggle('hidden');
        clickedCard.children[2].children[1].classList.toggle('hidden');
        clickedCard.children[2].children[2].classList.toggle('hidden');
    });
});

approveButton.addEventListener('click', function () {
    fetch(`${apiUrl}MakePublic?deckId=${deckId}`, { method: "post" })
        .then(response => {
            console.log(response);
            if (response.ok) {
                alert("Deck has been approved!");
                window.location.href = "/Admin/Index/"
                //GetDeck();
            } else {
                alert("Something went wrong...");
            }
        })
});
denyButton.addEventListener('click', function () {
    fetch(`${apiUrl}MakePrivate?deckId=${deckId}`, { method: "post" })
        .then(response => {
            if (response.ok) {
                alert("Deck has been made Private.")
                reviewToggleButton.innerText = "Submit Deck For Review";
                window.location.href = "/Admin/Index/"
            }
            else {
                alert("Something went wrong...");
            }
        })
});