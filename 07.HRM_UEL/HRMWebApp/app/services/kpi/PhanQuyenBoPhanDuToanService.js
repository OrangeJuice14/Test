define(['app/app'], function (app) {
    "use strict";



    app.factory('PhanQuyenBoPhanDuToanService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};
        
        serviceResult.GetPhanQuyenBoPhanDuToanByUserId  = function (userId) {
            return $http.get('/Api/PhanQuyenBoPhanDuToanApi/GetPhanQuyenBoPhanDuToanByUserId?userId=' + userId);
        };

        serviceResult.Save = function (listDonViSelected, userId) {
            var deferred = $q.defer();
            $http({
                method: 'Post',
                url: '/Api/PhanQuyenBoPhanDuToanApi/PostSave?userId=' + userId,
                data: listDonViSelected
            }).then(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        };

        return serviceResult;
    }]);
});