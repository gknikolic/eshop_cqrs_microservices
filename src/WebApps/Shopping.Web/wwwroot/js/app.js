// app.js
var app = (function () {
    // Private method for handling BlockUI with a loader
    function blockUI(message) {
        $.blockUI({
            message: message || '<div class="spinner-border text-primary" role="status"><span class="sr-only">Loading...</span></div>',
            css: {
                border: 'none',
                backgroundColor: 'transparent'
            },
            overlayCSS: {
                backgroundColor: '#000',
                opacity: 0.6,
                cursor: 'wait'
            }
        });
    }

    // Private method for unblocking the UI
    function unblockUI() {
        $.unblockUI();
    }

    // Public method for displaying a confirmation dialog using Bootbox
    function confirmAction(message, callback) {
        bootbox.confirm({
            message: message || "Are you sure?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (typeof callback === 'function') {
                    callback(result);
                }
            }
        });
    }

    // Public method for displaying an alert using Bootbox
    function showAlert(message, callback) {
        bootbox.alert({
            message: message || "An alert message!",
            callback: function () {
                if (typeof callback === 'function') {
                    callback();
                }
            }
        });
    }

    // Exposing public methods
    return {
        confirmAction: confirmAction,
        showAlert: showAlert,
        blockUI: blockUI,
        unblockUI: unblockUI
    };
})();