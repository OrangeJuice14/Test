
define(['app/app', 'app/services/kpi/planKPIService', 'app/services/kpi/agentObjectService', 'app/services/kpi/staffService','app/services/kpi/departmentService'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('userPlanKPIController', ['$scope', '$modal', '$rootScope', 'planKPIService', 'agentObjectService', 'staffService',
            function ($scope, $modal, $rootScope, planKPIService, agentObjectService, staffService) {
                //cbbx
                agentObjectService.getList().then(function (result) {
                    $scope.AgentObjects = result.data;
                    $scope.selectedChange();
                });




                planKPIService.getListByUserId().then(function (result) {
                    $scope.userPlanKPIList = result.data;
                });


                staffService.getCurrentStaff().then(function (result) {
                    agentObjectService.GetUserAgentObjectId(result.data.Id).then(function (result2) {
                        $scope.UserAgentObjectId = result2.data;
                    });
                });
                

                $scope.selectedChange = function () {
                    $scope.dataSource = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return planKPIService.getListByAngentObjectId(null).then(function (result) {
                                    options.success(result.data);
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
                        //pageSize: 20
                    });
                };
                //end cbbx                
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.obj = null;

                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{ field: "Name", title: "Nội dung", width: "150px" },
                    {
                        template: "<div style='text-align:center'><a ng-click='DepartmentPlan(\"#:data.Id #\")' href='javascript:void(0)'>Xem kế hoạch đơn vị</a></div>",
                        width: "100px",
                    }],
                };

               
                $scope.New = function () {
                    $scope.isEdit = false;
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/planKPI/newplan.html',
                        controller: 'newPlanKPIController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            }
                        }
                    }).result.then(function (result) {
                            $scope.grid.dataSource.read();
                    });
                    //$scope.jqxWindowSettings.apply('open');
                };

                $scope.Edit = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/planKPI/detail.html',
                        controller: 'planKPIDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            selectedId: function () {
                                var row = $scope.grid.select();
                                var data = $scope.grid.dataItem(row);
                                return data != null ? data.Id : null
                            },
                            selectedParentId: function () {
                                var row = $scope.grid.select();
                                var data = $scope.grid.dataItem(row);
                                return data != null ? data.parentId : null
                            }
                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
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
                    planKPIService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        planKPIService.Delete($scope.obj).then(function () {
                            $scope.grid.dataSource.read();
                        });
                    });
                };
                $scope.DepartmentPlan = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/planKPI/departmentPlan.html',
                        controller: 'planDepartmentController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            selectedId: function () {
                                var row = $scope.grid.select();
                                var data = $scope.grid.dataItem(row);
                                return data != null ? data.Id : null
                            },
                            selectedParentId: function () {
                                var row = $scope.grid.select();
                                var data = $scope.grid.dataItem(row);
                                return data != null ? data.parentId : null
                            }
                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
                    });
                };
                $scope.SchollManagePlan = function (planId) {                  
                    window.location = "/#/schoolManagePlankpidetail/" + planId + "/" + $scope.UserAgentObjectId+"//0";
                }
            }
    ]);

    app.controller('planKPIDetailController', ['$scope', '$modalInstance', 'id','selectedId','selectedParentId', 'planKPIService', 'agentObjectService','departmentService',
        function ($scope, $modalInstance, id, selectedId, selectedParentId, planKPIService, agentObjectService, departmentService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.ParentPlans = [];
            planKPIService.getParentPlanListById(id).then(function (result) {
                $scope.ParentPlans = result.data;
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

            $scope.title = "Chi tiết kế hoạch";
            $scope.obj = {};

            if ($scope.isNew) {
                $scope.obj = {
                    Id: MANAGER.GUID_EMPTY,
                    Name: "",
                    AgentObjects: {},
                    AgentObjectIds: [],
                    //SelectedId: selectedId,
                    //SelectedParentId: selectedParentId
                };                
            } else {
                planKPIService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                    //$scope.obj.AgentObject.Id = result.data.AgentObjectId;
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    planKPIService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    planKPIService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
    app.controller('newPlanKPIController', ['$scope', '$modalInstance', 'id','planKPIService', 'agentObjectService',
        function ($scope, $modalInstance, id, planKPIService, agentObjectService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Chi tiết kế hoạch";
            $scope.obj = {};

            if ($scope.isNew) {
                $scope.obj = {
                    Id: MANAGER.GUID_EMPTY,
                    Name: ""
                };
            } else {
                planKPIService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    planKPIService.SavePlan($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    planKPIService.SavePlan($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
    app.controller('planDepartmentController', ['$scope', '$modalInstance', 'id', 'selectedId', 'selectedParentId', 'planKPIService', 'agentObjectService', 'departmentService', 'staffService',
       function ($scope, $modalInstance, id, selectedId, selectedParentId, planKPIService, agentObjectService, departmentService, staffService) {
           $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
           $scope.planId = id;       
           $scope.title = "Chi tiết kế hoạch";
           $scope.obj = {};
           $scope.cancel = function () {
               $modalInstance.dismiss('cancel');
           };

           $scope.dataSource = new kendo.data.DataSource({
               transport: {
                   read: function (options) {
                       return departmentService.getMainDepartment().then(function (result) {
                           options.success(result.data);
                       });
                   }
               },
               group: { field: "DepartmentTypeName", dir: "desc" },
               sort: { field: "Name", dir: "asc" },
               pageSize: 15
           });
           var templateEdit = "<div ng-switch on='#:data.DepartmentType #'>";
           templateEdit += "<div ng-switch-when='1'><div style='text-align:center'><a ng-click='DepartmentManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>Xem kế hoạch</a></div></div>";
           templateEdit += "<div ng-switch-when='4'><div style='text-align:center'><a ng-click='FacultyManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>Xem kế hoạch</a></div></div>";
           templateEdit += "<div ng-switch-default><div style='text-align:center'><a ng-click='OtherManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>Xem kế hoạch</a></div></div></div>";
           $scope.DepartmentManagePlan = function (Id) {
               if (Id == '406f6e70-c3d9-49d8-b6f6-e39649ba8458')
               {
                       staffService.getCurrentStaff().then(function (result2) {
                           $scope.normalStaffId = result2.data.Id;
                           $modalInstance.close();
                           window.location = "/#/kpi/schoolManagePlankpidetail/" + $scope.planId + "/4731710F-94FB-4EF5-9F48-05E12648AA62/" + $scope.normalStaffId + "/1";
                       });                  
               }
               else
               {
                   staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
                   $scope.agentObjectId = result.data;
                   staffService.getDepartmentLeaderId(Id).then(function (result2) {
                       $scope.normalStaffId = result2.data;
                       $modalInstance.close();
                       window.location = "/#/kpi/plankpidetail/" + $scope.planId + "/" + $scope.agentObjectId + "/" + $scope.normalStaffId + "/1/0";
                   });
               });
               }              
           }
           $scope.FacultyManagePlan = function (Id) {
               staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
                   $scope.agentObjectId = result.data;
                   staffService.getDepartmentLeaderId(Id).then(function (result2) {
                       $scope.normalStaffId = result2.data;
                       $modalInstance.close();
                       window.location = "/#/kpi/facultyManagePlankpidetail/" + $scope.planId + "/" + $scope.agentObjectId + "/" + $scope.normalStaffId + "/1/0";
                   });
               });
           }
           $scope.OtherManagePlan = function (Id) {
           }
           $scope.mainGridOptions = {
               sortable: {
                   mode: "multiple",
                   allowUnsort: true
               },
               pageable: true,
               groupable: true,
               filterable: {
                   messages: {
                       info: "Lọc bởi : ",
                       filter: "Lọc",
                       clear: "Xóa"
                   },
                   extra: false,
                   operators: {
                       string: {
                           contains: "Từ khóa"
                       }
                   }
               },
               selectable: "row",
               columns: [{
                   field: "Name",
                   title: "Đơn vị"
               },
              {
                  field: "DepartmentTypeName",
                  title: "Loại đơn vị",
                  width: "25%"
              },
               {
                   template: templateEdit,
                   width: "20%"
               }]
           };
       }
    ]);
});