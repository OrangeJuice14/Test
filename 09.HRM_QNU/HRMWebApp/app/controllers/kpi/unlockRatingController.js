
define(['app/app', 'app/services/kpi/planKPIService', 'app/services/kpi/agentObjectService', 'app/services/kpi/staffService','app/services/kpi/departmentService'], function (app) {
    "use strict";

    app.controller('unlockRatingController', ['$scope', '$modal', '$rootScope', '$state', '$stateParams', 'planKPIService', 'agentObjectService', 'staffService',
            function ($scope, $modal, $rootScope,$state, $stateParams, planKPIService, agentObjectService, staffService) {

                agentObjectService.getList().then(function (result) {
                    $scope.AgentObjects = result.data;
                    $scope.selectedChange();
                });
                staffService.getCurrentStaff().then(function (result) {
                    agentObjectService.GetUserAgentObjectId(result.data.Id).then(function (result2) {
                        $scope.UserAgentObjectId = result2.data;
                    });
                });
                $scope.planId = $stateParams.planId;
                planKPIService.getObj($scope.planId).then(function (result) {
                    $scope.planName=result.data.Name;
                });
                $scope.selectedChange = function () {
                    $scope.unlockRatingDataSource = new kendo.data.TreeListDataSource({
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
                    });
                };
                $scope.grid = {};
                $scope.unlockRatingGridOptions = {
                    sortable: true,
                    pageable: true,
                    selectable: true,
                    columns: [{
                        template: "<a ng-click='UnlockRatingPlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a>",
                        width: "60%",
                    }],
                };
                $scope.UnlockRatingPlan = function (planId) {
                    window.location = "/#/unlockRatingManage/" + planId;
                }                           
            }
    ]);
});