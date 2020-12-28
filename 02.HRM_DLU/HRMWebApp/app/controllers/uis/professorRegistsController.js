
define(['app/app', 'app/services/uis/professorService', ], function (app) {
    "use strict";

    app.controller('professorRegistsController', ['$scope', 'professorService',
        function ($scope, professorService) {
            $scope.obj = {};
            $scope.studyYearId ="";
            $scope.studyTermId = "";
            professorService.getListStudyYear().then(function (result) {
                $scope.studyYears = result.data;
                //$scope.studyYearId = $scope.studyYears[0].Id;
                $scope.selectedChangeStudyYear();
            });
            professorService.getListStudyTerm().then(function (result) {
                $scope.studyTerms = result.data;
                //$scope.studyTermId = $scope.studyTerms[0].Id;
                $scope.selectedChangeStudyYear();
            });
            $scope.selectedChangeStudyYear = function () {
                professorService.getProfessorScheduleStudy($scope.studyYearId, $scope.studyTermId).then(function (result) {
                    $scope.obj = result.data;
                });
            };

            professorService.getCurrent().then(function (result) {
                //$scope.current = result.data;
                $scope.studyYearId = result.data.currentYearStudy;
                $scope.studyTermId = result.data.currentTerm;
                professorService.getProfessorScheduleStudy($scope.studyYearId, $scope.studyTermId).then(function (result) {
                    $scope.obj = result.data;
                });

            });

        }
    ]);
});