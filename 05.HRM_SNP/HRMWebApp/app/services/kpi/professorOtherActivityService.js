define(['app/app'], function (app) {
    "use strict";

    app.factory('professorOtherActivityService', ['$http', '$q', function ($http, $q) {
        return {
            getObj: function(Id){
                return $http.get('/Api/professorOtherActivityApi/GetObj?Id=' + Id);
            },
            getListByPlanDetail: function (planDetailId) {
                return $http.get('/Api/professorOtherActivityApi/GetListByPlanDetail?Id=' + planDetailId);
            },
            Save: function (Obj)
            {
                var deferred = $q.defer();
                $http({
                    method: 'Put',
                    url: '/Api/professorOtherActivityApi/Put',
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
                    url: '/Api/professorOtherActivityApi/Delete',
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