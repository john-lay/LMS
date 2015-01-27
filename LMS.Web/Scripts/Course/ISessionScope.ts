/// <reference path="../typings/angularjs/angular.d.ts" />

module CourseSessionModule {
    'use strict';

    /*** ANGULAR SCOPE ***/
    export interface ICourseSessionScope extends ng.IScope {

        // PROPERTIES
        httpService: ng.IHttpService;
        treeData: kendo.IDataSource[];
        selectedUsers: ISessionUser[];
        courseSessionsList: ICourseSession[];
        currentCourseSession: ICourseSession;
        usersInSessionList: ISessionUser[];
        SelectAll: boolean;
        EditorTitle: string;
        treeOptions: kendo.ITreeOptions;

        // PUBLIC METHODS
        loadData(): void;
        attachChangeEvent(e: kendo.KendoEvent): void;
        checkedNodeIds(nodes: any, checkedNodes: any): void;
        populateSessionTable(): void;
        setCourseSession(dataItem: ICourseSession): void;
        getDefaultCourseSession(): ICourseSession;
        showCourseSessionCreator(): void;
        showCourseSessionEditor(dataItem: ICourseSession): void;
        showCourseSessionRemover(dataItem: ICourseSession): void;
        getDateStringFromJSONString(JSONString: string): string;
        covertToISODateString(dateString: string): string;
        parseDate(input: string): Date;
        enrolUsers(): void;
        getUsersInSession(dataItem: ICourseSession): void;
        SelectAllUsers(): void;
        removeSelectedUsers(): void;
        createCourseSession(): void;
        editCourseSession(): void;
        deleteCourseSession(): void;
        showEnrolUsersSuccess(): void;
        showEnrolUsersUpdate(): void;
        showCourseCategorySuccess(): void;
        showCourseCategoryUpdate(): void;
        showCourseCategoryDelete(): void;
        showSessionFailure(error: any): void;
    }

    export interface ISessionUser {
        id: number;
        text: string;
        IsEnrolled: boolean;
    }

    export interface ICourseSession {
        CourseId: number;
        CourseSessionId: number;
        EndDate: string;
        IsRolling: boolean;
        StartDate: string;
    }

    export interface IErrorData {
        Message: string;
        ExceptionMessage: string;
        ExceptionsType: string;
        StatckTrace: string;
        InnerException: IErrorData;
    }
} 