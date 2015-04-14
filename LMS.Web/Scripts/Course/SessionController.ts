/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/kendo/kendo.simplified.d.ts" />
/// <reference path="isessionscope.ts" />

module CourseSessionModule {
    'use strict';

    /*** GLOBAL VARIABLES ***/
    declare var API_URL: string;
    declare var TOKEN: string;
    declare var COURSE_ID: number;

    // Expose outside of the controller
    export var courseSessionScope: ICourseSessionScope;

    /*** ANGULAR CONTROLLER ***/
    export class CourseSessionController {

        constructor($scope: CourseSessionModule.ICourseSessionScope, $http: ng.IHttpService) {
            courseSessionScope = $scope;
            courseSessionScope.httpService = $http;
            courseSessionScope.treeData = [{ id: -1, text: "" }];
            courseSessionScope.selectedUsers = []; //[{ id: -1, text: "" }];
            courseSessionScope.courseSessionsList = []; //[{CourseSessionId: -1, StartDate: null, EndDate: null, IsRolling: false}];
            courseSessionScope.usersInSessionList = [];
            courseSessionScope.SelectAll = false;
            courseSessionScope.EditorTitle = "";

            courseSessionScope.loadData = function () {
                courseSessionScope.httpService({
                    method: "GET",
                    url: API_URL + "/UsersInUserGroups/GetUserGroupsAndUsers/",
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    $(".tree-view").hide();
                    courseSessionScope.treeData = JSON.parse(data);
                    $(".k-loading").parent().slideUp(function () {
                        $(".tree-view").slideDown(function () {
                            // style the group elements - this needs to be done as late as possible so the DOM items exist.
                            $(".k-sprite.group").parent().css({ "background-color": "#e7e7e7", "padding-right": "15px" });
                        });
                    });
                })
                .error(function (data: IErrorData, status: number) {
                    courseSessionScope.showSessionFailure(data);
                });
            };

            courseSessionScope.treeOptions = {
                checkboxes: {
                    checkChildren: true
                },
                dataBound: function (e) {
                    courseSessionScope.attachChangeEvent(e);
                }
            };

            courseSessionScope.attachChangeEvent = function (e) {
                var dataSource = e.sender.dataSource;
                dataSource.bind("change", function (e) {
                    var selectedNodes = 0;
                    var checkedNodes = [];
                    courseSessionScope.checkedNodeIds(dataSource.view(), checkedNodes);
                    // clear the contents of the selected user array before repopulating
                    courseSessionScope.selectedUsers.length = 0;
                    for (var i = 0; i < checkedNodes.length; i++) {
                        var nd = checkedNodes[i];
                        if (nd.checked) {
                            // only add users to the array (ignore user groups)
                            //console.log("item " + nd.id + ": " + nd.text + " checked.");
                            if (!nd.hasOwnProperty("spriteCssClass")) {
                                courseSessionScope.selectedUsers.push({ id: nd.id, text: nd.text, IsEnrolled: false });
                            }
                            selectedNodes++;
                        }
                    }
                    //console.log(courseSessionScope.selectedUsers);
                });
            };

            courseSessionScope.checkedNodeIds = function (nodes, checkedNodes) {
                for (var i = 0; i < nodes.length; i++) {
                    var ndo = nodes[i];
                    checkedNodes.push(ndo);
                    if (ndo.hasChildren) {
                        courseSessionScope.checkedNodeIds(ndo.children.view(), checkedNodes);
                    }
                }
            };

            courseSessionScope.populateSessionTable = function () {
                courseSessionScope.httpService({
                    method: "GET",
                    url: API_URL + "/CourseSessions/GetCourseSessions/" + COURSE_ID,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    courseSessionScope.courseSessionsList = JSON.parse(data);
                })
                .error(function (data: IErrorData, status: number) {
                    courseSessionScope.showSessionFailure(data);
                });
            }

            courseSessionScope.setCourseSession = function (dataItem) {
                courseSessionScope.currentCourseSession = dataItem;
                // try and convert the Microsoft ajax date string. i.e. "/Date(1421712000000)"
                if (courseSessionScope.currentCourseSession.StartDate.indexOf("/Date") !== -1) {
                    courseSessionScope.currentCourseSession.StartDate = courseSessionScope.getDateStringFromJSONString(dataItem.StartDate);
                    courseSessionScope.currentCourseSession.EndDate = courseSessionScope.getDateStringFromJSONString(dataItem.EndDate);
                }
            }

            courseSessionScope.getDefaultCourseSession = function () {
                var defaultCourseSession = {
                    CourseId: -1,
                    CourseSessionId: -1,
                    EndDate: null,
                    IsRolling: false,
                    StartDate: null
                };

                return JSON.parse(JSON.stringify(defaultCourseSession));
            }

            courseSessionScope.showCourseSessionCreator = function () {
                courseSessionScope.currentCourseSession = courseSessionScope.getDefaultCourseSession();
                courseSessionScope.EditorTitle = "Add Course Session";
                $("#AddCourseSession").removeClass("hidden");
                $("#EditCourseSession").addClass("hidden");
                $("#DeleteCourseSession").addClass("hidden");
            }

            courseSessionScope.showCourseSessionEditor = function (dataItem) {
                courseSessionScope.setCourseSession(dataItem);
                courseSessionScope.EditorTitle = "Edit Course Session";
                $("#AddCourseSession").addClass("hidden");
                $("#EditCourseSession").removeClass("hidden");
                $("#DeleteCourseSession").addClass("hidden");
            }

            courseSessionScope.showCourseSessionRemover = function (dataItem) {
                courseSessionScope.setCourseSession(dataItem);
                courseSessionScope.EditorTitle = "Are you sure you want to delete this session?";
                $("#AddCourseSession").addClass("hidden");
                $("#EditCourseSession").addClass("hidden");
                $("#DeleteCourseSession").removeClass("hidden");
            }

            courseSessionScope.getDateStringFromJSONString = function (JSONString) {
                var dateString = JSONString.match(/[0-9]+/g)[0];
                var formattedDate = new Date(parseInt(dateString, 10));
                var d = formattedDate.getDate().toString();
                var month = formattedDate.getMonth();
                month += 1; // JavaScript months are 0-11
                var m = month.toString();
                var y = formattedDate.getFullYear();

                // prepend single day/month with zero
                d = ('0' + d).slice(-2);
                m = ('0' + m).slice(-2);

                return d + "/" + m + "/" + y;
            }

            courseSessionScope.covertToISODateString = function (dateString) {
                var ISODate = courseSessionScope.parseDate(dateString);
                var d = ISODate.getDate();
                var m = ISODate.getMonth();
                m += 1; // JavaScript months are 0-11
                var y = ISODate.getFullYear();

                // The .NET framework expecs yyyy-MM-dd
                return y + "-" + m + "-" + d;
            }

            // parse a date from dd/mm/yyyy format
            courseSessionScope.parseDate = function (input) {
                var parts = input.split('/');
                // new Date(year, month [, day [, hours[, minutes[, seconds[, ms]]]]])
                return new Date(parseInt(parts[2], 10),
                                parseInt(parts[1], 10) - 1,
                                parseInt(parts[0], 10)); // Note: months are 0-based
            }

            courseSessionScope.enrolUsers = function () {

                var userData = [];

                $.each(courseSessionScope.selectedUsers, function (i, user) {
                    userData.push({ UserId: user.id });
                });

                courseSessionScope.httpService({
                    method: "POST",
                    url: API_URL + "/UsersInCourseSessions/AddUsersToCourseSession/" + courseSessionScope.currentCourseSession.CourseSessionId,
                    data: userData,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    $("#EnrolUsersModal").modal("hide");
                    courseSessionScope.showEnrolUsersSuccess();
                })
                .error(function (data: IErrorData, status: number) {
                    $("#EnrolUsersModal").modal("hide");
                    courseSessionScope.showSessionFailure(data);
                });
            }

            courseSessionScope.getUsersInSession = function (dataItem) {

                courseSessionScope.setCourseSession(dataItem);
                courseSessionScope.httpService({
                    method: "GET",
                    url: API_URL + "/UsersInCourseSessions/GetUsersInCourseSession/" + dataItem.CourseSessionId,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    var tempUsers = JSON.parse(data);
                    // add an isEnrolled property to the user object
                    $.each(tempUsers, function (i, user) {
                        tempUsers[i].IsEnrolled = false;
                    });

                    courseSessionScope.usersInSessionList = tempUsers;
                })
                .error(function (data: IErrorData, status: number) {
                    courseSessionScope.showSessionFailure(data);
                });
            }

            courseSessionScope.SelectAllUsers = function () {
                if (courseSessionScope.SelectAll === true) {
                    $.each(courseSessionScope.usersInSessionList, function (i, user) {
                        courseSessionScope.usersInSessionList[i].IsEnrolled = true;
                    });
                } else {
                    $.each(courseSessionScope.usersInSessionList, function (i, user) {
                        courseSessionScope.usersInSessionList[i].IsEnrolled = false;
                    });
                }
            }

            courseSessionScope.removeSelectedUsers = function () {

                var userData = [];

                $.each(courseSessionScope.usersInSessionList, function (i, user) {
                    if (user.IsEnrolled) {
                        userData.push({ UserId: courseSessionScope.usersInSessionList[i].id });
                    }
                });

                courseSessionScope.httpService({
                    method: "POST",
                    url: API_URL + "/UsersInCourseSessions/RemoveUsersFromCourseSession/" + courseSessionScope.currentCourseSession.CourseSessionId,
                    data: userData,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    $("#EditUsersInSessionModal").modal("hide");
                    courseSessionScope.showEnrolUsersUpdate();
                })
                .error(function (data: IErrorData, status: number) {
                    $("#EditUsersInSessionModal").modal("hide");
                    courseSessionScope.showSessionFailure(data);
                });
            }

            courseSessionScope.createCourseSession = function () {

                courseSessionScope.httpService({
                    method: "POST",
                    url: API_URL + "/CourseSessions/CreateCourseSession/",
                    data: {
                        CourseId: COURSE_ID,
                        StartDate: courseSessionScope.covertToISODateString(courseSessionScope.currentCourseSession.StartDate),
                        EndDate: courseSessionScope.covertToISODateString(courseSessionScope.currentCourseSession.EndDate),
                        IsRolling: courseSessionScope.currentCourseSession.IsRolling,
                    },
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    $("#EditCourseSessionModal").modal("hide");
                    courseSessionScope.showCourseCategorySuccess();
                    courseSessionScope.populateSessionTable();
                })
                .error(function (data: IErrorData, status: number) {
                    $("#EditCourseSessionModal").modal("hide");
                    courseSessionScope.showSessionFailure(data);
                });
            }

            courseSessionScope.editCourseSession = function () {

                courseSessionScope.httpService({
                    method: "PUT",
                    url: API_URL + "/CourseSessions/UpdateCourseSession/" + courseSessionScope.currentCourseSession.CourseSessionId,
                    data: {
                        CourseSessionId: courseSessionScope.currentCourseSession.CourseSessionId,
                        CourseId: COURSE_ID,
                        StartDate: courseSessionScope.covertToISODateString(courseSessionScope.currentCourseSession.StartDate),
                        EndDate: courseSessionScope.covertToISODateString(courseSessionScope.currentCourseSession.EndDate),
                        IsRolling: courseSessionScope.currentCourseSession.IsRolling,
                    },
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    $("#EditCourseSessionModal").modal("hide");
                    courseSessionScope.showCourseCategoryUpdate();
                    courseSessionScope.populateSessionTable();
                })
                .error(function (data: IErrorData, status: number) {
                    $("#EditCourseSessionModal").modal("hide");
                    courseSessionScope.showSessionFailure(data);
                });
            }

            courseSessionScope.deleteCourseSession = function () {

                courseSessionScope.httpService({
                    method: "DELETE",
                    url: API_URL + "/CourseSessions/DeleteCourseSession/" + courseSessionScope.currentCourseSession.CourseSessionId,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    $("#EditCourseSessionModal").modal("hide");
                    courseSessionScope.showCourseCategoryDelete();
                    courseSessionScope.populateSessionTable();
                })
                .error(function (data: IErrorData, status: number) {
                    $("#EditCourseSessionModal").modal("hide");
                    courseSessionScope.showSessionFailure(data);
                });
            }

            courseSessionScope.showEnrolUsersSuccess = function () {
                var usersAdded = [];

                $.each(courseSessionScope.selectedUsers, function (i, user) {
                    usersAdded.push(user.text);
                });

                var msg = "Users: <b>" + usersAdded.join(", ") + "</b> successfully added to session!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            courseSessionScope.showEnrolUsersUpdate = function () {
                var usersRemoved = [];

                $.each(courseSessionScope.usersInSessionList, function (i, user) {
                    if (user.IsEnrolled) {
                        usersRemoved.push(user.text);
                    }
                });

                var msg = "Users: <b>" + usersRemoved.join(", ") + "</b> successfully removed to session!";

                $(".alert-warning .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            courseSessionScope.showCourseCategorySuccess = function () {
                var msg = "Course session successfully created!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            courseSessionScope.showCourseCategoryUpdate = function () {
                var msg = "Course session successfully updated!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            courseSessionScope.showCourseCategoryDelete = function () {
                var msg = "Course session successfully deleted!";

                $(".alert-danger .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            courseSessionScope.showSessionFailure = function (data) {

                $(".alert-danger .msg")
                    .html(data.ExceptionMessage)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            // run the initialize method
            courseSessionScope.loadData();
            courseSessionScope.populateSessionTable();
            courseSessionScope.currentCourseSession = courseSessionScope.getDefaultCourseSession();
        }            
    }
}

// Attach the controller to the app
var app = angular.module("LMS", ["kendo.directives"]);
app.controller("CourseSessionController", ["$scope", "$http", CourseSessionModule.CourseSessionController]); 