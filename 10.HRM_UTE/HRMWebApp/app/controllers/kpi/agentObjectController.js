
define(['app/app', 'app/services/kpi/agentObjectService', 'app/services/kpi/targetGroupService'], function (app) {
    "use strict";

    app.controller('agentObjectController', ['$scope', '$modal', '$rootScope', 'agentObjectService','targetGroupService',
            function ($scope, $modal, $rootScope, agentObjectService, targetGroupService) {
                $("#sidebar").addClass("menu-compact");

                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.targetGroupDetailId = MANAGER.GUID_EMPTY;
                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return agentObjectService.getListByClassId($scope.targetGroupDetailId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.obj = null;
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: "row",
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('AGENTOBJECTSELECTION', this.dataItem(selectedRows[0]).Id);
                    },
                    columns: [{
                        field: "Name",
                        title: "Nội dung"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "50px",
                    }],
                };
                $scope.$on("TARGETGROUPDETAILSELECTION", function (event, args) {
                    $scope.targetGroupDetailId = args;
                    $scope.grid.dataSource.read();
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
                        templateUrl: '/app/views/kpi/agentObject/detail.html',
                        controller: 'agentObjectDetailController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            }
                        }
                    }).result.then(function (result) {
                            $scope.grid.dataSource.read();
                    });
                };

                $scope.Show = function () {
                    $scope.targetGroupDetailId = MANAGER.GUID_EMPTY;
                    $scope.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                return agentObjectService.getListByClassId($scope.targetGroupDetailId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                };

                $scope.Edit = function (Id) {
                    //var selectedItem = $scope.grid.dataItem($scope.grid.select());

                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/agentObject/detail.html',
                        controller: 'agentObjectDetailController',
                        windowClass: 'agentObjectDetail-dialog',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.then(function () {
                            $scope.grid.dataSource.read();
                    });
                }

                $scope.Delete = function (Id) {

                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                    if (!valid)
                        return;

                    agentObjectService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        agentObjectService.Delete($scope.obj).then(function () {
                                $scope.grid.dataSource.read();
                        });
                    });
                };
            }
    ]);

    app.controller('agentObjectDetailController', ['$scope', '$state', '$stateParams', '$modal', '$modalInstance', 'id', 'agentObjectService', 'targetGroupDetailService',
        function ($scope, $state, $stateParams, $modal, $modalInstance, id, agentObjectService, targetGroupDetailService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Chi tiết đối tượng";
            $scope.obj = {};
            $scope.workingModeLists = [];
            $scope.options = {
                format: "#.## tiết"
            }
            $scope.otherActivityOptions = {
                format: "#.## giờ"
            }
            $scope.densityOptions = {
                format: "#.## \\%",
                max: 100,
                min: 0
            }
            $scope.TTOptions = {
                format: "#.## %",
                max: 100,
                min: 0
            }
            agentObjectService.GetListAgentObjectType().then(function (result) {
                $scope.agentObjectTypes = result.data;
            });
            $scope.targetGroupDetails = {
                placeholder: "Chọn nhóm mục tiêu...",
                dataTextField: "Name",
                dataValueField: "Id",
                valuePrimitive: true,
                autoBind: false,
                dataSource: {
                    transport: {
                        read: function (options) {
                            return targetGroupDetailService.getList().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                }
            };

            

            if ($scope.isNew) {
                $scope.obj = {
                    Id: MANAGER.GUID_EMPTY,
                    Name: "",
                    AgentObjectDetails: []
                };
            } else {
                agentObjectService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }

            var tabStrip = $("#mainAgentObjectTabId").kendoTabStrip().data("kendoTabStrip");

            agentObjectService.getWorkingModeListForAdding(id).then(function (result) {
                $scope.workingModeLists = result.data;
            });

            $scope.NewWorkingMode = function () {
                //trường hợp agentObject chưa có, nên phải tạo trước khi thêm chế độ làm việc
                agentObjectService.Save($scope.obj).then(function (result) {
                    $scope.obj.Id = result.Id;

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/agentObject/workingModeDetail.html',
                        controller: 'workingModeDetailController',
                        windowClass: 'workingModeDetail-dialog',
                        size: 'sm',
                        resolve: {
                            agentObjectId: function () {
                                return $scope.obj.Id;
                            }
                        }
                    }).result.then(function (result) {
                        //agentObjectService.getObj(id).then(function (result) {
                        //    //$scope.obj = result.data;
                        //    var tabStrip = $("#mainAgentObjectTabId").kendoTabStrip().data("kendoTabStrip");
                        //    tabStrip.reload("li:first");
                        //});
                        //$state.reload();
                        //$scope.obj.AgentObjectDetails.push(result);
                        var tabStrip = $("#mainAgentObjectTabId").kendoTabStrip().data("kendoTabStrip");
                        tabStrip.append({ text: result.WorkingModeName, content: "Đã thêm. Vui lòng mở lại popup để load dữ liệu!" });
                        tabStrip.select((tabStrip.tabGroup.children("li").length - 1));
                    });
                });
            };

            $scope.save = function () {
                var flag = "";
                $.each($scope.obj.AgentObjectDetails, function (idx, item) {
                    if (item.NumberOfSectionDensity + item.ScienceResearchDensity + item.OtherActivityDensity != 100 && flag == "") {
                        flag = "Tổng trọng số của " + item.WorkingModeName + " phải bằng 100!";
                    }
                });
                if (flag != "") {
                    alert(flag);
                    return;
                }
                if ($scope.isNew) {
                    agentObjectService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    agentObjectService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                //$modalInstance.dismiss('cancel');
                $modalInstance.close();
            };
        }
    ]);

    app.controller('workingModeDetailController', ['$scope', '$modalInstance', 'agentObjectId', 'agentObjectService', 'targetGroupDetailService',
        function ($scope, $modalInstance, agentObjectId, agentObjectService, targetGroupDetailService) {
            $scope.obj = {
                Id: MANAGER.GUID_EMPTY,
                AgentObjectId: agentObjectId,
                NumberOfSection: 0,
                ScienceResearch: 0,
                OtherActivity: 0,
                NumberOfSectionDensity: 0,
                ScienceResearchDensity: 0,
                OtherActivityDensity: 0
            };
            $scope.options = {
                format: "#.## tiết"
            }
            $scope.otherActivityOptions = {
                format: "#.## giờ"
            }
            $scope.densityOptions = {
                format: "#.## \\%",
                max: 100,
                min: 0
            }
            agentObjectService.getWorkingModeListForAdding(agentObjectId).then(function (result) {
                $scope.workingModeLists = result.data;
                if (result.data.length > 0)
                    $scope.obj.WorkingModeId = result.data[0].Id;
            });

            $scope.save = function () {
                if ($scope.obj.NumberOfSectionDensity + $scope.obj.ScienceResearchDensity + $scope.obj.OtherActivityDensity != 100) {
                    alert("Tổng trọng số phải bằng 100!");
                    return;
                }
                agentObjectService.SaveWorkingModeDetail($scope.obj).then(function (result) {
                    $modalInstance.close(result);
                });
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);

    app.controller('agentObject_TytrongController', ['$scope', '$modal', '$rootScope', 'agentObjectService', 'targetGroupService',
        function ($scope, $modal, $rootScope, agentObjectService, targetGroupDetailService) {
            $scope.createWidget = false;
            $scope.grid = {};
            $scope.isEdit = false;
            $scope.resultList = [];
            $scope.targetGroupDetailId = MANAGER.GUID_EMPTY;
            $scope.dataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        return agentObjectService.getListagentObject().then(function (result) {
                            options.success(result.data);
                        });
                    }
                },
                pageSize: 20
            });
            $scope.obj = null;
            $scope.mainGridOptions = {
                sortable: true,
                pageable: true,
                selectable: "row",
                //change: function (e) {
                //    var selectedRows = this.select();
                //    $rootScope.$broadcast('AGENTOBJECTSELECTION', this.dataItem(selectedRows[0]).Id);
                //},
                columns: [{
                    field: "Name",
                    title: "Đối tượng - Nhóm mục tiêu"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                    width: "50px"
                },
                //{
                //    template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                //    width: "50px",
                //}
                ],
            };

              $scope.Edit = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/agent_TargetManage/detail.html',
                        controller: 'agentObject_TytrongDetailController',
                        windowClass: 'agentObjectDetail-dialog',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.then(function () {
                            $scope.grid.dataSource.read();
                    });
                }
        }
    ]);

    app.controller('agentObject_TytrongDetailController', ['$scope', '$state', '$stateParams', '$modal', '$modalInstance', 'id', 'agentObjectService', 'targetGroupDetailService',
       function ($scope, $state, $stateParams, $modal, $modalInstance, id, agentObjectService, targetGroupDetailService) {
           $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
           $scope.title = "Chi tiết đối tượng";
           $scope.obj = {};
           agentObjectService.GetListAgentObjectType().then(function (result) {
               $scope.agentObjectTypes = result.data;
           });

           targetGroupDetailService.getListTargetGroupDetailType().then(function (result) {
               $scope.targetGroupDetails = result.data;
           });

           agentObjectService.getagent_targetGroupObj(id).then(function (result) {
               $scope.obj = result.data;
           });

           $scope.save = function () {
               agentObjectService.Saveagent_targetGroup($scope.obj).then(function () {
                   $modalInstance.close();
               });
           }
            $scope.cancel = function () {
                //$modalInstance.dismiss('cancel');
                $modalInstance.close();
            };
       }
    ]);
});