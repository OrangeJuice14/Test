define(['app/app', 'moment', 'app/services/kpi/planKPIService', 'app/services/kpi/departmentService', 'app/services/kpi/reportService', 'app/services/kpi/staffService'], function (app) {
    "use strict";
    app.controller('totalPlanKPIDetailController', ['$scope', '$modal', '$rootScope', '$state', '$stateParams', 'planKPIService', 'departmentService', 'reportService', 'staffService',
            function ($scope, $modal, $rootScope, $state, $stateParams, planKPIService, departmentService, reportService, staffService, moment) {
                var moment = require('moment');
                $scope.grid = {};
                $scope.departmentId = "";
                $scope.planId = "";
                $scope.yearList = [];
                $scope.studyYearId = "";

                function getCurrentStudyYear() {
                    var result = '';
                    var currentYear = parseInt(moment().format('YYYY'));
                    var currentMonth = parseInt(moment().format('MM'));
                    if (currentMonth >= 9)
                        result = currentYear + '-' + parseInt(currentYear + 1);
                    else
                        result = parseInt(currentYear - 1) + '-' + currentYear;
                    return result;
                }
                $scope.yearList = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return planKPIService.getYearList().then(function (result) {
                                options.success(result.data);
                                var currentStudyYear = getCurrentStudyYear();
                                for (var i = 0, l = result.data.length; i < l; i++) {
                                    if (currentStudyYear == result.data[i].Name) {
                                        $scope.studyYearId = result.data[i].Id;
                                    }
                                }
                            });
                        }
                    }
                });
                $scope.getListPlanKPIByStudyYear = function () {
                    if ($scope.studyYearId != "") {
                        //$scope.planDataSource = new kendo.data.TreeListDataSource({
                        //    transport: {
                        //        read: function (options) {
                        //            return planKPIService.getListByDepartment($scope.studyYearId).then(function (result) {
                        //                options.success(result.data);
                        //            });
                        //        }
                        //    },
                        //    schema: {
                        //        model: {
                        //            id: "Id",
                        //            fields: {
                        //                parentId: { field: "ParentId", nullable: true },
                        //                Id: { field: "Id", type: "string" }
                        //            },
                        //            expanded: true
                        //        }
                        //    }
                        //});
                        $scope.planDataSource = {
                            transport: {
                                read: function (options) {
                                    return planKPIService.getListByStudyYearId($scope.studyYearId).then(function (result) {
                                        options.success(result.data);
                                        $scope.planId = result.data[0].Id;
                                    });
                                }
                            }
                        }
                    }
                }

                $scope.departmentDataSource = new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            return departmentService.getList().then(function (result) {
                                var list = result.data;
                                options.success(list);
                                $scope.departmentId = list[0].Id;
                            });
                        },
                    }
                });

                //var departmentList = [];
                //evaluationBoardService.getDepartmentList().then(function (result) {
                //    departmentList = result.data;
                //    if (result.data.length > 0) {
                //        $scope.departmentId = result.data[0].Id;
                //        if ($scope.departmentId != "" && $scope.planId != "")
                //            $scope.Search();
                //    }
                //});

                $scope.$watch('studyYearId', function (newVal, oldVal) {
                    $scope.getListPlanKPIByStudyYear();
                });

                staffService.getCurrentStaff().then(function (result) {
                    $scope.staffId = result.data.Id;
                });

                $scope.Search = function () {
                    $scope.dataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                return reportService.getListPlanKPIDetailByDepartment($scope.planId, $scope.departmentId).then(function (result) {
                                    var data = result.data;
                                    if (data.length > 0) {
                                        for (i = 0; i < data.length; i++) {
                                            data[i].Checked = false;
                                        }
                                    }
                                    options.success(data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                }
                $scope.mainGridOptions = {
                    sortable: true,
                    pageable: {
                        buttonCount: 7
                    },
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
                            title: "STT",
                            template: function (dataItem) {
                                var index = $scope.dataSource.indexOf(dataItem) + 1;
                                return "<p style='text-align:center; margin: 0'>" + index + "</p>";
                            },
                            width: 45
                        },
                        {
                            field: "StaffName",
                            title: "Họ tên"
                        },
                        {
                            field: "PositionName",
                            title: "Chức vụ",
                            filterable: false
                        },
                        {
                            //field: "AgentObjectName",
                            title: "Đối tượng KPI",
                            template: function (dataItem) {
                                if (dataItem.AgentObjectTypeId == 999)
                                    return "Loại khác";
                                else return dataItem.AgentObjectName;
                            }
                        },
                        {
                            title: "Xem kế hoạch",
                            width: 150,
                            template: function (dataItem) {
                                var result = "";
                                var param = dataItem.Id + "/" + dataItem.AgentObjectId + "/" + dataItem.StaffId + "/" + dataItem.DepartmentId;
                                switch (dataItem.AgentObjectTypeId) {
                                    case 1:
                                        result = "/#/kpi/professorPlankpidetail/" + param + "/{{2}}";
                                        break;
                                    case 2:
                                        result = "/#/kpi/plankpidetail/" + param + "/{{2}}/{{0}}";
                                        break;
                                    case 3: 
                                        result = "/#/kpi/departmentManagePlankpidetail/" + param + "/{{2}}/{{0}}";
                                        break;
                                    case 4: 
                                        result = "";
                                        break;
                                    case 5: 
                                        result = "/#/kpi/facultyManagePlankpidetail/" + param + "/{{2}}/{{0}}";
                                        break;
                                    case 6: case 12: 
                                        result = "/#/kpi/subjectManagePlankpidetail/" + param + "/{{2}}";
                                        break;
                                    case 7: 
                                        result = "/#/kpi/subDepartmentPlankpiDetail/" + param + "/{{2}}/{{0}}";
                                        break;
                                    case 8: 
                                        result = "/#/kpi/subFacultyManagePlankpidetail/" + param + "/{{2}}/{{0}}";
                                        break;
                                    case 10: 
                                        result = "/#/kpi/principalPlankpidetail/" + param + "/{{2}}/{{0}}";
                                        break;
                                    case 11: 
                                        result = "/#/kpi/vicePrincipalPlankpidetail/" + param + "/{{2}}/{{0}}";
                                        break;
                                }
                                if (result == "")
                                    return "";
                                return "<a href=" + result + ">Xem kế hoạch</a>";
                                //return "<a href=" + result + " target='_blank'>Xem kế hoạch</a>";
                            }
                        },
                        {
                            title: "Xem đánh giá",
                            width: 150,
                            template: function (dataItem) {
                                var result = "";
                                var param = dataItem.Id + "/" + dataItem.AgentObjectId + "/" + dataItem.StaffId + "/" + $scope.staffId + "/" + dataItem.DepartmentId + "/2";
                                switch (dataItem.AgentObjectTypeId) {
                                    case 1: 
                                        result = "/#/kpi/professorRatingKPI/" + param;
                                        break;
                                    case 2:
                                        result = "/#/kpi/ratingKPI/" + param;
                                        break;
                                    case 3: 
                                        result = "/#/kpi/departmentRatingKPI/" + param;
                                        break;
                                    case 4: 
                                        result = "";
                                        break;
                                    case 5: 
                                        result = "/#/kpi/facultyRatingKPI/" + param;
                                        break;
                                    case 6: case 12: 
                                        result = "/#/kpi/subjectRatingKPI/" + param;
                                        break;
                                    case 7: 
                                        result = "/#/kpi/subDepartmentRatingKPI/" + param;
                                        break;
                                    case 8: 
                                        result = "/#/kpi/subFacultyRatingKPI/" + param;
                                        break;
                                    case 10:
                                        result = "/#/kpi/principalRatingKPI/" + param;
                                        break;
                                    case 11: 
                                        result = "/#/kpi/vicePrincipalRatingKPI/" + param;
                                        break;
                                }
                                if (result == "")
                                    return "";
                                return "<a href=" + result + ">Xem đánh giá</a>";
                            }
                        },
                    ],
                };
            }
    ]);
});