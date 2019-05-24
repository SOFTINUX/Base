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

/*----------------------------------------------------------------*/
/*------------------------ events handler ------------------------*/
/*----------------------------------------------------------------*/

document.getElementById('editRoleRightExtensionsList').addEventListener('click', event_ => {
    row_clicked(event_.target.closest('div.row'));
}, false);

document.getElementById('editRoleLeftExtensionsList').addEventListener('click', event_ => {
    row_clicked(event_.target.closest('div.row'));
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
        for (let _i = 0, _ilen = _subEl.length; _i < _ilen; _i++) {
            _subEl[_i].classList.add('in');
        }
    } else {
        element.dataset.state = 'closed';
        // TODO change icon to closed double chevron

        // collapse all the children
        const elementRow = Array.from(document.getElementsByClassName('extension-row'));
        for (let item of elementRow) {
            if (!item.classList.contains('collapsed')) {
                item.classList.add('collapsed');
                item.setAttribute('aria-expanded', 'false');
            }
        }
        for (let _i = 0, _ilen = _subEl.length; _i < _ilen; _i++) {
            _subEl[_i].classList.remove('in');
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
        input_form_group_validator('#edit_role_name_input');
        return;
    }

    saveEditRole();
});

document.getElementById('role_name_input').addEventListener('change', () => {
    input_form_group_validator('#role_name_input');
});

// Focusout
document.getElementById('role_name_input').addEventListener('focusout', () => {
    input_form_group_validator('#role_name_input');
});

/*----------------------------------------------------------------*/
/*------------------------ functions -----------------------------*/
/*----------------------------------------------------------------*/

function removeRoleLink(element_) {
    if (!element_) {
        console.log('You must pass this as argument of removeRoleLink onlick.');
        return;
    }
    const splitted = element_.parentNode.dataset.roleId.split('_');

    document.getElementById('moduleName').innerText = splitted[0];
    document.getElementById('selectedRoleName').innerText = splitted[1];

    $('#myModal').modal('show');
}

/*---------------------------------------------------------------------------------------------*/
/*------------------------ User interactions that trigger ajax calls --------------------------*/
/*---------------------------------------------------------------------------------------------*/

// save new role with its extensions and permission
document.getElementById('save-add-role-btn').addEventListener('click', () => {
    const roleNameInputElt = document.getElementById('role_name_input');
    if (!roleNameInputElt.value) {
        window.toastr.warning('No role name given.', 'Role not saved!');
        input_form_group_validator('#role_name_input');
        return;
    }
    var _selectedExtensions = [];
    $('#addRoleRightExtensionsList > option').each(function () {
        _selectedExtensions.push(this.value);
    });
    const postData = {
        RoleName: roleNameInputElt.value,
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