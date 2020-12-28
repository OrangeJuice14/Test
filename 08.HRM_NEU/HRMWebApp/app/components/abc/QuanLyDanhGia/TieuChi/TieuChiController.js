
define(['app/app',
    'app/directives/ckeditorDirectives',
    'app/components/abc/QuanLyDanhGia/TieuChi/TieuChiService',
    'app/components/abc/QuanLyDanhGia/BoTieuChi/BoTieuChiService',
    'app/components/abc/QuanLyDanhGia/DieuKienTieuChi/DieuKienTieuChiService',
    'app/components/abc/QuanLyDanhGia/XepLoaiDanhGia/XepLoaiDanhGiaController',
    'app/components/abc/QuanLyDanhGia/XepLoaiDanhGia/XepLoaiDanhGiaService'], function (app) {
        "use strict";

        app.controller('TieuChiController', ['$scope', '$rootScope', '$modal', '$stateParams', 'TieuChiService', 'BoTieuChiService', 'XepLoaiDanhGiaService',
            function ($scope, $rootScope, $modal, $stateParams, TieuChiService, BoTieuChiService, XepLoaiDanhGiaService) {
                $scope.ListXepLoai = {};
                BoTieuChiService.GetById($stateParams.BoTieuChiId).then(res => {
                    $scope.BoTieuChi = res.data;
                });

                $scope.GetListTieuChi = function () {
                    $scope.TieuChiList = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return TieuChiService.GetByBoTieuId($stateParams.BoTieuChiId).then(res => {
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
                        width: "50px",
                        title: "STT",
                        template: "<div style='float:right;text-align:center; width:20px; display:inline-block; color:black'>#:data.STT #</div>"
                    }, {
                        template: "<div style='color: black; ' ng-bind-html='\"#:data.NoiDung #\"'></div>",
                        title: "Nội dung",
                    }, {
                        template: "<div style='text-align:center; color:black'>{{(dataItem.DiemToiDa == 0 || dataItem.DiemToiDa == null) ? '' : dataItem.DiemToiDa}}</div>",
                        title: "Điểm tối đa",
                        width: "90px",
                    }, {
                        template: "<div style='margin:0px 25px;'><input type='checkbox' ng-model='dataItem.DiemTru' disabled='true'></div>",
                        title: "Điểm trừ",
                        width: "80px",
                    }, {
                        template: "<div style='width: 30px; vertical-align: top; margin-top:-5px'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success' style='padding: 0px 5px;'><i class='fa fa-pencil' style='margin-right: 0px'></i>  </button></div>",
                        title: "",
                        width: "60px",
                    }, {
                        template: "<div style='width: 30px; vertical-align: top; margin-top:-5px'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs' style='padding: 0px 5px'><i class='fa fa-times' style='margin-right: 0px'></i>  </button></div>",
                        title: "",
                        width: "60px",
                    }]
                }

                $scope.New = function () {
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
                                return null;
                            },
                        }
                    }).result.then(function () {
                        $scope.GetListTieuChi();
                    });
                }

                $scope.Edit = function (TieuChiId) {
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
                                return TieuChiId;
                            },
                        }
                    }).result.then(function () {
                        $scope.GetListTieuChi();
                    });
                }

                $scope.Delete = function (TieuChiId) {
                    TieuChiService.DeleteById(TieuChiId, $rootScope.session.UserId).then(res => {
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

                $scope.GetXepLoai = function () {
                    $scope.LoaddingXepLoai = true;
                    XepLoaiDanhGiaService.GetListXepLoaiDanhGiaByBoTieuChiId($stateParams.BoTieuChiId).then(res => {
                        $scope.ListXepLoai = res.data;
                        $scope.ListXepLoai.forEach(ObjXepLoai => {
                            ObjXepLoai.DieuKien = "";
                            if (ObjXepLoai.TuDiem != null)
                                ObjXepLoai.DieuKien += "Từ " + ObjXepLoai.TuDiem +" điểm ";
                            if (ObjXepLoai.DenDiem != null)
                                ObjXepLoai.DieuKien += "Đến " + ObjXepLoai.DenDiem + " điểm ";
                            //if()
                            if (ObjXepLoai.HasDieuKienTieuChi) {
                                XepLoaiDanhGiaService.GetDieuKienXepLoaiPhuByXepLoaiDanhGiaId(ObjXepLoai.Id).then(resDieuKienPhu => {
                                    ObjXepLoai.DieuKien += "và các tiêu chí thuộc " + resDieuKienPhu.data.TieuChiNoiDung + " đạt " + resDieuKienPhu.data.DiemDat + "% số điểm." ;
                                })
                            } else if (ObjXepLoai.HasDieuKienPhu) {
                                ObjXepLoai.DieuKien += "và đạt " + ObjXepLoai.DiemDat + "% số điểm tất cả các tiêu chí.";
                            }
                        })
                        $scope.LoaddingXepLoai = false;
                    })
                }
                $scope.GetXepLoai();
                $scope.XepLoaiDetails = function (xepLoaiDanhGiaId = null) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/XepLoaiDanhGia/XepLoaiDanhGia.html',
                        controller: 'XepLoaiDanhGiaController',
                        size: 'lg',
                        resolve: {
                            BoTieuChiId: function () {
                                return $scope.BoTieuChi.Id;
                            },
                            XepLoaiDanhGiaId: function () {
                                return xepLoaiDanhGiaId;
                            }
                        }
                    }).result.then(function () {
                        $scope.GetXepLoai();
                    });
                }
                $scope.DeleteXepLoai = function (xepLoaiDanhGiaId) {
                    XepLoaiDanhGiaService.Delete(xepLoaiDanhGiaId, $rootScope.session.UserId).then(res => {
                        if (res == 1) {
                            $scope.GetXepLoai();
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                        }
                        else

                            Notify('Thất bại!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                    })
                }
            }
        ]);

        app.controller('TieuChiDetailsController', ['$scope', '$rootScope', '$modal', '$stateParams', '$modalInstance', 'BoTieuChi', 'TieuChiId', 'TieuChiService', 'BoTieuChiService', 'DieuKienTieuChiService',
            function ($scope, $rootScope, $modal, $stateParams, $modalInstance, BoTieuChi, TieuChiId, TieuChiService, BoTieuChiService, DieuKienTieuChiService) {
                $scope.BoTieuChi = angular.copy(BoTieuChi);
                $scope.SelectDieuKienBoTieuChi = null;
                var DieuKienTieuChiEmpty = {
                    TieuChiId: angular.copy(TieuChiId),
                    DieuKienDiemTieuChiId: undefined,
                    DieuKienDiemBoTieuChiId: undefined
                }

                $scope.ListTieuChiOption = {
                    dataTextField: "NoiDung",
                    dataValueField: "Id",
                    placeholder: "Chọn tiêu chí...",
                    valuePrimitive: true,
                    autoBind: false,
                    filter: 'contains',
                    valueTemplate: `<div ng-bind-html="' #:data.NoiDung # '"></div> `,
                    template: `<div ng-bind-html="' #:data.NoiDung # '"></div> `,
                    change: function () {
                        this.refresh();
                    }
                }
                //var DieuKienTieuChiPromise = new Promise(function (resolve, reject) {
                $scope.ListBoTieuChiOption = {
                    dataTextField: "Name",
                    dataValueField: "Id",
                    placeholder: "Chọn bộ tiêu chí...",
                    valuePrimitive: true,
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return BoTieuChiService.GetAll().then(res => {

                                    options.success(res.data.filter(e => e.LoaiBoTieuChi >= $scope.BoTieuChi.LoaiBoTieuChi)); // vd như khi bộ tiêu chí hiện tại là tháng thì sẽ chỉ load ra những bộ tiêu chí tháng chứ không ra bộ tiêu chí quý hoặc là năm
                                    //resolve();
                                });
                            }
                        }
                    },
                    change: function () {

                        BoTieuChiService.GetById($scope.SelectDieuKienBoTieuChiId).then(res => {
                            $scope.BoTieuChiSelected = res.data;
                            //TieuChi.DieuKienThoiGian = BoTieuChi.LoaiBoTieuChi == 3 && BoTieuChiSelected.LoaiBoTieuChi == 3 ? 1 : ''
                        })

                        if ($scope.ListDieuKienTieuChi.length > 0 && $scope.ListDieuKienTieuChi[0].DieuKienDiemBoTieuChiId != $scope.SelectDieuKienBoTieuChiId) {
                            $scope.ListDieuKienTieuChi = [];
                            $scope.ListDieuKienTieuChi.push(angular.copy(DieuKienTieuChiEmpty));
                            $scope.ListDieuKienTieuChi[$scope.ListDieuKienTieuChi.length - 1].DieuKienDiemBoTieuChiId = $scope.SelectDieuKienBoTieuChiId;
                        }

                        $scope.TieuChiDataSource = new kendo.data.DataSource({
                            transport: {
                                read: function (options) {
                                    return TieuChiService.GetByBoTieuId($scope.SelectDieuKienBoTieuChiId).then(res => {
                                        res.data.forEach(item => {
                                            item.NoiDung = new DOMParser().parseFromString('<!doctype html><body>' + item.NoiDung, 'text/html').body.innerHTML;
                                        })
                                        options.success(res.data);
                                    });
                                }
                            }
                        });

                        this.refresh();
                    }
                }
                //});

                var ListTieuChiDataPromise = new Promise(function (resolve, reject) {

                    $scope.CbListTieuChiData = {
                        dataTextField: "NoiDung",
                        dataValueField: "Id",
                        placeholder: "Chọn tiêu chí...",
                        valuePrimitive: true,
                        autoBind: true,
                        valueTemplate: "<div ng-bind-html='\"#:data.NoiDung #\"'></div> ",
                        template: "<div ng-bind-html='\"#:data.NoiDung #\"'></div>  ",
                        dataSource: {
                            transport: {
                                read: function (options) {
                                    return TieuChiService.GetByBoTieuId($scope.BoTieuChi.Id).then(res => {
                                        res.data.forEach(item => {
                                            item.NoiDung = new DOMParser().parseFromString('<!doctype html><body>' + item.NoiDung, 'text/html').body.innerHTML;
                                        })
                                        options.success(res.data);
                                        resolve();
                                    });
                                }
                            }
                        }, change: function () {
                        }
                    }
                })
                //$scope.$watch('ListDieuKienTieuChi',function (n, o) {
                //    console.log(o == n);
                //    console.log(o);
                //    console.log(n);
                //})

                if (TieuChiId == null) {

                    $scope.TieuChi = {};
                    $scope.TieuChi.IsAutoScore = false;
                    $scope.TieuChi.BoTieuChiId = $scope.BoTieuChi.Id;
                    $scope.TieuChi.HeSoTieuChiCon = 100;

                } else {

                    ListTieuChiDataPromise.then(function () {

                        TieuChiService.GetById(TieuChiId).then(res => {
                            $scope.TieuChi = res.data;
                            if ($scope.TieuChi.ParentId != null) {
                                $scope.CheckedChil = true;
                            }
                            //$scope.getMaxDiem();

                            //DieuKienTieuChiPromise.then(function () {
                            DieuKienTieuChiService.GetByTieuChiId(TieuChiId).then(res => {

                                if (res.data.length != 0) {
                                    $scope.ListDieuKienTieuChi = res.data;
                                    $scope.SelectDieuKienBoTieuChiId = res.data[0].DieuKienDiemBoTieuChiId;
                                    BoTieuChiService.GetById($scope.SelectDieuKienBoTieuChiId).then(res => {
                                        $scope.BoTieuChiSelected = res.data;
                                    })
                                }
                                else {
                                    $scope.ListDieuKienTieuChi = [];
                                    $scope.ListDieuKienTieuChi.push(angular.copy(DieuKienTieuChiEmpty));
                                    $scope.ListDieuKienTieuChi[$scope.ListDieuKienTieuChi.length - 1].DieuKienDiemBoTieuChiId = $scope.SelectDieuKienBoTieuChiId;
                                }

                                $scope.TieuChiDataSource = new kendo.data.DataSource({
                                    transport: {
                                        read: function (options) {
                                            return TieuChiService.GetByBoTieuId($scope.SelectDieuKienBoTieuChiId).then(res => {
                                                res.data.forEach(item => {
                                                    item.NoiDung = new DOMParser().parseFromString('<!doctype html><body>' + item.NoiDung, 'text/html').body.innerHTML;
                                                })
                                                options.success(res.data);
                                            });
                                        }
                                    }
                                });
                            })
                            //})
                        })
                    })
                }


                $scope.save = function () {
                    if ($scope.TieuChi.STT == undefined || $scope.TieuChi.STT == null)
                        $scope.TieuChi.STT = "";
                    $scope.TieuChi.NoiDung = new DOMParser().parseFromString('<!doctype html><body>' + $scope.TieuChi.NoiDung, 'text/html').body.innerHTML.replace(/&nbsp;/g, " ");
                    $scope.TieuChi.AddUserId = $rootScope.session.UserId;
                    $scope.TieuChi.LastEditUserId = $rootScope.session.UserId;
                    if (!$scope.CheckedChil) {
                        $scope.TieuChi.ParentId = null;
                    }
                    TieuChiService.SaveOrUpdate($scope.TieuChi).then(res => {
                        if (res.data == 1) {
                            if($scope.TieuChi.IsAutoScore)
                                DieuKienTieuChiService.Save($scope.ListDieuKienTieuChi).then(resDieuKienTieuChi => { })

                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $modalInstance.close();
                        }
                    })
                }

                //$scope.getMaxDiem = function () {
                //    if ($scope.TieuChi.ParentId != null) {
                //        TieuChiService.GetById($scope.TieuChi.ParentId).then(res => {
                //            //$scope.MaxDiem = res.data.DiemToiDa;
                //            //$scope.TieuChi.DiemToiDa = res.data.DiemToiDa;
                //        })
                //    }
                //}

                $scope.CheckParent = function () {

                    if ($scope.CheckedChil != true) {
                        $scope.TieuChi.ParentId = null;
                        $scope.MaxDiem = undefined;
                    }

                }

                $scope.AddNewRow = function () {
                    $scope.ListDieuKienTieuChi.push(angular.copy(DieuKienTieuChiEmpty));
                }
                $scope.DelRow = function (itemDel) {
                    $scope.ListDieuKienTieuChi = $scope.ListDieuKienTieuChi.filter(e => e != itemDel);
                }
            }
        ]);
    });