define(['app/app'], function (app) {
    "use strict";

    app.factory('scienceResearchService', ['$http', '$q', function ($http, $q) {
        return {
            getObj: function (Id) {
                return $http.get('/Api/scienceResearchApi/GetObj?Id=' + Id);
            },
            getListByPlanDetail: function (planDetailId) {
                return $http.get('/Api/ScienceResearchApi/GetListByPlanDetail?Id=' + planDetailId);
            },
            getMaxOrderNumberResearch: function (planKPIDetailId) {
                return $http.get('/Api/ScienceResearchApi/GetMaxOrderNumberResearch?planKPIDetailId=' + planKPIDetailId);
            },
            Save: function (Obj) {
                var deferred = $q.defer();
                $http({
                    method: 'Put',
                    url: '/Api/scienceResearchApi/Put',
                    data: Obj
                }).success(function (result) {
                    deferred.resolve(result);
                });
                return deferred.promise;
            },
            Delete: function (Obj) {
                var deferred = $q.defer();
                $http({
                    method: 'Delete',
                    url: '/Api/scienceResearchApi/Delete',
                    headers: {
                        'Content-Type': 'application/json; charset=utf8'
                    },
                    data: Obj
                }).success(function (result) {
                    deferred.resolve(result);
                });
                return deferred.promise;
            }

        }
    }]);
});