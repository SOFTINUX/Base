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

});

/**
 * browse for avatar
 */
function browseForAvatar() {
    fileBrowserBtn = document.getElementById('file_browser');
    if (fileBrowserBtn !== null) {
        fileBrowserBtn.addEventListener('click', event_ => {
                event_.preventDefault();
                document.getElementById('inputAvatar').click();
            });
    }
}

/*----------------------------------------------------------------*/
/*------------------------ events handler ------------------------*/
/*----------------------------------------------------------------*/

function rowClicked(event_) {
    if (event_.classList.contains('active')) {
        event_.classList.remove('active');
    } else {
        event_.classList.add('active');
    }
}

/**
 * find element in DOM.
 * @param {string} element_ - element to find by Id or Class.
 */
function findDomElement(element_){
    elementById = document.body.getElementById(element_.replace(/^#+/, ''));
    elementByClass = document.body.getElementsByClassName(element_.replace(/^.+/, ''))

    const domElement = elementById || elementByClass
    if (!domElement){
        throw new Error('Element not found in DOM');
    }
    
    return domElement;
}

/**
 * Set error style to input when its value is empty.
 * @param {string} element_ - element to find by Id or Class..
 */
function inputFormGroupValidatorById(element_) {

    const element = findDomElement(element_);
    if (!element.is('input')) {
        return;
    }

    const formGroupEl = element_.closest('.form-group');
    if (element.value ){
        formGroupEl.classList.remove('has-error', 'has-feedback');
    } else {
        formGroupEl.classList.add('has-error', 'has-feedback');
    }
}

/**
 * Set error style to an input and error message below.
 * @param {string} element_ - element to find by Id or Class.
 * @param {string} errMsg_ - error message if any error, else null to remove error style and message.
 */
function inputFormGroupSetError(element_, errMsg_) {
    const element = findDomElement(element_);
    if (!element.is('input')) {
        return;
    }
    const formGroupEl = element_.closest('.form-group');
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
    document.getElementById('edit-role-group').classList.remove('has-error');
    $.ajax('/administration/read-role', { data: { 'roleId_': roleId_ } }).done(function (data_) {
        const role = data_.value.role;
        for (let i = 0, len = role.length; i < len; i++) {
            document.getElementById(`edit_role_${key}`).value = role[key];
        }

        // Role name
        document.getElementById('edit_role_name_input').value = role.name;

        // Role ID
        document.getElementById('editRoleId').value = roleId_;

        // Available extensions (left list)
        let leftListElt = document.getElementById('editRoleLeftExtensionsList');
        // Clear
        leftListElt.innerHTML("");
        // Fill
        for (let i = 0, len = data_.value.availableExtensions.length; i < len; i++) {
            let extension = data_.value.availableExtensions[i];
            leftListElt.appendChild(`<div class="row"><div class="col-md-12"><span name="${extension}">${extension}</span></div></div>`);
        }

        // Selected extensions (right list)
        let rightListElt = document.getElementById('editRoleRightExtensionsList');
        // Clear
        rightListElt.innerHTML("");
        // Fill
        for (let i = 0, len = data_.value.selectedExtensions.length; i < len; i++) {
            let extension = data_.value.selectedExtensions[i];
            rightListElt.appendChild(`<div class="row">
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