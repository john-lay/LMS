/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/kendo/kendo.simplified.d.ts" />
/// <reference path="iadminscope.ts" />

module ManageAdminModule {
    'use strict';

    /*** GLOBAL VARIABLES ***/
    declare var API_URL: string;
    declare var TOKEN: string;

    // Expose outside of the controller
    export var manageAdminScope: IManageAdminScope;

    /*** ANGULAR CONTROLLER ***/
    export class UserManageAdminController {

        constructor($scope: ManageAdminModule.IManageAdminScope, $http: ng.IHttpService) {
            manageAdminScope = $scope;
            manageAdminScope.httpService = $http;
            manageAdminScope.ClientsList = [{ ClientId: -1, Name: "Please select a client..." }];
            manageAdminScope.CurrentClient = manageAdminScope.ClientsList[0];
            manageAdminScope.PasswordsMatch = true;

            manageAdminScope.itemTemplate = "<span ng-click='edit(dataItem)'>{{dataItem.text}}</span>";
            manageAdminScope.treeData = [{ id: -1, text: "" }];
            
            manageAdminScope.init = function () {
                manageAdminScope.loadClients(API_URL + "/clients/GetClients/");
                manageAdminScope.loadAdminUsers();
            }

            manageAdminScope.getDefaultUser = function () {

                var defaultUser: IUser = {
                    id: -1,
                    ClientId: -1,
                    FirstName: "",
                    LastName: "",
                    EmailAddress: "",
                    Password: "",
                    ConfirmPassword: ""
                }

                return JSON.parse(JSON.stringify(defaultUser));
            }

            manageAdminScope.loadClients = function (file) {
                $http({
                    method: "GET",
                    url: file,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: IClient) {
                    manageAdminScope.ClientsList = manageAdminScope.ClientsList.concat(data);
                })
                .error(function (data: IErrorData, status: number) {
                    manageAdminScope.showUserFailure(data);
                });
            };

            manageAdminScope.loadAdminUsers = function () {
                $http({
                    method: "GET",
                    url: API_URL + "/Users/GetAdmins/",
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    $(".tree-view").hide();
                    manageAdminScope.treeData = JSON.parse(data);
                    $(".k-loading").parent().slideUp(function () {
                        $(".tree-view").slideDown(function () {
                            // style the group elements - this needs to be done as late as possible so the DOM items exist.
                            $(".k-sprite.group").parent().css({ "background-color": "#e7e7e7", "padding-right": "15px" });
                        });
                    });
                })
                .error(function (data: IErrorData, status: number) {
                    manageAdminScope.showUserFailure(data);
                });
            };

            manageAdminScope.checkPasswordsMatch = function () {
                manageAdminScope.PasswordsMatch = manageAdminScope.NewUser.Password === manageAdminScope.NewUser.ConfirmPassword;
            }

            manageAdminScope.create = function () {
                var newUser = manageAdminScope.NewUser;
                newUser.ClientId = manageAdminScope.CurrentClient.ClientId;

                if (manageAdminScope.PasswordsMatch) {
                    $http({
                        method: "POST",
                        url: API_URL + "/Account/RegisterAdmin/",
                        data: newUser,
                        headers: {
                            "Authorization": "Bearer " + TOKEN
                        }
                    })
                    .success(function (data: string) {
                        $("#CreateUserModal").modal('hide');
                        manageAdminScope.showUserSuccess();
                    })
                    .error(function (data: IErrorData, status: number) {
                        $("#CreateUserModal").modal('hide');
                        manageAdminScope.showUserFailure(data);
                    });
                }
            }

            // show the editor dialog and form
            manageAdminScope.edit = function (dataItem) {
                if (!dataItem.hasOwnProperty('spriteCssClass')) { // && !dataItem.spriteCssClass === "group") {
                    $http({
                        method: "GET",
                        url: API_URL + "/Users/GetUser/" + dataItem.id,
                        headers: {
                            "Authorization": "Bearer " + TOKEN
                        }
                    })
                    .success(function (data: IUser) {
                        manageAdminScope.CurrentUser = manageAdminScope.getDefaultUser();
                        manageAdminScope.CurrentUser.FirstName = data.FirstName;
                        manageAdminScope.CurrentUser.LastName = data.LastName;
                        manageAdminScope.CurrentUser.id = dataItem.id;
                        $("#UpdateUserModal").modal("show");
                    })
                    .error(function (data: IErrorData, status: number) {
                        manageAdminScope.showUserFailure(data);
                    });
                }
            }

            manageAdminScope.update = function () {
                $http({
                    method: "PATCH",
                    url: API_URL + "/Users/UpdateUser/" + manageAdminScope.CurrentUser.id,
                    data: { FirstName: manageAdminScope.CurrentUser.FirstName, LastName: manageAdminScope.CurrentUser.LastName, UserId: manageAdminScope.CurrentUser.id },
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    $("#UpdateUserModal").modal("hide");
                    manageAdminScope.showUserUpdate();
                })
                .error(function (data: IErrorData, status: number) {
                    $("#UpdateUserModal").modal("hide");
                    manageAdminScope.showUserFailure(data);
                });
            }

            manageAdminScope.delete = function () {
                $http({
                    method: "DELETE",
                    url: API_URL + "/Users/DeleteUser/" + manageAdminScope.CurrentUser.id,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    $("#UpdateUserModal").modal("hide");
                    manageAdminScope.showUserDelete();
                })
                .error(function (data: IErrorData, status: number) {
                    $("#UpdateUserModal").modal("hide");
                    manageAdminScope.showUserFailure(data);
                });
            }

            manageAdminScope.showUserSuccess = function () {
                var msg = "Admin user: <b>" + manageAdminScope.NewUser.FirstName + " " + manageAdminScope.NewUser.LastName + "</b> successfully created!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // reset field
                manageAdminScope.NewUser = manageAdminScope.getDefaultUser();

                // update tree
                manageAdminScope.loadAdminUsers();
            }

            manageAdminScope.showUserUpdate = function () {
                var msg = "Admin user: <b>" + manageAdminScope.CurrentUser.FirstName + " " + manageAdminScope.CurrentUser.LastName + "</b> successfully updated!";
                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // reset field
                manageAdminScope.NewUser = manageAdminScope.getDefaultUser();

                // update tree
                manageAdminScope.loadAdminUsers();
            }

            manageAdminScope.showUserDelete = function () {
                var msg = "Admin user: <b>" + manageAdminScope.CurrentUser.FirstName + " " + manageAdminScope.CurrentUser.LastName + "</b> successfully deleted!";
                $(".alert-danger .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // reset field
                manageAdminScope.NewUser = manageAdminScope.getDefaultUser();

                // update tree
                manageAdminScope.loadAdminUsers();
            }

            manageAdminScope.showUserFailure = function (data) {

                $(".alert-danger .msg")
                    .html(data.ExceptionMessage)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            // initialize data
            manageAdminScope.init();
            manageAdminScope.NewUser = manageAdminScope.getDefaultUser();
        }
    }
}

// Attach the controller to the app
var app = angular.module("LMS", ["kendo.directives"]);
app.controller("UserManageAdminController", ["$scope", "$http", ManageAdminModule.UserManageAdminController]); 