define(['app/app'], function (app) {
    "use strict";
    app.factory('diemthuongdiemtruService', ['$http', '$q', function ($http, $q) {
        var serviceResult = {};

        serviceResult.getListStudyYear = function () {
            return $http.get('/Api/DiemthuongdiemtruApi/GetListStudyYear');
        };
        serviceResult.getListDictionaryByManageCode = function (manageCode) {
            return $http.get('/Api/DiemthuongdiemtruApi/GetListDictionaryByManageCode?manageCode=' + manageCode);
        };
        serviceResult.searchdiemthuongdiemtru = function ( studyYear, studyTerm) {
            return $http.get('/Api/DiemthuongdiemtruApi/GetSearchdiemthuongdiemtru?studyYear=' + studyYear + '&studyTerm=' + studyTerm);
        };
        serviceResult.getDVCC = function (MaNhomHoatDong) {
            return $http.get('/Api/DiemthuongdiemtruApi/GetDVCC?MaNhomHoatDong=' + MaNhomHoatDong);
        };
        serviceResult.searchdiemthuongdiemtruuser = function (studyYear, studyTerm) {
            return $http.get('/Api/DiemthuongdiemtruApi/GetSearchdiemthuongdiemtruUser?studyYear=' + studyYear + '&studyTerm=' + studyTerm);
        };
        serviceResult.ListOtherActivityUserDetail = function (id) {
            return $http.get('/Api/DiemthuongdiemtruApi/GetListOtherActivityUserDetail?id=' + id);
        };
        serviceResult.ListUserOtherActivity = function (MaHoatDong) {
            return $http.get('/Api/DiemthuongdiemtruApi/GetListUserOtherActivity');
        };
        serviceResult.getObj = function (id) {
            return $http.get('/Api/DiemthuongdiemtruApi/GetObj?id=' + id);
        };
        serviceResult.getObjs = function () {
            return $http.get('/Api/DiemthuongdiemtruApi/GetObjs');
        };
        serviceResult.getListProfessorCriterion = function () {
            return $http.get('/Api/DiemthuongdiemtruApi/GetListProfessorCriterion');
        };
        serviceResult.getListDictionaryByManageCode = function (MaNhomHoatDong) {
            return $http.get('/Api/DiemthuongdiemtruApi/GetListDictionaryByManageCode?MaNhomHoatDong=' + MaNhomHoatDong);
        };
        serviceResult.getUserNhap = function () {
            return $http.get('/Api/DiemthuongdiemtruApi/GetUserNhap');
        };
        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/DiemthuongdiemtruApi/Put',
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
                url: '/Api/DiemthuongdiemtruApi/Delete',
                headers: {
                    'Content-Type': 'application/json; charset=utf8'
                },
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };
        serviceResult.getList = function () {
            return $http.get('/Api/DiemthuongdiemtruApi/getList');
        };
        return serviceResult;
    }]);
});