let right = 0;
let wrong = 0;
const deckId = document.querySelector('input#deckId').value;
const apiUrl = `http://localhost:${location.port}/API/`;

const frontOfCard = document.querySelector('div#study-card-front');
const backOfCard = document.querySelector('div#study-card-back');
const scoreTracker = document.querySelector('div#score-tracker');
const correctButton = document.querySelector('div#correct-button');
const wrongButton = document.querySelector('div#wrong-button');
const scoreDisplay = document.querySelector('div#score-count p');

console.log(scoreDisplay);

fetch(`${apiUrl}getdeck?id=${deckId}`)
    .then(response => {
        response.json()
            .then(data => {
                console.log(data);
            });
    });

function computeScore(correct) {
    if (correct) {
        right++;
    }
    else {
        wrong++;
    }
}

correctButton.addEventListener('click', function () {
    computeScore(true);
    scoreDisplay.innerText = `Correct: ${right} - Wrong: ${wrong}`;
});
wrongButton.addEventListener('click', function () {
    computeScore(false);
    scoreDisplay.innerText = `Correct: ${right} - Wrong: ${wrong}`;
});
