// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

'use strict';

// Documentation https://developer.mozilla.org/fr/docs/Web/Guide/AJAX/Premiers_pas
// Voir aussi https://stackoverflow.com/a/24468752

/**
 * Make http request.
 * @export
 * @param {string } type_ - type of request (GET/POST/PUT/PATCH/DELETE)
 * @param {string} url_ - url (without query string)
 * @param {json} data_ - json data
 * @param {function(number, string)} responseCallback_ - function that is executed with 2 parameters: response status (number), response text (string). Optional
 */
 export default function makeAjaxRequest(type_, url_, data_, responseCallback_) {
    type_ = type_.toUpperCase();
    let httpRequest = new XMLHttpRequest();

    httpRequest.setRequestHeader('Content-Type', 'application/json');

    // on définit la fonction qui traite le retour (la réponse du serveur)
    httpRequest.onreadystatechange = () => {requestResult(httpRequest, responseCallback_);
    };

    if (type_ === 'GET' && data_) {
        // on ajoute les données à l'url
        url_ += `?data=${encodeURIComponent(JSON.stringify(data_))}`;
    }

    // on ouvre la requete http en mode ajax (le true en dernier paramètre = en asynchrone)
     try {
         httpRequest.open(type_, url_, true);
         // POST/PUT/PATCH et présence de données JSON. NB: the controller should use "[FromBody"]
         if ((type_ === 'POST' || type_ === 'PUT' || type_ === 'PATCH') && data_) {
             httpRequest.send(JSON.stringify(data_));
         } else {
             // on envoie la requete http de type GET
             httpRequest.send();
         }
     }
     catch (_e) {
         window.toastr.error('Cannot send or open request.', 'ERROR');
     }
 }

/**
 * treatment of http request response
 * @param {object} httpRequest_ - httpRequest object
 * @param {function(number, string)} responseCallback_ - function that is executed with 2 parameters: response status (number), response text (string). Optional
 */
function requestResult(httpRequest_, responseCallback_) {
    try {
        // on regarde si la requete est finie
        if (httpRequest_.readyState === XMLHttpRequest.DONE) {
            // When there is no response text returned by server, use status text
            let responseText = httpRequest_.responseText ? httpRequest_.responseText : httpRequest_.statusText;            // si le retour est un code 200 (ok) ou 201 (created) ou 400 (bad request)
            if (httpRequest_.status === 200 || httpRequest_.status === 201 || httpRequest_.status === 400) {
                console.log(responseText);
            } else {
                window.toastr.error(responseText, 'ERROR');
                console.log('Ajax error: ', responseText);
            }

            if (responseCallback_) {
                responseCallback_(httpRequest_.status, responseText);
            }

        }
    }
    catch( _e ) {
        alert(`Une exception s’est produite : ${_e.message}`);
        window.toastr.error(`Une exception s’est produite : ${_e.message}`, 'ERROR');
        responseCallback_('', _e.message);
    }
}