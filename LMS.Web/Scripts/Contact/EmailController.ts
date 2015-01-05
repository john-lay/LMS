/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/kendo/kendo.simplified.d.ts" />
/// <reference path="iemailscope.ts" />

module EmailModule {
    'use strict';

    /*** GLOBAL VARIABLES ***/
    declare var API_URL: string;
    declare var TOKEN: string;

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
            emailScope.treeData = [{ id: -1, text: "" }];
            emailScope.itemTemplate = "<span ng-click='addRecipient(dataItem)'>{{dataItem.text}}</span>";

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
                    .error((data: IErrorData, status: number) => {
                        emailScope.errorStatus = status;
                        emailScope.errorData = data;

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
                return /^[a-z0-9!#$%&'*+\/=?^_`{|}~.-]+@[a-z0-9]([a-z0-9-]*[a-z0-9])?(\.[a-z0-9]([a-z0-9-]*[a-z0-9])?)*$/i.test(email);
            }

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
                        $(".k-loading").parent().slideUp(() => {
                            $(".tree-view").slideDown();
                        });
                    })
                    .error((data: IErrorData, status: number) => {
                        emailScope.errorStatus = status;
                        emailScope.errorData = data;
                        emailScope.errorURL = file;
                        //setTimeout(function () {
                        //    emailScope.errorWindow.center().open();
                        //});
                    });
            };

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

            emailScope.showEmailFailure = (msg: IErrorData) => {

                $(".alert-danger .msg")
                    .html(msg.ExceptionMessage)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            // initialize data
            emailScope.loadData(API_URL + "/UsersInUserGroups/GetUserGroupsAndUserEmails/");
        }
    }
}

// Attach the controller to the app
var app = angular.module("LMS", ["kendo.directives"]);
app.controller("EmailController", ["$scope", "$http", EmailModule.EmailController]);