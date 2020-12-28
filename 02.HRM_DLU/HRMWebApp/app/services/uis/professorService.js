define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('professorService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getCurrent = function () {
            return $http.get('/api/coreUisApi/GetCurrentStudyYear');
        };


        serviceResult.getObj = function() {
            return $http.get('/api/coreUisApi/GetProfessorInfo');
        };
        serviceResult.getListStudyYear = function() {
            return $http.get('/api/coreUisApi/GetListStudyYear');
        };
        serviceResult.getListStudyTerm = function () {
            return $http.get('/api/coreUisApi/GetListStudyTerm');
        };
        serviceResult.getListStudyWeek = function (YearStudy, TermID) {
            return $http.get('/api/coreUisApi/GetListStudyWeek?YearStudy=' + YearStudy + '&TermID=' + TermID);
        };
        serviceResult.getProfessorSchedule = function (YearStudy, TermID, Week) {
            if (YearStudy != null && YearStudy!=""  && TermID != null && TermID!=""  && Week!=null && Week!= "")
                return $http.get('/api/coreUisApi/GetProfessorSchedule?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Week=' + Week);
        };

        serviceResult.getProfessorScheduleStudy = function (YearStudy, TermID) {
            return $http.get('/api/coreUisApi/GetProfessorScheduleStudy?YearStudy=' + YearStudy + '&TermID=' + TermID);
        };

    



        return serviceResult;
    }]);
});