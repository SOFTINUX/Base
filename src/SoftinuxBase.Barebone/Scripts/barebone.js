// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

(function(application) {
    application.eventHandlers = application.eventHandlers || [];
    application.registerEventHandler = function(eventHandler) {
        application.eventHandlers.push(eventHandler);
    };

    application.initializeEventHandlers = function() {
        $("body").on("click", "button", function() {
            handleEvent("click", "button", this);
        });

        $("body").on("change", "input", function() {
            handleEvent("change", "input", this);
        });
    };

    function handleEvent(eventName, tagName, element) {
        for (var i = 0; i < application.eventHandlers.length; i++) {
            if (application.eventHandlers[i].eventName == eventName && application.eventHandlers[i].tagName == tagName) {
                application.eventHandlers[i].action(element);
            }
        }
    }
})(window.application = window.application || {});

$(document).ready(
    function() {
        application.initializeEventHandlers();
    }
);

// AdminLTE menu tweak: remember which menu group was open
// https://github.com/almasaeed2010/AdminLTE/issues/1806

/** add active class and stay opened when selected */
var url = window.location;

// for sidebar menu entirely but not cover treeview
// $('ul.sidebar-menu a').filter(function() {
// 	 return this.href == url;
// }).parent().addClass('active');

// for treeview
$('ul.treeview-menu a').filter(function() {
	 return this.href == url;
}).parentsUntil(".sidebar-menu > .treeview-menu").addClass('active');

// Global ajax error handler
$(document).ajaxError(function(event, jqXhr, settings, errorMessage) {
    var errMsg = jqXhr.responseText ? jqXhr.responseText : errorMessage;
    window.toastr.error(errMsg, 'ERROR');
    console.log("Error calling ", settings.url, " ", errMsg);
  });