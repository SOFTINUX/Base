'use strict';

// vanilla version of https://github.com/santhony7/pressAndHold/blob/master/jquery.pressAndHold.js
// See also: https://www.kirupa.com/html5/press_and_hold.htm -- better pattern, more modern...

 // Variables of this module
 let _pressedHtmlElement = undefined;
 // default values
 let _settings = {
    holdTime: 700,
    progressIndicatorRemoveDelay: 300,
    progressIndicatorColor: "#ff0000",
    progressIndicatorOpacity: 0.6
};
let _timer = null;
let _decaCounter = null;
let _progressIndicatorHTML = null;

/*
 * @param {HTMLElement} - the html element that will be pressed and hold
 * @param { { holdTime: int, progressIndicatorRemoveDelay: int, progressIndicatorColor: string, progressIndicatorOpacity: number} }
 *        options, default values: {
			holdTime: 700,
			progressIndicatorRemoveDelay: 300,
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

function _startPressAndHold() {
    _pressedHtmlElement.dispatchEvent(new CustomEvent("start.pressAndHold"));
    _timer = setInterval(function() {
        _decaCounter += 10;
        _pressedHtmlElement.querySelectorAll(".holdButtonProgress")[0].css.left = ((_decaCounter / _settings.holdTime) * 100 - 100) + "%";
        if (_decaCounter == _settings.holdTime) {
            exitTimer(_timer);
            _pressedHtmlElement.dispatchEvent(new CustomEvent("complete.pressAndHold"));
        }
    }, 10);
}

function init() {
    console.log('init');

    // Style pressed html element
    _pressedHtmlElement.display = "block";
    _pressedHtmlElement.overflow = "hidden";
    _pressedHtmlElement.position = "relative";

    _progressIndicatorHTML = '<div class="holdButtonProgress" style="height: 100%; width: 100%; position: absolute; top: 0; left: -100%; background-color:' + _settings.progressIndicatorColor + '; opacity:' + _settings.progressIndicatorOpacity + ';"></div>';

    _pressedHtmlElement.insertAdjacentHTML('afterbegin', _progressIndicatorHTML);

    _pressedHtmlElement.addEventListener('mouseup', event_ => {
        exitTimer(_timer);
    });

    _pressedHtmlElement.addEventListener('mouseleave', event_ => {
        exitTimer(_timer);
    });

    _pressedHtmlElement.addEventListener('touchend', event_ => {
        exitTimer(_timer);
    });

    _pressedHtmlElement.addEventListener('mousedown', event_ => {
        if (event_.button != 2) {
            _startPressAndHold();
         }
    });

    _pressedHtmlElement.addEventListener('touchstart', event_ => {
        _startPressAndHold();
    });
}

function exitTimer(timer) {
    clearTimeout(timer);
    _pressedHtmlElement.removeEventListener('mouseleave');
    _pressedHtmlElement.removeEventListener('touchend');

    setTimeout(function() {
       document.querySelectorAll(".holdButtonProgress")[0].css.left = "-100%";
       _pressedHtmlElement.dispatchEvent(new CustomEvent("complete.pressAndHold"));
    }, _settings.progressIndicatorRemoveDelay);
}
