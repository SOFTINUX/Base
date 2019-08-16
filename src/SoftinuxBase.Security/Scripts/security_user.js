// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

/// <reference path = '../../SoftinuxBase.Barebone/node_modules/jquery/dist/jquery.js' />
/// <reference path = '../../SoftinuxBase.Barebone/node_modules/toastr/toastr.js' />

/* eslint-disable no-undef */

'use strict';

/* ---------------------------------------------------------------- */
/* ------------------------ on page load ------------------------ */
/* ---------------------------------------------------------------- */
window.toastr.options.positionClass = 'toast-top-right';

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
		if (element.value) {
			formGroupEl.classList.remove('has-error', 'has-feedback');
		} else {
			formGroupEl.classList.add('has-error', 'has-feedback');
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
			formGroupEl.classList.remove('has-error', 'has-feedback');
			formGroupEl.querySelectorAll('span.help-block')[0].innerHTML = '';
		} else {
			formGroupEl.classList.add('has-error', 'has-feedback');
			formGroupEl.querySelectorAll('span.help-block')[0].innerHTML = errMsg_;
		}
	}
}

/* ------------------------------------------------------------------------------------- */
/* ------------------------ Functions that trigger an ajax call ------------------------ */
/* ------------------------------------------------------------------------------------- */
