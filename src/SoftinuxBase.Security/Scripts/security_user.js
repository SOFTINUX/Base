// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

///<reference path = '../../SoftinuxBase.Barebone/node_modules/jquery/dist/jquery.js' />

/* Anonymous function to avoid polluting global namespace */
$(function () {
    window.toastr.options.positionClass = 'toast-top-right';

    browseForAvatar();

    /* User interactions that trigger UI changes but no ajax call */

    // Click
    $('#save_profile_btn').click(function (event_) {
        edit_state('profile_form_fieldset', 'save_profile_btn', event_);
    });

    $('#cancel_save_profile_btn').click(function (event_) {
        cancel_edit_state('profile_form', 'profile_form_fieldset', 'save_profile_btn', 'Edit', event_);
    });

    $('#change_pwd-btn').click(function (event_) {
        edit_state('pwd_form_fliedset', 'change_pwd-btn', event_);
    });

    $('#cancel_change_pwd-btn').click(function (event_) {
        cancel_edit_state('pwd_form', 'pwd_form_fliedset', 'change_pwd-btn', 'Change', event_);
    });

    $('#add-role').click(function () {
        $('#edit-role-area').addClass('hidden');

        if ($('#add-role-area').hasClass('hidden'))
            $('#add-role-area').removeClass('hidden');
        else
            $('#add-role-area').addClass('hidden');
    });

    $('#edit-role').click(function () {
        $('#add-role-area').addClass('hidden');

        if ($('#edit-role-area').hasClass('hidden'))
            $('#edit-role-area').removeClass('hidden');
        else
            $('#edit-role-area').addClass('hidden');
    });

    $('#cancel-add-role-btn').click(function () {
        $('#edit-role-area').addClass('hidden');
        $('#add-role-area').addClass('hidden');
    });

    $('#cancel-edit-role-btn').click(function () {
        $('#edit-role-area').addClass('hidden');
        $('#add-role-area').addClass('hidden');
    });

    // permissions administration: collapsing
    $('#collapse').on('click', function () {
        var state = $(this).data('state');
        if (state === 'closed') {
            $(this).data('state', 'open');
            // TODO change icon to open double chevron

            // open all the collapsed children
            $('.extension-row.collapsed').each(function () {
                $(this).removeClass('collapsed');
                $(this).data('aria-expanded', 'true');
                $('.row.collapse').each(function () {
                    $(this).addClass('in');
                });
            });

        } else {
            $(this).data('state', 'closed');
            // TODO change icon to closed double chevron

            // collapse all the children
            $('.extension-row').each(function () {
                if (!$(this).hasClass('collapsed')) {
                    $(this).addClass('collapsed');
                    $(this).data('aria-expanded', 'false');
                    $('.row.collapse').each(function () {
                        $(this).removeClass('in');
                    });
                }
            });
        }
    });

    // selected/unselected extensions management
    $('#addRoleBtnRight').click(function (e_) {
        addBtnRight(e_, 'availableExtensions', 'selectedExtensions');
    });
    $('#editRoleBtnRight').click(function (e_) {
        addBtnRight(e_, 'edit_role_availableExtensions', 'edit_role_selectedExtensions');
    });

    // selected/unselected extensions management
    $('#addRoleBtnAllRight').click(function (e_) {
        addBtnRight(e_, 'availableExtensions', 'selectedExtensions', true);
    });
    $('#editRoleBtnAllRight').click(function (e_) {
        addBtnRight(e_, 'edit_role_availableExtensions', 'edit_role_selectedExtensions', true);
    });

    // selected/unselected extensions management
    $('#addRoleBtnLeft').click(function (e_) {
        addBtnRight(e_, 'selectedExtensions', 'availableExtensions');
    });
    $('#editRoleBtnLeft').click(function (e_) {
        addBtnRight(e_, 'edit_role_selectedExtensions', 'edit_role_availableExtensions');
    });

    // selected/unselected extensions management
    $('#addRoleBtnAllLeft').click(function (e_) {
        addBtnRight(e_, 'selectedExtensions', 'availableExtensions', true);
    });

    $('#editRoleBtnAllLeft').click(function (e_) {
        addBtnRight(e_, 'edit_role_selectedExtensions', 'edit_role_availableExtensions', true);
    });

    // permission dropdown
    $('#acl-sel li').click(function (e_) {
        var $target = $(e_.currentTarget);
        $target.closest('.bs-dropdown-to-select-acl-group')
            .find('[data-bind="bs-drp-sel-acl-label"]').text($(this).text());
        $('input[name="acl-selected_value"]').val($(this).attr('data-value'));
    });

    // Keyup, change, paste
    $('#profile_form :input').bind('keyup change paste', function () {
        input_changed('save_profile_btn');
    });

    $('#pwd_form :input').bind('keyup change paste', function () {
        input_changed('change_pwd-btn');
    });

    // Change
    $('#inputAvatar').change(function () {
        $('#file_path').val($(this).val());
    });

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
        var roleNameInputElt = $('#role_name_input');
        if (!roleNameInputElt.val()) {
            window.toastr.warning('No role name given.', 'Role not saved!');
            input_form_group_validator('#role_name_input');
            return;
        }
        var selectedExtensions = [];
        $('#selectedExtensions>option').each(function () {
            selectedExtensions.push(this.value);
        });
        var postData = {
            RoleName: roleNameInputElt.val(),
            Extensions: selectedExtensions,
            Permission: $('#newRolePermission').val()
        };

        // console.log(postData);

        $.ajax('/administration/savenewrole', { method: 'POST', data: postData })
            .done(function (data_) {
                window.toastr.success(data_, 'New role created');
                input_form_group_set_error('#role_name_input', null);
                location.reload();
            })
            .fail(function (jqXhr_, testStatus_) {
                var errMsg = jqXhr_.responseText ? jqXhr_.responseText : testStatus_;
                input_form_group_set_error('#role_name_input', errMsg);
            });
    });

    $('#save-edit-role-btn').click(function () {
        if (!$('#name').val()) {
            window.toastr.warning('No new role name given.', 'Role not updated!');
            input_form_group_validator('#name');
            return;
        }

        saveEditRole('edit-role-form');
    });

});

/* Function declared in global namespace
so that they can be called by inline event handlers (onclick='...')
or by functions declared in anonymous function above */

/**
 *
 */
function browseForAvatar() {
    $('#file_browser').click(function (event_) {
        event_.preventDefault();
        $('#inputAvatar').click();
    });
}

/* events handler */

/**
 * Copy selected item(s) from left listbox to right listbox
 *
 * @param {object} event_ - dom event
 * @param {string} rolesListBoxLeft_ - left html listbox (select tag)
 * @param {string}rolesListBoxRight_ - right html listbox (select tag)
 * @param {boolean} bulk_ - bulk move
 */
function addBtnRight(event_, rolesListBoxLeft_, rolesListBoxRight_, bulk_) {

    bulk_ = !!bulk_;

    var extensionsAvailable = 'You must select at least one extension from the list of available extensions.';
    var extensionsNotAvailableTitle = 'No extensions are available.';
    var noExtensionsTitle = 'No extensions';
    var emptyExtensionList = 'Selected extensions list is empty.';
    var selectedOpts = $('#' + rolesListBoxLeft_ + (bulk_ ? ' option' : ' option:selected'));

    if (selectedOpts.length === 0) {
        window.toastr.warning(bulk_ ? extensionsAvailable : emptyExtensionList, bulk_ ? extensionsNotAvailableTitle : noExtensionsTitle);
        event_.preventDefault();
    }

    $('#' + rolesListBoxRight_).append($(selectedOpts).clone());
    $(selectedOpts).remove();
    event_.preventDefault();
}

/**
 *
 * @param {any} fieldsetid_
 * @param {any} editbtnid_
 * @param {any} event_
 */
function edit_state(fieldsetid_, editbtnid_, event_) {
    event_.preventDefault();
    $('#cancel_' + editbtnid_).removeClass('hidden');
    $('button#' + editbtnid_).addClass('hidden');
    if (editbtnid_ === 'save_profile_btn') {
        $('#file_browser').addClass('btn-primary');
        $('#file_browser').removeClass('btn-default');
    }
    if ($('#' + fieldsetid_).prop('disabled')) {
        $('#' + fieldsetid_).removeAttr('disabled');
        $('#' + editbtnid_).prop('disabled', true);
    }
}

/**
 *
 * @param {any} formid_
 * @param {any} fieldsetid_
 * @param {any} editbtnid_
 * @param {any} editbtntxt_
 * @param {any} event_
 */
function cancel_edit_state(formid_, fieldsetid_, editbtnid_, editbtntxt_, event_) {
    event_.preventDefault();
    $('#' + fieldsetid_).prop('disabled', true);
    $('#' + editbtnid_).prop('disabled', false);
    $('button#' + editbtnid_).text(editbtntxt_);
    $('#cancel_' + editbtnid_).addClass('hidden');
    $('button#' + editbtnid_).removeClass('hidden');
    $('#file_browser').removeClass('btn-primary');
    $('#' + editbtnid_).removeClass('btn-success').addClass('btn-primary');
    $('#' + formid_)[0].reset();
}

/**
 *
 * @param {any} editbtnid_
 */
function input_changed(editbtnid_) {
    $('#' + editbtnid_).prop('disabled', false);
    $('#' + editbtnid_).removeClass('btn-primary').addClass('btn-success');
    $('button#' + editbtnid_).text('Save');
    $('#cancel_' + editbtnid_).removeClass('hidden');
    $('button#' + editbtnid_).removeClass('hidden');
}

/**
 * display modal to choose what to remove about role
 */
function removeRoleLink() {
    console.log('Role name: ', $(event.target).parent().data('roleId'));
    var splitted = $(event.target).parent().data().roleId.split('_');
    console.log('Extension name: ', splitted[0], ' Role name: ', splitted[1]);

    $('#moduleName').text(splitted[0]);
    $('#roleName').text(splitted[1]);

    $('#myModal').modal('show');
}

/**
 * Set error style to input when its value is empty.
 * @param {string} el_ - jQuery selector string.
 */
function input_form_group_validator(el_) {
    // console.log(el);
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
 * @param {string} el_ - jQuery selector string.
 * @param {string} errMsg_ - error message if any error, else null to remove error style and message.
 */
function input_form_group_set_error(el_, errMsg_) {
    if (!$(el_).is('input')) {
        return;
    }
    var formGroupEl = $(el_).closest('.form-group');
    if (!errMsg_) {
        formGroupEl.removeClass('has-error').removeClass('has-feedback');
        formGroupEl.find('span.help-block').html('');
    } else {
        formGroupEl.addClass('has-error').addClass('has-feedback');
        formGroupEl.find('span.help-block').html(errMsg_);
    }
}

/* Functions that trigger an ajax call */

/**
 * Update the UI with selected role information. Ajax GET.
 * @param {any} roleId_ - roleId
 */
function passSelectedRoleOnEdition(roleId_) {
    $('#edit-role-group').removeClass('has-error');
    $.ajax('/administration/read-role', { data: { 'roleId_': roleId_ } }).done(function (data_) {
        var role = data_.value.role;
        for (var key in role) {
            $('#edit_role_' + key).val(role[key]);
        }

        // Role name
        $('#edit_role_name_input').val(role.name);

        // Role ID
        $('#editRoleId').val(roleId_);

        // Available extensions
        // Clear
        $('#edit_role_availableExtensions option').remove();
        var options = $('#edit_role_availableExtensions').prop('options');
        // Fill
        data_.value.availableExtensions.forEach(function (extension_) {
            options[options.length] = new Option(extension_, extension_);
        });

        // Selected extensions
        // Clear
        $('#edit_role_selectedExtensions option').remove();
        options = $('#edit_role_selectedExtensions').prop('options');
        // Fill
        console.log(data_.value);
        data_.value.selectedExtensions.forEach(function (extension_) {
            options[options.length] = new Option(extension_.name + ' (' + extension_.permission + ')', extension_);
        });
    });
}

/**
 * Click in permission checkbox. Calls savePermission().
 */
function checkClick() {
    const splitted = $(event.target)[0].id.split('_');
    const base = splitted[0] + '_' + splitted[1];
    var activeCheckedPermissions = 'NEVER';

    if (event.target.checked) {
        // when checking, impacted checkboxes become checked and disabled
        switch (splitted[2]) {
            case 'ADMIN':
                $('#' + base + '_WRITE, #' + base + '_READ').prop('checked', true).prop('disabled', true);
                break;
            case 'WRITE':
                $('#' + base + '_READ').prop('checked', true).prop('disabled', true);
                break;
        }
        savePermission(splitted[0], splitted[1], splitted[2]);
    }
    else {
        // when unchecking, first next checkbox becomes enabled
        switch (splitted[2]) {
            case 'ADMIN':
                $('#' + base + '_WRITE').prop('disabled', false);
                activeCheckedPermissions = 'WRITE';
                break;
            case 'WRITE':
                $('#' + base + '_READ').prop('disabled', false);
                activeCheckedPermissions = 'READ';
                break;
        }
        savePermission(splitted[0], splitted[1], activeCheckedPermissions);
    }
}

/**
 * Ajax call to update data: role-permission link update. Ajax GET (TODO change to POST).
 * @param {any} scope_ - scope
 * @param {any} role_ - role
 * @param {any} permission_- permission
 */
function savePermission(scope_, role_, permission_) {
    // string roleName_, string permissionId_, string scope_
    var params =
    {
        'roleName_': role_,
        'permissionId_': permission_,
        'scope_': scope_
    };
    $.ajax('/administration/updaterolepermission', { data: params });
}

/**
 * Ajax call to update data: role with its related data update. Ajax POST.
 */
function saveEditRole() {
    var selectedExtensions = [];
    $('#selectedExtensions>option').each(function () {
        selectedExtensions.push(this.value);
    });
    var postData = {
        RoleId: $('#editRoleId').val(),
        RoleName: $('#role_name_input').val(),
        Extensions: selectedExtensions,
        Permission: $('#newRolePermission').val()
    };

    console.log(postData);

    $.ajax('/administration/update-role',
        {
            method: 'POST',
            data: postData
            // headers: {
            //     'RequestVerificationToken': $('input:hidden[name='__RequestVerificationToken']', $form).val()
            // }
        })
        .done(function (data_) {
            window.toastr.success(data_, 'Saved');
            console.log(data_);
        })
        .fail(function (jqXhr_, testStatus_) {
            window.toastr.error(testStatus_, 'ERROR)');
            console.log(jqXhr_, testStatus_);
        });
}



