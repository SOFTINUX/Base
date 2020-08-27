// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

/* eslint-disable import/no-absolute-path */

/// <reference path = '../../SoftinuxBase.Barebone/Scripts/barebone_ajax.js' />
/// <reference path = './security_user.js' />

'use strict';

import makeAjaxRequest from '/Scripts/barebone_ajax.js';
import {inputFormGroupSetError, inputFormGroupValidator} from '/Scripts/security_user.js';
import {inputOnlyNumbers} from '/Scripts/toolbox.js';

/* Select 2 Boostrap 4 Theme for all Select2
   @see https://github.com/select2/select2/issues/2927
*/
// $.fn.select2.defaults.set('theme', 'bootstrap');

// template for unlink icon in edit role selectbox
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
window.passSelectedRoleOnEdition = passSelectedRoleOnEdition;
window.permissionCheckBoxClick = permissionCheckBoxClick;
window.savePermission = savePermission;
window.saveEditRole = saveEditRole;
window.deleteRole = deleteRole;
window.removeRoleLink = removeRoleLink;

/* ---------------------------------------------------------------- */
/* ------------------------ Global Constants ---------------------- */

/* ---------------------------------------------------------------- */

/**
 * make global constant of available extension.
 */
function defineExtensionsListOriginalState() {
    Object.defineProperty(window, 'RoleExtensionsListOriginalState', {
        value: document.getElementById('addRoleLeftExtensionsList').innerHTML,
        configurable: false,
        writable: false
    });
}

/* ---------------------------------------------------------------- */
/* ------------------------ events handlers ----------------------- */
/* ---------------------------------------------------------------- */
attachEditRoleChevronButtonsEventListener();
attachEditEventListener();

document.getElementById('editRoleLeftExtensionsList').addEventListener('click', event_ => {
    rowClicked(event_.target.closest('div.row'));
}, false);

/**
 * Handle the click on pseudo-dropdown that displays permission level:
 * set the label, set the value to hidden input.
 */
document.getElementById('acl-sel').addEventListener('click', event_ => {
    const clickedLiElt = event_.target.closest('li');
    clickedLiElt.closest('.bs-dropdown-to-select-acl-group').querySelectorAll('[data-bind="bs-drp-sel-acl-label"]')[0].innerText = clickedLiElt.innerText;
    document.getElementById('newRolePermission').value = clickedLiElt.dataset.permissionlvl;
}, false);

document.getElementById('save-edit-role-btn').addEventListener('click', () => {
    if (!document.getElementById('edit_role_name_input').value) {
        window.toastr.warning('No new role name given.', 'Role not updated!');
        inputFormGroupValidator('#edit_role_name_input');
        return;
    }

    saveEditRole();
    resetEditRoleForm();
});

document.getElementById('cancel-add-role-btn').addEventListener('click', () => {
    resetAddRoleForm();
});

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

document.getElementById('unlink-role-btn').addEventListener('click', () => {
    unlinkRolePermissionOnAllExtensions(document.getElementById('edit_role_normalizedName').value);
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

document.getElementById('role_name_input').addEventListener('change', () => {
    inputFormGroupValidator('#role_name_input');
});

// Focusout
document.getElementById('role_name_input').addEventListener('focusout', () => {
    inputFormGroupValidator('#role_name_input');
});

// only numbers
document.getElementById('role_name_input').addEventListener('keypress', event_ => {
    inputOnlyNumbers(event_);
}, false);
/* ---------------------------------------------------------------- */
/* ------------------------ functions ----------------------------- */

/* ---------------------------------------------------------------- */

function rowClicked(event_) {
    if (event_.classList.contains('active')) {
        event_.classList.remove('active');
    } else {
        event_.classList.add('active');
    }
}

/**
 * Move selected item(s) over left listbox to right listbox.
 *
 * @param {object} clickedElement_ - clicked HTML Button element
 * @param {string} transform_ - how to transform the event target :
 * - "to-left": transform a html option element to a html span element
 * - "to-right": transform a html span element to an html option element
 * - other value: clone the target.
 */
function btnChevronMoveExtension(clickedElement_, transform_) {
    if (clickedElement_.tagName === 'I')
        clickedElement_ = clickedElement_.parentNode;

    const bulk = clickedElement_.hasAttribute('data-bulk-move');

    const rootElt = document.getElementById(`${clickedElement_.dataset.fromlist}`);
    const selectedElts = transform_
        // if transform_ is defined, the selected list items are div elements, else select's options
        ? bulk ? rootElt.querySelectorAll(' div.row') : rootElt.querySelectorAll(' div.row.active')
        : bulk ? rootElt.querySelectorAll('option') : rootElt.selectedOptions;

    if (selectedElts.length === 0) {
        const emptyExtensionList = bulk ? 'You must have at least one extension in the list' : 'You must select at least one extension in the list';
        const emptyExtensionListTitle = 'No extension to move';
        window.toastr.warning(emptyExtensionList, emptyExtensionListTitle);
        return;
    }

    let newElts;
    switch (transform_) {
        case 'to-left':
            newElts = Array.from(selectedElts, createMovedElementLeft);
            break;
        case 'to-right':
            newElts = Array.from(selectedElts, createMovedElementRight);
            break;
        default:
            newElts = Array.from(selectedElts, currentElt_ => currentElt_.outerHTML);
            break;
    }

    for (const newElt of newElts) {
        document.getElementById(`${clickedElement_.dataset.tolist}`).insertAdjacentHTML('beforeend', newElt);
    }

    for (const item of selectedElts) {
        item.remove();
    }
}

/**
 * Create a html fragment containing mostly a span element styled as "modified",
 * using text from a html fragment containing mostly a span element.
 * @param {Object} target_ - html div element
 * @return {string} html div element outer html
 */
function createMovedElementLeft(target_) {
    return `<div class="row modified">
                <div class="col-md-12">${target_.querySelectorAll('span')[0].outerHTML}</div>
            </div>`;
}

/**
 * Create a html fragment containing mostly a span and a select elements styled as "modified",
 * using text from a html fragment containing mostly a span and a select elements.
 * @param {object} target_ - html div element
 * @return {string} html div element outer html
 */
function createMovedElementRight(target_) {
    const extension = target_.querySelectorAll('span')[0].getAttribute('name');
    return `<div class="row modified">
                <div class="col-md-6">
                    <span name="${extension}">${extension}</span>
                </div>
                <div class="col-md-6">
                    <select>
                        <option value="None">None</option>
                        <option value="Read" selected>Read</option>
                        <option value="Write">Write</option>
                        <option value="Admin">Admin</option>
                    </select>
                </div>
            </div>`;
}

export function removeRoleLink(element_) {
    if (!element_) {
        console.log('You must pass this as argument of removeRoleLink onlick.');
        return;
    }
    const splitted = element_.parentNode.dataset.roleId.split('_');

    document.getElementById('moduleName').innerText = splitted[0];
    document.getElementById('selectedRoleName').innerText = splitted[1];

    unlinkRolePermissionOnExtension(splitted[0], splitted[1]);
}

/* --------------------------------------------------------------------------------------------- */
/* ------------------------ User interactions that trigger ajax calls -------------------------- */
/* --------------------------------------------------------------------------------------------- */

/**
 * Save new role with its extensions and permission.
 */
document.getElementById('save-add-role-btn').addEventListener('click', () => {
    const roleNameInputElt = document.getElementById('role_name_input');
    if (!roleNameInputElt.value) {
        window.toastr.warning('No role name given.', 'Role not saved!');
        inputFormGroupValidator('#role_name_input');
        return;
    }

    const selectedExtensions = [];
    const addRoleRightExtensionsList = document.getElementById('addRoleRightExtensionsList');
    for (const listOption of addRoleRightExtensionsList.querySelectorAll('option')) {
        selectedExtensions.push(listOption.value);
    }

    const postData = {
        RoleName: roleNameInputElt.value,
        Extensions: selectedExtensions,
        PermissionValue: document.getElementById('newRolePermission').value
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
export function passSelectedRoleOnEdition(roleId_) {
    document.getElementById('edit-role-group').classList.remove('has-error');
    makeAjaxRequest('GET', '/administration/read-role', {roleId_: roleId_}, (responseStatus_, responseText_) => {
        if (responseStatus_ !== 200) {
            window.toastr.error(`Serveur return code ${responseStatus_} whith response: ${responseText_}`, 'Error');
            return;
        }

        const responseDataJson = JSON.parse(responseText_).value;
        // responseJson.value is ReadRoleViewModel C# class
        const role = responseDataJson.role;

        // Role name
        document.getElementById('edit_role_name_input').value = role.name;

        // Role ID
        document.getElementById('edit_role_id').value = role.id;

        // Role Normalized Name
        document.getElementById('edit_role_normalizedName').value = role.normalizedName;

        // Role concurrency stamp
        document.getElementById('edit_role_concurrencyStamp').value = role.concurrencyStamp;

        // Available extensions (left list)
        const leftListElt = document.getElementById('editRoleLeftExtensionsList');
        // Clear
        leftListElt.innerHTML = '';
        // Fill
        for (const extension of responseDataJson.availableExtensions) {
            leftListElt.insertAdjacentHTML('beforeend', `<div class="row"><div class="col-md-12"><span name="${extension}">${extension}</span></div></div>`);
        }

        // Selected extensions/permissions (right list)
        const rightListElt = document.getElementById('editRoleRightExtensionsList');
        // Clear
        rightListElt.innerHTML = '';
        // Fill
        let indexSelect = -1;
        for (const selectedExtension of responseDataJson.selectedExtensions) {
            indexSelect++;
            rightListElt.insertAdjacentHTML('beforeend', `<div class="row">
                            <div class="col-md-6">
                            <i class="fas fa-cubes"></i>
                                <span name="${selectedExtension.extensionName}">${selectedExtension.extensionName}</span>
                            </div>
                            <div class="col-md-6">
                                <select multiple style="width:100%;" id="selected-extension-${indexSelect}"></select>
                            </div>
                        </div>`);
            const selectElt = document.getElementById(`selected-extension-${indexSelect}`);
            let indexOptGroup = -1;
            for (const sectionName of Object.keys(selectedExtension.groupedBySectionPermissionDisplays)) {
                indexOptGroup++;
                selectElt.insertAdjacentHTML('beforeend', `<optgroup label="${sectionName}" id="selected-extension-${indexSelect}-optgroup-${indexOptGroup}"></optgroup>`);
                const optGroupElt = document.getElementById(`selected-extension-${indexSelect}-optgroup-${indexOptGroup}`);
                for (const permissionDisplay of selectedExtension.groupedBySectionPermissionDisplays[sectionName]) {
                    optGroupElt.insertAdjacentHTML('beforeend', `<option value="${permissionDisplay.permissionEnumValue}" selected="${permissionDisplay.selected}">${permissionDisplay.shortName} (${permissionDisplay.description})</option>`);
                }
            }
        }
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
 * TODO remove - obsolete function
 * Click in permission checkbox. Calls savePermission().
 * @param {HTMLCheckboxElement} clickedCheckbox_ - permission level checkbox
 */
export function permissionCheckBoxClick(clickedCheckbox_) {
    const splittedId = clickedCheckbox_.id.split('_');
    const baseId = splittedId[0] + '_' + splittedId[1];
    const writeCheckbox = document.getElementById(`${baseId}_WRITE`);
    const readCheckbox = document.getElementById(`${baseId}_READ`);
    let _activeCheckedPermissions = 'NEVER';

    if (clickedCheckbox_.checked) {
        // when checking, impacted checkboxes become checked and disabled
        switch (splittedId[2]) {
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
        savePermission(splittedId[0], splittedId[1], splittedId[2]);
    } else {
        // when unchecking, first next checkbox becomes enabled
        switch (splittedId[2]) {
            case 'ADMIN':
                writeCheckbox.disabled = false;
                _activeCheckedPermissions = 'WRITE';
                break;
            case 'WRITE':
                readCheckbox.disabled = false;
                _activeCheckedPermissions = 'READ';
                break;
        }
        savePermission(splittedId[0], splittedId[1], _activeCheckedPermissions);
    }
}

/**
 * TODO remove - obsolete function if not reused (see new function updateRolePermission())
 * Ajax call to update data: role-extension-permission link update. Ajax POST.
 * @param {any} extension_ - extension
 * @param {any} roleName_ - role name
 * @param {any} permission_ - permission (enum value)
 */
export function savePermission(extension_, roleName_, permission_) {
    const params = {
        RoleName: roleName_,
        PermissionValue: permission_,
        Extension: extension_
    };

    makeAjaxRequest('POST', '/administration/update-role-permission', params, (responseStatus_, responseText_) => {
        if (responseStatus_ === 200) {
            window.toastr.success(responseText_, 'Changes saved');
        } else if (responseStatus_ === 400) {
            window.toastr.error(responseText_, 'Role extension link NOT updated');
        } else {
            window.toastr.error('Cannot update role-extension\'s permission. See logs for errors', 'Error');
        }
    });
}

/**
 * Ajax call to update data: role with its related data update. Ajax POST.
 */
export function saveEditRole() {
    const _grants = [];
    let _noError = true;

    Array.prototype.forEach.call(document.querySelectorAll('#editRoleRightExtensionsList>div.row'), function (elt_) {
        let _extension, _permission;
        Array.prototype.forEach.call(elt_.querySelectorAll('div'), function (subElt_) {
            if (subElt_.querySelector('span'))
                _extension = elt_.querySelector('span').getAttribute('name');
            if (subElt_.querySelector('select'))
                _permission = subElt_.querySelector('select option:checked').value;
        });

        if (_extension && _permission)
            _grants.push({Extension: _extension, PermissionValue: _permission});
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

export function unlinkRolePermissionOnExtension(extensionName_, roleName_) {
    makeAjaxRequest('DELETE', `/administration/unlink-role-extension/${roleName_}/${extensionName_}`, {}, (responseStatus_, responseText_) => {
        if (responseStatus_ === 204) {
            window.toastr.success(`Role ${roleName_} unlinked from extension ${extensionName_}`, 'Link deleted');
            refreshPermissionsTabs();
        } else if (responseStatus_ === 400) {
            window.toastr.error(responseText_, 'Role extension link NOT deleted');
        } else {
            window.toastr.error('Cannot delete link. See logs for details', 'Error');
        }
    });
}

export function unlinkRolePermissionOnAllExtensions(roleName_) {
    makeAjaxRequest('DELETE', `/administration/unlink-role-all-extensions/${roleName_}`, {}, (responseStatus_, responseText_) => {
        if (responseStatus_ === 204) {
            window.toastr.success(`Role ${roleName_} unlinked from extensions`, 'Link deleted');
            refreshPermissionsTabs();
        } else if (responseStatus_ === 400) {
            window.toastr.error(responseText_, 'Role extension link NOT deleted');
        } else {
            window.toastr.error('Cannot delete link. See logs for details', 'Error');
        }
    });
}

export function deleteRole(roleNameList_) {
    makeAjaxRequest('DELETE', `/administration/delete-role/${roleNameList_}`, {}, (responseStatus_, responseText_) => {
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

function reloadRolesHtmlView() {
    return new window.Promise((resolve, reject) => {
        makeAjaxRequest('GET', '/administration/edit-role-tab', null, (responseStatus_, responseText_) => {
            document.getElementById('edit-role-tab-content').innerHTML = responseText_;
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
    Promise.all([reloadGrantPermissionsHtmlView(), reloadRolesHtmlView(), reloadBulkDeleteTab()])
        .then(() => {
            document.getElementById('editRoleLeftExtensionsList').addEventListener('click', event_ => {
                rowClicked(event_.target.closest('div.row'));
            }, false);

            document.getElementById('unlink-role-btn').addEventListener('click', () => {
                unlinkRolePermissionOnAllExtensions(document.getElementById('edit_role_normalizedName').value);
            });
            attachEditRoleChevronButtonsEventListener();

            useSelect2();
        })
        .catch((error) => console.log(error));
}

function resetAddRoleForm() {
    document.getElementById('role_name_input').value = '';
    document.getElementById('newRolePermission').value = 1;
    document.querySelector('[data-bind="bs-drp-sel-acl-label"]').innerText = 'Read';
    document.getElementById('addRoleRightExtensionsList').innerHTML = '';
    document.getElementById('addRoleLeftExtensionsList').innerHTML = window.RoleExtensionsListOriginalState;
}

function resetEditRoleForm() {
    document.getElementById('editRoleLeftExtensionsList').innerHTML = '';
    document.getElementById('editRoleRightExtensionsList').innerHTML = '';
    document.getElementById('edit_role_name_input').value = '';
    document.getElementById('editRoleId').value = '';
    document.getElementById('edit_role_id').value = '';
    document.getElementById('edit_role_normalizedName').value = '';
    document.getElementById('edit_role_concurrencyStamp').value = '';
}

function attachEditEventListener() {
    window.addEventListener('DOMContentLoaded', () => {
        if (document.getElementById('addRoleLeftExtensionsList').innerHTML)
            defineExtensionsListOriginalState();
        else
            document.getElementById('addRoleLeftExtensionsList').innerHTML = '<option value="ERROR">Error. See logs.</option>';
    });
}

function attachEditRoleChevronButtonsEventListener() {
    Array.from(document.querySelectorAll('button')).forEach(
        clickedElement_ => {
            clickedElement_.addEventListener('click',
                () => {
                    switch (clickedElement_.id) {
                        // Add selected/unselected extensions management
                        case 'addRoleBtnRight':
                        case 'addRoleBtnAllRight':
                        case 'addRoleBtnLeft':
                        case 'addRoleBtnAllLeft':
                            btnChevronMoveExtension(clickedElement_, '');
                            break;
                        // Edit selected/unselected extensions management
                        case 'editRoleBtnRight':
                        case 'editRoleBtnAllRight':
                        case 'editRoleBtnLeft':
                        case 'editRoleBtnAllLeft':
                            btnChevronMoveExtension(clickedElement_, clickedElement_.id.toLowerCase().includes('left') ? 'to-left' : 'to-right');
                            break;
                        default:
                            break;
                    }
                }, false);
        }
    );
}

// Initialize Select2.org select elements.
function useSelect2() {
    $('.select2bs4').select2({
        minimumResultsForSearch: Infinity,
        width: null,
        theme: 'bootstrap4',
        templateSelection: iformat,
        templateResult: iformat,
        allowHtml: true
    });
}
