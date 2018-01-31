// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

$(function() {
    $("#save_profile_btn").click(function(event) {
        edit_state("profile_form_fieldset", "save_profile_btn");
    });

    $("#cancel_save_profile_btn").click(function(event) {
        cancel_edit_state("profile_form", "profile_form_fieldset", "save_profile_btn", "Edit");
    });

    $("#profile_form :input").bind("keyup change paste", function() {
        input_changed("save_profile_btn");
    });

    $("#change_pwd-btn").click(function(event) {
        edit_state("pwd_form_fliedset", "change_pwd-btn");
    });

    $("#cancel_change_pwd-btn").click(function(event) {
        cancel_edit_state("pwd_form", "pwd_form_fliedset", "change_pwd-btn", "Change");
    });

    $("#pwd_form :input").bind("keyup change paste", function() {
        input_changed("change_pwd-btn");
    });
});

function edit_state(fieldsetid, editbtnid) {
    event.preventDefault();
    if ($("#" + fieldsetid).prop("disabled")) {
        $("#" + fieldsetid).removeAttr('disabled');
        $("#" + editbtnid).prop("disabled", true);
        $("#cancel_" + editbtnid).prop("visible", true);
    }
}

function cancel_edit_state(formid, fieldsetid, editbtnid, editbtntxt) {
    event.preventDefault();
    $("#" + fieldsetid).prop('disabled', true);
    $("#" + editbtnid).prop("disabled", false);
    $("button#" + editbtnid).text(editbtntxt);
    $("#cancel_" + editbtnid).addClass("hidden");
    $("#" + editbtnid).removeClass("btn-success").addClass("btn-primary");
    $("#" + formid)[0].reset();
}

function input_changed(editbtnid) {
    $("#" + editbtnid).prop("disabled", false);
    $("#" + editbtnid).removeClass("btn-primary").addClass("btn-success");
    $("button#" + editbtnid).text("Save");
    $("#cancel_" + editbtnid).removeClass("hidden");
}