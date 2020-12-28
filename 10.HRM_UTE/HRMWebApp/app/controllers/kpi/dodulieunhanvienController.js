
define(['app/app', 'app/services/kpi/dodulieunhanvienService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService', 'app/services/kpi/staffService', 'moment'], function (app) {
    "use strict";

    app.controller('dodulieunhanvienController', ['$scope', '$modal', '$rootScope', 'dodulieunhanvienService', 'departmentService',
            function ($scope, $modal, $rootScope, dodulieunhanvienService, departmentService) {
                var counter = 0;
                $scope.options = {
                    filter: "contains"
                }
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.grid1 = {};
                $scope.isEdit = false;
                $scope.CapMucTieu = 0;
                $scope.TargetGroupDetailId = MANAGER.GUID_EMPTY;
                $scope.CapDanhMuc = [
                {
                    Id: '1',
                    Name: 'Cấp 1',
                },
                {
                    Id: '2',
                    Name: 'Cấp 2',
                },
                {
                    Id: '3',
                    Name: 'Cấp 3',
                }];


                $scope.selectedChangeCap = function () {
                    if ($scope.CapMucTieu == '1') {
                        dodulieunhanvienService.getListTargetGroupDetail().then(function (result) {
                            $scope.TargetGroupDetail = result.data;
                        });
                    }
                    if ($scope.CapMucTieu == '2') {
                        dodulieunhanvienService.getListTargetGroupDetail().then(function (result) {
                            $scope.TargetGroupDetail2 = result.data;
                        });
                    }
                    if ($scope.CapMucTieu == '3') {
                        dodulieunhanvienService.getListTargetGroupDetail().then(function (result) {
                            $scope.TargetGroupDetail3 = result.data;
                        });
                    }
                    dodulieunhanvienService.getListManageCode($scope.CapMucTieu).then(function (result) {
                        $scope.ListManageCode = result.data;
                    });
                }

             
                dodulieunhanvienService.getListStudyYear().then(function (result) {
                    $scope.studyYears = result.data;
                });
                

                /////////// Master//////////////

                //$scope.dataSource = new kendo.data.DataSource({
                //    dataType: 'json',
                //    transport: {
                //        read: function (options) {
                //            var capmuctieu = $scope.CapMucTieu;
                //            //var studyYear = $scope.studyYear;
                //            var TargetGroupDetailId = $scope.TargetGroupDetailId;
                //            return dodulieunhanvienService.searchOtherActivity(capmuctieu, TargetGroupDetailId).then(function (result) {
                //                options.success(result.data);
                //            });
                //        }
                //    },
                //    pageSize: 20
                //});
                //$scope.dataSource1 = new kendo.data.DataSource({
                //    dataType: 'json',
                //    transport: {
                //        read: function (options) {
                //            var capmuctieu = $scope.CapMucTieu;
                //            //var studyYear = $scope.studyYear;
                //            var TargetGroupDetailId = $scope.TargetGroupDetailId;
                //            return dodulieunhanvienService.searchOtherActivityUser(capmuctieu, TargetGroupDetailId).then(function (result) {
                //                options.success(result.data);
                //            });
                //        }
                //    },
                //    pageSize: 20
                //});

                $scope.obj = null;
                function renderCounter() {
                    var result = 0;
                    var page = parseInt($scope.dataSource1.page()) - 1;
                    var pageSize = parseInt($scope.dataSource1.pageSize());
                    result = page * pageSize;
                    return result;
                }

                $scope.mainGridOptions = {
                    sortable: true,
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
                    pageable: {
                        buttonCount: 7
                    },
                    columns: [
                    {
                        template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i></button></div>",
                        width: "65px",
                    },
                    {
                        title: "STT",
                        template: function (dataItem) {
                            var index = $scope.dataSource.indexOf(dataItem) + 1;
                            return "<p style='text-align:center; margin: 0'>" + index + "</p>";
                        },
                        width: "45px"
                    },
                     {
                         field: "TargetDetail",
                         title: "Tên mục tiêu"
                     },
                    {
                        field: "StartTime",
                        title: "Ngày bắt đầu",
                        template: '#= kendo.toString(kendo.parseDate(Date), "MM/dd/yyyy")#'
                    },
                    {
                        field: "EndTime",
                        title: "Ngày kết thúc",
                        template: '#= kendo.toString(kendo.parseDate(Date), "MM/dd/yyyy")#'
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='DeleteMaster(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "65px",
                    }
                    ],
                };
                //////////////////////

                ///////////Detail//////////

                $scope.mainGridOptionsUser = {
                    sortable: true,
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
                    pageable: {
                        buttonCount: 7
                    },
                    columns: [
                    //{
                    //    template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i></button></div>",
                    //    width: "65px",
                    //},
                    {
                        title: "STT",
                        template: function (dataItem) {
                            var index = $scope.dataSource1.indexOf(dataItem) + 1;
                            return "<p style='text-align:center; margin: 0'>" + index + "</p>";
                        },
                        width: "45px"
                    },
                    {
                        field: "TargetDetail",
                        title: "Tên mục tiêu"
                    },
                    {
                        field: "StaffCode",
                        title: "Mã cán bộ"
                    },

                     {
                         field: "StaffName",
                         title: "Họ tên"
                     },
                    {
                        field: "NumberOfTime",
                        title: "Số lần/Số điểm",
                    },
                    {
                        template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                        width: "65px",
                    }
                    ],
                };

                ////////////////

                $scope.numericOptions = {
                    format: "n0",
                    min: 0
                }
                $scope.SearchDept = function () {
                    $scope.dataSource1 = new kendo.data.DataSource({
                        dataType: 'json',
                        transport: {
                            read: function (options) {
                                var capmuctieu = $scope.CapMucTieu;
                                var TargetGroupDetailId = $scope.TargetGroupDetailId;
                                return dodulieunhanvienService.searchOtherActivityUser(capmuctieu, TargetGroupDetailId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                    $scope.dataSource = new kendo.data.DataSource({
                        dataType: 'json',
                        transport: {
                            read: function (options) {
                                var capmuctieu = $scope.CapMucTieu;
                                var TargetGroupDetailId = $scope.TargetGroupDetailId;
                                return dodulieunhanvienService.searchOtherActivity(capmuctieu, TargetGroupDetailId).then(function (result) {
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
                        templateUrl: '/app/views/kpi/dodulieunhanvien/detail.html',
                        controller: 'dodulieunhanhvienDetailController',
                        backdrop: 'static',
                        resolve: {
                            id: function () {
                                return MANAGER.GUID_EMPTY;
                            },
                            staffId: function () {
                                return $scope.staffId;
                            }
                        }
                    }).result.then(function (result) {
                        onUploadSuccess();
                    });
                };

                $scope.Edit = function (Id) {
                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/dodulieunhanvien/detail.html',
                        controller: 'dodulieunhanhvienDetailController',
                        resolve: {
                            id: function () {
                                return Id;
                            },
                            staffId: function () {
                                return $scope.staffId;
                            },
                        }
                    }).result.then(function (result) {
                        onUploadSuccess();
                    });
                };
                function onUploadSuccess() {
                    $scope.grid.dataSource.read();
                    $scope.grid.refresh();
                    $scope.grid1.dataSource.read();
                    $scope.grid1.refresh();
                }
                dodulieunhanvienService.getUserNhap().then(function (result) {
                    $scope.UserNhaps = result.data;
                    $scope.WebGroupId = $scope.UserNhaps.WebGroupId
                });

                $scope.Delete = function (Id) {

                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }
                    var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                    if (!valid)
                        return;
                    dodulieunhanvienService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        if ($scope.WebGroupId == "00000000-0000-0000-0000-000000000001" || $scope.WebGroupId == "00000000-0000-0000-0000-000000000002" || $scope.WebGroupId == "00000000-0000-0000-0000-000000000003" || $scope.obj.IdUserNhap == $scope.UserNhaps.UserName) {
                            dodulieunhanvienService.Delete($scope.obj).then(function () {
                                dodulieunhanvienService.getList().then(function (result) {
                                    $scope.grid1.dataSource.read();
                                    $scope.grid.dataSource.read();
                                });
                            });
                        }
                        else {
                            alert("Bạn không có quyền xóa hoạt động này");
                            return;
                        }
                    });
                };

                $scope.DeleteMaster = function (Id) {

                    if (Id == "") {
                        alert("Bạn chưa chọn phần tử");
                        return;
                    }

                    dodulieunhanvienService.getObjMaster(Id).then(function (result) {
                        $scope.obj = result.data;
                        // if ($scope.obj.length > 1) {
                        $.each($scope.obj, function (idx, item) {
                            $scope.NguoiNhap = item.IdUserNhap;
                            return false;
                        });
                        //}
                        //else {
                        //    $scope.NguoiNhap = $scope.obj.IdUserNhap;
                        //}

                        if ($scope.WebGroupId == "00000000-0000-0000-0000-000000000001" || $scope.WebGroupId == "00000000-0000-0000-0000-000000000002" || $scope.WebGroupId == "00000000-0000-0000-0000-000000000003" || $scope.NguoiNhap == $scope.UserNhaps.UserName) {

                            var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                            if (!valid)
                                return;
                            //if ($scope.obj.length > 1) {
                            dodulieunhanvienService.DeleteMulti($scope.obj).then(function () {
                                dodulieunhanvienService.getList().then(function (result) {
                                    $scope.grid.dataSource.read();
                                    $scope.grid1.dataSource.read();
                                });
                            });
                            //}
                            //else {
                            //    dodulieunhanvienService.Delete($scope.obj).then(function () {
                            //        dodulieunhanvienService.getList().then(function (result) {
                            //            $scope.grid1.dataSource.read();
                            //            $scope.grid.dataSource.read();
                            //        });
                            //    });
                            //}
                        }
                        else {
                            alert("Bạn không có quyền xóa hoạt động này");
                            return;
                        }
                    });


                };
            }
    ]);
    app.controller('dodulieunhanhvienDetailController', ['$scope', '$rootScope', '$modalInstance', '$modal', '$filter', 'id', 'staffId', 'dodulieunhanvienService', 'staffService', 'departmentService',
        function ($scope, $rootScope, $modalInstance, $modal, $filter, id, staffId, dodulieunhanvienService, staffService, departmentService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Đổ MTCL nhân viên";
            var moment = require('moment');
            $scope.Id = id;
            $scope.obj = {};
            $scope.SubStaffIds = [];
            $scope.staffs = [];
            $scope.DepartmentList = [];
            $scope.ListStaffs = [];
            $scope.obj.DepartmentId = "";
            $scope.checked = false;
            $scope.DVCC = [];
            $scope.options = {
                filter: "contains"
            }
            $scope.staffResource = new kendo.data.DataSource({
                dataType: 'json',
                transport: {
                    read: function (options) {
                    }
                },
            });

            $scope.CapDanhMuc = [
                {
                    Id: '1',
                    Name: 'Cấp 1',
                },
                {
                    Id: '2',
                    Name: 'Cấp 2',
                },
                {
                    Id: '3',
                    Name: 'Cấp 3',
                }];

            $scope.selectedChangeCap = function () {
                if ($scope.obj.CapMucTieu == '1') {
                    dodulieunhanvienService.getListTargetGroupDetail().then(function (result) {
                        $scope.TargetGroupDetail = result.data;
                    });
                }
                if ($scope.obj.CapMucTieu == '2') {
                    dodulieunhanvienService.getListTargetGroupDetail().then(function (result) {
                        $scope.TargetGroupDetail2 = result.data;
                    });
                }
                if ($scope.obj.CapMucTieu == '3') {
                    dodulieunhanvienService.getListTargetGroupDetail().then(function (result) {
                        $scope.TargetGroupDetail3 = result.data;
                    });
                }
                dodulieunhanvienService.getListManageCode($scope.obj.CapMucTieu).then(function (result) {
                    $scope.ListManageCode = result.data;
                });
            }
            dodulieunhanvienService.getListStudyYear().then(function (result) {
                $scope.studyYears = result.data;
            });

            $scope.selectedChangeActivityGroup = function () {
                dodulieunhanvienService.getListDictionaryByManageCode($scope.obj.ActivityManageCode).then(function (result) {
                    $scope.professorCriterionsDictionaries = result.data;
                });
                dodulieunhanvienService.getDVCC($scope.obj.ActivityManageCode).then(function (result) {
                    $scope.ProfessorCriterion = result.data;
                    $scope.DVCC = $scope.ProfessorCriterion.DepartmentIds;
                });
            };
            $scope.selectedChangeDepartment = function () {
                $scope.staffResource = {};
                $scope.checked = false;
                staffService.getListByDepartmentId($scope.obj.DepartmentId).then(function (result) {
                    $scope.staffResource = result.data;
                });
            };
            $scope.numericOptions = {
                format: "n0",
                min: 0
            }
            //dodulieunhanvienService.getDonViCungCap().then(function (result) {
            //    $scope.obj.DonViCungCapId = result.data;
            //});

            $scope.uploadOptions = {
                async: {
                    saveUrl: "/MTCLImportData/SaveFileToData",
                    autoUpload: true
                },
                success: function (result) {
                    if (result.response == "1") {
                        alert("Import file lỗi!");
                    }
                    else
                        alert("Import file thành công!");
                    onUploadSuccess();
                }
            }
            function onUploadSuccess() {
                $scope.grid.dataSource.read();
                $scope.grid.refresh();
            }



            if ($scope.isNew) {
                var currentDate = moment();
                var currentYear = currentDate.format('YYYY');
                var currentMonth = currentDate.format('MM');

                dodulieunhanvienService.getListStudyYear().then(function (result) {
                    $.each(result.data, function (index, item) {
                        var itemYearBefore = item.StudyYear.substring(0, 4);
                        var itemYearAfter = item.StudyYear.slice(-4);
                        if (currentYear == itemYearAfter) {
                            if (parseInt(currentMonth) < 9)
                                $scope.obj.StudyYear = item.StudyYear;
                            else
                                $scope.obj.StudyYear = itemYearAfter + '-' + (parseInt(itemYearAfter) + 1);
                            return false;
                        }
                    });
                });
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return dodulieunhanvienService.ListUserMTCL().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
            } else {

                dodulieunhanvienService.getUserNhap().then(function (result) {
                    $scope.UserNhaps = result.data;
                });
                dodulieunhanvienService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return dodulieunhanvienService.ListMTCLUserDetail($scope.Id).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
            }

            function renderCounter() {
                var result = 0;
                var page = parseInt($scope.dataSource.page()) - 1;
                var pageSize = parseInt($scope.dataSource.pageSize());
                result = page * pageSize;
                return result;
            }
            $scope.mainGridUser = {
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
                pageable: {
                    buttonCount: 7
                },
                pageable: {
                    buttonCount: 7
                },
                columns: [
                {
                    title: "STT",
                    template: function (dataItem) {
                        var index = $scope.dataSource.indexOf(dataItem) + 1;
                        return "<p style='text-align:center; margin: 0'>" + index + "</p>";
                    },
                    width: "45px"
                },
                {
                    field: "MaCanBo",
                    title: "Mã cán bộ"
                },
                {
                    field: "TenCanBo",
                    title: "Họ tên"
                },
                {
                    field: "SoDiem",
                    title: "Số điểm",
                }]
            };
            $scope.EditStaff = function (StaffCode) {
                if (StaffCode == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/dodulieunhanvien/Adduser.html',
                    controller: 'dodulieunhanvienAddUserController',
                    resolve: {
                        id: function () {
                            return $scope.obj.Id;
                        },
                        StudyYear: function () {
                            return $scope.obj.StudyYear;
                        },
                        StudyTerm: function () {
                            return $scope.obj.StudyTerm;
                        },
                        ActivityManageCode: function () {
                            return $scope.obj.ActivityManageCode;
                        },
                        ManageCode: function () {
                            return $scope.obj.ManageCode;
                        },
                        Name: function () {
                            return $scope.obj.Name;
                        },
                        Date: function () {
                            return $scope.obj.Date;
                        },
                        DonViCungCapName: function () {
                            return $scope.obj.DonViCungCapName;
                        },
                        UserNhap: function () {
                            return $scope.obj.UserNhap;
                        },
                        IdUserNhap: function () {
                            return $scope.obj.IdUserNhap
                        },
                        StaffCode: function () {
                            return $scope.obj.StaffCode
                        },
                        StaffName: function () {
                            return $scope.obj.StaffName;
                        },
                        NumberOfTime: function () {
                            return $scope.obj.NumberOfTime;
                        }
                    }
                }).result.then(function (result) {
                    onUploadSuccess();
                });
            };
            $scope.Add = function () {
                $scope.staffId = $scope.obj.IdUserNhap;
                if ($scope.staffId.trim() != $scope.UserNhaps.UserName.trim()) {
                    alert("Bạn không có quyền thêm nhân viên!");
                    return;
                }
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/otherActivityData/Adduser.html',
                    controller: 'otherActivityDataAddUserController',
                    backdrop: 'static',
                    resolve: {
                        id: function () {
                            return MANAGER.GUID_EMPTY;
                        },
                        CapMucTieu: function () {
                            return $scope.obj.CapMucTieu
                        },
                        StudyYear: function () {
                            return $scope.obj.StudyYear;
                        },
                        ManageCode: function () {
                            return $scope.obj.ManageId;
                        },
                        Name: function () {
                            return $scope.obj.TargetDetail;
                        },
                        StartTime: function () {
                            return $scope.obj.StartTime;
                        },
                        TargetGroupDetailId: function () {
                            return $scope.obj.TargetGroupDetailId;
                        },
                        BasicResource: function () {
                            return $scope.obj.BasicResource;
                        },
                        EndTime: function () {
                            return $scope.obj.EndTime;
                        },
                        //UserNhap: function () {
                        //    return $scope.obj.UserNhap;
                        //},
                        //IdUserNhap: function () {
                        //    return $scope.obj.IdUserNhap
                        //},
                        StaffCode: function () {
                            return "";
                        },
                        StaffName: function () {
                            return "";
                        },
                        NumberOfTime: function () {
                            return "";
                        },
                    }
                }).result.then(function (result) {
                    onUploadSuccess();
                });
            };

            $scope.save = function () {

                if ($scope.isNew) {
                    //if ($scope.DVCC.length > 0) {
                    //    $.each($scope.DVCC, function (ind, it) {
                    //        if ($scope.UserNhaps.DepartmentId != it) {
                    //            alert("Bạn không có quyền đổ dữ liệu cho hoạt động này!");
                    //        }
                    //        else {
                    //            dodulieunhanvienService.Save($scope.obj).then(function (result) {
                    //                //if (result == MANAGER.GUID_EMPTY)
                    //                alert("Thành công!");
                    //                //else
                    //                //    alert("Có lỗi xảy ra. Vui lòng thử lại!");
                    //                $modalInstance.close(result);
                    //            });
                    //        }
                    //    });
                    //}
                    //else {
                    //    alert("Hoạt động này chưa được phân quyền đơn vị cung cấp");
                    //}
                    dodulieunhanvienService.Save($scope.obj).then(function (result) {
                        alert("Thành công!");
                        $modalInstance.close(result);
                    });
                } else {
                    $scope.staffId = $scope.obj.IdUserNhap;
                    if ($scope.staffId.trim() != $scope.UserNhaps.UserName.trim()) {
                        alert("Bạn không có quyền chỉnh sửa hoạt động này!");
                    } else {
                        dodulieunhanvienService.Saves($scope.obj).then(function (result) {
                            $modalInstance.close(result);
                        });
                    }
                }
            };

            $scope.cancel = function () {
                $modalInstance.close();
            };
        }
    ]);
    app.controller('otherActivityDataAddUserController', ['$scope', '$modalInstance', '$filter', 'id', 'StudyYear', 'StudyTerm', 'ActivityManageCode', 'ManageCode', 'Name', 'Date', 'DonViCungCapName', 'UserNhap', 'IdUserNhap', 'StaffCode', 'StaffName', 'NumberOfTime', 'dodulieunhanvienService', 'staffService', 'departmentService',
       function ($scope, $modalInstance, $filter, id, StudyYear, StudyTerm, ActivityManageCode, ManageCode, Name, Date, DonViCungCapName, UserNhap, IdUserNhap, StaffCode, StaffName, NumberOfTime, dodulieunhanvienService, staffService, departmentService) {
           $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
           $scope.obj = {};
           $scope.obj.Id = id;
           $scope.obj.StudyYear = StudyYear;
           $scope.obj.StudyTerm = StudyTerm;
           $scope.obj.ActivityManageCode = ActivityManageCode;
           $scope.obj.ManageId = ManageCode;
           $scope.obj.Name = Name;
           $scope.obj.Date = Date;
           $scope.obj.DonViCungCapName = DonViCungCapName;
           $scope.obj.UserNhap = UserNhap;
           $scope.obj.IdUserNhap = IdUserNhap;
           $scope.obj.NumberOfTime = NumberOfTime;
           $scope.obj.StaffCode = StaffCode;
           $scope.obj.StaffName = StaffName;

           $scope.save = function () {
               dodulieunhanvienService.SaveUsers($scope.obj).then(function (result) {
                   $modalInstance.close(result);
               });
           }

           $scope.cancel = function () {
               $modalInstance.close();
           };
       }
    ]);
});
