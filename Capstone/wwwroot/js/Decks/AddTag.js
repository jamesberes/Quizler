const cardId = document.querySelector('#card-id').value;
const deckId = document.querySelector('#deck-id').value;
let allInputs = document.querySelectorAll('.tag');
const form = document.querySelector('form');
const submitButton = document.querySelector('[type=submit');

allInputs[0].addEventListener('keyup', AddNewTagField);

submitButton.addEventListener('click', e => {
    e.preventDefault();
    let output = {tags: []};

    allInputs.forEach(node => {
        output.tags.push({ name: node.value, cardid: cardId });
    });

    PushToAPI(output);
});

function AddNewTagField(event) {
    GetAllInputs();

    if (allInputs[allInputs.length - 1].value != null && allInputs[allInputs.length - 1].value != '') {
        let div = document.createElement('div');
        div.classList.add('form-group');
        div.innerHTML = '<label class="control-label">Name</label><input class="form-control tag" />';

        form.insertBefore(div, form.querySelector('div.hidden-input'));
        div.focus();
        div.addEventListener('keyup', AddNewTagField);
    }
}

function GetAllInputs() {
    allInputs = document.querySelectorAll('.tag');
}

function PushToAPI(tags) {
    console.log('enter method')
    fetch(`http://localhost:${location.port}/API/AddTags`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json' 
        },
        body: JSON.stringify(tags)
    })
        .then(response => {
            console.log(response);
            if (response.ok) {
                window.location.replace(`http://localhost:${location.port}/Decks/UpdateCard?cardId=${cardId}`);
            }
            else {
                alert('Whoops. Something went wrong! Please try again.');
            }
        })
}

//<div class="form-group">
//    <label asp-for="Name" class="control-label"></label>
//    <input asp-for="Name" class="form-control" />
//    <span asp-validation-for="Name" class="text-danger"></span>
//</div>