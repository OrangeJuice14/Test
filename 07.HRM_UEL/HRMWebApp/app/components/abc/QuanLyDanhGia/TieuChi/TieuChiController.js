
define(['app/app',
    'app/directives/ckeditorDirectives',
    'app/services/abc/ABC_TieuChiService',
    'app/services/abc/ABC_BoTieuChiService',], function (app) {
        "use strict";

        app.controller('TieuChiController', ['$scope', '$rootScope', '$modal', '$stateParams', 'ABC_TieuChiService', 'ABC_BoTieuChiService',
            function ($scope, $rootScope, $modal, $stateParams, ABC_TieuChiService, ABC_BoTieuChiService) {
                $scope.ListXepLoai = {};
                ABC_BoTieuChiService.GetById($stateParams.BoTieuChiId).then(res => {
                    $scope.BoTieuChi = res.data;
                });

                $scope.GetListTieuChi = function () {
                    $scope.TieuChiList = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return ABC_TieuChiService.GetByBoTieuChiId($stateParams.BoTieuChiId).then(res => {
                                    options.success(res.data);
                                });
                            }
                        },
                        schema: {
                            model: {
                                id: "Id",
                                fields: {
                                    parentId: { field: "ParentId", nullable: true },
                                    Id: { field: "Id", type: "string" }
                                },
                                expanded: true
                            }
                        },
                    })
                }

                $scope.GetListTieuChi();

                $scope.TieuChiOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        width: "70px",
                        title: "",
                    }, {
                        width: "70px",
                        title: "Chỉ mục",
                            template: "<div style='text-align:center;width: 100%; display:inline-block; color:black'>{{dataItem.ChiMuc == null ? '': dataItem.ChiMuc}}</div>"
                    }, {
                        template: "<div style='color: black;' ng-bind-html='\"#:data.NoiDung #\"'></div>",
                        title: "Nội dung",
                    }, {
                        template: "<div style='text-align:center; color:black'>{{(dataItem.DiemToiDa == 0 || dataItem.DiemToiDa == null) ? '' : dataItem.DiemToiDa}}</div>",
                        title: "Điểm tối đa",
                        width: "90px",
                    }, {
                        template: "<div style='width: 30px; vertical-align: top; margin-top:-5px'><button ng-click='Modify(\"#:data.Id #\")' class='btn btn-block btn-success' style='padding: 0px 5px;'><i class='fa fa-pencil'></i>  </button></div>",
                        title: "",
                        width: "60px",
                    }, {
                        template: "<div style='width: 30px; vertical-align: top; margin-top:-5px'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs' style='padding: 0px 5px'><i class='fa fa-times'></i>  </button></div>",
                        title: "",
                        width: "60px",
                    }]
                }

                $scope.Modify = function (tieuChiId) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/TieuChi/TieuChi.Details.html',
                        controller: 'TieuChiDetailsController',
                        size: 'lg',
                        resolve: {
                            BoTieuChi: function () {
                                return $scope.BoTieuChi;
                            },
                            TieuChiId: function () {
                                return tieuChiId;
                            }, ParentId: function () {
                                return null;
                            }
                        }
                    }).result.then(function () {
                        $scope.GetListTieuChi();
                    });
                }

                $scope.Delete = function (tieuChiId) {
                    if (confirm("Bạn có chắc chắn xóa?")) {
                        ABC_TieuChiService.DeleteById(tieuChiId, $rootScope.session.UserId).then(res => {
                            switch (res.data) {
                                case 1: {
                                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                    $scope.GetListTieuChi();
                                    break;
                                }
                                case 2: {
                                    Notify('Đã có người đánh bộ tiêu chí này!', 'top-right', '3000', 'warning', 'exclamation-circle', true);
                                    break;
                                }
                                default: {
                                    Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                                    break;
                                }
                            }
                        })
                    }
                }

                //$scope.GetXepLoai = function () {
                //    $scope.LoaddingXepLoai = true;
                //    XepLoaiDanhGiaService.GetListXepLoaiDanhGiaByBoTieuChiId($stateParams.BoTieuChiId).then(res => {
                //        $scope.ListXepLoai = res.data;
                //        $scope.ListXepLoai.forEach(ObjXepLoai => {
                //            ObjXepLoai.DieuKien = "";
                //            if (ObjXepLoai.TuDiem != null)
                //                ObjXepLoai.DieuKien += "Từ " + ObjXepLoai.TuDiem +" điểm ";
                //            if (ObjXepLoai.DenDiem != null)
                //                ObjXepLoai.DieuKien += "Đến " + ObjXepLoai.DenDiem + " điểm ";
                //            //if()
                //            if (ObjXepLoai.HasDieuKienTieuChi) {
                //                XepLoaiDanhGiaService.GetDieuKienXepLoaiPhuByXepLoaiDanhGiaId(ObjXepLoai.Id).then(resDieuKienPhu => {
                //                    ObjXepLoai.DieuKien += "và các tiêu chí thuộc " + resDieuKienPhu.data.TieuChiNoiDung + " đạt " + resDieuKienPhu.data.DiemDat + "% số điểm." ;
                //                })
                //            } else if (ObjXepLoai.HasDieuKienPhu) {
                //                ObjXepLoai.DieuKien += "và đạt " + ObjXepLoai.DiemDat + "% số điểm tất cả các tiêu chí.";
                //            }
                //        })
                //        $scope.LoaddingXepLoai = false;
                //    })
                //}
                //$scope.GetXepLoai();

                //$scope.XepLoaiDetails = function (xepLoaiDanhGiaId = null) {
                //    $modal.open({
                //        animation: true,
                //        templateUrl: '/app/components/abc/QuanLyDanhGia/XepLoaiDanhGia/XepLoaiDanhGia.html',
                //        controller: 'XepLoaiDanhGiaController',
                //        size: 'lg',
                //        resolve: {
                //            BoTieuChiId: function () {
                //                return $scope.BoTieuChi.Id;
                //            },
                //            XepLoaiDanhGiaId: function () {
                //                return xepLoaiDanhGiaId;
                //            }
                //        }
                //    }).result.then(function () {
                //        $scope.GetXepLoai();
                //    });
                //}

                //$scope.DeleteXepLoai = function (xepLoaiDanhGiaId) {
                //    XepLoaiDanhGiaService.Delete(xepLoaiDanhGiaId, $rootScope.session.UserId).then(res => {
                //        if (res == 1) {
                //            $scope.GetXepLoai();
                //            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                //        }
                //        else

                //            Notify('Thất bại!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                //    })
                //}
            }
        ]);

        app.controller('TieuChiDetailsController', ['$scope', '$rootScope', '$modal', '$stateParams', '$modalInstance', 'BoTieuChi', 'TieuChiId', 'ParentId', 'ABC_TieuChiService', 'ABC_BoTieuChiService',
            function ($scope, $rootScope, $modal, $stateParams, $modalInstance, BoTieuChi, TieuChiId, ParentId, ABC_TieuChiService, ABC_BoTieuChiService) {
                $scope.BoTieuChi = angular.copy(BoTieuChi);
                $scope.TieuChi = {};
                $scope.TieuChiId = angular.copy(TieuChiId);
                $scope.ShowErrors = false;
                $scope.Saving = false;
                $scope.GetListTieuChiChild = function () {
                    ABC_TieuChiService.GetByParentId($scope.TieuChiId).then(res => {
                        $scope.ListTieuChiChild = res.data;
                    });
                }
                if (TieuChiId == null) {
                    $scope.TieuChi = {};
                    $scope.TieuChi.BoTieuChiId = $scope.BoTieuChi.Id;
                    $scope.TieuChi.ParentId = ParentId;
                } else {
                    ABC_TieuChiService.GetById(TieuChiId).then(res => {
                        $scope.TieuChi = res.data;
                    });
                    $scope.GetListTieuChiChild();
                }


                $scope.save = function () {
                    $scope.Saving = true;
                    if ($scope.TieuChi.STT == undefined || $scope.TieuChi.STT == null)
                        $scope.TieuChi.STT = "";
                    $scope.TieuChi.NoiDung = new DOMParser().parseFromString('<!doctype html><body>' + $scope.TieuChi.NoiDung, 'text/html').body.innerHTML.replace(/&nbsp;/g, " ");
                    ABC_TieuChiService.SaveOrUpdate($scope.TieuChi.Id, $scope.TieuChi, $rootScope.session.UserId).then(res => {
                        if (res.data == 1) {

                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $modalInstance.close();
                        }
                        $scope.Saving = false;
                    })
                }

                $scope.Modify = function (tieuChiId) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/TieuChi/TieuChi.Details.html',
                        controller: 'TieuChiDetailsController',
                        size: 'lg',
                        resolve: {
                            BoTieuChi: function () {
                                return $scope.BoTieuChi;
                            },
                            TieuChiId: function () {
                                return tieuChiId;
                            }, ParentId: function () {
                                return $scope.TieuChi.Id;
                            }
                        }
                    }).result.then(function () {
                        $scope.GetListTieuChiChild();
                    });
                }

                $scope.cancel = function () {
                    $modalInstance.close();
                }

                $scope.$watch('TieuChi.ListDiem', function (newvalue, oldvalue) {
                    let patt = new RegExp('^([0-9]+([.][0-9]*)?[;])*$');
                    if (newvalue != null && !patt.test(newvalue)) {
                        $scope.ShowErrorsListDiem = true;
                    } else {
                        $scope.ShowErrorsListDiem = false;
                    }
                });

                $scope.Delete = function (tieuChiId) {
                    if (confirm("Bạn có chắc chắn xóa?")) {
                        ABC_TieuChiService.DeleteById(tieuChiId, $rootScope.session.UserId).then(res => {
                            switch (res.data) {
                                case 1: {
                                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                    $scope.GetListTieuChiChild();
                                    break;
                                }
                                case 2: {
                                    Notify('Đã có người đánh bộ tiêu chí này!', 'top-right', '3000', 'warning', 'exclamation-circle', true);
                                    break;
                                }
                                default: {
                                    Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                                    break;
                                }
                            }
                        })
                    }
                }
            }
        ]);
    });