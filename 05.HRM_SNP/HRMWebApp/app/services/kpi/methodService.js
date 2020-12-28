define(['app/app'], function (app) {
    "use strict";

    app.factory('methodService', ['$http', '$q', function ($http, $q) {
        return {
            getObj: function(Id){
                return $http.get('/Api/methodApi/GetObj?Id=' + Id);
            },
            getListByPlanDetail: function (planDetailId) {
                return $http.get('/Api/methodApi/GetListByPlanDetail?Id=' + planDetailId);
            },
            getMethodDetail: function (methodId) {
                return $http.get('/Api/methodApi/GetMethodDetail?Id=' + methodId);
            },
            Save: function (Obj)
            {
                var deferred = $q.defer();
                $http({
                    method: 'Put',
                    url: '/Api/methodApi/Put',
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
                    url: '/Api/methodApi/Delete',
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