const getImageTextbox = document.getElementById('image-text').form[0];
const getFrontTextbox = document.getElementById('front-text').form[1];
const form = document.getElementById('edit-card-form');
const frontTextboxPlaceholder = getFrontTextbox.placeholder;
const imageTextboxPlaceholder = getImageTextbox.placeholder;
const submitButton = document.querySelector('[type=submit]');
const placeholderText = "You can only have an image OR text";
let originalFrontTextboxValue = getFrontTextbox.value;
let originalImageTextboxValue = getImageTextbox.value;

if (originalImageTextboxValue) {
    getFrontTextbox.disabled = true;
    getFrontTextbox.placeholder = placeholderText;
}

if (originalFrontTextboxValue) {
    getImageTextbox.disabled = true;
    getImageTextbox.placeholder = placeholderText;
}

getFrontTextbox.addEventListener('input', i => {
    if (getFrontTextbox.value !== '') {
        getImageTextbox.disabled = true;
        getImageTextbox.placeholder = placeholderText;
    }
    else {
        getImageTextbox.disabled = false
        if (originalImageTextboxValue) {
            getImageTextbox.value = originalImageTextboxValue;
        }
        else {
            getImageTextbox.placeholder = imageTextboxPlaceholder
        }
    }
});

getImageTextbox.addEventListener('input', i => {
    if (getImageTextbox.value !== '') {
        getFrontTextbox.disabled = true
        getFrontTextbox.placeholder = placeholderText;
    }
    else {
        getFrontTextbox.disabled = false
        if (originalFrontTextboxValue) {
            getFrontTextbox.value = originalFrontTextboxValue;
        }
        else {
            getFrontTextbox.placeholder = placeholderText;
        }
    }
});
