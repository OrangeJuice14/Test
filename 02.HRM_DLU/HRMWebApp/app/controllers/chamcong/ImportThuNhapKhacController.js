
define(['app/app', 'app/services/kpi/agentObjectService'], function (app) {
    "use strict";
    app.controller('ImportThuNhapKhacController', ['$scope', '$modal', '$rootScope', 'agentObjectService',
            function ($scope, $modal, $rootScope, agentObjectService) {             
                $scope.kyTinhLuongResource = {
                    placeholder: "Chọn kỳ tính lương...",
                    dataTextField: "Name",
                    dataValueField: "Oid",
                    valuePrimitive: true,
                    filter: "contains",
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return agentObjectService.getList_KyTinhLuong().then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        }
                    }
                };
                //agentObjectService.getList_KyTinhLuong().then(function (result) {
                //    $scope.kyTinhLuongList = result.data;
                //});
                //$scope.kyTinhLuong = "";
                //agentObjectService.getList_LoaiThuNhapKhac().then(function (result) {
                //    $scope.loaiThuNhapList = result.data;
                //});
                $scope.loaiThuNhap = "";
                $scope.options = {
                    filter: "contains"
                }
                $scope.uploadOptions = {
                    async: {
                        saveUrl: "/ThuNhapKhacImportData/SaveFileToData",
                        autoUpload: true
                    },
                    upload: function (e) {
                        //e.data = { kyTinhLuong: $scope.kyTinhLuong, loaiThuNhap: $scope.loaiThuNhap };
                        e.data = { kyTinhLuong: $scope.kyTinhLuong};
                    },
                    success: onUploadSuccess,
                    localization: {
                        select: "Chọn mẫu lương...",
                        cancel: "Hủy",
                        retry: "Thử lại",
                        remove: "Xóa",
                        uploadSelectedFiles: "Những file đang tải",
                        dropFilesHere: "Kéo thả file vào đây để tải lên",
                        statusUploading: "Đang tải",
                        statusUploaded: "Đã tải xong",
                        statusWarning: "Cảnh báo",
                        statusFailed: "Thất bại",
                        headerStatusUploading: "Đang tải...",
                        headerStatusUploaded: "Hoàn thành"
                    }
                }

                function onUploadSuccess(e) {
                    if (e.response == "1") {
                        alert("Thành công!");
                    }
                    else {
                        alert("Dữ liệu lỗi, vui lòng kiểm tra lại dữ liệu hoặc thông báo nhà cung cấp phần mền !!!");
                    }
                }
                $scope.numericOptions = {
                    format: "n0",
                    min: 0
                }
                $scope.New = function () {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/chamcong/QuanLyChamCong/LoaiThuNhapKhac.html',
                        controller: 'LoaiThuNhapKhacDetailController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            }
                        }
                    }).result.then(function (result) {
                        agentObjectService.getList_LoaiThuNhapKhac().then(function (result) {
                            $scope.loaiThuNhapList = result.data;
                        });
                    });
                };
            }
    ]);
    app.controller('LoaiThuNhapKhacDetailController', ['$scope', '$modalInstance', 'id', 'agentObjectService',
       function ($scope, $modalInstance, id, agentObjectService) {
           $scope.obj = {};
           $scope.obj = {
               Id: MANAGER.GUID_EMPTY,
               Name: "",
           };
           $scope.save = function () {
               agentObjectService.SaveThuNhapKhac($scope.obj).then(function () {
                   alert("Thành công!")
                   $modalInstance.close();
               });
           };
           $scope.cancel = function () {
               $modalInstance.dismiss('cancel');
           };
       }
    ]);
});