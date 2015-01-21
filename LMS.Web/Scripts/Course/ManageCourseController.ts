/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/kendo/kendo.simplified.d.ts" />
/// <reference path="imanagecoursescope.ts" />

module ManageCourseModule {
    'use strict';

    /*** GLOBAL VARIABLES ***/
    declare var API_URL: string;
    declare var TOKEN: string;
    declare var EDIT_URL: string;
    declare var SESSION_URL: string;

    // Expose outside of the controller
    export var courseManageScope: IManageCourseScope;

    /*** ANGULAR CONTROLLER ***/
    export class CourseManageController {

        constructor($scope: ManageCourseModule.IManageCourseScope, $http: ng.IHttpService) {
            courseManageScope = $scope;
            courseManageScope.httpService = $http;
            courseManageScope.treeData = [{ id: -1, text: "" }];
            courseManageScope.itemTemplate = "<span ng-click='openEditor(dataItem)'>{{dataItem.text}}</span>";

            courseManageScope.init = function () {
                courseManageScope.loadData(API_URL + "/CoursesInCourseCategories/GetCourseCategoriesAndCourses/");
            }

            courseManageScope.getDefaultCategory = function () {
                var defaultCategory: ICategory = { Name: "", Notes: "", CourseCategoryId: -1 };
                return JSON.parse(JSON.stringify(defaultCategory));
            }

            courseManageScope.getDefaultCourse = function () {
                var defaultCourse: ICourse = { Name: "", Description: "", CourseId: -1 };
                return JSON.parse(JSON.stringify(defaultCourse));
            }

            courseManageScope.loadData = function (file) {
                courseManageScope.httpService({
                    method: "GET",
                    url: file,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: string) {
                    $(".tree-view").hide();
                    courseManageScope.treeData = JSON.parse(data);
                    $(".k-loading").parent().slideUp(function () {
                        $(".tree-view").slideDown();
                    });
                })
                .error(function (data: IErrorData, status: number) {
                    courseManageScope.showCourseFailure(data);
                });
            };

            // the user clicked on the tree. Ascertain whether to open the category editor or the course editor
            courseManageScope.openEditor = function (dataItem) {
                $.each(courseManageScope.treeData, function (i, categoryNode) {
                    if (categoryNode.text == dataItem.text) {
                        courseManageScope.loadCategoryEditor(dataItem.id);
                    }

                    $.each(categoryNode.items, function (j, courseNode) {
                        if (courseNode.text == dataItem.text) {
                            // set the current course (for course management)
                            courseManageScope.Course.Name = dataItem.text;
                            courseManageScope.Course.CourseId = dataItem.id;
                            // set the current category (for course deletion)
                            courseManageScope.Category.CourseCategoryId = categoryNode.id;
                            courseManageScope.loadCourseEditor();
                        }
                    });
                });
            }

            courseManageScope.loadCategoryCreator = function () {
                // hide editor and show loading
                $("#EditorContainer").slideUp();
                $("#CourseContainer").hide();

                // clear the fields
                courseManageScope.Category = courseManageScope.getDefaultCategory();

                $("#EditorContainer").slideDown(function () {
                    // hide the update and delete buttons, show the create button
                    $(".editor-btn").addClass("hidden");
                    $("#CreateCategory").show();
                });
            }

            courseManageScope.loadCategoryEditor = function (categoryId) {
                // hide editor and show loading
                $("#EditorContainer").slideUp();
                $("#CourseContainer").hide();
                $("#LoadingContainer").slideDown(function () {
                    //get the data
                    courseManageScope.httpService({
                        method: "GET",
                        url: API_URL + "/CourseCategories/GetCourseCategory/" + categoryId,
                        headers: {
                            "Authorization": "Bearer " + TOKEN
                        }
                    })
                    .success(function (data: ICategory) {
                        courseManageScope.Category = data;
                    })
                    .error(function (data: IErrorData, status: number) {
                        courseManageScope.showCourseFailure(data);
                    });

                    // hide loading and show the update editor
                    $("#LoadingContainer").slideUp();
                    $("#EditorContainer").slideDown();

                    // show the update / delete category buttons, and the create course button
                    $("#CreateCategory").hide();
                    $(".editor-btn").removeClass("hidden");
                });
            }

            courseManageScope.createCategory = function () {
                courseManageScope.httpService({
                    method: "POST",
                    url: API_URL + "/CourseCategories/CreateCourseCategory/",
                    data: { Name: courseManageScope.Category.Name, Notes: courseManageScope.Category.Notes },
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: ICategory) {
                    courseManageScope.Category = data;
                    courseManageScope.showCategorySuccess();
                })
                .error(function (data: IErrorData, status: number) {
                    courseManageScope.showCourseFailure(data);
                });
            }

            courseManageScope.updateCategory = function () {
                courseManageScope.httpService({
                    method: "PUT",
                    url: API_URL + "/CourseCategories/UpdateCourseCategory/" + courseManageScope.Category.CourseCategoryId,
                    data: courseManageScope.Category,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    courseManageScope.showCategoryUpdated();
                })
                .error(function (data: IErrorData, status: number) {
                    courseManageScope.showCourseFailure(data);
                });
            }

            courseManageScope.deleteCategory = function () {
                courseManageScope.httpService({
                    method: "DELETE",
                    url: API_URL + "/CourseCategories/DeleteCourseCategory/" + courseManageScope.Category.CourseCategoryId,
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    $("#DeleteCategoryModal").modal("hide");
                    // clear the fields
                    courseManageScope.Category = courseManageScope.getDefaultCategory();
                    courseManageScope.Course = courseManageScope.getDefaultCourse();
                    courseManageScope.showCategoryDelete();
                })
                .error(function (data: IErrorData, status: number) {
                    $("#DeleteCategoryModal").modal("hide");
                    courseManageScope.showCourseFailure(data);
                });
            }

            courseManageScope.loadCourseEditor = function () {
                var editUrl = EDIT_URL + "/" + courseManageScope.Course.CourseId;
                var sessionUrl = SESSION_URL + "/" + courseManageScope.Course.CourseId;

                // hide editor and show loading
                $("#EditorContainer").hide();
                $("#CourseContainer").slideDown(function () {
                    $("#EditCourse").attr("href", editUrl);
                    $("#ManageSessions").attr("href", sessionUrl);
                    $(this).removeClass("hidden");
                });
            }

            courseManageScope.clearCourse = function () {
                courseManageScope.Course = courseManageScope.getDefaultCourse();
            }

            courseManageScope.createCourse = function () {
                courseManageScope.httpService({
                    method: "POST",
                    url: API_URL + "/Courses/CreateCourse/",
                    data: { Name: courseManageScope.Course.Name, Description: courseManageScope.Course.Description },
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data: ICourse) {
                    courseManageScope.Course = data;
                    courseManageScope.addCourseToCourseCategory();
                })
                .error(function (data: IErrorData, status: number) {
                    courseManageScope.showCourseFailure(data);
                });
            }

            courseManageScope.addCourseToCourseCategory = function () {
                courseManageScope.httpService({
                    method: "POST",
                    url: API_URL + "/CoursesInCourseCategories/AddCourseToCourseCategory/",
                    data: { CourseCategoryId: courseManageScope.Category.CourseCategoryId, CourseId: courseManageScope.Course.CourseId },
                    headers: {
                        "Authorization": "Bearer " + TOKEN
                    }
                })
                .success(function (data) {
                    $("#CreateCourseModal").modal("hide");
                    courseManageScope.showCourseSuccess();
                })
                .error(function (data: IErrorData, status: number) {
                    courseManageScope.showCourseFailure(data);
                });
            }

            courseManageScope.deleteCourse = function () {
                courseManageScope.httpService({
                    method: "DELETE",
                    url: API_URL + "/CoursesInCourseCategories/DeleteCourseInCourseCategory/",
                    data: { CourseCategoryId: courseManageScope.Category.CourseCategoryId, CourseId: courseManageScope.Course.CourseId },
                    headers: {
                        "Authorization": "Bearer " + TOKEN,
                        "Content-Type": "application/json"
                    }
                })
                .success(function (data) {
                    $("#DeleteCourseModal").modal("hide");
                    courseManageScope.showCourseDelete();
                })
                .error(function (data: IErrorData, status: number) {
                    courseManageScope.showCourseFailure(data);
                });
            }

            courseManageScope.showCategorySuccess = function () {
                var msg = "Category: <b>" + courseManageScope.Category.Name + "</b> successfully created!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                $('#treeview').slideUp();
                courseManageScope.init();
                $('#treeview').slideDown();
                courseManageScope.loadCategoryEditor(courseManageScope.Category.CourseCategoryId);
            }

            courseManageScope.showCategoryUpdated = function () {
                var msg = "Category: <b>" + courseManageScope.Category.Name + "</b> successfully updated!";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                $('#treeview').slideUp();
                courseManageScope.init();
                $('#treeview').slideDown();
            }

            courseManageScope.showCategoryDelete = function () {
                var msg = "Category: <b>" + courseManageScope.Category.Name + "</b> successfully deleted!";

                $(".alert-danger .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                $('#treeview').slideUp();
                courseManageScope.init();
                $('#treeview').slideDown();
                courseManageScope.loadCategoryCreator();
            }

            courseManageScope.showCourseSuccess = function () {
                var msg = "Course: <b>" + courseManageScope.Course.Name + "</b> successfully added to category: <b>" + courseManageScope.Category.Name + "</b>.";

                $(".alert-success .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                $('#treeview').slideUp();
                courseManageScope.init();
                $('#treeview').slideDown();
            }

            courseManageScope.showCourseDelete = function () {
                var msg = "Course: <b>" + courseManageScope.Course.Name + "</b> successfully deleted!";

                $(".alert-danger .msg")
                    .html(msg)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();

                $('#treeview').slideUp();
                courseManageScope.init();
                $('#treeview').slideDown();
                courseManageScope.loadCategoryCreator();
            }

            courseManageScope.showCourseFailure = function (data) {

                $(".alert-danger .msg")
                    .html(data.ExceptionMessage)
                    .parent()
                    .removeClass("hidden")
                    .slideDown();
            }

            // run the initialize method
            courseManageScope.init();
            courseManageScope.Category = courseManageScope.getDefaultCategory();
            courseManageScope.Course = courseManageScope.getDefaultCourse();

        }
    }
}

// Attach the controller to the app
var app = angular.module("LMS", ["kendo.directives"]);
app.controller("CourseManageController", ["$scope", "$http", ManageCourseModule.CourseManageController]); 