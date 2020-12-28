define(['app/app'], function (app) {
    "use strict";
    app.factory('dodulieudanhgiaService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};
        serviceResult.getList = function () {
            return $http.get('/Api/dodulieudanhgiaApi/GetList');
        };
        serviceResult.getListData = function () {
            return $http.get('/Api/dodulieudanhgiaApi/GetListData');
        };
        serviceResult.SaveFileToData = function (manageCode) {
            return $http.get('/Api/dodulieudanhgiaApi/GetListDictionaryByManageCode?manageCode=' + manageCode);
        };
        serviceResult.getObj = function (id) {
            return $http.get('/Api/dodulieudanhgiaApi/GetObj?id=' + id);
        };
        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/dodulieudanhgiaApi/Delete',
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
});
