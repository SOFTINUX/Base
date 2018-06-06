// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

$(function() {
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
    $.ajax("/administration/updaterolepermission", { data: params }, );
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