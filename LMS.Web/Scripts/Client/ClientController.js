var ClientModule;
(function (ClientModule) {
    'use strict';

    var ClientModuleController = (function () {
        function ClientModuleController() {
        }
        return ClientModuleController;
    })();
    ClientModule.ClientModuleController = ClientModuleController;
})(ClientModule || (ClientModule = {}));

// Attach the controller to the app
app.controller("ClientModuleController", ClientModule.ClientModuleController);
//# sourceMappingURL=ClientController.js.map
