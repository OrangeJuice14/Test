define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('dodulieunhanvienService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/dodulieunhanvienApi/getList');
        };
        serviceResult.getListManageCode = function (capmuctieu) {
            return $http.get('/Api/dodulieunhanvienApi/GetListManageCode?capmuctieu=' + capmuctieu);
        };

        serviceResult.getListProfessorCriterion = function () {
            return $http.get('/Api/dodulieunhanvienApi/GetListProfessorCriterion');
        };
        serviceResult.ListUserMTCL = function () {
            return $http.get('/Api/dodulieunhanvienApi/GetListUserMTCL');
        };
        serviceResult.ListMTCLUserDetail = function (id) {
            return $http.get('/Api/dodulieunhanvienApi/GetlistMTCLUserDetail?id=' + id);
        };
        serviceResult.getDVCC = function (MaNhomHoatDong) {
            return $http.get('/Api/dodulieunhanvienApi/GetDVCC?MaNhomHoatDong=' + MaNhomHoatDong);
        };
        serviceResult.getListPosition = function () {
            return $http.get('/Api/dodulieunhanvienApi/GetListPosition');
        };
        serviceResult.getListStudyYear = function () {
            return $http.get('/Api/dodulieunhanvienApi/GetListStudyYear');
        };
        serviceResult.getListTargetGroupDetail = function () {
            return $http.get('/Api/dodulieunhanvienApi/GetListTargetGroupDetail');
        };
        serviceResult.getUserNhap = function () {
            return $http.get('/Api/dodulieunhanvienApi/GetUserNhap');
        };
        serviceResult.getListDictionaryByManageCode = function (manageCode) {
            return $http.get('/Api/dodulieunhanvienApi/GetListDictionaryByManageCode?manageCode='+manageCode);
        };
        serviceResult.getListByStaffId = function (staffId) {
            return $http.get('/Api/dodulieunhanvienApi/GetListByStaffId?staffId='+staffId);
        };
        serviceResult.searchOtherActivity = function (capmuctieu, TargetGroupDetailId) {
            return $http.get('/Api/dodulieunhanvienApi/GetSearchOtherActivity?capmuctieu=' + capmuctieu  + '&TargetGroupDetailId=' + TargetGroupDetailId);
        };
        serviceResult.searchOtherActivityUser = function (capmuctieu, TargetGroupDetailId) {
            return $http.get('/Api/dodulieunhanvienApi/GetSearchOtherActivityUser?capmuctieu=' + capmuctieu + '&TargetGroupDetailId=' + TargetGroupDetailId);
        };

        serviceResult.getObj = function (id) {
            return $http.get('/Api/dodulieunhanvienApi/GetObj?id=' + id);
        };
        serviceResult.getObjMaster = function (id) {
            return $http.get('/Api/dodulieunhanvienApi/GetObjMaster?id=' + id);
        };

        serviceResult.getPositionObj = function (id) {
            return $http.get('/Api/dodulieunhanvienApi/GetPositionObj?id=' + id);
        };

        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/dodulieunhanvienApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.Saves = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/dodulieunhanvienApi/Puts',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        serviceResult.SaveUsers = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/dodulieunhanvienApi/PutUser',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.SavePosition = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/dodulieunhanvienApi/PutPosition',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/dodulieunhanvienApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        serviceResult.DeleteMulti = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/dodulieunhanvienApi/DeleteMulti',
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