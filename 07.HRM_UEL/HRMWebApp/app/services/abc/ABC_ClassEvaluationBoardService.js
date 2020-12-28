define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_ClassEvaluationBoardService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/ABC_ClassEvaluationBoardApi/GetList');
        }
        serviceResult.getListStaff = function () {
            return $http.get('/Api/ABC_ClassEvaluationBoardApi/GetListStaff');
        }
        serviceResult.getEvaluationBoardList = function () {
            return $http.get('/Api/ABC_ClassEvaluationBoardApi/GetEvaluationBoardList');
        }
        serviceResult.getListStaffEvaluationBySupervisor = function (classEvaluationId) {
            return $http.get('/Api/ABC_ClassEvaluationBoardApi/GetListStaffEvaluationBySupervisor?classEvaluationId=' + classEvaluationId);
        }
        serviceResult.GetSupervisorType = function (staffId) {
            return $http.get('/Api/ABC_ClassEvaluationBoardApi/GetSupervisorType?staffId=' + staffId);
        }
        serviceResult.GetDanhSachDonViDuocPhanQuyenChoHieuTruong = function (staffId) {
            return $http.get('/Api/ABC_ClassEvaluationBoardApi/GetDanhSachDonViDuocPhanQuyenChoHieuTruong?staffId=' + staffId);
        }
        serviceResult.GetListStaffEvaluationByDepartment = function (classEvaluationId, departmentId) {
            return $http.get('/Api/ABC_ClassEvaluationBoardApi/GetListStaffEvaluationByDepartment?classEvaluationId=' + classEvaluationId + '&departmentId=' + departmentId);
        }
        return serviceResult;
    }]);
});