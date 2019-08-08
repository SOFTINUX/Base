// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

///<reference path = '../../SoftinuxBase.Barebone/Scripts/barebone_ajax.js' />
///<reference path = './security_user.js' />

'use strict';

import makeAjaxRequest from '/Scripts/barebone_ajax.js';
import { inputFormGroupSetError, inputFormGroupValidator } from '/Scripts/security_user.js';

// Manage click on buttons
Array.from(document.querySelectorAll('button')).forEach(
    clickedElement_ => {
        clickedElement_.addEventListener('click',
            () => {
                const addRoleArea = document.querySelector('#add-role-area');
                const editRoleArea = document.querySelector('#edit-role-area');

                switch (clickedElement_.id) {
                    case 'add-role-btn':
                        editRoleArea.style.display = 'none';
                        addRoleArea.style.display = addRoleArea.style.display !== 'none' ? 'none' : 'block';
                        break;
                    case 'edit-role-btn':
                        addRoleArea.style.display = 'none';
                        editRoleArea.style.display = editRoleArea.style.display !== 'none' ? 'none' : 'block';
                        break;
                    case 'cancel-add-role-btn':
                    case 'cancel-edit-role-btn':
                        editRoleArea.style.display = 'none';
                        addRoleArea.style.display = 'none';
                        break;
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

/*----------------------------------------------------------------*/
/*------------------------ expose functions ----------------------*/
/*----------------------------------------------------------------*/
window.passSelectedRoleOnEdition = passSelectedRoleOnEdition;
window.permissionCheckBoxClick = permissionCheckBoxClick;
window.savePermission = savePermission;
window.saveEditRole = saveEditRole;
window.deleteRole = deleteRole;
window.removeRoleLink = removeRoleLink;

/*----------------------------------------------------------------*/
/*------------------------ events handlers ------------------------*/
/*----------------------------------------------------------------*/
document.getElementById('editRoleRightExtensionsList').addEventListener('click', event_ => {
    rowClicked(event_.target.closest('div.row'));
}, false);

document.getElementById('editRoleLeftExtensionsList').addEventListener('click', event_ => {
    rowClicked(event_.target.closest('div.row'));
}, false);

/**
* Toggle collapsed state for permissions administration table.
*/
document.getElementById('collapse').addEventListener('click', event_ => {
    let element = event_.target;
    if (element.tagName === 'I')
        element = element.parentNode;
    const subEl = document.getElementsByClassName('row collapse');

    if (element.dataset.state === 'closed') {
        element.dataset.state = 'open';
        // TODO change icon to open double chevron

        // open all the collapsed children
        const elements = Array.from(document.getElementsByClassName('extension-row collapsed'));
        for (let item of elements) {
            item.classList.remove('collapsed');
            item.setAttribute('aria-expanded', 'true');
        }
        for (let item of subEl) {
            item.classList.add('in');
        }
    } else {
        element.dataset.state = 'closed';
        // TODO change icon to closed double chevron

        // collapse all the children
        const elementRow = Array.from(document.getElementsByClassName('extension-row'));
        for (let item of elementRow) {
            item.classList.add('collapsed');
            item.setAttribute('aria-expanded', 'false');
        }
        for (let item of subEl) {
            item.classList.remove('in');
        }
    }
}, false);

/**
    * Handle the click on pseudo-dropdown that displays permission level:
    * set the label, set the value to hidden input.
    */
document.getElementById('acl-sel').addEventListener('click', event_ => {
    let clickedLiElt = event_.target.closest('li');
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
});

document.getElementById('role_name_input').addEventListener('change', () => {
    inputFormGroupValidator('#role_name_input');
});

// Focusout
document.getElementById('role_name_input').addEventListener('focusout', () => {
    inputFormGroupValidator('#role_name_input');
});
/*----------------------------------------------------------------*/
/*------------------------ functions -----------------------------*/
/*----------------------------------------------------------------*/

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

    let newElts = [];
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

    for (let newElt of newElts) {
        document.getElementById(`${clickedElement_.dataset.tolist}`).insertAdjacentHTML('beforeend', newElt);
    }

    for (let item of selectedElts) {
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

    $('#myModal').modal('show');
    //document.getElementById('myModal').classList.toggle('in');
    //document.getElementById('myModal').classList.toggle('show');
}

/*---------------------------------------------------------------------------------------------*/
/*------------------------ User interactions that trigger ajax calls --------------------------*/
/*---------------------------------------------------------------------------------------------*/

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

    let selectedExtensions = [];
    const addRoleRightExtensionsList = document.getElementById('addRoleRightExtensionsList');
    for (let listOption of addRoleRightExtensionsList.querySelectorAll('option')) {
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
            location.reload();
        } else {
            inputFormGroupSetError('#role_name_input', responseText_ ? responseText_ : responseStatus_);
        }
    });
});

/**
 * Update the UI with selected role information. Ajax GET.
 * @param {any} roleId_ - roleId
 */
export function passSelectedRoleOnEdition(roleId_) {
    document.getElementById('edit-role-group').classList.remove('has-error');
    $.ajax('/administration/read-role', { data: { 'roleId_': roleId_ } }).done(function (data_) {
        // data_.value is ReadRoleViewModel C# class
        const role = data_.value.role;

        // Role name
        document.getElementById('edit_role_name_input').value = role.name;

        // Role ID
        document.getElementById('editRoleId').value = roleId_;

        // Available extensions (left list)
        let leftListElt = document.getElementById('editRoleLeftExtensionsList');
        // Clear
        leftListElt.innerHTML = '';
        // Fill
        for (let extension of data_.value.availableExtensions) {
            leftListElt.insertAdjacentHTML('beforeend', `<div class="row"><div class="col-md-12"><span name="${extension}">${extension}</span></div></div>`);
        }

        // Selected extensions (right list)
        let rightListElt = document.getElementById('editRoleRightExtensionsList');
        // Clear
        rightListElt.innerHTML = '';
        // Fill
        for (let extension of data_.value.selectedExtensions) {
            rightListElt.insertAdjacentHTML('beforeend', `<div class="row">
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
 * @param {HTMLCheckboxElement} clickedCheckbox_ - permission level checkbox
 */
export function permissionCheckBoxClick(clickedCheckbox_) {
    const splittedId = clickedCheckbox_.id.split('_');
    const baseId = splittedId[0] + '_' + splittedId[1];
    const writeCheckbox = document.getElementById(`${baseId}_WRITE`);
    const readCheckbox = document.getElementById(`${baseId}_READ`);
    var _activeCheckedPermissions = 'NEVER';

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
    }
    else {
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
 * Ajax call to update data: role-extension-permission link update. Ajax GET (TODO change to POST).
 * @param {any} extension_ - extension
 * @param {any} roleName_ - role name
 * @param {any} permission_ - permission (enum value)
 */
export function savePermission(extension_, roleName_, permission_) {
    const params = {
        'roleName_': roleName_,
        'permissionValue_': permission_,
        'extension_': extension_
    };

    makeAjaxRequest('POST', '/administration/update-role-permission', params, (responseStatus_, responseText_) => {
        if (responseStatus_ === 201) {
            window.toastr.success(responseText_, 'Changes saved');
        } else {
            window.toastr.error('Cannot update role permissions. See logs for errors', 'Error');
        }
    });
}

/**
 * Ajax call to update data: role with its related data update. Ajax POST.
 */
export function saveEditRole() {
    let _grants = [];
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
            _grants.push({ Extension: _extension, PermissionValue: _permission });
        else {
            window.toastr.error('Cannot update role from client', 'Error');
            _noError = false;
        }
    });

    if (!_noError) return;

    const postData = {
        RoleId: $('#editRoleId').val(),
        RoleName: $('#edit_role_name_input').val(),
        Grants: _grants
    };

    makeAjaxRequest('POST', '/administration/update-role', postData, (responseStatus_, responseText_) => {
        if (responseStatus_ === 201) {
            window.toastr.success(responseText_, 'Changes saved');
            location.reload();
        } else {
            window.toastr.error('Cannot update role. See logs for errors', 'Error');
        }
    });
}

export function deleteRole(role_) {
    const postData = {
        'roleName_': role_
    };

    makeAjaxRequest('POST', '/administration/delete-role', postData, (responseStatus_, responseText_) => {
        if (responseStatus_ === 201) {
            window.toastr.success(responseText_, 'Role deleted');
            location.reload();
        } else {
            window.toastr.error('Cannot delete role. See logs for errors', 'Error');
        }
    });
}