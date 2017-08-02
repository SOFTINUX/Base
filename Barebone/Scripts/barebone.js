(function(application) {
    application.eventHandlers = application.eventHandlers || [];
    application.registerEventHandler = function(eventHandler) {
        application.eventHandlers.push(eventHandler);
    };

    application.initializeEventHandlers = function() {
        $("body").on("click", "button", function() {
            handleEvent("click", "button", this);
        });

        $("body").on("change", "input", function() {
            handleEvent("change", "input", this);
        });
    };

    function handleEvent(eventName, tagName, element) {
        for (var i = 0; i < application.eventHandlers.length; i++) {
            if (application.eventHandlers[i].eventName == eventName && application.eventHandlers[i].tagName == tagName) {
                application.eventHandlers[i].action(element);
            }
        }
    }
})(window.application = window.application || {});

$(document).ready(
    function() {
        application.initializeEventHandlers();
    }
);