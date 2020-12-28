define(['app/app', 'app/services/kpi/StaffService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService', 'app/services/kpi/ratingKPIService', 'app/services/kpi/planKPIService', 'app/services/kpi/reportService', 'moment'], function (app) {
    "use strict";

    app.controller('reportController', ['$scope', '$modal', '$rootScope', '$state', '$stateParams', 'staffService', 'departmentService', 'ratingKPIService', 'planKPIService', 'reportService',
        function ($scope, $modal, $rootScope, $state, $stateParams, staffService, departmentService, ratingKPIService, planKPIService, reportService) {
            $scope.planId = MANAGER.GUID_EMPTY;
            $scope.deptId = MANAGER.GUID_EMPTY;
            var moment = require('moment');
            $scope.studyYearId = "";
            $scope.show = 0;

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
                    $scope.plansDataSource = new kendo.data.TreeListDataSource({
                        transport: {
                            read: function (options) {
                                return planKPIService.getListByDepartment($scope.studyYearId).then(function (result) {
                                    options.success(result.data);
                                    $scope.planId = result.data[0].Id;
                                });
                            }
                        },
                        schema: {
                            model: {
                                id: "Id",
                                fields: {
                                    parentId: { field: "ParentId", nullable: true },
                                    Id: { field: "Id", type: "string" }
                                },
                                expanded: true
                            }
                        }
                    });
                }
            }
            $scope.$watch('studyYearId', function (newVal, oldVal) {
                $scope.getListPlanKPIByStudyYear();
            });

            $scope.departmentsDataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        return departmentService.getList().then(function (result) {
                            var list = result.data;
                            list.unshift({ Id: MANAGER.GUID_EMPTY, Name: 'Tất cả' });
                            options.success(list);
                        });
                    },
                }
            });
            //$scope.departmentsDataSource = {
            //    dataTextField: "Name",
            //    dataValueField: "Id",
            //    valuePrimitive: true,
            //    autoBind: false,
            //    filter: "contains",
            //    dataSource: {
            //        transport: {
            //            read: function (options) {
            //                return departmentService.getList().then(function (result) {
            //                    options.success(result.data);
            //                });
            //            },
            //        }
            //    }
            //};
            $scope.SearchAll = function () {
                $scope.show = 1;
                $scope.dataSourceAll = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return reportService.getReportTotalDepartmentResult($scope.planId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
            };
            $scope.SearchDetail = function () {
                $scope.show = 2;
                $scope.dataSourceDetail = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return reportService.getReportDepartmentStaffResult($scope.planId, $scope.deptId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
            };
            $scope.ExportExcel = function () {
                if ($scope.show == 1)
                    $scope.gridAll.saveAsExcel();
                else if ($scope.show == 2)
                    $scope.gridDetail.saveAsExcel();
            };
            $scope.mainGridOptionsAll = {
                sortable: true,
                pageable: {
                    buttonCount: 7
                },
                excel: {
                    allPages: true
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
                            var index = $scope.dataSourceAll.indexOf(dataItem) + 1;
                            return "<p style='text-align:center; margin: 0'>" + index + "</p>";
                        },
                        width: 45
                    },
                    {
                        field: "TenBoPhan",
                        title: "Tên bộ phận"
                    },
                    {
                        field: "TenBoMon",
                        title: "Tên bộ môn"
                    },
                    {
                        field: "SoNV",
                        title: "SL NV"
                    },
                    {
                        field: "BoPhanKhoa",
                        title: "Bộ phận khóa",
                        template: function (dataItem) {
                            if (dataItem.BoPhanKhoa == true)
                                return "<input type='checkbox' checked='checked' disabled />";
                            else return "<input type='checkbox' disabled />";
                        }
                    },
                    {
                        field: "BoMonKhoa",
                        title: "Bộ môn khóa",
                        template: function (dataItem) {
                            if (dataItem.BoMonKhoa == true)
                                return "<input type='checkbox' checked='checked' disabled />";
                            else return "<input type='checkbox' disabled />";
                        }
                    },
                    {
                        field: "TruongDVDaDanhGia",
                        title: "Trưởng ĐV đánh giá",
                        template: function (dataItem) {
                            if (dataItem.TruongDVDaDanhGia == true)
                                return "<input type='checkbox' checked='checked' disabled />";
                            else return "<input type='checkbox' disabled />";
                        }
                    },
                    {
                        field: "NVDaDanhGia",
                        title: "Cá nhân đánh giá",
                        template: function (dataItem) {
                            if (dataItem.NVDaDanhGia == true)
                                return "<input type='checkbox' checked='checked' disabled />";
                            else return "<input type='checkbox' disabled />";
                        }
                    },
                ],
            };
            $scope.mainGridOptionsDetail = {
                sortable: true,
                pageable: {
                    buttonCount: 7
                },
                excel:{
                    allPages: true
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
                            var index = $scope.dataSourceDetail.indexOf(dataItem) + 1;
                            return "<p style='text-align:center; margin: 0'>" + index + "</p>";
                        },
                        width: 45
                    },
                    {
                        field: "TenBoPhan",
                        title: "Tên bộ phận"
                    },
                    {
                        field: "HoTen",
                        title: "Họ tên"
                    },
                    {
                        field: "TruongDonViDuyet",
                        title: "Trưởng ĐV đã duyệt",
                        template: function (dataItem) {
                            if (dataItem.TruongDonViDuyet == true)
                                return "<input type='checkbox' checked='checked' disabled />";
                            else return "<input type='checkbox' disabled />";
                        }
                    },
                    {
                        field: "CaNhanDanhGia",
                        title: "Cá nhân đánh giá",
                        template: function (dataItem) {
                            if (dataItem.CaNhanDanhGia == true)
                                return "<input type='checkbox' checked='checked' disabled />";
                            else return "<input type='checkbox' disabled />";
                        }
                    },
                    {
                        field: "TruongDonViDanhGia",
                        title: "Trưởng ĐV đánh giá",
                        template: function (dataItem) {
                            if (dataItem.TruongDonViDanhGia == true)
                                return "<input type='checkbox' checked='checked' disabled />";
                            else return "<input type='checkbox' disabled />";
                        }
                    }
                ],
            };

            $scope.StaffExportToExcel = function () {
                window.open("/ExcelExport/KPIReportExport.ashx?planId=" + $scope.planId + "&option=" + 1);
            }

            $scope.DepartmentExportToExcel = function () {
                window.open("/ExcelExport/KPIReportExport.ashx?planId=" + $scope.planId + "&option=" + 2);
            }
            $scope.PlanStaffExportToExcel = function () {
                window.open("/ExcelExport/KPIReportExport.ashx?planId=" + $scope.planId + "&option=" + 3);
            }
        }
    ]);
});