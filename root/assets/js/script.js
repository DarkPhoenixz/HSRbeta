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

character.addEventListener("change", function () {
    document.querySelector("img").src = baseSrc + character.value + baseSrcTerminator;
    displayAll();
});



let switchButton = document.querySelector(".switch");
switchButton.addEventListener("click", function () {
    let button = document.querySelector(".switchButton");
    let style = window.getComputedStyle(button);
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
let subCount = 1;
addButton.addEventListener("click", function (e) {
    e.stopPropagation();
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
        subCount++;
    });
    select.addEventListener("change", function () {
        currentOpt = select.options[select.selectedIndex].text;
        let value = slider.value * stepsJson[currentOpt];
        val.innerHTML = value.toFixed(2);
        codeDisp(this.name, this.value, this);
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
        val.innerHTML = value.toFixed(2); 
        

        codeDisp("slider", this.value, this);
    });
    
    value.appendChild(val);
    slidecontainer.appendChild(value);

    sub.appendChild(slidecontainer);

    let organizer = document.querySelector(".subOrganizer");
    organizer.appendChild(sub);
});



let lightconeInput = document.querySelector("#lightcones");

lightconeInput.addEventListener("change", function () {
    
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

var codes = [];
function codeDisp(name, value, el){
    
    if (value === "") {
        return;
    }

    if (name === "character") {
        codes = ["var XHead = new BattleRelic {}\n", "var XHands = new BattleRelic {}\n", "var XBody = new BattleRelic {}\n", "var XFeet = new BattleRelic {}\n", "var XSphere = new BattleRelic {}\n", "var XRope = new BattleRelic {}\n"];
        document.querySelectorAll("select").forEach((el, index) => { if (index == 0) return; el.selectedIndex = 0; document.querySelector(".subOrganizer").innerHTML = ""; selectCount = "a";});
        document.querySelector(".lc").style.opacity = "1";

        let charName = el.options[el.selectedIndex].text;
        charName = charName.replace(" ", "");

        let code = document.querySelector(".relics");
        code.innerHTML = "";

        for (let i = 0; i < 6; i++) {
            let sample = codes[i];
            sample = sample.replace("X", charName);
            codes[i] = sample;
            code.innerHTML += sample + "\n";
        }
    }

    if (name === "lightcones") {
        document.querySelector(".lcVal").innerHTML = value; 
    }

    if (name === "setFull") {
        let code = document.querySelector(".relics");
        code.innerHTML =  "";

        
        for (let i = 0; i < 4; i++) {
            //console.log(codes[i].match(/{([^}]*)}/)[1]);
            //console.log(codes[i]);
            if (codes[i].includes("Id")) {
                let sample = codes[i];
                sample = sample.split("= ");
                
                sample[2] = sample[2].split(",");
                
                sample[2][0] = Number(value) + i;
                
                if (i == 0 || i == 1) sample = sample[0] + "= " + sample[1] + "= " + sample[2][0] + "," + sample[2][1] + "= " + sample[3] + "= " + sample[4] ;
                else sample = sample[0] + "= " + sample[1] + "= " + sample[2][0] + "," + sample[2][1] + "= " + sample[3] ;
                codes[i] = sample;                
            }

            else {
                let sample = codes[i];
                if (i == 0 || i == 1) {
                    sample = sample.replace("{}",
"Relic {\n\
    Id = " + (Number(value) + i) + ",\n\
    Level = 15,\n\
    MainAffixId = 1,\n\
    SubAffixLists = {}\n\
}");
                }
                else {
                    sample = sample.replace("{}", 
"Relic {\n\
    Id = " + (Number(value) + i) + ",\n\
    Level = 15\n\
}");
                        
                }
                codes[i] = sample;
            }
        }
        codes.forEach((code) => {
            document.querySelector(".relics").innerHTML += code + "\n";
        });    
    }

    if (name === "setHalf") {
        document.querySelector(".halfSetVal").innerHTML = value;
    }

    if (name === "ornaments") {
        let code = document.querySelector(".relics");
        code.innerHTML =  "";

        for (let i = 4; i < 6; i++) {
            if (codes[i].includes("Id")) {
                let sample = codes[i];
                sample = sample.split("= ");
                
                sample[2] = sample[2].split(",");
                
                sample[2][0] = Number(value) + (i -4);
               
                if (i == 0 || i == 1) sample = sample[0] + "= " + sample[1] + "= " + sample[2][0] + "," + sample[2][1] + "= " + sample[3] + "= " + sample[4];
                else sample = sample[0] + "= " + sample[1] + "= " + sample[2][0] + "," + sample[2][1] + "= " + sample[3];
                codes[i] = sample;
            }

            else {
                let sample = codes[i];
                sample = sample.replace("{}", 
"Relic {\n\
    Id = " + (Number(value) + (i - 4)) + ",\n\
    Level = 15\n\
}");       
                codes[i] = sample;
            }
        }
        codes.forEach((code) => {
            document.querySelector(".relics").innerHTML += code + "\n";
        });
        
    }

    if (name === "bodyMain") {
        let sample = codes[2];
        if (sample.includes("MainAffixId")) {
            sample = sample.split("= ");
            sample[4] = sample[4].split("\n");
            sample[4][0] = value;
            sample[4] = sample[4].join("\n");
            sample = sample.join("= ");
            codes[2] = sample;
            console.log(sample);
        }
        else {
            sample = sample.split("\n");
            sample[2] = sample[2].concat(",\n\
    MainAffixId = " + value + ",\n\    SubAffixLists = {}");
            sample = sample.join("\n");
            codes[2] = sample;
        
               
        }
        document.querySelector(".relics").innerHTML = "";
        codes.forEach((code) => {
            document.querySelector(".relics").innerHTML += code + "\n";
        });
    }

    if (name === "feetMain") {
        let sample = codes[3];
        if (sample.includes("MainAffixId")) {
            sample = sample.split("= ");
            sample[4] = sample[4].split("\n");
            sample[4][0] = value;
            sample[4] = sample[4].join("\n");
            sample = sample.join("= ");
            codes[3] = sample;
            console.log(sample);
        }
        else {
        sample = sample.split("\n");
        sample[2] = sample[2].concat(",\n\
    MainAffixId = " + value + ",\n\    SubAffixLists = {}");
        sample = sample.join("\n");
        codes[3] = sample;
        console.log(sample);
        }

        document.querySelector(".relics").innerHTML = "";
        codes.forEach((code) => {
            document.querySelector(".relics").innerHTML += code + "\n";
        });
    }
    if (name === "sphereMain") {
        let sample = codes[4];
        if (sample.includes("MainAffixId")) {
            sample = sample.split("= ");
            sample[4] = sample[4].split("\n");
            sample[4][0] = value;
            sample[4] = sample[4].join("\n");
            sample = sample.join("= ");
            codes[4] = sample;
            console.log(sample);
        }
        else {
        sample = sample.split("\n");
        sample[2] = sample[2].concat(",\n\
    MainAffixId = " + value + ",\n\    SubAffixLists = {}");
        sample = sample.join("\n");
        codes[4] = sample;
        console.log(sample);
        }

        document.querySelector(".relics").innerHTML = "";
        codes.forEach((code) => {
            document.querySelector(".relics").innerHTML += code + "\n";
        });
    }
    if (name === "ropeMain") {
        let sample = codes[5];
        if (sample.includes("MainAffixId")) {
            sample = sample.split("= ");
            sample[4] = sample[4].split("\n");
            sample[4][0] = value;
            sample[5] = sample[4].join("\n");
            sample = sample.join("= ");
            codes[3] = sample;
            console.log(sample);
        }
        else {
        sample = sample.split("\n");
        sample[2] = sample[2].concat(",\n\
    MainAffixId = " + value + ",\n\    SubAffixLists = {}");
        sample = sample.join("\n");
        codes[5] = sample;
        console.log(sample);
        }

        document.querySelector(".relics").innerHTML = "";
        codes.forEach((code) => {
            document.querySelector(".relics").innerHTML += code + "\n";
        });
    }
    
    console.log(name);
    if (name === "slider") {
        let code = document.querySelector(".relics");
        code.innerHTML =  "";
        if (value === "") {
            return;
        }
        if (codes[0].includes("Step")) {
            let sample = codes[0];
            sample = sample.split("= ");
            sample[8] = sample[8].split("\n");
            sample[8][0] = value;
            sample[8] = sample[8].join("\n");
            sample = sample.join("= ");
            codes[0] = sample;
            console.log(sample);
        }
        else { 
            let sample = codes[0];
            let id = el.parentElement.previousElementSibling.options[el.parentElement.previousElementSibling.selectedIndex].value;
            sample = sample.replace("{}", 
"{\n\
        new RelicAffix = {\n\
            AffixId = " + id + ",\n\
            Step = " + value + "\n\
        }");
            codes[0] = sample; 
        }       

        codes.forEach((code) => {
            document.querySelector(".relics").innerHTML += code + "\n";
        });
    }

    if (name.includes("sub")) {
        let code = document.querySelector(".relics");
        code.innerHTML =  "";

        if (codes[0].includes("Step")) {
            let sample = codes[0];
            sample = sample.split("= ");
            sample[7] = sample[7].split(",");
            sample[7][0] = value;
            sample[7] = sample[7].join(",");
            sample = sample.join("= ");
            codes[0] = sample;
            
        }

        codes.forEach((code) => {
            document.querySelector(".relics").innerHTML += code + "\n";
        });
    }
}