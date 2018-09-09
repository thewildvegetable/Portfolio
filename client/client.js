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

//erase all underlines
const eraseUnderlines = () => {
    let buttons = document.getElementsByClassName("contentButton");
    for (let i = 0; i < buttons.length; i++){
        buttons[i].style.textDecoration = "none";
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
  contactMenuButton = document.querySelector("#Contact");
  resumeMenuButton = document.querySelector("#Resume");
  clickButton = document.querySelector("#projectClick");
  bumpButton = document.querySelector("#projectBump");
  showdownButton = document.querySelector("#projectShowdown");
  dynamiteButton = document.querySelector("#projectDynamite");
  idleButton = document.querySelector("#projectIdle");
    
  //set event listeners
  aboutMenuButton.onclick = (e) => {
      hideContent();
      eraseUnderlines();
      aboutMenuButton.style.textDecoration = "underline";
      document.querySelector("#AboutContent").style.display = "block";
  };
  portfolioMenuButton.onclick = (e) => {
      hideContent();
      eraseUnderlines();
      portfolioMenuButton.style.textDecoration = "underline";
      document.querySelector("#PortfolioContent").style.display = "block";
  };
  contactMenuButton.onclick = (e) => {
      hideContent();
      eraseUnderlines();
      contactMenuButton.style.textDecoration = "underline";
      document.querySelector("#ContactContent").style.display = "block";
  };
  resumeMenuButton.onclick = (e) => {
      hideContent();
      eraseUnderlines();
      resumeMenuButton.style.textDecoration = "underline";
      document.querySelector("#ResumeContent").style.display = "block";
  };
    
  clickButton.onclick = (e) => {
      hideProject();
      document.querySelector("#portfolioClick").style.display = "block";
  };
  bumpButton.onclick = (e) => {
      hideProject();
      document.querySelector("#portfolioBump").style.display = "block";
  };
  showdownButton.onclick = (e) => {
      hideProject();
      document.querySelector("#portfolioShowdown").style.display = "block";
  };
  dynamiteButton.onclick = (e) => {
      hideProject();
      document.querySelector("#portfolioDynamite").style.display = "block";
  };
  idleButton.onclick = (e) => {
      hideProject();
      document.querySelector("#portfolioIdle").style.display = "block";
  };
};

window.onload = init;