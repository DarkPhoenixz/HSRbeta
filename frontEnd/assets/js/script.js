let character;
let characterValue = 0;

let baseSrc = "./assets/img/character/";
let baseSrcTerminator = ".webp";

character = document.querySelector("#character");
console.log(character);

character.addEventListener("change", function () {
    document.querySelector("img").src = baseSrc + character.value + baseSrcTerminator;
    displayPreview();
});

function displayPreview(){
    let previewPlaceholder = document.querySelector(".previewPlaceholder");
    let style = window.getComputedStyle(previewPlaceholder);
    if (style.display === "flex") {
        previewPlaceholder.style.display = "none";
        
        let preview = document.querySelector(".preview");
        preview.style.display = "flex";

        setTimeout(() => {
            preview.style.opacity = 1;
        }, 100);
    }
}
