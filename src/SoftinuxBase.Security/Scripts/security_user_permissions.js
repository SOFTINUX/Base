﻿// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

/* eslint-disable import/no-absolute-path */

/// <reference path = '../../SoftinuxBase.Barebone/Scripts/barebone_ajax.js' />
/// <reference path = './security_user.js' />

let bareboneAjaxModule;
let securityUserModule;
try {
    bareboneAjaxModule = await import('/Scripts.barebone.min.js');
} catch (err) {
    bareboneAjaxModule = await import('/Scripts.barebone_ajax.js');
}

try {
    securityUserModule = await import('/Scripts.security_user.js');
} catch (err) {
}

'use strict';

import { inputOnlyAlphanumeric } from '/Scripts/toolbox.js';

/* Select 2 Boostrap 4 Theme for all Select2
   @see https://github.com/select2/select2/issues/2927
*/
// $.fn.select2.defaults.set('theme', 'bootstrap');

// Select2: template for unlink icon in edit role selectbox
function iformat(icon) {
    const originalOption = icon.element;
    return $('<span><i class="' + $(originalOption).data('icon-base') + ' ' + $(originalOption).data('icon') + '"></i> ' + icon.text + '</span>');
}

$(document).ready(function () {
    useSelect2();
});

/* ---------------------------------------------------------------- */
/* ------------------------ expose functions ---------------------- */
/* ---------------------------------------------------------------- */
window.viewSelectedRole = viewSelectedRole;
window.deleteRole = deleteRole;

/* ---------------------------------------------------------------- */
/* ------------------------ events handlers ----------------------- */
/* ---------------------------------------------------------------- */

const bulkDeleteBtn = document.getElementById('bulk-delete-btn');
if (bulkDeleteBtn) {
    bulkDeleteBtn.addEventListener('click',
        () => {
            if (document.getElementById('availableRolesForDelete').selectedOptions.length === 0)
                window.toastr.warning('No role selected', 'Warning');
            else {
                deleteRole(Array.from(document.getElementById('availableRolesForDelete').options)
                    .filter(option => option.selected).map(option => option.value));
            }
            console.log(Array.from(document.getElementById('availableRolesForDelete').options)
                .filter(option => option.selected).map(option => option.value));
        });
}
const cancelBulkDeleteBtn = document.getElementById('cancel-bulk-delete-btn');
if (cancelBulkDeleteBtn) {
    cancelBulkDeleteBtn.addEventListener('click',
        () => {
            document.getElementById('availableRolesForDelete').selectedIndex = -1;
        });
}

const saveRenameRoleBtn = document.getElementById('save-rename-role-btn');
if (saveRenameRoleBtn) {
    saveRenameRoleBtn.addEventListener('click',
        (event_) => {
            saveEditRoleName();
        });
}

const unlinkRoleBtn = document.getElementById('unlink-role-btn');
if (unlinkRoleBtn) {
    unlinkRoleBtn.addEventListener('click',
        (event_) => {
            console.log(event_.target);
            deleteAllPermissionsOfRole(event_.target.attributes['data-name']);
        });
}

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
const roleNameInput = document.getElementById('role_name_input');
if (roleNameInput) {
    roleNameInput.addEventListener('change',
        () => {
            securityUserModule ? securityUserModule.inputFormGroupValidator('#role_name_input') : inputFormGroupValidator('#role_name_input');
        });
}

// Focusout
if (roleNameInput) {
    roleNameInput.addEventListener('focusout',
        () => {
            securityUserModule ?securityUserModule.inputFormGroupValidator('#role_name_input') : inputFormGroupValidator('#role_name_input');
        });
}

// Keypress: only numbers
if (roleNameInput) {
    roleNameInput.addEventListener('keypress',
        event_ => {
            inputOnlyAlphanumeric(event_);
        },
        false);
}

/* -------------------------------------------------------------------------------------------- */
/* ------------------------ Modal initialization ---------------------------------------------- */
/* -------------------------------------------------------------------------------------------- */

const renameRoleModal = $('#renameRoleModal');
if (renameRoleModal.length) {
    renameRoleModal.on('show.bs.modal',
        function (event) {
            const roleNameInputElt = document.getElementById('role_rename_input');
            roleNameInputElt.value = '';
        });
}

/* --------------------------------------------------------------------------------------------- */
/* ------------------------ Exported functions ------------------------------------------------- */
/* --------------------------------------------------------------------------------------------- */

// None yet

/* --------------------------------------------------------------------------------------------- */
/* ------------------------ User interactions that trigger ajax calls -------------------------- */
/* --------------------------------------------------------------------------------------------- */

/**
 * Save new role.
 */
const saveAddRoleBtn = document.getElementById('save-add-role-btn');
if (saveAddRoleBtn) {
    saveAddRoleBtn.addEventListener('click',
        () => {
            const roleNameInputElt = document.getElementById('role_name_input');
            if (!roleNameInputElt.value) {
                window.toastr.warning('No role name given.', 'Role not saved!');
                securityUserModule ? securityUserModule.inputFormGroupValidator('#role_name_input') : inputFormGroupValidator('#role_name_input');
                return;
            }

            const postData = {
                RoleName: roleNameInputElt.value
            };

            bareboneAjaxModule.makeAjaxRequest('POST',
                '/administration/save-new-role',
                postData,
                (responseStatus_, responseText_) => {
                    if (responseStatus_ === 201) {
                        window.toastr.success(responseText_, 'New role created');
                        securityUserModule ? securityUserModule.inputFormGroupSetError('#role_name_input', null) : inputFormGroupSetError('#role_name_input', null);
                        refreshPermissionsTabs();
                        resetAddRoleForm();
                    } else {
                        securityUserModule ? securityUserModule.inputFormGroupSetError('#role_name_input', responseText_ || responseStatus_) : inputFormGroupSetError('#role_name_input', responseText_ || responseStatus_);
                    }
                });
        });
}

/**
 * Update the UI with selected role information. Ajax GET.
 * @param {any} roleId_ - roleId
 */
export function viewSelectedRole(roleId_) {
    bareboneAjaxModule.makeAjaxRequest('GET', '/administration/read-role', { roleId_: roleId_ }, (responseStatus_, responseText_) => {
        if (responseStatus_ !== 200) {
            window.toastr.error(responseText_, 'Error reading role');
            return;
        }

        const responseDataJson = JSON.parse(responseText_).value;
        // responseJson.value is ReadRoleViewModel C# class
        const role = responseDataJson.role;

        // Use role name for dynamic buttons
        document.getElementById('unlink-role-btn').innerHTML = `Remove all permissions of role <b>${role.name}</b>`;
        document.getElementById('unlink-role-btn').attributes['data-name'] = role.name;
        document.getElementById('unlink-role-row').style.display = responseDataJson.selectedExtensions.length ? 'block' : 'none';
        document.getElementById('rename-role-btn').innerHTML = `Rename role <b>${role.name}</b>`;
        document.getElementById('rename-role-btn').attributes['data-name'] = role.name;
        document.getElementById('rename-role-div').style.display = 'block';

        // Selected extensions/permissions
        const rightListElt = document.getElementById('selectedRoleAssignedExtensionsList');
        // Clear
        rightListElt.innerHTML = '';
        // Fill
        if (responseDataJson.selectedExtensions.length) {
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
                    const permissionDisplays = selectedExtension.groupedBySectionPermissionDisplays[sectionName];
                    if (permissionDisplays.filter(p_ => p_.selected).length === 0) {
                        continue;
                    }
                    indexSection++;
                    cellElt.insertAdjacentHTML('beforeend', `<span class="text-muted">${sectionName}</span>
                <select multiple disabled class="assigned-permissions" id="selected-extension-${indexExtension}-section-${indexSection}"></select>`);
                    const selectElt = document.getElementById(`selected-extension-${indexExtension}-section-${indexSection}`);
                    for (const permissionDisplay of permissionDisplays) {
                        if (permissionDisplay.selected) {
                            selectElt.insertAdjacentHTML('beforeend', `<option value="${permissionDisplay.permissionEnumValue}" selected="true" title="${permissionDisplay.description}">${permissionDisplay.shortName}</option>`);
                        }
                    }
                }
            }
        } else {
            rightListElt.insertAdjacentHTML('beforeend', '<tr><td colspan="2"><i class="fas fa-folder-open"></i> No assigned permission</td></tr>');
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

    bareboneAjaxModule.makeAjaxRequest('POST', '/administration/update-role-permission', params, (responseStatus_, responseText_) => {
        if (responseStatus_ === 204) {
            window.toastr.success(responseText_, 'Changes saved');
            // If this role's permissions are currently viewed, refresh
            const viewedRoleDropdown = document.getElementById('availableRoles');
            const viewedRole = viewedRoleDropdown.options[viewedRoleDropdown.selectedIndex];
            const viewedRoleName = viewedRole.value;
            const viewedRoleId = viewedRole.attributes['data-id'].value;

            if (event_.params.data.id === viewedRoleName) {
                viewSelectedRole(viewedRoleId);
            }
        } else if (responseStatus_ === 400) {
            window.toastr.error(responseText_, 'Permission to role link NOT updated');
        } else {
            window.toastr.error('Cannot update permission to role link. See logs for errors', 'Error');
        }
    });
}

/**
 * Ajax call to update role name. Ajax POST.
 */
export function saveEditRoleName() {
    const roleNameInputElt = document.getElementById('role_rename_input');
    if (!roleNameInputElt.value) {
        window.toastr.warning('No new role name given.', 'Changes not saved!');
        securityUserModule ? securityUserModule.inputFormGroupValidator('#role_rename_input') : inputFormGroupValidator('#role_rename_input');
        return;
    }
    const roleSelectElt = document.getElementById('availableRoles');
    const roleId = roleSelectElt.options[roleSelectElt.selectedIndex].getAttribute('data-id');
    const postData = {
        RoleId: roleId,
        RoleName: roleNameInputElt.value
    };

    bareboneAjaxModule.makeAjaxRequest('POST', '/administration/update-role', postData, (responseStatus_, responseText_) => {
        if (responseStatus_ === 201) {
            window.toastr.success(responseText_, 'Changes saved');
            $('#renameRoleModal').modal('hide');
            refreshPermissionsTabs().then(() => {
                $('#availableRoles').val(roleNameInputElt.value); // Select the option with value of new role name
                $('#availableRoles').trigger('change'); // Notify any JS components that the value changed
            });
        } else if (responseStatus_ === 400) {
            window.toastr.error(responseText_, 'Cannot update role');
        } else {
            window.toastr.error('Cannot update role. See logs for errors', 'Error');
        }
    });
}

export function deleteAllPermissionsOfRole(roleName_) {
    bareboneAjaxModule.makeAjaxRequest('DELETE', `/administration/remove-role-permissions/${roleName_}`, {}, (responseStatus_, responseText_) => {
        if (responseStatus_ === 204) {
            window.toastr.success(`All permissions removed from role ${roleName_}`, 'Delete OK');
            refreshPermissionsTabs();
        } else if (responseStatus_ === 400) {
            window.toastr.error(responseText_, 'Role\'s permissions NOT deleted');
        } else {
            window.toastr.error('Cannot delete role\'s permissions. See logs for details', 'Error');
        }
    });
}

export function deleteRole(roleNameList_) {
    bareboneAjaxModule.makeAjaxRequest('DELETE', `/administration/delete-role/${roleNameList_}`, {}, (responseStatus_, responseText_) => {
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
        bareboneAjaxModule.makeAjaxRequest('GET', '/administration/read-permissions-grants', null, (responseStatus_, responseText_) => {
            document.getElementById('GrantPermissionsTable').innerHTML = responseText_;
            // Reattach the event listeners
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
            resolve();
        });
    });
}

function reloadEditRoleHtmlView() {
    return new window.Promise((resolve, reject) => {
        bareboneAjaxModule.makeAjaxRequest('GET', '/administration/edit-role-tab', null, (responseStatus_, responseText_) => {
            document.getElementById('edit_role_form').innerHTML = responseText_;
            resolve();
        });
    });
}

function reloadBulkDeleteTab() {
    return new window.Promise((resolve, reject) => {
        bareboneAjaxModule.makeAjaxRequest('GET', '/administration/bulk-delete-role-tab', null, (responseStatus_, responseText_) => {
            document.getElementById('availableRolesForDelete').innerHTML = responseText_;
            resolve();
        });
    });
}

// Refresh the permissions tabs and return the promise used for this operation
function refreshPermissionsTabs() {
    return Promise.all([reloadGrantPermissionsHtmlView(), reloadEditRoleHtmlView(), reloadBulkDeleteTab()])
        .then(() => {
            let unlinkRoleBtn = document.getElementById('unlink-role-btn');
            unlinkRoleBtn.addEventListener('click', () => {
                deleteAllPermissionsOfRole(unlinkRoleBtn.attributes['data-name']);
            });
            useSelect2();
        })
        .catch((error) => console.log(error));
}

function resetAddRoleForm() {
    document.getElementById('role_name_input').value = '';
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
