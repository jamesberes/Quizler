const deckId = document.querySelector('input#deckId').value;
const apiUrl = `http://localhost:${location.port}/API/`;

fetch(`${apiUrl}getdeck?id=${deckId}`)
    .then(response => {
        response.json()
            .then(data => {
                console.log(data);
            });
    });
