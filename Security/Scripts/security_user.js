// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

$(function () {
    window.toastr.options.positionClass = 'toast-top-right';

    browseForAvatar();
    //freezePermissionCheckBox();

    $("#save_profile_btn").click(function (event) {
        edit_state("profile_form_fieldset", "save_profile_btn", event);
    });

    $("#cancel_save_profile_btn").click(function (event) {
        cancel_edit_state("profile_form", "profile_form_fieldset", "save_profile_btn", "Edit", event);
    });

    $("#profile_form :input").bind("keyup change paste", function () {
        input_changed("save_profile_btn");
    });

    $("#change_pwd-btn").click(function (event) {
        edit_state("pwd_form_fliedset", "change_pwd-btn", event);
    });

    $("#cancel_change_pwd-btn").click(function (event) {
        cancel_edit_state("pwd_form", "pwd_form_fliedset", "change_pwd-btn", "Change", event);
    });

    $("#pwd_form :input").bind("keyup change paste", function () {
        input_changed("change_pwd-btn");
    });

    $('#inputAvatar').change(function () {
        $('#file_path').val($(this).val());
    });

    $('#add-role').click(function () {
        $("#edit-role-area").addClass("hidden");

        if ($("#add-role-area").hasClass("hidden"))
            $("#add-role-area").removeClass("hidden");
        else
            $("#add-role-area").addClass("hidden");
    });

    $('#edit-role').click(function () {
        $("#add-role-area").addClass("hidden");

        if ($("#edit-role-area").hasClass("hidden"))
            $("#edit-role-area").removeClass("hidden");
        else
            $("#edit-role-area").addClass("hidden");
    });

    $('#save-add-role-btn').click(function () {
        var roleNameInputElt = $("#role_name_input");
         if (!roleNameInputElt.val()) {
            window.toastr.warning('No role given.', 'Role not saved!');
            input_form_group_validator("#role_name_input");
            return;
        }
        var selectedExtensions = [];
        $("#selectedExtensions>option").each(function() {
            selectedExtensions.push(this.value);
        });
        var postData = {
            RoleName: roleNameInputElt.val(),
            Extensions: selectedExtensions,
            Permission: $("#newRolePermission").val()
        };

        console.log(postData);

        $.ajax("/administration/savenewrole", {method: 'POST', data: postData})
        .done(function(data){
            window.toastr.success(data, 'New role created)');
            console.log(data);
        })
        .fail(function(jqXHR, testStatus){
            window.toastr.error(testStatus, 'ERROR');
            console.log(jqXHR, testStatus);
            console.log(jqXHR.responseText);
        });

        // normally it's possible to make role with no associated module
        //if ($('#selectedExtensions option').length == 0) {
        //    toastr.warning('You must select at least one extension from the list of available extensions.',
        //        'No extension selected');
        //    return;
        //}
        window.toastr.success('role saved.');
    });

    $('#cancel-add-role-btn').click(function () {
        $("#edit-role-area").addClass("hidden");
        $("#add-role-area").addClass("hidden");
    });

    $('#role_name_input').focusout(function () {
        input_form_group_validator("#role_name_input");
    });

    $('#role_name_input').change(function () {
        input_form_group_validator("#role_name_input");
    });

    $('#cancel-edit-role-btn').click(function () {
        $("#edit-role-area").addClass("hidden");
        $("#add-role-area").addClass("hidden");
    });

    $('#save-edit-role-btn').click(function () {
        if (!$("#name").val()) {
            window.toastr.warning('No new role name given.', 'Role not updated!');
            input_form_group_validator("#name");
            return;
        }

        saveEditRole("edit-role-form");
        //$("#edit-role-form").submit();
    });

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

    $('#btnRight').click(function (e) {
        var selectedOpts = $('#availableExtensions option:selected');
        if (selectedOpts.length === 0) {
            window.toastr.warning('You must select at least one extension from the list of available extensions.', 'No extension selected');
            e.preventDefault();
        }

        $('#selectedExtensions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });

    $('#btnAllRight').click(function (e) {
        var selectedOpts = $('#availableExtensions option');
        if (selectedOpts.length === 0) {
            window.toastr.warning('No extensions are available.', 'No extensions');
            e.preventDefault();
        }

        $('#selectedExtensions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });

    $('#btnLeft').click(function (e) {
        var selectedOpts = $('#selectedExtensions option:selected');
        if (selectedOpts.length === 0) {
            window.toastr.warning('You must select at least one extension from the list of selected extensions.', 'No extension selected');
            e.preventDefault();
        }

        $('#availableExtensions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });

    $('#btnAllLeft').click(function (e) {
        var selectedOpts = $('#selectedExtensions option');
        if (selectedOpts.length === 0) {
            window.toastr.warning('Selected extensions list is empty.', 'No extensions');
            e.preventDefault();
        }

        $('#availableExtensions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });

    $('#acl-sel li').click(function (e) {
        var $target = $( e.currentTarget );
        $target.closest('.bs-dropdown-to-select-acl-group')
            .find('[data-bind="bs-drp-sel-acl-label"]').text( $(this).text() );
        $("input[name*='acl-selected_value']" ).val( $(this).attr('data-value') );
    });
});

function browseForAvatar() {
    $('#file_browser').click(function (event) {
        event.preventDefault();
        $('#inputAvatar').click();
    });
}

/* events handler */
function listenToPermissionsCheckboxEvents() {
    console.log("attaching event handling");
    $('input').on('change', function (event) {
        console.log("input was changed!");
        event.preventDefault();
    });
}

function savePermission(scope, role, permission) {
    // string roleName_, string permissionId_, string scope_
    var params =
    {
        "roleName_": role,
        "permissionId_": permission,
        "scope_": scope
    };
    $.ajax("/administration/updaterolepermission", { data: params });
}

function edit_state(fieldsetid, editbtnid, event) {
    event.preventDefault();
    $("#cancel_" + editbtnid).removeClass("hidden");
    $("button#" + editbtnid).addClass("hidden");
    if (editbtnid === "save_profile_btn") {
        $("#file_browser").addClass("btn-primary");
        $("#file_browser").removeClass("btn-default");
    }
    if ($("#" + fieldsetid).prop("disabled")) {
        $("#" + fieldsetid).removeAttr('disabled');
        $("#" + editbtnid).prop("disabled", true);
    }
}

function cancel_edit_state(formid, fieldsetid, editbtnid, editbtntxt, event) {
    event.preventDefault();
    $("#" + fieldsetid).prop('disabled', true);
    $("#" + editbtnid).prop("disabled", false);
    $("button#" + editbtnid).text(editbtntxt);
    $("#cancel_" + editbtnid).addClass("hidden");
    $("button#" + editbtnid).removeClass("hidden");
    $("#file_browser").removeClass("btn-primary");
    $("#" + editbtnid).removeClass("btn-success").addClass("btn-primary");
    $("#" + formid)[0].reset();
}

function input_changed(editbtnid) {
    $("#" + editbtnid).prop("disabled", false);
    $("#" + editbtnid).removeClass("btn-primary").addClass("btn-success");
    $("button#" + editbtnid).text("Save");
    $("#cancel_" + editbtnid).removeClass("hidden");
    $("button#" + editbtnid).removeClass("hidden");
}

function checkClick() {
    var splitted = $(event.target)[0].id.split('_');
    var base = splitted[0] + "_" + splitted[1];
    var activeCheckedPermissions = "NEVER";

    if (event.target.checked) {
        // when checking, impacted checkboxes become checked and disabled
        switch (splitted[2]) {
            case "ADMIN":
                $("#" + base + "_WRITE, #" + base + "_READ").prop("checked", true).prop("disabled", true);
                break;
            case "WRITE":
                $("#" + base + "_READ").prop("checked", true).prop("disabled", true);
                break;
        }
        savePermission(splitted[0], splitted[1], splitted[2]);
    }
    else {
        // when unchecking, first next checkbox becomes enabled
        switch (splitted[2]) {
            case "ADMIN":
                $("#" + base + "_WRITE").prop("disabled", false);
                activeCheckedPermissions = "WRITE";
                break;
            case "WRITE":
                $("#" + base + "_READ").prop("disabled", false);
                activeCheckedPermissions = "READ";
                break;
        }
        savePermission(splitted[0], splitted[1], activeCheckedPermissions);
    }
}

function removeRoleLink() {
    console.log("Role name: ", $(event.target).parent().data("roleId"));
    var splitted = $(event.target).parent().data().roleId.split('_');
    console.log("Extension name: ", splitted[0], " Role name: ", splitted[1]);

    $("#moduleName").text(splitted[0]);
    $("#roleName").text(splitted[1]);

    $('#myModal').modal('show');
}

function input_form_group_validator(el) {
    console.log(el);
    if (!$(el).is('input')) {
        return;
    }

    if ($(el).val()) {
        $(el).closest('.form-group').removeClass('has-error').removeClass('has-feedback');
    } else {
        $(el).closest('.form-group').addClass('has-error').addClass('has-feedback');
    }
}

function passSelectedRoleOnEdition(roleId_){
    $("#edit-role-group").removeClass("has-error");
    $.ajax("/administration/findrole", {data: {"roleId_": roleId_}}).done(function(data){
        for (key in data.value)
        {
            $("#" + key).val(data.value[key]);
        }
    });
}

function saveEditRole(formId_){
    var $form =$("#"+formId_);
    var data = $form.serialize();
    $.ajax("/administration/updaterolename",
    {
        method: 'POST',
        data: data,
        headers: {
            "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]', $form).val()
        }
    })
    .done(function(data){
        window.toastr.success(data, "Saved");
        console.log(data);
    })
    .fail(function(jqXHR, testStatus){
        window.toastr.error(testStatus, 'ERROR)');
        console.log(jqXHR, testStatus);
    });
}