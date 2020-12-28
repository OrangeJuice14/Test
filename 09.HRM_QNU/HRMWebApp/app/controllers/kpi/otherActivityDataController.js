
define(['app/app', 'app/services/kpi/otherActivityDataService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService', 'app/services/kpi/departmentService', 'app/services/kpi/staffService'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('otherActivityDataController', ['$scope', '$modal', '$rootScope', 'otherActivityDataService', 'departmentService',
            function ($scope, $modal, $rootScope, otherActivityDataService, departmentService) {
                $scope.options = {
                    filter: "contains"
                }
                departmentService.getList().then(function (result) {
                    $scope.departments = result.data;
                });
                otherActivityDataService.getListStudyYear().then(function (result) {
                    $scope.studyYears = result.data;
                });
                otherActivityDataService.getListProfessorCriterion().then(function (result) {
                    $scope.professorCriterions = result.data;
                    $scope.selectedChangeActivityGroup();
                });
                $scope.selectedChangeActivityGroup = function () {
                    otherActivityDataService.getListDictionaryByManageCode($scope.ActivityManageCode).then(function (result) {
                        $scope.professorCriterionsDictionaries = result.data;
                    });
                };
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.scienceResearchDataName = '';
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.ActivityManageCode = '';
                $scope.ManageCode = '';
                $scope.departmentId = MANAGER.GUID_EMPTY;
                $scope.staffId = MANAGER.GUID_EMPTY;
                $scope.deptId = MANAGER.GUID_EMPTY;
                $scope.studyYear = '';
                $scope.StudyTerm = '';
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            if ($scope.deptId == undefined)
                                $scope.deptId = MANAGER.GUID_EMPTY;
                            var deptId = $scope.deptId;
                            var studyYear = $scope.studyYear;
                            var activityManageCode = $scope.ActivityManageCode;
                            var manageCode = $scope.ManageCode;
                            var studyTerm = $scope.StudyTerm;
                            return otherActivityDataService.searchOtherActivity(deptId, studyYear, activityManageCode, manageCode, studyTerm).then(function (result) {
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
                        //template: "<a ng-click='Edit(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.StaffName #</a>",
                        field: "StaffName",
                        title: "Họ tên"
                    },

                     {
                         field: "DepartmentName",
                         title: "Đơn vị"
                     },
                    {
                        field: "Name",
                        title: "Hoạt động"
                    },
                    //{
                    //    field: "NumberOfTime",
                    //    title: "Số lần"
                    //},
                    {
                        field: "StudyTerm",
                        title: "Học kỳ",
                        width: "60px"
                    },
                    {
                        field: "StudyYear",
                        title: "Năm học",
                        width: "100px"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "50px",
                    }
                    ],
                };
                //$scope.$on("STAFFSELECTION", function (event, args) {
                //    $scope.staffId = args;
                //    $scope.grid.dataSource.read();
                //});
                $scope.uploadOptions = {
                    async: {
                        saveUrl: "/OtherActivityImportData/SaveFileToData",
                        autoUpload: true
                    },
                    success: onUploadSuccess
                }
                function onUploadSuccess(e) {
                    $scope.grid.dataSource.read();
                    $scope.grid.refresh();
                }
                $scope.numericOptions = {
                    format: "n0",
                    min: 0
                }
                $scope.SearchDept = function () {
                    $scope.dataSource = new kendo.data.DataSource({
                        dataType: 'json',
                        transport: {
                            read: function (options) {
                                if ($scope.deptId == undefined)
                                    $scope.deptId = MANAGER.GUID_EMPTY;
                                var deptId = $scope.deptId;
                                var studyYear = $scope.studyYear;
                                var activityManageCode = $scope.ActivityManageCode;
                                var manageCode = $scope.ManageCode;
                                var studyTerm = $scope.StudyTerm;
                                return otherActivityDataService.searchOtherActivity(deptId, studyYear, activityManageCode, manageCode, studyTerm).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                };
                $scope.New = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/otherActivityData/detail.html',
                        controller: 'otherActivityDataDetailController',
                        backdrop: false,
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
    app.controller('otherActivityDataDetailController', ['$scope', '$modalInstance', 'id', 'staffId', 'otherActivityDataService', 'staffService', 'departmentService',
        function ($scope, $modalInstance, id, staffId, otherActivityDataService, staffService, departmentService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Chi tiết hoạt động khác";
            $scope.obj = {};
            $scope.SubStaffIds = [];
            $scope.staffIds = [];
            $scope.staffNames = [];
            $scope.DepartmentList = [];
            $scope.DepartmentId = "";
            $scope.staffResource = new kendo.data.DataSource({
                dataType: 'json',
                transport: {
                    read: function (options) {
                        return staffService.getListByDepartmentId($scope.DepartmentId).then(function (result) {
                            options.success(result.data);
                        });
                        //return staffService.getSearchOnlyProfessor().then(function (result) {
                        //    options.success(result.data);
                        //});
                    }
                },
            });
            //$scope.staffOption = {
            //    placeholder: "",
            //    dataTextField: "Name",
            //    dataValueField: "Id",
            //    valuePrimitive: true,
            //    filter: "contains",
            //    autoBind: false
            //};
            $scope.staffOption = {
                sortable: true,
                selectable: "multiple",
                filterable: {
                    messages: {
                        info: "Lọc bởi : ",
                        filter: "Lọc",
                        clear: "Xóa"
                    },
                    extra: false,
                    operators: {
                        string: {
                            contains: "Từ khóa"
                        }
                    }
                },
                columns: [
                {
                    field: "Checked",
                    template: '<input ng-model = "dataItem.Checked" type="checkbox"></input>',
                    width: "50px",
                    title: ""
                },
                {
                    field: "Name",
                    title: "Họ tên"
                },
                 //{
                 //    field: "DepartmentName",
                 //    title: "Đơn vị"
                 //}               
                ]
            };
            $scope.onClick = function () {
                $scope.staffNames=[];
                for (var i = 0; i < $scope.grid.dataSource.data().length; i++) {
                    var item = $scope.grid.dataSource.at(i);
                    if (item.Checked) {
                        $scope.staffNames.push(item.Name);
                        $scope.staffIds.push(item.Id);
                    }               
                }
            }
            otherActivityDataService.getListProfessorCriterion().then(function (result) {
                $scope.professorCriterions = result.data;
                $scope.selectedChangeActivityGroup();
            });
            otherActivityDataService.getListStudyYear().then(function (result) {
                $scope.studyYears = result.data;
            });
            departmentService.getList().then(function (result) {
                $scope.DepartmentList = result.data;
                $scope.DepartmentId = $scope.DepartmentList[0].Id;
                $scope.selectedChangeDepartment();
            });
            $scope.selectedChangeActivityGroup = function () {
                otherActivityDataService.getListDictionaryByManageCode($scope.obj.ActivityManageCode).then(function (result) {
                    $scope.professorCriterionsDictionaries = result.data;
                });
            };
            $scope.selectedChangeDepartment = function () {
                $scope.staffResource = {};
                staffService.getListByDepartmentId($scope.DepartmentId).then(function (result) {
                    $scope.staffResource = result.data;
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
                    otherActivityDataService.Save($scope.obj).then(function () {
                        alert("Thành công!");
                    });
                } else {
                    otherActivityDataService.Save($scope.obj).then(function () {
                        $modalInstance.close();
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.close();
            };
        }
    ]);

});