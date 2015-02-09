/// <reference path="../typings/angularjs/angular.d.ts" />

module UserDashboardModule {
    'use strict';

    /*** ANGULAR SCOPE ***/
    export interface IUserDashboardScope extends ng.IScope {

        // PROPERTIES
        httpService: ng.IHttpService;
        CoursesList: ICourse[];
        calendarDates: number[]; // array of Date.getTime()
        calendarConfig: ICalendarConfig;

        // PUBLIC METHODS
        loadData(): void;
        calculateTimeRemaining(course: ICourse): void;
        checkUserInput(course: ICourse): void;
        updateCalendar(): void;
        submit(course: ICourse): void;
        getDateStringFromJsonString(jsonString: string): string;
        getDateObjectFromJsonString(jsonString: string): Date;
        showCourseUpdate(course: ICourse): void;
        showUserFailure(error: any): void;
    }

    export interface ICalendarDates {
        Confirm: boolean;
    }

    export interface ICalendarConfig {
        value: Date;
        dates: number[]; // array of Date.getTime()
        month: ICalendarMonth;
        footer: string; // | boolean
    }

    export interface ICalendarMonth {
        content: string;
    }

    export interface ICourse {
        Confirm: boolean;
        CourseName: string;
        CourseContentResource: string;
        CourseContentName: string;
        CourseSessionId: number;
        CourseSessionStartDate: string;
        CourseSessionEndDate: string;
        CourseSessionStartDateString: string;
        CourseSessionEndDateString: string;
        SubmittedWithoutCheck: boolean;
        LearningComplete: boolean;
        PercentToFill: number;
        DaysRemaining: number;
        DaysRemainingString: string;
    }

    export interface IErrorData {
        Message: string;
        ExceptionMessage: string;
        ExceptionsType: string;
        StatckTrace: string;
        InnerException: IErrorData;
    }
} 