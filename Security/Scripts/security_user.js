// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

$(function() {
    toastr.options.positionClass = 'toast-top-right';

    browseForAvatar();
    //freezePermissionCheckBox();

    $("#save_profile_btn").click(function(event) {
        edit_state("profile_form_fieldset", "save_profile_btn", event);
    });

    $("#cancel_save_profile_btn").click(function(event) {
        cancel_edit_state("profile_form", "profile_form_fieldset", "save_profile_btn", "Edit", event);
    });

    $("#profile_form :input").bind("keyup change paste", function() {
        input_changed("save_profile_btn");
    });

    $("#change_pwd-btn").click(function(event) {
        edit_state("pwd_form_fliedset", "change_pwd-btn", event);
    });

    $("#cancel_change_pwd-btn").click(function(event) {
        cancel_edit_state("pwd_form", "pwd_form_fliedset", "change_pwd-btn", "Change", event);
    });

    $("#pwd_form :input").bind("keyup change paste", function() {
        input_changed("change_pwd-btn");
    });

    $('#inputAvatar').change(function(){
        $('#file_path').val($(this).val());
    });

    $('#add-role').click(function () {
        if ($("#add-role-field").hasClass("hidden"))
            $("#add-role-field").removeClass("hidden");
        else
            $("#add-role-field").addClass("hidden");
    });

    $('#save-role-btn').click(function () {
        console.log($("#role_name_input").val());
        if (!$("#role_name_input").val()) {
            toastr.warning('No role given.', 'Role not saved!');
            return;
        }

        if ($('#selectedExtensions option').length == 0) {
            toastr.warning('You must select at least one extension from the list of available extensions.', 'No extension selected');
            return;
        }
        toastr.success('role saved.');
    });

    $('#collapse').on('click', function () {
        $('.extension-row').each(function() {
            if ($(this).hasClass('collapsed'))
            {
                $(this).removeClass('collapsed');
                $(this).data('aria-expended', 'true');
                $('.row.collapse').each(function() {
                    $(this).toggleClass('in');
                });
            }
            else{
                $(this).toggleClass('collapsed');
                $(this).data('aria-expended', 'false');
                $('.row.collapse').each(function() {
                    $(this).removeClass('in');
                });
            }
        });
    });

    $('#btnRight').click(function(e) {
        var selectedOpts = $('#availableExtensions option:selected');
        if (selectedOpts.length == 0) {
            toastr.warning('You must select at least one extension from the list of available extensions.', 'No extension selected');
            e.preventDefault();
        }

        $('#selectedExtensions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });

    $('#btnAllRight').click(function (e) {
        var selectedOpts = $('#availableExtensions option');
        if (selectedOpts.length == 0) {
            toastr.warning('No extensions are available.', 'No extensions');
            e.preventDefault();
        }

        $('#selectedExtensions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });

    $('#btnLeft').click(function (e) {
        var selectedOpts = $('#selectedExtensions option:selected');
        if (selectedOpts.length == 0) {
            toastr.warning('You must select at least one extension from the list of selected extensions.', 'No extension selected');
            e.preventDefault();
        }

        $('#availableExtensions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });

    $('#btnAllLeft').click(function (e) {
        var selectedOpts = $('#selectedExtensions option');
        if (selectedOpts.length == 0) {
            toastr.warning('Selected extensions list is empty.', 'No extensions');
            e.preventDefault();
        }

        $('#availableExtensions').append($(selectedOpts).clone());
        $(selectedOpts).remove();
        e.preventDefault();
    });
});

function browseForAvatar() {
    $('#file_browser').click(function(event){
        event.preventDefault();
        $('#inputAvatar').click();
    });
}

/* events handler */
function listenToPermissionsCheckboxEvents() {
    console.log("attaching event handling");
    $('input').on('change', function(event){
        console.log("input was changed!");
        event.preventDefault();
    });
}

function savePermission(scope, role, permission) {
    // string roleId_, string permissionId_, string scope_
    let params =
    {
        "roleId_": role,
        "permissionId_": permission,
        "scope_": scope
    };
    $.ajax("/administration/updaterolepermission", { data: params } );
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

    if(event.target.checked) {
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

    $("#moduleName").text( splitted[0] );
    $("#roleName").text( splitted[1] );

    $('#myModal').modal('show');
}