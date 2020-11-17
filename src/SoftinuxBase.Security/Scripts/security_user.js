// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

/// <reference path = '../../../node_modules/jquery/dist/jquery.js' />
/// <reference path = '../../../node_modules/toastr/toastr.js' />

/* eslint-disable no-undef */

'use strict';

import {findDomElement, getElementType} from '/Scripts/toolbox.js';

/* ---------------------------------------------------------------- */
/* ------------------------ on page load ------------------------ */
/* ---------------------------------------------------------------- */
window.toastr.options.positionClass = 'toast-top-right';
window.toastr.options.timeOut = 5000; // How long the toast will display without user interaction
window.toastr.options.extendedTimeOut = 20000; // How long the toast will display after a user hovers over it

/* ---------------------------------------------------------------- */
/* ------------------------ events handler ------------------------ */
/* ---------------------------------------------------------------- */

/* ---------------------------------------------------------------- */
/* ------------------------ functions ------------------------ */

/* ---------------------------------------------------------------- */

/**
 * Set error style to input when its value is empty.
 * @param {string} element_ - Id or Class.
 */
export function inputFormGroupValidator(element_) {
    const elements = findDomElement(element_);
    for (const element of elements) {
        if (!Object.is(getElementType(element), 'input')) {
            continue;
        }
        
        const formGroupEl = element.closest('.form-group');
        const helpBlockElt = formGroupEl.querySelectorAll('span.help-block')[0];

        if (element.value) {
            element.classList.remove('is-invalid');
            helpBlockElt.classList.remove('invalid-feedback');
            helpBlockElt.style.display = 'none';
        } else {
            element.classList.add('is-invalid');
            helpBlockElt.classList.add('invalid-feedback');
            helpBlockElt.style.display = 'block';
        }
    }
}

/**
 * Set error style to an input and its error message below.
 * @param {string} element_ - Id or Class.
 * @param {string} errMsg_ - error message if any error, else null to remove error style and message.
 */
export function inputFormGroupSetError(element_, errMsg_) {
    const elements = findDomElement(element_);
    for (const element of elements) {
        if (!Object.is(getElementType(element), 'input')) {
            continue;
        }
        const formGroupEl = element.closest('.form-group');
        if (!errMsg_) {
            element.classList.remove('is-valid');
            element.classList.remove('is-invalid');
            const helpBlockElt = formGroupEl.querySelectorAll('span.help-block')[0];
            helpBlockElt.innerHTML = '';
            helpBlockElt.style.display = 'none';
            helpBlockElt.classList.remove('invalid-feedback');
        } else {
            element.classList.remove('is-valid');
            element.classList.add('is-invalid');
            const helpBlockElt = formGroupEl.querySelectorAll('span.help-block')[0];
            helpBlockElt.innerHTML = errMsg_;
            helpBlockElt.classList.add('invalid-feedback');
            helpBlockElt.style.display = 'block';
        }
    }
}

/* ------------------------------------------------------------------------------------- */
/* ------------------------ Functions that trigger an ajax call ------------------------ */
/* ------------------------------------------------------------------------------------- */
