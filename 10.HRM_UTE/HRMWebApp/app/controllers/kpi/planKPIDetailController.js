
define(['app/app', 'app/services/kpi/planKPIDetailService', 'app/services/kpi/planKPIService', 'app/services/kpi/targetGroupDetailService', 'app/services/kpi/criterionService', 'app/services/kpi/authService', 'app/services/kpi/staffService', 'app/services/kpi/departmentService', 'app/services/kpi/fileAttachmentService', 'app/services/kpi/professorCriterionService', 'app/services/kpi/agentObjectService', 'moment', 'app/controllers/kpi/planKPIDetailInformationController', 'app/directives/directives', 'scripts/jquery.fixedheadertable'], function (app) {
    "use strict";
    app.controller('planKPIDetailController', ['$scope', '$sce', '$window', '$modal', '$rootScope', '$state', '$stateParams', '$timeout', 'planKPIDetailService', 'planKPIService', 'targetGroupDetailService', 'criterionService', 'authService', 'staffService', 'departmentService', 'fileAttachmentService', 'agentObjectService', 'professorCriterionService',
            function ($scope, $sce, $window, $modal, $rootScope, $state, $stateParams, $timeout, planKPIDetailService, planKPIService, targetGroupDetailService, criterionService, authService, staffService, departmentService, fileAttachmentService, agentObjectService, professorCriterionService, moment) {
                $("#sidebar").addClass("menu-compact");
                var moment = require('moment');

                $scope.departmentLeaderId = "";
                $scope.departmentLeaderName = "";
                $scope.IsLoading = false;
                $scope.isDisable = true;
                $scope.isExpired = false;
                $scope.isLock = true;
                $scope.InformationUrl = "";
                $scope.planType = 0;
                $scope.IsMoved = true;
                $scope.CapMucTieu = 0;

                $scope.TABLECOLORS = MANAGER.TABLECOLORS;
                $scope.planId = $stateParams.planId;
                $scope.agentObjectId = $stateParams.agentObjectId;
                $scope.isSupervisor = $rootScope.isSupervisor ? $rootScope.isSupervisor : 0;// $stateParams.isSupervisor;

                if ($stateParams.isSupervisor != null && $stateParams.isSupervisor != undefined) {
                    $scope.isSupervisor = $stateParams.isSupervisor;
                }

                $scope.supervisorId = $stateParams.supervisorId;
                $scope.isConfig = $stateParams.isConfig;
                $scope.normalStaffId = $stateParams.normalStaffId != "" ? $stateParams.normalStaffId : MANAGER.GUID_EMPTY;
                $scope.IsSupervisorPlaning = false;
                $scope.SubDepartmentIds = [];
                $scope.MeasureUnits = [];
                $scope.isAdmin = 0;
                $scope.userRole = $state.current.name == "schoolManagePlankpidetail" ? 1 : 2;

                $scope.agentObjectTypeId = 0;
                $scope.departmentId = MANAGER.GUID_EMPTY;
                $scope.departmentName = "";
                $scope.staffLeaderId = MANAGER.GUID_EMPTY;
                $scope.staffLeaderName = "";
                $scope.planName = "kế hoạch";
                $scope.criterionList = [];
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.isEdit = false;
                $scope.resultList = null;
                $scope.obj = null;
                $scope.criterionList = {};
                $scope.startPlanTime = "";
                $scope.endPlanTime = "";
                $scope.workingModeLocked = true;
                $scope.LeaderLockedWorkingModeString = "";
                $scope.staffPlanDetailMakingObject = {};


                $scope.options = {
                    format: "n1",
                    // decimals: 1
                }
                $scope.UpdateVision = function () {
                    planKPIDetailService.saveVision($scope.staffPlanDetailMakingObject).then(function (result) {
                        //if (result == 1)
                        //{
                        //    alert("Lưu thành công");
                        //}
                        //else
                        //{
                        //    alert("Thất bại");
                        //}
                    });
                }
                $scope.UpdateMission = function () {
                    planKPIDetailService.saveMission($scope.staffPlanDetailMakingObject).then(function (result) {
                        //if (result == 1) {
                        //    alert("Lưu thành công");
                        //}
                        //else {
                        //    alert("Thất bại");
                        //}
                    });
                }
                $scope.downloadFile = function (Id) {
                    fileAttachmentService.downloadFile(Id);


                }

                function UpdatePlanDetailCache() {
                    planKPIDetailService.updatePlanDetailCache();
                }

                //$scope.UpdatePlanDetailCache = function () {
                //    planKPIDetailService.updatePlanDetailCache();
                //}

                $scope.dateChange = function (e) {
                    var startDate = e.sender;
                    var isValid = moment(startDate.value()).isBetween($scope.startPlanTime, $scope.endPlanTime);
                    //if (!isValid)
                    //{
                    //    alert("Nằm ngoài khoảng thời gian kế hoạch!");
                    //    //startDate.value(null);
                    //}   
                }

                planKPIDetailService.getMeasureUnits().then(function (result) {
                    $scope.MeasureUnits = result.data;
                });
                staffService.getCurrentUserGroupId().then(function (result) {
                    var groupId = result.data;
                    if ((groupId == "00000000-0000-0000-0000-000000000001") || (groupId == "00000000-0000-0000-0000-000000000002") || (groupId == "00000000-0000-0000-0000-000000000003")) {
                        $scope.isAdmin = 1;
                    }
                });

                staffService.getCurrentStaff().then(function (result) {
                    $scope.staffId = result.data.Id;
                });
                planKPIService.getObj($scope.planId, $scope.agentObjectTypeId).then(function (result) {
                    $scope.planName = result.data.Name;
                });

                function createTabHeader() {
                    setTimeout(function () {
                        $.each($('.planTable'), function (index, item) {
                            $(item).fixedHeaderTable({ cloneHeadToFoot: true, height: screen.height - 130 });
                        });
                        //$('selector').fixedHeaderTable({ height: 500 });
                        //var  planHeader = $('.planHeader');
                        var headerOffset = 500;
                        $("#mainTabId").kendoTabStrip({
                            navigatable: false
                        }).data("kendoTabStrip");
                        $("#mainTabId").removeAttr("tabindex");

                        var isFirefox = typeof InstallTrigger !== 'undefined';
                        if (isFirefox == true) {
                            var contentWidth = document.getElementById('infoTable').clientWidth;
                            $("#inner").width(contentWidth);
                            $(".fht-table-wrapper").width(contentWidth);
                        }

                    }, 100);

                }

                function getPlanDetailList() {
                    $scope.IsLoading = true;
                    planKPIDetailService.getPlanMakingDetail($scope.planId, $scope.agentObjectId, $scope.normalStaffId, $stateParams.departmentId, $scope.userRole, $scope.isSupervisor).then(function (result) {
                        if (result.data.Id == MANAGER.GUID_EMPTY && result.data.StaffDTO.AgentObjectTypeId == 1 && result.data.PlanTypeId == 2) {
                            alert("Chưa soạn kế hoạch năm");
                            window.location = "/#/kpi/userPlanKPI";
                        }
                        if (result.data.Id == '00000000-0000-0000-0000-000000000001') {
                            alert("Người dùng chưa có tài khoản đăng nhập!")
                            window.history.back();
                            return;
                        }
                        $scope.staffPlanDetailMakingObject = result.data;
                        $scope.isDisable = $scope.staffPlanDetailMakingObject.IsDisable;
                        $scope.startPlanTime = moment($scope.staffPlanDetailMakingObject.StartPlanTime).format('DD/MM/YYYY');
                        $scope.endPlanTime = moment($scope.staffPlanDetailMakingObject.EndPlanTime).format('DD/MM/YYYY');
                        $scope.startPlan = $scope.staffPlanDetailMakingObject.StartPlanTime;
                        $scope.endPlan = $scope.staffPlanDetailMakingObject.EndPlanTime;
                        var currentDate = moment(new Date());
                        var endPlanTime = moment($scope.endPlan);
                        //if (currentDate.isAfter(endPlanTime, 'day'))
                        //    $scope.isExpired = true; //tạm thời bỏ quá thời hạn
                        $scope.departmentId = result.data.StaffDTO.Department.Id;
                        $scope.departmentName = result.data.StaffDTO.Department.Name;
                        $scope.staffLeaderId = result.data.StaffDTO.Id;
                        $scope.staffLeaderName = result.data.StaffDTO.Name;
                        $scope.agentObjectTypeId = result.data.StaffDTO.AgentObjectTypeId;

                        $scope.planType = result.data.PlanTypeId;
                        switch ($scope.agentObjectTypeId) {
                            case 1:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/professorDetail.html";
                                break;
                            case 2:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/staffDetail.html";
                                break;
                            case 3:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/departmentDetail.html";
                                break;
                            case 4:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/manage.html";
                                break;
                            case 5:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/facultyDetail.html";
                                break;
                            case 6:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/subjectDetail.html";
                                break;
                            case 7:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/subDepartmentDetail.html";
                                break;
                            case 8:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/subFacultyDetail.html";
                                break;
                            case 9:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/subSubjectDetail.html";
                                break;
                            case 10:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/principalDetail.html";
                                break;
                            case 11:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/vicePrincipalDetail.html";
                                break;
                            case 12:
                                $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/subjectDetail.html";
                                break;
                        }

                        if ($scope.userRole == 1) {
                            $scope.InformationUrl = "/app/views/kpi/planKPIDetailInformation/manage.html";
                            $scope.staffPlanDetailMakingObject.StaffDTO.AgentObjectTypeId = 4;
                            $scope.agentObjectTypeId = 4;
                        }

                        //staffService.getStaffByAgentObjectType($scope.agentObjectTypeId, $scope.agentObjectTypeId == 4 ? MANAGER.GUID_EMPTY : result.data.StaffDTO.DepartmentId, $scope.userRole).then(function (result) {
                        //    $scope.StaffLeaders = result.data;
                        //    $.each(result.data, function (idx, item) {
                        //        if (item.Position.Name == "Trưởng Phòng") {
                        //            $scope.departmentLeaderId = item.Id;
                        //            $scope.departmentLeaderName = item.Name;
                        //        }

                        //    });
                        //});

                        //staffService.getDepartmentLeader($scope.agentObjectTypeId == 4 ? MANAGER.GUID_EMPTY : result.data.StaffDTO.DepartmentId).then(function (result) {

                        //    $scope.departmentLeaderId = result.data.Id;
                        //    $scope.departmentLeaderName = result.data.Name;
                        //});

                        staffService.getDirectLeader($scope.agentObjectTypeId == 4 ? MANAGER.GUID_EMPTY : result.data.StaffDTO.DepartmentId, $scope.agentObjectId, $scope.normalStaffId).then(function (result) {
                            $scope.departmentLeaderId = result.data.Id;
                            $scope.departmentLeaderName = result.data.Name;
                            //Nếu null thì gán bằng ''
                            if (!$scope.departmentLeaderName) {
                                $scope.departmentLeaderName = "";
                            }
                        });

                        planKPIDetailService.getWorkingModeByPlanStaff($scope.staffPlanDetailMakingObject.PlanStaffId).then(function (result) {
                            if (result.data != null)
                                $scope.agentObjectDetail = result.data;
                            else $scope.agentObjectDetail = { Id: MANAGER.GUID_EMPTY };
                        });

                        planKPIDetailService.getCheckIsLockWorkingMode($scope.staffPlanDetailMakingObject.PlanStaffId).then(function (result) {
                            $scope.workingModeLocked = result.data;
                            if (result.data == true)
                                $scope.LeaderLockedWorkingModeString = "(Cấp trên đã duyệt)";
                        });

                        $scope.viceDepartmentStaffResource = {
                            placeholder: "Chọn Phó khoa...",
                            dataTextField: "Name",
                            dataValueField: "Id",
                            valuePrimitive: true,
                            filter: "contains",
                            autoBind: false,
                            dataSource: {
                                transport: {
                                    read: function (options) {
                                        return staffService.getViceDepartmentStaff($scope.agentObjectTypeId, $scope.agentObjectTypeId == 4 ? MANAGER.GUID_EMPTY : result.data.StaffDTO.DepartmentId).then(function (result) {
                                            options.success(result.data);
                                        });
                                    }
                                }
                            }
                        };
                        $scope.professorInSubjectResource = {
                            placeholder: "Chọn Giảng viên ...",
                            dataTextField: "Name",
                            dataValueField: "Id",
                            valuePrimitive: true,
                            filter: "contains",
                            autoBind: false,
                            dataSource: {
                                transport: {
                                    read: function (options) {
                                        return staffService.getProfessorInSubject($scope.agentObjectTypeId).then(function (result) {
                                            options.success(result.data);
                                        });
                                    }
                                }
                            }
                        };
                        $scope.subjectDepartmentsResource = {
                            placeholder: "Chọn bộ môn...",
                            dataTextField: "Name",
                            dataValueField: "Id",
                            valuePrimitive: true,
                            filter: "contains",
                            autoBind: false,
                            dataSource: {
                                transport: {
                                    read: function (options) {
                                        return departmentService.getSubjectDepartment(result.data.StaffDTO.DepartmentId).then(function (result) {
                                            options.success(result.data);
                                        });
                                    }
                                }
                            }
                        };
                        $scope.staffResource = {
                            placeholder: "Chọn nhân viên...",
                            dataTextField: "Name",
                            dataValueField: "Id",
                            valuePrimitive: true,
                            filter: "contains",
                            autoBind: false,
                            dataSource: {
                                transport: {
                                    read: function (options) {
                                        return staffService.getStaffByAgentObjectType(99, result.data.StaffDTO.DepartmentId).then(function (result) {
                                            options.success(result.data);
                                        });
                                    }
                                }
                            }
                        };
                        $scope.IsLoading = false;

                        setTimeout(function () {
                            $('.collapseHeader').click(function () {
                                $(this).parent().nextUntil('div').slideToggle(100, function () {
                                });

                                $(this).find('span').text(function (_, value) { return value == '-' ? '+' : '-' });
                            });

                            //$("#mainTabId").kendoTabStrip().data("kendoTabStrip");
                        }, 500);
                        createTabHeader();


                        //UpdatePlanDetailCache();

                        //canh đều chiều cao các dòng
                        setTimeout(function () {
                            $('#mainTabId .k-tabstrip-items .k-item').click(function () {
                                setTimeout(function () {
                                    $scope.refreshEqualHeight();
                                }, 200);
                            });
                        }, 200);

                        $scope.refreshEqualHeight();
                    });

                }
                getPlanDetailList();

                $scope.refreshEqualHeight = function () {
                    setTimeout(function () {
                        var iiparent = 1;
                        $('.table-equal_height').each(function () {
                            var parentOfParentSelector = ".table-equal_height.table-" + iiparent;
                            var iparent = 1;
                            $(this).find($('.tr-equal_height')).each(function () {
                                var parentSelector = ".tr-equal_height.tr-" + iparent;
                                var $el = $(this);
                                var child = $(".equal_height.one");
                                var child2 = $('.row-height');

                                $el.find(child).each(function () {
                                    var $el2 = $(this);

                                    var index = 1;
                                    $el2.find(child2).each(function () {
                                        var $el3 = $(this);
                                        var childSelector = parentOfParentSelector + ' ' + parentSelector + ' .equal_height:not(.one) .row-height.row-' + index;

                                        $(childSelector).css('height', $el3.height());
                                        index++;
                                    });

                                });
                                iparent++;
                            });
                            iiparent++;
                        });
                    }, 100);
                }

                //tính số giờ HĐ khác của giảng viên đã đăng ký
                $scope.totalOtherActivityHour = function () {
                    var result = 0;
                    if ($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings != undefined) {
                        $.each($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings, function (idx, item) {
                            $.each(item.PlanKPIDetails, function (idx2, item2) {
                                if (item.TargetGroupDetailTypeId == 4) {
                                    $.each(item2.ProfessorOtherActivities, function (idx3, item3) {
                                        result += item3.NumberOfHour * item3.NumberOfTime;
                                    });
                                }
                            });
                        });
                    }
                    return result;
                }
                //tính số giờ NCKH của giảng viên đã đăng ký
                $scope.totalScienceResearchHour = function () {
                    var result = 0;
                    if ($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings != undefined) {
                        $.each($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings, function (idx, item) {
                            $.each(item.PlanKPIDetails, function (idx2, item2) {
                                if (item.TargetGroupDetailTypeId == 5) {
                                    $.each(item2.ScienceResearches, function (idx3, item3) {
                                        result += item3.NumberOfHour * item3.NumberOfResearch;
                                    });
                                }
                            });
                        });
                    }
                    return result;
                }
                //// tính điểm giảng dạy của giảng viên đã đăng ký
                $scope.totalProfessorPoint = function () {
                    var result = 0;
                    if ($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings != undefined) {
                        $.each($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings, function (idx, item) {
                            if (item.TargetGroupDetailTypeId == 0) {
                                $.each(item.PlanKPIDetails, function (idx2, item2) {
                                    $.each(item2.CriterionDictionaries, function (idx3, item3) {
                                        if (item2.CurrentKPI != null && item3.Id == item2.CurrentKPI)
                                            result += item3.Record;
                                    });
                                });
                            }
                        });
                    }
                    return result;
                }

                $scope.refreshPlanDetailList = function (id, agentObjectTypeId, targetGroupDetailId, type) {
                    $scope.IsLoading = true;
                    //Thêm mới
                    if (type == 1) {
                        $.each($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings, function (idx, item) {
                            if (item.TargetGroupId == targetGroupDetailId) {
                                planKPIDetailService.getObjDTO(id, agentObjectTypeId).then(function (result) {
                                    var pO = result.data;
                                    item.PlanKPIDetails.push(pO);
                                });
                                $state.go($state.current, {}, { reload: true });
                            }
                        });
                    }
                    else
                        //Sửa
                        if (type == 2) {
                            $.each($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings, function (idx, item) {
                                //if (item.TargetGroupDetailTypeId == 7) {
                                //    var deleteIndex = -1;
                                //    $.each(item.PlanKPIDetails, function (i, item1) {
                                //        planKPIDetailService.getObjDTO(id, agentObjectTypeId).then(function (result) {
                                //            if (id != item1.Id && result.data.IsMoved == true && item1.FromCriterionId == result.data.FromCriterionId) {
                                //                deleteIndex = i;                                                
                                //                item.PlanKPIDetails.splice(deleteIndex, deleteIndex + 1);
                                //            }
                                //            if ((id != item1.Id && result.data.IsMoved == true) || (item.PlanKPIDetails.length == 0 && result.data.IsMoved == true)) {
                                //                var p = result.data;
                                //                p.IsMoved = false;
                                //                item.PlanKPIDetails.push(p);
                                //            }
                                //        });
                                //        return false;
                                //    });
                                //}
                                //else {
                                $.each(item.PlanKPIDetails, function (ii, item2) {
                                    if (item2.Id == id) {
                                        planKPIDetailService.getObjDTO(id, agentObjectTypeId).then(function (result) {
                                            item.PlanKPIDetails[ii] = result.data;
                                            $state.go($state.current, {}, { reload: true });
                                        });
                                    }
                                });
                                //    }
                            });
                        }
                        else
                            //Xóa
                            if (type == 3) {
                                $.each($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings, function (idx, item) {

                                    if (item.TargetGroupId == targetGroupDetailId) {
                                        var deleteIndex = -1;
                                        $.each(item.PlanKPIDetails, function (idx2, item2) {
                                            if (item2.Id == id) {
                                                deleteIndex = idx2;
                                            }
                                        });
                                        item.PlanKPIDetails.splice(deleteIndex, 1);
                                    }
                                });
                            }
                    $scope.IsLoading = false;

                    $scope.refreshEqualHeight();
                }

                $scope.ExportExcel = function () {
                    window.open("/ExcelExport/PlanDetailExport.ashx?planId=" + $scope.planId + "&agentObjectId=" + $scope.agentObjectId + "&normalStaffId=" + $scope.normalStaffId + "&departmentId=" + $scope.departmentId + "&option=" + $scope.agentObjectTypeId + "&userRole=" + $scope.userRole);
                }



                $scope.departmentsDataSource = {
                    placeholder: "Chọn Đơn vị...",
                    dataTextField: "Name",
                    dataValueField: "Id",
                    valuePrimitive: true,
                    autoBind: false,
                    filter: "contains",
                    dataSource: {
                        transport: {
                            read: function (options) {
                                return departmentService.getMainDepartment().then(function (result) {
                                    options.success(result.data);
                                });
                            },
                        },
                        group: { field: "DepartmentTypeName" }
                    }
                };

                departmentService.getMainDepartment().then(function (result) {
                    $scope.Departments = result.data;
                });



                $scope.numericOptions = {
                    format: "n0",
                    min: 0
                }




                var newPlanDetail = function () {
                    if ($scope.agentObjectTypeId == 5 || $scope.agentObjectTypeId == 3) {
                        return {
                            Id: MANAGER.GUID_EMPTY,
                            Name: "",
                            ExcecuteMethod: "",
                            BasicResource: "",
                            CriterionId: MANAGER.GUID_EMPTY,
                            StartTime: $scope.startPlan,
                            EndTime: $scope.endPlan,
                            LeadDepartment: { Id: $scope.departmentId, Name: $scope.departmentName },
                            CanDelete: true,
                            IsLockable: true,
                            FromCriterionId: MANAGER.GUID_EMPTY
                        }
                    }
                    else
                        if ($scope.agentObjectTypeId == 6) {
                            return {
                                Id: MANAGER.GUID_EMPTY,
                                Name: "",
                                ExcecuteMethod: "",
                                BasicResource: "",
                                CriterionId: "",
                                StartTime: $scope.startPlan,
                                EndTime: $scope.endPlan,
                                //StaffLeader: { Id: $scope.staffLeaderId, StaffProfile: { Name: $scope.staffLeaderName } },
                                CanDelete: true,
                                IsLockable: true
                            }
                        }
                        else {
                            return {
                                Id: MANAGER.GUID_EMPTY,
                                Name: "",
                                ExcecuteMethod: "",
                                BasicResource: "",
                                CriterionId: MANAGER.GUID_EMPTY,
                                StartTime: $scope.startPlan,
                                EndTime: $scope.endPlan,
                                CanDelete: true,
                                IsLockable: true,
                                FromCriterionId: MANAGER.GUID_EMPTY
                            }
                        }
                }


                $scope.staffPlanDetailMakingObject = {
                    Id: $scope.staffId,
                    targetGroupList: []
                };

                //không sử dụng
                $scope.newPlan = function (targetId) {
                    $.each($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings, function (idx, item) {
                        if (item.TargetGroupId == targetId) {
                            var planDetail = newPlanDetail();
                            planDetail.Criterions =
                            item.PlanKPIDetails.push(planDetail);
                            return;
                        }
                    });
                }


                $scope.getCriterionName = function (criterionId, list) {
                    var result = "";
                    $.each(list, function (idx, item) {
                        if (item.Id == criterionId) {
                            result = item.Name;
                        }
                    });
                    return result;
                }

                $scope.criterionUpdate = function (selectedValue, TargetGroupId, agentObjectTypeId) {
                    $.each($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings, function (idx, item) {
                        if (item.TargetGroupId == TargetGroupId) {
                            var selectedCount = 0;
                            $.each(item.PlanKPIDetails, function (idx, item2) {
                                if (item2.CriterionId == selectedValue) {
                                    selectedCount++;
                                    if (selectedCount > 1) {
                                        item2.CriterionId = "";
                                        alert("Tiêu chí này đã được chọn trước đó!");
                                        return;
                                    }
                                    if (agentObjectTypeId == 2)
                                        item2.TargetDetail = $scope.getCriterionName(item2.CriterionId, item.Criterions);
                                }
                            });
                        }
                    });
                }

                $scope.lock = function (planStaffId) {
                    $scope.IsLoading = true;
                    planKPIDetailService.Lock($scope.staffPlanDetailMakingObject).then(function (result) {
                        $scope.IsLoading = false;
                        Notify("Khóa thành công", 'top-right', '2000', 'success', 'fa-check-square-o', true);
                        Notify('Đang gửi Mail...', 'top-right', '10000', 'success', 'fa-envelope-o', true);
                        planKPIDetailService.saveSendMail(result);
                        $scope.isDisable = true;
                        $state.go($state.current, {}, { reload: true });
                    });
                }

                $scope.LockPlanStaff = function (planStaffId) {
                    $scope.IsLoading = true;
                    planKPIDetailService.LockPlanStaff($scope.staffPlanDetailMakingObject).then(function (result) {
                        $scope.IsLoading = false;
                        if (result)
                            alert("Khóa thành công");
                        else
                            alert("Mở khóa thành công");
                        $scope.isDisable = true;
                        $state.go($state.current, {}, { reload: true });
                    });


                }

                $scope.save = function () {
                    $scope.IsLoading = true;
                    planKPIDetailService.Save($scope.staffPlanDetailMakingObject).then(function () {
                        alert("Lưu thành công");
                        $scope.IsLoading = false;
                        $state.go($state.current, {}, { reload: true });
                        //window.location.reload();
                    });
                }
                $scope.saveProfessor = function () {
                    $scope.IsLoading = true;
                    planKPIDetailService.Save1($scope.staffPlanDetailMakingObject).then(function () {
                        alert("Lưu thành công");
                        $scope.IsLoading = false;
                        $state.go($state.current, {}, { reload: true });
                        //window.location.reload();
                    });
                }

                $scope.cancel = function () {

                }



                $scope.Delete = function (index, targetId) {
                    $.each($scope.staffPlanDetailMakingObject.TargetGroupPlanMakings, function (idx, item) {
                        if (item.TargetGroupId == targetId) {
                            item.PlanKPIDetails.splice(index, 1);
                            return;
                        }
                    });
                }

                //không sử dụng
                $scope.newPlan = function (targetId, type) {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: $scope.InformationUrl,
                        controller: 'planKPIDetailInformationController',
                        size: 'lg',
                        resolve: {
                            targetId: function () {
                                return targetId;
                            },
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            userRole: function () {
                                return $scope.userRole;
                            },
                            planStaffId: function () {
                                return $scope.staffPlanDetailMakingObject.PlanStaffId;
                            },
                            staffId: function () {
                                return $scope.staffPlanDetailMakingObject.StaffDTO.Id;
                            },
                            plan: function () {
                                return {
                                    Id: $scope.planId,
                                    startPlan: $scope.startPlan,
                                    endPlan: $scope.endPlan,
                                    planType: $scope.planType
                                };
                            },
                            department: function () {
                                return {
                                    departmentId: $scope.departmentId,
                                    departmentName: $scope.departmentName
                                };
                            },
                            agentObjectTypeId: function () {
                                return $scope.agentObjectTypeId;
                            },
                            isAddition: function () {
                                return type == 1 ? false : true;
                            },
                            targetGroupDetailTypeId: function () {
                                return
                            }
                        }
                    }).result.then(function (result) {
                        if (result != "" && result != "2" && result != "3") {
                            $scope.isDisable = false;
                            setTimeout(function () {

                                $scope.refreshPlanDetailList(result, $scope.agentObjectTypeId, targetId, 1);
                                $scope.$apply();

                            }, 500);
                        }
                    });
                }

                $scope.editPlan = function (targetId, planDetailId) {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: $scope.InformationUrl,
                        controller: 'planKPIDetailInformationController',
                        size: 'lg',
                        resolve: {
                            targetId: function () {
                                return targetId;
                            },
                            id: function () {
                                return planDetailId;
                            },
                            userRole: function () {
                                return $scope.userRole;
                            },
                            planStaffId: function () {
                                return $scope.staffPlanDetailMakingObject.PlanStaffId;
                            },
                            staffId: function () {
                                return $scope.staffPlanDetailMakingObject.StaffDTO.Id;
                            },
                            plan: function () {
                                return {
                                    Id: $scope.planId,
                                    startPlan: $scope.startPlan,
                                    endPlan: $scope.endPlan,
                                    planType: $scope.planType,
                                };
                            },
                            department: function () {
                                return {
                                    departmentId: $scope.departmentId,
                                    departmentName: $scope.departmentName
                                };
                            },
                            agentObjectTypeId: function () {
                                return $scope.agentObjectTypeId;
                            },
                            isAddition: function () {
                                return false;
                            }
                        }
                    }).result.then(function (result) {
                        if (result != "" && result != "2" && result != "3") {
                            $scope.isDisable = false;
                            setTimeout(function () {
                                $scope.refreshPlanDetailList(result, $scope.agentObjectTypeId, targetId, 2);
                                $scope.$apply();
                            }, 500);
                        }
                    });
                }

                //Hàm này dùng khi Trưởng phòng sửa trọng số cho nhân viên khi duyệt kế hoạch
                //Nếu không sửa thì không được phép duyệt
                $scope.departmentEditStaffPlanDetail = function (targetId, planDetailId) {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: "/app/views/kpi/planKPIDetailInformation/staffDetail_DepartmentDetail.html",
                        controller: 'planKPIDetailInformationController',
                        size: 'lg',
                        resolve: {
                            targetId: function () {
                                return targetId;
                            },
                            id: function () {
                                return planDetailId;
                            },
                            userRole: function () {
                                return $scope.userRole;
                            },
                            planStaffId: function () {
                                return $scope.staffPlanDetailMakingObject.PlanStaffId;
                            },
                            staffId: function () {
                                return $scope.staffPlanDetailMakingObject.StaffDTO.Id;
                            },
                            plan: function () {
                                return {
                                    Id: $scope.planId,
                                    startPlan: $scope.startPlan,
                                    endPlan: $scope.endPlan
                                };
                            },
                            department: function () {
                                return {
                                    departmentId: $scope.departmentId,
                                    departmentName: $scope.departmentName
                                };
                            },
                            agentObjectTypeId: function () {
                                return $scope.agentObjectTypeId;
                            },
                            isAddition: function () {
                                return false;
                            }
                        }
                    }).result.then(function (result) {
                        if (result != "" && result != "2" && result != "3") {
                            $scope.isDisable = false;
                            setTimeout(function () {
                                $scope.refreshPlanDetailList(result, $scope.agentObjectTypeId, targetId, 2);
                                $scope.$apply();
                            }, 500);
                        }
                    });
                }

                $scope.newPlan = function (targetId, type) {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: $scope.InformationUrl,
                        controller: 'planKPIDetailInformationController',
                        size: 'lg',
                        //backdrop: false,
                        resolve: {
                            targetId: function () {
                                return targetId;
                            },
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            userRole: function () {
                                return $scope.userRole;
                            },
                            planStaffId: function () {
                                return $scope.staffPlanDetailMakingObject.PlanStaffId;
                            },
                            staffId: function () {
                                return $scope.staffPlanDetailMakingObject.StaffDTO.Id;
                            },
                            plan: function () {
                                return {
                                    Id: $scope.planId,
                                    startPlan: $scope.startPlan,
                                    endPlan: $scope.endPlan,
                                    planType: $scope.planType
                                };
                            },
                            department: function () {
                                return {
                                    departmentId: $scope.departmentId,
                                    departmentName: $scope.departmentName
                                };
                            },
                            agentObjectTypeId: function () {
                                return $scope.agentObjectTypeId;
                            },
                            isAddition: function () {
                                return type == 1 ? false : true;
                            },
                            targetGroupDetailTypeId: function () {
                                return
                            }
                        }
                    }).result.then(function (result) {
                        if (result != "" && result != "2" && result != "3") {
                            $scope.isDisable = false;
                            setTimeout(function () {
                                //$(".fht-tbody").first().scrollTop(800);

                                $scope.refreshPlanDetailList(result, $scope.agentObjectTypeId, targetId, 1);
                                $scope.$apply();


                            }, 100);
                            setTimeout(function () {

                                //($(".fht-tbody").first())[0].scrollHeight = ($(".fht-tbody").first())[0].scrollHeight + 100;

                                $('html, body').animate({ scrollTop: $(document).height() }, 'fast', function () {
                                    var height = ($(".fht-tbody").eq(0))[0].scrollHeight;
                                    $(".fht-tbody").eq(0).scrollTop(height + 1000);
                                });
                            }, 500);
                        }
                    });
                }
                $scope.delete = function (targetId, planDetailId) {
                    var isValid = window.confirm("Bạn có muốn xóa kế hoạch này không");
                    if (!isValid)
                        return;
                    planKPIDetailService.getObj(planDetailId, $scope.agentObjectTypeId).then(function (result) {

                        planKPIDetailService.Delete(result.data).then(function (result) {
                            if (result == 1) {
                                setTimeout(function () {
                                    $scope.refreshPlanDetailList(planDetailId, $scope.agentObjectTypeId, targetId, 3);
                                    $scope.$apply();
                                }, 500);
                                $scope.isDisable = false;
                                alert("Xóa thành công!");
                                return;
                            }
                            else {
                                alert("Xóa thất bại!");
                                return;
                            }
                        });
                    });
                }
                $scope.disable = function (targetId, planDetailId) {

                    planKPIDetailService.getObj(planDetailId, $scope.agentObjectTypeId).then(function (result) {

                        var isValid = false;
                        if (result.data.IsDisable == false)
                            isValid = window.confirm("Bạn có muốn ngừng kế hoạch này không?");
                        else
                            isValid = window.confirm("Bạn có muốn tái sử dụng kế hoạch này không?");
                        if (!isValid)
                            return;
                        var obj = result.data;
                        planKPIDetailService.Disable(obj).then(function (result) {
                            if (result == 1) {
                                setTimeout(function () {
                                    $scope.refreshPlanDetailList(planDetailId, $scope.agentObjectTypeId, targetId, 2);
                                    $scope.$apply();
                                }, 500);
                                //$scope.isDisable = false;
                                if (obj.IsDisable == false)
                                    alert("Ngừng sử dụng thành công!");
                                else
                                    alert("Đã được tái sử dụng!");
                                return;
                            }
                            else {
                                alert("Ngừng sử dụng thất bại!");
                                return;
                            }
                        });
                    });

                }

                agentObjectService.getWorkingModeListByAgentObject($scope.agentObjectId).then(function (result) {
                    $scope.workingModeLists = result.data;
                });

                $scope.renderHtml = function (htmlCode) {
                    return $sce.trustAsHtml(htmlCode);
                };

                $scope.$watch('agentObjectDetail.Id', function (newVal, oldVal) {
                    var hint = "";
                    $scope.workingModeHint = "";
                    if ($scope.workingModeLists != undefined) {
                        $.each($scope.workingModeLists, function (idx, item) {
                            if ($scope.agentObjectDetail.Id == item.Id) {
                                hint = "GD: " + item.NumberOfSection + " tiết (" + item.NumberOfSectionDensity + "%)    ";
                                hint += "NCKH: " + item.ScienceResearch + " tiết (" + item.ScienceResearchDensity + "%)    ";
                                hint += "HĐK: " + item.OtherActivity + " giờ (" + item.OtherActivityDensity + "%)";
                                $scope.workingModeHint = $sce.trustAsHtml(hint);
                            }
                        });
                    }
                });


                //phải truyền id từ view vào controller
                //hoặc phải có dấu chấm nếu dùng ng-model nếu không thì controller not update data
                //"If you use ng-model, you have to have a dot in there." 
                $scope.saveWorkingMode = function () {
                    if ($scope.agentObjectDetail.Id == MANAGER.GUID_EMPTY) {
                        alert("Chưa chọn chế độ làm việc");
                        return;
                    }
                    planKPIDetailService.getSaveWorkingMode($scope.staffPlanDetailMakingObject.PlanStaffId, $scope.agentObjectDetail.Id).then(function (result) {
                        if (result.data == 1) {
                            planKPIDetailService.getWorkingModeByPlanStaff($scope.staffPlanDetailMakingObject.PlanStaffId).then(function (result1) {
                                $scope.agentObjectDetail = result1.data;
                            });
                        }
                        else {
                            alert("Cấp trên đã duyệt chế độ làm việc của bạn. Không thể chỉnh sửa!");
                            return;
                        }
                    });
                }

                $scope.$on('$locationChangeStart', function (event) {
                    if (!$scope.isSupervisor && $scope.isDisable == false && $scope.agentObjectTypeId != 1 && $scope.agentObjectTypeId != 2 && $scope.agentObjectTypeId != 7 && $scope.agentObjectTypeId != 8 && $scope.agentObjectTypeId != 10 && $scope.agentObjectTypeId != 11) {
                        var answer = confirm("Kế hoạch chưa được khóa, bạn có chắc chắn muốn thoát không?")
                        if (!answer) {
                            event.preventDefault();
                        }
                    }
                });
                $(function () {
                    var isFirefox = typeof InstallTrigger !== 'undefined';
                    if (isFirefox == true) {
                        var contentWidth = document.getElementById('infoTable').clientWidth;
                        $("#inner").width(contentWidth);
                    }
                });


            }
    ]);

});