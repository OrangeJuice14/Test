define(['app/app', 'app/services/kpi/ratingKPIService', 'app/services/kpi/professorCriterionService', 'app/services/kpi/professorOtherActivityService', 'app/services/kpi/scienceResearchService', 'moment'], function (app) {
    app.controller('ratingKPIInformationController', ['$scope', '$modal', '$modalInstance', 'planDetailId', 'criterionTypeId', 'targetGroupDetailTypeId', 'criterionId', 'ratingKPIService', 'professorCriterionService', 'professorOtherActivityService', 'scienceResearchService',
            function ($scope, $modal, $modalInstance, planDetailId, criterionTypeId, targetGroupDetailTypeId, criterionId, ratingKPIService, professorCriterionService, professorOtherActivityService, scienceResearchService) {
                $scope.criterionTypeId = criterionTypeId;
                $scope.targetGroupDetailTypeId = targetGroupDetailTypeId;
                $scope.obj = {};
                $scope.obj.Id = planDetailId;
                $scope.obj.ActivityIds = [];
                $scope.obj.ScienceResearchIds = [];
                
                var selectTabStrip = function () {
                    setTimeout(function () {
                        var tabStrip = $("#mainInformationTabId").kendoTabStrip().data("kendoTabStrip");
                        if (targetGroupDetailTypeId == 4) {
                            tabStrip.select("li:first");
                        }
                        if (targetGroupDetailTypeId == 5) {
                            tabStrip.select("li:last");
                        }
                    }, 100);
                }
                selectTabStrip();

                ////////////////////Professor Other Activity////////////////////          
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
                                return professorCriterionService.getDictionnaryByCriterionId(criterionId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        }
                    }
                };

                $scope.newActivity = function () {
                    $scope.isEdit = false;
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/ratingKPI/activityDetail.html',
                        controller: 'activityDetailResultController',
                        resolve: {
                            planKPIDetailId: function () {
                                return planDetailId;
                            },
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            criterionTypeId: function () {
                                return criterionTypeId;
                            }
                        }
                    }).result.then(function (result) {
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
                        templateUrl: '/app/views/kpi/ratingKPI/activityDetail.html',
                        controller: 'activityDetailResultController',
                        resolve: {
                            planKPIDetailId: function () {
                                return planDetailId;
                            },
                            id: function () {
                                return activityId;
                            },
                            criterionTypeId: function () {
                                return criterionTypeId;
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
                            return professorOtherActivityService.getListByPlanDetail(planDetailId).then(function (result) {
                                options.success(result.data);
                                $.each(result.data, function (idx, item) {
                                    $scope.obj.ActivityIds.push(item.CriterionDictionaryId);
                                })
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
                        template: "<div style='width: 30px;'><button ng-if='#:data.IsRating#==1' ng-click='editActivity(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "65px"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-if='#:data.IsRating#==1' ng-click='deleteActivity(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "65px"
                    }],
                };
                $scope.saveActivity = function () {
                    ratingKPIService.SaveActivity($scope.obj).then(function (result) {
                        if (result != "0") {
                            $scope.activityGrid.dataSource.read();
                            $scope.activityGrid.refresh();
                        }
                        else
                            if (result == "0") {
                                alert("Lưu thất bại");
                            }
                    });
                }

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
                                return professorCriterionService.getDictionnaryByCriterionId(criterionId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        }
                    }
                };

                $scope.newResearch = function () {
                    $scope.isEdit = false;
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/planKPIDetailInformation/scienceResearchDetail.html',
                        controller: 'scienceResearchDetailResultController',
                        resolve: {
                            planKPIDetailId: function () {
                                return planDetailId;
                            },
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            criterionTypeId: function () {
                                return criterionTypeId;
                            }
                        }
                    }).result.then(function (result) {
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
                        controller: 'scienceResearchDetailResultController',
                        resolve: {
                            planKPIDetailId: function () {
                                return planDetailId;
                            },
                            id: function () {
                                return researchId;
                            },
                            criterionTypeId: function () {
                                return criterionTypeId;
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
                            return scienceResearchService.getListByPlanDetail(planDetailId).then(function (result) {
                                options.success(result.data);
                                $.each(result.data, function (idx, item) {
                                    $scope.obj.ScienceResearchIds.push(item.CriterionDictionaryId);
                                })
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
                        template: "<div style='width: 30px;'><button ng-if='#:data.IsRating#==1' ng-click='editResearch(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                        width: "65px"
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-if='#:data.IsRating#==1' ng-click='deleteResearch(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "65px"
                    }],
                };
                $scope.saveResearch = function () {
                    ratingKPIService.SaveActivity($scope.obj).then(function (result) {
                        if (result != "0") {
                            $scope.researchGrid.dataSource.read();
                            $scope.researchGrid.refresh();
                        }
                        else
                            if (result == "0") {
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

                $scope.save = function () {
                    alert("Lưu thành công");
                    $modalInstance.close();
                }

                $scope.cancel = function () {
                    $modalInstance.close();
                }
            }
    ]);
    app.controller('activityDetailResultController', ['$scope', '$modal', '$modalInstance', 'id', 'planKPIDetailId', 'criterionTypeId', 'professorOtherActivityService', 'fileAttachmentService',
            function ($scope, $modal, $modalInstance, id, planKPIDetailId, criterionTypeId, professorOtherActivityService, fileAttachmentService) {
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                $scope.id = id;
                $scope.criterionTypeId = criterionTypeId;
                $scope.fileAttachments = [];
                $scope.options = {
                    format: "n0",
                    max: 100,
                    min: 0
                }
                $scope.orders = {
                    format: "n0",
                    min: 1
                }
                $scope.planKPIDetailId = planKPIDetailId;

                if ($scope.isNew && criterionTypeId == 5) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        NumberOfHour: 0,
                        NumberOfTime: 0,
                        PlanKPIDetailId: planKPIDetailId,
                        IsRating: 1 //IsRating = 1: hoạt động được thêm bên đánh giá, 0: bên kế hoạch
                    };
                    professorOtherActivityService.getMaxOrderNumberActivity(planKPIDetailId).then(function (result) {
                        $scope.obj.OrderNumber = result.data + 1;
                    });
                } else {
                    professorOtherActivityService.getObj(id).then(function (result) {
                        $scope.obj = result.data;
                    });
                }

                //////////////////////////Upload File///////////////////////////
                $scope.uploadActivityDetailFile = function () {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/ratingKPI/resultDetailFileDetail.html',
                        controller: 'activityDetailFileController',
                        //backdrop: 'static',
                        //keyboard: false,
                        resolve: {
                            activityDetailId: function () {
                                return id;
                            }
                        }
                    }).result.finally(function () {
                        setTimeout(function () {
                            //refresh file list
                            fileAttachmentService.getListByActivityDetail(id).then(function (result) {
                                $scope.fileAttachments = result.data;
                            });
                            $scope.$apply();
                        }, 500);
                    });
                }

                fileAttachmentService.getListByActivityDetail(id).then(function (result) {
                    $scope.fileAttachments = result.data;
                });

                $scope.downloadFile = function (Id) {
                    fileAttachmentService.downloadFile(Id);
                }
                ////////////////////////////////////////////////////////////////

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
    app.controller('scienceResearchDetailResultController', ['$scope', '$modalInstance', 'id', 'planKPIDetailId', 'criterionTypeId', 'scienceResearchService',
            function ($scope, $modalInstance, id, planKPIDetailId, criterionTypeId, scienceResearchService) {
                $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
                $scope.id = id;
                $scope.criterionTypeId = criterionTypeId;
                $scope.options = {
                    format: "n0",
                    max: 100,
                    min: 0
                }
                $scope.orders = {
                    format: "n0",
                    min: 1
                }
                $scope.planKPIDetailId = planKPIDetailId;

                if ($scope.isNew && criterionTypeId == 5) {
                    $scope.obj = {
                        Id: MANAGER.GUID_EMPTY,
                        Name: "",
                        NumberOfResearch: 0,
                        PlanKPIDetailId: planKPIDetailId,
                        IsRating: 1 //IsRating = 1: hoạt động được thêm bên đánh giá, 0: bên kế hoạch
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
    app.controller('activityDetailFileController', ['$scope', '$modalInstance', 'activityDetailId', 'fileAttachmentService',
                function ($scope, $modalInstance, activityDetailId, fileAttachmentService) {
                    $scope.uploadOptions = {
                        async: {
                            saveUrl: "/FileUpload/SaveActivityDetailFile?activityDetailId=" + activityDetailId,
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
                                return fileAttachmentService.getListByActivityDetail(activityDetailId).then(function (result) {
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
                            template: "<div style='width: 30px;'><button ng-click='downloadFile(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-download'></i>  </button></div>",
                            width: "65px"
                        },
                        {
                            template: "<div style='width: 30px;'><button ng-click='deleteFile(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                            width: "65px",
                        }],
                    };
                    $scope.downloadFile = function (Id) {
                        fileAttachmentService.downloadFile(Id);
                    }

                    $scope.deleteFile = function (Id) {
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
                                $scope.fileGrid.dataSource.read();
                                $scope.fileGrid.refresh();
                            });
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

                    $scope.ok = function () {
                        $modalInstance.close();
                    };
                }
    ]);
});