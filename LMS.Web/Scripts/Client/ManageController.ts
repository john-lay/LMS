/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="imanagescope.ts" />
/// <reference path="../typings/custom.d.ts" />

module ClientModule {
    'use strict';

    /*** GLOBAL VARIABLES ***/
    declare var API_URL: string;
    declare var TOKEN: string;

    // Expose outside of the controller
    export var clientManageScope: IManageScope;

    /*** ANGULAR CONTROLLER ***/
    export class ClientManageController {

        constructor($scope: ClientModule.IManageScope, $http: ng.IHttpService) {
            clientManageScope = $scope;
            clientManageScope.httpService = $http;
            clientManageScope.ClientsList = [];
            clientManageScope.NewClientName = "";
            clientManageScope.DefaultClient = {ClientId: -1, Name: ""};
            clientManageScope.CurrentClient = clientManageScope.DefaultClient;

            clientManageScope.init = () => {
                clientManageScope.loadData(API_URL + "/clients/GetClients/");
            }

            clientManageScope.loadData = file => {
                $http({
                    method: "GET",
                    url: file,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success((data: IClient[]) => {
                    $("#clients").hide();
                    clientManageScope.ClientsList = data;
                    $(".k-loading").parent().slideUp(() => {
                        $("#clients").slideDown();
                    });
                })
                .error((data: IErrorData, status: number) => {
                    clientManageScope.showClientFailure(data);
                });
            };

            // create new client
            clientManageScope.create = () => {
                $http({
                    method: "POST",
                    url: API_URL + "/clients/CreateClient/",
                    data: {
                        "Name": clientManageScope.NewClientName
                    },
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(data => {
                    $("#CreateModal").modal("hide");
                    clientManageScope.showClientSuccess();
                })
                .error((data: IErrorData, status: number) => {
                    $("#CreateModal").modal("hide");
                    clientManageScope.showClientFailure(data);
                });
            };

            clientManageScope.setCurrentClient = client => {
                clientManageScope.CurrentClient = client;
            }

            // update/edit client
            clientManageScope.update = () => {
                $http({
                    method: "PUT",
                    url: API_URL + "/clients/UpdateClient/" + clientManageScope.CurrentClient.ClientId,
                    data: {
                        "ClientId": clientManageScope.CurrentClient.ClientId,
                        "Name": clientManageScope.CurrentClient.Name
                    },
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(data => {
                    $("#EditModal").modal("hide");
                    clientManageScope.showClientUpdate();
                })
                .error((data: IErrorData, status: number) => {
                    $("#EditModal").modal("hide");
                    clientManageScope.showClientFailure(data);
                });
            }

            // delete client
            clientManageScope.delete = () => {
                $http({
                    method: "DELETE",
                    url: API_URL + "/clients/DeleteClient/" + clientManageScope.CurrentClient.ClientId,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(data => {
                        $("#DeleteModal").modal("hide");
                        clientManageScope.showClientDelete();
                    })
                .error((data: IErrorData, status: number) => {
                    $("#DeleteModal").modal("hide");
                    clientManageScope.showClientFailure(data);
                });
            }

            clientManageScope.showClientSuccess = () => {
                var msg: string = "Client: <b>" + clientManageScope.NewClientName + "</b> successfully created!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // reset field
                clientManageScope.NewClientName = "";

                $('#clients').slideUp();
                clientManageScope.init();
                $('#clients').slideDown();
            }

            clientManageScope.showClientUpdate = () => {
                var msg: string = "Client: <b>" + clientManageScope.CurrentClient.Name + "</b> successfully updated!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // reset field
                clientManageScope.CurrentClient = clientManageScope.DefaultClient;

                $('#clients').slideUp();
                clientManageScope.init();
                $('#clients').slideDown();
            }

            clientManageScope.showClientDelete = () => {
                var msg: string = "Client: <b>" + clientManageScope.CurrentClient.Name + "</b> successfully deleted!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // reset field
                clientManageScope.CurrentClient = clientManageScope.DefaultClient;

                $('#clients').slideUp();
                clientManageScope.init();
                $('#clients').slideDown();
            }

            clientManageScope.showClientFailure = data => {

                $(".alert-danger .msg")
                    .html(data.ExceptionMessage)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            // initialize data
            clientManageScope.init();
        }
    }
}

// Attach the controller to the app
var app = angular.module("LMS", []);
app.controller("ClientManageController", ["$scope", "$http", ClientModule.ClientManageController]);