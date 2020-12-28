define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('giaHanNhapDiemService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.getCurrent = function () {
            return $http.get('/api/coreUisApi/GetCurrentStudyYear');
        };
        serviceResult.getListStudyYear = function() {
            return $http.get('/api/coreUisApi/GetListStudyYear');
        };
        serviceResult.getListStudyTerm = function () {
            return $http.get('/api/coreUisApi/GetListStudyTerm');
        };
        serviceResult.LoaiNhapDiem_sel = function () {
            return $http.get('/api/coreUisApi/LoaiNhapDiem_sel');
        };
      
        serviceResult.GiaHanNhapDiem_sel = function (YearStudy, TermID,Type) {
            if (YearStudy != null && YearStudy!=""  && TermID != null && TermID!="")
                return $http.get('/api/coreUisApi/GiaHanNhapDiem_sel?YearStudy=' + YearStudy + '&TermID=' + TermID+'&Type='+Type);
        };
        serviceResult.GiaHanNhapDiem_Upd = function (YearStudy, TermID, Type,data) {
            if (YearStudy != null && YearStudy != "" && TermID != null && TermID != "")
                return $http.post('/api/coreUisApi/GiaHanNhapDiem_Upd?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Type=' + Type, data);
                //$http.post('/api/coreUisApi/GiaHanNhapDiem_Upd?YearStudy=' + YearStudy + '&TermID=' + TermID + '&Type=' + Type, data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;' } })
                //.then(function (response) {
                //    return response;
                //});
        };

        return serviceResult;
    }]);
});