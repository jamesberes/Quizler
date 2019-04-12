let maxLoadedDeckId = 0;
let resultCount = 10;

function LazyLoad(apiUrl, container) {
    fetch(apiUrl + `LazyLoadDecks?startId=${maxLoadedDeckId}`)
        .then(result => {
            result.json().then(data => {

                if (data.length < 10) {
                    preloader.classList.add('hidden');
                }

                data.forEach(item => {
                    let deck = document.createElement('a');
                    deck.classList.add('deck-link');
                    deck.href = `/Decks/ViewDeck?deckId=${item.id}`;
                    deck.innerHTML = `<div class="deck" id="${item.id}">` +
                        `<h1>${item.name}</h1>` +
                        `<p>${item.description}</p>` +
                        '</div>'
                    maxLoadedDeckId = item.id;
                    container.appendChild(deck);
                })
                resultCount = data.length;
                if (resultCount > 9) {
                    window.addEventListener('scroll', LazyLoad(apiUrl, container));
                }
            })
        });
}