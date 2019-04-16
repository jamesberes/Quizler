const decksDiv = document.querySelector('div.decks');
const preloader = document.querySelector('img.preload');
const searchBar = document.querySelector('input#search-bar');
let filteredResults;

const url = `http://localhost:${location.port}/API/LazyLoadDecks`;

LazyLoad(url, decksDiv);

searchBar.addEventListener('input', e => {
    let searchTerm = searchBar.value.toLowerCase();
    filteredResults = allResults.filter(deck => {
        if (deck.name.toLowerCase().includes(searchTerm)) {
            return true;
        } else if (cardSearch(deck.cards, searchTerm)) {
            return true;
        }
    });
    RedrawScreen();
});

function cardSearch(cards, searchTerm) {
    let output = false;
    cards.forEach(card => {
        if (card.front.toLowerCase().includes(searchTerm)) {
            output = true;
        }
    });
    return output;
}

function RedrawScreen() {
    let allDecks = document.querySelectorAll('a.deck-link');

    allDecks.forEach(deck => {
        decksDiv.removeChild(deck)
    })

    filteredResults.forEach(item => {

        let deck = document.createElement('a');
        deck.classList.add('deck-link');
        deck.href = `/Decks/ViewDeck?deckId=${item.id}`;

        let deckDiv = document.createElement('div');
        deckDiv.classList.add('deck');
        deckDiv.id = `${item.id}`;

        let h1 = document.createElement('h1');
        h1.innerText = item.name;

        let p = document.createElement('p');
        p.innerText = item.description;

        maxLoadedDeckId = item.id;

        deckDiv.appendChild(h1);
        deckDiv.appendChild(p);

        deck.appendChild(deckDiv);

        decksDiv.appendChild(deck);

    })
}