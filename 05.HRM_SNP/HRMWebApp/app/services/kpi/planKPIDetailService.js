define(['app/app'], function (app) {
    "use strict";

  

    app.factory('planKPIDetailService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/planKPIDetailApi/GetList');
        }

        serviceResult.getObj = function (id) {
            return $http.get('/Api/planKPIDetailApi/GetObj?id=' + id);
        }

        serviceResult.getObjDTO = function (id, agentObjectTypeId) {
            return $http.get('/Api/planKPIDetailApi/GetObjDTO?id=' + id + "&agentObjectTypeId=" + agentObjectTypeId);
        }

        serviceResult.getPlanMakingDetail = function (id, agentObjectId,normalStaffId,userRole,isSupervisor) {
            return $http.get('/Api/planKPIDetailApi/GetList?planId=' + id + '&agentObjectId=' + agentObjectId + "&normalStaffId=" + normalStaffId+"&userRole="+userRole+"&isSupervisor="+isSupervisor);
        }

        serviceResult.getMeasureUnits = function () {
            return $http.get('/Api/planKPIDetailApi/GetMeasureUnits');
        }

        serviceResult.getPlanDetailActivities = function (planKPIDetailId) {
            return $http.get('/Api/planKPIDetailApi/GetPlanDetailActivities?planKPIDetailId='+planKPIDetailId);
        }

        serviceResult.Save1 = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/Put1',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.Lock = function (Obj) {
            //return $http.get('/Api/planKPIDetailApi/GetLockPlan?planStaffId=' + psId);
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/PutLock',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;


            //$http({
            //    method: 'Get',
            //    url: '/Api/planKPIDetailApi/GetLock?psId='+psId,
            //    headers: {
            //        'Content-Type': 'application/json; charset=utf8'
            //    }
            //}).success(function (result) {
            //    deferred.resolve(result);
            //});

            //return deferred.promise;
        }

        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/planKPIDetailApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj,
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});