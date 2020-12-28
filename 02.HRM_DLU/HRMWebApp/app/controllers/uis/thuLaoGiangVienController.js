
define(['app/app', 'app/services/uis/professorService',  'app/services/uis/thuLaoGiangVienService', ], function (app) {
    "use strict";

    app.controller('thuLaoGiangVienController', ['$scope','professorService', 'thuLaoGiangVienService',
        function ($scope, professorService, thuLaoGiangVienService) {
            $scope.obj = {};
            $scope.obj1 = {};
            $scope.obj2 = {};
            $scope.studyYearId ="";
            $scope.studyTermId = "";
           // $scope.dotThanhToanId = "";

            professorService.getListStudyYear().then(function (result) {
                $scope.studyYears = result.data;
                $scope.selectedChangeStudyYear();
            });
            professorService.getListStudyTerm().then(function (result) {
                $scope.studyTerms = result.data;
                $scope.selectedChangeStudyYear();
            });

            thuLaoGiangVienService.CauHinhChotGio_GetByNamHocHocKy($scope.studyYearId, $scope.studyTermId).then(function (result) {
                $scope.DotThanhToans = result.data;
                $scope.dotThanhToanId = result.data[0].MaCauHinhGioChot;
                $scope.selectedChangeStudyYear();
            });


            $scope.selectedChangeStudyYear = function () {
                
                thuLaoGiangVienService.KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang0($scope.studyYearId, $scope.studyTermId, $scope.dotThanhToanId).then(function (result) {
                    $scope.obj = result.data;
                    if(result.data.length ==0)
                    {
                        $("#tblTable0").hide();
                    }
                   
                });
                thuLaoGiangVienService.KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang1($scope.studyYearId, $scope.studyTermId, $scope.dotThanhToanId).then(function (result) {
                    $scope.obj1 = result.data;
                    if (result.data.length == 0) {
                        $("#tblTable1").hide();
                    }
                });
                thuLaoGiangVienService.KetQuaThanhToanThuLao_ThuLaoTrenWeb_Dlu_Bang2($scope.studyYearId, $scope.studyTermId, $scope.dotThanhToanId).then(function (result) {
                    $scope.obj2 = result.data;
                   
                    if (result.data.length == 0) {
                        $("#tblTable2").hide();
                    }
                });
            
            };

           

            professorService.getCurrent().then(function (result) {
                $scope.studyYearId = result.data.currentYearStudy;
                $scope.studyTermId = result.data.currentTerm;
              
            });

        }
    ]);
});