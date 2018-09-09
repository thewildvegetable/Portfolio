//menu buttons
let aboutMenuButton;
let portfolioMenuButton;
let contactMenuButton;
let resumeMenuButton;

//portfolio buttons
let clickButton;
let bumpButton;
let showdownButton;
let dynamiteButton;
let idleButton;

//hide current main content
const hideContent = () => {
    let content = document.getElementsByClassName("content");
    for (let i = 0; i < content.length; i++){
        content[i].style.display = "none";
    }
};

//hide current project
const hideProject = () => {
    let portfolioPiece = document.getElementsByClassName("portfolioPiece");
    for (let i = 0; i < portfolioPiece.length; i++){
        portfolioPiece[i].style.display = "none";
    }
};

const init = () => {
  //get buttons
  aboutMenuButton = document.querySelector("#About");
  portfolioMenuButton = document.querySelector("#Portfolio");
  aboutMenuButton = document.querySelector("#AboutContent");
  aboutMenuButton = document.querySelector("#AboutContent");
  aboutMenuButton = document.querySelector("#AboutContent");
  aboutMenuButton = document.querySelector("#AboutContent");
    
  //set event listeners
  lobbyButton.onclick = (e) => {
      document.querySelector("#roomSelection").style.display = "block";
      document.querySelector("#skins").style.display = "none";
  };
  skinButton.onclick = (e) => {
      document.querySelector("#roomSelection").style.display = "none";
      document.querySelector("#skins").style.display = "block";
  };
  buyButton.onclick = purchaseSkin;
  equipButton.onclick = equipSkin;
  closeButton.onclick = (e) => {
      document.querySelector("#unsuccessfulEquip").style.display = "none";
  };

  
};

window.onload = init;