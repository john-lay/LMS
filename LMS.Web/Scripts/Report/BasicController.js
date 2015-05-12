// Because of difficulties with using datatables with angular, were using plain old jQuery
(function () {
    var app = angular.module("LMS", []);
    var ReportController = function ($scope) { }
    app.controller("ReportController", ["$scope", ReportController]);
}());

// TODO: port the old jquery UI controls to angular

$(function () {
    $('.datepicker').datepicker({
        todayHighlight: true,
        startDate: '-10y',
        autoclose: true,
        format: 'dd/mm/yyyy'
    });

    $.ajax({
        url: API_URL + "/Reports/GetBasicReport/",
        type: "GET",
        beforeSend: function (request) {
            request.setRequestHeader("Authorization", "Bearer " + TOKEN);
        },
        success: function (result) {
            REPORT_DATA = formatDatesInData(JSON.parse(result));
            populateDropDowns();
            initialiseReport();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showReportError(jqXHR);
        }
    });

    $("#ApplyReportFilter").click(function () {
        // clone report data array
        var filteredData = REPORT_DATA.slice();

        for (var i = filteredData.length - 1; i >= 0; i--) {
            if (typeof filteredData[i] !== 'undefined') {
                if ($("#EditStartDate").val() !== "" && getDateObjectFromDateString($("#EditStartDate").val()) > getDateObjectFromDateString(filteredData[i].StartDate)
                || $("#EditEndDate").val() !== "" && getDateObjectFromDateString($("#EditEndDate").val()) < getDateObjectFromDateString(filteredData[i].EndDate)
                || $("#UserGroup").val() !== "" && $("#UserGroup").val() !== filteredData[i].UserGroupName
                //|| $("#LearningStatus").val() !== "" && $("#LearningStatus").val() !== filteredData[i].LearningComplete
                || $("#CourseCategory").val() !== "" && $("#CourseCategory").val() !== filteredData[i].CourseCategoryName
                || $("#Course").val() !== "" && $("#Course").val() !== filteredData[i].CourseName) {
                    filteredData.splice(i, 1);
                }
            }            
        }

        // update table
        $('#ReportResults').dataTable().fnClearTable();

        if (filteredData.length > 0)
            $('#ReportResults').dataTable().fnAddData(filteredData);
    });

    $("#ResetReportFilter").click(function () {
        // reset filter options
        $("#EditStartDate").val("");
        $("#EditEndDate").val("");
        $("#UserGroup")[0].selectedIndex = 0;
        $("#LearningStatus")[0].selectedIndex = 0;
        $("#CourseCategory")[0].selectedIndex = 0;
        $("#Course")[0].selectedIndex = 0;

        // update table
        $('#ReportResults').dataTable().fnClearTable();
        $('#ReportResults').dataTable().fnAddData(REPORT_DATA);
    });

    function showReportError(data) {
        $(".alert-danger .msg")
            .html(data.ExceptionMessage)
            .parent()
            .removeClass("hidden")
            .slideDown();
    }

    function initialiseReport() {
        $(".table-loading").hide();

        $('#ReportResults').dataTable({
            dom: 'T<"clear">lfrtip',
            data: REPORT_DATA,
            columns: [
                {
                    title: "Selected",
                    data: "EmailAddress",
                    render: function (data, type, row) {
                        return '<input type="checkbox" class="user" value="' + data + '" />';
                    }
                },
                { title: "First Name", data: "FirstName" },
                { title: "Last Name", data: "LastName" },
                { title: "In Group", data: "UserGroupName" },
                { title: "Learning Complete", data: "LearningComplete" },
                { title: "Course", data: "CourseName" },
                { title: "Start Date", data: "StartDate" },
                { title: "End Date", data: "EndDate" },
                { title: "Rolling Course", data: "IsRolling" }
            ],
            tableTools: {
                "sSwfPath": SWF_PATH
            }
        });
    }

    function populateDropDowns() {
        $.each(REPORT_DATA, function (i, item) {
            if (item.UserGroupName !== null && USER_GROUP_LIST.indexOf(item.UserGroupName) === -1) USER_GROUP_LIST.push(item.UserGroupName);
            if (item.CourseCategoryName !== null && COURSE_CATEGORY_LIST.indexOf(item.CourseCategoryName) === -1) COURSE_CATEGORY_LIST.push(item.CourseCategoryName);
            if (item.CourseName !== null && COURSE_LIST.indexOf(item.CourseName) === -1) COURSE_LIST.push(item.CourseName);
        });

        // populate user group
        var userOption = '';
        for (var i = 0; i < USER_GROUP_LIST.length; i++) {
            userOption += '<option value="' + USER_GROUP_LIST[i] + '">' + USER_GROUP_LIST[i] + '</option>';
        }
        $('#UserGroup').append(userOption);

        // populate course categories
        var catOption = '';
        for (var j = 0; j < COURSE_CATEGORY_LIST.length; j++) {
            catOption += '<option value="' + COURSE_CATEGORY_LIST[j] + '">' + COURSE_CATEGORY_LIST[j] + '</option>';
        }
        $('#CourseCategory').append(catOption);

        // populate courses
        var courseOption = '';
        for (var k = 0; k < COURSE_LIST.length; k++) {
            courseOption += '<option value="' + COURSE_LIST[k] + '">' + COURSE_LIST[k] + '</option>';
        }
        $('#Course').append(courseOption);
    }

    function formatDatesInData(data) {
        var result = data;
        $.each(result, function (i, item) {
            result[i].StartDate = getDateStringFromJSONString(result[i].StartDate);
            result[i].EndDate = getDateStringFromJSONString(result[i].EndDate);
        });

        return result;
    }

    function getDateStringFromJSONString(JSONString) {
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

    // parse a date from dd/mm/yyyy format
    function getDateObjectFromDateString(input) {
        var parts = input.split('/');
        // new Date(year, month [, day [, hours[, minutes[, seconds[, ms]]]]])
        return new Date(parts[2], parts[1] - 1, parts[0]); // Note: months are 0-based
    }

    // populate the email recipients when the email users dialog opens
    $('#EmailUsersModal').on('shown.bs.modal', function () {
        var recipientsList = [];
        $('#ReportResults input:checkbox').each(function () {
            recipientsList.push($(this).val());
        });
        $('#Recipients').val(recipientsList.join(','));
        $('#Subject').val('');
        $('#EmailMessage').val('');
    });

    $("#SendEmail").click(function () {
        $.ajax({
            url: API_URL + "/Contact/SendEmail/",
            beforeSend: function (request) {
                request.setRequestHeader("Authorization", "Bearer " + TOKEN);
            },
            data: {
                "recipients": $('#Recipients').val().split(','),
                "subject": $('#Subject').val(),
                "body": $('#EmailMessage').val()
            },
            type: "POST",
            success: function (result) {
                $('#EmailUsersModal').modal('hide');
                showEmailSuccess();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //console.log(errorThrown);
                showEmailFailure(errorThrown);
            }
        });
    });

    function showEmailSuccess() {
        var msg = 'Email to: <b>' + $('#Recipients').val() + '</b> successfully send!';

        $('.alert-success .msg')
            .html(msg)
            .parent()
            .removeClass('hidden')
            .slideDown();
    }

    function showEmailFailure(msg) {
        $('.alert-danger .msg')
            .html(msg.ExceptionMessage)
            .parent()
            .removeClass('hidden')
            .slideDown();
    }    
});