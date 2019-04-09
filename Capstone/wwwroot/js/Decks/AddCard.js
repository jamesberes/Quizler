let tagCount = 1;
const tagDiv = document.querySelector('#tags');
const firstTag = document.querySelector('#Tags_0_');
const addTagButton = document.querySelector('#AddNewTag');

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

//function AddNewTag() {
//    console.log('typed');
//    const newTag = document.createElement('input');
//    newTag.className = 'form-control';
//    newTag.type = 'text';
//    newTag.id = `Tags_${tagCount}_`;
//    newTag.name = `Tags[${tagCount}]`;
//    //newTag.addEventListener('oninput', AddNewTag());
//}