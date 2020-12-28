define(['app/app', 'app/directives/directives', 'app/services/abc/classificationService'], function (app) {
    "use strict";
    app.controller('classificationSetController', ['$scope', '$modal', '$rootScope', 'classificationService',
        function ($scope, $modal, $rootScope, classificationService) {
            $scope.dataSource = new kendo.data.DataSource({
                dataType: 'json',
                transport: {
                    read: function (options) {
                        return classificationService.getListClassificationSet().then(function (result) {
                            options.success(result.data);
                            $scope.list = result.data;
                            $scope.grid.select("tr:eq(1)");
                        });
                    }
                },
                pageSize: 20,
            });

            $scope.options = {
                sortable: true,
                pageable: {
                    buttonCount: 7
                },
                selectable: true,
                change: function (e) {
                    var selectedRows = this.select();
                    $rootScope.$broadcast('CLASSIFICATIONSETSELECTION', this.dataItem(selectedRows[0]).Id);
                },
                columns: [{
                    template: "<a ng-click='detail(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>",
                    //field: "Name",
                    title: "Tên"
                },
                {
                    template: "<format-date-time my-value='\"#:data.ThoiGianApDung#\"'></format-date-time>",
                    //field: "ThoiGianApDung",
                    title: "Thời gian áp dụng"
                },
                {
                    template: "<button href='javascript:void(0)' ng-click='delete(\"#:data.Id #\")' class=\"btn btn-block btn-danger btn-xs\"><i class=\"fa fa-times\"></i></button>",
                    width: 50
                }]
            };

            $scope.detail = function (item) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_CauHinh/ClassificationSetDetail.html',
                    controller: 'classificationSetDetailController',
                    resolve: {
                        obj: function () {
                            return angular.copy($scope.list[$scope.list.findIndex(q => q.Id == item)]);
                        }
                    }
                }).result.then(function () {
                    $scope.grid.dataSource.read();
                });
            }
            $scope.delete = function (id) {
                if (confirm("Bạn chắc chắn muốn xóa?")) {
                    var item = angular.copy($scope.list[$scope.list.findIndex(q => q.Id == id)]);
                    classificationService.DeleteClassificationSet(item).then(() => {
                        $scope.grid.dataSource.read();
                    }, (error) => {
                        alert("Thất bại!");
                    });
                }
            }
        }
    ]);
    app.controller('classificationsController', ['$scope', '$modal', '$rootScope', 'classificationService',
        function ($scope, $modal, $rootScope, classificationService) {
            $scope.ClassificationSetId = MANAGER.GUID_EMPTY;

            $scope.dataSource = new kendo.data.DataSource({
                dataType: 'json',
                transport: {
                    read: function (options) {
                        return classificationService.getClassificationBySet($scope.ClassificationSetId).then(function (result) {
                            options.success(result.data);
                            $scope.list = result.data;
                        });
                    }
                },
                pageSize: 20,
            });
            $scope.options = {
                sortable: true,
                pageable: {
                    buttonCount: 7
                },
                columns: [{
                    field: "MinRecord",
                    title: "Từ"
                }, {
                    field: "MaxRecord",
                    title: "Đến nhỏ hơn"
                }, {
                    field: "Rank",
                    title: "Xếp loại"
                }, {
                    template: "<input type='checkbox' ng-checked='#:data.IsEligible#' disabled='disabled' />",
                    title: "Có điều kiện"
                }, {
                    template: "<button href='javascript:void(0)' ng-click='detail(\"#:data.Id #\")' class=\"btn btn-block btn-success btn-xs\"><i class=\"fa fa-pencil\"></i></button>",
                    width: 55
                }, {
                    template: "<button href='javascript:void(0)' ng-click='delete(\"#:data.Id #\")' class=\"btn btn-block btn-danger btn-xs\"><i class=\"fa fa-times\"></i></button>",
                    width: 55
                }]
            };
            $scope.$on("CLASSIFICATIONSETSELECTION", function (event, args) {
                $scope.ClassificationSetId = args;
                $scope.grid.dataSource.read();
            });
            $scope.detail = function (item) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/abc/ABC_CauHinh/ClassificationsDetail.html',
                    controller: 'classificationsDetailController',
                    resolve: {
                        obj: function () {
                            return angular.copy($scope.list[$scope.list.findIndex(q => q.Id == item)]);
                        },
                        setId: function() {
                            return $scope.ClassificationSetId;
                        }
                    }
                }).result.then(function () {
                    $scope.grid.dataSource.read();
                });
            }
            $scope.delete = function (id) {
                if (confirm("Bạn chắc chắn muốn xóa?")) {
                    var item = angular.copy($scope.list[$scope.list.findIndex(q => q.Id == id)]);
                    classificationService.DeleteClassification(item).then(() => {
                        $scope.grid.dataSource.read();
                    }, (error) => {
                        alert("Thất bại!");
                    });
                }
            }
        }
    ]);
    app.controller('classificationSetDetailController', ['$scope', '$modalInstance', 'obj', 'classificationService',
        function ($scope, $modalInstance, obj, classificationService) {
            $scope.isNew = obj == null ? true : false;
            $scope.obj = $scope.isNew ? {} : obj;
            $scope.save = function () {
                classificationService.PutClassificationSet($scope.obj).then(function (result) {
                    $modalInstance.close();
                }, function (error) {
                    alert("Thất bại!");
                });
            };
        }
    ]);
    app.controller('classificationsDetailController', ['$scope', '$modalInstance', 'obj', 'setId', 'classificationService',
        function ($scope, $modalInstance, obj, setId, classificationService) {
            $scope.isNew = obj == null ? true : false;
            if ($scope.isNew) {
                $scope.obj = {};
                $scope.obj.ABC_ClassificationSetId = setId;
            } else {
                $scope.obj = obj;
            }
            $scope.save = function () {
                classificationService.PutClassification($scope.obj).then(function (result) {
                    $modalInstance.close();
                }, function (error) {
                    alert("Thất bại!");
                });
            };
            $scope.orders = {
                format: "n0",
                min: 0
            }
        }
    ]);
});