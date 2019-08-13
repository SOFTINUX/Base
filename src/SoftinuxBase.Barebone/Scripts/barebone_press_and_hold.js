// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

'use strict';

// Credits: https://www.kirupa.com/html5/press_and_hold.htm (modern pattern)

// Variables of this module
let _pressedHtmlElement = undefined;
// default values
let _settings = {
    holdTime: 700,
    progressIndicatorColor: "#ff0000",
    progressIndicatorOpacity: 0.6
};
let _timerID = null;
let _counter = 0;
let _progressIndicatorHTML = null;
let _pressHoldEvent = new CustomEvent("pressHold");

/*
 * @param {HTMLElement} - the html element that will be pressed and hold
 * @param { { holdTime: int, progressIndicatorColor: string, progressIndicatorOpacity: number} }
 *        options, default values: {
			holdTime: 700,
			progressIndicatorColor: "#ff0000",
			progressIndicatorOpacity: 0.6

		}
 */
export default function pressAndHold(htmlElement, options) {
    // set pressed element
    _pressedHtmlElement = htmlElement;

    // override settings with parameter options, if any applicable
    Object.assign(_settings, options);

    init();
}

function init() {
    console.log('init');

    // Style pressed html element
    _pressedHtmlElement.style.display = "block";
    _pressedHtmlElement.style.overflow = "hidden";
    _pressedHtmlElement.style.position = "relative";

    _progressIndicatorHTML = '<div class="holdButtonProgress" style="height: 100%; width: 100%; position: absolute; top: 0; left: -100%; background-color:' + _settings.progressIndicatorColor + '; opacity:' + _settings.progressIndicatorOpacity + ';"></div>';

    _pressedHtmlElement.insertAdjacentHTML('beforeend', _progressIndicatorHTML);

    // Listening for the mouse and touch events
    _pressedHtmlElement.addEventListener("mousedown", pressingDown, false);
    _pressedHtmlElement.addEventListener("mouseup", notPressingDown, false);
    _pressedHtmlElement.addEventListener("mouseleave", notPressingDown, false);

    _pressedHtmlElement.addEventListener("touchstart", pressingDown, false);
    _pressedHtmlElement.addEventListener("touchend", notPressingDown, false);

    // Listening for our custom pressHold event
    _pressedHtmlElement.addEventListener("pressHold", doSomething, false);
}

function pressingDown(e) {
    // Start the timer
    requestAnimationFrame(timerFunc);

    e.preventDefault();

    //console.log("Pressing!");
}

function notPressingDown(e) {
    // Stop the timer
    cancelAnimationFrame(_timerID);
    _counter = 0;
    _pressedHtmlElement.querySelectorAll(".holdButtonProgress")[0].style.left = "-100%";

    //console.log("Not pressing!");
}

//
// Runs at 60fps when you are pressing down.
//
function timerFunc() {
    //console.log("Timer tick!");

    if (_counter < (_settings.holdTime * 60/1000)) {
        _timerID = requestAnimationFrame(timerFunc);
        _counter++;

        _pressedHtmlElement.querySelectorAll(".holdButtonProgress")[0].style.left = ((_counter / (_settings.holdTime * 60/1000)) * 100 - 100) + "%";

    } else {
        //console.log("Press threshold reached!");
        _pressedHtmlElement.dispatchEvent(_pressHoldEvent);
    }
}

function doSomething(e) {
    console.log("pressHold event fired!");
}

