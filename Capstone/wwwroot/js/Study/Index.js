let right = 0;
let wrong = 0;
const deckId = document.querySelector('input#deckId').value;
const apiUrl = `http://localhost:${location.port}/API/`;

const studyCard = document.querySelector('div#study-card');
const frontOfCard = document.querySelector('h2#card-front');
const backOfCard = document.querySelector('h3#card-back');
const scoreTracker = document.querySelector('div#score-tracker');
const correctButton = document.querySelector('div#correct-button');
const wrongButton = document.querySelector('div#wrong-button');
const scoreDisplay = document.querySelector('div#score-count p');
const completeSession = document.querySelector('div#complete-session');
let unansweredQuestions = [];
let answeredQuestions = [];

fetch(`${apiUrl}getdeck?id=${deckId}`)
    .then(response => {
        response.json()
            .then(data => {
                unansweredQuestions = data.cards;
                frontOfCard.innerText = unansweredQuestions[0].front;
                backOfCard.innerText = unansweredQuestions[0].back;
                studyCard.classList.remove('hidden');
            });
    });

function ComputeScore(correct) {
    if (correct) {
        right++;
    }
    else {
        wrong++;
    }
}

function NextCard() {
    if (unansweredQuestions.length > 0) {
        answeredQuestions.push(unansweredQuestions[0]);
        unansweredQuestions.shift();
    }

    if (unansweredQuestions.length > 0) {
        frontOfCard.classList.remove('hidden');
        backOfCard.classList.add('hidden');
        scoreTracker.classList.add('hide-score');
        frontOfCard.innerText = unansweredQuestions[0].front;
        backOfCard.innerText = unansweredQuestions[0].back;
    } else {
        CompleteStudySession();
    }
}

function FlipCard() {
    frontOfCard.classList.toggle('hidden');
    backOfCard.classList.toggle('hidden');
    scoreTracker.classList.toggle('hide-score');
}

function CompleteStudySession() {
    studyCard.classList.add('hidden');
    scoreTracker.classList.add('hidden');
    completeSession.querySelector('p.correct').innerText = `You got ${right} questions correct.`;
    completeSession.querySelector('p.incorrect').innerText = `You got ${wrong} questions wrong.`;
    completeSession.classList.remove('hidden');
}

studyCard.addEventListener('click', FlipCard);

correctButton.addEventListener('click', function () {
    ComputeScore(true);
    scoreDisplay.innerText = `Correct: ${right} - Wrong: ${wrong}`;
    NextCard();
});
wrongButton.addEventListener('click', function () {
    ComputeScore(false);
    scoreDisplay.innerText = `Correct: ${right} - Wrong: ${wrong}`;
    NextCard();
});
