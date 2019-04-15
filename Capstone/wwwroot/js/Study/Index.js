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
const completeSession = document.querySelector('div#complete-session');
const endSessionButton = document.querySelector('div#complete-button');
const image = document.querySelector('#img-front');
const cancelButton = document.querySelector('#cancel-button');
const studyModeDiv = document.querySelector('#select-study-mode');
const randomOrderButton = document.querySelector('#random-order-btn');
const sequentialOrderButton = document.querySelector('#sequential-order-button');


let unansweredQuestions = [];
let answeredQuestions = [];
let hasBeenFlipped = false;
let random = false;

fetch(`${apiUrl}getdeck?id=${deckId}`)
    .then(response => {
        response.json()
            .then(data => {
                unansweredQuestions = data.cards;

                randomOrderButton.addEventListener('click', e => {
                    unansweredQuestions = shuffle(unansweredQuestions);
                    studyModeDiv.classList.add('fade-animation');
                    studyModeDiv.addEventListener('animationend', e => {
                        studyModeDiv.classList.add('hidden');
                        DisplayFirstCard();
                    });
                });

                sequentialOrderButton.addEventListener('click', e => {
                    studyModeDiv.classList.add('fade-animation');
                    studyModeDiv.addEventListener('animationend', e => {
                        studyModeDiv.classList.add('hidden');
                        DisplayFirstCard();
                    });
                });
            });
    });

function DisplayFirstCard() {
    if (unansweredQuestions[0].imageURL != '') {
        image.src = unansweredQuestions[0].imageURL;
        frontOfCard.innerText = '';
    } else {
        frontOfCard.innerText = unansweredQuestions[0].front;
        image.src = '';

    }
    backOfCard.innerText = unansweredQuestions[0].back;
    studyCard.classList.remove('hidden');
}

function shuffle(array) {
    var currentIndex = array.length, temporaryValue, randomIndex;

    while (0 !== currentIndex) {

        randomIndex = Math.floor(Math.random() * currentIndex);
        currentIndex -= 1;

        temporaryValue = array[currentIndex];
        array[currentIndex] = array[randomIndex];
        array[randomIndex] = temporaryValue;
    }

    return array;
}

function ComputeScore(correct) {
    if (correct) {
        right++;
    }
    else {
        wrong++;
    }
}


function NextCard() {
    hasBeenFlipped = false;
    if (unansweredQuestions.length > 0) {
        answeredQuestions.push(unansweredQuestions[0]);
        unansweredQuestions.shift();
        //studyCard.classList.remove('fade-animation');
        //studyCard.classList.add('fade-animation');

    }

    if (unansweredQuestions.length > 0) {
        frontOfCard.classList.remove('hidden');
        backOfCard.classList.add('hidden');
        image.classList.remove('hidden');
        scoreTracker.classList.add('hide-score');

        if (unansweredQuestions[0].imageURL != '') {
            image.src = unansweredQuestions[0].imageURL;
            frontOfCard.innerText = '';
        } else {
            frontOfCard.innerText = unansweredQuestions[0].front;
            image.src = '';
        }
        backOfCard.innerText = unansweredQuestions[0].back;

    } else {
        cancelButton.classList.add('hidden');
        CompleteStudySession();
    }
}

async function FlipCard() {
    hasBeenFlipped = true;
    let frontToBack;

    studyCard.classList.add('flip');

    if (!frontOfCard.classList.contains('hidden')) {
        frontOfCard.classList.add('hidden');
        image.classList.add('hidden');
        frontToBack = true;
    }
    if (!backOfCard.classList.contains('hidden')) {
        backOfCard.classList.add('hidden');
        frontToBack = false;
    }

    scoreTracker.classList.remove('hidden');
    studyCard.addEventListener('animationend', e => {
        studyCard.classList.remove('flip');
        scoreTracker.classList.toggle('hide-score');
        if (frontToBack) {
            backOfCard.classList.remove('hidden');
            frontOfCard.classList.add('hidden');
            image.classList.add('hidden');
        } else {
            frontOfCard.classList.remove('hidden');
            image.classList.remove('hidden');
            backOfCard.classList.add('hidden');
        }
    });
}

function CompleteStudySession() {
    studyCard.classList.add('hidden');
    scoreTracker.classList.add('hidden');
    completeSession.querySelector('p.correct').innerText = `You got ${right} question${right === 1 ? '' : 's'} correct.`;
    completeSession.querySelector('p.incorrect').innerText = `You got ${wrong} question${wrong === 1 ? '' : 's'} wrong.`;
    completeSession.classList.remove('hidden');
    endSessionButton.classList.add('hidden');
}

studyCard.addEventListener('click', FlipCard);

correctButton.addEventListener('click', function () {
    ComputeScore(true);
    NextCard();
});

wrongButton.addEventListener('click', function () {
    ComputeScore(false);
    NextCard();
});

studyCard.addEventListener('mouseover', e => {
    if (hasBeenFlipped) {
        scoreTracker.classList.remove('hide-score');
    }
});

scoreTracker.addEventListener('mouseover', e => {
    if (hasBeenFlipped) {
        scoreTracker.classList.remove('hide-score');
    }
});

studyCard.addEventListener('mouseout', e => {
    if (hasBeenFlipped) {
        scoreTracker.classList.add('hide-score');
    }
});

scoreTracker.addEventListener('mouseout', e => {
    if (hasBeenFlipped) {
        scoreTracker.classList.add('hide-score');
    }
});

endSessionButton.addEventListener('click', () => {
    CompleteStudySession();
});