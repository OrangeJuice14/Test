
define(['app/app', 'app/services/uis/professorService', 'app/services/uis/diemThiTheoNhomService', ], function (app) {
    "use strict";

    app.controller('diemThiTheoNhomController', ['$scope', 'professorService', 'diemThiTheoNhomService',
        function ($scope, professorService,diemThiTheoNhomController) {
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
                diemThiTheoNhomController.GetExamiationProfessor($scope.studyYearId, $scope.studyTermId).then(function (result) {
                    $scope.obj = result.data;
                });
            };

            professorService.getCurrent().then(function (result) {
                $scope.studyYearId = result.data.currentYearStudy;
                $scope.studyTermId = result.data.currentTerm;
                diemThiTheoNhomController.GetExamiationProfessor($scope.studyYearId, $scope.studyTermId).then(function (result) {
                    $scope.obj = result.data;
                });

            });

        }
    ]);
});