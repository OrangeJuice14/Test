
define(['app/app', 'app/services/kpi/planKPIService', 'app/services/kpi/StaffService', 'app/services/kpi/agentObjectService','app/services/kpi/ratingKPIService', 'moment'], function (app) {
    "use strict";
    app.controller('planStaffDetailController', ['$scope', '$modal', '$rootScope', '$stateParams', 'planKPIService', 'agentObjectService', 'staffService',
            function ($scope, $modal, $rootScope, $stateParams, planKPIService, agentObjectService, staffService) {
                agentObjectService.getUserAgentObjectTypeId().then(function (result) {
                    $scope.agentObjectTypeId = result.data;
                });
                var moment = require('moment');
                $scope.date=new Date();
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
                    var isSame = moment(date1).isSame(date2,'day');
                    if (!isSame) {
                        var isAfter = moment(date1).isAfter(date2,'day');
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

                var templateEdit = "<span ng-switch on='#:data.PlanType #'>";
                templateEdit += "<span ng-switch-when='1'><span ng-if='dateCompare(\"#:data.EndTime#\",date)>=0 && (agentObjectTypeId==3 || agentObjectTypeId==4 || agentObjectTypeId==5 || agentObjectTypeId==6 || agentObjectTypeId==100)'><a class='year' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanType #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span><span ng-if='dateCompare(\"#:data.EndTime#\",date)<0  || agentObjectTypeId==1 || agentObjectTypeId==2'>#:data.Name #</span></span>";
                templateEdit += "<span ng-switch-when='2'><span ng-if='dateCompare(\"#:data.EndTime#\",date)>=0 && (agentObjectTypeId==1 || agentObjectTypeId==3 || agentObjectTypeId==4 || agentObjectTypeId==5 || agentObjectTypeId==6 || agentObjectTypeId==100 || agentObjectTypeId==99 || agentObjectTypeId==98)'><a class='semester' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanType #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span><span ng-if='dateCompare(\"#:data.EndTime#\",date)<0 || agentObjectTypeId==2'>#:data.Name #</span></span>";
                templateEdit += "<span ng-switch-when='3'><span ng-if='(agentObjectTypeId==2 || agentObjectTypeId==3 || agentObjectTypeId==4 || agentObjectTypeId==5 || agentObjectTypeId==6 || agentObjectTypeId==7 || agentObjectTypeId==8 || agentObjectTypeId==9 || agentObjectTypeId==100 || agentObjectTypeId==99 || agentObjectTypeId==97)'><a  class='month' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanType #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span>";


                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.obj = null;
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [
                 
                        {                          
                            template: templateEdit,
                            width: "100px",
                        }
                        ],
                };
                $scope.Edit = function (Id, PlanType,ratingStartTime,ratingEndTime) {
                    
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
                            ratingEndTime: function()
                            {
                                return ratingEndTime;
                            }

                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
                    });
                }              
            }
    ]);
    app.controller('planManageDetailController', ['$scope', '$modalInstance', 'id','planType','ratingStartTime','ratingEndTime', '$stateParams', 'planKPIService', 'agentObjectService', 'staffService','ratingKPIService',
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
            $scope.canRating = ($scope.dateCompare(ratingStartTime, $scope.date) <= 0 && $scope.dateCompare(ratingEndTime, $scope.date) >= 0);
            if ($scope.canRating==false)
            {
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
                staffService.getDepartmentStaff($scope.agentObjectTypeId).then(function (result) {
                    $scope.staffPlanKPIList = result.data;
                });
            });                   
            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);
});