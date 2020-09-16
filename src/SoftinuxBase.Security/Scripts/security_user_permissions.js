// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

/* eslint-disable import/no-absolute-path */

/// <reference path = '../../SoftinuxBase.Barebone/Scripts/barebone_ajax.js' />
/// <reference path = './security_user.js' />

'use strict';

import makeAjaxRequest from '/Scripts/barebone_ajax.js';
import { inputFormGroupSetError, inputFormGroupValidator } from '/Scripts/security_user.js';
import { inputOnlyAlphanumeric } from '/Scripts/toolbox.js';

/* Select 2 Boostrap 4 Theme for all Select2
   @see https://github.com/select2/select2/issues/2927
*/
// $.fn.select2.defaults.set('theme', 'bootstrap');

// Select2: template for unlink icon in edit role selectbox
function iformat(icon) {
    var originalOption = icon.element;
    return $('<span><i class="' + $(originalOption).data('icon-base') + ' ' + $(originalOption).data('icon') + '"></i> ' + icon.text + '</span>');
}

$(document).ready(function () {
    useSelect2();
});

/* ---------------------------------------------------------------- */
/* ------------------------ expose functions ---------------------- */
/* ---------------------------------------------------------------- */
window.viewSelectedRole = viewSelectedRole;
window.editRoleName = editRoleName;
window.deleteRole = deleteRole;

/* ---------------------------------------------------------------- */
/* ------------------------ events handlers ----------------------- */
/* ---------------------------------------------------------------- */

document.getElementById('bulk-delete-btn').addEventListener('click', () => {
    if (document.getElementById('availableRolesForDelete').selectedOptions.length === 0)
        window.toastr.warning('No role selected', 'Warning');
    else {
        deleteRole(Array.from(document.getElementById('availableRolesForDelete').options).filter(option => option.selected).map(option => option.value));
    }
    console.log(Array.from(document.getElementById('availableRolesForDelete').options).filter(option => option.selected).map(option => option.value));
});

document.getElementById('cancel-bulk-delete-btn').addEventListener('click', () => {
    document.getElementById('availableRolesForDelete').selectedIndex = -1;
});

document.getElementById('unlink-role-btn').addEventListener('click', (event_) => {
    unlinkRolePermissionOnAllExtensions(event_.target.attributes['data-name']);
});

Array.prototype.forEach.call(document.querySelectorAll('select.update-role-permission'), function (element_) {
    $(element_).on('select2:select', function (event_) {
        // we should use jQuery event system here
        updateRolePermission(event_);
    });
    $(element_).on('select2:unselect', function (event_) {
        // we should use jQuery event system here
        console.log('unselected', event_.params);
        updateRolePermission(event_);
    });
});

// Change
document.getElementById('role_name_input').addEventListener('change', () => {
    inputFormGroupValidator('#role_name_input');
});

// Focusout
document.getElementById('role_name_input').addEventListener('focusout', () => {
    inputFormGroupValidator('#role_name_input');
});

// Keypress: only numbers
document.getElementById('role_name_input').addEventListener('keypress', event_ => {
    inputOnlyAlphanumeric(event_);
}, false);
/* ---------------------------------------------------------------- */
/* ------------------------ Exported functions ----------------------------- */
/* ---------------------------------------------------------------- */

// None yet

/* --------------------------------------------------------------------------------------------- */
/* ------------------------ User interactions that trigger ajax calls -------------------------- */
/* --------------------------------------------------------------------------------------------- */

/**
 * Save new role.
 */
document.getElementById('save-add-role-btn').addEventListener('click', () => {
    const roleNameInputElt = document.getElementById('role_name_input');
    if (!roleNameInputElt.value) {
        window.toastr.warning('No role name given.', 'Role not saved!');
        inputFormGroupValidator('#role_name_input');
        return;
    }

    const postData = {
        RoleName: roleNameInputElt.value,
    };

    makeAjaxRequest('POST', '/administration/save-new-role', postData, (responseStatus_, responseText_) => {
        if (responseStatus_ === 201) {
            window.toastr.success(responseText_, 'New role created');
            inputFormGroupSetError('#role_name_input', null);
            refreshPermissionsTabs();
        } else {
            inputFormGroupSetError('#role_name_input', responseText_ || responseStatus_);
        }
    });

    resetAddRoleForm();
});

/**
 * Update the UI with selected role information. Ajax GET.
 * @param {any} roleId_ - roleId
 */
export function viewSelectedRole(roleId_) {
    makeAjaxRequest('GET', '/administration/read-role', { roleId_: roleId_ }, (responseStatus_, responseText_) => {
        if (responseStatus_ !== 200) {
            window.toastr.error(`Server return code: ${responseStatus_} with response: ${responseText_}`, 'Error');
            return;
        }

        const responseDataJson = JSON.parse(responseText_).value;
        // responseJson.value is ReadRoleViewModel C# class
        const role = responseDataJson.role;

        // Use role name for dynamic buttons
        document.getElementById('unlink-role-btn').innerHTML = `Remove all permissions of role <b>${role.name}</b>`
        document.getElementById('unlink-role-btn').attributes['data-name'] = role.name;
        document.getElementById('unlink-role-row').style.display = 'block';
        document.getElementById('rename-role-btn').innerHTML = `Rename role <b>${role.name}</b>`
        document.getElementById('rename-role-btn').attributes['data-name'] = role.name;
        document.getElementById('rename-role-div').style.display = 'block';

        // Selected extensions/permissions
        const rightListElt = document.getElementById('selectedRoleAssignedExtensionsList');
        // Clear
        rightListElt.innerHTML = '';
        // Fill
        let indexExtension = -1;
        for (const selectedExtension of responseDataJson.selectedExtensions) {
            indexExtension++;
            rightListElt.insertAdjacentHTML('beforeend', `<tr><td>
                            <i class="fas fa-cubes"></i>
                                ${selectedExtension.extensionName}
                            </td><td id="selected-extension-${indexExtension}"></td>
                        </tr>`);
            const cellElt = document.getElementById(`selected-extension-${indexExtension}`);
            let indexSection = -1;
            for (const sectionName of Object.keys(selectedExtension.groupedBySectionPermissionDisplays)) {
                indexSection++;
                cellElt.insertAdjacentHTML('beforeend', `<span class="text-muted">${sectionName}</span>
                <select multiple disabled class="assigned-permissions" id="selected-extension-${indexExtension}-section-${indexSection}"></select>`);
                const selectElt = document.getElementById(`selected-extension-${indexExtension}-section-${indexSection}`);
                for (const permissionDisplay of selectedExtension.groupedBySectionPermissionDisplays[sectionName]) {
                    selectElt.insertAdjacentHTML('beforeend', `<option value="${permissionDisplay.permissionEnumValue}" selected="${permissionDisplay.selected}" title="${permissionDisplay.description}">${permissionDisplay.shortName}</option>`);
                }
            }
        }
        useSelect2('assigned-permissions');
    });
}

/**
 * Call the API (Ajax POST) to add or remove a link between a role and a permission (for an extension).
 *  @param {any} event_ - the Select2 event (https://select2.org/programmatic-control/events).
 **/
function updateRolePermission(event_) {
    const params = {
        RoleName: event_.params.data.id,
        PermissionValue: Number(event_.params.data.element.attributes['data-permission'].value),
        ExtensionName: event_.params.data.element.attributes['data-extension'].value,
        Add: event_.params.data.selected
    };

    makeAjaxRequest('POST', '/administration/update-role-permission', params, (responseStatus_, responseText_) => {
        if (responseStatus_ === 204) {
            window.toastr.success(responseText_, 'Changes saved');
        } else if (responseStatus_ === 400) {
            window.toastr.error(responseText_, 'Permission to role link NOT updated');
        } else {
            window.toastr.error('Cannot update permission to role link. See logs for errors', 'Error');
        }
    });
}

/**
 * Open a modal window to enter new role name.
 * @param {HTMLButtonElement} renameRoleBtn_ - the clicked button.
 */
export function editRoleName(renameRoleBtn_) {
    alert(`
                    TODO
                    open
                    a
                    modal
                    to
                    enter
                    new name
                    for role ${renameRoleBtn_.attributes['data-name']}`)
}

/**
 * Ajax call to update role name. Ajax POST.
 */
export function saveEditRoleName() {
    // TODO 2: update code: only update role name. Make this be called by the modal confirmation.
    const _grants = [];
    let _noError = true;

    Array.prototype.forEach.call(document.querySelectorAll('#selectedRoleAssignedExtensionsList>div.row'), function (elt_) {
        let _extension, _permission;
        Array.prototype.forEach.call(elt_.querySelectorAll('div'), function (subElt_) {
            if (subElt_.querySelector('span'))
                _extension = elt_.querySelector('span').getAttribute('name');
            if (subElt_.querySelector('select'))
                _permission = subElt_.querySelector('select option:checked').value;
        });

        if (_extension && _permission)
            _grants.push({ Extension: _extension, PermissionValue: _permission });
        else {
            window.toastr.error('Cannot update role from client', 'Error');
            _noError = false;
        }
    });

    if (!_noError) return;

    const postData = {
        RoleId: document.getElementById('edit_role_id').value,
        RoleName: document.getElementById('edit_role_name_input').value,
        Grants: _grants
    };

    makeAjaxRequest('POST', '/administration/update-role', postData, (responseStatus_, responseText_) => {
        if (responseStatus_ === 201) {
            window.toastr.success(responseText_, 'Changes saved');
            refreshPermissionsTabs();
        } else {
            window.toastr.error('Cannot update role. See logs for errors', 'Error');
        }
    });
}

export function unlinkRolePermissionOnAllExtensions(roleName_) {
    makeAjaxRequest('DELETE', ` / administration / unlink - role - all - extensions /${roleName_}`, {}, (responseStatus_, responseText_) => {
        if (responseStatus_ === 204) {
            window.toastr.success(`Role ${roleName_}
                    unlinked
                    from
                    extensions`, 'Link deleted');
            refreshPermissionsTabs();
        } else if (responseStatus_ === 400) {
            window.toastr.error(responseText_, 'Role extension link NOT deleted');
        } else {
            window.toastr.error('Cannot delete link. See logs for details', 'Error');
        }
    });
}

export function deleteRole(roleNameList_) {
    makeAjaxRequest('DELETE', ` / administration / delete -role /${roleNameList_}`, {}, (responseStatus_, responseText_) => {
        if (responseStatus_ === 200) {
            window.toastr.success(`${roleNameList_}`, 'Role(s) deleted');
            refreshPermissionsTabs();
        } else {
            window.toastr.error('Cannot delete role(s). See logs for details', 'Error');
            console.log(responseStatus_, responseText_);
        }
    });
}

function reloadGrantPermissionsHtmlView() {
    return new window.Promise((resolve, reject) => {
        makeAjaxRequest('GET', '/administration/read-permissions-grants', null, (responseStatus_, responseText_) => {
            document.getElementById('GrantPermissionsTable').innerHTML = responseText_;
            resolve();
        });
    });
}

function reloadEditRoleHtmlView() {
    return new window.Promise((resolve, reject) => {
        makeAjaxRequest('GET', '/administration/edit-role-tab', null, (responseStatus_, responseText_) => {
            document.getElementById('edit_role_form').innerHTML = responseText_;
            resolve();
        });
    });
}

function reloadBulkDeleteTab() {
    return new window.Promise((resolve, reject) => {
        makeAjaxRequest('GET', '/administration/bulk-delete-role-tab', null, (responseStatus_, responseText_) => {
            document.getElementById('availableRolesForDelete').innerHTML = responseText_;
            resolve();
        });
    });
}

function refreshPermissionsTabs() {
    Promise.all([reloadGrantPermissionsHtmlView(), reloadEditRoleHtmlView(), reloadBulkDeleteTab()])
        .then(() => {
            document.getElementById('unlink-role-btn').addEventListener('click', () => {
                unlinkRolePermissionOnAllExtensions(document.getElementById('edit_role_normalizedName').value);
            });
            useSelect2();
        })
        .catch((error) => console.log(error));
}

function resetAddRoleForm() {
    document.getElementById('role_name_input').value = '';
}

function resetEditRoleForm() {
    // TODO reuse?
    document.getElementById('edit_role_name_input').value = '';
}

// Initialize Select2.org select elements.
// @param {class_} optional selector class instead of 'select2bs4'
function useSelect2(class_) {
    const selectorClass = class_ || 'select2bs4';
    $(`.${selectorClass}`).select2({
        minimumResultsForSearch: Infinity,
        width: null,
        theme: 'bootstrap4',
        templateSelection: iformat,
        templateResult: iformat,
        allowHtml: true
    });
}
