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
const randomOrderCheckbox = document.querySelector('#random-checkbox');
const sequentialOrderCheckbox = document.querySelector('#sequential-checkbox');
const startStudySessionButton = document.querySelector('#start-session-btn');
const normalStudyModeBtn = document.querySelector('#normal-mode-btn');
const lightningStudyModeBtn = document.querySelector('#lightning-mode-button');
const setTimeForm = document.querySelector('form.set-time');
const minutesInput = document.querySelector('#minutes');
const countdownClock = document.querySelector('#countdown-clock');


let unansweredQuestions = [];
let answeredQuestions = [];
let hasBeenFlipped = false;
let random = false;
let lightningRound = false;
let cardTime = 0;
let intervalId;

sequentialOrderButton.style.backgroundColor = '#e6e6e6';
normalStudyModeBtn.style.backgroundColor = '#e6e6e6';


fetch(`${apiUrl}getdeck?id=${deckId}`)
    .then(response => {
        response.json()
            .then(data => {
                unansweredQuestions = data.cards;

                randomOrderButton.addEventListener('click', e => {
                    random = true;
                    randomOrderButton.style.backgroundColor = '#e6e6e6';
                    sequentialOrderButton.style.backgroundColor = '#fff';
                });

                sequentialOrderButton.addEventListener('click', e => {
                    random = false;
                    sequentialOrderButton.style.backgroundColor = '#e6e6e6';
                    randomOrderButton.style.backgroundColor = '#fff';
                });

                normalStudyModeBtn.addEventListener('click', e => {
                    lightningRound = false;
                    normalStudyModeBtn.style.backgroundColor = '#e6e6e6';
                    lightningStudyModeBtn.style.backgroundColor = '#fff';
                    setTimeForm.classList.add('hidden');
                });

                lightningStudyModeBtn.addEventListener('click', e => {
                    lightningRound = true;
                    lightningStudyModeBtn.style.backgroundColor = '#e6e6e6';
                    normalStudyModeBtn.style.backgroundColor = '#fff';
                    setTimeForm.classList.remove('hidden');
                });

                minutesInput.addEventListener('change', e => {
                    cardTime = parseInt(minutesInput.value);
                });

                startStudySessionButton.addEventListener('click', e => {
                    DisplayFirstCard();
                    scoreTracker.classList.remove('hidden');
                });
            });
    });

function DisplayFirstCard() {
    if (random) {
        unansweredQuestions = shuffle(unansweredQuestions);
    }

    if (lightningRound && cardTime > 0) {
        startTimer();
    }

    studyModeDiv.classList.add('fade-out-animation');
    studyModeDiv.addEventListener('animationend', e => {
        studyModeDiv.classList.add('hidden');
        studyCard.classList.remove('hidden');
    });

    if (unansweredQuestions[0].imageURL != '') {
        image.src = unansweredQuestions[0].imageURL;
        frontOfCard.innerText = '';
    } else {
        frontOfCard.innerText = unansweredQuestions[0].front;
        image.src = '';

    }
    backOfCard.innerText = unansweredQuestions[0].back;

}

function startTimer() {
    let timer = cardTime;
    intervalId = setInterval(() => {
        let minutes = Math.floor(timer / 60);
        let seconds = Math.floor(timer % 60);

        let minutesDisplay = minutes < 10 ? `0${minutes}` : minutes;
        let secondsDisplay = seconds < 10 ? `0${seconds}` : seconds;

        countdownClock.innerText = `${minutesDisplay}:${secondsDisplay}`;

        if (timer > 0) {
            timer--;
        }
        else {
            clearInterval(intervalId);
            wrong++;
            NextCard();
            if (unansweredQuestions.length > 0) {
                startTimer();
            }
        }
    }, 1000);
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
        clearInterval(intervalId);
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

    studyCard.addEventListener('animationend', e => {
        studyCard.classList.remove('flip');
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
    clearInterval(intervalId);
    studyCard.classList.add('hidden');
    scoreTracker.classList.add('hidden');
    completeSession.querySelector('p.correct').innerText = `You got ${right} question${right === 1 ? '' : 's'} correct.`;
    completeSession.querySelector('p.incorrect').innerText = `You got ${wrong} question${wrong === 1 ? '' : 's'} wrong.`;
    completeSession.classList.remove('hidden');
    endSessionButton.classList.add('hidden');
    studyModeDiv.classList.add('hidden');
}

studyCard.addEventListener('click', FlipCard);

correctButton.addEventListener('click', function () {
    ComputeScore(true);
    NextCard();
    clearInterval(intervalId);
    if (lightningRound && cardTime > 0) {
        startTimer();
    }
});

wrongButton.addEventListener('click', function () {
    ComputeScore(false);
    NextCard();
    clearInterval(intervalId);
    if (lightningRound && cardTime > 0) {
        startTimer();
    }
});

endSessionButton.addEventListener('click', () => {
    CompleteStudySession();
});