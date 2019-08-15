// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

'use strict';

// Credits: https://www.kirupa.com/html5/press_and_hold.htm (modern pattern)

// Variables of this module
let pressedHtmlElement = undefined;
// default settings
let settings = {
    holdTime: 700,
    progressIndicatorColor: '#ff0000',
    progressIndicatorOpacity: 0.6
};
let timerId = null;
let counter = 0;
let progressIndicatorHtml = null;
let pressHoldEvent = new CustomEvent('pressHold');

/*
 * @param {HTMLElement} htmlElement_ - the html element that will be pressed and hold
 * @param {
 *          {
 *              holdTime: int,
 *              progressIndicatorColor: string,
 *              progressIndicatorOpacity: number
 *          }
 *        } settings_.
 * Default settings:
 *      {
            holdTime: 700,
            progressIndicatorColor: "#ff0000",
            progressIndicatorOpacity: 0.6
        }
 */
export default function pressAndHold(htmlElement_, settings_) {
    // set pressed element
    pressedHtmlElement = htmlElement_;

    // override settings with parameter options, if any applicable
    Object.assign(settings, settings_);

    init();
}

function init() {
    console.log('init');

    // Style pressed html element
    pressedHtmlElement.style.display = 'block';
    pressedHtmlElement.style.overflow = 'hidden';
    pressedHtmlElement.style.position = 'relative';

    progressIndicatorHtml = `<div class="holdButtonProgress" style="height: 100%; width: 100%; position: absolute; top: 0; left: -100%; background-color:${settings.progressIndicatorColor}; opacity:${settings.progressIndicatorOpacity};"></div>`;

    pressedHtmlElement.insertAdjacentHTML('beforeend', progressIndicatorHtml);

    // Listening for the mouse and touch events
    pressedHtmlElement.addEventListener('mousedown', pressingDown, false);
    pressedHtmlElement.addEventListener('mouseup', notPressingDown, false);
    pressedHtmlElement.addEventListener('mouseleave', notPressingDown, false);

    // ReSharper disable Html.EventNotResolved
    pressedHtmlElement.addEventListener('touchstart', pressingDown, false);
    pressedHtmlElement.addEventListener('touchend', notPressingDown, false);

    // Listening for our custom pressHold event
    pressedHtmlElement.addEventListener('pressHold', doSomething, false);
    // ReSharper restore Html.EventNotResolved
}

function pressingDown(e_) {
    // Start the timer
    requestAnimationFrame(timerFunc);

    e_.preventDefault();

    //console.log("Pressing!");
}

function notPressingDown() {
    // Stop the timer
    window.cancelAnimationFrame(timerId);
    counter = 0;
    pressedHtmlElement.querySelectorAll('.holdButtonProgress')[0].style.left = '-100%';

    //console.log("Not pressing!");
}

//
// Runs at 60fps when you are pressing down.
//
function timerFunc() {
    //console.log("Timer tick!");

    if (counter < settings.holdTime * 60 / 1000) {
        timerId = requestAnimationFrame(timerFunc);
        counter++;

        pressedHtmlElement.querySelectorAll('.holdButtonProgress')[0].style.left = counter / (settings.holdTime * 60 / 1000) * 100 - 100 + '%';

    } else {
        //console.log("Press threshold reached!");
        pressedHtmlElement.dispatchEvent(pressHoldEvent);
    }
}

function doSomething() {
    console.log('pressHold event fired!');
}

