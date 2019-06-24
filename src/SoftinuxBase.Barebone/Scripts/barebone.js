// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

(function (application_) {
    'use strict';

    application_.eventHandlers = application_.eventHandlers || [];
    application_.registerEventHandler = function (eventHandler_) {
        application_.eventHandlers.push(eventHandler_);
    };

    application_.initializeEventHandlers = function () {
        $('body').on('click', 'button', function () {
            handleEvent('click', 'button', this);
        });

        $('body').on('change', 'input', function () {
            handleEvent('change', 'input', this);
        });
    };

    function handleEvent(eventName_, tagName_, element_) {
        for (let i = 0; i < application_.eventHandlers.length; i++) {
            if (application_.eventHandlers[i].eventName === eventName_ && application_.eventHandlers[i].tagName === tagName_) {
                application_.eventHandlers[i].action(element_);
            }
        }
    }
})(window.application = window.application || {});

$(document).ready(
    function () {
        application.initializeEventHandlers();
    }
);

// AdminLTE menu tweak: remember which menu group was open
// https://github.com/almasaeed2010/AdminLTE/issues/1806

/** Add active class and stay opened when selected
 * Vanilla JS version, thanks to https://gomakethings.com/how-to-get-all-parent-elements-with-vanilla-javascript/#climbing-up-the-dom
 */
keepTreeviewMenuOpened();

function keepTreeviewMenuOpened() {
    [].forEach.call(document.querySelectorAll('ul.treeview-menu a'), (anchor_) => {
        if (anchor_.href == window.location) {
            [].forEach.call(getParents(anchor_), (elem_) => {
                elem_.classList.add('active');
            });
        }
    });
}

/**
 * Gather all the parent nodes of an element "elem" until current element matches selector "selector"
 * and return the list of all the elements.
 * @export
 * @param {*} elem
 * @param {*} selector
 * @returns
 */
export function getParents(elem, selector) {

    // Set up a parent array
    let parents = [];

    // Push each parent element to the array
    for ( ; elem && elem !== document; elem = elem.parentNode ) {
        if (elem.matches(selector)) {
            parents.push(elem);
        }
    }

    // Return our parent array
    return parents;
};
