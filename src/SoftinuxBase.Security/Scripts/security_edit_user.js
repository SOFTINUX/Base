'use strict';

[].forEach.call(document.querySelectorAll('button'),
    function (event_) {
        event_.addEventListener('click',
            function (e_) {
                switch (event_.id) {
                    case 'save_profile_btn':
                        edit_state('profile_form_fieldset', 'save_profile_btn', e_);
                        break;
                    case 'cancel_save_profile_btn':
                        cancel_edit_state('profile_form', 'profile_form_fieldset', 'save_profile_btn', 'Edit', e_);
                        break;
                    case 'change_pwd-btn':
                        edit_state('pwd_form_fliedset', 'change_pwd-btn', e_);
                        break;
                    case 'cancel_change_pwd-btn':
                        cancel_edit_state('pwd_form', 'pwd_form_fliedset', 'change_pwd-btn', 'Change', e_);
                        break;
                    default:
                        break;
                }
            }
        ,false);
    }
);

/*----------------------------------------------------------------*/
/*------------------------ events handler ------------------------*/
/*----------------------------------------------------------------*/

// Keyup, change, paste
let profileForm = document.getElementById('profile_form');
if (profileForm !== null) {
    profileForm = profileForm.getElementsByTagName('input');
    for (let inputField of profileForm) {
        inputField.addEventListener('keyup', () => input_changed('save_profile_btn'));
        inputField.addEventListener('paste', () => input_changed('save_profile_btn'));
        inputField.addEventListener('change', () => input_changed('save_profile_btn'));
    }
}

let changePasswordForm = document.getElementById('pwd_form');
if (changePasswordForm !== null) {
    changePasswordForm = changePasswordForm.getElementsByTagName('input');
    for (let pwdField of changePasswordForm) {
        pwdField.addEventListener('keyup', () => input_changed('change_pwd-btn'));
        pwdField.addEventListener('paste', () => input_changed('change_pwd-btn'));
        pwdField.addEventListener('change', () => input_changed('change_pwd-btn'));
    }
}

/*----------------------------------------------------------------*/
/*------------------------ functions -----------------------------*/
/*----------------------------------------------------------------*/

/**
 *
 * @param {string} fieldsetid_ - fieldset html ID selector
 * @param {string} editbtnid_ - edit button html ID selector
 * @param {any} event_ - event handler
 */
function edit_state(fieldsetid_, editbtnid_, event_) {
    event_.preventDefault();
    let editbtnId = document.getElementById(editbtnid_);
    let editCanBtnId = document.getElementById(`cancel_${editbtnid_}`);
    let fieldSetId = document.getElementById(fieldsetid_);

    editCanBtnId.classList.remove('hidden');
    editbtnId.classList.add('hidden');
    if (editbtnid_ === 'save_profile_btn') {
        let fileBrowser = document.getElementById('file_browser');
        fileBrowser.classList.remove('btn-primary');
        fileBrowser.classList.add('btn-default');
    }
    if (fieldSetId.disabled) {
        fieldSetId.removeAttribute('disabled');
        editbtnId.setAttribute('disaled', true);
    }
}

/**
 *
 * @param {string} formid_ - form html ID selector
 * @param {string} fieldsetid_ - fieldset html ID selector
 * @param {string} editbtnid_ - edit button html ID selector
 * @param {string} editbtntxt_ - edit button html text
 * @param {any} event_ - event handler
 */
function cancel_edit_state(formid_, fieldsetid_, editbtnid_, editbtntxt_, event_) {
    event_.preventDefault();

    document.getElementById(editbtnid_).classList.remove('hidden', 'btn-success');
    document.getElementById(editbtnid_).classList.add('btn-primary');
    document.getElementById(editbtnid_).disabled = false;
    document.getElementById(editbtnid_).innerHTML = editbtntxt_;

    document.getElementById(fieldsetid_).disabled = true;
    document.getElementById(`cancel_${editbtnid_}`).classList.add('hidden');
    document.getElementById('file_browser').classList.remove('btn-primary');
    document.getElementById(formid_).reset();
}

/**
 *
 * @param {string} editbtnid_ - button html ID selector
 */
function input_changed(editbtnid_) {
    const element = document.getElementById(editbtnid_);
    element.disabled = false;
    element.classList.remove('hidden', 'btn-primary');
    element.classList.add('btn-success');
    element.innerText = 'Save';
    // show the corresponding cancel button when it exists
    let cancelButton = document.getElementById(`cancel_${editbtnid_.replace('save', 'cancel')}`);
    if (cancelButton) {
        cancelButton.classList.remove('hidden');
    }
}