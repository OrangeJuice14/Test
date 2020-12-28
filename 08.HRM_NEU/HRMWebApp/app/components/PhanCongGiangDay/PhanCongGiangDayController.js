define(['app/app',
    'app/directives/directives',
    'app/components/PhanCongGiangDay/PhanCongGiangDayService',
    'moment',
    'app/components/PMS/PMSService'],
    function (app) {
        "use strict";
        app.controller('PhanCongGiangDayController', ['$scope', '$rootScope', '$modal', 'PhanCongGiangDayService', 'PMSService',
            function ($scope, $rootScope, $modal, PhanCongGiangDayService, PMSService) {
                $scope.DanhSachPhanCongGiangDay = [];
                $scope.hocKyList = [];
                $scope.namHocList = [];
                $scope.ListBoPhanQuanLy = [];

                $scope.getHocKy = function () {
                    PMSService.GetHocKy($scope.namHocId).then(function (result) {
                        $scope.hocKyList = result.data;
                        if ($scope.hocKyList.length > 0) {
                            $scope.hocKyId = $scope.hocKyList[0].Oid;
                        }
                        else $scope.hocKyId = MANAGER.GUID_EMPTY;
                    })
                }
                $scope.LoadData = function () {
                    PhanCongGiangDayService.GetThoiKhoaBieu_DanhSachPhanCongGiangDay($scope.namHocId, $scope.hocKyId, $scope.BoPhanSelected).then(res => {
                        $scope.DanhSachPhanCongGiangDay = res.data
                    })
                }
                PMSService.GetNamHoc().then(function (result) {
                    $scope.namHocList = result.data;
                    if ($scope.namHocList.length > 0) {
                        var namHoc = $scope.namHocList.filter(q => moment(moment()).isSameOrAfter(q.NgayBatDau) && moment(moment()).isSameOrBefore(q.NgayKetThuc))[0];
                        $scope.namHocId = namHoc ? namHoc.Oid : $scope.namHocList[0].Oid;
                    }
                    $scope.getHocKy();
                })

                PhanCongGiangDayService.GetBoPhanQuanLy($rootScope.session.UserId).then(res => {
                    $scope.ListBoPhanQuanLy = res.data;
                    if ($scope.ListBoPhanQuanLy.length > 0) {
                        $scope.BoPhanSelected = $scope.ListBoPhanQuanLy[0].Id;
                    }
                })

                $scope.Add = function (BoMonPhanCong) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/PhanCongGiangDay/PhanCongGiangDay.Details.html',
                        controller: 'PhanCongGiangDayDetailsController',
                        size: 'xs',
                        resolve: {
                            BoMonPhanCong: function () {
                                return BoMonPhanCong;
                            },
                            BoMonId: function () {
                                return $scope.BoPhanSelected;
                            }
                        }
                    }).result.then(function () {
                        $scope.LoadData();
                    });
                }
            }
        ]);
        app.controller('PhanCongGiangDayDetailsController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'BoMonPhanCong', 'BoMonId', 'PhanCongGiangDayService', 'PMSService',
            function ($scope, $rootScope, $modal, $modalInstance, BoMonPhanCong, BoMonId, PhanCongGiangDayService, PMSService) {
                $scope.BoMonPhanCong = angular.copy(BoMonPhanCong);
                $scope.BoMonId = angular.copy(BoMonId);
                $scope.GiangVietSelected = null;
                $scope.ListGiangVienOption = {
                    dataTextField: "StaffInfoStaffProfileName",
                    dataValueField: "StaffInfoId",
                    valueTemplate: "<div ng-bind-html='\"#:data.UserName # : #:data.StaffInfoStaffProfileName #\"'></div> ",
                    template: "<div ng-bind-html='\"#:data.UserName # : #:data.StaffInfoStaffProfileName #\"'></div>  ",
                    placeholder: "Chọn giảng viên...",
                    valuePrimitive: true,
                    autoBind: true,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return PhanCongGiangDayService.GetListUserInBoMonId($scope.BoMonId).then(res => {
                                    options.success(res.data);
                                });
                            }
                        }
                    },
                    change: function () {
                        this.refresh();
                    }
                }
                $scope.Save = function () {
                    if ($scope.GiangVietSelected != null) {
                        PhanCongGiangDayService.PutPhanCongGiangDay($scope.BoMonPhanCong.OidChiTiet, $scope.GiangVietSelected).then(res => {
                            if (res > 0) {
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                $modalInstance.close();
                            } else {

                                Notify('Lỗi!', 'top-right', '3000', 'error', 'exclamation-circle', true);
                            }
                        })
                    } else {
                        Notify('Chọn giảng viên.', 'top-right', '3000', 'warning', 'exclamation-circle', true);
                    }
                }
            }
        ]);
    });