
define(['app/app', 'app/services/kpi/otherActivityDataService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService', 'app/services/kpi/staffService', 'moment'], function (app) {
    "use strict";

    //var HRMWebAppModule = angular.module('HRMWebApp');

    app.controller('otherActivityDataController', ['$scope', '$modal', '$rootScope', 'otherActivityDataService', 'departmentService',
            function ($scope, $modal, $rootScope, otherActivityDataService, departmentService) {
                var counter = 0;
                $scope.options = {
                    filter: "contains"
                }
                $scope.createWidget = false;
                $scope.grid = {};
                $scope.grid1 = {};
                $scope.scienceResearchDataName = '';
                $scope.isEdit = false;
                $scope.resultList = [];
                $scope.ActivityManageCode = '';
                $scope.ManageCode = '';
                $scope.departmentId = MANAGER.GUID_EMPTY;
                $scope.staffId = MANAGER.GUID_EMPTY;
                $scope.studyYear = '';
                $scope.StudyTerm = '';
                $scope.StudyTerms = [
                {
                    Id: 'HK01',
                    Name: 'Học kỳ I',
                },
                {
                    Id: 'HK02',
                    Name: 'Học kỳ II',
                },
                {
                    Id: 'CaNam',
                    Name: 'Cả năm',
                }];

                departmentService.getList().then(function (result) {
                    $scope.departments = result.data;
                    //$scope.departments.splice(0, 0, { Id: '00000000-0000-0000-0000-000000000000', Name: '... Tất cả đơn vị ...' });
                    //$scope.deptId = $scope.departments.length > 0 ? $scope.departments[0].Id : null;
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


                /////////// Master//////////////

                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            //if ($scope.deptId == undefined)
                            //    $scope.deptId = MANAGER.GUID_EMPTY;
                            var deptId = $scope.departmentId;
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
                $scope.dataSource1 = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            //if ($scope.deptId == undefined)
                            //    $scope.deptId = MANAGER.GUID_EMPTY;
                            var deptId = $scope.departmentId;
                            var studyYear = $scope.studyYear;
                            var activityManageCode = $scope.ActivityManageCode;
                            var manageCode = $scope.ManageCode;
                            var studyTerm = $scope.StudyTerm;
                            return otherActivityDataService.searchOtherActivityUser(deptId, studyYear, activityManageCode, manageCode, studyTerm).then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });

                $scope.obj = null;
                function renderCounter() {
                    var result = 0;
                    var page = parseInt($scope.dataSource1.page()) - 1;
                    var pageSize = parseInt($scope.dataSource1.pageSize());
                    result = page * pageSize;
                    return result;
                }

                //$scope.mainGridOptions = {
                //    sortable: true,
                //    pageable: {
                //        buttonCount: 7
                //    },
                //    //dataBound: function () {
                //    //    var rows = this.items();
                //    //    $(rows).each(function () {
                //    //        var index = $(this).index() + 1 + ($scope.dataSource.page() - 1) * $scope.dataSource.pageSize();
                //    //        var rowLabel = $(this).find(".row-number");
                //    //        $(rowLabel).html(index);
                //    //    });
                //    //},
                //    columns: [
                //    {
                //        title: "STT",
                //        template: function (dataItem) {
                //            var index = $scope.dataSource.indexOf(dataItem) + 1;
                //            return "<p style='text-align:center; margin: 0'>" + index + "</p>";
                //        },
                //        width: "45px"
                //    },
                //    {
                //        //template: "<a ng-click='Edit(\"#:data.Id #\")'  href='javascript:void(0)'>#:data.StaffName #</a>",
                //        field: "StaffName",
                //        title: "Họ tên"
                //    },

                //     {
                //         field: "DepartmentName",
                //         title: "Đơn vị"
                //     },
                //    {
                //        field: "Name",
                //        title: "Hoạt động"
                //    },
                //    //{
                //    //    field: "NumberOfTime",
                //    //    title: "Số lần"
                //    //},
                //    {
                //        field: "StudyTerm",
                //        title: "Học kỳ",
                //        width: "60px"
                //    },
                //    {
                //        field: "StudyYear",
                //        title: "Năm học",
                //        width: "100px"
                //    },
                //    {
                //        template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                //        width: "65px",
                //    }
                //    ],
                //};
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
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('DULIEUSELECTION', this.dataItem(selectedRows[0]).Id);
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
                         field: "ManageName",
                         title: "Tên danh mục"
                     },
                    {
                        field: "Date",
                        title: "Ngày hoạt động",
                        template: '#= kendo.toString(kendo.parseDate(Date), "MM/dd/yyyy")#'
                    },
                     {
                         field: "Name",
                         title: "Tên hoạt động"
                     },
                    {
                        field: "DonViCungCapName",
                        title: "Đơn vị cung cấp"
                    },
                    {
                        field: "UserNhap",
                        title: "Người nhập dữ liệu",
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
                    change: function (e) {
                        var selectedRows = this.select();
                        $rootScope.$broadcast('STAFFSELECTION', this.dataItem(selectedRows[0]).Id);
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
                        field: "Name",
                        title: "Tên hoạt động"
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
                                if ($scope.deptId == undefined)
                                    $scope.deptId = MANAGER.GUID_EMPTY;
                                var deptId = $scope.deptId;
                                var studyYear = $scope.studyYear;
                                var activityManageCode = $scope.ActivityManageCode;
                                var manageCode = $scope.ManageCode;
                                var studyTerm = $scope.StudyTerm;
                                return otherActivityDataService.searchOtherActivityUser(deptId, studyYear, activityManageCode, manageCode, studyTerm).then(function (result) {
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
                                var deptId = $scope.departmentId;
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
                        templateUrl: '/app/views/kpi/otherActivityData/detail.html',
                        controller: 'otherActivityDataDetailController',
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
                otherActivityDataService.getUserNhap().then(function (result) {
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
                    otherActivityDataService.getObj(Id).then(function (result) {
                        $scope.obj = result.data;
                        if ($scope.WebGroupId == "00000000-0000-0000-0000-000000000001" || $scope.WebGroupId == "00000000-0000-0000-0000-000000000002" || $scope.WebGroupId == "00000000-0000-0000-0000-000000000003" || $scope.obj.IdUserNhap == $scope.UserNhaps.UserName) {
                            otherActivityDataService.Delete($scope.obj).then(function () {
                                otherActivityDataService.getList().then(function (result) {
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

                    otherActivityDataService.getObjMaster(Id).then(function (result) {
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
                            otherActivityDataService.DeleteMulti($scope.obj).then(function () {
                                otherActivityDataService.getList().then(function (result) {
                                    $scope.grid.dataSource.read();
                                    $scope.grid1.dataSource.read();
                                });
                            });
                            //}
                            //else {
                            //    otherActivityDataService.Delete($scope.obj).then(function () {
                            //        otherActivityDataService.getList().then(function (result) {
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
    app.controller('otherActivityDataDetailController', ['$scope', '$rootScope', '$modalInstance', '$modal', '$filter', 'id', 'staffId', 'otherActivityDataService', 'staffService', 'departmentService',
        function ($scope, $rootScope, $modalInstance, $modal, $filter, id, staffId, otherActivityDataService, staffService, departmentService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Đổ dữ liệu hoạt động";
            var moment = require('moment');
            $scope.Id = id;
            $scope.obj = {};
            $scope.SubStaffIds = [];
            //$scope.staffIds = [];
            $scope.staffs = [];
            //$scope.staffNames = [];
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
                        //return staffService.getListByDepartmentId($scope.DepartmentId).then(function (result) {
                        //    options.success(result.data);
                        //});
                        //return staffService.getSearchOnlyProfessor().then(function (result) {
                        //    options.success(result.data);
                        //});
                    }
                },
            });

            //===========Không còn dùng nữa==============///
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
                    template: '<input ng-model = "dataItem.Checked" type="checkbox" ng-click="selectedChangeCheckbox()"></input>',
                    width: "35px",
                    title: "<input id='checkAll' type='checkbox' ng-click='checkAll()' ng-model='checked'></input>",
                    filterable: false,
                    sortable: false,
                },
                {
                    field: "Name",
                    title: "Họ tên"
                }]
            };
            $scope.onClick = function () {
                var view = $scope.grid.dataSource.view();
                for (var i = 0; i < view.length; i++) {
                    var item = view[i];
                    var found = $filter('filter')($scope.staffs, { Id: item.Id }, true);
                    if (item.Checked && found.length == 0) {
                        $scope.staffs.push(item);
                    }
                }
            }
            $scope.Delete = function (id) {
                $scope.staffs = $filter('filter')($scope.staffs, function (item, idx) { return item.Id != id; });
            }
            $scope.checkAll = function () {
                var view = $scope.grid.dataSource.view();
                for (var i = 0; i < view.length; i++) {
                    var item = view[i];
                    item.Checked = !$scope.checked;
                }
                $scope.checked = !$scope.checked;
            }
            $scope.selectedChangeCheckbox = function () {
                var checkedNumber = 0;
                var view = $scope.grid.dataSource.view();
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
            //=================End================//

            otherActivityDataService.getListTargetGroupdetail().then(function (result) {
                $scope.TargetGroupdetail = result.data;
                $scope.selectedChangeTagetGroup();
            });

            $scope.selectedChangeTagetGroup = function () {
                otherActivityDataService.getTargetGroupdetailType($scope.obj.NhomMucTieuId).then(function (result) {
                    $scope.TargetGroupdetailtype = result.data;
                    $.each(result.data, function (idx, item) {
                        $scope.obj.TargetGroupdetailType = item.TargetGroupDetailTypeId;
                    });
                });

                otherActivityDataService.getListProfessorCriterion($scope.obj.NhomMucTieuId).then(function (result) {
                    $scope.professorCriterions = result.data;
                    $scope.selectedChangeActivityGroup();
                });
            }

            otherActivityDataService.getListStudyYear().then(function (result) {
                $scope.studyYears = result.data;
            });

            $scope.selectedChangeActivityGroup = function () {
                otherActivityDataService.getListDictionaryByManageCode($scope.obj.ActivityManageCode).then(function (result) {
                    $scope.professorCriterionsDictionaries = result.data;
                });
                otherActivityDataService.getDVCC($scope.obj.ActivityManageCode).then(function (result) {
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
            //otherActivityDataService.getDonViCungCap().then(function (result) {
            //    $scope.obj.DonViCungCapId = result.data;
            //});

            $scope.uploadOptions = {
                async: {
                    saveUrl: "/OtherActivityImportData/SaveFileToData",
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
                //$scope.obj = {
                //    Id: MANAGER.GUID_EMPTY
                //};
                //  $scope.obj.StaffId = staffId;
                $scope.StudyTerms = [
                {
                    Id: 'HK01',
                    Name: 'Học kỳ I',
                },
                {
                    Id: 'HK02',
                    Name: 'Học kỳ II',
                },
                {
                    Id: 'CaNam',
                    Name: 'Cả năm',
                }];
                $scope.obj.StudyTerm = $scope.StudyTerms.length > 0 ? $scope.StudyTerms[0].Id : null
                otherActivityDataService.getUserNhap().then(function (result) {
                    $scope.UserNhaps = result.data;
                    $scope.obj.UserNhap = $scope.UserNhaps.HoVaTen;
                    $scope.obj.DonViCungCapName = $scope.UserNhaps.DepartmentName;
                    $scope.obj.IdUserNhap = $scope.UserNhaps.UserName;
                });
                var currentDate = moment();
                var currentYear = currentDate.format('YYYY');
                var currentMonth = currentDate.format('MM');
                //if (parseInt(currentMonth) <= 2 || parseInt(currentMonth) >= 9) {
                //    $scope.obj.StudyTerm = "HK01";
                //}
                //else {
                //    $scope.obj.StudyTerm = "HK02";
                //}
                otherActivityDataService.getListStudyYear().then(function (result) {
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
                            return otherActivityDataService.ListUserOtherActivity().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });
            } else {
                $scope.StudyTerms = [
                  {
                      Id: 'HK01',
                      Name: 'Học kỳ I',
                  },
                  {
                      Id: 'HK02',
                      Name: 'Học kỳ II',
                  },
                  {
                      Id: 'CaNam',
                      Name: 'Cả năm',
                  }];
                $scope.obj.StudyTerm = $scope.StudyTerms.length > 0 ? $scope.StudyTerms[0].Id : null;

                otherActivityDataService.getUserNhap().then(function (result) {
                    $scope.UserNhaps = result.data;
                });
                otherActivityDataService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return otherActivityDataService.ListOtherActivityUserDetail($scope.Id).then(function (result) {
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
                change: function (e) {
                    var selectedRows = this.select();
                    $rootScope.$broadcast('STAFFSELECTION', this.dataItem(selectedRows[0]).StaffCode);
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
                    field: "StaffCode",
                    title: "Mã cán bộ"
                },
                {
                    field: "StaffName",
                    title: "Họ tên"
                },
                {
                    field: "NumberOfTime",
                    title: "Số lần",
                }
                //, {
                //    template: function (dataItem) {
                //        var index = $scope.dataSource.indexOf(dataItem) + 1;
                //    }
                //    //template: "<div style='width: 30px;'><button ng-click='EditStaff(\"#:data.StaffCode#\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i></button></div>",
                //    //width: "65px",
                //}
                ]
            };
            $scope.EditStaff = function (StaffCode) {
                if (StaffCode == "") {
                    alert("Bạn chưa chọn phần tử");
                    return;
                }
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: '/app/views/kpi/otherActivityData/Adduser.html',
                    controller: 'otherActivityDataAddUserController',
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
                    if ($scope.TargetGroupdetailType == 0 || $scope.TargetGroupdetailType == 4 || $scope.TargetGroupdetailType == 5) {
                        if ($scope.DVCC.length > 0) {
                            $.each($scope.DVCC, function (ind, it) {
                                if ($scope.UserNhaps.DepartmentId != it) {
                                    alert("Bạn không có quyền đổ dữ liệu cho hoạt động này!");
                                }
                                else {
                                    otherActivityDataService.Save($scope.obj).then(function (result) {
                                        //if (result == MANAGER.GUID_EMPTY)
                                        alert("Thành công!");
                                        //else
                                        //    alert("Có lỗi xảy ra. Vui lòng thử lại!");
                                        $modalInstance.close(result);
                                    });
                                }
                            });
                        }
                        else {
                            alert("Hoạt động này chưa được phân quyền đơn vị cung cấp");
                        }
                    }
                    else {
                        otherActivityDataService.Save($scope.obj).then(function (result) {
                            alert("Thành công!");
                            $modalInstance.close(result);
                        });
                    }

                } else {
                    $scope.staffId = $scope.obj.IdUserNhap;
                    if ($scope.staffId.trim() != $scope.UserNhaps.UserName.trim()) {
                        alert("Bạn không có quyền chỉnh sửa hoạt động này!");
                    } else {
                        otherActivityDataService.Saves($scope.obj).then(function (result) {
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
    app.controller('otherActivityDataAddUserController', ['$scope', '$modalInstance', '$filter', 'id', 'StudyYear', 'StudyTerm', 'ActivityManageCode', 'ManageCode', 'Name', 'Date', 'DonViCungCapName', 'UserNhap', 'IdUserNhap', 'StaffCode', 'StaffName', 'NumberOfTime', 'otherActivityDataService', 'staffService', 'departmentService',
       function ($scope, $modalInstance, $filter, id, StudyYear, StudyTerm, ActivityManageCode, ManageCode, Name, Date, DonViCungCapName, UserNhap, IdUserNhap, StaffCode, StaffName, NumberOfTime, otherActivityDataService, staffService, departmentService) {
           $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
           $scope.obj = {};
           $scope.obj.Id = id;
           $scope.obj.StudyYear = StudyYear;
           $scope.obj.StudyTerm = StudyTerm;
           $scope.obj.ActivityManageCode = ActivityManageCode;
           $scope.obj.ManageCode = ManageCode;
           $scope.obj.Name = Name;
           $scope.obj.Date = Date;
           $scope.obj.DonViCungCapName = DonViCungCapName;
           $scope.obj.UserNhap = UserNhap;
           $scope.obj.IdUserNhap = IdUserNhap;
           $scope.obj.NumberOfTime = NumberOfTime;
           $scope.obj.StaffCode = StaffCode;
           $scope.obj.StaffName = StaffName;

           $scope.save = function () {
               otherActivityDataService.SaveUsers($scope.obj).then(function (result) {
                   $modalInstance.close(result);
               });
           }

           $scope.cancel = function () {
               $modalInstance.close();
           };
       }
    ]);
});
