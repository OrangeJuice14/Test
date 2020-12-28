
define(['app/app', 'app/services/kpi/otherActivityDataService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('otherActivityDataController', ['$scope', '$modal', '$rootScope', 'otherActivityDataService',
            function ($scope, $modal, $rootScope, otherActivityDataService) {
                $scope.options = {
                    filter: "contains"
                }
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.scienceResearchDataName = '';
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.departmentId = MANAGER.GUID_EMPTY;
                $scope.staffId=MANAGER.GUID_EMPTY;
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return otherActivityDataService.getListByStaffId($scope.staffId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
                $scope.obj = null;
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: {
                        buttonCount: 7
                    },
                    columns: [
                    {
                        field: "Name",
                        title: "Hoạt động"
                    },
                    {
                        field: "NumberOfTime",
                        title: "Số lần"
                    },
                    {
                        field: "StudyTerm",
                        title: "Học kỳ"
                    },
                    {
                        field: "StudyYear",
                        title: "Năm học"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "50px"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "50px",
                    }
                    ],
                };
                $scope.$on("STAFFSELECTION", function (event, args) {
                    $scope.staffId = args;
                    $scope.grid.dataSource.read();
                });
                
                $scope.numericOptions = {
                    format: "n0",
                    min: 0
                }

                $scope.New = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/otherActivityData/detail.html',
                        controller: 'otherActivityDataDetailController',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            staffId: function () {
                                return $scope.staffId;
                            }
                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
                        $scope.grid.refresh();
                    });
                };
                $scope.Edit = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/otherActivityData/detail.html',
                        controller: 'otherActivityDataDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            staffId: function () {
                                return $scope.staffId;
                            }                           
                        }
                    }).result.then(function () {
                        $scope.grid.dataSource.read();
                        $scope.grid.refresh();
                    });
                };
               
                $scope.Delete = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                    if (!valid)
                        return;
                    otherActivityDataService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        otherActivityDataService.Delete($scope.obj).then(function () {
                            otherActivityDataService.getList().then(function (result) {
                                $scope.grid.dataSource.read();
                            });
                        });
                    });
                };
            }
    ]);
    app.controller('otherActivityDataDetailController', ['$scope', '$modalInstance', 'id','staffId', 'otherActivityDataService',
        function ($scope, $modalInstance, id,staffId, otherActivityDataService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Chi tiết hoạt động khác";
            $scope.obj = {};
            otherActivityDataService.getListProfessorCriterion().then(function (result) {
                $scope.professorCriterions = result.data;
                $scope.selectedChangeActivityGroup();
            });
            otherActivityDataService.getListStudyYear().then(function (result) {
                $scope.studyYears = result.data;
            });
            $scope.selectedChangeActivityGroup = function () {
                otherActivityDataService.getListDictionaryByManageCode($scope.obj.ActivityManageCode).then(function (result) {
                    $scope.professorCriterionsDictionaries = result.data;
                });
            };
            $scope.numericOptions = {
                format: "n0",
                min: 0
            }
            if ($scope.isNew) {
                $scope.obj = {
                    Id: MANAGER.GUID_EMPTY                
                };
                $scope.obj.StaffId = staffId;
            } else {
                otherActivityDataService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
            }
            $scope.save = function () {
                if ($scope.isNew) {
                    if ($scope.obj.Password != $scope.obj.PasswordConfirm) {
                        alert("Nhập lại mật khẩu không trùng với mật khẩu");
                        return;
                    }
                    otherActivityDataService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                } else {
                    otherActivityDataService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
            };
        }
    ]);

});