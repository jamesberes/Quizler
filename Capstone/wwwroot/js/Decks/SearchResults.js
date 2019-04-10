let hiddenInputs = document.querySelectorAll(".hidden-cardId");
hiddenInputs.forEach(input => {
    let cardId = input.parentElement.parentElement.parentElement.querySelector(".td-cardId").innerText
    input.value = cardId;
})