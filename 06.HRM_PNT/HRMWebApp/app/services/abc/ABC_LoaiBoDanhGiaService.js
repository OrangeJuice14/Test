define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_LoaiBoDanhGiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.GetAll = function () {
            return $http.get('/Api/ABC_LoaiBoDanhGiaApi/GetAllDTO');
        }

        return serviceResult;
    }]);
});