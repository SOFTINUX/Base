'use strict';

// Keyup, change, paste
const profileForm = document.getElementById('profile_form').getElementsByTagName('input');
profileForm.addEventListener('keyup', input_changed('save_profile_btn'));
profileForm.addEventListener('paste', input_changed('save_profile_btn'));
profileForm.addEventListener('change', input_changed('save_profile_btn'));

document.getElementById('pwd_form').getElementsByTagName('input').addEventListener('keyup', input_changed('change_pwd-btn'));
document.getElementById('pwd_form').getElementsByTagName('input').addEventListener('paste', input_changed('change_pwd-btn'));
document.getElementById('pwd_form').getElementsByTagName('input').addEventListener('change', input_changed('change_pwd-btn'));