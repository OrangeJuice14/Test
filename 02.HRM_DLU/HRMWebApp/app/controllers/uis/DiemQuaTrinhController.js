
define(['app/app', 'app/services/uis/professorService',  'app/services/uis/diemQuaTrinhService', ], function (app) {
    "use strict";

    app.controller('diemQuaTrinhController', ['$scope','professorService', 'diemQuaTrinhService',
        function ($scope, professorService,diemQuaTrinhService) {
            $scope.obj = {};
            $scope.studyYearId ="";
            $scope.studyTermId = "";
            professorService.getListStudyYear().then(function (result) {
                $scope.studyYears = result.data;
                $scope.selectedChangeStudyYear();
            });
            professorService.getListStudyTerm().then(function (result) {
                $scope.studyTerms = result.data;
                $scope.selectedChangeStudyYear();
            });
            $scope.selectedChangeStudyYear = function () {
                diemQuaTrinhService.GetScheduleStudyUnitsToProfessor($scope.studyYearId, $scope.studyTermId).then(function (result) {
                    $scope.obj = result.data;
                });
            };

            professorService.getCurrent().then(function (result) {
                $scope.studyYearId = result.data.currentYearStudy;
                $scope.studyTermId = result.data.currentTerm;
                diemQuaTrinhService.GetScheduleStudyUnitsToProfessor($scope.studyYearId, $scope.studyTermId).then(function (result) {
                    $scope.obj = result.data;
                });

            });

        }
    ]);
});