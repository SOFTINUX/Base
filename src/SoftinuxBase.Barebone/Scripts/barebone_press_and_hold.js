'use strict';

// vanilla version of https://github.com/santhony7/pressAndHold/blob/master/jquery.pressAndHold.js
// See also: https://www.kirupa.com/html5/press_and_hold.htm -- better pattern, more modern...

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
Barebone = {};

Barebone.pressAndHold = function (htmlElement, options) {
    this._pressedHtmlElement = htmlElement;
    // default values
    this._settings = {
        holdTime: 700,
        progressIndicatorRemoveDelay: 300,
        progressIndicatorColor: "#ff0000",
        progressIndicatorOpacity: 0.6

    };
    // use parameter options
    Object.assign(this._settings, options);

    // other internal variables
    this._timer = null;
    this._decaCounter = null;
    this._progressIndicatorHTML = null;
    this.init();
}

Barebone.pressAndHold.prototype._startPressAndHold = function() {
    this._pressedHtmlElement.dispatchEvent(new CustomEvent("start.pressAndHold"));
    this._timer = setInterval(function() {
        decaCounter += 10;
        this._pressedHtmlElement.querySelectorAll(".holdButtonProgress")[0].css.left = ((decaCounter / _this.settings.holdTime) * 100 - 100) + "%";
        if (decaCounter == _this.settings.holdTime) {
            _this.exitTimer(this._timer);
            this._pressedHtmlElement.dispatchEvent(new CustomEvent("complete.pressAndHold"));
        }
    }, 10);
}

Barebone.pressAndHold.prototype.init = function() {
    console.log('init');

    // Style pressed html element
    this._pressedHtmlElement.display = "block";
    this._pressedHtmlElement.overflow = "hidden";
    this._pressedHtmlElement.position = "relative";

    this._progressIndicatorHTML = '<div class="holdButtonProgress" style="height: 100%; width: 100%; position: absolute; top: 0; left: -100%; background-color:' + this.settings.progressIndicatorColor + '; opacity:' + this.settings.progressIndicatorOpacity + ';"></div>';

    this._pressedHtmlElement.insertAdjacentHTML('afterbegin', this._progressIndicatorHTML);

    var _this = this;

    this._pressedHtmlElement.addEventListener('mouseup', event_ => {
        _this.exitTimer(_this._timer);
    });

    this._pressedHtmlElement.addEventListener('mouseleave', event_ => {
        _this.exitTimer(_this._timer);
    });

    this._pressedHtmlElement.addEventListener('touchend', event_ => {
        _this.exitTimer(_this._timer);
    });

    this._pressedHtmlElement.addEventListener('mousedown', event_ => {
        if (event_.button != 2) {
            _this._startPressAndHold();
         }
    });

    this._pressedHtmlElement.addEventListener('touchstart', event_ => {
        _this._startPressAndHold();
    });
}

Barebone.pressAndHold.prototype.exitTimer = function(timer) {
    var _this = this;
    clearTimeout(this._timer);
    this._pressedHtmlElement.removeEventListener('mouseleave');
    this._pressedHtmlElement.removeEventListener('touchend');

    setTimeout(function() {
       document.querySelectorAll(".holdButtonProgress")[0].css.left = "-100%";
       this._pressedHtmlElement.dispatchEvent(new CustomEvent("complete.pressAndHold"));
    }, this._settings.progressIndicatorRemoveDelay);
}
