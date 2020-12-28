
define(['app/app','app/services/kpi/agentObjectService'], function (app) {
    "use strict";
    app.controller('ImportDuLieuChamCongController', ['$scope', '$modal', '$rootScope','agentObjectService',
            function ($scope, $modal, $rootScope, agentObjectService) {
                agentObjectService.getList_KyTinhLuong().then(function (result) {
                    $scope.kyTinhLuongList = result.data;
                });
                $scope.kyTinhLuong="";
                agentObjectService.getList_ThongTinTruong().then(function (result) {
                    $scope.thongTinTruongList = result.data;
                });
                $scope.thongTinTruong="";
                $scope.options = {
                    filter: "contains"
                }           
                 $scope.uploadOptions = {
                    async: {
                        saveUrl: "/ChamCongImportData/SaveFileToData",
                        autoUpload: true
                    },
                    upload: function (e) {
                        e.data = { kyTinhLuong: $scope.kyTinhLuong, thongTinTruong:  $scope.thongTinTruong };
                    },
                    success: onUploadSuccess,
                }

                 function onUploadSuccess(e) {
                     if (e.response=="1")
                     {
                         alert("Thành công!");
                     }
                     else
                     {
                         alert("Dữ liệu lỗi, vui lòng kiểm tra lại!");
                     }
                }
                $scope.numericOptions = {
                    format: "n0",
                    min: 0
                }
            }
    ]);
});