define(['app/app'], function (app) {
    "use strict";

    

    app.factory('danhmucMTCLService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        //=======Danh mục cấp 2========//

        serviceResult.getListDanhMuc = function (managecode) {
            return $http.get('/Api/danhmucMTCLApi/GetListDanhMuc?managecode=' + managecode);
        }
        serviceResult.getListByDanhMucId = function (id) {
            return $http.get('/Api/danhmucMTCLApi/GetListByDanhMucId?Id='+ id);
        }
        serviceResult.getListStudyYear = function () {
            return $http.get('/Api/danhmucMTCLApi/GetListStudyYear');
        }
        serviceResult.getBoPhan = function () {
            return $http.get('/Api/danhmucMTCLApi/GetBoPhan');
        }

        serviceResult.getBGH = function () {
            return $http.get('/Api/danhmucMTCLApi/GetBGH');
        }
      
        serviceResult.Save = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/danhmucMTCLApi/Put',
                data: Obj
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }
        //serviceResult.SaveManageCode = function (Obj) {
        //    var deferred = $q.defer();
        //    $http({
        //        method: 'Put',
        //        url: '/Api/criterionApi/PutManageCode',
        //        data: Obj
        //    }).success(function (result) {
        //        deferred.resolve(result);
        //    });
        //    return deferred.promise;
        //}
        serviceResult.Delete = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/Api/danhmucMTCLApi/Delete',
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