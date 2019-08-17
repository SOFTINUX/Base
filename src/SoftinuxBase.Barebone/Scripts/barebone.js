// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See License.txt in the project root for license information.

// (function (application_) {
//     'use strict';

//     application_.eventHandlers = application_.eventHandlers || [];
//     application_.registerEventHandler = function (eventHandler_) {
//         application_.eventHandlers.push(eventHandler_);
//     };

//     application_.initializeEventHandlers = function () {
//         $('body').on('click', 'button', function () {
//             handleEvent('click', 'button', this);
//         });

//         $('body').on('change', 'input', function () {
//             handleEvent('change', 'input', this);
//         });
//     };

//     function handleEvent(eventName_, tagName_, element_) {
//         for (let i = 0; i < application_.eventHandlers.length; i++) {
//             if (application_.eventHandlers[i].eventName === eventName_ && application_.eventHandlers[i].tagName === tagName_) {
//                 application_.eventHandlers[i].action(element_);
//             }
//         }
//     }
// })(window.application = window.application || {});

// $(document).ready(
//     function () {
//         application.initializeEventHandlers();
//     }
// );

// AdminLTE menu tweak: remember which menu group was open
// https://github.com/almasaeed2010/AdminLTE/issues/1806

[].forEach.call(document.querySelectorAll('ul.treeview-menu a'), (anchor_) => {
    if (window.location.href.startsWith(anchor_.href)) {
        let currentElement = anchor_;
        for (; currentElement && currentElement !== document; currentElement = currentElement.parentNode) {
            if (currentElement.matches('li')) {
                // a parent li in menu (menu iteml li (immediate parent of anchor) or menu group li)
                currentElement.classList.add('active');
            } else if (currentElement.matches('.sidebar-menu > .treeview-menu')) {
                // the root of menu is hit, stop iterating parent nodes
                break;
            }
        }
    }
});
