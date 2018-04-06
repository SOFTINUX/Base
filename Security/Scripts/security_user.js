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
    $('input').on('ifChanged', function(event){
        permissionCheckboxChanged($(this));
    });
}

/* Param : checkbox element (jQuery selector) */
function permissionCheckboxChanged(jCheckBox) {
    // slave checkbox ?
    var slaveCheckBox = jCheckBox.attr("data-slave-cb");
    if(slaveCheckBox) {
        var currentCheckBoxChecked = jCheckBox.is(':checked');
        var slaveCheckBox = jCheckBox.closest("tr").children("td").find("input:checkbox[value='"+slaveCheckBox+"']")[0];
        if (currentCheckBoxChecked) {
            // current checkbox value is saved as new permission
            // FIXME fix code and uncomment function call
            //savePermission(jCheckBox.closest("tr").attr("data-entity-id"),
            //    jCheckBox.closest("tbody").attr("data-entity-id"),
            //    jCheckBox.value());
            // disable and uncheck slave cb
            $(slaveCheckBox).iCheck('check').iCheck('disable');
            // cascade event
            permissionCheckboxChanged($(slaveCheckBox));
        } else {
            // slave checkbox value is saved as new permission
            // FIXME fix code and uncomment function call
            //savePermission(jCheckBox.closest("tr").attr("data-entity-id"),
            //    jCheckBox.closest("tbody").attr("data-entity-id"),
            //    slaveCheckBox);
            // enable slave cb
            $(slaveCheckBox).iCheck('enable');
        }
    }
}

function savePermission(role, scope, permission) {
    console.log("Role: " + role + ", scope: " + scope + ", permission: " + permission);
    // TODO ajax call to administration/updaterolepermission
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