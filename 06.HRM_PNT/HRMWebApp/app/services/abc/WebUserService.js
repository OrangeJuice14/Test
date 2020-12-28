define(['app/app'], function (app) {
    "use strict";

    app.factory('WebUserService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.getWebUserByUserId = function (UserId) {
            var deferred = $q.defer();
            $http({
                method: 'Get',
                url: '/Api/WebUserApi/GetWebUserByUserId?UserId=' + UserId
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});



