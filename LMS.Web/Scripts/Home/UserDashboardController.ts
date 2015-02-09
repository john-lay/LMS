/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/kendo/kendo.simplified.d.ts" />
/// <reference path="iuserdashboardscope.ts" />

module UserDashboardModule {
    'use strict';

    /*** GLOBAL VARIABLES ***/
    declare var API_URL: string;
    declare var TOKEN: string;
    declare var USER_ID: string;

    // Expose outside of the controller
    export var userDashboardScope: IUserDashboardScope;

    /*** ANGULAR CONTROLLER ***/
    export class UserDashboardController {

        constructor($scope: UserDashboardModule.IUserDashboardScope, $http: ng.IHttpService) {
            userDashboardScope = $scope;
            userDashboardScope.httpService = $http;
            userDashboardScope.CoursesList = [];
            userDashboardScope.calendarDates = [];

            userDashboardScope.loadData = function () {
                userDashboardScope.httpService({
                    method: "GET",
                    url: API_URL + "/Courses/GetCoursesByUser/" + USER_ID,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    userDashboardScope.CoursesList.length = 0;
                    userDashboardScope.calendarDates.length = 0;
                    userDashboardScope.CoursesList = JSON.parse(data);

                    // populate CourseSessionStartDateString and CourseSessionEndDateString
                    $.each(userDashboardScope.CoursesList, function (i, value) {
                        userDashboardScope.CoursesList[i].CourseSessionStartDateString = userDashboardScope.getDateStringFromJsonString(value.CourseSessionStartDate);
                        userDashboardScope.CoursesList[i].CourseSessionEndDateString = userDashboardScope.getDateStringFromJsonString(value.CourseSessionEndDate);
                        userDashboardScope.CoursesList[i].SubmittedWithoutCheck = false;
                        if (!value.LearningComplete) {
                            userDashboardScope.calculateTimeRemaining(userDashboardScope.CoursesList[i]);
                        }
                    });
                })
                .error(function (data: IErrorData, status: number) {
                    userDashboardScope.showUserFailure(data);
                });
            };

            userDashboardScope.calendarConfig = {
                value: new Date(), // today
                dates: userDashboardScope.calendarDates,
                month: {
                    // template for dates in month view
                    // NOTE: content needs the full object path to the calendarDates
                    content: '# if ($.inArray(data.date.getTime(), UserDashboardModule.userDashboardScope.calendarDates) != -1) { #' +
                    '<div class="highlight-day">#= data.value #</div>' +
                    '# } else { #' +
                    '#= data.value #' +
                    '# } #'
                },
                footer: '<div class="highlight-day-cell highlight-day-legend">&nbsp;</div> Completed by dates'//false
            }

            userDashboardScope.calculateTimeRemaining = function (course) {
                var startDate: any = userDashboardScope.getDateObjectFromJsonString(course.CourseSessionStartDate);
                var endDate: any = userDashboardScope.getDateObjectFromJsonString(course.CourseSessionEndDate);
                //endDate.setDate(endDate.getDate() + 2);
                var today: any = new Date();

                if (today >= endDate) {
                    course.PercentToFill = 100;
                    course.DaysRemaining = 0;
                    course.DaysRemainingString = "0 days remaining";
                } else {
                    var totalMilliseconds: number = Math.ceil(endDate - startDate);
                    var totalDays: number = totalMilliseconds / (1000 * 60 * 60 * 24);
                    var timeRemainingMilliseconds: number = Math.ceil(endDate - today);
                    var timeRemainingDays: number = Math.ceil(timeRemainingMilliseconds / (1000 * 60 * 60 * 24));
                    var timeRemainingPercent: number = Math.ceil(timeRemainingDays / totalDays * 100);

                    course.PercentToFill = 99 - timeRemainingPercent; // we really want 100 - x. But 1% progress looks better on the UI
                    course.DaysRemaining = timeRemainingDays;
                    course.DaysRemainingString = timeRemainingDays === 1 ? "1 day remaining" : timeRemainingDays + " days remaining";
                    //console.log("start date = " + startDate);
                    //console.log("end date = " + endDate);
                    //console.log("total days = " + totalDays);
                    //console.log("remaining = " + timeRemainingDays);
                    //console.log("perc = " + timeRemainingPercent);
                }

                // push the end date to the calendar (push as getTime integer for $.inArray comparison)
                userDashboardScope.calendarDates.push(endDate.getTime());

                // reinitialise calendar and highlight events
                userDashboardScope.updateCalendar();
            }

            userDashboardScope.checkUserInput = function (course) {
                course.SubmittedWithoutCheck = !course.LearningComplete;
            }

            userDashboardScope.updateCalendar = function () {
                $("#calendar").data('kendoCalendar').navigateToPast();
                $("#calendar").data('kendoCalendar').navigateToFuture();
                $(".highlight-day").parent().parent().addClass("highlight-day-cell");
            }

            userDashboardScope.submit = function (course) {
                if (!course.LearningComplete) {
                    course.SubmittedWithoutCheck = true;
                } else {
                    course.SubmittedWithoutCheck = false;
                    userDashboardScope.httpService({
                        method: "PATCH",
                        url: API_URL + "/UsersInCourseSessions/UpdateUserResultInCourseSession/" + course.CourseSessionId,
                        data: {
                            CourseSessionId: course.CourseSessionId,
                            UserId: USER_ID,
                            LearningComplete: course.LearningComplete
                        },
                        headers: {
                            "Authorization": "Bearer " + TOKEN
                        }
                    })
                    .success(function (data) {
                        userDashboardScope.showCourseUpdate(course);
                        userDashboardScope.loadData();
                        // reinitialise calendar and highlight events
                        userDashboardScope.updateCalendar();
                    })
                    .error(function (data: IErrorData, status: number) {
                        userDashboardScope.showUserFailure(data);
                    });
                }
            }

            userDashboardScope.getDateStringFromJsonString = function (jsonString) {
                var dateString = jsonString.match(/[0-9]+/g)[0];
                var formattedDate = new Date(parseInt(dateString, 10));
                var d = formattedDate.getDate().toString();
                var m = formattedDate.getMonth().toString();
                m += 1; // JavaScript months are 0-11
                var y = formattedDate.getFullYear();

                // prepend single day/month with zero
                d = ('0' + d).slice(-2);
                m = ('0' + m).slice(-2);

                return d + "/" + m + "/" + y;
            }

            userDashboardScope.getDateObjectFromJsonString = function (jsonString) {
                var dateString = jsonString.match(/[0-9]+/g)[0];
                return new Date(parseInt(dateString, 10));
            }

            userDashboardScope.showCourseUpdate = function (course) {
                var msg = "Congratulations, course: <b>" + course.CourseName + "</b> successfully completed!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            userDashboardScope.showUserFailure = function (data) {
                $(".alert-danger .msg")
                    .html(data.ExceptionMessage)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            // initialize data
            userDashboardScope.loadData();
        }
    }
}

// Attach the controller to the app
var app = angular.module("LMS", ["kendo.directives"]);
app.controller("UserDashboardController", ["$scope", "$http", UserDashboardModule.UserDashboardController]); 