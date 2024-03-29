// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

/* eslint-disable brace-style */

'use strict';

// Documentation https://developer.mozilla.org/fr/docs/Web/Guide/AJAX/Premiers_pas
// Voir aussi https://stackoverflow.com/a/24468752

/**
 * Make http request.
 * Return values is in responseCallback_ function.
 * @export
 * @param {string} type_ - type of request (GET/POST/PUT/PATCH/DELETE)
 * @param {string} url_ - url (without query string)
 * @param {json} data_ - json data
 * @param {function(number, string)} responseCallback_ - function that is executed with 2 parameters: response status (number), response text (string). Optional
 */
export function makeAjaxRequest(type_, url_, data_, responseCallback_) {
    type_ = type_.toUpperCase();
    const httpRequest = new XMLHttpRequest();

    if (type_ === 'GET' && data_) {
        // add data to url
        const query = [];
        // ReSharper disable once MissingHasOwnPropertyInForeach
        for (const key in data_) {
            query.push(encodeURIComponent(key) + '=' + encodeURIComponent(data_[key]));
        }
        url_ += query.length ? '?' + query.join('&') : '';
    }

    // we open the http request in ajax mode (true last parameter = asynchronous)
    try {
        httpRequest.open(type_, url_, true);
        httpRequest.setRequestHeader('Content-Type', 'application/json');
        // we define the function that processes the server response
        httpRequest.onreadystatechange = () => { requestResult(httpRequest, responseCallback_); };

        // POST/PUT/PATCH and presence of JSON. NB: the controller should use "[FromBody"]
        if ((type_ === 'POST' || type_ === 'PUT' || type_ === 'PATCH') && data_) {
            httpRequest.send(JSON.stringify(data_));
        } else {
            // we send the GET type http request
            httpRequest.send();
        }
    }
    catch (e) {
        window.toastr.error('Cannot send or open request.', 'ERROR');
        console.error(`Error on ajax request: ${e.message}`);
    }
}

/**
 * treatment of http request response
 * @param {object} httpRequest_ - httpRequest object
 * @param {function(number, string)} responseCallback_ - function that is executed with 2 parameters: response status (number), response text (string). Optional
 */
function requestResult(httpRequest_, responseCallback_) {
    try {
        // we look if the request is finished
        if (httpRequest_.readyState === XMLHttpRequest.DONE) {
            // When there is no response text returned by server, use status text
            const responseText = httpRequest_.responseText ? httpRequest_.responseText : httpRequest_.statusText;
            if (httpRequest_.status !== 200 && httpRequest_.status !== 201 && httpRequest_.status !== 204 && httpRequest_.status !== 400) {
                // 200, 201, 204 and 400 indicate successful response by the server
                window.toastr.error(responseText, 'ERROR');
                console.error('Ajax error: ', responseText);
            }

            if (responseCallback_) {
                responseCallback_(httpRequest_.status, responseText.toString());
            }
        }
    }
    catch (e) {
        alert(`Une exception s’est produite : ${e.message}`);
        window.toastr.error(`Une exception s’est produite : ${e.message}`, 'ERROR');
        responseCallback_(0, e.message);
    }
}
