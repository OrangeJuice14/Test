
define(['app/app', 'app/services/kpi/professorCriterionService'], function (app) {
    "use strict";
    app.controller('criterionDictionaryController', ['$scope', '$modalInstance', 'id','criterionId','type', 'professorCriterionService',
            function ($scope, $modalInstance, id, criterionId,type, professorCriterionService) {
                $scope.options = {
                    format: "n0",
                    max: 100
                };
                $scope.type = type;
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                $scope.isEdit = false;
                $scope.obj = {};
                $scope.title = "Chi tiết từ điển dữ liệu";

                if ($scope.isNew) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        ProfessorCriterion: {
                            Id: criterionId
                        }
                    };
                } else {
                    professorCriterionService.getDictionary(id).then(function (result) {
                        $scope.obj = result.data;
                        $scope.obj.ProfessorCriterion = {
                            Id: $scope.obj.CriterionId
                        }                           
                    });
                }
                
                $scope.save = function () {
                    professorCriterionService.SaveDictionary($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                };

                $scope.delete = function () {
                    professorCriterionService.SaveDictionary($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                };

                professorCriterionService.getCriterionTypeList().then(function (result) {
                    $scope.criterionTypes = result.data;
                });

                $scope.selectedChangeType = function () {
                    alert($scope.obj.CriterionType.Id);
                }

                $scope.cancel = function () {
                    $modalInstance.close();
                };
                    
                
            }
    ]);
});