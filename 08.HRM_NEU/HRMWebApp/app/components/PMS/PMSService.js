define(['app/app'], function (app) {

    app.factory('PMSService', ['$http', '$q', function ($http, $q) {

        var serviceResult = {};

        serviceResult.GetNamHoc = function () {
            return $http.get('/Api/PMSApi/GetNamHoc');
        }
        serviceResult.GetHocKy = function (NamHoc) {
            return $http.get('/Api/PMSApi/GetHocKy?NamHoc=' + NamHoc);
        }
        serviceResult.GetBacDaoTao = function () {
            return $http.get('/Api/PMSApi/GetBacDaoTao');
        }
        serviceResult.GetHeDaoTao = function () {
            return $http.get('/Api/PMSApi/GetHeDaoTao');
        }
        serviceResult.GetBoPhan = function () {
            return $http.get('/Api/PMSApi/GetBoPhan');
        }
        serviceResult.DanhSachThoiKhoaBieu = function (NamHoc, HocKy, GiangVien) {
            return $http.get('/Api/PMSApi/DanhSachThoiKhoaBieu?NamHoc=' + NamHoc + '&HocKy=' + HocKy + '&GiangVien=' + GiangVien);
        }
        serviceResult.XacNhanThoiKhoaBieu = function (OidChiTiet, User, GhiChu) {
            return $http.get('/Api/PMSApi/XacNhanThoiKhoaBieu?OidChiTiet=' + OidChiTiet + '&User=' + User + '&GhiChu=' + GhiChu);
        }
        serviceResult.KeKhaiHDTKB = function (NamHoc, HocKy, GiangVien) {
            return $http.get('/Api/PMSApi/KeKhaiHDTKB?NamHoc=' + NamHoc + '&HocKy=' + HocKy + '&GiangVien=' + GiangVien);
        }
        serviceResult.KeKhaiHDTKBCapNhat = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/PMSApi/KeKhaiHDTKBCapNhat',
                data: Obj
            }).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        }
        serviceResult.KeKhaiHDTKBThem = function (Obj) {
            var deferred = $q.defer();
            $http({
                method: 'Put',
                url: '/Api/PMSApi/KeKhaiHDTKBThem',
                data: Obj
            }).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        }
        serviceResult.DeleteFile = function (url) {
            var deferred = $q.defer();
            $http({
                method: 'Delete',
                url: '/PMSUpload/DeleteFile?url=' + url
            }).then(function (result) {
                deferred.resolve(result);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        }
        serviceResult.KiemTraKhoaImport = function (NamHoc, HocKy, NhanVien) {
            return $http.get('/Api/PMSApi/KiemTraKhoaImport?NamHoc=' + NamHoc + '&HocKy=' + HocKy + '&NhanVien=' + NhanVien);
        }
        serviceResult.DanhSachHoatDong = function (NhomHoatDong) {
            return $http.get('/Api/PMSApi/DanhSachHoatDong?NhomHoatDong=' + NhomHoatDong);
        }
        serviceResult.GetLoaiHoatDongAll = function () {
            return $http.get('/Api/PMSApi/GetLoaiHoatDongAll');
        }
        serviceResult.HDKhac_LayDanhSachKeKhai = function (NamHoc, HocKy, GiangVien) {
            return $http.get('/Api/PMSApi/HDKhac_LayDanhSachKeKhai?NamHoc=' + NamHoc + '&HocKy=' + HocKy + '&GiangVien=' + GiangVien);
        }
        serviceResult.HDKhac_KeKhai = function (NamHoc, HocKy, GiangVien, HoatDong, BoMon, SoGioThucHien, GhiChu, NgayThucHien) {
            return $http.get('/Api/PMSApi/HDKhac_KeKhai?NamHoc=' + NamHoc + '&HocKy=' + HocKy + '&GiangVien=' + GiangVien + '&HoatDong=' + HoatDong + '&BoMon=' + BoMon + '&SoGioThucHien=' + SoGioThucHien + '&GhiChu=' + GhiChu + '&NgayThucHien=' + NgayThucHien);
        }
        serviceResult.HDKhac_CapNhatGioKeKhai = function (OidChiTiet, SoGio, GhiChu, User) {
            return $http.get('/Api/PMSApi/HDKhac_CapNhatGioKeKhai?OidChiTiet=' + OidChiTiet + '&SoGio=' + SoGio + '&GhiChu=' + GhiChu + '&User=' + User);
        }
        serviceResult.HDKhac_LayDanhSachKeKhai_Duyet = function (NamHoc, HocKy, BoPhan, NhanVien) {
            return $http.get('/Api/PMSApi/HDKhac_LayDanhSachKeKhai_Duyet?NamHoc=' + NamHoc + '&HocKy=' + HocKy + '&BoPhan=' + BoPhan + '&NhanVien=' + NhanVien);
        }
        serviceResult.HDKhac_DonVi_Duyet = function (OidChiTiet, TrangThai, User) {
            return $http.get('/Api/PMSApi/HDKhac_DonVi_Duyet?OidChiTiet=' + OidChiTiet + '&TrangThai=' + TrangThai + '&User=' + User);
        }

        return serviceResult;
    }]);
});