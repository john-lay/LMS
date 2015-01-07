/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/kendo/kendo.simplified.d.ts" />
/// <reference path="iemailscope.ts" />
var EmailModule;
(function (EmailModule) {
    'use strict';

    

    // Expose outside of the controller
    EmailModule.emailScope;

    /*** ANGULAR CONTROLLER ***/
    var EmailController = (function () {
        function EmailController($scope, $http) {
            EmailModule.emailScope = $scope;
            EmailModule.emailScope.httpService = $http;
            EmailModule.emailScope.RecipientsList = [];
            EmailModule.emailScope.Subject = "";
            EmailModule.emailScope.Body = "";
            EmailModule.emailScope.treeData = [{ id: -1, text: "" }];
            EmailModule.emailScope.itemTemplate = "<span ng-click='addRecipient(dataItem)'>{{dataItem.text}}</span>";

            EmailModule.emailScope.send = function () {
                if (EmailModule.emailScope.RecipientsList.length > 0) {
                    $http({
                        method: "POST",
                        url: API_URL + "/Contact/SendEmail/",
                        data: {
                            "recipients": EmailModule.emailScope.RecipientsList,
                            "subject": EmailModule.emailScope.Subject,
                            "body": EmailModule.emailScope.Body
                        },
                        headers: {
                            "Authorization": "Bearer " + TOKEN
                        }
                    }).success(function (data) {
                        EmailModule.emailScope.showEmailSuccess();
                    }).error(function (data, status) {
                        EmailModule.emailScope.showEmailFailure(data);
                    });
                }
            };

            EmailModule.emailScope.addRecipient = function (dataItem) {
                // check the recipient hasn't already been added to the recipient list and validate the email address to prevent email groups being added to the list
                if (!EmailModule.emailScope.alreadyAdded(dataItem.text) && EmailModule.emailScope.isValidEmail(dataItem.text)) {
                    EmailModule.emailScope.RecipientsList.push(dataItem.text);
                }
            };

            EmailModule.emailScope.removeRecipient = function (index) {
                EmailModule.emailScope.RecipientsList.splice(index, 1);
            };

            EmailModule.emailScope.alreadyAdded = function (email) {
                return EmailModule.emailScope.RecipientsList.indexOf(email) !== -1;
            };

            EmailModule.emailScope.isValidEmail = function (email) {
                // regex taken from angular library. EMAIL_REGEXP
                return /^[a-z0-9!#$%&'*+\/=?^_`{|}~.-]+@[a-z0-9]([a-z0-9-]*[a-z0-9])?(\.[a-z0-9]([a-z0-9-]*[a-z0-9])?)*$/i.test(email);
            };

            EmailModule.emailScope.loadData = function (file) {
                EmailModule.emailScope.httpService({
                    method: "GET",
                    url: file,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                }).success(function (data) {
                    $(".tree-view").hide();
                    EmailModule.emailScope.treeData = JSON.parse(data);
                    $(".k-loading").parent().slideUp(function () {
                        $(".tree-view").slideDown();
                    });
                }).error(function (data, status) {
                    EmailModule.emailScope.showEmailFailure(data);
                });
            };

            EmailModule.emailScope.showEmailSuccess = function () {
                var msg = "Email to: <b>" + EmailModule.emailScope.RecipientsList.join(", ") + "</b> successfully send!";

                $(".alert-success .msg").html(msg).parent().removeClass("hidden").slideDown();

                // reset form
                EmailModule.emailScope.RecipientsList = [];
                EmailModule.emailScope.Subject = "";
                EmailModule.emailScope.Body = "";
            };

            EmailModule.emailScope.showEmailFailure = function (msg) {
                $(".alert-danger .msg").html(msg.ExceptionMessage).parent().removeClass("hidden").slideDown();
            };

            // initialize data
            EmailModule.emailScope.loadData(API_URL + "/UsersInUserGroups/GetUserGroupsAndUserEmails/");
        }
        return EmailController;
    })();
    EmailModule.EmailController = EmailController;
})(EmailModule || (EmailModule = {}));

// Attach the controller to the app
var app = angular.module("LMS", ["kendo.directives"]);
app.controller("EmailController", ["$scope", "$http", EmailModule.EmailController]);
//# sourceMappingURL=EmailController.js.map
