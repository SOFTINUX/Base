class CheckBox {
    private main: HTMLInputElement;

    constructor(element: HTMLInputElement) {
        if (element.type !== "checkbox") {
            console.error("Html input element is not checkbox");
            return;
        }
        this.main = element;
    }

    ChangeCheckboxState(): void {
        // if make check action
        if (this.main.checked) {
            let parent: HTMLElement;
            do {
                parent=this.main.parentElement;
            }
            while(parent.tagName !== "TR" && !parent.getAttribute("data-role-id"));

            /* WIP : récupérer les checkbox à traiter.
               A faire :
                1. remonter de this.main au "td" parent (réutiliser la boucle ci-dessus en affectant tdParent.
                   Renommer parent en trParent)
                2. Passer au td suivant (élément suivant)
                3. récupérer tous les input checkbox non cochés. Et les cocher
            */
            let matchedChildren:NodeList = parent.querySelectorAll("input[type=checkbox][checked]");
        }
        return;
    }

}