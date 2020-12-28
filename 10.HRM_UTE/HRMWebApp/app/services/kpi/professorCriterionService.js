define(['app/app'], function (app) {
    "use strict";

    

    app.factory('professorCriterionService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/professorCriterionApi/GetList');
        }
        
        serviceResult.getListByTargetGroupDetailId = function (targetId) {
            return $http.get('/Api/professorCriterionApi/GetListByTargetGroupDetailId?targetId='+targetId);
        }
        
        serviceResult.getListChucDanh = function () {
            return $http.get('/Api/professorCriterionApi/GetListChucDanh');
        }


        serviceResult.getListByClassId = function (id) {
            return $http.get('/Api/professorCriterionApi/GetListbyId?classId=' + id);
        }

        serviceResult.getCheckHasDictionary = function (id) {
            return $http.get('/Api/professorCriterionApi/GetCheckHasDictionary?id=' + id);
        }

        serviceResult.getListUser = function (userId) {
            return $http.get('/Api/professorCriterionApi/GetListbyId?classId=' + id);
        }
        
        serviceResult.Search = function (targetGroupDetailId, studyYear, departmentId) {
            return $http.get('/Api/professorCriterionApi/GetSearch?targetGroupDetailId=' + targetGroupDetailId + '&departmentId=' + departmentId +'&studyYear=' + studyYear);
        }
        
        
        serviceResult.getObj = function (id) {
            return $http.get('/Api/professorCriterionApi/GetListByClass?id=' + id);
        }

        serviceResult.getCriterionTypeList = function () {
            return $http.get('/Api/professorCriterionApi/GetCriterionTypeList');
        }
        serviceResult.getDonViTinhList = function () {
            return $http.get('/Api/professorCriterionApi/GetDonViTinhList');
        }
        serviceResult.getDanhGiaChiTietList = function () {
            return $http.get('/Api/professorCriterionApi/GetDanhGiaChiTietList');
        }        
        serviceResult.getStudyYear = function () {
            return $http.get('/Api/professorCriterionApi/GetStudyYear');
        }        
        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/professorCriterionApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/professorCriterionApi/Delete',
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
        serviceResult.getDictionnaryByCriterionId = function (id) {
            return $http.get('/Api/professorCriterionApi/getDictionnaryByCriterionId?id=' + id);
        }
        serviceResult.getDictionnaryByProfessorCriterionId = function (id) {
            return $http.get('/Api/professorCriterionApi/getDictionnaryByProfessorCriterionId?id=' + id);
        }
        serviceResult.getDictionary = function (id) {
            return $http.get('/Api/professorCriterionApi/GetDictionary?id=' + id);
        }
        serviceResult.getDictionnaryByTargetGroupDetailId = function (id) {
            return $http.get('/Api/professorCriterionApi/GetDictionnaryByTargetGroupDetailId?id=' + id);
        }
        serviceResult.SaveDictionary = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/professorCriterionApi/PutDictionary',
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
                url: '/Api/professorCriterionApi/DeleteDictionary',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        return serviceResult;
    }]);
});