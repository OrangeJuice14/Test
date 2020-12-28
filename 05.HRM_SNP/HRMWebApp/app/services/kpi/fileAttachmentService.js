define(['app/app'], function (app) {
    "use strict";

    app.factory('fileAttachmentService', ['$http', '$q', function ($http, $q) {
        return {
            getObj: function (Id) {
                return $http.get('/Api/FileAttachmentApi/GetObj?Id=' + Id);
            },
            getListByPlanDetail: function (planDetailId) {
                return $http.get('/Api/FileAttachmentApi/GetListByPlanDetail?Id=' + planDetailId);
            },
            getListByResultDetail: function (resultDetailId) {
                return $http.get('/Api/FileAttachmentApi/GetListByResultDetail?Id=' + resultDetailId);
            },
            getplanDetailFileDetail: function (planDetailFileId) {
                return $http.get('/Api/FileAttachmentApi/GetplanDetailFileDetail?Id=' + planDetailFileId);
            },
            downloadFile: function (Id) {
                //var deferred = $q.defer();
                //$http({
                //    method: 'Put',
                //    url: '/UploadFile/DownloadFile?Id=' + Id,
                //    data: Obj
                //}).success(function (result) {
                //    deferred.resolve(result);
                //});
                //return deferred.promise;
                window.open('/FileUpload/DownloadFile?Id=' + Id, '_blank', '');
                //window.location = '/FileUpload/DownloadFile?Id=' + Id;

                //return $http.get('/FileUpload/DownloadFile?Id=' + Id);
            },
            Save: function (Obj) {
                var deferred = $q.defer();
                $http({
                    method: 'Put',
                    url: '/Api/FileAttachmentApi/Put',
                    data: Obj
                }).success(function (result) {
                    deferred.resolve(result);
                });
                return deferred.promise;
            },
            Delete: function (Obj) {
                var deferred = $q.defer();
                $http({
                    method: 'Delete',
                    url: '/Api/FileAttachmentApi/Delete',
                    headers: {
                        'Content-Type': 'application/json; charset=utf8'
                    },
                    data: Obj
                }).success(function (result) {
                    deferred.resolve(result);
                });
                return deferred.promise;
            }

        }
    }]);
});