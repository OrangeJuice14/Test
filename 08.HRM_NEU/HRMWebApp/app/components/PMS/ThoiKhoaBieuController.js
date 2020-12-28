define(['app/app', 'app/directives/directives', 'app/components/PMS/PMSService', 'moment', 'jquery.fileupload'], function (app) {
    "use strict";

    app.controller('ThoiKhoaBieuController', ['$scope', '$rootScope', '$state', '$modal', 'PMSService',
        function ($scope, $rootScope, $state, $modal, PMSService) {
            var moment = require('moment');
            $scope.IsLoading = false;
            PMSService.GetNamHoc().then(function (result) {
                $scope.namHocList = result.data;
                if ($scope.namHocList.length > 0) {
                    var namHoc = $scope.namHocList.filter(q => moment(moment()).isSameOrAfter(q.NgayBatDau) && moment(moment()).isSameOrBefore(q.NgayKetThuc))[0];
                    $scope.namHocId = namHoc ? namHoc.Oid : $scope.namHocList[0].Oid;
                }
                else $scope.namHocId = MANAGER.GUID_EMPTY;
                $scope.getHocKy();
            })
            $scope.getHocKy = function () {
                PMSService.GetHocKy($scope.namHocId).then(function (result) {
                    $scope.hocKyList = result.data;
                    $scope.hocKyList.unshift({
                        Oid: MANAGER.GUID_EMPTY,
                        TenHocKy: 'Cả năm'
                    })
                    if ($scope.hocKyList.length > 0) {
                        var hocKy = $scope.hocKyList.filter(q => moment(moment()).isSameOrAfter(q.TuNgay) && moment(moment()).isSameOrBefore(q.DenNgay))[0];
                        $scope.hocKyId = hocKy ? hocKy.Oid : MANAGER.GUID_EMPTY;
                    }
                    else $scope.hocKyId = MANAGER.GUID_EMPTY;
                    $scope.loadDataTKB();
                    $scope.loadDataKeKhai();
                    $scope.HDKhac_LayDanhSachKeKhai();
                    $scope.KiemTraKhoaImport();
                })
            }
            PMSService.GetBacDaoTao().then(function (result) {
                $scope.listBacDaoTao = result.data;
            })
            PMSService.GetHeDaoTao().then(function (result) {
                $scope.listHeDaoTao = result.data;
            })
            PMSService.GetBoPhan().then(function (result) {
                $scope.listBoPhan = result.data;
            })
            $scope.loadData = function () {
                $scope.loadDataTKB();
                $scope.loadDataKeKhai();
                $scope.HDKhac_LayDanhSachKeKhai();
                $scope.loadDataLoaiHuongDan();
                $scope.KiemTraKhoaImport();
            }
            $scope.loadDataTKB = function () {
                $scope.IsLoading = true;
                PMSService.DanhSachThoiKhoaBieu($scope.namHocId, $scope.hocKyId, $rootScope.session.Id).then(function (result) {
                    $scope.obj = {};
                    if (result.data.length > 0) {
                        $scope.obj = result.data[0];
                    }
                    $scope.listTKB = result.data;
                    $scope.IsLoading = false;
                })
            }
            $scope.loadDataKeKhai = function () {
                $scope.IsLoading = true;
                PMSService.KeKhaiHDTKB($scope.namHocId, $scope.hocKyId, $rootScope.session.Id).then(function (result) {
                    $scope.listKeKhai = result.data;
                    $scope.IsLoading = false;
                })
            }
            $scope.loadDataLoaiHuongDan = function () {
                $scope.IsLoading = true;
                PMSService.GetLoaiHoatDongAll().then(function (result) {
                    $scope.listHoatDong = result.data;
                    $scope.IsLoading = false;
                })
            }
            $scope.duocImport = false;
            $scope.KiemTraKhoaImport = function () {
                PMSService.KiemTraKhoaImport($scope.namHocId, $scope.hocKyId, $rootScope.session.Id).then(function (result) {
                    $scope.duocImport = result.data.KetQua_Import;
                })
            }

            function download(dataurl, filename) {
                var a = document.createElement("a");
                a.href = dataurl;
                a.setAttribute("download", filename);
                a.click();
            }

            $scope.isUploading = false;
            $('#fileupload').fileupload({
                autoUpload: true,
                singleFileUploads: true,
                disableImageResize: false,
                uploadTemplateId: null,
                downloadTemplateId: null,
                add: function (e, data) {
                    $scope.isUploading = true;
                    if ($scope.namHocId == MANAGER.GUID_EMPTY || $scope.hocKyId == MANAGER.GUID_EMPTY) {
                        Notify('Vui lòng chọn học kỳ!', 'top-right', '3000', 'warning', 'fa-warning', true);
                        return;
                    }
                    data.url = '/PMSUpload/Import?NamHoc=' + $scope.namHocId + '&HocKy=' + $scope.hocKyId + '&username=' + $rootScope.session.UserName;
                    var acceptFileTypes = /(\.|\/)(xlsx?)$/i;

                    $.each(data.files, function (index, file) {
                        if (!acceptFileTypes.test(file['name'])) {
                            if (file.error == null) file.error = [];
                            file.error.push('Chỉ chấp nhận file excel');
                        }
                        if (file['size'] && file['size'] > 20971520) {
                            if (file.error == null) file.error = [];
                            file.error.push('Dung lượng file quá lớn (tối đa 20MB)');
                        }
                    });

                    $.each(data.files, function (index, file) {
                        $('#div_files').empty();
                        var error = file.error != null ? file.error.join(', ') : '';
                        file.context = $('<span>' + file.name + '</span><span style="color: red">' + error + '</span>')
                        .appendTo('#div_files');
                    });
                    data.files = data.files.filter(function (f) {
                        return f.error == null;
                    });
                    if (data.files.length > 0) {
                        $scope.$apply();
                        data.submit();
                    }
                },
                done: function (e, data) {
                    if (data.result != "0") {
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        download(location.protocol + '//' + location.hostname + (location.port ? ':' + location.port : '') + data.result, data.result.split("/")[data.result, data.result.split("/").length -1])
                        //window.open("https://docs.google.com/gview?url="+window.location.hostname +  data.result);
                        //PMSService.DeleteFile(data.result)
                        $scope.loadData();
                        //downloadURI(data.result);
                        //$state.go($state.current, {}, { reload: true });
                    } else {
                        Notify('File không có dữ liệu import!', 'top-right', '3000', 'warning', 'fa-warning', true);
                    }
                    $scope.isUploading = false;
                    $scope.$apply();
                    $('#div_files').empty();
                },
                error: function (e, data) {
                    $scope.isUploading = false;
                    Notify('Đã có lỗi xảy ra!', 'top-right', '3000', 'warning', 'fa-warning', true);
                    console.log(e.responseText);
                    $scope.$apply();
                }
            });
            $scope.xacNhan = function (item) {
                PMSService.XacNhanThoiKhoaBieu(item.Oid, $rootScope.session.UserName, item.GhiChu).then(function (result) {
                    if (result.data != 0) {
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        $scope.loadData();
                    }
                    else Notify('Có lỗi!', 'top-right', '3000', 'warning', 'fa-warning', true);
                })
            }
            $scope.themMoiKeKhaiRow = function () {
                $scope.taoMoiKeKhai = true;
                $scope.keKhaiMoi = {
                    Oid_NhanVien: $rootScope.session.Id,
                    Oid_LoaiHuongDan: '',
                    NamHoc: $scope.namHocId,
                    HocKy: $scope.hocKyId,
                    TenMonHoc: '',
                    LopHocPhan: '',
                    BacDaoTao: '',
                    HeDaoTao: '',
                    SoLuongHuongDan: ''
                }
            }
            $scope.themMoiKeKhai = function (item) {
                var obj = {
                    Oid_NhanVien: $rootScope.session.Id,
                    Oid_LoaiHuongDan: item.Oid_LoaiHuongDan,
                    NamHoc: $scope.namHocId,
                    HocKy: $scope.hocKyId,
                    TenMonHoc: item.TenMonHoc,
                    LopHocPhan: item.LopHocPhan,
                    BoMon: item.BoMon,
                    BacDaoTao: item.BacDaoTao,
                    HeDaoTao: item.HeDaoTao,
                    SoBaiKiemTra: item.SoBaiKiemTra,
                    SoBaiThi: item.SoBaiThi,
                    SoBaiTapLon: item.SoBaiTapLon,
                    SoBaiTieuLuan: item.SoBaiTieuLuan,
                    SoDeAnMonHoc: item.SoDeAnTotNghiep,
                    SoChuyenDeTN: item.SoChuyenDeTotNghiep,
                    SoHDKhac: item.SoHDKhac,
                    SoSlotHoc: item.SoSlotHoc,
                    SoTraLoiCauHoi: item.SoTraLoiCauHoiTrenHeThongHocTap,
                    SoTruyCapLopHoc: item.SoTruyCapLopHoc,
                    SoDeRaDe: item.SoDeRaDe,
                    SoLuongHuongDan: item.SoLuongHuongDan
                }
                PMSService.KeKhaiHDTKBThem(obj).then(function (result) {
                    if (result.data != 0){
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        $scope.taoMoiKeKhai = false;
                        $scope.loadDataKeKhai();
                    }
                    else Notify('Có lỗi!', 'top-right', '3000', 'warning', 'fa-warning', true);
                })
            }
            $scope.xacNhanKeKhai = function (item) {
                var obj = {
                    OidChiTiet: item.Oid,
                    User: $rootScope.session.UserName,
                    SoBaiKT: item.SoBaiKiemTra,
                    SoBaiThi: item.SoBaiThi,
                    SoBaiTapLon: item.SoBaiTapLon,
                    SoBaiTieuLuan: item.SoBaiTieuLuan,
                    SoDeAnMonHoc: item.SoDeAnTotNghiep,
                    SoChuyenDeTN: item.SoChuyenDeTotNghiep,
                    SoHDKhac: item.SoHDKhac,
                    SoSlotHoc: item.SoSlotHoc,
                    SoTraLoiCauHoi: item.SoTraLoiCauHoiTrenHeThongHocTap,
                    SoTruyCapLopHoc: item.SoTruyCapLopHoc,
                    SoDeRaDe: item.SoDeRaDe,
                    SoLuongHuongDan: item.SoLuongHuongDan
                }
                PMSService.KeKhaiHDTKBCapNhat(obj).then(function (result) {
                    if (result.data != 0){
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        $scope.loadDataKeKhai();
                    }
                    else Notify('Có lỗi!', 'top-right', '3000', 'warning', 'fa-warning', true);
                })
            }

            //Hoạt động khác
            $scope.hoatDongKhac = {};
            $scope.hoatDongKhac.NgayThucHien = new Date();
            PMSService.DanhSachHoatDong().then(result => {
                $scope.nhomHoatDongList = result.data;
                if ($scope.nhomHoatDongList.length > 0)
                    $scope.hoatDongKhac.NhomHoatDong = $scope.nhomHoatDongList[0].Oid;
                $scope.getHoatDong();
            })
            $scope.getHoatDong = function () {
                PMSService.DanhSachHoatDong($scope.hoatDongKhac.NhomHoatDong).then(result => {
                    $scope.hoatDongList = result.data;
                    if ($scope.hoatDongList.length > 0)
                        $scope.hoatDongKhac.HoatDong = $scope.hoatDongList[0].Oid;
                })
            }
            $scope.HDKhac_LayDanhSachKeKhai = function () {
                PMSService.HDKhac_LayDanhSachKeKhai($scope.namHocId, $scope.hocKyId, $rootScope.session.Id).then(result => {
                    $scope.listHoatDongKhac = result.data;
                })
            }
            $scope.themHoatDongKhac = function (item) {
                PMSService.HDKhac_KeKhai($scope.namHocId, $scope.hocKyId, $rootScope.session.Id, $scope.hoatDongKhac.HoatDong, $scope.hoatDongKhac.BoMon || MANAGER.GUID_EMPTY, $scope.hoatDongKhac.SoGioThucHien, $scope.hoatDongKhac.GhiChu, moment($scope.hoatDongKhac.NgayThucHien).format('YYYY/MM/DD')).then(function (result) {
                    if (result.data && result.data.TrangThai == 1) {
                        Notify(result.data.DienGiai || 'Lỗi không xác định!', 'top-right', '3000', 'success', 'fa-check', true);
                        $scope.HDKhac_LayDanhSachKeKhai();
                    }
                    else Notify(result.data.DienGiai || 'Lỗi không xác định!', 'top-right', '3000', 'warning', 'fa-warning', true);
                })
            }
            $scope.capNhatHoatDongKhac = function (item) {
                PMSService.HDKhac_CapNhatGioKeKhai(item.Oid, item.SoGioThucHien, item.GhiChu, $rootScope.session.UserName).then(function (result) {
                    if (result.data != 0) {
                        Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        $scope.HDKhac_LayDanhSachKeKhai();
                    }
                    else Notify('Có lỗi!', 'top-right', '3000', 'warning', 'fa-warning', true);
                })
            }
        }
    ]);
});