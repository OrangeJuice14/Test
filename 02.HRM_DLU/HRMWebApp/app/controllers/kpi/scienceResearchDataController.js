
define(['app/app', 'app/services/kpi/scienceResearchDataService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('scienceResearchDataController', ['$scope', '$modal', '$rootScope', 'scienceResearchDataService',
            function ($scope, $modal, $rootScope, scienceResearchDataService) {
                $scope.options = {
                    filter: "contains"
                }
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.scienceResearchDataName = '';
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.departmentId = MANAGER.GUID_EMPTY;
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return scienceResearchDataService.getList().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.obj = null;
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: {
                        buttonCount: 7
                    },
                    selectable: true,
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('scienceResearchDataSELECTION', this.dataItem(selectedRows[0]).Id);
                    },
                    columns: [{
                        field: "StaffCode",
                        title: "Mã nhân viên"
                    },
                    {
                        field: "Name",
                        title: "Hoạt động"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "50px",
                    }
                    ],
                };
                $scope.uploadOptions = {
                    async: {
                        saveUrl: "/ScienceResearchImportData/SaveFileToData",
                        autoUpload: true
                    },
                    success: onUploadSuccess
                }

                function onUploadSuccess(e) {
                    $scope.grid.dataSource.read();
                    $scope.grid.refresh();
                }
                $scope.numericOptions = {
                    format: "n0",
                    min: 0
                }

           
                $scope.Edit = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/scienceResearchData/detail.html',
                        controller: 'scienceResearchDataDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            }                           
                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
                        $scope.grid.refresh();
                    });
                };
                $scope.Search = function () {
                    $scope.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                var skip = options.data.skip;
                                var take = options.data.take;
                                var departmentId = $scope.departmentId;
                                var scienceResearchDataName = $scope.scienceResearchDataName;
                                return scienceResearchDataService.search(skip, take, departmentId, scienceResearchDataName).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20,
                        schema: {
                            data: "data", // records are returned in the "data" field of the response
                            total: "total" // total number of records is in the "total" field of the response
                        },
                        serverPaging: true // enable server paging
                    });
                };
                $scope.Delete = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                    if (!valid)
                        return;
                    scienceResearchDataService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        scienceResearchDataService.Delete($scope.obj).then(function () {
                            scienceResearchDataService.getList().then(function (result) {
                                $scope.grid.dataSource.read();
                            });
                        });
                    });
                };
            }
    ]);
    app.controller('scienceResearchDataDetailController', ['$scope', '$modalInstance', 'id', 'scienceResearchDataService', 'departmentService', 'agentObjectService',
        function ($scope, $modalInstance, id, scienceResearchDataService, departmentService, agentObjectService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Chi tiết nghiên cứu khoa học";
            $scope.obj = {};
            departmentService.getList().then(function (result) {
                $scope.departments = result.data;
            });
            $scope.agentObjects = {
                placeholder: "Chọn đối tượng...",
                dataTextField: "Name",
                dataValueField: "Id",
                valuePrimitive: true,
                autoBind: false,
                dataSource: {
                    transport: {
                        read: function (options) {
                            return agentObjectService.getList().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                }
            };
            $scope.numericOptions = {
                format: "n0",
                min: 0
            }
            if ($scope.isNew) {
                $scope.obj = {
                    Id: MANAGER.GUID_EMPTY,
                    Name: "",
                    UserName: "",
                    Password: "",
                    //PasswordConfirm:"",
                    Department: { Id: departmentId },
                    AgentObjects: {}
                };
            } else {
                scienceResearchDataService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                    $scope.obj.Department.Id = result.data.DepartmentId;
                    //$scope.obj.PasswordConfirm = $scope.obj.Password;
                });
            }
            $scope.save = function () {
                if ($scope.isNew) {
                    if ($scope.obj.Password != $scope.obj.PasswordConfirm) {
                        alert("Nhập lại mật khẩu không trùng với mật khẩu");
                        return;
                    }
                    scienceResearchDataService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    scienceResearchDataService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);

});