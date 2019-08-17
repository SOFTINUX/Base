// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

/* eslint-disable no-undef */

// Documentation: https://adminlte.io/docs/2.4/js-box-refresh
'use strict';

$.fn.extend({
    // Add loadWithSpinner function to an element selected by jQuery.
    // Params:
    //          boxId: id of box which .box-body subcontent will be replaced by ajax response
    //          urlToGet: url that ajax should call
    //          urlQueryParams: GET query paramaters (example: {search_term: 'layout'}, which renders to URL/?search_term=layout).
    //          responseType: Response type (example: 'json' or 'html')
    loadWithSpinner: function (boxId_, urlToGet_, urlQueryParams_, responseType_) {
        $(boxId_).refreshBox({
            // The URL for the source.
            source: urlToGet_,
            // GET query paramaters (example: {search_term: 'layout'}, which renders to URL/?search_term=layout).
            params: urlQueryParams_ || {},
            // The CSS selector to the refresh button.
            trigger: '.refresh-btn',
            // The CSS selector to the target where the content should be rendered. This selector should exist within the box.
            content: '.box-body',
            // Whether to automatically render the content.
            loadInContent: true,
            // Response type (example: 'json' or 'html')
            responseType: responseType_,
            // The HTML template for the ajax spinner.
            overlayTemplate: '<div class="overlay"><div class="fa fa-refresh fa-spin"></div></div>',
            // Called before the ajax request is made.
            onLoadStart: function () {
                // Do something before sending the request.
            },
            // Called after the ajax request is made.
            onLoadDone: function () {
                // Do something after receiving a response.
            }
        });
    }
});
