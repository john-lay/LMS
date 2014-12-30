/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/kendo/kendo.all.d.ts" />

module EmailModule {
    'use strict';

    /*** GLOBAL VARIABLES ***/
    declare var API_URL: string;
    declare var TOKEN: string;

    /*** ANGULAR SCOPE ***/
    export interface IEmailScope extends ng.IScope {
        
        // PROPERTIES
        httpService: ng.IHttpService;
        RecipientsList: string[];
        Subject: string;
        Body: string;
        itemTemplate: string;
        treeData: kendo.data.DataSource<kendo.data.Model>;
        

        // PUBLIC METHODS
        loadData(file: string): void;
        send(): void;
        addRecipient(dataItem: kendo.data.DataSource<kendo.data.Model>): void;
        removeRecipient(index: number): void;
        alreadyAdded(email: string): boolean;
        isValidEmail(email: string): boolean;
        showEmailSuccess(): void;
        showEmailFailure(error: any): void;
    }

    // Expose outide of the controller
    export var emailScope: IEmailScope;

    /*** ANGULAR CONTROLLER ***/
    export class EmailController {

        constructor($scope: EmailModule.IEmailScope, $http: ng.IHttpService) {
            emailScope = $scope;
            emailScope.httpService = $http;
            emailScope.RecipientsList = [];
            emailScope.Subject = "";
            emailScope.Body = "";
            emailScope.treeData = [{ text: "" }];
            emailScope.itemTemplate = "<span ng-click='addRecipient(dataItem)'>{{dataItem.text}}</span>";

            // initialize data
            emailScope.loadData(API_URL + "/UsersInUserGroups/GetUserGroupsAndUserEmails/");

            emailScope.loadData = (file) => {
                emailScope.httpService({
                    method: "GET",
                    url: file,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success((data: string) => {
                    $(".tree-view").hide();
                    emailScope.treeData = JSON.parse(data);
                    $(".k-loading").parent().slideUp(function() {
                        $(".tree-view").slideDown();
                    });
                })
                .error((data, status) => {
                    //emailScope.errorStatus = status;
                    //emailScope.errorData = data;
                    //emailScope.errorURL = file;
                    //setTimeout(function () {
                    //    emailScope.errorWindow.center().open();
                    //});
                });
            };

            emailScope.send = () => {
                if (emailScope.RecipientsList.length > 0) {
                    $http({
                            method: "POST",
                            url: API_URL + "/Contact/SendEmail/",
                            data: {
                                "recipients": emailScope.RecipientsList,
                                "subject": emailScope.Subject,
                                "body": emailScope.Body
                            },
                            headers: {
                                "Authorization": "Bearer " + TOKEN
                            }
                        })
                        .success(data => {
                            emailScope.showEmailSuccess();
                        })
                        .error((data: any, status) => {
                        //emailScope.errorStatus = status;
                        //emailScope.errorData = data;

                        emailScope.showEmailFailure(data);
                    });
                }
            }

            emailScope.addRecipient = (dataItem) => {
                // check the recipient hasn't already been added to the recipient list and validate the email address to prevent email groups being added to the list
                if (!emailScope.alreadyAdded(dataItem.text) && emailScope.isValidEmail(dataItem.text)) {
                    emailScope.RecipientsList.push(dataItem.text);
                }
            };

            emailScope.removeRecipient = (index) => {
                emailScope.RecipientsList.splice(index, 1);
            }

            emailScope.alreadyAdded = (email) => {
                return emailScope.RecipientsList.indexOf(email) !== -1;
            }

            emailScope.isValidEmail = (email) => {
                // regex taken from angular library. EMAIL_REGEXP
                return /^[a-z0-9!#$%&'*+\/=?^_`{|}~.-]+@@[a-z0-9]([a-z0-9-]*[a-z0-9])?(\.[a-z0-9]([a-z0-9-]*[a-z0-9])?)*$/i.test(email);
            }

            

            //emailScope.loadData = function (file) {
            //    $http({
            //        method: "GET",
            //        url: file,
            //        headers: {
            //            "Authorization": "Bearer " + TOKEN
            //        }
            //    })
            //    .success(function (data) {
            //        $(".tree-view").hide();
            //        $scope.treeData = JSON.parse(data);
            //        $(".k-loading").parent().slideUp(function () {
            //            $(".tree-view").slideDown();
            //        });
            //    })
            //    .error(function (data, status) {
            //        $scope.errorStatus = status;
            //        $scope.errorData = data;
            //        $scope.errorURL = file;
            //        setTimeout(function () {
            //            $scope.errorWindow.center().open();
            //        });
            //    });
            //};

            emailScope.showEmailSuccess = () => {
                var msg = "Email to: <b>" + emailScope.RecipientsList.join(", ") + "</b> successfully send!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                // reset form
                emailScope.RecipientsList = [];
                emailScope.Subject = "";
                emailScope.Body = "";
            }

            emailScope.showEmailFailure = (msg) => {

                $(".alert-danger .msg")
                    .html(msg.ExceptionMessage)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }
        }
    }
}

// Attach the controller to the app
app.controller("EmailController", ["$scope", "$http", EmailModule.EmailController]);