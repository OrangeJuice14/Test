
define(['app/app', 'app/services/kpi/criterionService'], function (app) {
    "use strict";

    app.controller('configurationController', ['$scope', '$modal', '$rootScope', 'criterionService',
            function ($scope, $modal, $rootScope, criterionService) {
                $scope.grid = {};       
                $scope.obj = null;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return criterionService.getListConfiguration().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: "row",
                    columns: [
                        {
                            field: "MaxBonusRecord",
                            title: "Điểm thưởng tối đa"
                        },
                        {
                            field: "TotalHourDefault",
                            title: "Số giờ định mức"
                        },
                        {
                            field: "ScienceResearchConfig",
                            title: "NCKH",
                            width: 80
                        },
                        {
                            field: "OtherActivityConfig",
                            title: "HĐK",
                            width: 80
                        },

                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    }],
                };            
                $scope.Edit = function (Id) {

                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/configuration/detail.html',
                        controller: 'configurationDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.finally(function () {
                        $scope.grid.dataSource.read();
                    });
                }
            }
    ]);
    app.controller('configurationDetailController', ['$scope', '$modalInstance', 'id', 'criterionService',
       function ($scope, $modalInstance, id,  criterionService) {
           $scope.obj = {};
                $scope.options = {
                    format: "n0",
                    min:0
                }
                criterionService.getConfigurationById(id).then(function (result) {
                   $scope.obj = result.data;
               });

           $scope.save = function () {
                   criterionService.SaveConfiguration($scope.obj).then(function (result) {
                       if (result == 0) {
                           alert("Lỗi!")
                       }
                       else {
                        alert("Thành công!")
                           $modalInstance.close();
                       }

                   });             
           };

           $scope.cancel = function () {
               $modalInstance.close();
           };
       }
    ]);
});