let maxLoadedDeckId = 0;
let resultCount = 10;
let allResults = [];

function LazyLoad(apiUrl, container) {
    fetch(apiUrl + `?startId=${maxLoadedDeckId}`)
        .then(result => {
            result.json().then(data => {

                if (data.length < 10) {
                    preloader.classList.add('hidden');
                }
                
                data.forEach(item => {

                    allResults.push(item);

                    let deck = document.createElement('a');
                    deck.classList.add('deck-link');
                    deck.href = `/Decks/ViewDeck?deckId=${item.id}`;

                    let deckDiv = document.createElement('div');
                    deckDiv.classList.add('deck');
                    deckDiv.id = `${item.id}`;
                    deckDiv.style.backgroundColor = item.deckColor;
                    deckDiv.style.color = item.textColor;

                    let h1 = document.createElement('h1');
                    h1.innerText = item.name;

                    let p = document.createElement('p');
                    p.innerText = item.description;

                    maxLoadedDeckId = item.id;

                    deckDiv.appendChild(h1);
                    deckDiv.appendChild(p);

                    deck.appendChild(deckDiv);

                    container.appendChild(deck);

                })
                resultCount = data.length;
                if (resultCount > 9) {
                    window.addEventListener('scroll', LazyLoad(apiUrl, container));
                }
                return data;
            })
        });
}