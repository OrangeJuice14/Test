
define(['app/app', 'app/services/kpi/StaffService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService', 'app/services/kpi/ratingKPIService'], function (app) {
    "use strict";
    
    //var HRMWebAppModule = angular.module('HRMWebApp');
    
    app.controller('staffController', ['$scope', '$modal', '$rootScope', '$state', '$stateParams', 'staffService', 'departmentService','ratingKPIService',
            function ($scope, $modal, $rootScope,$state, $stateParams, staffService,departmentService,ratingKPIService) {
                $scope.options = {
                    filter: "contains"
                }
                $scope.unlockRating = {};
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.staffName = '';
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.departmentId = MANAGER.GUID_EMPTY;
                departmentService.getList().then(function (result) {
                    $scope.departments = result.data;
                });
                $scope.departmentsDataSource = {
                    dataTextField: "Name",
                    dataValueField: "Id",
                    valuePrimitive: true,
                    autoBind: false,
                    filter: "contains",
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return departmentService.getList().then(function (result) {
                                    options.success(result.data);
                                });
                            },
                        }
                    }
                };
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            var skip = options.data.skip;
                            var take = options.data.take;
                            var departmentId = $scope.departmentId;
                            return staffService.getListPaging(skip, take, departmentId).then(function (result) {
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
                 $scope.dataSourceAll = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return staffService.getListAll().then(function (result) {
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
                        $rootScope.$broadcast('staffSELECTION', this.dataItem(selectedRows[0]).Id);
                    },
                    columns: [{
                        field: "Name",
                        title: "Nhân viên"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    },
                    ],
                };
                $scope.unlockRatingOptions = {
                    sortable: true,
                    pageable: {
                        buttonCount: 7
                    },
                    selectable: true,
                    columns: [
                        {
                            template: "<input type='checkbox' class='checkbox' ng-click='onClick($event,dataItem)' />",
                            width: "32px"
                        },
                        {
                            field: "Name",
                            title: "Cá nhân"
                        }
                    ]
                };
                $scope.$on("UNLOCKDEPARTMENT", function (event, args) {
                    $scope.departmentId = args;
                    $scope.grid.dataSource.read();
                });
                $scope.arr = [];
                $scope.UnlockRating = function () {
                    $scope.unlockRating.PlanKPIId = $stateParams.planId;
                    $scope.unlockRating.DepartmentId = $scope.departmentId;
                    $scope.unlockRating.StaffIds = [];
                    for (var i in $scope.arr) {
                        if ($scope.arr[i]) {
                            $scope.unlockRating.StaffIds.push(i);
                        }
                    }
                    ratingKPIService.SaveUnlockRating($scope.unlockRating).then(function (result) {
                        if (result == 1) {
                            alert("Mở khóa thành công!");
                        }
                        else if (result == 2) {
                            alert("Chưa chọn thời gian!");
                        }
                        else if (result == 0) {
                            alert("Lỗi!");
                        }
                    });
                };
                $scope.onClick = function (e, dataItem) {
                    var element = $(e.currentTarget);
                    var checked = element.is(':checked');
                    $scope.arr[dataItem.Id] = checked;
                };
                $scope.mainGridOptionsAll = {
                    sortable: true,
                    pageable: {
                        buttonCount: 7
                    },
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
                    columns: [
                        {
                            field: "ManageCode",
                            title: "Mã quản lý",
                            width: "15%"
                        },
                        {
                        field: "Name",
                        title: "Nhân viên"
                        },
                        {
                            field: "DepartmentName",
                            title: "Đơn vị"
                        },
                    {
                        template: "<div style='width: 30px;'><button ng-click='EditStaffRecord(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    },
                    ],
                };
                $scope.$on("DEPARTMENTSELECTION", function (event, args) {
                    $scope.departmentId = args;
                    $scope.grid.dataSource.read();
                    staffService.getAutoComplete($scope.departmentId).then(function (result) {
                        $scope.staffNames = result.data;
                    });
                    var staffSource = staffService.getAutoComplete($scope.departmentId).then(function (result) {
                        $scope.staffNames = result.data;
                    });
                    $scope.staffNames = {
                        dataTextField: 'Name',
                        dataType: 'json',
                        dataSource: new kendo.data.DataSource({
                            transport: {
                                read: function (options) {
                                    var departmentId = $scope.departmentId;
                                    return staffService.getAutoComplete(departmentId).then(function (result) {
                                        options.success(result.data);
                                    });
                                }
                            }
                        })
                    }
                });
                $scope.Edit = function(Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/staff/detail.html',
                        controller: 'staffDetailController',
                        resolve: {
                            id: function() {
                                return Id;
                            },
                            departmentId: function () {
                                return MANAGER.GUID_EMPTY;
                            }
                        }
                    }).result.then(function() {
                        $scope.grid.dataSource.read();
                    });
                };
                $scope.EditStaffRecord = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/editRecord/detail.html',
                        controller: 'planStaffDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            }
                        }
                    }).result.then(function () {
                    });
                };
                $scope.Search = function () {
                    $scope.dataSourceAll = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                var skip = options.data.skip;
                                var take = options.data.take;
                                var departmentId = $scope.departmentId;
                                var staffName = $scope.staffName;
                                return staffService.search(skip, take, departmentId, staffName).then(function (result) {
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
                $scope.SearchDept = function () {
                    $scope.dataSourceAll = new kendo.data.DataSource({
                        dataType: 'json',
                        transport: {
                            read: function (options) {
                                var deptId = $scope.deptId;
                                return staffService.searchDept(deptId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
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
                    staffService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        staffService.Delete($scope.obj).then(function () {
                            staffService.getList().then(function (result) {
                                $scope.grid.dataSource.read();
                            });
                        });
                    });
                };
            }
    ]);
    app.controller('staffDetailController', ['$scope', '$modalInstance', 'id', 'departmentId', 'staffService', 'departmentService', 'agentObjectService',
        function ($scope, $modalInstance, id, departmentId, staffService, departmentService, agentObjectService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Chi tiết đối tượng";
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
                staffService.getObj(id).then(function (result) {
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
                    staffService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    staffService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
    app.controller('planStaffDetailController', ['$scope', '$modalInstance','$modal', 'id', 'staffService', 'departmentService', 'agentObjectService','ratingKPIService',
        function ($scope, $modalInstance,$modal, id, staffService, departmentService, agentObjectService, ratingKPIService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
        if ($scope.isNew==false) 
        {
            staffService.getAgentObjectTypeIdByStaffId(id).then(function (result) {
                $scope.agentObjectTypeId = result.data;
            });
            staffService.getAgentObjectIdByStaffId(id).then(function (result) {
                $scope.agentObjectId = result.data;
            });
        } 
            $scope.dataSource = new kendo.data.DataSource({
                dataType: 'json',
                transport: {
                    read: function (options) {
                        return ratingKPIService.getListByStaffId(id).then(function (result) {
                            options.success(result.data);
                        });
                    }
                },
                pageSize: 10
            });
            $scope.mainGridOptions = {
                sortable: true,
                pageable: true,
                columns: [{
                    title: "Tên kế hoạch",
                    template: "<a ng-click='Edit(\"#:data.Id #\")'>#:data.PlanName #</a>",
                    width: "40%"
                },
                {
                    field: "TotalRecord",
                    title: "Số điểm",
                    width: "20%"
                },
                {
                    field: "TotalRecordSecond",
                    title: "Điểm chỉnh sửa",
                    width: "20%"
                },
                {
                    field: "NumberOfEditing",
                    title: "Số lần sửa",
                    width: "20%"
                }
                ],
            };
            $scope.Edit = function (Id) {
            $scope.resultId=Id;
            ratingKPIService.getStaffIdByResultId(Id).then(function (result) {
                $scope.staffId = result.data;
                ratingKPIService.getPlanIdByResultId($scope.resultId).then(function (result) {
                    $scope.planId = result.data;
                    switch ($scope.agentObjectTypeId) {
                        case 1:
                            window.location = "/#/professorRatingKPI/" + $scope.planId + "/"+$scope.agentObjectId+"/"+$scope.staffId+"//1";
                            break;
                        case 2:
                            window.location = "/#/ratingKPI/" + $scope.planId + "/"+$scope.agentObjectId+"/"+$scope.staffId+"//1";
                            break;
                    }
                });    
            });
                

            $modalInstance.dismiss('cancel');
                //if (Id == "") {
                //    alert("Bạn chưa chọn phần tử");
                //    return;
                //}
                //var modalInstance = $modal.open({
                //    animation: true,
                //    templateUrl: '/app/views/kpi/editRecord/editRecord.html',
                //    controller: 'editRecordDetailController',
                //    resolve: {
                //        id: function () {
                //            return Id;
                //        }
                //    }
                //}).result.then(function () {
                //    $scope.grid.dataSource.read();
                //});
            };
            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
    app.controller('editRecordDetailController', ['$scope', '$modalInstance', 'id','ratingKPIService' ,
    function ($scope, $modalInstance, id, ratingKPIService) {
        $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
        $scope.obj = {};
        if ($scope.isNew==false) 
        {
            ratingKPIService.getResult(id).then(function (result) {
                $scope.obj = result.data;
            });
        } 
        $scope.save = function () {
            if ($scope.isNew==false) 
            {
                ratingKPIService.saveEditRecord($scope.obj).then(function (result) {
                    if (result == 1)
                    {
                        alert("Chỉnh sửa thành công!");
                        $modalInstance.close();
                    }
                    else if (result==2)
                    {
                        alert("Không thể chỉnh sửa điểm của kế hoạch này!");
                    }
                    else if (result==0)
                    {
                        alert("Lỗi!");
                    }                  
                });
            }
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }
    ]);

});