define(['app/app'], function (app) {
    "use strict";

    app.factory('planKPIDetail_KPIService', ['$http', '$q', function ($http, $q) {
        return {
            getObj: function(Id){
                return $http.get('/Api/planKPIDetail_KPIApi/GetObj?Id=' + Id);
            },
            getListByPlanDetail: function (planDetailId) {
                return $http.get('/Api/planKPIDetail_KPIApi/GetListByPlanDetail?Id=' + planDetailId);
            },
            getMaxOrderNumberKPI: function (planDetailId){
                return $http.get('/Api/planKPIDetail_KPIApi/GetMaxOrderNumberKPI?planKPIDetailId=' + planDetailId);
            },
            Save: function (Obj)
            {
                var deferred = $q.defer();
                $http({
                    method: 'Put',
                    url: '/Api/planKPIDetail_KPIApi/Put',
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
                    url: '/Api/planKPIDetail_KPIApi/Delete',
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