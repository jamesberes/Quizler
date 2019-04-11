const deckId = document.querySelector('input#deckId').value;
const apiUrl = 'http://localhost:62451/API/';

fetch(`${apiUrl}getdeck?id=${deckId}`)
    .then(response => {
        response.json()
            .then(data => {
                console.log(data);
            });
    });