const cards = document.querySelectorAll('div.card');
const reviewToggleButton = document.querySelector(".btn-toggle-review");
const apiUrl = `http://localhost:${location.port}/API/`;
const deckId = document.querySelector('.deckId-button').id;
let deck;

let cardFlipped = false;

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

function isOverflown(element) {
    return element.scrollHeight > element.clientHeight || element.scrollWidth > element.clientWidth;
}

cards.forEach(card => {

    card.addEventListener('click', event => {
        let clickedCard = event.path[1];
        if (event.target.className == 'card') {
            clickedCard = event.target;
        }

        if (cardFlipped) {
            cards.forEach(c => {
                c.children[1].classList.add('hidden');
                c.children[2].children[0].classList.add('hidden');
                c.children[2].children[1].classList.add('hidden');
                c.children[2].children[2].classList.add('hidden');

                c.style.height = '20vh';
                c.style.width = '30%';
            });

            cardFlipped = false;
        }

        clickedCard.children[1].classList.toggle('hidden');
        clickedCard.children[2].children[0].classList.toggle('hidden');
        clickedCard.children[2].children[1].classList.toggle('hidden');
        clickedCard.children[2].children[2].classList.toggle('hidden');


        if (clickedCard.style.height == 'auto') {
            clickedCard.style.height = '20vh';
            clickedCard.style.width = '30%';
        } else {
            clickedCard.style.height = 'auto';
            clickedCard.style.width = '100%';
        }

        cardFlipped = true;
    });
});

reviewToggleButton.addEventListener('click', function () {
    if (!deck.forReview && !deck.publicDeck) {
        fetch(`${apiUrl}ToggleForReferral?deckId=${deckId}`, { method: "post" })
            .then(response => {
                console.log(response);
                if (response.ok) {
                    alert("Once an admin approves your deck, it will be posted to the Community Decks page!");
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
                    alert("You have stopped sharing your deck with the Quizler community")
                    reviewToggleButton.innerText = "Share With Community";
                    GetDeck();
                }
                else {
                    alert("Something went wrong...");
                }
            })
    }

})