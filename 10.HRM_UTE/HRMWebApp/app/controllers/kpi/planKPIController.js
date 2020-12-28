
define(['app/app', 'app/services/kpi/planKPIService', 'app/services/kpi/agentObjectService', 'app/services/kpi/staffService', 'app/services/kpi/departmentService', 'app/services/kpi/studyYearService', 'moment'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('planKPIController', ['$scope', '$modal','$state', '$rootScope','$location', 'planKPIService', 'agentObjectService','staffService', 'studyYearService','departmentService',
            function ($scope, $modal, $state, $rootScope, $location, planKPIService, agentObjectService, staffService, studyYearService, departmentService) {
                var moment = require('moment');
                //cbbx
                agentObjectService.getList().then(function (result) {
                    $scope.AgentObjects = result.data;
                    //$scope.selectedChange();
                }); 

                studyYearService.getList().then(function (result) {
                    $scope.studyYears = result.data;
                    var currentStudyYear = getCurrentStudyYear();
                    for (var i = 0, l = result.data.length; i < l; i++) {
                        if (currentStudyYear == result.data[i].Name) {
                            $scope.planKPIYear = result.data[i].Id;
                        }
                    }
                });

                
                var getYearFirst = function (year) {
                    return year.slice(0, 4);
                }
                var getYearLast = function (year) {
                    return year.slice(-4);
                }

                planKPIService.getListByUserId().then(function (result) {
                    $scope.userPlanKPIList = result.data;
                });

                $.ajax({
                        type: 'GET',
                        url:  '/Api/staffApi/GetCurrentStaff',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (result) {
                            $scope.UserAgentObjectId = result;
                             $.ajax({
                                type: 'GET',
                                url: '/Api/agentObjectApi/GetUserAgentObjectId?userId=' + result.Id,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,
                                success: function (result2) {
                                    $scope.UserAgentObjectId = result2;
                                }
                            });
                        }
                    });


               

                //staffService.getCurrentStaff().then(function (result) {
                //    $.ajax({
                //        type: 'GET',
                //        url: '/Api/agentObjectApi/GetUserAgentObjectId?userId=' + result.data.Id,
                //        contentType: "application/json; charset=utf-8",
                //        dataType: "json",
                //        async: false,
                //        success: function (result) {
                //            $scope.UserAgentObjectId = result;
                //        }
                //    });

                

                //    //agentObjectService.GetUserAgentObjectId(result.data.Id).then(function (result2) {
                //    //    $scope.UserAgentObjectId = result2.data;
                //    //});

                //});
                function getCurrentStudyYear() {
                    var result = '';
                    var currentYear = parseInt(moment().format('YYYY'));
                    var currentMonth = parseInt(moment().format('MM'));
                    if (currentMonth >= 9)
                        result = currentYear + '-' + parseInt(currentYear + 1);
                    else
                        result = parseInt(currentYear - 1) + '-' + currentYear;
                    return result;
                }
                $scope.yearSelectedChange = function () {
                    if ($scope.planKPIYear != "" && $scope.planKPIYear != undefined) {
                        $scope.dataSource = new kendo.data.TreeListDataSource({
                            transport: {
                                read: function (options) {
                                    return planKPIService.getListByStudyYearId($scope.planKPIYear).then(function (result) {
                                        options.success(result.data);
                                        $.each(result.data, function (idx, item) {
                                            $scope.planId = item.Id;
                                            $scope.planType = item.PlanTypeId;
                                            $scope.RatingStartTime = item.RatingStartTime;
                                            $scope.RatingEndTime = item.RatingEndTime;
                                        })
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
                            }
                        });
                    }
                }
                
                $scope.$watch('planKPIYear', function (newVal, oldVal) {
                    $scope.yearSelectedChange();
                });

                $scope.selectedChange = function () {
                    $scope.dataSource = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return planKPIService.getListByAngentObjectId($scope.obj.AgentObject.Id).then(function (result) {
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
                    columns: [{
                        template: "<a ng-show='#:data.PlanTypeId #==1' class='year'  ng-click='SchollManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a>"
                        //<a ng-show='#:data.PlanTypeId #==2' class='semester' ng-click='SchollManagePlan(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.PlanTypeId #==3' class='month' ng-click='SchollManagePlan(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>"
                        //ng-click='SchollManagePlan(\"#:data.Id #\")'
                    }]
                };
                $scope.mainGridOptions2 = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                            template: "<a ng-show='#:data.PlanTypeId #==1' ng-click='Edit(\"#:data.Id #\")' class='year'  href='javascript:void(0)'>#:data.Name #</a>"
                        //<a ng-show='#:data.PlanTypeId #==2' class='semester' ng-click='Edit(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a><a ng-show='#:data.PlanTypeId #==3' class='month' ng-click='Edit(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>"
                    }]
                };
                $scope.mainGridOptions3 = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: "<a class='year' ng-show='#:data.PlanTypeId #==1'  href='javascript:void(0)'>#:data.Name #</a>"
                        //<a class='semester' ng-show='#:data.PlanTypeId #==2' ng-click='DepartmentPlan(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a><a class='month' ng-show='#:data.PlanTypeId #==3' ng-click='DepartmentPlan(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.Name #</a>"

                        //ng-click='DepartmentPlan(\"#:data.Id #\")'
                    }]
                };
                $scope.NewPlan = function () {
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
                $scope.New = function () {
                    $scope.isEdit = false;
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/planKPI/new.html',
                        controller: 'planKPIDetailController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            selectedId: function () {
                        return MANAGER.GUID_EMPTY;
                    }
                        }
                    }).result.then(function (result) {
                            $scope.grid.dataSource.read();
                    });
                    //$scope.jqxWindowSettings.apply('open');
                };
                $scope.CreateAllYear = function () {
                    var valid = window.confirm("Bạn muốn tạo không?");
                    if (!valid)
                        return;
                    planKPIService.getCheckExistPlanKPI($scope.planKPIYear).then(function (result) {
                        if (result.data == false) {
                            planKPIService.getCreatePlanKPIAllYear($scope.planKPIYear).then(function (result) {
                                Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                                $scope.grid.dataSource.read();
                            });
                        }
                        else {
                            Notify('Đã tồn tại!', 'top-right', '3000', 'warning', 'fa-warning', true);
                        }
                    });
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

                // Xem kế hoạch đơn vị

                //$scope.DepartmentPlan = function (Id) {
                //    if (Id == "") {
                //        alert("Bạn chưa chọn phần tử");
                //        return;
                //    }
                //    var modalInstance = $modal.open({
                //        animation: true,
                //        backdrop: false,
                //        templateUrl: '/app/views/kpi/planKPI/departmentPlan.html',
                //        controller: 'planDepartmentController',
                //        resolve: {
                //            id: function () {
                //                return Id;
                //            },
                //            selectedId: function () {
                //                var row = $scope.grid.select();
                //                var data = $scope.grid.dataItem(row);
                //                return data != null ? data.Id : null
                //            },
                //            selectedParentId: function () {
                //                var row = $scope.grid.select();
                //                var data = $scope.grid.dataItem(row);
                //                return data != null ? data.parentId : null
                //            }
                //        }
                //    }).result.then(function () {
                //        $scope.grid.dataSources.read();
                //    });
                //};

                $scope.dataSources = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return departmentService.getMainDeptWithoutUserDept().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    group: { field: "DepartmentTypeName", dir: "desc" },
                    sort: { field: "Name", dir: "asc" },
                    pageSize: 15
                });
                var templateEdits = "<div ng-switch on='#:data.DepartmentType #'>";
                templateEdits += "<div ng-switch-when='1'><div><a ng-click='DepartmentManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
                templateEdits += "<div ng-switch-when='4'><div><a ng-click='FacultyManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
                templateEdits += "<div ng-switch-default><div><a ng-click='OtherManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div></div>";

                $scope.DepartmentManagePlan = function (Id) {
                    //Ban giám hiệu
                    if (Id == '406f6e70-c3d9-49d8-b6f6-e39649ba8458')
                    {
                        staffService.getCurrentStaff().then(function (result2) {
                            $scope.normalStaffId = result2.data.Id;
                            $modalInstance.close();
                            $rootScope.isSupervisor = 1;
                            $state.go("kpi/SchoolManagePlankpidetail", { planId: $scope.planId, agentObjectId: "4731710F-94FB-4EF5-9F48-05E12648AA62", normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1 });
                            //window.location = "/#/kpi/SchoolManagePlankpidetail/" + $scope.planId + "/4731710F-94FB-4EF5-9F48-05E12648AA62/" + $scope.normalStaffId + "/1";
                        });                  
                    }
                    else
                    {
                        staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
                            $scope.agentObjectId = result.data;
                            staffService.getDepartmentLeaderId(Id).then(function (result2) {
                                $scope.normalStaffId = result2.data;
                                $modalInstance.close();
                                $rootScope.isSupervisor = 1;
                                $state.go("departmentManagePlankpidetail", { planId: $scope.planId, agentObjectId: $scope.agentObjectId, normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1, isConfig: 0 });
                                //window.location = "/#/kpi/plankpidetail/" + $scope.planId + "/" + $scope.agentObjectId + "/" + $scope.normalStaffId + "/1/0";
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
                            $rootScope.isSupervisor = 1;
                            $state.go('facultyManagePlankpidetail', { planId: $scope.planId, agentObjectId: $scope.agentObjectId, normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1, isConfig: 0 })
                        });
                    });
                }
                $scope.DepartmentRating = function (Id) {
                    //Ban giám hiệu
                    if (Id == '406f6e70-c3d9-49d8-b6f6-e39649ba8458') {
                        alert('Chưa thể đánh giá!');
                    }
                    else {
                        staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
                            $scope.agentObjectId = result.data;
                            $modalInstance.close();
                            $rootScope.isAdminRating = 1;
                            $state.go('departmentRatingKPI', { planId: $scope.planId, agentObjectId: $scope.agentObjectId, planStaffId: "", supervisorId: "", departmentId: Id, isAdminRating: 1 })
                            //window.location = "/#/kpi/departmentRatingKPI/" + $scope.planId + "/" + $scope.agentObjectId + "///" + Id + "/1";
                        });
                    }
                }
                $scope.FacultyRating = function (Id) {
                }
                $scope.OtherManagePlan = function (Id) {
                }
                $scope.mainGridOptions5 = {
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
                        template: templateEdits,
                        field: "Name",
                        title: "Đơn vị"
                    },
                     {
                         field: "DepartmentTypeName",
                         title: "Loại đơn vị",
                         width: "30%"
                     }]
                };
            
                // end xem kế hoạch đơn vị//

                $scope.SchollManagePlan = function (planId) {
                    //alert(planId + ' ' + $scope.UserAgentObjectId);
                    //$location.path("/#/schoolManagePlankpidetail/" + planId + "/" + $scope.UserAgentObjectId + "//0");
                    //var pathstr = "/schoolManagePlankpidetail/" + planId + "/" + $scope.UserAgentObjectId + "//0";
                    //$location.path() = "/#/userPlanKPI";
                    //$location.path("/#/schoolManagePlankpidetail/d662dbf9-6076-42ff-9c90-b4b8196389c1/4731710f-94fb-4ef5-9f48-05e12648aa62//0");
                    //$location.path(pathstr);
                    $state.go('schoolManagePlankpidetail', { 'planId': planId, 'agentObjectId': $scope.UserAgentObjectId, 'normalStaffId': MANAGER.GUID_EMPTY, 'departmentId': MANAGER.GUID_EMPTY, 'isSupervisor': 0 });
         
                    //window.location = "/#/kpi/schoolManagePlankpidetail/" + planId + "/" + $scope.UserAgentObjectId + "//0";
                }
            }
    ]);

    app.controller('planKPIDetailController', ['$scope', '$modalInstance', 'id','selectedId', 'planKPIService', 'agentObjectService','departmentService',
        function ($scope, $modalInstance, id, selectedId, planKPIService, agentObjectService, departmentService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.ParentPlans = [];
            if (!$scope.isNew)
            {
                planKPIService.getParentPlanListById(id).then(function (result) {
                    $scope.ParentPlans = result.data;
                });
            }
             $scope.PlanTypes = [];
            planKPIService.getListPlanType().then(function (result) {
                $scope.PlanTypes = result.data;
            });
            $scope.title = "Chi tiết kế hoạch";
            $scope.obj = {};

            if ($scope.isNew) {
                $scope.obj = {
                    Id: MANAGER.GUID_EMPTY,
                    StartTime: new Date(),
                    EndTime: new Date(),
                    RatingStartTime: new Date(),
                    RatingEndTime: new Date(),
                };                
            } else {
                planKPIService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }

            $scope.save = function () {
                if ($scope.isNew) {
                    planKPIService.SaveNew($scope.obj).then(function () {
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
    app.controller('planDepartmentController', [ '$rootScope', '$scope','$state', '$modalInstance', 'id', 'selectedId', 'selectedParentId', 'planKPIService', 'agentObjectService', 'departmentService', 'staffService',
       function ($rootScope,$scope, $state, $modalInstance, id, selectedId, selectedParentId, planKPIService, agentObjectService, departmentService, staffService) {
           $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
           $scope.planId = id;       
           $scope.title = "Chi tiết kế hoạch";
           $scope.obj = {};
           $scope.cancel = function () {
               $modalInstance.dismiss('cancel');
           };

           $scope.dataSources = new kendo.data.DataSource({
               transport: {
                   read: function (options) {
                       return departmentService.getMainDeptWithoutUserDept().then(function (result) {
                           options.success(result.data);
                       });
                   }
               },
               group: { field: "DepartmentTypeName", dir: "desc" },
               sort: { field: "Name", dir: "asc" },
               pageSize: 15
           });
           var templateEdit = "<div ng-switch on='#:data.DepartmentType #'>";
           templateEdit += "<div ng-switch-when='1'><div><a ng-click='DepartmentManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
           templateEdit += "<div ng-switch-when='4'><div><a ng-click='FacultyManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
           templateEdit += "<div ng-switch-default><div><a ng-click='OtherManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div></div>";
           var templateEdit1 = "<div ng-switch on='#:data.DepartmentType #'>";
           templateEdit1 += "<div ng-switch-when='1'><div style='text-align:center'><a ng-click='DepartmentRating(\"#:data.Id #\")' href='javascript:void(0)'><span ng-if='#:data.IsManaged #'>Đánh giá</span></a></div></div>";
           templateEdit1 += "<div ng-switch-when='4'><div style='text-align:center'><a ng-click='FacultyRating(\"#:data.Id #\")' href='javascript:void(0)'><span ng-if='#:data.IsManaged #'>Đánh giá</span></a></div></div>";
           templateEdit1 += "<div ng-switch-default><div style='text-align:center'></div></div></div>";
           $scope.DepartmentManagePlan = function (Id) {
               //Ban giám hiệu
               if (Id == '406f6e70-c3d9-49d8-b6f6-e39649ba8458')
               {
                       staffService.getCurrentStaff().then(function (result2) {
                           $scope.normalStaffId = result2.data.Id;
                           $modalInstance.close();
                           $rootScope.isSupervisor = 1;
                           $state.go("kpi/SchoolManagePlankpidetail", { planId: $scope.planId, agentObjectId: "4731710F-94FB-4EF5-9F48-05E12648AA62", normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1 });
                           //window.location = "/#/kpi/SchoolManagePlankpidetail/" + $scope.planId + "/4731710F-94FB-4EF5-9F48-05E12648AA62/" + $scope.normalStaffId + "/1";
                       });                  
               }
               else
               {
                   staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
                   $scope.agentObjectId = result.data;
                   staffService.getDepartmentLeaderId(Id).then(function (result2) {
                       $scope.normalStaffId = result2.data;
                       $modalInstance.close();
                       $rootScope.isSupervisor = 1;
                       $state.go("departmentManagePlankpidetail", { planId: $scope.planId, agentObjectId: $scope.agentObjectId, normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1, isConfig: 0 });
                       //window.location = "/#/kpi/plankpidetail/" + $scope.planId + "/" + $scope.agentObjectId + "/" + $scope.normalStaffId + "/1/0";
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
                       $rootScope.isSupervisor = 1;
                       $state.go('facultyManagePlankpidetail', { planId: $scope.planId, agentObjectId: $scope.agentObjectId, normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1, isConfig: 0 })
                   });
               });
           }
           $scope.DepartmentRating = function (Id) {
               //Ban giám hiệu
               if (Id == '406f6e70-c3d9-49d8-b6f6-e39649ba8458') {
               alert('Chưa thể đánh giá!');
               }
               else {
                   staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
                       $scope.agentObjectId = result.data;
                       $modalInstance.close();
                       $rootScope.isAdminRating = 1;
                       $state.go('departmentRatingKPI', { planId: $scope.planId, agentObjectId: $scope.agentObjectId, planStaffId: "", supervisorId: "", departmentId: Id, isAdminRating: 1 })
                           //window.location = "/#/kpi/departmentRatingKPI/" + $scope.planId + "/" + $scope.agentObjectId + "///" + Id + "/1";
                   });
               }
           }
           $scope.FacultyRating = function (Id) {
           }
           $scope.OtherManagePlan = function (Id) {
           }
           $scope.mainGridOptions5 = {
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
                   template: templateEdit,
                   title: "Đơn vị"
               },
              {
                  field: "DepartmentTypeName",
                  title: "Loại đơn vị",
                  width: "25%"
              },
               {
                   template: templateEdit1,
                   width: "20%"
               }]
           };
       }
    ]);
});