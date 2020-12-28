
define(['app/app', 'app/services/kpi/planKPIService', 'app/services/kpi/StaffService', 'app/services/kpi/agentObjectService', 'app/services/kpi/ratingKPIService', 'app/services/kpi/departmentService', 'moment'], function (app) {
    "use strict";
    app.controller('planStaffDetailController', ['$scope', '$modal', '$rootScope', '$stateParams', 'planKPIService', 'agentObjectService', 'staffService',
            function ($scope, $modal, $rootScope, $stateParams, planKPIService, agentObjectService, staffService) {
                agentObjectService.getUserAgentObjectTypeId().then(function (result) {
                    $scope.agentObjectTypeId = result.data;
                });
                var moment = require('moment');
                $scope.date = new Date();
                //planKPIService.getDateTime().then(function (result) {
                //    $scope.date = result.data;
                //});
                //alert($scope.date);
                $scope.dataSource = new kendo.data.TreeListDataSource({
                    transport: {
                        read: function (options) {
                            return planKPIService.getListByDepartment().then(function (result) {
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
                $scope.dateCompare = function (date1, date2) {
                    var result = -1;
                    var isSame = moment(date1).isSame(date2, 'day');
                    if (!isSame) {
                        var isAfter = moment(date1).isAfter(date2, 'day');
                        if (isAfter) {
                            result = 1;
                        }
                        else
                            result = -1;
                    }
                    else
                        result = 0;
                    return result;
                }

                //var templateEdit = "<span ng-switch on='#:data.PlanType #'>";
                //templateEdit += "<span ng-switch-when='1'><span ng-if='dateCompare(\"#:data.EndTime#\",date)>=0 && (agentObjectTypeId==3 || agentObjectTypeId==4 || agentObjectTypeId==5 || agentObjectTypeId==6 || agentObjectTypeId==100)'><a class='year' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanType #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span><span ng-if='dateCompare(\"#:data.EndTime#\",date)<0  || agentObjectTypeId==1 || agentObjectTypeId==2'>#:data.Name #</span></span>";
                //templateEdit += "<span ng-switch-when='2'><span ng-if='dateCompare(\"#:data.EndTime#\",date)>=0 && (agentObjectTypeId==1 || agentObjectTypeId==3 || agentObjectTypeId==4 || agentObjectTypeId==5 || agentObjectTypeId==6 || agentObjectTypeId==100 || agentObjectTypeId==99 || agentObjectTypeId==98)'><a class='semester' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanType #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span><span ng-if='dateCompare(\"#:data.EndTime#\",date)<0 || agentObjectTypeId==2'>#:data.Name #</span></span>";
                //templateEdit += "<span ng-switch-when='3'><span ng-if='dateCompare(\"#:data.EndTime#\",date)>=0 && (agentObjectTypeId==2 || agentObjectTypeId==3 || agentObjectTypeId==4 || agentObjectTypeId==5 || agentObjectTypeId==6 || agentObjectTypeId==7 || agentObjectTypeId==8 || agentObjectTypeId==9 || agentObjectTypeId==100 || agentObjectTypeId==99 || agentObjectTypeId==97)'><a  class='month' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanType #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span><span ng-if='dateCompare(\"#:data.EndTime#\",date)<0'>#:data.Name #</span></span></span>";

                var templateEdit = "<span ng-switch on='#:data.PlanTypeId #'>";
                templateEdit += "<span ng-switch-when='1'><span ><a class='year' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanTypeId #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span></span>";
                templateEdit += "<span ng-switch-when='2'><span ><a class='semester' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanTypeId #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span></span>";
                templateEdit += "<span ng-switch-when='3'><span ><a class='month' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanTypeId #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span></span>";

                var templateView = "<span ng-switch on='#:data.PlanTypeId #'>";
                templateView += "<span ng-switch-when='1'><span ><a class='year' ng-click='DepartmentPlan(\"#:data.Id #\")'>#:data.Name #</a></span></span>";
                templateView += "<span ng-switch-when='2'><span ><a class='semester' ng-click='DepartmentPlan(\"#:data.Id #\")'>#:data.Name #</a></span></span>";
                templateView += "<span ng-switch-when='3'><span ><a class='month' ng-click='DepartmentPlan(\"#:data.Id #\")'>#:data.Name #</a></span></span>";

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
                        template: templateEdit,
                        width: "100px",
                    }]
                };
                $scope.mainGridOptions2 = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: templateView,
                        width: "100px",
                    }]
                };




                $scope.Edit = function (Id, PlanType, ratingStartTime, ratingEndTime) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/planKPIDetail/planManageDetail.html',
                        controller: 'planManageDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            planType: function () {
                                return PlanType;
                            },
                            ratingStartTime: function () {
                                return ratingStartTime;
                            },
                            ratingEndTime: function () {
                                return ratingEndTime;
                            }

                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
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
            }
    ]);
    app.controller('planManageDetailController', ['$scope', '$modalInstance', 'id', 'planType', 'ratingStartTime', 'ratingEndTime', '$stateParams', 'planKPIService', 'agentObjectService', 'staffService', 'ratingKPIService',
        function ($scope, $modalInstance, id, planType, ratingStartTime, ratingEndTime, stateParams, planKPIService, agentObjectService, staffService, ratingKPIService) {
            var moment = require('moment');
            $scope.date = new Date();

            $scope.dateCompare = function (date1, date2) {
                var result = -1;
                var isSame = moment(date1).isSame(date2, 'day');
                if (!isSame) {
                    var isAfter = moment(date1).isAfter(date2, 'day');
                    if (isAfter) {
                        result = 1;
                    }
                    else
                        result = -1;
                }
                else
                    result = 0;
                return result;
            }

            $scope.normalStaffId = MANAGER.GUID_EMPTY;
            $scope.planId = id;
            $scope.planType = planType;
            $scope.freeRating = false;
            $scope.canRating = ($scope.dateCompare(ratingStartTime, $scope.date) <= 0 && $scope.dateCompare(ratingEndTime, $scope.date) >= 0);
            if ($scope.canRating == false) {
                ratingKPIService.getCheckUnlockable($scope.planId).then(function (result) {
                    $scope.canRating = result.data;
                });
            }
            staffService.getCurrentStaff().then(function (result) {
                $scope.staffId = result.data.Id;
            });



            if ($scope.normalStaffId == MANAGER.GUID_EMPTY) {
                planKPIService.getPlanListDepartment($scope.normalStaffId, id).then(function (result) {
                    $scope.userPlanKPIList = result.data;
                });
            }
            else {
                planKPIService.getListByUserId($scope.normalStaffId).then(function (result) {
                    $scope.userPlanKPIList = result.data;
                });
            }

            agentObjectService.getUserAgentObjectTypeId().then(function (result) {
                $scope.agentObjectTypeId = result.data;
                staffService.getDepartmentStaff($scope.agentObjectTypeId, $scope.planId).then(function (result) {
                    $scope.staffPlanKPIList = result.data;
                });
                //Danh sách phó phòng ban
                staffService.getViceDepartmentStaff($scope.agentObjectTypeId, MANAGER.GUID_EMPTY, $scope.planId).then(function (result) {
                    $scope.viceDepartmentStaffPlanKPIList = result.data;
                });

                staffService.getViceDepartmentStaff($scope.agentObjectTypeId, MANAGER.GUID_EMPTY, $scope.planId).then(function (result) {
                    $scope.vicePrincipalStaffPlanKPIList = result.data;
                });
            });
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
           templateEdit += "<div ng-switch-when='1'><div><a ng-click='DepartmentManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
           templateEdit += "<div ng-switch-when='4'><div><a ng-click='FacultyManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
           templateEdit += "<div ng-switch-default><div><a ng-click='OtherManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div></div>";
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
                   template: templateEdit,
                   field: "Name",
                   title: "Đơn vị"
               },
                {
                    field: "DepartmentTypeName",
                    title: "Loại đơn vị",
                    width: "30%"
                }]
           };
       }
    ]);
});