define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('otherActivityDataService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/otherActivityDataApi/getList');
        };
        serviceResult.getListTargetGroupdetail = function () {
            return $http.get('/Api/otherActivityDataApi/GetListTargetGroupdetail');
        };

        serviceResult.getTargetGroupdetailType = function (NhomMucTieuId) {
            return $http.get('/Api/otherActivityDataApi/GetTargetGroupdetailType?NhomMucTieuId=' + NhomMucTieuId);
        };

        serviceResult.getListProfessorCriterion = function (NhomMucTieuId) {
            return $http.get('/Api/otherActivityDataApi/GetListProfessorCriterion?NhomMucTieuId=' + NhomMucTieuId);
        };
        serviceResult.ListUserOtherActivity = function () {
            return $http.get('/Api/otherActivityDataApi/GetListUserOtherActivity');
        };
        serviceResult.ListOtherActivityUserDetail = function (id) {
            return $http.get('/Api/otherActivityDataApi/GetListOtherActivityUserDetail?id=' + id);
        };
        serviceResult.getDVCC = function (MaNhomHoatDong) {
            return $http.get('/Api/otherActivityDataApi/GetDVCC?MaNhomHoatDong=' + MaNhomHoatDong);
        };
        serviceResult.getListPosition = function () {
            return $http.get('/Api/otherActivityDataApi/GetListPosition');
        };
        serviceResult.getListStudyYear = function () {
            return $http.get('/Api/otherActivityDataApi/GetListStudyYear');
        };
        serviceResult.getUserNhap = function () {
            return $http.get('/Api/otherActivityDataApi/GetUserNhap');
        };
        serviceResult.getListDictionaryByManageCode = function (manageCode) {
            return $http.get('/Api/otherActivityDataApi/GetListDictionaryByManageCode?manageCode='+manageCode);
        };
        serviceResult.getListByStaffId = function (staffId) {
            return $http.get('/Api/otherActivityDataApi/GetListByStaffId?staffId='+staffId);
        };
        serviceResult.searchOtherActivity = function (deptId, studyYear,activityManageCode, manageCode, studyTerm) {
            return $http.get('/Api/otherActivityDataApi/GetSearchOtherActivity?deptId=' + deptId + '&studyYear=' + studyYear + '&activityManageCode=' + activityManageCode + '&manageCode=' + manageCode + '&studyTerm=' + studyTerm);
        };
        serviceResult.searchOtherActivityUser = function (deptId, studyYear, activityManageCode, manageCode, studyTerm) {
            return $http.get('/Api/otherActivityDataApi/GetSearchOtherActivityUser?deptId=' + deptId + '&studyYear=' + studyYear + '&activityManageCode=' + activityManageCode + '&manageCode=' + manageCode + '&studyTerm=' + studyTerm);
        };
        //serviceResult.getListPaging = function (skip, take,departmentId) {
        //    return $http.get('/Api/otherActivityDataApi/GetListPaging?skip=' + skip + '&take=' + take + '&departmentId=' + departmentId);

        //};

        serviceResult.getObj = function (id) {
            return $http.get('/Api/otherActivityDataApi/GetObj?id=' + id);
        };
        serviceResult.getObjMaster = function (id) {
            return $http.get('/Api/otherActivityDataApi/GetObjMaster?id=' + id);
        };

        serviceResult.getPositionObj = function (id) {
            return $http.get('/Api/otherActivityDataApi/GetPositionObj?id=' + id);
        };

        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/otherActivityDataApi/Put',
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
                url: '/Api/otherActivityDataApi/Puts',
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
                url: '/Api/otherActivityDataApi/PutUser',
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
                url: '/Api/otherActivityDataApi/PutPosition',
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
                url: '/Api/otherActivityDataApi/Delete',
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
                url: '/Api/otherActivityDataApi/DeleteMulti',
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