define(['app/app', 'app/services/kpi/methodService', 'app/services/kpi/planKPIDetailService', 'app/services/kpi/departmentService', 'app/services/kpi/staffService', 'app/services/kpi/criterionService', 'app/services/kpi/professorCriterionService', 'app/services/kpi/planKPIDetail_KPIService', 'app/services/kpi/professorOtherActivityService', 'app/services/kpi/targetGroupDetailService', 'app/services/kpi/fileAttachmentService', 'app/services/kpi/scienceResearchService', 'moment'], function (app) {
    app.controller('planKPIDetailInformationController', ['$scope', '$modal', '$modalInstance', 'targetId', 'id', 'userRole', 'planStaffId', 'staffId', 'plan', 'department', 'agentObjectTypeId', 'isAddition', 'methodService', 'planKPIDetailService', 'departmentService', 'staffService', 'criterionService', 'professorCriterionService', 'planKPIDetail_KPIService', 'professorOtherActivityService', 'fileAttachmentService', 'targetGroupDetailService', 'scienceResearchService',

        function ($scope, $modal, $modalInstance, targetId, id, userRole, planStaffId, staffId, plan, department, agentObjectTypeId, isAddition, methodService, planKPIDetailService, departmentService, staffService, criterionService, professorCriterionService, planKPIDetail_KPIService, professorOtherActivityService, fileAttachmentService, targetGroupDetailService, scienceResearchService) {
            $scope.methodList = [];
            $scope.title = "";
            $scope.plan = plan;
            $scope.department = department;
            $scope.ManageCodes = [];
            $scope.SubDepartmentIds = [];
            $scope.SubStaffIds = [];
            $scope.agentObjectTypeId = agentObjectTypeId;
            $scope.subDepartments = [];
            $scope.targetGroupDetailDictionaries = [];
            $scope.professorCriterions = [];
            $scope.targetGroupDetailTypeId = 0;
            $scope.fromCriterion = null;
            $scope.haveMethod = false;
            $scope.options = {
                format: "n0",
                max: 100,
                min: 0
            }
            criterionService.getListManageCode().then(function (result) {
                $scope.ManageCodes = result.data;
            });
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
                            return departmentService.getSubjectDepartment(department.departmentId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                }
            };
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
                            return staffService.getViceDepartmentStaff($scope.agentObjectTypeId, $scope.agentObjectTypeId == 4 ? MANAGER.GUID_EMPTY : department.departmentId, $scope.plan.Id).then(function (result) {
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
            var newPlanDetail = function () {
                if ($scope.agentObjectTypeId == 5 || $scope.agentObjectTypeId == 3) {
                    return {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        ExcecuteMethod: "",
                        BasicResource: "",
                        CriterionId: "",
                        PreviousKPI: "",
                        CurrentKPI: "",
                        StartTime: $scope.plan.startPlan,
                        EndTime: $scope.plan.endPlan,
                        TargetGroupDetail: { Id: targetId },
                        LeadDepartment: { Id: $scope.department.departmentId, Name: $scope.department.departmentName },
                        CanDelete: true,
                        IsLockable: true,
                        PlanStaffId: planStaffId,
                        PlanId: $scope.plan.Id,
                        StaffId: staffId,
                        IsAddition: isAddition
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
                            PreviousKPI: "",
                            CurrentKPI: "",
                            StartTime: $scope.startPlan,
                            EndTime: $scope.endPlan,
                            CanDelete: true,
                            IsLockable: true,
                            TargetGroupDetail: { Id: targetId },
                            PlanStaffId: planStaffId,
                            PlanId: $scope.plan.Id,
                            StaffId: staffId,
                            IsAddition: isAddition
                        }
                    }
                    else {
                        return {
                            Id: MANAGER.GUID_EMPTY,
                            Name: "",
                            ExcecuteMethod: "",
                            BasicResource: "",
                            CriterionId: "",
                            PreviousKPI: "",
                            CurrentKPI: "",
                            StartTime: $scope.plan.startPlan,
                            EndTime: $scope.plan.endPlan,
                            TargetGroupDetail: { Id: targetId },
                            CanDelete: true,
                            IsLockable: true,
                            PlanStaffId: planStaffId,
                            PlanId: $scope.plan.Id,
                            StaffId: staffId,
                            IsAddition: isAddition
                        }
                    }
            }

            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.canEditMethods = $scope.isNew;
            var isFromCriterion = true;

            if ($scope.isNew) {
                $scope.obj = newPlanDetail();
                $scope.obj.StartTime = $scope.plan.startPlan;
                $scope.obj.EndTime = $scope.plan.endPlan;
                var tabStrip = $("#mainTabId").kendoTabStrip().data("kendoTabStrip");
            } else {
                planKPIDetailService.getObj(id, $scope.agentObjectTypeId).then(function (result) {
                    $scope.obj = result.data;
                    $scope.obj.PlanStaffId = planStaffId;
                    $scope.obj.PlanId = $scope.plan.Id;
                    $scope.obj.StaffId = staffId;
                    $scope.obj.TargetGroupDetail = { Id: targetId };
                    $scope.fromCriterion = $scope.obj.FromCriterion;
                    $scope.canEditMethods = $scope.isNew || $scope.obj.FromCriterionId == MANAGER.GUID_EMPTY;
                    $scope.professorCriterionId = $scope.obj.FromProfessorCriterionId;
                    //$scope.obj.Method = { Id: result.data.MethodId }


                });
            }

            criterionService.getDictionnaryByTargetGroupDetailId(targetId).then(function (result) {
                $scope.targetGroupDetailDictionaries = result.data;
            });


            planKPIDetailService.getMeasureUnits().then(function (result) {
                $scope.MeasureUnits = result.data;
            });

            targetGroupDetailService.getTargetGroupDetailTypeId(targetId).then(function (result) {
                $scope.targetGroupDetailTypeId = result.data;

            });
            $scope.activityResource = {
                placeholder: "Chọn hoạt động ...",
                dataTextField: "Name",
                dataValueField: "Id",
                valuePrimitive: true,
                filter: "contains",
                autoBind: false,
                dataSource: {
                    transport: {
                        read: function (options) {
                            return professorCriterionService.getDictionnaryByCriterionId($scope.professorCriterionId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                }
            };
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

            methodService.getListByPlanDetail(targetId).then(function (result) {
                $scope.methodList = result.data;
            });


            departmentService.getMainDepartmentListHierarchy(id).then(function (result) {
                $scope.treeData = new kendo.data.HierarchicalDataSource({
                    data: result.data
                });

            });

            $scope.uploadOptions = {
                async: {
                    saveUrl: "/FileUpload/SavePlanDetailFile?id=" + id + "&planStaffId=" + planStaffId + "&targetGroupDetailId=" + targetId,
                    autoUpload: true
                },
                success: onUploadSuccess
            }

            function onUploadSuccess(e) {
                $scope.fileGrid.dataSource.read();
                $scope.fileGrid.refresh();
            }
            $scope.fileDataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        return fileAttachmentService.getListByPlanDetail(id).then(function (result) {
                            options.success(result.data);
                        });
                    }
                },
                pageSize: 20
            });
            $scope.fileMainGridOptions = {
                sortable: true,
                pageable: false,
                columns: [{
                    field: "FileName",
                    title: "Tên file"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='downdLoadFile(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-download'></i>  </button></div>",
                    width: "50px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='deletePlanDetailFile(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                    width: "50px",
                }],
            };


            $scope.treeOptions = {
                checkboxes: {
                    checkChildren: true
                },
                dataTextField: ["Name", "Name"],
                check: onCheck,
                dataBound: treeDataBound
            };



            function checkedNodeIds(nodes, checkedNodes) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].checked) {
                        $scope.obj.SubDepartmentIds.push(nodes[i].Id);
                    }

                    if (nodes[i].hasChildren) {
                        checkedNodeIds(nodes[i].children.view(), $scope.obj.SubDepartmentIds);
                    }
                }
            }

            function onCheck() {
                $scope.obj.SubDepartmentIds = [];
                var treeView = $scope.departmentTree,
                 message;
                checkedNodeIds(treeView.dataSource.view(), $scope.obj.SubDepartmentIds);
                //   if ($scope.obj.SubDepartmentIds.length > 0) {
                //       alert($scope.obj.SubDepartmentIds.length);
                //    message = "IDs of checked nodes: " + $scope.obj.SubDepartmentIds.join(",");
                //} else {
                //    message = "No nodes checked.";
                //}
                //$("#result").html(message);
            }

            function treeDataBound(e) {
                this.expand(".k-item");
                //this.updateIndeterminate();
            }

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
                            return staffService.getStaffByAgentObjectType(99, $scope.department.departmentId, userRole).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                }
            };

            staffService.getStaffByAgentObjectType($scope.agentObjectTypeId, $scope.agentObjectTypeId == 4 ? MANAGER.GUID_EMPTY : $scope.department.departmentId, userRole).then(function (result) {
                $scope.StaffLeaders = result.data;
                $.each(result.data, function (idx, item) {
                    if (item.Position.Name == "Trưởng Phòng")
                        $scope.departmentLeaderId = item.Id;
                });
            });


            $scope.newMethod = function () {
                $scope.isEdit = false;
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/methodDetail.html',
                    controller: 'methodDetailController',
                    resolve: {
                        planKPIDetailId: function () {
                            return id;
                        },
                        id: function () {
                            return MANAGER.GUID_EMPTY;
                        },
                        plan: function () {
                            return {
                                Id: $scope.plan.Id,
                                startPlan: $scope.plan.startPlan,
                                endPlan: $scope.plan.endPlan
                            };
                        },
                        planStaffId: function () {
                            return planStaffId;
                        }, targetGroupDetailId: function () {
                            return targetId;
                        }
                    }
                }).result.then(function (result) {
                    if (id == MANAGER.GUID_EMPTY || typeof (id) == "undefined") {
                        id = result;
                        $scope.obj.Id = id;

                        //planKPIDetailService.getObj(id).then(function (result) {
                        //    $scope.obj.Id = id;
                        //    $scope.obj = result.data;
                        //    $scope.obj.PlanStaffId = planStaffId;
                        //    $scope.obj.PlanId = $scope.plan.Id;
                        //    $scope.obj.StaffId = staffId;
                        //    $scope.obj.TargetGroupDetail = { Id: targetId };
                        //    //$scope.obj.Method = { Id: result.data.MethodId }
                        //});
                    }
                    $scope.grid.dataSource.read();
                    $scope.grid.refresh();

                });
            }


            $scope.editMethod = function (methodId) {
                if (methodId == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/methodDetail.html',
                    controller: 'methodDetailController',
                    resolve: {
                        planKPIDetailId: function () {
                            return id;
                        },
                        id: function () {
                            return methodId;
                        },
                        plan: function () {
                            return {
                                Id: $scope.plan.Id,
                                startPlan: $scope.plan.startPlan,
                                endPlan: $scope.plan.endPlan
                            };
                        },
                        planStaffId: function () {
                            return planStaffId;
                        }, targetGroupDetailId: function () {
                            return targetId;
                        }
                    }
                }).result.finally(function (result) {
                    $scope.grid.dataSource.read();
                    $scope.grid.refresh();
                });
            }

            $scope.deleteMethod = function (Id) {
                if (Id == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }
                var isValid = window.confirm("Bạn có muốn xóa phần tử này không?");
                if (!isValid)
                    return;
                methodService.getObj(Id).then(function (result) {
                    result.data.Method = { Id: result.data.MethodId }
                    methodService.Delete(result.data).then(function () {

                        if (result.data == 1) {
                            alert("Xóa thất bại!");
                        }

                        $scope.grid.dataSource.read();
                        $scope.grid.refresh();
                    });
                });
            }

            $scope.newKPI = function () {
                $scope.isEdit = false;
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/planKPIDetail_KPIDetail.html',
                    controller: 'KPIDetailController',
                    resolve: {
                        planKPIDetailId: function () {
                            return id;
                        },
                        id: function () {
                            return MANAGER.GUID_EMPTY;
                        },
                        fromCriterion: function () {
                            return $scope.fromCriterion;
                        },
                        planStaffId: function () {
                            return planStaffId;
                        }, targetGroupDetailId: function () {
                            return targetId;
                        }
                    }
                }).result.then(function (result) {
                    if (id == MANAGER.GUID_EMPTY || typeof (id) == "undefined") {
                        id = result;
                        $scope.obj.Id = id;
                        //planKPIDetailService.getObj(id).then(function (result) {
                        //    $scope.obj.Id = id;
                        //    $scope.obj = result.data;
                        //    $scope.obj.PlanStaffId = planStaffId;
                        //    $scope.obj.PlanId = $scope.plan.Id;
                        //    $scope.obj.StaffId = staffId;
                        //    $scope.obj.TargetGroupDetail = { Id: targetId };
                        //    //$scope.obj.Method = { Id: result.data.MethodId }
                        //});
                    }
                    $scope.grid1.dataSource.read();
                    $scope.grid1.refresh();

                });
            }


            $scope.editKPI = function (KPIId) {
                if (KPIId == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }

                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/planKPIDetail_KPIDetail.html',
                    controller: 'KPIDetailController',
                    resolve: {
                        planKPIDetailId: function () {
                            return id;
                        },
                        id: function () {
                            return KPIId;
                        },
                        fromCriterion: function () {
                            return $scope.fromCriterion;
                        },
                        planStaffId: function () {
                            return planStaffId;
                        },
                        targetGroupDetailId: function () {
                            return targetId;
                        }
                    }
                }).result.finally(function (result) {
                    $scope.grid1.dataSource.read();
                    $scope.grid1.refresh();
                });
            }

            $scope.deleteKPI = function (Id) {
                if (Id == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }
                var isValid = window.confirm("Bạn có muốn xóa phần tử này không?");
                if (!isValid)
                    return;
                planKPIDetail_KPIService.getObj(Id).then(function (result) {
                    planKPIDetail_KPIService.Delete(result.data).then(function () {
                        $scope.grid1.dataSource.read();
                        $scope.grid1.refresh();
                    });
                });
            }

            $scope.newPlanDetailFile = function () {
                $scope.isEdit = false;
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/planDetailFileDetail.html',
                    controller: 'planDetailFileController',
                    resolve: {
                        planKPIDetailId: function () {
                            return id;
                        },
                        id: function () {
                            return MANAGER.GUID_EMPTY;
                        },
                        planStaffId: function () {
                            return planStaffId;
                        }, targetGroupDetailId: function () {
                            return targetId;
                        }
                    }
                }).result.then(function (result) {
                    if (id == MANAGER.GUID_EMPTY || typeof (id) == "undefined") {
                        id = result;
                        $scope.obj.Id = id;
                        //planKPIDetailService.getObj(id).then(function (result) {
                        //    $scope.obj.Id = id;
                        //    $scope.obj = result.data;
                        //    $scope.obj.PlanStaffId = planStaffId;
                        //    $scope.obj.PlanId = $scope.plan.Id;
                        //    $scope.obj.StaffId = staffId;
                        //    $scope.obj.TargetGroupDetail = { Id: targetId };
                        //    //$scope.obj.planDetailFile = { Id: result.data.planDetailFileId }
                        //});
                    }
                    $scope.fileGrid.dataSource.read();
                    $scope.fileGrid.refresh();

                });
            }


            $scope.downdLoadFile = function (Id) {
                fileAttachmentService.downloadFile(Id);


            }

            $scope.deletePlanDetailFile = function (Id) {
                if (Id == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }
                var isValid = window.confirm("Bạn có muốn xóa phần tử này không?");
                if (!isValid)
                    return;
                fileAttachmentService.getObj(Id).then(function (result) {
                    result.data.PlanDetailFile = { Id: result.data.planDetailFileId }
                    fileAttachmentService.Delete(result.data).then(function () {

                        if (result.data == 1) {
                            alert("Xóa thất bại!");
                        }
                        $scope.fileGrid.dataSource.read();
                        $scope.fileGrid.refresh();
                    });
                });
            }

            //////////////////////////////////////////////////////////////
            $scope.dataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        return methodService.getListByPlanDetail(id).then(function (result) {
                            options.success(result.data);
                        });
                    }
                },
                pageSize: 20
            });
            $scope.mainGridOptions = {
                sortable: true,
                pageable: false,
                columns: [{
                    field: "Name",
                    title: "Nội dung"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='editMethod(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                    width: "50px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='deleteMethod(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                    width: "50px",
                }],
            };
            ////////////////////////////////////////          
            $scope.editActivity = function (activityId) {
                if (activityId == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }

                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/activityDetail.html',
                    controller: 'activityDetailController',
                    resolve: {
                        planKPIDetailId: function () {
                            return id;
                        },
                        id: function () {
                            return activityId;
                        },
                        planStaffId: function () {
                            return planStaffId;
                        },
                        targetGroupDetailId: function () {
                            return targetId;
                        },
                        professorCriterionId: function () {
                            return $scope.professorCriterionId;
                        }
                    }
                }).result.finally(function (result) {
                    $scope.activityGrid.dataSource.read();
                    $scope.activityGrid.refresh();
                });
            }
            $scope.deleteActivity = function (Id) {
                if (Id == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }
                var isValid = window.confirm("Bạn có muốn xóa hoạt động này không?");
                if (!isValid)
                    return;
                professorOtherActivityService.getObj(Id).then(function (result) {
                    professorOtherActivityService.Delete(result.data).then(function () {
                        $scope.activityGrid.dataSource.read();
                        $scope.activityGrid.refresh();
                    });
                });
            }
            $scope.activityDataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        return professorOtherActivityService.getListByPlanDetail(id).then(function (result) {
                            options.success(result.data);
                        });
                    }
                },
                pageSize: 20
            });
            $scope.activityMainGridOptions = {
                sortable: true,
                pageable: false,
                columns: [{
                    field: "Name",
                    title: "Hoạt động đăng ký"
                },
                {
                    field: "NumberOfHour",
                    title: "Số giờ"
                },
                {
                    field: "NumberOfTime",
                    title: "Số lần"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='editActivity(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                    width: "50px"
                },
            {
                template: "<div style='width: 30px;'><button ng-click='deleteActivity(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                width: "50px"
            }],
            };
            $scope.saveActivity = function () {
                planKPIDetailService.Save($scope.obj).then(function (result) {
                    if (result != "" && result != "2" && result != "3") {
                        $scope.activityGrid.dataSource.read();
                    }
                    else
                        if (result == "2") {
                            alert("Lưu thất bại");
                        }
                });
            }
            ////////////////////////////////////////
            $scope.kpiDataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        return planKPIDetail_KPIService.getListByPlanDetail(id).then(function (result) {
                            options.success(result.data);
                        });
                    }
                },
                pageSize: 20
            });
            $scope.kpiMainGridOptions = {
                sortable: true,
                pageable: false,
                columns: [{
                    field: "Name",
                    title: "KPI đăng ký"
                },
                {
                    field: "MeasureUnitName",
                    title: "Đơn vị tính"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='editKPI(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                    width: "50px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='deleteKPI(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                    width: "50px",
                }],
            };


            ////////////////////////////////////////Science research//////////////////////////////
            $scope.researchResource = {
                placeholder: "Chọn hoạt động ...",
                dataTextField: "Name",
                dataValueField: "Id",
                valuePrimitive: true,
                filter: "contains",
                autoBind: false,
                dataSource: {
                    transport: {
                        read: function (options) {
                            return professorCriterionService.getDictionnaryByCriterionId($scope.professorCriterionId).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                }
            };


            $scope.editResearch = function (researchId) {
                if (researchId == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }

                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/scienceResearchDetail.html',
                    controller: 'scienceResearchDetailController',
                    resolve: {
                        planKPIDetailId: function () {
                            return id;
                        },
                        id: function () {
                            return researchId;
                        },
                        planStaffId: function () {
                            return planStaffId;
                        },
                        targetGroupDetailId: function () {
                            return targetId;
                        },
                        professorCriterionId: function () {
                            return $scope.professorCriterionId;
                        }
                    }
                }).result.finally(function (result) {
                    $scope.researchGrid.dataSource.read();
                    $scope.researchGrid.refresh();
                });
            }

            $scope.researchDataSource = new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        return scienceResearchService.getListByPlanDetail(id).then(function (result) {
                            options.success(result.data);
                        });
                    }
                },
                pageSize: 20
            });
            $scope.researchMainGridOptions = {
                sortable: true,
                pageable: false,
                columns: [{
                    field: "Name",
                    title: "KPI đăng ký"
                },
                {
                    field: "NumberOfResearch",
                    title: "Số lượng"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='editResearch(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                    width: "50px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='deleteResearch(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                    width: "50px",
                }],
            };
            $scope.saveResearch = function () {

                planKPIDetailService.Save($scope.obj).then(function (result) {
                    if (result != "" && result != "2" && result != "3") {
                        $scope.researchGrid.dataSource.read();
                    }



                    else
                        if (result == "2") {
                            alert("Lưu thất bại");
                        }
                });
            }
            $scope.deleteResearch = function (Id) {
                if (Id == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }
                var isValid = window.confirm("Bạn có muốn xóa hoạt động này không?");
                if (!isValid)
                    return;
                scienceResearchService.getObj(Id).then(function (result) {
                    scienceResearchService.Delete(result.data).then(function () {
                        $scope.researchGrid.dataSource.read();
                        $scope.researchGrid.refresh();
                    });
                });
            }
            ////////////////////////////////////////////////////////////////



            //////////////////////////////////////////

            $scope.save = function () {
                $scope.IsLoading = true;

                //Kiểm tra đã có method hay chưa
                if (!$scope.isNew) {
                    methodService.getCheckPlanDetailMethod($scope.obj.Id).then(function (result1) {
                        $scope.haveMethod = result1.data;
                        if ($scope.haveMethod || agentObjectTypeId=="1") {
                            planKPIDetailService.Save($scope.obj).then(function (result) {
                                if (result != "" && result != "2" && result != "3") {

                                    if ($scope.isNew) {
                                        $scope.isNew = false;
                                        $scope.obj.Id = result;
                                        id = result;
                                        if ($scope.targetGroupDetailTypeId != 4) {
                                            $scope.uploadFiles.options.async.saveUrl = "/FileUpload/SavePlanDetailFile?id=" + result + "&planStaffId=" + planStaffId + "&targetGroupDetailId=" + targetId;
                                        }
                                        else {
                                            alert("Lưu thành công");
                                            $modalInstance.close(result);
                                        }
                                    }
                                    else {
                                        alert("Lưu thành công");
                                        $modalInstance.close(result);
                                    }


                                }
                                else
                                    if (result == "2") {
                                        alert("Lưu thất bại");
                                    }
                                    else
                                        if (result == "3") {
                                            alert("Đơn vị chủ trì trùng với đơn vị phối hợp");
                                        }
                            });
                        }
                        else {
                            alert("Chưa có các bước thực hiện");
                        }
                    });

                }
                else {
                    planKPIDetailService.Save($scope.obj).then(function (result) {
                        if (result != "" && result != "2" && result != "3") {

                            if ($scope.isNew) {
                                $scope.isNew = false;
                                $scope.obj.Id = result;
                                id = result;
                                if ($scope.targetGroupDetailTypeId != 4) {
                                    $scope.uploadFiles.options.async.saveUrl = "/FileUpload/SavePlanDetailFile?id=" + result + "&planStaffId=" + planStaffId + "&targetGroupDetailId=" + targetId;
                                }
                                else {
                                    alert("Lưu thành công");
                                    $modalInstance.close(result);
                                }
                            }
                            else {
                                alert("Lưu thành công");
                                $modalInstance.close(result);
                            }


                        }
                        else
                            if (result == "2") {
                                alert("Lưu thất bại");
                            }
                            else
                                if (result == "3") {
                                    alert("Đơn vị chủ trì trùng với đơn vị phối hợp");
                                }
                    });
                }

            }

            $scope.cancel = function () {
                $modalInstance.dismiss('cancel');
                //Kiểm tra đã có method hay chưa
                //if (!$scope.isNew) {
                //    methodService.getCheckPlanDetailMethod($scope.obj.Id).then(function (result1) {
                //        $scope.haveMethod = result1;
                //        if ($scope.haveMethod) {
                //            $modalInstance.dismiss('cancel');
                //        }
                //        else {
                //            alert("Chưa có các bước thực hiện");
                //        }
                //    });

                //}
            }
        }
    ]);

    app.controller('methodDetailController', ['$scope', '$modalInstance', 'id', 'plan', 'planKPIDetailId', 'planStaffId', 'targetGroupDetailId', 'methodService', 'planKPIDetailService',
            function ($scope, $modalInstance, id, plan, planKPIDetailId, planStaffId, targetGroupDetailId, methodService, planKPIDetailService) {
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                //$scope.minDate = new Date();
                if ($scope.isNew) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        StartTime: plan.startPlan,
                        EndTime: plan.endPlan,
                        PlanKPIDetail: { Id: planKPIDetailId, PlanStaff: { Id: planStaffId }, TargetGroupDetail: { Id: targetGroupDetailId } }
                    };
                } else {
                    methodService.getObj(id).then(function (result) {
                        $scope.obj = result.data;
                        $scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                    });
                }

                $scope.save = function () {
                    methodService.Save($scope.obj).then(function (result) {
                        if (result != MANAGER.GUID_EMPTY) {
                            methodService.getUpdatePlanDetailDic(result).then(function (result) {
                            });
                            alert("Lưu thành công");
                            $modalInstance.close(result);
                        }
                        else {
                            alert("Lưu thất bại");
                        }
                    });

                };

                $scope.cancel = function () {
                    $modalInstance.close();
                };
            }
    ]);
    app.controller('planDetailFileController', ['$scope', '$modalInstance', 'id', 'planKPIDetailId', 'planStaffId', 'targetGroupDetailId', 'fileAttachmentService', 'planKPIDetailService',
            function ($scope, $modalInstance, id, planKPIDetailId, planStaffId, targetGroupDetailId, fileAttachmentService, planKPIDetailService) {
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                if ($scope.isNew) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        PlanKPIDetail: { Id: planKPIDetailId, PlanStaff: { Id: planStaffId }, TargetGroupDetail: { Id: targetGroupDetailId } }
                    };
                } else {
                    fileAttachmentService.getObj(id).then(function (result) {
                        $scope.obj = result.data;
                        $scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                    });
                }

                $scope.save = function () {
                    fileAttachmentService.Save($scope.obj).then(function (result) {
                        if (result != MANAGER.GUID_EMPTY) {
                            alert("Lưu thành công");
                            $modalInstance.close(result);
                        }
                        else {
                            alert("Lưu thất bại");
                        }
                    });

                };

                $scope.cancel = function () {
                    $modalInstance.close();
                };
            }
    ]);
    app.controller('KPIDetailController', ['$scope', '$modalInstance', 'id', 'fromCriterion', 'planKPIDetailId', 'planStaffId', 'targetGroupDetailId', 'planKPIDetail_KPIService', 'planKPIDetailService',
            function ($scope, $modalInstance, id, fromCriterion, planKPIDetailId, planStaffId, targetGroupDetailId, planKPIDetail_KPIService, planKPIDetailService) {
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                $scope.fromCriterion = fromCriterion == null || '' ? false : true;
                $scope.MeasureUnits = [];
                planKPIDetailService.getMeasureUnits().then(function (result) {
                    $scope.MeasureUnits = result.data;
                });
                if ($scope.isNew) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        PlanKPIDetail: { Id: planKPIDetailId, PlanStaff: { Id: planStaffId }, TargetGroupDetail: { Id: targetGroupDetailId } }
                    };
                } else {
                    planKPIDetail_KPIService.getObj(id).then(function (result) {
                        $scope.obj = result.data;
                        $scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                    });
                }
                $scope.save = function () {
                    planKPIDetail_KPIService.Save($scope.obj).then(function (result) {
                        if (result != MANAGER.GUID_EMPTY) {
                            alert("Lưu thành công");
                            $modalInstance.close(result);
                        }
                        else {
                            alert("Lưu thất bại");
                        }
                    });

                };

                $scope.cancel = function () {
                    $modalInstance.close();
                };
            }
    ]);
    app.controller('activityDetailController', ['$scope', '$modalInstance', 'id', 'planKPIDetailId', 'professorOtherActivityService',
            function ($scope, $modalInstance, id, planKPIDetailId, professorOtherActivityService) {
                $scope.id = id;
                $scope.options = {
                    format: "n0",
                    max: 100,
                }
                $scope.planKPIDetailId = planKPIDetailId;
                professorOtherActivityService.getObj($scope.id).then(function (result) {
                    $scope.obj = result.data;
                    //$scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                });
                $scope.Save = function () {
                    professorOtherActivityService.Save($scope.obj).then(function (result) {
                        if (result == 1) {
                            alert("Lưu thành công");
                            $modalInstance.close();
                        }
                        else {
                            alert("Lưu thất bại");
                        }
                    });

                };

                $scope.cancel = function () {
                    $modalInstance.close();
                };
            }
    ]);
    app.controller('scienceResearchDetailController', ['$scope', '$modalInstance', 'id', 'planKPIDetailId', 'scienceResearchService',
            function ($scope, $modalInstance, id, planKPIDetailId, scienceResearchService) {
                $scope.id = id;
                $scope.options = {
                    format: "n0",
                    max: 100,
                }
                $scope.planKPIDetailId = planKPIDetailId;
                scienceResearchService.getObj($scope.id).then(function (result) {
                    $scope.obj = result.data;
                    //$scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                });
                $scope.Save = function () {
                    scienceResearchService.Save($scope.obj).then(function (result) {
                        if (result == 1) {
                            alert("Lưu thành công");
                            $modalInstance.close();
                        }
                        else {
                            alert("Lưu thất bại");
                        }
                    });

                };

                $scope.cancel = function () {
                    $modalInstance.close();
                };
            }
    ]);
});