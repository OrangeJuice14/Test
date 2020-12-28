define(['app/app'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.factory('thuLaoGiangVienService', ['$http', '$q', function ($http, $q) {

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
        serviceResult.CauHinhChotGio_GetByNamHocHocKy = function (YearStudy, TermID) {
            return $http.get('/api/coreUisApi/CauHinhChotGio_GetByNamHocHocKy?NamHoc='+YearStudy+'&HocKy='+TermID);
        };
      
        serviceResult.KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang0 = function (YearStudy, TermID, DotThanhToan) {
            if (YearStudy != null && YearStudy!=""  && TermID != null && TermID!="")
                return $http.get('/api/coreUisApi/KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang0?NamHoc=' + YearStudy + '&HocKy=' + TermID + '&DotThanhToan=' + DotThanhToan);
        };
        serviceResult.KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang1 = function (YearStudy, TermID, DotThanhToan) {
            if (YearStudy != null && YearStudy != "" && TermID != null && TermID != "")
                return $http.get('/api/coreUisApi/KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang1?NamHoc=' + YearStudy + '&HocKy=' + TermID + '&DotThanhToan=' + DotThanhToan);
        };
        serviceResult.KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang2 = function (YearStudy, TermID, DotThanhToan) {
            if (YearStudy != null && YearStudy != "" && TermID != null && TermID != "")
                return $http.get('/api/coreUisApi/KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang2?NamHoc=' + YearStudy + '&HocKy=' + TermID + '&DotThanhToan=' + DotThanhToan);
        };
        

        return serviceResult;
    }]);
});