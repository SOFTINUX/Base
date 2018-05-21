var CheckBox = (function () {
    function CheckBox(element) {
        if (element.type !== "checkbox") {
            console.error("Html input element is not checkbox");
            return;
        }
        this.main = element;
    }
    CheckBox.prototype.ChangeCheckboxState = function () {
        if (this.main.checked) {
            var parent_1;
            do {
                parent_1 = this.main.parentElement;
            } while (parent_1.tagName !== "TR" && !parent_1.getAttribute("data-role-id"));
            var matchedChildren = parent_1.querySelectorAll("input[type=checkbox][checked]");
        }
        return;
    };
    return CheckBox;
}());
//# sourceMappingURL=Security.js.map