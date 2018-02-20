import * as $ from "jquery";

class SecurityUserEditButton {

    EditModeState(fieldSetId:string, buttonId: string, addCss:string, removeCss: string) {
        event.preventDefault();
        $(buttonId).removeClass(removeCss).addClass(addCss);
        $("#cancel_" + buttonId).removeClass("hidden");
        $("button#" + buttonId).addClass("hidden");

        if (buttonId === "save_profile_btn") {
            $("#file_browser").addClass("btn-primary");
            $("#file_browser").removeClass("btn-default");
        }
        if ($("#" + fieldSetId).prop("disabled")) {
            $("#" + fieldSetId).removeAttr('disabled');
            $("#" + buttonId).prop("disabled", true);
        }
    }
}
