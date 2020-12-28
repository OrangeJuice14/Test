define(['app/app', 'app/services/kpi/methodService', 'app/services/kpi/planKPIDetailService', 'app/services/kpi/departmentService', 'app/services/kpi/staffService', 'app/services/kpi/criterionService', 'app/services/kpi/professorCriterionService', 'app/services/kpi/planKPIDetail_KPIService', 'app/services/kpi/professorOtherActivityService', 'app/services/kpi/targetGroupDetailService', 'app/services/kpi/fileAttachmentService', 'app/services/kpi/scienceResearchService', 'app/services/kpi/danhmucMTCLService', 'moment'], function (app) {
    app.controller('planKPIDetailInformationController', ['$scope', '$modal', '$modalInstance', 'targetId', 'id', 'userRole', 'planStaffId', 'staffId', 'plan', 'department', 'agentObjectTypeId', 'isAddition', 'methodService', 'planKPIDetailService', 'departmentService', 'staffService', 'criterionService', 'professorCriterionService', 'planKPIDetail_KPIService', 'professorOtherActivityService', 'fileAttachmentService', 'targetGroupDetailService', 'scienceResearchService', 'danhmucMTCLService',

        function ($scope, $modal, $modalInstance, targetId, id, userRole, planStaffId, staffId, plan, department, agentObjectTypeId, isAddition, methodService, planKPIDetailService, departmentService, staffService, criterionService, professorCriterionService, planKPIDetail_KPIService, professorOtherActivityService, fileAttachmentService, targetGroupDetailService, scienceResearchService, danhmucMTCLService) {
            var temList = [];
            var maxRecord;
            var staffType = (agentObjectTypeId == 6 || agentObjectTypeId == 12) ? 1 : 2;
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.professorActivityList = [];
            $scope.methodList = [];
            $scope.title = "";
            $scope.plan = plan;
            $scope.department = department;
            $scope.userRole = userRole;
            $scope.SubDepartmentIds = [];
            $scope.SubStaffIds = [];
            $scope.staffIds = [];
            $scope.staffNames = [];
            $scope.gridSubStaff = {};
            $scope.agentObjectTypeId = agentObjectTypeId;
            $scope.subDepartments = [];
            $scope.targetGroupDetailDictionaries = [];
            $scope.density = {};
            $scope.minValue = 0;
            $scope.maxValue = 100;
            $scope.densityDictionaries = [];
            $scope.professorCriterions = [];
            $scope.grid = {};
            $scope.targetGroupDetailTypeId = 0;
            $scope.fromCriterion = null;
            $scope.haveMethod = false;

            $scope.options = {
                format: "n0",
                max: 100,
                min: 0
            }
            $scope.orders = {
                format: "n0",
                min: 1
            }
            $scope.DensityOptions = {
                format: "n0",
                max: 100,
                min: 1
            }
            $scope.MaxRecordOptions = {
                format: "n0"
            }
            $scope.SubStaffsOptions = {
                format: "n0",
                min: 1,
                max: 100
                //change: subStaffDensityChange,
            }
            function subStaffDensityChange() {
                var totalResult = 0;
                $.each($scope.obj.PlanDetailSubStaffs, function (idx, item) {
                    totalResult += item.Density;
                });
                //if (totalResult != 100) {
                //    alert("Tổng trọng số giao việc phải bằng 100");

                //}
            }

            $scope.subStaffDensityChange = function () {
                var totalResult = 0;
                $.each($scope.obj.PlanDetailSubStaffs, function (idx, item) {
                    totalResult += item.Density;
                });
                //if (totalResult != 100) {
                //    alert("Tổng trọng số giao việc phải bằng 100");
                //}
            }
            $scope.implementStaff_Faculty = function (e) {

            }
            //$scope.densityNumeric = {};
            //$scope.$on("kendoWidgetCreated", function () {
            //    $scope.densityNumeric.element.on("keyup", function () {
            //        $scope.densityNumeric.value($scope.densityNumeric.element.val());
            //        $scope.densityNumeric.trigger("change");
            //    });
            //});
            //$(".inputDensity").bind("keydown", function (e) {
            //    if (e.which == 13) {
            //        ALERT('A');
            //        this.trigger("keydown", { which: 9 });
            //    }
            //});
            //$(".inputDensity").keypress(function (e) {
            //    alert('press');
            //    if (e.which == 13) {
            //        ALERT('A');
            //        this.trigger("keydown", { which: 9 });
            //    }
            //});

            //$scope.manageCodeChange = function (manageCodeId) {
            //    $.each($scope.ManageCodes, function (idx, item) {
            //        if (item.Id == manageCodeId && item.ProfessorCriterion != null) {
            //            professorCriterionService.getDictionnaryByProfessorCriterionId(item.ProfessorCriterion.Id).then(function (result) {
            //                $scope.professorActivityList = result.data;
            //            });
            //        }
            //        else {
            //            $scope.professorActivityList = [];
            //        }
            //    });
            //}

            criterionService.getListManageCodeDTO(staffType).then(function (result) {
                $scope.ManageCodes = result.data;
            });

            $scope.selectManageCode = function () {
                danhmucMTCLService.getListDanhMuc($scope.obj.ManageCode.Id).then(function (result) {
                    $scope.DanhMucMTCLs = result.data;
                });
            }

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
                placeholder: "Chọn phó khoa ...",
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
            $scope.departmentStaffResource = {
                placeholder: "Chọn người thực hiện ...",
                dataTextField: "Name",
                dataValueField: "Id",
                valuePrimitive: true,
                filter: "contains",
                autoBind: false,
                dataSource: {
                    transport: {
                        read: function (options) {
                            return staffService.getStaffInFaculty($scope.agentObjectTypeId, $scope.agentObjectTypeId == 4 ? MANAGER.GUID_EMPTY : department.departmentId, $scope.plan.Id).then(function (result) {
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
                        CriterionId: MANAGER.GUID_EMPTY,
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
                        IsAddition: isAddition,
                        StaffLeader: {},
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
                            IsAddition: isAddition,
                            FromCriterionId: MANAGER.GUID_EMPTY
                        }
                    }
                    else {
                        return {
                            Id: MANAGER.GUID_EMPTY,
                            Name: "",
                            ExcecuteMethod: "",
                            BasicResource: "",
                            CriterionId: MANAGER.GUID_EMPTY,
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
                            IsAddition: isAddition,
                            FromCriterionId: MANAGER.GUID_EMPTY
                        }
                    }
            }

            $scope.canEditMethods = $scope.isNew;
            var isFromCriterion = true;

            if ($scope.isNew) {
                $scope.obj = newPlanDetail();
                planKPIDetailService.getMaxOrderNumberPlanKPIDetail(planStaffId, targetId).then(function (result) {
                    $scope.obj.OrderNumber = result.data + 1;
                });
                $scope.obj.StartTime = $scope.plan.startPlan;
                $scope.obj.EndTime = $scope.plan.endPlan;
                $scope.obj.PlanDetailSubStaffs = [];
                var tabStrip = $("#mainInformationTabId").kendoTabStrip().data("kendoTabStrip")
                getDensityDictionary();
            } else {
                planKPIDetailService.getObj(id, $scope.agentObjectTypeId).then(function (result) {
                    $scope.obj = result.data;
                    $scope.obj.PlanStaffId = planStaffId;
                    $scope.obj.PlanId = $scope.plan.Id;
                    $scope.obj.StaffId = staffId;
                    $scope.obj.TargetGroupDetail = { Id: targetId };
                    $scope.IsMoved = $scope.obj.IsMoved;
                    $scope.fromCriterion = $scope.obj.FromCriterion;
                    $scope.canEditMethods = $scope.isNew || $scope.obj.FromCriterionId == MANAGER.GUID_EMPTY;
                    $scope.professorCriterionId = $scope.obj.FromProfessorCriterionId;
                    temList = $scope.obj.PlanDetailSubStaffs;
                    if ($scope.planStaffGrid != undefined && $scope.obj.PlanDetailSubStaffs != undefined) {
                        $.each($scope.planStaffGrid.dataSource.data(), function (idx, item) {
                            $.each($scope.obj.PlanDetailSubStaffs, function (idx, item2) {
                                if (item2.StaffId == item.Id)
                                    item.Checked = true;
                            });
                        });
                    }
                    if ($scope.planProfessorGrid != undefined && $scope.obj.PlanDetailSubStaffs != undefined) {
                        $.each($scope.planProfessorGrid.dataSource.data(), function (idx, item) {
                            $.each($scope.obj.PlanDetailSubStaffs, function (idx2, item2) {
                                if (item2.StaffId == item.Id)
                                    item.Checked = true;
                            });
                        });
                    }
                    //$scope.obj.Method = { Id: result.data.MethodId }

                    getDensityDictionary();
                    getCriterionDictionary();
                    $scope.selectManageCode();
                });
            }

            criterionService.getDictionnaryByTargetGroupDetailId(targetId).then(function (result) {
                $scope.targetGroupDetailDictionaries = result.data;
            });

            function getCriterionDictionary() {
                if ($scope.obj.ManageCode != null && $scope.obj.ManageCode.ProfessorCriterion != null) {
                    professorCriterionService.getDictionnaryByProfessorCriterionId($scope.obj.ManageCode.ProfessorCriterion.Id).then(function (result) {
                        $scope.professorActivityList = result.data;
                    });
                }
            }

            function getDensityDictionary() {
                criterionService.getDensityDictionary().then(function (result) {
                    $scope.densityDictionaries = result.data;

                    if ($scope.obj != undefined && $scope.obj.MaxRecord != undefined) {
                        $.each($scope.densityDictionaries, function (idx, item) {
                            if ($scope.obj.MaxRecord > item.Record && $scope.obj.MaxRecord < item.MaxRecord) {
                                $scope.density = item.Id;
                                $scope.minValue = item.Record;
                                $scope.maxValue = item.MaxRecord;
                            }
                        });
                    }
                });
            }
            $scope.changeDensity = function (densityId) {
                criterionService.getDensityDictionaryById(densityId).then(function (result) {
                    $scope.minValue = result.data.Record;
                    $scope.maxValue = result.data.MaxRecord;
                });

            }
            planKPIDetailService.getMeasureUnits().then(function (result) {
                $scope.MeasureUnits = result.data;
            });


            targetGroupDetailService.getTargetGroupDetailTypeId(targetId).then(function (result) {
                $scope.targetGroupDetailTypeId = result.data;
            });

            // lấy danh sách tab targetgroupdetail cho trưởng bộ môn
            targetGroupDetailService.GetListbyGV().then(function (result) {
                $scope.targetGroupDetails = result.data;
                $scope.obj.TargetGroupDetailId = $scope.targetGroupDetails[0].Id;
                //$scope.obj.TargetGroupDetail = result.data[0];
            });

            // lấy ProfessorCriterion by targetgroupdetail
            $scope.GetProfessorCriterion = function () {
                professorCriterionService.getListByTargetGroupDetailId($scope.obj.TargetGroupDetailId).then(function (result) {
                    $scope.ProfessorCriterion = result.data;
                    $scope.obj.ProfessorCriterionId = $scope.ProfessorCriterion[0].Id;
                    //$scope.obj.TargetGroupDetail = result.data[0];
                });
            }

            // lấy ProfessorCriterion by targetgroupdetail
            $scope.GetCriterionDictionary = function () {
                professorCriterionService.getDictionnaryByCriterionId($scope.obj.ProfessorCriterionId).then(function (result) {
                    $scope.CriterionDictionary = result.data;
                    $scope.obj.CriterionDictionaryId = $scope.CriterionDictionary[0].Id;
                    //$scope.obj.TargetGroupDetail = result.data[0];
                });
            }

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
                            setTimeout(function () {
                                if ($scope.professorCriterionId != undefined) {
                                    return professorCriterionService.getDictionnaryByCriterionId($scope.professorCriterionId).then(function (result) {
                                        $.each(result.data, function (idx, item) {
                                            item.Name = item.Name + ' - Số giờ: ' + item.NumberOfHour;
                                        });
                                        options.success(result.data);
                                    });
                                }
                            }, 300);
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
                            return departmentService.getDepartment($scope.agentObjectTypeId).then(function (result) {
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
                    width: "65px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='deletePlanDetailFile(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                    width: "65px",
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
            $scope.onClick = function () {
                $scope.obj.PlanDetailSubStaffs = [];
                var staffNumber = 0;
                var totalDensity = 0;
                for (var i = 0; i < $scope.planStaffGrid.dataSource.data().length; i++) {
                    var item = $scope.planStaffGrid.dataSource.at(i);
                    if (item.Checked) {
                        staffNumber++;
                        var density = 0;
                        $.each(temList, function (idx, item2) {
                            if (item2.StaffId == item.Id)
                                density = item2.Density
                        });
                        $scope.obj.PlanDetailSubStaffs.push({ StaffName: item.Name, Density: density, StaffId: item.Id });
                        $scope.SubStaffIds.push(item.Id);
                    }
                }
                //$.each($scope.obj.PlanDetailSubStaffs, function (idx, item) { //chia đều trọng số cho từng nhân viên
                //    //item.Density = parseInt((100 / staffNumber).toFixed());
                //    item.Density = parseInt(Math.floor(100 / staffNumber));
                //    totalDensity += item.Density;
                //});
                //if (totalDensity != 100) { //nếu chia ra số dư
                //    var x = 100 - totalDensity;
                //    var i = 0;
                //    while (x > 0 && i < $scope.obj.PlanDetailSubStaffs.length) {
                //        $scope.obj.PlanDetailSubStaffs[i].Density++;
                //        i++;
                //        x--;
                //        if (x > 0 && i == $scope.obj.PlanDetailSubStaffs.length)
                //            i = 0; //lặp đến khi nào hết số dư
                //    }
                //}
            }
            //$scope.$watch('obj.PlanDetailSubStaffs', function (newValue, oldValue) {
            //    $scope.totalDensity = 0;
            //    if ($scope.obj != undefined) {
            //        $.each($scope.obj.PlanDetailSubStaffs, function (idx, item) {
            //            $scope.totalDensity += item.Density;
            //        });
            //    }
            //}, true);
            $scope.checkAll = function (dataGrid) {
                var view = dataGrid.dataSource.view();
                for (var i = 0; i < view.length; i++) {
                    var item = view[i];
                    item.Checked = !$scope.checked;
                }
                $scope.checked = !$scope.checked;
            }
            $scope.selectedChangeCheckbox = function (dataGrid) {
                var checkedNumber = 0;
                var view = dataGrid.dataSource.view();
                for (var i = 0; i < view.length; i++) {
                    var item = view[i];
                    if (item.Checked)
                        checkedNumber++;
                }
                if (checkedNumber == view.length) {
                    $scope.checked = true;
                    $('#checkAll').prop('checked', true);
                }
                else {
                    $scope.checked = false;
                    $('#checkAll').prop('checked', false);
                }
            }
            $scope.staffGridResource = new kendo.data.DataSource({
                dataType: 'json',
                transport: {
                    read: function (options) {
                        return staffService.getStaffByAgentObjectType(99, $scope.department.departmentId, userRole).then(function (result) {
                            options.success(result.data);

                            if ($scope.obj != undefined && $scope.obj.PlanDetailSubStaffs != undefined) {
                                $.each($scope.planStaffGrid.dataSource.data(), function (idx, item) {
                                    $.each($scope.obj.PlanDetailSubStaffs, function (idx, item2) {
                                        if (item2.StaffId == item.Id)
                                            item.Checked = true;
                                    });
                                });
                            }
                        });
                    }
                },
            });

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
                height: 300,
                columns: [
                {
                    field: "Checked",
                    template: '<input ng-model = "dataItem.Checked" type="checkbox" ng-click="selectedChangeCheckbox(planStaffGrid)"></input>',
                    width: "40px",
                    title: "<input id='checkAll' type='checkbox' ng-click='checkAll(planStaffGrid)' ng-model='checked'></input>",
                    filterable: false,
                    sortable: false
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

            $scope.selectProfessor = function () {
                $scope.obj.PlanDetailSubStaffs = [];
                for (var i = 0; i < $scope.planProfessorGrid.dataSource.data().length; i++) {
                    var item = $scope.planProfessorGrid.dataSource.at(i);
                    if (item.Checked) {
                        var density = 0;
                        $.each(temList, function (idx, item2) {
                            if (item2.StaffId == item.Id)
                                density = item2.Density
                        });
                        $scope.obj.PlanDetailSubStaffs.push({ StaffName: item.Name, Density: density, StaffId: item.Id });
                        $scope.SubStaffIds.push(item.Id);
                    }
                }
            }

            //$scope.PlanDetailSubStaffs =
            //otherActivityDataService.getListProfessorCriterion().then(function (result) {
            //    $scope.professorCriterions = result.data;
            //    $scope.selectedChangeActivityGroup();
            //});
            $scope.professorGridResource = {
                dataType: 'json',
                transport: {
                    read: function (options) {
                        return staffService.getProfessorInSubject($scope.agentObjectTypeId).then(function (result) {
                            options.success(result.data);

                            if ($scope.obj != undefined && $scope.obj.PlanDetailSubStaffs != undefined) {
                                $.each($scope.planProfessorGrid.dataSource.data(), function (idx, item) {
                                    $.each($scope.obj.PlanDetailSubStaffs, function (idx2, item2) {
                                        if (item2.StaffId == item.Id)
                                            item.Checked = true;
                                    });
                                });
                            }
                        });
                    }
                }
            };

            $scope.professorOption = {
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
                height: 300,
                columns: [
                    {
                        field: "Checked",
                        filterable: false,
                        template: '<input ng-model = "dataItem.Checked" type="checkbox" ng-click="selectedChangeCheckbox(planProfessorGrid)"></input>',
                        width: "40px",
                        title: "<input id='checkAll' type='checkbox' ng-click='checkAll(planProfessorGrid)' ng-model='checked'></input>",
                        sortable: false
                    },
                    {
                        field: "Name",
                        title: "Họ tên"
                    }
                ]
            };

            staffService.getStaffByAgentObjectType($scope.agentObjectTypeId, $scope.agentObjectTypeId == 4 ? MANAGER.GUID_EMPTY : $scope.department.departmentId, userRole).then(function (result) {
                $scope.StaffLeaders = result.data;
                if ($scope.isNew) {
                    if ($scope.obj != undefined && $scope.obj.StaffLeader != undefined && result.data.length > 0) {
                        $scope.obj.StaffLeader.Id = result.data[0].Id;
                    }
                }
            });


            $scope.newMethod = function () {
                $scope.isEdit = false;
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/methodDetail.html',
                    controller: 'methodDetailController',
                    windowClass: 'app-modal-window',
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
                        },
                        department: function () {
                            return { departmentId: $scope.department };
                        },
                        userRole: function () {
                            return { userRoleId: $scope.userRole }
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
                    windowClass: 'app-modal-window',
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
                        },
                        department: function () {
                            return { departmentId: $scope.department };
                        },
                        userRole: function () {
                            return { userRoleId: $scope.userRole }
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
                    field: "OrderNumber",
                    title: "STT",
                    width: "50px"
                },
                {
                    field: "Name",
                    title: "Nội dung"
                },
                {
                    field: "StartTimeString",
                    title: "Ngày bắt đầu",
                    width: "110px"
                },
                {
                    field: "EndTimeString",
                    title: "Ngày kết thúc",
                    width: "110px"
                },
                {
                    template: "<div style='width: 30px;' ><button ng-click='editMethod(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                    //ng-if='canEditMethods && fromCriterion==null'
                    width: "65px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='deleteMethod(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                    width: "65px"
                }],
            };
            ////////////////////Professor Other Activity////////////////////          
            $scope.newActivity = function () {
                $scope.isEdit = false;
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/activityDetail.html',
                    controller: 'activityDetailController',
                    resolve: {
                        planKPIDetailId: function () {
                            return id;
                        },
                        id: function () {
                            return MANAGER.GUID_EMPTY;
                        },
                        planStaffId: function () {
                            return planStaffId;
                        },
                        targetGroupDetailId: function () {
                            return targetId;
                        },
                        professorCriterionId: function () {
                            return $scope.professorCriterionId;
                        },
                        criterionTypeId: function () {
                            return $scope.obj.CriterionTypeId;
                        }
                    }
                }).result.then(function (result) {
                    if (id == MANAGER.GUID_EMPTY || typeof (id) == "undefined") {
                        id = result;
                        $scope.obj.Id = id;
                    }
                    $scope.activityGrid.dataSource.read();
                    $scope.activityGrid.refresh();

                });
            }

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
                        },
                        criterionTypeId: function () {
                            return $scope.obj.CriterionTypeId;
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
                    title: "Số giờ",
                    width: "65px"
                },
                {
                    field: "NumberOfTime",
                    title: "Số lần",
                    width: "65px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='editActivity(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                    width: "65px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='deleteActivity(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                    width: "65px"
                }],
            };
            $scope.saveActivity = function () {
                planKPIDetailService.SaveActivity($scope.obj).then(function (result) {
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
                    field: "OrderNumber",
                    title: "STT",
                    width: "50px"
                },
                {
                    field: "Name",
                    title: "KPI đăng ký"
                },
                {
                    field: "MeasureUnitName",
                    title: "Đơn vị tính",
                    width: "120px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='editKPI(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                    width: "65px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-disabled='fromCriterion!=null' ng-click='deleteKPI(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                    width: "65px",
                }],
            };


            ////////////////////////////////////////Science research//////////////////////////////
            $scope.researchResource = {
                placeholder: "Chọn hoạt động nghiên cứu khoa học ...",
                dataTextField: "Name",
                dataValueField: "Id",
                valuePrimitive: true,
                filter: "contains",
                autoBind: false,
                dataSource: {
                    transport: {
                        read: function (options) {
                            setTimeout(function () {
                                if ($scope.professorCriterionId != undefined) {
                                    return professorCriterionService.getDictionnaryByCriterionId($scope.professorCriterionId).then(function (result) {
                                        $.each(result.data, function (idx, item) {
                                            item.Name = item.Name + ' - Số tiết: ' + item.Record;
                                        });
                                        options.success(result.data);
                                    });
                                }
                            }, 300);
                        }
                    }
                }
            };

            $scope.newResearch = function () {
                $scope.isEdit = false;
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/planKPIDetailInformation/scienceResearchDetail.html',
                    controller: 'scienceResearchDetailController',
                    resolve: {
                        planKPIDetailId: function () {
                            return id;
                        },
                        id: function () {
                            return MANAGER.GUID_EMPTY;
                        },
                        planStaffId: function () {
                            return planStaffId;
                        },
                        targetGroupDetailId: function () {
                            return targetId;
                        },
                        professorCriterionId: function () {
                            return $scope.professorCriterionId;
                        },
                        criterionTypeId: function () {
                            return $scope.obj.CriterionTypeId;
                        }
                    }
                }).result.then(function (result) {
                    if (id == MANAGER.GUID_EMPTY || typeof (id) == "undefined") {
                        id = result;
                        $scope.obj.Id = id;
                    }
                    $scope.researchGrid.dataSource.read();
                    $scope.researchGrid.refresh();

                });
            }

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
                        },
                        criterionTypeId: function () {
                            return $scope.obj.CriterionTypeId;
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
                    field: "NumberOfHour",
                    title: "Số tiết",
                    width: "75px"
                },
                {
                    field: "NumberOfResearch",
                    title: "Số lượng",
                    width: "75px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='editResearch(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                    width: "65px"
                },
                {
                    template: "<div style='width: 30px;'><button ng-click='deleteResearch(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                    width: "65px",
                }],
            };
            $scope.saveResearch = function () {

                planKPIDetailService.SaveActivity($scope.obj).then(function (result) {
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

                //if (agentObjectTypeId == 5 || agentObjectTypeId == 8) //Trưởng khoa, phó khoa giao việc cho giáo vụ khoa
                //{
                //    if ($scope.obj.SubStaffIds.length > 0) {
                //        //var staffNumber = 0;
                //        //$.each($scope.obj.SubStaffIds, function (idx, item) {
                //        //    staffNumber++;
                //        //});
                //        //var density = parseInt((100 / staffNumber).toFixed());
                //        $scope.obj.PlanDetailSubStaffs.length = 0;
                //        $.each($scope.obj.SubStaffIds, function (idx, item) {
                //            $scope.obj.PlanDetailSubStaffs.push({ Density: 100, StaffId: item });
                //        });
                //    }
                //}

                //if (agentObjectTypeId == 3) {
                //    if ($scope.obj.PlanDetailSubStaffs.length > 0) {
                //        var totalResult = 0;
                //        $.each($scope.obj.PlanDetailSubStaffs, function (idx, item) {
                //            totalResult += item.Density;
                //        });
                //        //if (totalResult != 100) {
                //        //    alert("Tổng trọng số giao việc phải bằng 100");
                //        //    return;
                //        //}
                //    }
                //}


                //Kiểm tra đã có method hay chưa
                if (!$scope.isNew) {
                    methodService.getCheckPlanDetailMethod($scope.obj.Id).then(function (result1) {
                        $scope.haveMethod = result1.data;
                        // if ($scope.haveMethod || agentObjectTypeId == "1" || $scope.targetGroupDetailTypeId == 3) {
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
                                        //alert("Lưu thành công");
                                        Notify("Lưu thành công", 'top-right', '2000', 'success', 'fa-check-square-o', true);
                                        $modalInstance.close(result);
                                    }
                                }
                                else {
                                    if ($scope.obj.chuyennhom == true) {
                                        planKPIDetailService.Chuyennhom($scope.obj).then(function (result) {
                                            $scope.IsLoading = false;
                                            $scope.IsMoved = true;
                                            Notify("Chuyển nhóm thành công", 'top-right', '1000', 'success', 'fa-check-square-o', true);
                                        });
                                    }

                                    Notify("Lưu thành công", 'top-right', '2000', 'success', 'fa-check-square-o', true);
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
                        //  }
                        //else {
                        //    alert("Chưa có các bước thực hiện");
                        //}
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
                                    //alert("Lưu thành công");
                                    Notify("Lưu thành công", 'top-right', '2000', 'success', 'fa-check-square-o', true);
                                    $modalInstance.close(result);
                                }
                            }
                            else {
                                //alert("Lưu thành công");
                                Notify("Lưu thành công", 'top-right', '2000', 'success', 'fa-check-square-o', true);
                                $modalInstance.close(result);
                            }
                            //Chọn tab thứ 2
                            var tabToActive = $("#theSecondTab");
                            //$("#mainInformationTabId").kendoTabStrip().data("kendoTabStrip").activateTab(tabToActive);
                            $("#mainInformationTabId").data("kendoTabStrip").select(1);

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

    app.controller('methodDetailController', ['$scope', '$modalInstance', 'id', 'plan', 'planKPIDetailId', 'planStaffId', 'targetGroupDetailId', 'department', 'userRole', 'methodService', 'planKPIDetailService', 'staffService', 'departmentService',
            function ($scope, $modalInstance, id, plan, planKPIDetailId, planStaffId, targetGroupDetailId, department, userRole, methodService, planKPIDetailService, staffService, departmentService) {
                var moment = require('moment');
                $scope.obj = {};
                $scope.department = department;
                $scope.userRole = userRole;
                var temList = [];
                $scope.SubStaffIds = [];
                var temListDep = [];
                $scope.obj.PlanDetailSubStaffs = [];
                $scope.obj.LeadDepartment = [];
                var data = [];
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
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                $scope.orders = {
                    format: "n0",
                    min: 1
                }

                $scope.planStart = plan.startPlan;
                $scope.planEnd = plan.endPlan;
                //$scope.minDate = new Date();

                // girdview đơn vị cùng thực hiện //
                $scope.departmentsDataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return departmentService.getMainDepartment().then(function (result) {
                                options.success(result.data);

                                if ($scope.obj != undefined && $scope.obj.LeadDepartment != undefined) {
                                    $.each($scope.grid.dataSource.data(), function (idx, item) {
                                        $.each($scope.obj.LeadDepartment, function (idx, item2) {
                                            if (item2.DepartmentId.Id == item.Id)
                                                item.Checked = true;
                                        });
                                    });
                                }
                            });
                        }
                    }
                });

                $scope.departmentsmainoptions = {
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
                    height: 200,
                    columns: [
                    {
                        field: "Checked",
                        template: '<input ng-model = "dataItem.Checked" type="checkbox" ng-click="selectedChangeCheckboxDep(grid)"></input>',
                        width: "40px",
                        title: "<input id='checkAllDep' type='checkbox' ng-click='checkAllDep(grid)' ng-model='checked'></input>",
                        filterable: false,
                        sortable: false
                    },
                    {
                        field: "Name",
                        title: "Đơn vị"
                    }
                    ]
                };

                $scope.onClickDep = function () {
                    $scope.obj.LeadDepartment = [];
                    var depNumber = 0;
                    var DiemSo = 0;
                    for (var i = 0; i < $scope.grid.dataSource.data().length; i++) {
                        var item = $scope.grid.dataSource.at(i);
                        if (item.Checked) {
                            depNumber++;
                            var density = 0;
                            $.each(temListDep, function (idx, item2) {
                                if (item2.Id == item.Id)
                                    density = item2.DiemSo
                            });
                            $scope.obj.LeadDepartment.push({ DepartmentId: { Id: item.Id, Name: item.Name }, DiemSo: item.density });
                        }
                    }
                }
                $scope.selectedChangeCheckboxDep = function (dataGrid) {
                    var checkedNumber = 0;
                    var view = dataGrid.dataSource.view();
                    for (var i = 0; i < view.length; i++) {
                        var item = view[i];
                        if (item.Checked)
                            checkedNumber++;
                    }
                    if (checkedNumber == view.length) {
                        $scope.checked = true;
                        $('#checkAllDep').prop('checked', true);
                    }
                    else {
                        $scope.checked = false;
                        $('#checkAllDep').prop('checked', false);
                    }
                }
                $scope.checkAllDep = function (dataGrid) {
                    var view = dataGrid.dataSource.view();
                    for (var i = 0; i < view.length; i++) {
                        var item = view[i];
                        item.Checked = !$scope.checked;
                    }
                    $scope.checked = !$scope.checked;
                }

                $scope.onClick = function () {
                    $scope.obj.PlanDetailSubStaffs = [];
                    var staffNumber = 0;
                    var totalDensity = 0;

                    for (var i = 0; i < $scope.planStaffGrid.dataSource.data().length; i++) {
                        var item = $scope.planStaffGrid.dataSource.at(i);
                        var data = temList.filter(r=>r.StaffId != item.Id)

                        if (item.Checked) {
                            staffNumber++;
                            var density = 0;
                            $.each(temList, function (idx, item2) {
                                if (item2.StaffId == item.Id)
                                    density = item2.Density
                            });
                            $scope.obj.PlanDetailSubStaffs.push({ StaffName: item.Name, StaffId: item.Id });
                            $scope.SubStaffIds.push(item.Id);
                        }

                    }
                    if (data.length > 0) {
                        $.each(data, function (i, it) {
                            $scope.obj.PlanDetailSubStaffs.push({ StaffName: it.StaffName, StaffId: it.StaffId });
                            $scope.SubStaffIds.push(it.StaffId);
                        })

                    }
                }

                $scope.checkAll = function (dataGrid) {
                    var view = dataGrid.dataSource.view();
                    for (var i = 0; i < view.length; i++) {
                        var item = view[i];
                        item.Checked = !$scope.checked;
                    }
                    $scope.checked = !$scope.checked;
                }
                $scope.selectedChangeCheckbox = function (dataGrid) {
                    var checkedNumber = 0;
                    var view = dataGrid.dataSource.view();
                    for (var i = 0; i < view.length; i++) {
                        var item = view[i];
                        if (item.Checked)
                            checkedNumber++;
                    }
                    if (checkedNumber == view.length) {
                        $scope.checked = true;
                        $('#checkAll').prop('checked', true);
                    }
                    else {
                        $scope.checked = false;
                        $('#checkAll').prop('checked', false);
                    }
                }

                $scope.staffGridResource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return staffService.getStaffByAgentObjectType(99, $scope.department.departmentId.departmentId, $scope.userRole.userRoleId).then(function (result) {
                                options.success(result.data);

                                if ($scope.obj != undefined && $scope.obj.PlanDetailSubStaffs != undefined) {
                                    $.each($scope.planStaffGrid.dataSource.data(), function (idx, item) {
                                        $.each($scope.obj.PlanDetailSubStaffs, function (idx, item2) {
                                            if (item2.StaffId == item.Id)
                                                item.Checked = true;
                                        });
                                    });
                                }
                            });
                        }
                    },
                });
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
                    height: 200,
                    columns: [
                    {
                        field: "Checked",
                        template: '<input ng-model = "dataItem.Checked" type="checkbox" ng-click="selectedChangeCheckbox(planStaffGrid)"></input>',
                        width: "40px",
                        title: "<input id='checkAll' type='checkbox' ng-click='checkAll(planStaffGrid)' ng-model='checked'></input>",
                        filterable: false,
                        sortable: false
                    },
                    {
                        field: "Name",
                        title: "Họ tên"
                    }
                    ]
                };

                if ($scope.isNew) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        StartTime: plan.startPlan,
                        EndTime: plan.endPlan,
                        PlanKPIDetail: { Id: planKPIDetailId, TargetGroupDetail: { Id: targetGroupDetailId } },
                        PlanStaff: planStaffId,
                    };
                    methodService.getMaxOrderNumberMethods(planKPIDetailId).then(function (result) {
                        $scope.obj.OrderNumber = result.data + 1;
                    });
                } else {
                    methodService.getObj(id).then(function (result) {
                        $scope.obj = result.data;
                        $scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                        $scope.PlanStaff = planStaffId;
                        temList = result.data.PlanDetailSubStaffs;
                    });
                    //temList = $scope.obj.PlanDetailSubStaffs;
                    if ($scope.planStaffGrid != undefined && $scope.obj.PlanDetailSubStaffs != undefined) {
                        $.each($scope.planStaffGrid.dataSource.data(), function (idx, item) {
                            $.each($scope.obj.PlanDetailSubStaffs, function (idx2, item2) {
                                if (item2.StaffId == item.Id)
                                    item.Checked = true;

                            });
                        });
                    }
                    var DepartmentId = { DepartmentId: { Id: $scope.department.departmentId.departmentId, Name: $scope.department.departmentId.departmentName } }

                    $scope.obj.LeadDepartment.push(DepartmentId)
                    if ($scope.grid != undefined && $scope.obj.LeadDepartment != undefined) {
                        $.each($scope.grid.dataSource.data(), function (idx, item) {
                            $.each($scope.obj.LeadDepartment, function (idx2, item2) {
                                if (item2.DepartmentId.Id == item.Id)
                                    item.Checked = true;
                            });
                        });
                    }
                }

                $scope.save = function () {
                    $scope.compareStart = $scope.dateCompare($scope.planStart, $scope.obj.StartTime);
                    $scope.compareEnd = $scope.dateCompare($scope.obj.EndTime, $scope.planEnd);
                    $scope.compareTime = $scope.dateCompare($scope.obj.EndTime, $scope.obj.StartTime);

                    if ($scope.compareStart > 0 || $scope.compareEnd > 0) {
                        alert("Ngày phải nằm trong khoảng thời gian của kế hoạch!")
                    }
                    else
                        if ($scope.compareTime < 0) {
                            alert("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!")
                        }
                        else {
                            methodService.Save($scope.obj).then(function (result) {
                                if (result != MANAGER.GUID_EMPTY) {
                                    //alert("Lưu thành công");
                                    $modalInstance.close(result);
                                }
                                else {
                                    alert("Lưu thất bại");
                                }
                            });
                        }
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
                            //alert("Lưu thành công");
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
                $scope.options = {
                    format: "n0",
                    min: 0
                }
                $scope.orders = {
                    format: "n0",
                    min: 1
                }
                planKPIDetailService.getMeasureUnits().then(function (result) {
                    $scope.MeasureUnits = result.data;
                });
                if ($scope.isNew) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        PlanKPIDetail: { Id: planKPIDetailId, PlanStaff: { Id: planStaffId }, TargetGroupDetail: { Id: targetGroupDetailId } }
                    };
                    planKPIDetail_KPIService.getMaxOrderNumberKPI(planKPIDetailId).then(function (result) {
                        $scope.obj.OrderNumber = result.data + 1;
                    });
                } else {
                    planKPIDetail_KPIService.getObj(id).then(function (result) {
                        $scope.obj = result.data;
                        $scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                    });
                }
                $scope.save = function () {
                    planKPIDetail_KPIService.Save($scope.obj).then(function (result) {
                        if (result != MANAGER.GUID_EMPTY) {
                            //alert("Lưu thành công");
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
    app.controller('activityDetailController', ['$scope', '$modalInstance', 'id', 'planKPIDetailId', 'criterionTypeId', 'professorCriterionId', 'professorOtherActivityService',
            function ($scope, $modalInstance, id, planKPIDetailId, criterionTypeId, professorCriterionId, professorOtherActivityService) {
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                $scope.id = id;
                $scope.criterionTypeId = criterionTypeId;
                $scope.options = {
                    format: "n2",
                    min: 0
                }
                $scope.orders = {
                    format: "n0",
                    min: 1
                }
                $scope.planKPIDetailId = planKPIDetailId;
                //professorOtherActivityService.getObj(id).then(function (result) {
                //    $scope.obj = result.data;
                //    //$scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                //});

                if ($scope.isNew && criterionTypeId == 5) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        NumberOfHour: 0,
                        NumberOfTime: 0,
                        PlanKPIDetailId: planKPIDetailId
                    };
                    professorOtherActivityService.getMaxOrderNumberActivity(planKPIDetailId).then(function (result) {
                        $scope.obj.OrderNumber = result.data + 1;
                    });
                } else {
                    professorOtherActivityService.getObj(id).then(function (result) {
                        $scope.obj = result.data;
                    });
                }

                $scope.Save = function () {
                    professorOtherActivityService.Save($scope.obj).then(function (result) {
                        if (result == 1) {
                            //alert("Lưu thành công");
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
    app.controller('scienceResearchDetailController', ['$scope', '$modalInstance', 'id', 'planKPIDetailId', 'criterionTypeId', 'scienceResearchService',
            function ($scope, $modalInstance, id, planKPIDetailId, criterionTypeId, scienceResearchService) {
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                $scope.id = id;
                $scope.criterionTypeId = criterionTypeId;
                $scope.options = {
                    format: "n2",
                    min: 0
                }
                $scope.orders = {
                    format: "n0",
                    min: 1
                }
                $scope.planKPIDetailId = planKPIDetailId;
                //scienceResearchService.getObj(id).then(function (result) {
                //    $scope.obj = result.data;
                //    //$scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                //});

                if ($scope.isNew && criterionTypeId == 5) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        NumberOfResearch: 0,
                        NumberOfHour: 0,
                        PlanKPIDetailId: planKPIDetailId
                    };
                    scienceResearchService.getMaxOrderNumberResearch(planKPIDetailId).then(function (result) {
                        $scope.obj.OrderNumber = result.data + 1;
                    });
                } else {
                    scienceResearchService.getObj(id).then(function (result) {
                        $scope.obj = result.data;
                    });
                }

                $scope.Save = function () {
                    scienceResearchService.Save($scope.obj).then(function (result) {
                        if (result == 1) {
                            //alert("Lưu thành công");
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