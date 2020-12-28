
define(['app/app', 'app/services/uis/professorService',  'app/services/uis/giaHanNhapDiemService', ], function (app) {
    "use strict";

    app.controller('giaHanNhapDiemController', ['$scope','professorService', 'giaHanNhapDiemService',
        function ($scope, professorService,giaHanNhapDiemService) {
            $scope.obj = {};
            $scope.studyYearId ="";
            $scope.studyTermId = "";
            $scope.typeid = "";

            professorService.getListStudyYear().then(function (result) {
                $scope.studyYears = result.data;
                $scope.selectedChangeStudyYear();
            });
            professorService.getListStudyTerm().then(function (result) {
                $scope.studyTerms = result.data;
                $scope.selectedChangeStudyYear();
            });

            giaHanNhapDiemService.LoaiNhapDiem_sel().then(function (result) {
                $scope.types = result.data;
                $scope.selectedChangeStudyYear();
            });


            $scope.selectedChangeStudyYear = function () {
                
                giaHanNhapDiemService.GiaHanNhapDiem_sel($scope.studyYearId, $scope.studyTermId, $scope.typeid).then(function (result) {
                    $scope.obj = result.data;
                });
                if ($scope.typeid != "2") {
                    $(".thPhongThi").attr("style", "display:none");
                    $(".thNgayThi").attr("style", "display:none");
                }
                else {
                    $(".thPhongThi").removeAttr("style");
                    $(".thNgayThi").removeAttr("style");
                }
            };

            $scope.saveGiahan = function () {

                giaHanNhapDiemService.GiaHanNhapDiem_Upd($scope.studyYearId, $scope.studyTermId, $scope.typeid, $scope.obj).then(function (result) {
                    if(result.data=="0")
                    {
                        $scope.selectedChangeStudyYear();
                        alert("Gia hạn thành công");
                    }
                    else
                    {
                    alert("Lỗi: Lưu gia hạn thất bại");
                       }

                });
               
            };

            professorService.getCurrent().then(function (result) {
                $scope.studyYearId = result.data.currentYearStudy;
                $scope.studyTermId = result.data.currentTerm;
                $scope.typeid= "0";
                //giaHanNhapDiemService.GiaHanNhapDiem_sel($scope.studyYearId, $scope.studyTermId,$scope.typeid).then(function (result) {
                //    $scope.obj = result.data;
                //});
                
            });

        }
    ]);
});