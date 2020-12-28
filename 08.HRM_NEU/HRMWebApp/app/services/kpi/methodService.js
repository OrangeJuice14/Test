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
            getUpdatePlanDetailDic: function (planDetailId) {
                return $http.get('/Api/methodApi/GetUpdatePlanDetailDic?planDetailId=' + planDetailId);
            },
            getMethodDetail: function (methodId) {
                return $http.get('/Api/methodApi/GetMethodDetail?Id=' + methodId);
            },
            getCheckPlanDetailMethod: function (planDetailId) {
                return $http.get('/Api/methodApi/GetCheckPlanDetailMethod?planDetailId=' + planDetailId);
            },
            getMaxOrderNumberMethods: function (planDetailId){
                return $http.get('/Api/methodApi/GetMaxOrderNumberMethods?planKPIDetailId=' + planDetailId);
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