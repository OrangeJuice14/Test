
define(['app/app',
    'app/services/abc/ABC_GroupDanhGiaService',],
    function (app) {
        "use strict";

        app.controller('GroupDanhGiaController', ['$scope', '$rootScope', '$modal', 'ABC_GroupDanhGiaService',
            function ($scope, $rootScope, $modal, ABC_GroupDanhGiaService) {
                function GetData() {
                    $scope.NhomDanhGiaData = {
                        type: "odata",
                        transport: {
                            read: function (options) {
                                return ABC_GroupDanhGiaService.GetAll().then(res => {
                                    options.success(res.data);
                                    //$scope.$apply();
                                })
                            }
                        },
                        schema: {
                            data: function (data) {
                                return data;
                            },
                            total: function (data) {
                                return data['odata.count'];
                            },
                        },
                        //pageSize: 20,
                    }
                }
                GetData();

                $scope.NhomDanhGiaOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        width: " 120px",
                        title: "Thứ tự cấp bậc",
                        template: "<div style='text-align:center; width:80px; display:inline-block;'>{{dataItem.STT}}</div>"
                    }, {
                        width: "",
                        title: "Tên nhóm",
                        template: "<div style='text-align:center; width:auto; display:inline-block;'>{{dataItem.Name}}</div>"
                    }, {
                        width: "70px",
                        title: "",
                        template: "<div style='width: 30px;'><button ng-click='Details(dataItem)' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>"
                    }, {
                        width: "70px",
                        title: "",
                        template: "<div style='width: 30px;'><button ng-click='Delete(dataItem)' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>"
                    },]
                }

                $scope.New = function () {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/GroupDanhGia/GroupDanhGia.Details.html',
                        controller: 'GroupDanhGiaDetailsController',
                        size: 'xs',
                        resolve: {
                            GroupDanhGia: function () {
                                return {};
                            },
                        }
                    }).result.then(function () {
                        GetData();
                    });
                }

                $scope.Details = function (obj) {
                    $modal.open({
                        animation: true,
                        templateUrl: '/app/components/abc/QuanLyDanhGia/GroupDanhGia/GroupDanhGia.Details.html',
                        controller: 'GroupDanhGiaDetailsController',
                        size: 'xs',
                        resolve: {
                            GroupDanhGia: function () {
                                return obj;
                            },
                        }
                    }).result.then(function () {
                        GetData();
                    });
                }
                $scope.Delete = function (obj) {
                    if (confirm("Bạn có chắc chắn muốn xóa không ?")) {
                        ABC_GroupDanhGiaService.Delete(obj, $rootScope.session.UserId).then(res => {
                            if (res == 1) {
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                GetData();
                            }
                        })
                    }
                }
            }
        ]);

        app.controller('GroupDanhGiaDetailsController', ['$scope', '$rootScope', '$modal', '$modalInstance', 'GroupDanhGia', 'ABC_GroupDanhGiaService',
            function ($scope, $rootScope, $modal, $modalInstance, GroupDanhGia, ABC_GroupDanhGiaService) {
                $scope.GroupDanhGia = angular.copy(GroupDanhGia);
                $scope.Save = function () {
                    ABC_GroupDanhGiaService.SaveOrUpdate($scope.GroupDanhGia.Id, $scope.GroupDanhGia, $rootScope.session.UserId).then(res => {
                        if (res == 1) {
                            Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                            $modalInstance.close();
                        }
                    })
                }
            }
        ]);
    });