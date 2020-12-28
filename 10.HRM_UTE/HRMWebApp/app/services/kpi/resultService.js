define(['app/app'], function (app) {
    "use strict";

    app.factory('resultService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function (planId, agentObjectId) {
            return $http.get('/Api/resultApi/GetResultList?planId=' + planId + "&agentObjectId=" + agentObjectId);
        }

        return serviceResult;

    }]);
});