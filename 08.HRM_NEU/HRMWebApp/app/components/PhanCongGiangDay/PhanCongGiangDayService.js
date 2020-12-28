define(['app/app'], function (app) {

    app.factory('PhanCongGiangDayService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};
        serviceResult.GetThoiKhoaBieu_DanhSachPhanCongGiangDay = function (NamHoc, HocKy, BoMonQuanLy) {
            return $http.get('/Api/PhanCongGiangDayApi/GetThoiKhoaBieu_DanhSachPhanCongGiangDay?NamHoc=' + NamHoc + '&HocKy=' + HocKy + '&BoMonQuanLy=' + BoMonQuanLy);
        }

        serviceResult.GetBoPhanQuanLy = function (UserId) {
            return $http.get('/Api/PhanCongGiangDayApi/GetBoPhanQuanLy?UserId=' + UserId);
        }
        serviceResult.GetListUserInBoMonId = function (boMonId) {
            return $http.get('/Api/PhanCongGiangDayApi/GetListUserInBoMonId?boMonId=' + boMonId);
        }
        serviceResult.PutPhanCongGiangDay = function (id_Chitiet, giangVienId) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/PhanCongGiangDayApi/PutPhanCongGiangDay?id_Chitiet=' + id_Chitiet + '&giangVienId=' + giangVienId
            }).success(function (result) {
                deferred.resolve(result);
            });
            return deferred.promise;
        }

        return serviceResult;
    }]);
});