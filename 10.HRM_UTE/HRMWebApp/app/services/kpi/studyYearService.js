define(['app/app'], function (app) {
    "use strict";
    
    app.factory('studyYearService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/studyYearApi/GetList');
        }

        return serviceResult;
    }]);
});