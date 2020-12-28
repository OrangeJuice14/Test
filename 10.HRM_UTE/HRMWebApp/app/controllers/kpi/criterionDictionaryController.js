
define(['app/app', 'app/services/kpi/professorCriterionService'], function (app) {
    "use strict";
    app.controller('criterionDictionaryController', ['$scope', '$modalInstance', 'id', 'criterionId', 'type', 'targetGroupDetailId', 'professorCriterionService',
            function ($scope, $modalInstance, id, criterionId, type, targetGroupDetailId, professorCriterionService) {
                $scope.options = {
                    format: "n0"
                };
                $scope.NumberOfHour_options = {
                    format: "n1"
                };
                $scope.type = type;
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                $scope.isEdit = false;
                $scope.obj = {};
                $scope.title = "Chi tiết từ điển dữ liệu";
                $scope.obj.criterionId = criterionId;

                if ($scope.isNew) {
                    $scope.obj.Id = MANAGER.GUID_EMPTY;
                    $scope.obj.Name = "";
                    $scope.obj.ProfessorCriterionId = $scope.obj.criterionId;

                } else {
                    professorCriterionService.getDictionary(id).then(function (result) {
                        $scope.obj = result.data;
                        $scope.obj.ProfessorCriterionId = $scope.obj.CriterionId;

                    });
                }
                $scope.studyYears = {
                    placeholder: "Chọn năm học...",
                    dataTextField: "Name",
                    dataValueField: "Id",
                    valuePrimitive: true,
                    autoBind: false,
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return professorCriterionService.getStudyYear().then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        }
                    }
                };

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