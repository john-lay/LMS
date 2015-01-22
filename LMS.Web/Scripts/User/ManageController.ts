/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/kendo/kendo.simplified.d.ts" />
/// <reference path="imanagescope.ts" />

module ManageUserModule {
    'use strict';

    /*** GLOBAL VARIABLES ***/
    declare var API_URL: string;
    declare var TOKEN: string;

    // Expose outside of the controller
    export var userManageScope: IManageUserScope;

    /*** ANGULAR CONTROLLER ***/
    export class UserManageController {

        constructor($scope: ManageUserModule.IManageUserScope, $http: ng.IHttpService) {
            userManageScope = $scope;
            userManageScope.httpService = $http;
            userManageScope.treeData = [{ id: -1, text: "" }];
            userManageScope.itemTemplate = "<span ng-click='openEditor(dataItem)'>{{dataItem.text}}</span>";
            userManageScope.PasswordsMatch = true;

            userManageScope.loadData = function () {
                userManageScope.httpService({
                    method: "GET",
                    url: API_URL + "/UsersInUserGroups/GetUserGroupsAndUsers/",
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    $(".tree-view").hide();
                    userManageScope.treeData = JSON.parse(data);
                    $(".k-loading").parent().slideUp(function () {
                        $(".tree-view").slideDown(function () {
                            // style the group elements - this needs to be done as late as possible so the DOM items exist.
                            $(".k-sprite.group").parent().css({ "background-color": "#e7e7e7", "padding-right": "15px" });
                        });
                    });
                })
                .error(function (data: IErrorData, status: number) {
                    userManageScope.showUserFailure(data);
                });
            };

            userManageScope.checkPasswordsMatch = function () {
                userManageScope.PasswordsMatch = userManageScope.CurrentUser.Password === userManageScope.CurrentUser.ConfirmPassword;
            }

            userManageScope.createNewUser = function () {
                if (userManageScope.PasswordsMatch) {
                    userManageScope.httpService({
                        method: "POST",
                        url: API_URL + "/Account/Register/",
                        data: {
                            FirstName: userManageScope.CurrentUser.FirstName,
                            LastName: userManageScope.CurrentUser.LastName,
                            EmailAddress: userManageScope.CurrentUser.EmailAddress,
                            Password: userManageScope.CurrentUser.Password
                        },
                        headers: {
                            "Authorization": "Bearer " + TOKEN
                        }
                    })
                    .success(function (data) {
                        userManageScope.showUserSuccess();
                    })
                    .error(function (data: IErrorData, status: number) {
                        userManageScope.showUserFailure(data);
                    });
                }
            };

            // the user clicked on the tree. Ascertain whether to open the user editor or ignore
            userManageScope.openEditor = function (dataItem) {
                $.each(userManageScope.treeData, function (i, groupNode) {
                    if (groupNode.text === dataItem.text) {
                        //console.log("user not in group: " + dataItem.text)
                        userManageScope.loadUserEditor(dataItem.id);
                    }
                    if (groupNode.hasOwnProperty("items")) {
                        $.each(groupNode.items, function (j, userNode) {
                            if (userNode.text === dataItem.text) {
                                //console.log("user in a group: " + dataItem.text)                               
                                userManageScope.loadUserEditor(dataItem.id);
                            }
                        });
                    }
                });
            }

            userManageScope.loadUserEditor = function (selectedUserId) {
                userManageScope.httpService({
                    method: "GET",
                    url: API_URL + "/Users/GetUser/" + selectedUserId,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: IUser) {
                    userManageScope.CurrentUser = userManageScope.getDefaultUser();
                    userManageScope.CurrentUser.FirstName = data.FirstName;
                    userManageScope.CurrentUser.LastName = data.LastName;
                    userManageScope.CurrentUser.UserId = selectedUserId;
                    $("#UpdateUserModal").modal("show");

                })
                .error(function (data: IErrorData, status: number) {
                    userManageScope.showUserFailure(data);
                });
            }

            userManageScope.editUser = function () {
                userManageScope.httpService({
                    method: "PATCH",
                    url: API_URL + "/Users/UpdateUser/" + userManageScope.CurrentUser.UserId,
                    data: {
                        FirstName: userManageScope.CurrentUser.FirstName,
                        LastName: userManageScope.CurrentUser.LastName,
                        UserId: userManageScope.CurrentUser.UserId
                    },
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    $("#UpdateUserModal").modal("hide");
                    userManageScope.showUserUpdate();

                })
                .error(function (data: IErrorData, status: number) {
                    userManageScope.showUserFailure(data);
                });
            }

            userManageScope.deleteUser = function () {
                userManageScope.httpService({
                    method: "DELETE",
                    url: API_URL + "/Users/DeleteUser/" + userManageScope.CurrentUser.UserId,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    $("#UpdateUserModal").modal("hide");
                    userManageScope.showUserDelete();

                })
                .error(function (data: IErrorData, status: number) {
                    userManageScope.showUserFailure(data);
                });
            }

            userManageScope.getDefaultUser = function () {

                var defaultUser: IUser = {
                    id: -1,
                    UserId: -1,
                    ClientId: -1,
                    FirstName: "",
                    LastName: "",
                    EmailAddress: "",
                    Password: "",
                    ConfirmPassword: ""
                }

                return JSON.parse(JSON.stringify(defaultUser));
            }

            userManageScope.showUserSuccess = function () {
                var msg = "User: <b>" + userManageScope.CurrentUser.FirstName + " " + userManageScope.CurrentUser.LastName + "</b> successfully created!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // reset form
                userManageScope.CurrentUser = userManageScope.getDefaultUser();

                // update tree
                userManageScope.loadData();
            }

            userManageScope.showUserUpdate = function () {
                var msg = "User: <b>" + userManageScope.CurrentUser.FirstName + " " + userManageScope.CurrentUser.LastName + "</b> successfully updated!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // update tree
                userManageScope.loadData();
            }

            userManageScope.showUserDelete = function () {
                var msg = "User: <b>" + userManageScope.CurrentUser.FirstName + " " + userManageScope.CurrentUser.LastName + "</b> successfully deleted!";

                $(".alert-danger .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // update tree
                userManageScope.loadData();
            }

            userManageScope.showUserFailure = function (data) {

                $(".alert-danger .msg")
                    .html(data.ExceptionMessage)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            // initialize data
            userManageScope.loadData();
            userManageScope.CurrentUser = userManageScope.getDefaultUser();
        }
    }
}

// Attach the controller to the app
var app = angular.module("LMS", ["kendo.directives"]);
app.controller("UserManageController", ["$scope", "$http", ManageUserModule.UserManageController]); 