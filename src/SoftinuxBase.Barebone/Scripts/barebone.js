// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

(function(application_) {
    'use strict';

    application_.eventHandlers = application_.eventHandlers || [];
    application_.registerEventHandler = function(eventHandler_) {
        application_.eventHandlers.push(eventHandler_);
    };

    application_.initializeEventHandlers = function() {
        $('body').on('click', 'button', function() {
            handleEvent('click', 'button', this);
        });

        $('body').on('change', 'input', function() {
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
	return this.href === url;
}).parentsUntil('.sidebar-menu > .treeview-menu').addClass('active');

// Global ajax error handler
$(document).ajaxError(function(jqXhr_, settings_, errorMessage_) {
    const errMsg = jqXhr_.responseText ? jqXhr_.responseText : errorMessage_;
    window.toastr.error(errMsg, 'ERROR');
    console.log('Ajax error: ', errMsg);
  });