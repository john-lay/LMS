/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/kendo/kendo.simplified.d.ts" />
/// <reference path="iselectscope.ts" />

module SelectCourseModule {
    'use strict';

    /*** GLOBAL VARIABLES ***/
    declare var API_URL: string;
    declare var TOKEN: string;
    declare var SESSION_URL: string;

    // Expose outside of the controller
    export var courseSelectScope: IManageCourseScope;

    /*** ANGULAR CONTROLLER ***/
    export class CourseSelectController {

        constructor($scope: SelectCourseModule.IManageCourseScope, $http: ng.IHttpService) {
            courseSelectScope = $scope;
            courseSelectScope.httpService = $http;
            courseSelectScope.treeData = [{ id: -1, text: "" }];
            courseSelectScope.itemTemplate = "<span ng-click='courseSelected(dataItem)'>{{dataItem.text}}</span>";

            courseSelectScope.loadData = function () {
                courseSelectScope.httpService({
                    method: "GET",
                    url: API_URL + "/CoursesInCourseCategories/GetCourseCategoriesAndCourses/",
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    $(".tree-view").hide();
                    courseSelectScope.treeData = JSON.parse(data);
                    $(".k-loading").parent().slideUp(function () {
                        $(".tree-view").slideDown();
                    });
                })
                .error(function (data: IErrorData, status: number) {
                    courseSelectScope.showCourseFailure(data);
                });
            };

            courseSelectScope.courseSelected = function (dataItem) {

                // disabled the manage course session button
                $("#ManageCourseSession")
                    .addClass("disabled")
                    .attr("href", "#");

                // only enable it for a valid course (not a course category)
                $.each(courseSelectScope.treeData, function (i, categoryNode) {
                    $.each(categoryNode.items, function (j, courseNode) {
                        if (courseNode.text == dataItem.text) {
                            $("#ManageCourseSession")
                                .removeClass("disabled")
                                .attr("href", SESSION_URL + dataItem.id);
                        }
                    });
                });
            }

            courseSelectScope.showCourseFailure = function (data) {

                $(".alert-danger .msg")
                    .html(data.ExceptionMessage)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            // run the initialize method
            courseSelectScope.loadData();
            $("#ManageCourseSession").addClass("disabled");
        }
    }
}

// Attach the controller to the app
var app = angular.module("LMS", ["kendo.directives"]);
app.controller("CourseSelectController", ["$scope", "$http", SelectCourseModule.CourseSelectController]); 