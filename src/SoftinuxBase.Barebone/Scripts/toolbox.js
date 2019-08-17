// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

'use strict';

/**
 * Find element in DOM.
 * @param {string} element_ - Id or Class.
 * @return {Array} - return array of DOM elements.
 */
export function findDomElement(element_) {
    const elementById = document.getElementById(element_.replace(/^#+/, ''));
    const elementByClass = document.getElementsByClassName(element_.replace(/^.+/, ''));

    const domElement = elementById || elementByClass;
    if (!domElement) {
        throw new Error(`Element not found in DOM: ${element_}`);
    }

    return Array.of(domElement);
}

/**
 * Return type of object.
 * @param {string} element_ - element to get type.
 * @return {string} - return dom object type (NaO is Not an Object).
 */
export function getElementType(element_) {
    switch (Object.prototype.toString.call(element_)) {
        case '[object HTMLInputElement]':
            return 'input';
        case '[object Text]':
            return 'text';
        case '[object String]':
            return 'string';
        case '[object Number]':
            return 'number';
        case '[object Date]':
            return 'date';
        case '[object Function]':
            return 'function';
        case '[object RegExp]':
            return 'regexp';
        case '[object Array]':
            return 'array';
        default:
            return 'NaO';
    }
}
