/// <reference path="../typings/angularjs/angular.d.ts" />

module ManageUserModule {
    'use strict';

    /*** ANGULAR SCOPE ***/
    export interface IManageUserScope extends ng.IScope {

        // PROPERTIES
        httpService: ng.IHttpService;
        treeData: kendo.IDataSource[];
        itemTemplate: string;
        CurrentUser: IUser;
        PasswordsMatch: boolean;

        //// PUBLIC METHODS
        init(): void;
        loadData(): void;
        checkPasswordsMatch(): void;
        createNewUser(): void;
        openEditor(data: kendo.IDataSource): void;
        loadUserEditor(selectedUserId: number): void;
        editUser(): void;
        deleteUser(): void;
        getDefaultUser(): IUser;
        showUserSuccess(): void;
        showUserUpdate(): void;
        showUserDelete(): void;
        showUserFailure(error: any): void;
    }

    export interface IUser {
        ClientId?: number;
        UserId: number;
        FirstName: string;
        LastName: string;
        EmailAddress?: string;
        Password?: string;
        ConfirmPassword?: string;
    }

    export interface IErrorData {
        Message: string;
        ExceptionMessage: string;
        ExceptionsType: string;
        StatckTrace: string;
        InnerException: IErrorData;
    }
} 