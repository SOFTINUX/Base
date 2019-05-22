// Manage click on buttons
    [].forEach.call(document.querySelectorAll('button'),
        function (event_) {
            event_.addEventListener('click',
                function () {
                    const addRoleArea = document.querySelector('#add-role-area');
                    const editRoleArea = document.querySelector('#edit-role-area');

                    switch (event_.id) {
                        case 'add-role-btn':
                            editRoleArea.style.display = 'none';
                            addRoleArea.style.display = addRoleArea.style.display !== 'none' ? 'none' : 'block';
                            break;
                        case 'edit-role-btn':
                            addRoleArea.style.display = 'none';
                            editRoleArea.style.display = editRoleArea.style.display !== 'none' ? 'none' : 'block';
                            break;
                        case 'cancel-add-role-btn':
                            editRoleArea.style.display = 'none';
                            addRoleArea.style.display = 'none';
                            break;
                        case 'cancel-edit-role-btn':
                            editRoleArea.style.display = 'none';
                            addRoleArea.style.display = 'none';
                            break;
                        // Add selected/unselected extensions management
                        case 'addRoleBtnRight':
                            btnChevronMoveExtension(event_, '');
                            break;
                        case 'addRoleBtnAllRight':
                            btnChevronMoveExtension(event_, '');
                            break;
                        case 'addRoleBtnLeft':
                            btnChevronMoveExtension(event_, '');
                            break;
                        case 'addRoleBtnAllLeft':
                            btnChevronMoveExtension(event_, '');
                            break;
                        // Edit selected/unselected extensions management
                        case 'editRoleBtnRight':
                            btnChevronMoveExtension(event_, 'to-right');
                            break;
                        case 'editRoleBtnAllRight':
                            btnChevronMoveExtension(event_, 'to-right');
                            break;
                        case 'editRoleBtnLeft':
                            btnChevronMoveExtension(event_, 'to-left');
                            break;
                        case 'editRoleBtnAllLeft':
                            btnChevronMoveExtension(event_, 'to-left');
                            break;
                        default:
                            break;
                    }
                });
        });

    document.getElementById('editRoleRightExtensionsList').addEventListener('click', event_ => {
        row_clicked(event_.target.closest('div.row'));
    }, false);

    document.getElementById('editRoleLeftExtensionsList').addEventListener('click', event_ => {
        row_clicked(event_.target.closest('div.row'));
    }, false);

    // permissions administration: collapsing
    document.getElementById('collapse').addEventListener('click', () => {
        const element = document.getElementById('collapse');
        const state = document.getElementById('collapse').dataset.state;

        var _subEl;
        if (state === 'closed') {
            element.dataset.state = 'open';
            // TODO change icon to open double chevron

            // open all the collapsed children
            var _elements = Array.from(document.getElementsByClassName('extension-row collapsed'));
            for (let item of _elements) {
                item.classList.remove('collapsed');
                item.setAttribute('aria-expanded', 'true');
                _subEl = document.getElementsByClassName('row collapse');
                for (var _i = 0, _ilen = _subEl.length; _i < _ilen; _i++) {
                    _subEl[_i].classList.add('in');
                }
            }
        } else {
            element.dataset.state = 'closed';
            // TODO change icon to closed double chevron
            const elementRow = Array.from(document.getElementsByClassName('extension-row'));
            // collapse all the children
            for (let item of elementRow) {
                if (!item.classList.contains('collapsed')) {
                    item.classList.add('collapsed');
                    item.setAttribute('aria-expanded', 'false');
                    _subEl = document.getElementsByClassName('row collapse');
                    for (var _j = 0, _jlen = _subEl.length; _j < _jlen; _j++) {
                        _subEl[_j].classList.remove('in');
                    }
                }
            }
        }
    }, false);

    // permission dropdown
    document.getElementById('acl-sel').addEventListener('click', event_ => {
        let clickedLiElt = event_.target.closest('li');
        clickedLiElt.closest('.bs-dropdown-to-select-acl-group').querySelectorAll('[data-bind="bs-drp-sel-acl-label"]')[0].innerText = clickedLiElt.innerText;
        document.getElementById('newRolePermission').value = clickedLiElt.getAttribute('data-value');
    }, false);