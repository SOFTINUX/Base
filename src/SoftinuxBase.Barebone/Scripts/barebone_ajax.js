"use strict";

// Documentation https://developer.mozilla.org/fr/docs/Web/Guide/AJAX/Premiers_pas
// Voir aussi https://stackoverflow.com/a/24468752

Barebone = {};

/**
 * Make http request.
 * @param {string } type_ - type of request (GET/POST)
 * @param {string} url_ - url (without query string)
 * @param {json} data_ - json data
 * @param {function} responseCallback_ - function that is executed with response data. Optional
 * @return {boolean} - result
 */
Barebone.prototype.makeRequest = function(type_, url_, data_, responseCallback_) {
    let httpRequest = false;
    type_ = type_.toUpperCase();

    httpRequest = new XMLHttpRequest();

    // si on n'est pas arrivé à créer un objet de type http request
    if (!httpRequest) {
        alert("Abandon :( Impossible de créer une instance XMLHTTP");
        return false;
    }

    // on défini la fonction qui traite le retour (la réponse du serveur)
    httpRequest.onreadystatechange = () => {
        requestResult(httpRequest, responseCallback_);
    };

    if (type_ === "GET" && data_) {
        httpRequest.setRequestHeader("Content-Type", "application/json");
        // on ajoute les données à l'url
        url_ += "?data=" + encodeURIComponent(JSON.stringify(data_));
    }

    // on ouvre la requete http en mode ajax (le true en dernier paramètre = en ajax)
    httpRequest.open(type_, url_, true);

    // si test si on passe des data, si oui on est dans un type POST
    if (type_ === "POST" && data_) {
        httpRequest.setRequestHeader("Content-Type", "application/json");
        // on envoie la requete http de type POST
        httpRequest.send(JSON.stringify(data_));
        //httpRequest.send(encodeURIComponent(JSON.stringify(data_)));
    } else {
        // on envoie la requete http de type GET
        httpRequest.send();
    }

}

/**
 * treatment of http request response
 * @param {object} httpRequest - httpRequest object
 * @param {function} responseCallback_ - function that is executed with response data. Optional
 */
function requestResult(httpRequest, responseCallback_) {
    try {
        // on regarde si la requete est finie
        if (httpRequest.readyState === XMLHttpRequest.DONE) {
            // si le retour est un code 200 (ok) ou 201 (created) ou 400 (bad request)
            if (httpRequest.status === 200 || httpRequest.status === 201 || httpRequest.status === 400) {
                console.log(JSON.parse(httpRequest.responseText));
            } else {
                console.log("Un problème est survenu avec la requête. Texte de la réponse: " + httpRequest.responseText);
            }

            if (responseCallback_) {
                responseCallback_(httpRequest.responseText);
            }

        }
    }
    catch( e ) {
        alert("Une exception s’est produite : " + e.description);
    }
}