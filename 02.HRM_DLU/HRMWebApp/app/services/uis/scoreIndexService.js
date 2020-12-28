define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('scoreIndexService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getScore = function () {
            return 10;
        };
       
        serviceResult.getListStudyYear = function () {
            return $http.get('/api/coreUisApi/GetListStudyYear');
        };
        serviceResult.getListStudyTerm = function () {
            return $http.get('/api/coreUisApi/GetListStudyTerm');
        };
      
        return serviceResult;
    }]);
});