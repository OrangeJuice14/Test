
define(['app/app', 'app/services/kpi/resultService', 'app/services/kpi/targetGroupDetailService', 'app/services/kpi/criterionService', 'app/services/kpi/authService', 'app/services/kpi/fileAttachmentService', 'app/controllers/kpi/ratingKPIInformationController', 'app/directives/directives'], function (app) {
    "use strict";

    app.controller('resultController', ['$scope', '$document', '$modal', '$rootScope', '$state', '$stateParams', 'resultService', 'targetGroupDetailService', 'criterionService', 'authService', 'fileAttachmentService',
    function ($scope, $document, $modal, $rootScope, $state, $stateParams, resultService, targetGroupDetailService, criterionService, authService, fileAttachmentService) {
        $("#sidebar").addClass("menu-compact");
        $scope.TABLECOLORS = MANAGER.TABLECOLORS;
        $scope.recordTemp = 0;
        $scope.planId = $stateParams.planId;
        $scope.agentObjectId = $stateParams.agentObjectId;
        $scope.isAdminRating = $stateParams.isAdminRating;
        $scope.planStaffId = $stateParams.planStaffId != "" ? $stateParams.planStaffId : MANAGER.GUID_EMPTY;
        $scope.supervisorId = $stateParams.supervisorId != "" ? $stateParams.supervisorId : MANAGER.GUID_EMPTY;
        $scope.resultList = [];
        $scope.criterionDictionaries = [];
        $scope.isRecordValid = true;
               
        resultService.getList($scope.planId, $scope.agentObjectId).then(function (result) {


            $scope.resultList = result.data;
        });     
    }
    ]);
});