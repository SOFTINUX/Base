// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

'use strict';

/* ---------------------------------------------------------------- */
/* ------------------------ on page load -------------------------- */
/* ---------------------------------------------------------------- */
addEventListenersForAvatarSelection();

[].forEach.call(document.querySelectorAll('button'),
    function (button_) {
        button_.addEventListener('click',
            function (e_) {
                switch (button_.id) {
                    case 'save_profile_btn':
                        editState('profile_form_fieldset', 'save_profile_btn', e_);
                        break;
                    case 'cancel_save_profile_btn':
                        cancelEditState('profile_form', 'profile_form_fieldset', 'save_profile_btn', 'Edit', e_);
                        break;
                    case 'change_pwd-btn':
                        editState('pwd_form_fliedset', 'change_pwd-btn', e_);
                        break;
                    case 'cancel_change_pwd-btn':
                        cancelEditState('pwd_form', 'pwd_form_fliedset', 'change_pwd-btn', 'Change', e_);
                        break;
                    default:
                        break;
                }
            }
            , false);
    }
);

/* ---------------------------------------------------------------- */
/* ------------------------ events handler ------------------------ */
/* ---------------------------------------------------------------- */

// Keyup, change, paste
const profileForm = document.getElementById('profile_form');
if (profileForm) {
    const inputFields = profileForm.getElementsByTagName('input');
    if (inputFields) {
        for (const inputField of inputFields) {
            inputField.addEventListener('keyup', () => inputChanged('save_profile_btn'));
            inputField.addEventListener('paste', () => inputChanged('save_profile_btn'));
            inputField.addEventListener('change', () => inputChanged('save_profile_btn'));
        }
    }
}

const changePasswordForm = document.getElementById('pwd_form');
if (changePasswordForm) {
    const inputFields = changePasswordForm.getElementsByTagName('input');
    if (inputFields) {
        for (const pwdField of inputFields) {
            pwdField.addEventListener('keyup', () => inputChanged('change_pwd-btn'));
            pwdField.addEventListener('paste', () => inputChanged('change_pwd-btn'));
            pwdField.addEventListener('change', () => inputChanged('change_pwd-btn'));
        }
    }
}

/* ---------------------------------------------------------------- */
/* ------------------------ functions ----------------------------- */
/* ---------------------------------------------------------------- */

/*
 * Add events listeners for file selection : button and hidden file input.
*/
export function addEventListenersForAvatarSelection() {
    // When a file is chosen using file selector, put the selected file's name into the "file_path" text input
    const inputAvatar = document.getElementById('inputAvatar');
    if (inputAvatar) {
        inputAvatar.addEventListener('change', event_ => {
            document.getElementById('file_path').value = event_.target.files[0].name;
        }, false);
    }
    // When the "file_browser" button is clicked, trigger a click on not visible input of type file ("inputAvatar") to allow to select a file
    const fileBrowser = document.getElementById('file_browser');
    if (fileBrowser) {
        fileBrowser.addEventListener('click', event_ => {
            event_.preventDefault();
            // Open the file selector
            document.getElementById('inputAvatar').click();
        }, false);
    }
}

/**
 *
 * @param {string} fieldsetid_ - fieldset html ID selector
 * @param {string} editbtnid_ - edit button html ID selector
 * @param {Event} event_ - event handler
 */
function editState(fieldsetid_, editbtnid_, event_) {
    event_.preventDefault();
    const editbtnId = document.getElementById(editbtnid_);
    const editCanBtnId = document.getElementById(`cancel_${editbtnid_}`);
    const fieldSetId = document.getElementById(fieldsetid_);

    editCanBtnId.classList.remove('hidden');
    editbtnId.classList.add('hidden');
    if (editbtnid_ === 'save_profile_btn') {
        const fileBrowser = document.getElementById('file_browser');
        if (fileBrowser) {
            fileBrowser.classList.remove('btn-primary');
            fileBrowser.classList.add('btn-default');
        }
    }
    if (fieldSetId.disabled) {
        fieldSetId.disabled = false;
        editbtnId.disable = true;
    }
}

/**
 *
 * @param {string} formid_ - form html ID selector
 * @param {string} fieldsetid_ - fieldset html ID selector
 * @param {string} editbtnid_ - edit button html ID selector
 * @param {string} editbtntxt_ - edit button html text
 * @param {Event} event_ - event handler
 */
function cancelEditState(formid_, fieldsetid_, editbtnid_, editbtntxt_, event_) {
    event_.preventDefault();

    const editbtnId = document.getElementById(editbtnid_);
    editbtnId.classList.remove('hidden', 'btn-success');
    editbtnId.classList.add('btn-primary');
    editbtnId.disabled = false;
    editbtnId.innerHTML = editbtntxt_;

    document.getElementById(fieldsetid_).disabled = true;
    document.getElementById(`cancel_${editbtnid_}`).classList.add('hidden');
    const fileBrowser = document.getElementById('file_browser');
    if (fileBrowser) {
        fileBrowser.classList.remove('btn-primary');
    }
    document.getElementById(formid_).reset();
}

/**
 *
 * @param {string} editbtnid_ - button html ID selector
 */
function inputChanged(editbtnid_) {
    const element = document.getElementById(editbtnid_);
    element.disabled = false;
    element.classList.remove('hidden', 'btn-primary');
    element.classList.add('btn-success');
    element.innerText = 'Save';
    // show the corresponding cancel button when it exists
    const cancelButton = document.getElementById(`cancel_${editbtnid_.replace('save', 'cancel')}`);
    if (cancelButton) {
        cancelButton.classList.remove('hidden');
    }
}
