(function () {
    var app = angular.module("LMS", []);

    var CourseEditController = function ($scope, $http) { }
    app.controller("CourseEditController", ["$scope", "$http", CourseEditController]);
}());

// TODO: port the old jquery UI controls to angular

$(function () {
    $.ajax({
        url: API_URL + "/Courses/GetCourse/" + COURSE_ID,
        beforeSend: function (request) {
            request.setRequestHeader("Authorization", "Bearer " + TOKEN);
        },
        success: function (course) {
            $("#Name").val(course.Name);
            $("#Description").val(course.Description);
            $("#CourseType").val(course.CourseType);
        }
    });

    $("#SaveCourse").click(function () {
        $.ajax({
            url: API_URL + "/Courses/UpdateCourse/" + COURSE_ID,
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", "Bearer " + TOKEN);
            },
            data: { CourseId: COURSE_ID, Name: $("#Name").val(), Description: $("#Description").val(), CourseType: $("#CourseType").val() },
            type: "PUT",
            success: function (result) {
                showCourseUpdate();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(errorThrown);
            }
        });
    });
});

function showCourseUpdate() {
    var msg = "Course: <b>" + $("#Name").val() + "</b> successfully updated!";

    $(".alert-success .msg")
        .html(msg)
        .parent()
        .removeClass("hidden")
        .slideDown();
}