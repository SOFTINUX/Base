'use strict';

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

