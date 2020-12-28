
define(['app/app', 'app/services/uis/professorService', ], function (app) {
    "use strict";

    app.controller('professorScheduleController', ['$scope', 'professorService',
            function ($scope, professorService) {
                $scope.obj = {};
                $scope.studyYearId = "";
                $scope.studyTermId = "";
                $scope.studyWeekId = "";
                $scope.isChanging= false;
               

                professorService.getListStudyYear().then(function (result) {
                    $scope.studyYears = result.data;
                    $scope.studyYear = $scope.studyYears[0];
                    $scope.selectedChangeStudyYear();
                });
                professorService.getListStudyTerm().then(function (result) {
                    $scope.studyTerms = result.data;
                    $scope.studyTerm = $scope.studyTerms[0];
                    $scope.selectedChangeStudyYear();
                });
                $scope.selectedChangeStudyYear = function () {
                    professorService.getListStudyWeek($scope.studyYearId, $scope.studyTermId).then(function (result) {
                        $scope.studyWeeks = result.data;
                        

                        if ($scope.isChanging == false) {
                            $scope.studyWeek = $scope.studyWeeks[0];
                            $scope.studyWeekId = $scope.studyWeeks[0].Id;
                        }
                       
                        professorService.getProfessorSchedule($scope.studyYearId, $scope.studyTermId, $scope.studyWeekId).then(function (result) {
                            $scope.obj = result.data;
                        });
                    });

                };
                $scope.selectedChangeWeek = function () {
                    
                    $.each($scope.studyWeeks,function(index,item){
                        if(item.Id==$scope.studyWeekId)
                            $scope.studyWeek =item;
                    });
                    

                    professorService.getProfessorSchedule($scope.studyYearId, $scope.studyTermId, $scope.studyWeekId).then(function (result) {
                        $scope.obj = result.data;
                    });

                };
             
                
                professorService.getCurrent().then(function (result) {
                    //$scope.current = result.data;
                    $scope.isChanging = true;
                    $scope.studyYearId = result.data.currentYearStudy;
                    $scope.studyTermId = result.data.currentTerm;
                    $scope.studyWeekId = result.data.currentWeek;
                    var currentWeek = $scope.studyWeekId;
                    professorService.getListStudyWeek($scope.studyYearId, $scope.studyTermId).then(function (result) {
                        $scope.studyWeeks = result.data;
                        $scope.studyWeekId = currentWeek;
                        $.each($scope.studyWeeks, function (idx,item) {
                            if (item.Id == $scope.studyWeekId)
                                $scope.studyWeek = item;
                        });
                        professorService.getProfessorSchedule($scope.studyYearId, $scope.studyTermId, $scope.studyWeekId).then(function (result) {
                            $scope.obj = result.data;
                            setTimeout(function () {
                                $scope.isChanging = false;
                            }, 500);
                        });
                       
                    });

                   
                });

               

            }
    ]);



});