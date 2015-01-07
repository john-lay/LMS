/// <reference path="../typings/angularjs/angular.d.ts" />

module ClientModule {
    'use strict';

    /*** ANGULAR SCOPE ***/
    export interface IManageScope extends ng.IScope {

        // PROPERTIES
        httpService: ng.IHttpService;
        clientsList: IClient[];
        NewClientName: string;
        CurrentClient: IClient;

        // PUBLIC METHODS
        init(): void;
        loadData(file: string): void;
        create(): void;
        setCurrentClient(client: IClient): void;
        update(): void;
        delete(): void;
        showClientSuccess(): void;
        showClientUpdate(): void;
        showClientDelete(): void;
        showClientFailure(error: any): void;
    }

    export interface IClient {
        ClientId: number;
        Name: string;
        LogoTitle?: string;
        LogoResource?: string;
    }

    export interface IErrorData {
        Message: string;
        ExceptionMessage: string;
        ExceptionsType: string;
        StatckTrace: string;
        InnerException: IErrorData;
    }
} 