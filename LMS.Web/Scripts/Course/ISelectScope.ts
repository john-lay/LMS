/// <reference path="../typings/angularjs/angular.d.ts" />

module SelectCourseModule {
    'use strict';

    /*** ANGULAR SCOPE ***/
    export interface IManageCourseScope extends ng.IScope {

        // PROPERTIES
        httpService: ng.IHttpService;
        treeData: kendo.IDataSource[];
        itemTemplate: string;

        // PUBLIC METHODS
        loadData(): void;
        courseSelected(dataItem: kendo.IDataSource): void;
        showCourseFailure(error: any): void;
    }

    export interface IErrorData {
        Message: string;
        ExceptionMessage: string;
        ExceptionsType: string;
        StatckTrace: string;
        InnerException: IErrorData;
    }
} 