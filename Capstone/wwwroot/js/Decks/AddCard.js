let tagCount = 1;
const tagDiv = document.querySelector('#tags');
const firstTag = document.querySelector('#Tags_0_');
const addTagButton = document.querySelector('#AddNewTag');
const getFrontTextbox = document.getElementById('front-text').form[0];
const getImageTextbox = document.getElementById('front-text').form[1];
const form = document.getElementById('add-card-form');
const frontTextboxPlaceholder = getFrontTextbox.placeholder;
const imageTextboxPlaceholder = getImageTextbox.placeholder;
const submitButton = document.querySelector('[type=submit]');

getFrontTextbox.addEventListener('input', i => {
    if (getFrontTextbox.value !== '') {
        getImageTextbox.disabled = true
        getImageTextbox.placeholder = "You can only have an image OR text"
        submitButton.disabled = false;
    }
    else {
        getImageTextbox.disabled = false
        getImageTextbox.placeholder = imageTextboxPlaceholder
        submitButton.disabled = true;
    }
});

getImageTextbox.addEventListener('input', i => {
    if (getImageTextbox.value !== '') {

        getFrontTextbox.disabled = true
        getFrontTextbox.placeholder = "You can only have an image OR text"
        submitButton.disabled = false;
    }
    else {
        getFrontTextbox.disabled = false
        getFrontTextbox.placeholder = frontTextboxPlaceholder
        submitButton.disabled = true;
    }
});
