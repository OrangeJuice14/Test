
define(['app/app', 'app/services/kpi/planKPIService', 'app/services/kpi/StaffService', 'app/services/kpi/agentObjectService', 'app/services/kpi/ratingKPIService', 'app/services/kpi/departmentService', 'app/services/kpi/planKPIDetailService', 'app/services/kpi/planKPIDetailService', 'moment'], function (app) {
    "use strict";
    app.controller('planStaffDetailController', ['$scope', '$modal', '$rootScope', '$stateParams', 'planKPIService', 'agentObjectService', 'staffService', 'departmentService', 'ratingKPIService','planKPIDetailService',
    function ($scope, $modal, $rootScope, $stateParams, planKPIService, agentObjectService, staffService, departmentService, ratingKPIService, planKPIDetailService) {
        agentObjectService.getUserAgentObjectTypeId().then(function (result) {
            $scope.agentObjectTypeId = result.data;
        });
        var moment = require('moment');
        $scope.date = new Date();
        $scope.studyYearId = "";
        $scope.isSupervisor = 0;
        $scope.normalStaffId = MANAGER.GUID_EMPTY;
      //  $scope.planId = MANAGER.GUID_EMPTY;
        $scope.planType = 1;
        $scope.RatingStartTime = new Date();
        $scope.RatingEndTime = new Date();
        //planKPIService.getDateTime().then(function (result) {
        //    $scope.date = result.data;
        //});
        //alert($scope.date);
        //$scope.dataSource = new kendo.data.TreeListDataSource({
        //    transport: {
        //        read: function (options) {
        //            return planKPIService.getListByDepartment().then(function (result) {
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
        //    },
        //    //pageSize: 20
        //});

        var getSupervisorPromise = new Promise(function (resolve, reject) {
            staffService.getCheckIsSupervisor().then(function (result) {
                $scope.isSupervisor = result.data;
                resolve();
            });
        });

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
                        if ($scope.studyYearId == "")
                            $scope.studyYearId = result.data[result.data.length - 1].Id;
                    });
                }
            }
        });
        $scope.getListPlanKPIByStudyYear = function () {
            if ($scope.studyYearId != "") {
                $scope.dataSource = new kendo.data.TreeListDataSource({
                    transport: {
                        read: function (options) {
                            return planKPIService.getListByDepartment($scope.studyYearId).then(function (result) {
                                getSupervisorPromise.then(function () {
                                    options.success(result.data);
                                });
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

            ////planKPIService.getListByDepartment($scope.studyYearId).then(function (result) {
            ////    $.each(result.data, function (idx, item) {
            ////        $scope.planId = item.Id;
            ////        $scope.planType = item.PlanTypeId;
            ////        $scope.RatingStartTime = item.RatingStartTime;
            ////        $scope.RatingEndTime = item.RatingEndTime;
            ////        return;
            ////    })
            ////});

        });
     
        $scope.setSupervisor = function () {
            $rootScope.isSupervisor = 1;
        }
        $scope.dateCompare = function (date1, date2) {
            var result = -1;
            var isSame = moment(date1).isSame(date2, 'day');
            if (!isSame) {
                var isAfter = moment(date1).isAfter(date2, 'day');
                if (isAfter) {
                    result = 1;
                }
                else
                    result = -1;
            }
            else
                result = 0;
            return result;
        }

        //var templateEdit = "<span ng-switch on='#:data.PlanType #'>";
        //templateEdit += "<span ng-switch-when='1'><span ng-if='dateCompare(\"#:data.EndTime#\",date)>=0 && (agentObjectTypeId==3 || agentObjectTypeId==4 || agentObjectTypeId==5 || agentObjectTypeId==6 || agentObjectTypeId==100)'><a class='year' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanType #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span><span ng-if='dateCompare(\"#:data.EndTime#\",date)<0  || agentObjectTypeId==1 || agentObjectTypeId==2'>#:data.Name #</span></span>";
        //templateEdit += "<span ng-switch-when='2'><span ng-if='dateCompare(\"#:data.EndTime#\",date)>=0 && (agentObjectTypeId==1 || agentObjectTypeId==3 || agentObjectTypeId==4 || agentObjectTypeId==5 || agentObjectTypeId==6 || agentObjectTypeId==100 || agentObjectTypeId==99 || agentObjectTypeId==98)'><a class='semester' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanType #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span><span ng-if='dateCompare(\"#:data.EndTime#\",date)<0 || agentObjectTypeId==2'>#:data.Name #</span></span>";
        //templateEdit += "<span ng-switch-when='3'><span ng-if='dateCompare(\"#:data.EndTime#\",date)>=0 && (agentObjectTypeId==2 || agentObjectTypeId==3 || agentObjectTypeId==4 || agentObjectTypeId==5 || agentObjectTypeId==6 || agentObjectTypeId==7 || agentObjectTypeId==8 || agentObjectTypeId==9 || agentObjectTypeId==100 || agentObjectTypeId==99 || agentObjectTypeId==97)'><a  class='month' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanType #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span><span ng-if='dateCompare(\"#:data.EndTime#\",date)<0'>#:data.Name #</span></span></span>";

        var templateEdit = "<span ng-switch on='#:data.PlanTypeId #'>";
        templateEdit += "<span ng-switch-when='1'><a class='year' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanTypeId #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span>";
        //ng-click='Edit(\"#:data.Id #\",\"#:data.PlanTypeId #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'
        //templateEdit += "<span ng-switch-when='2'><span ><a class='semester' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanTypeId #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span></span>";
        //templateEdit += "<span ng-switch-when='3'><span ><a class='month' ng-click='Edit(\"#:data.Id #\",\"#:data.PlanTypeId #\",\"#:data.RatingStartTime #\",\"#:data.RatingEndTime #\")'>#:data.Name #</a></span></span>";

        var templateView = "<span ng-switch on='#:data.PlanTypeId #'>";
        templateView += "<span ng-switch-when='1'><span ><a class='year'> #:data.Name #</a></span></span>";
        //templateView += "<span ng-switch-when='2'><span ><a class='semester' ng-click='DepartmentPlan(\"#:data.Id #\")'>#:data.Name #</a></span></span>";
        //templateView += "<span ng-switch-when='3'><span ><a class='month' ng-click='DepartmentPlan(\"#:data.Id #\")'>#:data.Name #</a></span></span>";

        var templateDepartmentRating = "<span ng-switch on='#:data.PlanTypeId #'>";
        templateDepartmentRating += "<span ng-switch-when='1'><span ><a class='year' ng-click='DepartmentRating(\"#:data.Id #\")'>#:data.Name #</a></span></span>";
        //templateDepartmentRating += "<span ng-switch-when='2'><span ><a class='semester' ng-click='DepartmentRating(\"#:data.Id #\")'>#:data.Name #</a></span></span>";
        //templateDepartmentRating += "<span ng-switch-when='3'><span ><a class='month' ng-click='DepartmentRating(\"#:data.Id #\")'>#:data.Name #</a></span></span>";

        $scope.createWidget = false;
        $scope.grid = {};
        $scope.grids = {};
        $scope.isEdit = false;
        $scope.resultList = [];
        $scope.obj = null;
        $scope.mainGridOptions = {
            sortable: true,
            pageable: true,
            selectable: true,
            columns: [{
                template: templateEdit,
                width: "100px",
            }]
        };
        $scope.mainGridOptions2 = {
            sortable: true,
            pageable: true,
            selectable: true,
            columns: [{
                template: templateView,
                width: "100px",
            }]
        };
        $scope.mainGridOptions3 = {
            sortable: true,
            pageable: true,
            selectable: true,
            columns: [{
                template: templateDepartmentRating,
                width: "100px",
            }]
        };

        //------------ Lập kế hoach đánh giá -----------//
        $scope.Edit = function (Id, PlanType, ratingStartTime, ratingEndTime) {


            $scope.date = new Date();

            $scope.dateCompare = function (date1, date2) {
                var result = -1;
                var isSame = moment(date1).isSame(date2, 'day');
                if (!isSame) {
                    var isAfter = moment(date1).isAfter(date2, 'day');
                    if (isAfter) {
                        result = 1;
                    }
                    else
                        result = -1;
                }
                else
                    result = 0;
                return result;
            }
            $scope.setSupervisor = function () {
                $rootScope.isSupervisor = 1;
                $modalInstance.dismiss('cancel');
            }
            $scope.setSupervisor2 = function () {
                $rootScope.isSupervisor = 2; //thêm quyền chỉ xem kế hoạch
                $modalInstance.dismiss('cancel');
            }

            $scope.planId = Id;
            $scope.planType = PlanType;
            $scope.RatingStartTime = ratingStartTime;
            $scope.RatingEndTime = ratingEndTime;
            $scope.freeRating = false;
            $scope.canRating = ($scope.dateCompare($scope.RatingStartTime, $scope.date) <= 0 && $scope.dateCompare($scope.RatingEndTime, $scope.date) >= 0);

            if ($scope.canRating == false) {
                ratingKPIService.getCheckUnlockable($scope.planId).then(function (result) {
                    $scope.canRating = result.data;
                });
            }
            staffService.getCurrentStaff().then(function (result) {
                $scope.staffId = result.data.Id;
            });


            agentObjectService.getUserAgentObjectTypeId().then(function (result) {
                $scope.agentObjectTypeId = result.data;
                //Danh sách nhân viên, giảng viên
                staffService.getDepartmentStaff($scope.agentObjectTypeId, $scope.planId).then(function (result) {
                    //$scope.staffPlanKPIList = result.data;
                    if ($scope.agentObjectTypeId == 3) { //phòng ban
                        $scope.staffPlanKPIList = result.data.department;
                    } else if ($scope.agentObjectTypeId == 6) { //bộ môn
                        $scope.subjectProfessor = result.data.subjectProfessor; //danh sách giảng viên của bộ môn
                        $scope.subjectStaff = result.data.subjectStaff; //danh sách nhân viên của bộ môn
                    } else if ($scope.agentObjectTypeId == 5) { //khoa
                        $scope.professorListHasSubject = result.data.professorListHasSubject; //danh sách giảng viên có bộ môn (trưởng bộ môn đánh giá, trưởng khoa chỉ xem)
                        $scope.professorListSubjectNull = result.data.professorListSubjectNull; //danh sách giảng viên không thuộc bộ môn nào (trưởng khoa đánh giá)
                        $scope.staffListHasSubject = result.data.staffListHasSubject; //danh sách nhân viên có bộ môn (trưởng bộ môn đánh giá, trưởng khoa chỉ xem)
                        $scope.staffListSubjectNull = result.data.staffListSubjectNull; //danh sách nhân viên không bộ môn (trưởng khoa đánh giá)

                        //trưởng khoa có chức vụ kiêm nhiệm là trưởng bộ môn cùng khoa
                        //$scope.professorListHasSubject_SubPositionSubject = result.data.professorListHasSubject_SubPositionSubject;
                    }
                });
                // danh sách kế hoạc của chính người đó
                if ($scope.normalStaffId == MANAGER.GUID_EMPTY) {
                    planKPIService.getPlanListDepartment($scope.normalStaffId, $scope.planId).then(function (result) {
                        $scope.userPlanKPIList = result.data;
                    });
                }
                else {
                    planKPIService.getListByUserId($scope.normalStaffId).then(function (result) {
                        $scope.userPlanKPIList = result.data;
                    });
                }

                //Danh sách trưởng bộ môn, trưởng ngành
                staffService.getSubjectStaff($scope.agentObjectTypeId, $scope.planId).then(function (result) {
                    if ($scope.agentObjectTypeId == 5) {
                        $scope.subjectStaffList = result.data;
                    }
                });

                staffService.getViceDepartmentStaff($scope.agentObjectTypeId, MANAGER.GUID_EMPTY, $scope.planId).then(function (result) {
                    $scope.viceDepartmentStaffPlanKPIList = result.data; //Danh sách phó phòng ban, phó khoa
                    $scope.vicePrincipalStaffPlanKPIList = result.data; //Danh sách phó hiệu trưởng
                });

                //Danh sách nhân sự trong bộ phận kiêm nhiệm
                staffService.getStaffInSubPosition($scope.agentObjectTypeId, $scope.planId).then(function (result) {
                    $scope.staffListInSubPosition = [];
                    var keys = null;
                    keys = $.map(result.data, function (v, i) {
                        return i;
                    });
                    $(keys).each(function () {
                        //nếu có giảng viên thì hiện cột chế độ làm việc
                        var hasProfessor = false;
                        for (var i = 0, len = result.data[this].length; i < len; i++) {
                            if (result.data[this][i].AgentObjectTypeId == 1) {
                                hasProfessor = true;
                                break;
                            }
                        }
                        //nếu có người có chức vụ thì hiện cột chức vụ
                        var hasPosition = false;
                        for (var i = 0, len = result.data[this].length; i < len; i++) {
                            if (result.data[this][i].PositionName != "" && result.data[this][i].PositionName != null) {
                                hasPosition = true;
                                break;
                            }
                        }

                        $scope.staffListInSubPosition.push({
                            DepartmentName: this,
                            HasProfessor: hasProfessor,
                            HasPosition: hasPosition,
                            StaffList: result.data[this]
                        });
                    });
                });
            });

            $scope.LockWorkingModeProfessor = function (planstaffid) {
                planKPIDetailService.getLockWorkingModeProfessor(planstaffid).then(function (result) {
                    if (result.data == 1) {
                        if ($scope.subjectProfessor != undefined) {
                            $.each($scope.subjectProfessor, function (idx, item) {
                                if (item.PlanStaffId == planstaffid) {
                                    item.IsWorkingModeLocked = true;
                                    return;
                                }
                            });
                        }
                        if ($scope.professorListSubjectNull != undefined) {
                            $.each($scope.professorListSubjectNull, function (idx, item) {
                                if (item.PlanStaffId == planstaffid) {
                                    item.IsWorkingModeLocked = true;
                                    return;
                                }
                            });
                        }
                        if ($scope.professorListHasSubject_SubPositionSubject != undefined) {
                            $.each($scope.professorListHasSubject_SubPositionSubject, function (idx, item) {
                                if (item.PlanStaffId == planstaffid) {
                                    item.IsWorkingModeLocked = true;
                                    return;
                                }
                            });
                        }
                        if ($scope.staffListInSubPosition != undefined) {
                            $.each($scope.staffListInSubPosition, function (idx, item) {
                                if (item.PlanStaffId == planstaffid) {
                                    item.IsWorkingModeLocked = true;
                                    return;
                                }
                            });
                        }
                    }
                });
            }
            //------------- End chọn kế hoạch ----------------//
        }
        //    var modalInstance = $modal.open({
        //        animation: true,
        //        templateUrl: '/app/views/kpi/planKPIDetail/planManageDetail.html',
        //        controller: 'planManageDetailController',
        //        windowClass: 'planManageDetail-dialog',
        //        resolve: {
        //            id: function () {
        //                return Id;
        //            },
        //            planType: function () {
        //                return PlanType;
        //            },
        //            ratingStartTime: function () {
        //                return ratingStartTime;
        //            },
        //            ratingEndTime: function () {
        //                return ratingEndTime;
        //            }

        //        }
        //    }).result.then(function () {
        //        $scope.grid.dataSource.read();
        //    });
        //  };
        //-------------End------------//

        // ---------- Xem kế hoạch đơn vị--------------//
        $scope.dataSource2 = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    return departmentService.getDeptPlanOfUser().then(function (result) {
                        options.success(result.data);
                    });
                }
            }
        });
        $scope.dataSources = new kendo.data.DataSource({
            transport: {
                read: function (options) {
                    return departmentService.getMainDeptWithoutUserDept().then(function (result) {
                        options.success(result.data);
                    });
                }
            },
            group: { field: "DepartmentTypeName", dir: "desc" },
            sort: { field: "Name", dir: "asc" },
            pageSize: 15
        });

        $scope.DepartmentManagePlan = function (Id) {
            if (Id == '406f6e70-c3d9-49d8-b6f6-e39649ba8458') {
                staffService.getCurrentStaff().then(function (result2) {
                    $scope.normalStaffId = result2.data.Id;
                    //$rootScope.isAdmin = true;
                    $rootScope.isSupervisor = 1;
                    $modalInstance.close();
                    $state.go("schoolManagePlankpidetail", { planId: $scope.planId, agentObjectId: "4731710F-94FB-4EF5-9F48-05E12648AA62", normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1 });
                    //window.location = "#/kpi/schoolManagePlankpidetail/" + $scope.planId + "/4731710F-94FB-4EF5-9F48-05E12648AA62/" + $scope.normalStaffId + "/1";
                });
            }
            else {
                staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
                    $scope.agentObjectId = result.data;
                    staffService.getDepartmentLeaderId(Id).then(function (result2) {
                        $scope.normalStaffId = result2.data;
                        $modalInstance.close();
                        $rootScope.isSupervisor = 1;
                        $state.go("departmentManagePlankpidetail", { planId: $scope.planId, agentObjectId: $scope.agentObjectId, normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1, isConfig: 0 });
                        //window.location = "/kpi/departmentManagePlankpidetail/" + $scope.planId + "/" + $scope.agentObjectId + "/" + $scope.normalStaffId + "/1/0";
                    });
                });
            }
        }
        $scope.FacultyManagePlan = function (Id) {
            staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
                $scope.agentObjectId = result.data;
                staffService.getDepartmentLeaderId(Id).then(function (result2) {
                    $scope.normalStaffId = result2.data;
                    $modalInstance.close();
                    $rootScope.isSupervisor = 1;
                    $state.go('facultyManagePlankpidetail', { planId: $scope.planId, agentObjectId: $scope.agentObjectId, normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1, isConfig: 0 })
                    //window.location = "/kpi/facultyManagePlankpidetail/" + $scope.planId + "/" + $scope.agentObjectId + "/" + $scope.normalStaffId + "/1/0";
                });
            });
        }

        var templateEdits = "<div ng-switch on='#:data.DepartmentType #'>";
        templateEdits += "<div ng-switch-when='1'><div><a ng-click='DepartmentManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
        templateEdits += "<div ng-switch-when='4'><div><a ng-click='FacultyManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
        templateEdits += "<div ng-switch-default><div><a ng-click='OtherManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div></div>";

        $scope.mainGridOptions4 = {
            selectable: "row",
            columns: [{
                template: templateEdits,
                field: "Name",
                title: "Đơn vị của cá nhân"
            }]
        };
        $scope.mainGridOptions5 = {
            sortable: {
                mode: "multiple",
                allowUnsort: true
            },
            pageable: true,
            groupable: true,
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
            selectable: "row",
            columns: [{
                template: templateEdits,
                field: "Name",
                title: "Đơn vị"
            },
             {
                 field: "DepartmentTypeName",
                 title: "Loại đơn vị",
                 width: "30%"
             }]
        };
        // ---------------- End -------------//
        $scope.DepartmentRating = function (Id) {
            if (Id == "") {
                alert("Bạn chưa chọn phần tử");
                return;
            }

            $scope.planId = Id;
            $scope.title = "Chi tiết kế hoạch";

            var getCurrentStaffPromise = new Promise(function (resolve, reject) {
                staffService.getCurrentStaff().then(function (result) {
                    $scope.staffId = result.data.Id;
                    resolve();
                });
            });
            getCurrentStaffPromise.then(function () {
                staffService.getListDepartmentLeader($scope.planId).then(function (result) {
                    $scope.staffList = result.data;
                });
            });

            //var modalInstance = $modal.open({
            //    animation: true,
            //    templateUrl: '/app/views/kpi/planKPI/departmentRating.html',
            //    controller: 'ratingDepartmentController',
            //    resolve: {
            //        id: function () {
            //            return Id;
            //        }
            //    }
            //}).result.then(function () {
            //    $scope.grid.dataSource.read();
            //});
        };


    }
    ]);
    // ----- Không dùng  nữa -----//
    //app.controller('planManageDetailController', ['$rootScope', '$scope', '$modalInstance', 'id', 'planType', 'ratingStartTime', 'ratingEndTime', '$stateParams', 'planKPIService', 'agentObjectService', 'staffService', 'ratingKPIService', 'planKPIDetailService',
    //    function ($rootScope, $scope, $modalInstance, id, planType, ratingStartTime, ratingEndTime, stateParams, planKPIService, agentObjectService, staffService, ratingKPIService, planKPIDetailService) {
    //        var moment = require('moment');
    //        $scope.date = new Date();


    //        $scope.dateCompare = function (date1, date2) {
    //            var result = -1;
    //            var isSame = moment(date1).isSame(date2, 'day');
    //            if (!isSame) {
    //                var isAfter = moment(date1).isAfter(date2, 'day');
    //                if (isAfter) {
    //                    result = 1;
    //                }
    //                else
    //                    result = -1;
    //            }
    //            else
    //                result = 0;
    //            return result;
    //        }
    //        $scope.setSupervisor = function () {
    //            $rootScope.isSupervisor = 1;
    //            $modalInstance.dismiss('cancel');
    //        }
    //        $scope.setSupervisor2 = function () {
    //            $rootScope.isSupervisor = 2; //thêm quyền chỉ xem kế hoạch 
    //            $modalInstance.dismiss('cancel');
    //        }
    //        $scope.normalStaffId = MANAGER.GUID_EMPTY;
    //        $scope.planId = id;
    //        $scope.planType = planType;
    //        $scope.freeRating = false;
    //        $scope.canRating = ($scope.dateCompare(ratingStartTime, $scope.date) <= 0 && $scope.dateCompare(ratingEndTime, $scope.date) >= 0);
    //        if ($scope.canRating == false) {
    //            ratingKPIService.getCheckUnlockable($scope.planId).then(function (result) {
    //                $scope.canRating = result.data;
    //            });
    //        }
    //        staffService.getCurrentStaff().then(function (result) {
    //            $scope.staffId = result.data.Id;
    //        });



    //        if ($scope.normalStaffId == MANAGER.GUID_EMPTY) {
    //            planKPIService.getPlanListDepartment($scope.normalStaffId, id).then(function (result) {
    //                $scope.userPlanKPIList = result.data;
    //            });
    //        }
    //        else {
    //            planKPIService.getListByUserId($scope.normalStaffId).then(function (result) {
    //                $scope.userPlanKPIList = result.data;
    //            });
    //        }

    //        agentObjectService.getUserAgentObjectTypeId().then(function (result) {
    //            $scope.agentObjectTypeId = result.data;
    //            //Danh sách nhân viên, giảng viên
    //            staffService.getDepartmentStaff($scope.agentObjectTypeId, $scope.planId).then(function (result) {
    //                //$scope.staffPlanKPIList = result.data;
    //                if ($scope.agentObjectTypeId == 3) { //phòng ban
    //                    $scope.staffPlanKPIList = result.data.department;
    //                } else if ($scope.agentObjectTypeId == 6) { //bộ môn
    //                    $scope.subjectProfessor = result.data.subjectProfessor; //danh sách giảng viên của bộ môn
    //                    $scope.subjectStaff = result.data.subjectStaff; //danh sách nhân viên của bộ môn
    //                } else if ($scope.agentObjectTypeId == 5) { //khoa
    //                    $scope.professorListHasSubject = result.data.professorListHasSubject; //danh sách giảng viên có bộ môn (trưởng bộ môn đánh giá, trưởng khoa chỉ xem)
    //                    $scope.professorListSubjectNull = result.data.professorListSubjectNull; //danh sách giảng viên không thuộc bộ môn nào (trưởng khoa đánh giá)
    //                    $scope.staffListHasSubject = result.data.staffListHasSubject; //danh sách nhân viên có bộ môn (trưởng bộ môn đánh giá, trưởng khoa chỉ xem)
    //                    $scope.staffListSubjectNull = result.data.staffListSubjectNull; //danh sách nhân viên không bộ môn (trưởng khoa đánh giá)

    //                    //trưởng khoa có chức vụ kiêm nhiệm là trưởng bộ môn cùng khoa
    //                    //$scope.professorListHasSubject_SubPositionSubject = result.data.professorListHasSubject_SubPositionSubject;
    //                }
    //            });
    //            //Danh sách trưởng bộ môn, trưởng ngành
    //            staffService.getSubjectStaff($scope.agentObjectTypeId, $scope.planId).then(function (result) {
    //                if ($scope.agentObjectTypeId == 5) {
    //                    $scope.subjectStaffList = result.data;
    //                }
    //            });

    //            staffService.getViceDepartmentStaff($scope.agentObjectTypeId, MANAGER.GUID_EMPTY, $scope.planId).then(function (result) {
    //                $scope.viceDepartmentStaffPlanKPIList = result.data; //Danh sách phó phòng ban, phó khoa
    //                $scope.vicePrincipalStaffPlanKPIList = result.data; //Danh sách phó hiệu trưởng
    //            });

    //            //Danh sách nhân sự trong bộ phận kiêm nhiệm
    //            staffService.getStaffInSubPosition($scope.agentObjectTypeId, $scope.planId).then(function (result) {
    //                $scope.staffListInSubPosition = [];
    //                var keys = null;
    //                keys = $.map(result.data, function (v, i) {
    //                    return i;
    //                });
    //                $(keys).each(function () {
    //                    //nếu có giảng viên thì hiện cột chế độ làm việc
    //                    var hasProfessor = false;
    //                    for (var i = 0, len = result.data[this].length; i < len; i++) {
    //                        if (result.data[this][i].AgentObjectTypeId == 1) {
    //                            hasProfessor = true;
    //                            break;
    //                        }
    //                    }
    //                    //nếu có người có chức vụ thì hiện cột chức vụ
    //                    var hasPosition = false;
    //                    for (var i = 0, len = result.data[this].length; i < len; i++) {
    //                        if (result.data[this][i].PositionName != "" && result.data[this][i].PositionName != null) {
    //                            hasPosition = true;
    //                            break;
    //                        }
    //                    }

    //                    $scope.staffListInSubPosition.push({
    //                        DepartmentName: this,
    //                        HasProfessor: hasProfessor,
    //                        HasPosition: hasPosition,
    //                        StaffList: result.data[this]
    //                    });
    //                });
    //            });
    //        });

    //        $scope.LockWorkingModeProfessor = function (planstaffid) {
    //            planKPIDetailService.getLockWorkingModeProfessor(planstaffid).then(function (result) {
    //                if (result.data == 1) {
    //                    if ($scope.subjectProfessor != undefined) {
    //                        $.each($scope.subjectProfessor, function (idx, item) {
    //                            if (item.PlanStaffId == planstaffid) {
    //                                item.IsWorkingModeLocked = true;
    //                                return;
    //                            }
    //                        });
    //                    }
    //                    if ($scope.professorListSubjectNull != undefined) {
    //                        $.each($scope.professorListSubjectNull, function (idx, item) {
    //                            if (item.PlanStaffId == planstaffid) {
    //                                item.IsWorkingModeLocked = true;
    //                                return;
    //                            }
    //                        });
    //                    }
    //                    if ($scope.professorListHasSubject_SubPositionSubject != undefined) {
    //                        $.each($scope.professorListHasSubject_SubPositionSubject, function (idx, item) {
    //                            if (item.PlanStaffId == planstaffid) {
    //                                item.IsWorkingModeLocked = true;
    //                                return;
    //                            }
    //                        });
    //                    }
    //                    if ($scope.staffListInSubPosition != undefined) {
    //                        $.each($scope.staffListInSubPosition, function (idx, item) {
    //                            if (item.PlanStaffId == planstaffid) {
    //                                item.IsWorkingModeLocked = true;
    //                                return;
    //                            }
    //                        });
    //                    }
    //                }
    //            });
    //        }

    //        $scope.cancel = function () {
    //            $modalInstance.dismiss('cancel');
    //        };
    //    }
    //]);
    //    app.controller('planDepartmentController', ['$rootScope','$scope','$state', '$modalInstance', 'id', 'planKPIService', 'agentObjectService', 'departmentService', 'staffService',
    //       function ($rootScope, $scope,$state, $modalInstance, id, planKPIService, agentObjectService, departmentService, staffService) {
    //           $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
    //           $scope.planId = id;       
    //           $scope.title = "Chi tiết kế hoạch";
    //           $scope.obj = {};
    //           $scope.cancel = function () {
    //               $modalInstance.dismiss('cancel');
    //           };
    //           $scope.dataSource2 = new kendo.data.DataSource({
    //               transport: {
    //                   read: function (options) {
    //                       return departmentService.getDeptPlanOfUser().then(function (result) {
    //                           options.success(result.data);
    //                       });
    //                   }
    //               }
    //           });
    //           $scope.dataSource = new kendo.data.DataSource({
    //               transport: {
    //                   read: function (options) {
    //                       return departmentService.getMainDeptWithoutUserDept().then(function (result) {
    //                           options.success(result.data);
    //                       });
    //                   }
    //               },
    //               group: { field: "DepartmentTypeName", dir: "desc" },
    //               sort: { field: "Name", dir: "asc" },
    //               pageSize: 15
    //           });
    //           var templateEdit = "<div ng-switch on='#:data.DepartmentType #'>";
    //           templateEdit += "<div ng-switch-when='1'><div><a ng-click='DepartmentManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
    //           templateEdit += "<div ng-switch-when='4'><div><a ng-click='FacultyManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div>";
    //           templateEdit += "<div ng-switch-default><div><a ng-click='OtherManagePlan(\"#:data.Id #\")' href='javascript:void(0)'>#:data.Name #</a></div></div></div>";
    //           $scope.DepartmentManagePlan = function (Id) {
    //               if (Id == '406f6e70-c3d9-49d8-b6f6-e39649ba8458')
    //               {
    //                       staffService.getCurrentStaff().then(function (result2) {
    //                           $scope.normalStaffId = result2.data.Id;
    //                           //$rootScope.isAdmin = true;
    //                           $rootScope.isSupervisor = 1;
    //                           $modalInstance.close();
    //                           $state.go("schoolManagePlankpidetail", { planId: $scope.planId, agentObjectId: "4731710F-94FB-4EF5-9F48-05E12648AA62", normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1 });
    //                           //window.location = "#/kpi/schoolManagePlankpidetail/" + $scope.planId + "/4731710F-94FB-4EF5-9F48-05E12648AA62/" + $scope.normalStaffId + "/1";
    //                       });                  
    //               }
    //               else
    //               {
    //                   staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
    //                   $scope.agentObjectId = result.data;
    //                   staffService.getDepartmentLeaderId(Id).then(function (result2) {
    //                       $scope.normalStaffId = result2.data;
    //                       $modalInstance.close();
    //                       $rootScope.isSupervisor = 1;
    //                       $state.go("departmentManagePlankpidetail", { planId: $scope.planId, agentObjectId: $scope.agentObjectId, normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1, isConfig: 0 });
    //                       //window.location = "/kpi/departmentManagePlankpidetail/" + $scope.planId + "/" + $scope.agentObjectId + "/" + $scope.normalStaffId + "/1/0";
    //                   });
    //               });
    //               }              
    //           }
    //           $scope.FacultyManagePlan = function (Id) {
    //               staffService.getDepartmentLeaderAgentObjectId(Id).then(function (result) {
    //                   $scope.agentObjectId = result.data;
    //                   staffService.getDepartmentLeaderId(Id).then(function (result2) {
    //                       $scope.normalStaffId = result2.data;
    //                       $modalInstance.close();
    //                       $rootScope.isSupervisor = 1;
    //                       $state.go('facultyManagePlankpidetail', { planId: $scope.planId, agentObjectId: $scope.agentObjectId, normalStaffId: $scope.normalStaffId, departmentId: Id, isSupervisor: 1, isConfig: 0 })
    //                       //window.location = "/kpi/facultyManagePlankpidetail/" + $scope.planId + "/" + $scope.agentObjectId + "/" + $scope.normalStaffId + "/1/0";
    //                   });
    //               });
    //           }
    //           $scope.OtherManagePlan = function (Id) {
    //           }
    //           $scope.mainGridOptions = {
    //               sortable: {
    //                   mode: "multiple",
    //                   allowUnsort: true
    //               },
    //               pageable: true,
    //               groupable: true,
    //               filterable: {
    //                   messages: {
    //                       info: "Lọc bởi : ",
    //                       filter: "Lọc",
    //                       clear: "Xóa"
    //                   },
    //                   extra: false,
    //                   operators: {
    //                       string: {
    //                           contains: "Từ khóa"
    //                       }
    //                   }
    //               },
    //               selectable: "row",
    //               columns: [{
    //                   template: templateEdit,
    //                   field: "Name",
    //                   title: "Đơn vị"
    //               },
    //                {
    //                    field: "DepartmentTypeName",
    //                    title: "Loại đơn vị",
    //                    width: "30%"
    //                }]
    //           };
    //           $scope.mainGridOptions2 = {
    //               selectable: "row",
    //               columns: [{
    //                   template: templateEdit,
    //                   field: "Name",
    //                   title: "Đơn vị của cá nhân"
    //               }]
    //           };
    //       }
    //    ]);
    //    app.controller('ratingDepartmentController', ['$rootScope', '$scope', '$state', '$modalInstance', 'id', 'planKPIService', 'agentObjectService', 'departmentService', 'staffService',
    //       function ($rootScope, $scope, $state, $modalInstance, id, planKPIService, agentObjectService, departmentService, staffService) {
    //           $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
    //           $scope.planId = id;
    //           $scope.title = "Chi tiết kế hoạch";
    //           $scope.obj = {};
    //           $scope.cancel = function () {
    //               $modalInstance.dismiss('cancel');
    //           };

    //           var getCurrentStaffPromise = new Promise(function (resolve, reject) {
    //               staffService.getCurrentStaff().then(function (result) {
    //                   $scope.staffId = result.data.Id;
    //                   resolve();
    //               });
    //           });
    //           getCurrentStaffPromise.then(function () {
    //               staffService.getListDepartmentLeader($scope.planId).then(function (result) {
    //                   $scope.staffList = result.data;
    //               });
    //           });
    //       }
    //    ]);
});