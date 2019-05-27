'use strict';

// vanilla version of https://github.com/santhony7/pressAndHold/blob/master/jquery.pressAndHold.js

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
function pressAndHold(htmlElement, options) {
    this._pressedHtmlElement = htmlElement;
    // default values
    this._options = {
        holdTime: 700,
        progressIndicatorRemoveDelay: 300,
        progressIndicatorColor: "#ff0000",
        progressIndicatorOpacity: 0.6

    };
    // use parameter options
    Object.assign(this._options, options);

    // other internal variables
    this._timer = null;
    this._decaCounter = null;
    this._progressIndicatorHTML = null;
    this.init();
}

pressAndHold.prototype.init = function() {
    console.log('init');

    // Style pressed html element
    this._pressedHtmlElement.display = "block";
    this._pressedHtmlElement.overflow = "hidden";
    this._pressedHtmlElement.position = "relative";

    this._progressIndicatorHTML = '<div class="holdButtonProgress" style="height: 100%; width: 100%; position: absolute; top: 0; left: -100%; background-color:' + this.settings.progressIndicatorColor + '; opacity:' + this.settings.progressIndicatorOpacity + ';"></div>';

    this._pressedHtmlElement.insertAdjacentHTML('afterbegin', this._progressIndicatorHTML);

    // TODO convert with 2 event listeners, cf https://www.kirupa.com/html5/press_and_hold.htm

    $(this.element).mousedown(function(e) {
        if(e.button != 2){
            $(_this.element).trigger('start.pressAndHold');
            decaCounter = 0;
            timer = setInterval(function() {
                decaCounter += 10;
                $(_this.element).find(".holdButtonProgress").css("left", ((decaCounter / _this.settings.holdTime) * 100 - 100) + "%");
                if (decaCounter == _this.settings.holdTime) {
                    _this.exitTimer(timer);
                    $(_this.element).trigger('complete.pressAndHold');
                }
            }, 10);

            $(_this.element).on('mouseup.pressAndHold mouseleave.pressAndHold', function(event) {
                _this.exitTimer(timer);
            });

        }
    });
    // TODO
}

pressAndHold.prototype.exitTimer = function(timer) {
    var _this = this;
    clearTimeout(timer);
    $(this.element).off('mouseup.pressAndHold mouseleave.pressAndHold');
    setTimeout(function() {
        $(".holdButtonProgress").css("left", "-100%");
        $(_this.element).trigger('end.pressAndHold');
    }, this.settings.progressIndicatorRemoveDelay);
}
