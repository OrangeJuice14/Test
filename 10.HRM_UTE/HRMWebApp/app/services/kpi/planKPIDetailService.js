define(['app/app'], function (app) {
    "use strict";

  

    app.factory('planKPIDetailService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/planKPIDetailApi/GetList');
        }

        serviceResult.getObj = function (id, agentObjectTypeId) {
            return $http.get('/Api/planKPIDetailApi/GetObj?id=' + id + "&agentObjectTypeId=" + agentObjectTypeId);
        }
        serviceResult.saveVision = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/PutVision',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);          
            });
            return deferred.promise;
        }
        serviceResult.saveMission = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/PutMission',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.saveSendMail = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/PutSendMail',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.getObjDTO = function (id, agentObjectTypeId) {
            return $http.get('/Api/planKPIDetailApi/GetObjDTO?id=' + id + "&agentObjectTypeId=" + agentObjectTypeId);
        }

        serviceResult.getPlanMakingDetail = function (id, agentObjectId, normalStaffId, departmentId, userRole, isSupervisor) {
            return $http.get('/Api/planKPIDetailApi/GetList?planId=' + id + '&agentObjectId=' + agentObjectId + "&normalStaffId=" + normalStaffId + "&departmentId=" + departmentId + "&userRole=" + userRole + "&isSupervisor=" + isSupervisor);
        }

        serviceResult.getMeasureUnits = function () {
            return $http.get('/Api/planKPIDetailApi/GetMeasureUnits');
        }

        serviceResult.getMaxOrderNumberPlanKPIDetail = function (planStaffId, targetId) {
            return $http.get('/Api/planKPIDetailApi/GetMaxOrderNumberPlanKPIDetail?planStaffId=' + planStaffId + '&targetGroupId=' + targetId);
        }

        serviceResult.getWorkingModeByPlanStaff = function (planStaffId) {
            return $http.get('/Api/planKPIDetailApi/GetWorkingModeByPlanStaff?planStaffId=' + planStaffId);
        }

        serviceResult.getSaveWorkingMode = function (planStaffId, workingModeId) {
            return $http.get('/Api/planKPIDetailApi/GetSaveWorkingMode?planStaffId=' + planStaffId + '&workingModeId=' + workingModeId);
        }

        serviceResult.getLockWorkingModeProfessor = function (planStaffId) {
            return $http.get('/Api/planKPIDetailApi/GetLockWorkingModeProfessor?planStaffId=' + planStaffId);
        }

        serviceResult.getCheckIsLockWorkingMode = function (planStaffId) {
            return $http.get('/Api/planKPIDetailApi/GetCheckIsLockWorkingMode?planStaffId=' + planStaffId);
        }

        serviceResult.updatePlanDetailCache = function () {

            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/PutRefreshUpdatePlanDetailCache',
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;

            //return $http.get('/Api/planKPIDetailApi/PutUpdatePlanDetailCache');
        }

        serviceResult.getPlanDetailActivities = function (planKPIDetailId) {
            return $http.get('/Api/planKPIDetailApi/GetPlanDetailActivities?planKPIDetailId='+planKPIDetailId);
        }

        serviceResult.SaveActivity = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/PutActivity',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
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
                //serviceResult.updatePlanDetailCache().then(function () {
                    deferred.resolve(result);
                //});              
            });
            return deferred.promise;
        }
        serviceResult.Chuyennhom = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/PutChuyenNhom',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.LockPlanStaff = function (Obj) {
            //return $http.get('/Api/planKPIDetailApi/GetLockPlan?planStaffId=' + psId);
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/planKPIDetailApi/PutLockPlanStaff',
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

        serviceResult.Disable = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/planKPIDetailApi/DeleteDisable',
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