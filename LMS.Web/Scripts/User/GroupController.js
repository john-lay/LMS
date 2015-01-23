// Because of difficulties with porting the kendo tree the UI controls on this page remain as jQuery objects
(function () {
    var app = angular.module("LMS", []);

    var UserGroupController = function ($scope, $http) {}
    app.controller("UserGroupController", ["$scope", "$http", UserGroupController]);
}());

// TODO: port the old jquery UI controls to angular

var UserGroups;
var CurrentUserGroup;

function updateTree() {
    $("#treeview").slideUp();
    $.ajax({
        url: API_URL + "/UsersInUserGroups/GetUserGroupsAndUsers/",
        beforeSend: function (request) {
            request.setRequestHeader("Authorization", "Bearer " + TOKEN);
        },
        success: function (userGroups) {
            UserGroups = JSON.parse(userGroups);
            var treeview = $("#treeview").data("kendoTreeView");
            treeview.setDataSource(new kendo.data.HierarchicalDataSource({
                data: JSON.parse(userGroups)
            }));

            // style the group elements
            $(".k-sprite.group").parent().css({ "background-color": "#e7e7e7", "padding-right": "15px" });

            $("#treeview").slideDown();
        }
    });
}

$(function () {
    // initialise tree
    $.ajax({
        url: API_URL + "/UsersInUserGroups/GetUserGroupsAndUsers/",
        beforeSend: function (request) {
            request.setRequestHeader("Authorization", "Bearer " + TOKEN);
        },
        success: function (userGroups) {
            UserGroups = JSON.parse(userGroups);
            //console.log(userGroups);
            $("#treeview").kendoTreeView({
                dragAndDrop: true,
                drop: function (e) {
                    var sourceNode = $("#treeview").data('kendoTreeView').dataItem(e.sourceNode);
                    var destinationNode = $("#treeview").data('kendoTreeView').dataItem(e.destinationNode);
                    var sourceIsGroup = $(e.sourceNode).find(".group").length != 0;
                    var destinationIsGroup = $(e.destinationNode).find(".group").length != 0;

                    // do not allow root nodes to be dropped levels
                    if (sourceNode.IsParent != destinationNode.IsParent && e.dropPosition != "over") {
                        e.setValid(false);
                    }
                    // do not allow adding to children
                    if (!destinationNode.IsParent && e.dropPosition == "over") {
                        if (sourceIsGroup) {
                            e.setValid(false);
                        }
                        if (!sourceIsGroup && !destinationIsGroup) {
                            e.setValid(false);
                        }
                    }
                },
                dataSource: JSON.parse(userGroups)
            });

            // style the group elements
            $(".k-sprite.group").parent().css({ "background-color": "#e7e7e7", "padding-right": "15px" });
        }
    });

    $("#SaveGroup").click(function () {
        if ($("#GroupName").val() !== "") {
            $.ajax({
                url: API_URL + "/UserGroups/CreateGroup/",
                beforeSend: function (request) {
                    request.setRequestHeader("Authorization", "Bearer " + TOKEN);
                },
                data: { Name: $("#GroupName").val(), ParentId: -1 },
                type: "POST",
                success: function (result) {
                    $("#CreateUserGroupModal").modal("hide");
                    showGroupSuccess();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
        }
    });

    $("#SaveUpdatedGroup").click(function () {
        if ($("#UpdateGroupName").val() !== "") {
            $.ajax({
                url: API_URL + "/UserGroups/UpdateGroup/" + CurrentUserGroup.id,
                beforeSend: function (request) {
                    request.setRequestHeader("Authorization", "Bearer " + TOKEN);
                },
                data: { Name: $("#UpdateGroupName").val(), UserGroupId: CurrentUserGroup.id },
                type: "PUT",
                success: function (result) {
                    $("#UpdateUserGroupModal").modal("hide");
                    showGroupUpdate();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(errorThrown);
                }
            });
        }
    });

    // bind to ajax created elements (clicking on categories in tree)
    $(document).on('click', '.k-in', function () {
        var isGroup = $(this).find(".group").length != 0;
        var Name = $(this).html();
        Name = Name.split("</span>")[1];
        if (isGroup) {
            $.each(UserGroups, function () {
                if (this.text == Name) {
                    CurrentUserGroup = this;

                    // show editor
                    $("#UpdateGroupName").val(this.text);
                    $("#UpdateUserGroupModal").modal("show");
                }
            });
        }
    });
});

function showGroupSuccess() {
    var msg = "Group: <b>" + $("#GroupName").val() + "</b> successfully created!";

    $(".alert-success .msg")
        .html(msg)
        .parent()
        .removeClass("hidden")
        .slideDown();

    updateTree();
}

$("#UpdateGroupOrder").click(function () {
    $.ajax({
        url: API_URL + "/UsersInUserGroups/UpdateUsersInGroup/",
        data: { '': getUserGroupsJSON() },
        type: "PUT",
        success: function (result) {
            showGroupOrderUpdate();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    });

});

function getUserGroupsJSON() {
    var tree = $("#treeview").data("kendoTreeView").dataSource.view();
    var list = [];

    $.each(tree, function (groupIndex, group) {
        // loop on items only if they are a group type item (don't want to interact with the ungrouped users)
        if (group.hasOwnProperty('items')) {
            $.each(group.items, function (userIndex, user) {
                var userGroup = { "UserGroupId": group.id, "UserId": user.id }
                list.push(userGroup);
            });
        }
    });

    return list;
}

function showGroupOrderUpdate() {
    var msg = "User Group order successfully updated!";

    $(".alert-success .msg")
        .html(msg)
        .parent()
        .removeClass("hidden")
        .slideDown();

    updateTree();
}

function showGroupUpdate() {
    var msg = "User Group: <b>" + $("#UpdateGroupName").val() + "</b> successfully updated!";

    $(".alert-success .msg")
        .html(msg)
        .parent()
        .removeClass("hidden")
        .slideDown();

    updateTree();
}