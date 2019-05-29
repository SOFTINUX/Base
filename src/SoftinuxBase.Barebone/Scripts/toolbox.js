/**
 * Find element in DOM.
 * @param {string} element_ - Id or Class.
 * @return {Array} - return array of DOM elements.
 */
function findDomElement(element_){
    elementById = document.getElementById(element_.replace(/^#+/, ''));
    elementByClass = document.getElementsByClassName(element_.replace(/^.+/, ''))

    const domElement = elementById || elementByClass;
    if (!domElement){
        throw new Error(`Element not found in DOM: ${element_}`);
    }

    return Array.of(domElement);
}

/**
 * Return type of object.
 * @param {string} element_ - element to get type.
 * @return {string} - return dom object type.
 */
function getElementType(element_){
    switch(Object.prototype.toString.call(element_)){
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