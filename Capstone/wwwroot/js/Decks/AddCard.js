let tagCount = 1;
const tagDiv = document.querySelector('#tags');
const firstTag = document.querySelector('#Tags_0_');
const addTagButton = document.querySelector('#AddNewTag');
const getFrontTextbox = document.getElementById('front-text').form[0];
const getImageTextbox = document.getElementById('front-text').form[1];
const frontTextboxPlaceholder = getFrontTextbox.placeholder;
const imageTextboxPlaceholder = getImageTextbox.placeholder;

addTagButton.addEventListener('click', e => {
    console.log('typed');
    const newTag = document.createElement('input');
    newTag.className = 'form-control';
    newTag.type = 'text';
    newTag.id = `Tags_${tagCount}_`;
    newTag.name = `Tags[${tagCount}]`;
    
    tagDiv.appendChild(newTag);
    tagCount++;
    //newTag.addEventListener('oninput', AddNewTag());
});

getFrontTextbox.addEventListener('input', i => {
    if (getFrontTextbox.value !== '') {
        getImageTextbox.disabled = true
        getImageTextbox.placeholder = "You can only have an image OR text"
    }
    else {
        getImageTextbox.disabled = false
        getImageTextbox.placeholder = imageTextboxPlaceholder
    }
});

getImageTextbox.addEventListener('input', i => {
    if (getImageTextbox.value !== '') {
        
        getFrontTextbox.disabled = true
        getFrontTextbox.placeholder = "You can only have an image OR text"
    }
    else {
        getFrontTextbox.disabled = false
        getFrontTextbox.placeholder = frontTextboxPlaceholder
    }
});

