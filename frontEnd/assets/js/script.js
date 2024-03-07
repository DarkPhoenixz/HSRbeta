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
    preview.style.display = "none";
    let code = document.querySelector(".code");
    code.style.display = "flex";
    document.querySelector(".preview").style.display = "flex";
    setTimeout(() => {
        console.log(document.querySelector(".preview"));
        
        document.querySelector(".preview").style.opacity = "1";
    }, 200);
}
var relicsJson;
fetch('./assets/data/relics.json')
  .then(response => response.json())
  .then(data => {
    // Use the data from the JSON file here
    console.log(data);
    relicsJson = data;
  })
  .catch(error => {
    console.error('Error:', error);
});

var stepsJson;
fetch('./assets/data/steps.json')
  .then(response => response.json())
  .then(data => {
    // Use the data from the JSON file here
    console.log(data);
    stepsJson = data;
  })
  .catch(error => {
    console.error('Error:', error);
});



let addButton = document.querySelector(".addButton");
let selectCount = "a";
let subCount = 0;
addButton.addEventListener("click", function () {
    let currentOpt = 0;
    if (selectCount === "m") {
        return;
    }
    let sub = document.createElement("div");
    sub.classList.add("sub");

    let select = document.createElement("select");
    select.id = "select_" + selectCount;
    select.name = "sub_" + selectCount;
    selectCount = String.fromCharCode(selectCount.charCodeAt(0) + 1);

    let option = document.createElement("option");
    option.value = "";
    option.innerHTML = "Substat";
    option.disabled = true;
    option.selected = true;
    select.appendChild(option);

    Object.keys(stepsJson).forEach((step) => {
        let option = document.createElement("option");
        option.value = subCount;
        option.innerHTML = step;
        select.appendChild(option);
       
    });
    select.addEventListener("change", function () {
        currentOpt = select.options[select.selectedIndex].text;
        console.log(currentOpt);
        let value = slider.value * stepsJson[currentOpt];
        console.log(stepsJson[currentOpt]);
        val.innerHTML = value.toFixed(2);
    });
    sub.appendChild(select);

    let slidecontainer = document.createElement("div");
    slidecontainer.classList.add("slidecontainer");

    let slider = document.createElement("input");
    slider.type = "range";
    slider.min = "0";
    slider.max = "300";
    slider.step = "10";
    slider.value = "0";
    slider.classList.add("slider");
    slider.id = "myRange";
    
    slidecontainer.appendChild(slider);

    let value = document.createElement("div");
    value.classList.add("value");
    value.innerHTML = "Value: ";
    let val = document.createElement("div");
    val.innerHTML = "0";
    val.classList.add("val");
    slider.addEventListener("input", function () {
        if (currentOpt == 0) {
            return;
        }
        let value = slider.value * stepsJson[currentOpt];
        console.log(stepsJson[currentOpt]);
        val.innerHTML = value.toFixed(2); 
        subCount++;
    });
    value.appendChild(val);
    slidecontainer.appendChild(value);

    sub.appendChild(slidecontainer);

    let organizer = document.querySelector(".subOrganizer");
    organizer.appendChild(sub);
});

let head = "";
let hands = "";
let body = "";
let feet = "";
let sphere = "";
let rope = "";
let lc = "";

let lightconeInput = document.querySelector("#lightcones");

lightconeInput.addEventListener("change", function () {
    document.querySelector(".lc").style.opacity = "1";
    document.querySelector(".lcVal").innerHTML = lightconeInput.value; 
});

let halfSetFlag = false;
document.querySelector(".switch").addEventListener("click", function () {
    halfSetFlag = !halfSetFlag;
    if (halfSetFlag) {
        document.querySelector(".halfSet").addEventListener("change", function () {

        });
    }
    else {
        document.querySelector(".halfSet").removeEventListener("change", function () {});
    }
});

document.querySelector("#")
/*
var jLHead = new BattleRelic
            {
                Id = 61041,
                Level = 15,
                MainAffixId = 1,
                SubAffixLists = {
                    new RelicAffix
                    {
                        AffixId = 8,
                        Step = 60
                    },
                    new RelicAffix
                    {
                        AffixId = 9,
                        Step = 120
                    },
                    new RelicAffix
                    {
                        AffixId = 7,
                        Step = 60
                    },
                    new RelicAffix
                    {
                        AffixId = 5,
                        Step = 30
                    }
                }
            };
                */