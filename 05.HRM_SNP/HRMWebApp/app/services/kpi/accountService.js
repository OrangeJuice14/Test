(function () {
    "use strict";

    var HRMWebAppModule = angular.module('HRMWebApp');

    HRMWebAppModule.factory('accountService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.login = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Post',
                url: '/Account/Login',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        return serviceResult;
    }]);
})();