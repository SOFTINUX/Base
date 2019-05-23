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
/*------------------------ events handler ------------------------*/
/*----------------------------------------------------------------*/

document.getElementById('editRoleRightExtensionsList').addEventListener('click', event_ => {
    row_clicked(event_.target.closest('div.row'));
}, false);

document.getElementById('editRoleLeftExtensionsList').addEventListener('click', event_ => {
    row_clicked(event_.target.closest('div.row'));
}, false);

// permissions administration: collapsing
document.getElementById('collapse').addEventListener('click', () => {
    const element = document.getElementById('collapse');
    const state = element.dataset.state;
    const _subEl = document.getElementsByClassName('row collapse');

    if (state === 'closed') {
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
    document.getElementById('newRolePermission').value = clickedLiElt.getAttribute('data-value');
}, false);


function removeRoleLink(element_) {
    if (!element_) {
        console.log('You must pass this as argument of removeRoleLink onlick.');
        return;
    }
    const splitted = element_.parentNode.dataset.roleId.split('_');

    document.getElementById('moduleName').innerText = splitted[0];
    document.getElementById('roleName').innerText = splitted[1];

    $('#myModal').modal('show');
}