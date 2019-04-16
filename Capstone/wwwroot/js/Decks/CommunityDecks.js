const decksDiv = document.querySelector('div.decks');
const preloader = document.querySelector('img.preload');
const officialButton = document.getElementById('official-button');
const officialUrl = `http://localhost:${location.port}/API/LazyLoadPublicAdminDecks`;
let isOfficial = false;
let filteredResults = [];

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

LazyLoad(officialUrl, decksDiv);

officialButton.addEventListener('click', b => {

    isOfficial = !isOfficial;

    if (isOfficial) {
        filteredResults = allResults.filter(item => item.isAdminDeck);
        officialButton.innerText = "View All Community Decks";
        RedrawScreen();
    }
    else {
        filteredResults = allResults;
        officialButton.innerText = "View Official Decks Only";
        RedrawScreen();
    }
});

