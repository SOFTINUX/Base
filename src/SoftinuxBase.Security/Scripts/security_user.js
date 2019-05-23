// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

///<reference path = '../../SoftinuxBase.Barebone/node_modules/jquery/dist/jquery.js' />
///<reference path = '../../SoftinuxBase.Barebone/node_modules/toastr/toastr.js' />

/* Anonymous function to avoid polluting global namespace */
$(function () {
    'use strict';

    window.toastr.options.positionClass = 'toast-top-right';

    browseForAvatar();

    // Change
    let inputAvatar = document.getElementById('inputAvatar');
    if(inputAvatar) {
        inputAvatar.addEventListener('change', event_ => {
            document.getElementById('file_path').value = event_.target.value;
        });
    }

    $('#role_name_input').change(function () {
        input_form_group_validator('#role_name_input');
    });

    // Focusout
    $('#role_name_input').focusout(function () {
        input_form_group_validator('#role_name_input');
    });

    /* User interactions that trigger ajax calls */

    // save new role with its extensions and permission
    $('#save-add-role-btn').click(function () {
        const roleNameInputElt = $('#role_name_input');
        if (!roleNameInputElt.val()) {
            window.toastr.warning('No role name given.', 'Role not saved!');
            input_form_group_validator('#role_name_input');
            return;
        }
        var _selectedExtensions = [];
        $('#addRoleRightExtensionsList > option').each(function () {
            _selectedExtensions.push(this.value);
        });
        const postData = {
            RoleName: roleNameInputElt.val(),
            Extensions: _selectedExtensions,
            PermissionValue: $('#newRolePermission').val()
        };

        $.ajax('/administration/save-new-role', { method: 'POST', data: postData })
            .done(function (data_) {
                window.toastr.success(data_, 'New role created');
                input_form_group_set_error('#role_name_input', null);
                location.reload();
            })
            .fail(function (jqXhr_, testStatus_) {
                const errMsg = jqXhr_.responseText ? jqXhr_.responseText : testStatus_;
                input_form_group_set_error('#role_name_input', errMsg);
            });
    });

    $('#save-edit-role-btn').click(function () {
        if (!$('#edit_role_name_input').val()) {
            window.toastr.warning('No new role name given.', 'Role not updated!');
            input_form_group_validator('#edit_role_name_input');
            return;
        }

        saveEditRole();
    });

});

/* Function declared in global namespace
so that they can be called by inline event handlers (onclick='...')
or by functions declared in anonymous function above */

/**
 * browse for avatar
 */
function browseForAvatar() {
    $('#file_browser').click(function (event_) {
        event_.preventDefault();
        $('#inputAvatar').click();
    });
}

/*----------------------------------------------------------------*/
/*------------------------ events handler ------------------------*/
/*----------------------------------------------------------------*/

/**
 *
 * @param {any} formid_ - formid_
 * @param {any} fieldsetid_ - fieldsetid_
 * @param {any} editbtnid_ - editbtnid_
 * @param {any} editbtntxt_ - editbtntxt_
 * @param {any} event_ - event_
 */
function cancel_edit_state(formid_, fieldsetid_, editbtnid_, editbtntxt_, event_) {
    event_.preventDefault();
    $(`#${fieldsetid_}`).prop('disabled', true);
    $(`#${editbtnid_}`).prop('disabled', false);
    $(`button#${editbtnid_}`).text(editbtntxt_);
    $(`#cancel_${editbtnid_}`).addClass('hidden');
    $(`button#${editbtnid_}`).removeClass('hidden');
    $('#file_browser').removeClass('btn-primary');
    $(`#${editbtnid_}`).removeClass('btn-success').addClass('btn-primary');
    $(`#${formid_}`)[0].reset();
}

/**
 *
 * @param {string} editbtnid_ - button html ID selector
 */
function input_changed(editbtnid_) {
    const element = document.getElementById(editbtnid_);
    element.disabled = false;
    element.classList.remove('btn-primary');
    element.classList.add('btn-success');
    element.innerText = 'Save';
    element.classList.remove('hidden');
    // show the corresponding cancel button when it exists
    let cancelButton = document.getElementById(`#cancel_${editbtnid_.replace('save', 'cancel')}`);
    if (cancelButton) {
        cancelButton.classList.remove('hidden');
    }
}

function row_clicked(event_) {
    if (event_.classList.contains('active')) {
        event_.classList.remove('active');
    } else {
        event_.classList.add('active');
    }
}

/**
 * Set error style to input when its value is empty.
 * @param {string} el_ - jQuery selector string.
 */
function input_form_group_validator(el_) {
    if (!$(el_).is('input')) {
        return;
    }

    if ($(el_).val()) {
        $(el_).closest('.form-group').removeClass('has-error').removeClass('has-feedback');
    } else {
        $(el_).closest('.form-group').addClass('has-error').addClass('has-feedback');
    }
}

/**
 * Set error style to an input and error message below.
 * @param {string} element_ - jQuery selector string.
 * @param {string} errMsg_ - error message if any error, else null to remove error style and message.
 */
function input_form_group_set_error(element_, errMsg_) {
    if (!$(element_).is('input')) {
        return;
    }
    const formGroupEl = $(element_).closest('.form-group');
    if (!errMsg_) {
        formGroupEl.removeClass('has-error').removeClass('has-feedback');
        formGroupEl.find('span.help-block').html('');
    } else {
        formGroupEl.addClass('has-error').addClass('has-feedback');
        formGroupEl.find('span.help-block').html(errMsg_);
    }
}

/*-------------------------------------------------------------------------------------*/
/*------------------------ Functions that trigger an ajax call ------------------------*/
/*-------------------------------------------------------------------------------------*/

/**
 * Update the UI with selected role information. Ajax GET.
 * @param {any} roleId_ - roleId
 */
function passSelectedRoleOnEdition(roleId_) {
    $('#edit-role-group').removeClass('has-error');
    $.ajax('/administration/read-role', { data: { 'roleId_': roleId_ } }).done(function (data_) {
        const role = data_.value.role;
        for (let i = 0, len = role.length; i < len; i++) {
            $(`#edit_role_${key}`).val(role[key]);
        }

        // Role name
        $('#edit_role_name_input').val(role.name);

        // Role ID
        $('#editRoleId').val(roleId_);

        // Available extensions (left list)
        let leftListElt = $('#editRoleLeftExtensionsList');
        // Clear
        leftListElt.html("");
        // Fill
        for (let i = 0, len = data_.value.availableExtensions.length; i < len; i++) {
            let extension = data_.value.availableExtensions[i];
            leftListElt.append(`<div class="row"><div class="col-md-12"><span name="${extension}">${extension}</span></div></div>`);
        }

        // Selected extensions (right list)
        let rightListElt = $('#editRoleRightExtensionsList');
        // Clear
        rightListElt.html("");
        // Fill
        for (let i = 0, len = data_.value.selectedExtensions.length; i < len; i++) {
            let extension = data_.value.selectedExtensions[i];
            rightListElt.append(`<div class="row">
                            <div class="col-md-6">
                                <span name="${extension.extensionName}">${extension.extensionName}</span>
                            </div>
                            <div class="col-md-6">
                                <select>
                                    <option value="None" ${extension.permissionName === 'None' ? "selected" : ""}>None</option>
                                    <option value="Read" ${extension.permissionName === 'Read' ? "selected" : ""}>Read</option>
                                    <option value="Write" ${extension.permissionName === 'Write' ? "selected" : ""}>Write</option>
                                    <option value="Admin" ${extension.permissionName === 'Admin' ? "selected" : ""}>Admin</option>
                                </select>
                            </div>
                        </div>`);
        }

    });
}

/**
 * Click in permission checkbox. Calls savePermission().
 */
function checkClick() {
    const splitted = $(event.target)[0].id.split('_');
    const base = splitted[0] + '_' + splitted[1];
    const writeCheckbox = document.getElementById(`${base}_WRITE`);
    const readCheckbox = document.getElementById(`${base}_READ`);
    var _activeCheckedPermissions = 'NEVER';

    if (event.target.checked) {
        // when checking, impacted checkboxes become checked and disabled
        switch (splitted[2]) {
            case 'ADMIN':
                writeCheckbox.checked = true;
                writeCheckbox.disabled = true;
                readCheckbox.checked = true;
                readCheckbox.disabled = true;
                break;
            case 'WRITE':
                readCheckbox.checked = true;
                readCheckbox.disabled = true;
                break;
        }
        savePermission(splitted[0], splitted[1], splitted[2]);
    }
    else {
        // when unchecking, first next checkbox becomes enabled
        switch (splitted[2]) {
            case 'ADMIN':
                writeCheckbox.disabled = false;
                _activeCheckedPermissions = 'WRITE';
                break;
            case 'WRITE':
                readCheckbox.disabled = false;
                _activeCheckedPermissions = 'READ';
                break;
        }
        savePermission(splitted[0], splitted[1], _activeCheckedPermissions);
    }
}

/**
 * Ajax call to update data: role-extension-permission link update. Ajax GET (TODO change to POST).
 * @param {any} extension_ - extension
 * @param {any} roleName_ - role name
 * @param {any} permission_ - permission (enum value)
 */
function savePermission(extension_, roleName_, permission_) {
    const params = {
        'roleName_': roleName_,
        'permissionValue_': permission_,
        'extension_': extension_
    };
    $.ajax('/administration/update-role-permission', { data: params });
}

/**
 * Ajax call to update data: role with its related data update. Ajax POST.
 */
function saveEditRole() {
    var _grants = [];
    $('#editRoleRightExtensionsList>div.row').each(function (index_, elt_) {
        _grants.push({ Extension: $(elt_).find('span').attr('name'), PermissionValue: $(elt_).find('select').val() });
    });
    const postData = {
        RoleId: $('#editRoleId').val(),
        RoleName: $('#edit_role_name_input').val(),
        Grants: _grants
    };

    $.ajax('/administration/update-role',
        {
            method: 'POST',
            data: postData
        })
        .done(function (data_) {
            window.toastr.success(data_, 'Changes saved');
            location.reload();
        });
}

function deleteRole(role_) {
    const postData = {
        'roleName_': role_
    };

    $.ajax('/administration/delete-role',
        {
            method: 'POST',
            data: postData
        })
        .done(function (data_) {
            window.toastr.success(data_, 'Role deleted');
            console.log(data_);
        });
}