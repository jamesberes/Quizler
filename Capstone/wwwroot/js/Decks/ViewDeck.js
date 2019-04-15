const cards = document.querySelectorAll('div.card');
const reviewToggleButton = document.querySelector(".btn-toggle-review");
const apiUrl = `http://localhost:${location.port}/API/`;
const deckId = document.querySelector('.deckId-button').id;
let deck;
console.log(deckId);

function GetDeck() {
    fetch(`${apiUrl}GetDeck?Id=${deckId}`)
        .then(response => {
            response.json()
                .then(data => {
                    deck = data;
                    if (deck.forReview && !deck.publicDeck) {
                        reviewToggleButton.classList.add("disabled");
                    }
                    else if (deck.publicDeck) {
                        reviewToggleButton.classList.remove("disabled");
                    }
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
        clickedCard.children[2].classList.toggle('hidden');
        clickedCard.children[3].classList.toggle('hidden');
        clickedCard.children[4].classList.toggle('hidden');
    });
});

reviewToggleButton.addEventListener('click', function () {
    if (!deck.forReview && !deck.publicDeck) {
        fetch(`${apiUrl}ToggleForReferral?deckId=${deckId}`, { method: "post" })
            .then(response => {
                console.log(response);
                if (response.ok) {
                    alert("Deck has been submitted for review!");
                    reviewToggleButton.innerText = "Under Review...";
                    GetDeck();
                } else {
                    alert("Something went wrong...");
                }
            })
    } else if (deck.publicDeck) {
        fetch(`${apiUrl}MakePrivate?deckId=${deckId}`, { method: "post" })
            .then(response => {
                if (response.ok) {
                    alert("Deck has been made Private.")
                    reviewToggleButton.innerText = "Submit Deck For Review";
                    GetDeck();
                }
                else {
                    alert("Something went wrong...");
                }
            })
    }
    
})