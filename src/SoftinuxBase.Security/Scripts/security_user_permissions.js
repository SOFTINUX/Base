// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

// Manage click on buttons
[].forEach.call(document.querySelectorAll('button'),
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
    });

/*----------------------------------------------------------------*/
/*------------------------ events handlers ------------------------*/
/*----------------------------------------------------------------*/

document.getElementById('editRoleRightExtensionsList').addEventListener('click', event_ => {
    rowClicked(event_.target.closest('div.row'));
}, false);

document.getElementById('editRoleLeftExtensionsList').addEventListener('click', event_ => {
    rowClicked(event_.target.closest('div.row'));
}, false);

// permissions administration: collapsing
document.getElementById('collapse').addEventListener('click', (event_) => {
    let element = event_.target;
    if (element.tagName === 'I')
        element = element.parentNode;
    const _subEl = document.getElementsByClassName('row collapse');

    if (element.dataset.state === 'closed') {
        element.dataset.state = 'open';
        // TODO change icon to open double chevron

        // open all the collapsed children
        const _elements = Array.from(document.getElementsByClassName('extension-row collapsed'));
        for (let item of _elements) {
            item.classList.remove('collapsed');
            item.setAttribute('aria-expanded', 'true');
        }
        for (let item of _subEl) {
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
        for (let item of _subEl) {
            item.classList.remove('in');
        }
    }
}, false);

// permission dropdown
document.getElementById('acl-sel').addEventListener('click', event_ => {
    let clickedLiElt = event_.target.closest('li');
    clickedLiElt.closest('.bs-dropdown-to-select-acl-group').querySelectorAll('[data-bind="bs-drp-sel-acl-label"]')[0].innerText = clickedLiElt.innerText;
    document.getElementById('newRolePermission').value = clickedLiElt.dataset.permissionlevel;
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
 * Move selected item(s) over left listbox to right listbox
 *
 * @param {object} event_ - HTML Button element
 * @param {string} transform_ - how to transform the event target :
 * - "to-left": transform the html option element to a html span element
 * - "to-right": transform a html span element to an html option element
 * - other value making cloning on the target.
 */
function btnChevronMoveExtension(event_, transform_) {
    if (event_.tagName === 'I')
        event_ = event_.parentNode;

    const bulk = event_.hasAttribute('data-bulk-move');

    const rootElt = document.getElementById(`${event_.dataset.fromlist}`);
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
        document.getElementById(`${event_.dataset.tolist}`).insertAdjacentHTML('beforeend', newElt);
    }

    for (let item of selectedElts) {
        item.remove();
    }
}

/**
 * Create a html fragment containing mostly a span element, from a html fragment containing mostly a span element.
 * @param {Object} target_ - html div element
 * @return {string} html div
 */
function createMovedElementLeft(target_) {
    return `<div class="row modified">
                <div class="col-md-12">${target_.querySelectorAll('span')[0].outerHTML}</div>
            </div>`;
}

/**
 * Create a html fragment containing mostly a span and a select elements, from a html fragment contaning mostly a span and a select elements.
 * @param {object} target_ - html div element
 * @return {string} html div
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

function removeRoleLink(element_) {
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

// save new role with its extensions and permission
document.getElementById('save-add-role-btn').addEventListener('click', () => {

    const roleNameInputElt = document.getElementById('role_name_input');
    if (!roleNameInputElt.value) {
        window.toastr.warning('No role name given.', 'Role not saved!');
        inputFormGroupValidator('#role_name_input');
        return;
    }

    let _selectedExtensions = [];
    const addRoleRightExtensionsList = document.getElementById('addRoleRightExtensionsList');
    for (let listOption of addRoleRightExtensionsList.querySelectorAll('option')) {
        _selectedExtensions.push(listOption.value);
    }

    const postData = {
        RoleName: roleNameInputElt.value,
        Extensions: _selectedExtensions,
        PermissionValue: document.getElementById('newRolePermission').value
    };

    $.ajax('/administration/save-new-role', { method: 'POST', data: postData })
        .done(function (data_) {
            window.toastr.success(data_, 'New role created');
            inputFormGroupSetError('#role_name_input', null);
            location.reload();
        })
        .fail(function (jqXhr_, testStatus_) {
            const errMsg = jqXhr_.responseText ? jqXhr_.responseText : testStatus_;
            inputFormGroupSetError('#role_name_input', errMsg);
        });
});

/**
 * Update the UI with selected role information. Ajax GET.
 * @param {any} roleId_ - roleId
 */
function passSelectedRoleOnEdition(roleId_) {
    document.getElementById('edit-role-group').classList.remove('has-error');
    $.ajax('/administration/read-role', { data: { 'roleId_': roleId_ } }).done(function (data_) {
        // data_.value is ReadRoleViewModel
        const role = data_.value.role;

        // for (let i = 0, len = role.length; i < len; i++) {
        //     document.getElementById(`edit_role_${key}`).value = role[key];
        // }

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