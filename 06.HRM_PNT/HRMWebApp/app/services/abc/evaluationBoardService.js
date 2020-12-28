define(['app/app'], function (app) {
    "use strict";

    

    app.factory('evaluationBoardService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getList = function () {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetList');
        }
        serviceResult.getListEvaluationManage = function () {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetListEvaluationManage');
        }
        serviceResult.getListStaffEvaluationBySupervisor = function (evaluationId) {
            return $http.get('/Api/aBC_EvaluationBoardApi/getListStaffEvaluationBySupervisor?evaluationId=' + evaluationId);
        }
        serviceResult.getListStaffSyntheticEvaluation = function (evaluationId) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetListStaffSyntheticEvaluation?evaluationId=' + evaluationId);
        }
        serviceResult.getListDepartmentLeaderSyntheticEvaluation = function (evaluationId) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetListDepartmentLeaderSyntheticEvaluation?evaluationId=' + evaluationId);
        }
        serviceResult.getListDepartmentLeaderSyntheticEvaluation_Principal = function (evaluationId, isGet1) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetListDepartmentLeaderSyntheticEvaluation_Principal?evaluationId=' + evaluationId + '&isGet1=' + isGet1);
        }
        serviceResult.getListDepartmentLeaderEvaluation = function (evaluationId, supervisorId) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetListDepartmentLeaderEvaluation?evaluationId=' + evaluationId);
        }
        serviceResult.getCheckIsSupervisor = function () {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetCheckIsSupervisor');
        }
        serviceResult.getCreateEvaluationBoard = function (year) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetCreateEvaluationBoard?year=' + year);
        }
        serviceResult.getCheckExistEvaluationBoard = function (year) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetCheckExistEvaluationBoard?year=' + year);
        }
        serviceResult.getCheckLockAllChildRating = function (evaluationId, staffId, departmentId, staffType) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetCheckLockAllChildRating?evaluationId=' + evaluationId + '&staffId=' + staffId + '&departmentId=' + departmentId + '&staffType=' + staffType);
        }
        serviceResult.getCurrentUser = function () {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetCurrentUser');
        }
        serviceResult.getDepartmentList = function () {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetDepartmentList');
        }
        serviceResult.getListStaffEvaluationByDepartment = function (evaluationId, departmentId) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetListStaffEvaluationByDepartment?evaluationId=' + evaluationId + '&departmentId=' + departmentId);
        }
        serviceResult.getCurrentUserDepartment = function () {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetCurrentUserDepartment');
        }
        serviceResult.getEvaluationBoardList = function () {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetEvaluationBoardList');
        }
        serviceResult.getListStaffSyntheticEvaluationExcel = function (evaluationId, departmentId, isGet1) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetListStaffSyntheticEvaluationExcel?evaluationId=' + evaluationId + '&departmentId=' + departmentId + '&isGet1=' + isGet1);
        }
        serviceResult.getLockUnlockRating = function (evaluationId, departmentId,type) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetLockUnlockRating?evaluationId=' + evaluationId + '&departmentId=' + departmentId + '&type=' + type);
        }
        serviceResult.getLockedRatingDepartment = function (evaluationId, departmentId) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetLockedRatingDepartment?evaluationId=' + evaluationId + '&departmentId=' + departmentId);
        }
        serviceResult.getUnlockRatingStaff = function (ratingId, unlockMode) {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetUnlockRatingStaff?ratingId=' + ratingId + '&unlockMode=' + unlockMode);
        }
        serviceResult.getCheckIsAdminGroup = function () {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetCheckIsAdminGroup');
        }
        serviceResult.getListStaff = function () {
            return $http.get('/Api/aBC_EvaluationBoardApi/GetListStaff');
        }
        return serviceResult;
    }]);
});