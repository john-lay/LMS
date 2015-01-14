/// <reference path="../typings/angularjs/angular.d.ts" />

module ManageAdminModule {
    'use strict';

    /*** ANGULAR SCOPE ***/
    export interface IManageAdminScope extends ng.IScope {

        // PROPERTIES
        httpService: ng.IHttpService;
        ClientsList: IClient[];
        CurrentClient: IClient;
        PasswordsMatch: boolean;
        itemTemplate: string;
        treeData: kendo.IDataSource[];
        NewUser: IUser;
        CurrentUser: IUser;

        //// PUBLIC METHODS
        init(): void;
        getDefaultUser(): IUser;
        loadClients(file: string): void;
        loadAdminUsers(): void;
        checkPasswordsMatch(): void;
        create(): void;
        edit(dataItem: IUser): void;
        update(): void;
        delete(): void;
        showUserSuccess(): void;
        showUserUpdate(): void;
        showUserDelete(): void;
        showUserFailure(error: any): void;

        //send(): void;
        //addRecipient(dataItem: kendo.IDataSource): void;
        //removeRecipient(index: number): void;
        //alreadyAdded(email: string): boolean;
        //isValidEmail(email: string): boolean;
        //showEmailSuccess(): void;
        //showEmailFailure(error: any): void;
    }

    export interface IClient {
        ClientId: number;
        Name: string;
        LogoTitle?: string;
        LogoResource?: string;
    }

    export interface IUser {
        ClientId: number;
        id: number;
        FirstName: string;
        LastName: string;
        EmailAddress: string;
        Password: string;
        ConfirmPassword: string;
    }

    export interface IErrorData {
        Message: string;
        ExceptionMessage: string;
        ExceptionsType: string;
        StatckTrace: string;
        InnerException: IErrorData;
    }
} 