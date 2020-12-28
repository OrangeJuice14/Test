
(function () {
    "use strict";

    var HRMWebAppModule = angular.module('HRMWebApp');

    HRMWebAppModule.controller('taskKPIController', ['$scope', '$modal', '$rootScope', 'taskHRMWebAppervice',
            function ($scope, $modal, $rootScope, taskHRMWebAppervice) {
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = null;

                $scope.obj = null;
                taskHRMWebAppervice.getList().then(function (result) {
                    $scope.resultList = result.data;
                    $scope.gridSettings =
                    {
                        width: '100%',
                        source: $scope.resultList,
                        columnsresize: true,
                        theme: 'energyblue',
                        columns: [
                             { text: 'Name', dataField: 'Name' },
                             { text: '', width: 40, cellsrenderer: editRender },
                             { text: '', width: 40, cellsrenderer: deleteRender }],
                        rowselect: function (event) {
                            var args = event.args;
                            $rootScope.$broadcast('TASK_SELECTION', args.row.Id);
                        }
                    };
                    $scope.createWidget = true;
                });

                var editRender = function (row, columnfield, value, defaulthtml, columnproperties) {
                    return "<button ng-click='Edit(value)' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button>";
                };
                var deleteRender = function (row, columnfield, value, defaulthtml, columnproperties) {
                    return "<button ng-click='Delete(value)' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button>";
                };


                $scope.New = function () {
                    $scope.isEdit = false;

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/taskKPI/detail.html',
                        controller: 'taskKPIDetailController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            }
                        }
                    }).result.finally(function (result) {
                        taskHRMWebAppervice.getList().then(function (result) {
                            $scope.resultList = result.data;
                        
                        });
                    });


                    //$scope.jqxWindowSettings.apply('open');
                };

                $scope.Edit = function () {
                    var selectedIndex = $scope.grid.getselectedrowindex();
                    if (selectedIndex == -1) {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var item = $scope.grid.getrowdata(selectedIndex);
                    $scope.obj = item;

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/taskKPI/detail.html',
                        controller: 'taskKPIDetailController',
                        resolve: {
                            id: function () {
                                return item.Id;
                            }
                        }
                    }).result.finally(function () {
                        taskHRMWebAppervice.getList().then(function (result) {
                            $scope.resultList = result.data;
                        });
                    });
                    //$scope.jqxWindowSettings.apply('open');
                }




                $scope.Delete = function () {
                    var selectedIndex = $scope.grid.getselectedrowindex();
                    if (selectedIndex == -1) {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                    if (!valid)
                        return;

                    var item = $scope.grid.getrowdata(selectedIndex);
                    $scope.obj = item;

                    taskHRMWebAppervice.Delete($scope.obj).then(function () {

                        taskHRMWebAppervice.getList().then(function (result) {
                            $scope.resultList = result.data;
                        });
                        $scope.jqxWindowSettings.apply('close');
                    });
                };


            }
    ]);

    HRMWebAppModule.controller('taskKPIDetailController', ['$scope', '$modalInstance', 'id', 'taskHRMWebAppervice',
        function ($scope, $modalInstance, id, taskHRMWebAppervice) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            //$scope.bicycleTypes = bicyclesService.getBicycleTypes();
            $scope.title = "Chi tiết công việc";
            $scope.obj = {};

            if ($scope.isNew) {

                $scope.obj = {
                    Id: MANAGER.GUID_EMPTY,
                    Name: "",
                };
            } else {

                taskHRMWebAppervice.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                  
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    taskHRMWebAppervice.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    taskHRMWebAppervice.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.close();
            };
        }
    ]);
})();