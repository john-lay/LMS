/// <reference path="../typings/angularjs/angular.d.ts" />

module ManageCourseModule {
    'use strict';

    /*** ANGULAR SCOPE ***/
    export interface IManageCourseScope extends ng.IScope {

        // PROPERTIES
        httpService: ng.IHttpService;
        treeData: kendo.IDataSource[];
        itemTemplate: string;
        Category: ICategory;
        Course: ICourse;

        // PUBLIC METHODS
        init(): void;
        loadData(file: string): void;
        getDefaultCategory(): ICategory;
        getDefaultCourse(): ICourse;
        openEditor(data: kendo.IDataSource): void;
        loadCategoryCreator(): void;
        loadCategoryEditor(categoryId: number): void;
        createCategory(): void;
        updateCategory(): void;
        deleteCategory(): void;
        loadCourseEditor(): void;
        clearCourse(): void;
        createCourse(): void;
        addCourseToCourseCategory(): void;
        deleteCourse(): void;
        showCategorySuccess(): void;
        showCategoryUpdated(): void;
        showCategoryDelete(): void;
        showCourseSuccess(): void;
        showCourseDelete(): void;
        showCourseFailure(error: any): void;
    }

    export interface ICategory {
        Name: string;
        Notes: string;
        CourseCategoryId: number;
    }

    export interface ICourse {
        Name: string;
        Description: string;
        CourseId: number;
    }

    export interface IErrorData {
        Message: string;
        ExceptionMessage: string;
        ExceptionsType: string;
        StatckTrace: string;
        InnerException: IErrorData;
    }
} 