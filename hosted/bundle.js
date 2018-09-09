"use strict";

//menu buttons
var aboutMenuButton = void 0;
var portfolioMenuButton = void 0;
var contactMenuButton = void 0;
var resumeMenuButton = void 0;

//portfolio buttons
var clickButton = void 0;
var bumpButton = void 0;
var showdownButton = void 0;
var dynamiteButton = void 0;
var idleButton = void 0;

//hide current content
var hideContent = function hideContent() {
    var content = document.getElementsByClassName("content");
    for (var i = 0; i < content.length; i++) {
        content[i].style.display = "none";
    }
    hideProject();
};

//erase all underlines
var eraseUnderlines = function eraseUnderlines() {
    var buttons = document.getElementsByClassName("contentButton");
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].style.textDecoration = "none";
    }
};

//hide current project
var hideProject = function hideProject() {
    var portfolioPiece = document.getElementsByClassName("portfolioPiece");
    for (var i = 0; i < portfolioPiece.length; i++) {
        portfolioPiece[i].style.display = "none";
    }
};

var init = function init() {
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
    aboutMenuButton.onclick = function (e) {
        hideContent();
        eraseUnderlines();
        aboutMenuButton.style.textDecoration = "underline";
        document.querySelector("#AboutContent").style.display = "block";
    };
    portfolioMenuButton.onclick = function (e) {
        hideContent();
        eraseUnderlines();
        portfolioMenuButton.style.textDecoration = "underline";
        document.querySelector("#PortfolioContent").style.display = "block";
    };
    contactMenuButton.onclick = function (e) {
        hideContent();
        eraseUnderlines();
        contactMenuButton.style.textDecoration = "underline";
        document.querySelector("#ContactContent").style.display = "block";
    };
    resumeMenuButton.onclick = function (e) {
        hideContent();
        eraseUnderlines();
        resumeMenuButton.style.textDecoration = "underline";
        document.querySelector("#ResumeContent").style.display = "block";
    };

    clickButton.onclick = function (e) {
        hideProject();
        document.querySelector("#portfolioClick").style.display = "block";
    };
    bumpButton.onclick = function (e) {
        hideProject();
        document.querySelector("#portfolioBump").style.display = "block";
    };
    showdownButton.onclick = function (e) {
        hideProject();
        document.querySelector("#portfolioShowdown").style.display = "block";
    };
    dynamiteButton.onclick = function (e) {
        hideProject();
        document.querySelector("#portfolioDynamite").style.display = "block";
    };
    idleButton.onclick = function (e) {
        hideProject();
        document.querySelector("#portfolioIdle").style.display = "block";
    };
};

window.onload = init;
