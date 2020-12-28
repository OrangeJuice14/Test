define(['app/app'], function (app) {
    "use strict";

    

    app.factory('ABC_CriterionService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/ABC_CriterionApi/GetList');
        }
        serviceResult.getListCriterionDetailType = function () {
            return $http.get('/Api/ABC_CriterionApi/GetListCriterionDetailType');
        }
        serviceResult.SaveCriterion = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_CriterionApi/PutCriterion',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.getCriterionById = function (id) {
            return $http.get('/Api/ABC_CriterionApi/GetCriterionById?id=' + id);
        }
        serviceResult.getListCriterionDetailByCriterionId = function (id) {
            return $http.get('/Api/ABC_CriterionApi/GetListCriterionDetailByCriterionId?id='+ id);
        }
        serviceResult.getCheckHasCriterionDetail = function (id) {
            return $http.get('/Api/ABC_CriterionApi/GetCheckHasCriterionDetail?id=' + id);
        }
        serviceResult.SaveCriterionDetail = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_CriterionApi/PutCriterionDetail',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.getCriterionDetailById = function (id) {
            return $http.get('/Api/ABC_CriterionApi/GetCriterionDetailById?id=' + id);
        }

        serviceResult.DeleteCriterionDetail = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/ABC_CriterionApi/DeleteCriterionDetail',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.DeleteCriterion = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/ABC_CriterionApi/DeleteCriterion',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.getCheckHasDictionary = function (id) {
            return $http.get('/Api/criterionApi/GetCheckHasDictionary?id=' + id);
        }

        serviceResult.getListUser = function (userId) {
            return $http.get('/Api/criterionApi/GetListbyId?classId=' + id);
        }
        
        serviceResult.Search = function (targetGroupDetailId, departmentId) {
            return $http.get('/Api/criterionApi/GetSearch?targetGroupDetailId=' + targetGroupDetailId + '&departmentId=' + departmentId);
        }
        
        
        serviceResult.getObj = function (id) {
            return $http.get('/Api/criterionApi/GetListByClass?id=' + id);
        }

        serviceResult.getCriterionTypeList = function () {
            return $http.get('/Api/criterionApi/GetCriterionTypeList');
        }

        serviceResult.getDictionnaryByCriterionId = function (id) {
            return $http.get('/Api/criterionApi/getDictionnaryByCriterionId?id='+id);
        }

        serviceResult.getDictionnaryByTargetGroupDetailId = function (id) {
            return $http.get('/Api/criterionApi/GetDictionnaryByTargetGroupDetailId?id=' + id);
        }

        
        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/criterionApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        //MeasureUnit
        serviceResult.getListMeasureUnit = function () {
            return $http.get('/Api/criterionApi/GetListMeasureUnit');
        }
        serviceResult.getMeasureUnit = function (id) {
            return $http.get('/Api/criterionApi/GetMeasureUnit?id='+id);
        }
        serviceResult.SaveMeasure = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/criterionApi/PutMeasureUnit',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.DeleteMeasure = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/criterionApi/DeleteMeasureUnit',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
         
        //ManageCode
        serviceResult.getListManageCode = function () {
            return $http.get('/Api/criterionApi/GetListManageCode');
        }
        serviceResult.getManageCode = function (id) {
            return $http.get('/Api/criterionApi/GetManageCode?id=' + id);
        }
        serviceResult.SaveNewManageCode = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/criterionApi/PutNewManageCode',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.SaveManageCode = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/criterionApi/PutManageCode',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.DeleteManageCode = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/criterionApi/DeleteManageCode',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        } 
        
        //Dictionary
        serviceResult.getDictionary = function (id) {
            return $http.get('/Api/criterionApi/GetDictionary?id=' + id);
        }

        serviceResult.SaveDictionary = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/criterionApi/PutDictionary',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.DeleteDictionary = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/criterionApi/DeleteDictionary',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        //TargetGroup 2,3 dictionary
        serviceResult.getListTargetGroupDetail = function () {
            return $http.get('/Api/criterionApi/GetListTargetGroupDetail');
        }
        serviceResult.getListDictionaryByTargetGroupDetailId = function (id) {
            return $http.get('/Api/criterionApi/GetListDictionaryByTargetGroupDetailId?id='+id);
        }
        serviceResult.getCriterionDictionary = function (id) {
            return $http.get('/Api/criterionApi/GetCriterionDictionary?id=' + id);
        }
        serviceResult.deleteCriterionDictionary = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/criterionApi/DeleteCriterionDictionary',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }  
        serviceResult.SaveCriterionDictionary = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/criterionApi/PutCriterionDictionary',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }       
        //Configuration
         serviceResult.getListConfiguration = function () {
            return $http.get('/Api/criterionApi/GetListConfiguration');
        }
        serviceResult.getConfigurationById = function (id) {
            return $http.get('/Api/criterionApi/GetConfigurationById?id=' + id);
        }
        serviceResult.SaveConfiguration = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/criterionApi/PutConfiguration',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }  
        return serviceResult;
    }]);
});