/// <reference path="../typings/angularjs/angular.d.ts" />

module EmailModule {
     'use strict';

    /*** ANGULAR SCOPE ***/
    export interface IEmailScope extends ng.IScope {

        // PROPERTIES
        httpService: ng.IHttpService;
        RecipientsList: string[];
        Subject: string;
        Body: string;
        itemTemplate: string;
        treeData: kendo.IDataSource[];

        // PROPERTIES - ajax errors
        errorStatus: number; // http errors: 404, 500, etc
        errorData: IErrorData;
        errorURL: string;

        // PUBLIC METHODS
        loadData(file: string): void;
        send(): void;
        addRecipient(dataItem: kendo.IDataSource): void;
        removeRecipient(index: number): void;
        alreadyAdded(email: string): boolean;
        isValidEmail(email: string): boolean;
        showEmailSuccess(): void;
        showEmailFailure(error: any): void;
    }

    export interface IErrorData {
        Message: string;
        ExceptionMessage: string;
        ExceptionsType: string;
        StatckTrace: string;
        InnerException: IErrorData;
    }
 }