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
 * @param {any} fieldsetid_ - fieldsetid_
 * @param {any} editbtnid_ - editbtnid_
 * @param {any} event_ - event_
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


