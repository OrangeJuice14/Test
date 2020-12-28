
define(['app/app', 'app/services/kpi/criterionService','app/services/kpi/professorCriterionService', 'app/services/kpi/targetGroupDetailService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService', 'app/controllers/kpi/criterionDictionaryController', ], function (app) {
    "use strict";
    //var HRMWebAppModule = angular.module('HRMWebApp');
    app.controller('criterionController', ['$scope', '$rootScope', '$modal', 'criterionService','professorCriterionService', 'targetGroupDetailService', 'departmentService', 'agentObjectService',
            function ($scope, $rootScope, $modal, criterionService, professorCriterionService, targetGroupDetailService, departmentService, agentObjectService) {
                //$scope.customOptions = {
                //    transport: {
                //        read: function (options) {
                //            return departmentService.getList().then(function (result) {
                //                options.success(result.data);
                //            });
                //        }
                //    }
                //}
                $scope.TargetGroupDetailId = MANAGER.GUID_EMPTY;
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = null;
                $scope.taskId = MANAGER.GUID_EMPTY;
                $scope.obj = {};

                //dropdownlist
                agentObjectService.getListProfessor().then(function (result) {
                    $scope.agentObjecs = result.data;
                    $scope.obj.AgentObject = $scope.agentObjecs[0].Id;
                    $scope.selectedChangeAgentObject();
                    //$scope.obj.AgentObject = result.data[0];
                });

                $scope.selectedChangeAgentObject = function () {
                    targetGroupDetailService.GetListbyAgentObjectId($scope.obj.AgentObject).then(function (result) {
                        $scope.targetGroupDetails = result.data;
                        $scope.targetGroupDetails.splice(0, 0, { Id: '00000000-0000-0000-0000-000000000000', Name: 'Nhóm tiêu chí (tất cả)' });
                        $scope.TargetGroupDetailId = $scope.targetGroupDetails[0].Id;
                        //$scope.obj.TargetGroupDetail = result.data[0];
                    });
                };

                departmentService.getList().then(function (result) {
                    $scope.departments = result.data;
                    $scope.obj.Department = $scope.departments[0].Id;
                    //$scope.obj.Department = result.data[0];
                });
                //end dropdownlist

                $scope.dataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return professorCriterionService.getList().then(function (result) {

                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });

                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        field: "Name",
                        title: "Nội dung"
                    },
                    {
                        field: "Record",
                        title: "Số điểm"
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

                $scope.Search = function () {
                    //var departmentId =typeof($scope.obj.Department.Id) != "undefined" ? $scope.obj.Department.Id : MANAGER.GUID_EMPTY;
                    //$scope.dataSource = new kendo.data.DataSource({
                    //    transport: {
                    //        read: function (options) {
                    //            return criterionService.Search($scope.obj.TargetGroupDetail, departmentId).then(function (result) {
                    //                options.success(result.data);
                    //            });
                    //        }
                    //    },
                    //    pageSize: 20
                    //});           
                    $scope.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                return professorCriterionService.Search($scope.TargetGroupDetailId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                };

                $scope.New = function () {
                    $scope.isEdit = false;
                    if ($scope.TargetGroupDetailId==MANAGER.GUID_EMPTY)
                    {
                        alert("Bạn chưa chọn nhóm mục tiêu");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/criterion/detail.html',
                        controller: 'criterionDetailController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            targetGroupDetailId: function () {
                                return $scope.TargetGroupDetailId;
                            },
                        }
                    }).result.then(function (result) {
                        $scope.grid.dataSource.read();
                    });
                };

                $scope.Edit = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/criterion/detail.html',
                        controller: 'criterionDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            targetGroupDetailId: function () {
                                return $scope.TargetGroupDetailId;
                            },
                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
                    });
                }

                //$scope.EditContent = function (Id) {
                //    if (Id == "") {
                //        alert("Bạn chưa chọn phần tử");
                //        return;
                //    }

                //    var modalInstance = $modal.open({
                //        animation: true,
                //        templateUrl: '/app/views/kpi/criterion/contentDictionaryManage.html',
                //        controller: 'criterionContentDictionaryController',
                //        resolve: {
                //            id: function () {
                //                return Id;
                //            }
                //        }
                //    }).result.finally(function () {
                //        $scope.grid.dataSource.read();
                //    });
                //}

                $scope.Delete = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    professorCriterionService.getCheckHasDictionary(Id).then(function (result) {
                        var check = result.data;
                        if (check==true)
                        {
                            alert("Tiêu chí này đã có từ điển con, xóa tiêu chí các từ điển con sẽ mất!");
                            return;
                        }
                        else
                        {
                            var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                            if (!valid)
                                return;
                            professorCriterionService.getObj(Id).then(function (result) {
                                professorCriterionService.Delete(result.data).then(function () {
                                    if (result.data == 0)
                                    {
                                        alert("Xóa thất bại!");
                                    }
                                    else if (result.data == 1)
                                    {
                                        alert("Xóa thành công!");
                                    }
                                    else if (result.data == 0)
                                    {
                                        alert("Tiêu chí đang được sử dụng!");
                                    }
                                    $scope.grid.dataSource.read();
                                });
                            });
                        }
                    });                   
                };
            }
    ]);

    app.controller('criterionDetailController', ['$scope', '$modal', '$modalInstance', 'id', 'targetGroupDetailId', 'professorCriterionService', 'targetGroupDetailService', 'departmentService', 'agentObjectService',
    function ($scope, $modal, $modalInstance, id, targetGroupDetailId, professorCriterionService, targetGroupDetailService, departmentService, agentObjectService) {
        $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
        $scope.options = {
            format: "n0",
            max: 100
        };
        $scope.criterionId = id;
        $scope.targetGroupDetailId = targetGroupDetailId;
        $scope.type = 0;
        $scope.obj = {};
        //agentObjectService.getList().then(function (result) {
        //    $scope.agentObjecs = result.data;
        //    if (criterionService.AgentObject == undefined)
        //        return;
        //    $scope.obj.AgentObject = criterionService.AgentObject;
        //    $scope.selectedChangeAgentObject();

        //});

        // selecting
        //$scope.selectedChangeAgentObject = function () {
        //    targetGroupDetailService.GetListbyAgentObjectId($scope.obj.AgentObject.Id).then(function (result) {
        //        $scope.targetGroupDetails = result.data;
        //        if (criterionService.TargetGroupDetail == undefined)
        //            return;
        //        $scope.obj.TargetGroupDetail = criterionService.TargetGroupDetail;
        //    });
        //};

        $scope.dataSource = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    return professorCriterionService.getDictionnaryByCriterionId($scope.criterionId).then(function (result) {
                        options.success(result.data);
                    });
                }
            },
            pageSize: 20
        });

        $scope.mainDictionaryGridOptions = {
            sortable: true,
            pageable: true,
            selectable: true,
            columns: [{
                field: "Name",
                title: "Nội dung"
            },
            {
                field: "Record",
                title: "Số điểm"
            },
            {
                template: "<div style='width: 30px;'><button ng-click='EditDictionary(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                width: "50px"
            },
            {
                template: "<div style='width: 30px;'><button ng-click='DeleteDictionary(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                width: "50px",
            }],
        };

        $scope.mainDictionaryGridOptions2 = {
            sortable: true,
            pageable: true,
            selectable: true,
            columns: [{
                field: "ManageCode",
                title: "Mã hoạt động"
            }, {
                field: "Name",
                title: "Tên hoạt động"
            },
            {
                field: "NumberOfHour",
                title: "Số giờ"
            },
            {
                template: "<div style='width: 30px;'><button ng-click='EditDictionary(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                width: "50px"
            },
            {
                template: "<div style='width: 30px;'><button ng-click='DeleteDictionary(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                width: "50px",
            }],
        };

        targetGroupDetailService.getTargetGroupDetailNameByCriterion($scope.criterionId).then(function (result) {
            $scope.type = result.data.TargetGroupDetailTypeId;
            $scope.TargetGroupDetailName = result.data.Name;
        });

        //targetGroupDetailService.getList().then(function (result) {
        //    $scope.targetGroupDetails = result.data;
        //    //$scope.obj.TargetGroupDetail = { Id: targetGroupDetailId };
        //    //$scope.obj.TargetGroupDetail = criterionService.TargetGroupDetail;
        //});

        professorCriterionService.getCriterionTypeList().then(function (result) {
            $scope.criterionTypes = result.data;
        });

        // end selecting
        departmentService.getList().then(function (result) {
            $scope.departments = result.data;
            //$scope.obj.Department = criterionService.Department;
        });

        $scope.title = "Chi tiết chỉ tiêu";
        if ($scope.isNew)
        {
            if($scope.type!=4)
            {
                $scope.obj =
                   {
                       Id: MANAGER.GUID_EMPTY,
                       Name: "",
                       TargetGroupDetail: { Id: targetGroupDetailId }
                   };

            }
            else
            {
                $scope.obj =
                   {
                       Id: MANAGER.GUID_EMPTY,
                       Name: "",
                       TargetGroupDetail: { Id: targetGroupDetailId },
                       CriterionType: { Id: 3 }
                   };
            }
           
        }
        else
        {
            professorCriterionService.getObj(id).then(function (result) {
                $scope.obj = result.data;
                $scope.obj.TargetGroupDetail.Id = result.data.TargetGroupDetailId;
                $scope.obj.CriterionType.Id = result.data.CriterionTypeId;
            });
        }

        $scope.save = function () {
            if ($scope.isNew) {
                professorCriterionService.Save($scope.obj).then(function () {
                    $modalInstance.close();
                });
            } else {
                professorCriterionService.Save($scope.obj).then(function () {
                    $modalInstance.close();
                });
            }
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

        //Dictionary:
        $scope.NewDictionary = function () {
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: '/app/views/kpi/criterion/dictionaryDetail.html',
                controller: 'criterionDictionaryController',
                resolve: {
                    id: function () {
                        return MANAGER.GUID_EMPTY;
                    },
                    criterionId: function () {
                        return $scope.criterionId;
                    },
                    type: function () {
                        return $scope.type;
                    }
                }
            }).result.then(function (result) {
                $scope.grid.dataSource.read();
            });
        }

        $scope.EditDictionary = function (dictionaryId) {
            if (dictionaryId == "") {
                alert("Bạn chưa chọn phần tử");
                return;
            }
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: '/app/views/kpi/criterion/dictionaryDetail.html',
                controller: 'criterionDictionaryController',
                resolve: {
                    id: function () {
                        return dictionaryId;
                    },
                    criterionId: function () {
                        return $scope.criterionId;
                    },
                    type: function () {
                        return $scope.type;
                    }
                }
            }).result.then(function () {
                $scope.grid.dataSource.read();
            });
        }

        $scope.DeleteDictionary = function (Id) {
            if (Id == "") {
                alert("Bạn chưa chọn phần tử");
                return;
            }
            var valid = window.confirm("Bạn có thật sự muốn xóa không?");
            if (!valid)
                
                return;
            professorCriterionService.getDictionary(Id).then(function (result) {
                professorCriterionService.DeleteDictionary(result.data).then(function () {
                    $scope.grid.dataSource.read();
                });
            });
        };
    }
    ]);
});