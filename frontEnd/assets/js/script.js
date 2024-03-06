let character;
let characterValue = 0;

let baseSrc = "./assets/img/character/";
let baseSrcTerminator = ".webp";
var count = 0;
document.addEventListener("DOMContentLoaded", function () {
    document.querySelector(".organizer").style.opacity = "1";
    document.querySelectorAll("select")[0].style.opacity = "1";
    document.querySelectorAll("select")[1].style.opacity = "0";
});


character = document.querySelector("#character");
console.log(character);

character.addEventListener("change", function () {
    document.querySelector("img").src = baseSrc + character.value + baseSrcTerminator;
    displayAll();
    console.log(baseSrc + character.value + baseSrcTerminator);
});



let switchButton = document.querySelector(".switch");
switchButton.addEventListener("click", function () {
    let button = document.querySelector(".switchButton");
    let style = window.getComputedStyle(button);
    console.log(style.left);
    if (style.left == "0px") {
        button.style.left = "25px";
        document.querySelector("#setHalf").style.display = "block";
    }
    else {
        button.style.left = "0px";
        document.querySelector("#setHalf").style.display = "none";
    }
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

function displayAll(){
    document.querySelectorAll("select")[1].style.opacity = "1";
    document.querySelector(".organizerSwitch").style.opacity = "1";
    let inputs = document.querySelectorAll(".organizer");
    inputs.forEach((input) => {
        input.style.opacity = "1";
    });

    let preview = document.querySelector(".previewPlaceholder");
    preview.style.opacity = "0";
    document.querySelector(".preview").style.display = "flex";
    setTimeout(() => {
        console.log(document.querySelector(".preview"));
        
        document.querySelector(".preview").style.opacity = "1";
    }, 200);
}

fetch('./assets/data/relics.json')
  .then(response => response.json())
  .then(data => {
    // Use the data from the JSON file here
    console.log(data);
  })
  .catch(error => {
    console.error('Error:', error);
  });
