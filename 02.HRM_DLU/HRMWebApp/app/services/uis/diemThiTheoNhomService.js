define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('diemThiTheoNhomService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getCurrent = function () {
            return $http.get('/api/coreUisApi/GetCurrentStudyYear');
        };
        serviceResult.getListStudyYear = function() {
            return $http.get('/api/coreUisApi/GetListStudyYear');
        };
        serviceResult.getListStudyTerm = function () {
            return $http.get('/api/coreUisApi/GetListStudyTerm');
        };
      
        serviceResult.GetExamiationProfessor = function (YearStudy, TermID) {
            if (YearStudy != null && YearStudy!=""  && TermID != null && TermID!="")
                return $http.get('/api/coreUisApi/GetExamiationProfessor?YearStudy=' + YearStudy + '&TermID=' + TermID );
        };

        return serviceResult;
    }]);
});