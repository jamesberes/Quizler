const getImageTextbox = document.getElementById('image-text').form[0];
const getFrontTextbox = document.getElementById('front-text').form[1];
const form = document.getElementById('edit-card-form');
const getFrontTextboxPlaceholder = getFrontTextbox.placeholder;
const getImageTextboxPlaceholder = getImageTextbox.placeholder;
const submitButton = document.querySelector('[type=submit]');
const placeholderRestriction = "You can only have an image OR text";
const defaultImagePlaceholder = "Image URL...";
const defaultFrontTextPlaceholder = "What is 2 + 2 ?";
let originalFrontTextboxValue = getFrontTextbox.value;
let originalImageTextboxValue = getImageTextbox.value;

if (originalImageTextboxValue) {
    getFrontTextbox.disabled = true;
    getFrontTextbox.placeholder = placeholderRestriction;
}

if (originalFrontTextboxValue) {
    getImageTextbox.disabled = true;
    getImageTextbox.placeholder = placeholderRestriction;
}

getFrontTextbox.addEventListener('input', i => {
    if (getFrontTextbox.value !== '') {
        getImageTextbox.disabled = true;
        getImageTextbox.placeholder = placeholderRestriction;
    }
    else {
        getImageTextbox.disabled = false
        if (originalImageTextboxValue) {
            getImageTextbox.value = originalImageTextboxValue;
        }
        else {
            getImageTextbox.placeholder = defaultImagePlaceholder;
        }
    }
});

getImageTextbox.addEventListener('input', i => {
    if (getImageTextbox.value !== '') {
        getFrontTextbox.disabled = true
        getFrontTextbox.placeholder = placeholderRestriction;
    }
    else {
        getFrontTextbox.disabled = false
        if (originalFrontTextboxValue) {
            getFrontTextbox.value = originalFrontTextboxValue;
        }
        else {
            getFrontTextbox.placeholder = defaultFrontTextPlaceholder;
        }
    }
});
