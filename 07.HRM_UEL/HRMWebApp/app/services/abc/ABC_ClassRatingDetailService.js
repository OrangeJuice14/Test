define(['app/app'], function (app) {
    "use strict";

    app.factory('ABC_ClassRatingDetailService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.GetClassRatingDetail = function (evaluationId, staffId, supervisorId, departmentId, isAdminRating) {
            return $http.get('/Api/ABC_ClassRatingDetailApi/GetClassRatingDetail?evaluationId=' + evaluationId + '&staffId=' + staffId + "&supervisorId=" + supervisorId + "&departmentId=" + departmentId + "&isAdminRating=" + isAdminRating);
        }

        serviceResult.PutClassRatingDetail = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_ClassRatingDetailApi/PutClassRatingDetail',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        serviceResult.PutListClassRatingDetail = function (staffList) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/ABC_ClassRatingDetailApi/PutListClassRatingDetail',
                data: staffList
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});