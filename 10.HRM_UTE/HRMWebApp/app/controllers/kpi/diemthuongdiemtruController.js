define(['app/app', 'app/services/kpi/diemthuongdiemtruService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService','app/services/kpi/staffService', 'moment'], function (app) {
    "use strict";
    app.controller('diemthuongdiemtruController', ['$scope', '$modal', '$rootScope', 'diemthuongdiemtruService', 'departmentService',
           function ($scope, $modal, $rootScope, diemthuongdiemtruService, departmentService) {
               var counter = 0;
               $scope.options = {
                   filter: "contains"
               }
               diemthuongdiemtruService.getListStudyYear().then(function (result) {
                   $scope.studyYears = result.data;
               });
               $scope.selectedChangeActivityGroup = function () {
                   diemthuongdiemtruService.getListDictionaryByManageCode($scope.ActivityManageCode).then(function (result) {
                       $scope.professorCriterionsDictionaries = result.data;
                   });
               };

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

               $scope.dataSource = new kendo.data.DataSource({
                   dataType: 'json',
                   transport: {
                       read: function (options) {
                           var studyYear = $scope.studyYear;
                           var studyTerm = $scope.StudyTerm;
                           return diemthuongdiemtruService.searchdiemthuongdiemtru(studyYear, studyTerm).then(function (result) {
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
                           var studyYear = $scope.studyYear;
                           var studyTerm = $scope.StudyTerm;
                           return diemthuongdiemtruService.searchdiemthuongdiemtruuser(studyYear, studyTerm).then(function (result) {
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

               $scope.mainGridOptions = {
                   sortable: true,
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
                       field: "NoiDung",
                       title: "Nội dung hoạt động"
                   },
                   {
                       field: "UserNhap",
                       title: "Người nhập dữ liệu",
                   },
                   {
                       field: "DonViCungCap",
                       title: "Đơn vị cung cấp"
                   }
                   ],
               };

               $scope.mainGridOptionsUser = {
                   sortable: true,
                   pageable: {
                       buttonCount: 7
                   },
                   columns: [
                   {
                       title: "STT",
                       template: function (dataItem) {
                           var index = $scope.dataSource1.indexOf(dataItem) + 1;
                           return "<p style='text-align:center; margin: 0'>" + index + "</p>";
                       },
                       width: "45px"
                   },
                   {
                       field: "MaCanBo",
                       title: "Mã số CBVC"
                   },
                    {
                        field: "TenCanBo",
                        title: "Họ tên"
                    },
                    {
                        field: "NoiDungHoatDong",
                        title: "Nội dung thưởng điểm/trừ điểm"
                    },
                   {
                       field: "SoDiem",
                       title: "Số điểm",
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
                               var studyYear = $scope.studyYear;
                               var studyTerm = $scope.StudyTerm;
                               return diemthuongdiemtruService.searchdiemthuongdiemtruuser(studyYear, studyTerm).then(function (result) {
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
                               var studyYear = $scope.studyYear;
                               var studyTerm = $scope.StudyTerm;
                               return diemthuongdiemtruService.searchdiemthuongdiemtru(studyYear, studyTerm).then(function (result) {
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
                       templateUrl: '/app/views/kpi/diemthuongdiemtru/detail.html',
                       controller: 'DiemthuongdiemtruDetailController',
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
                       templateUrl: '/app/views/kpi/diemthuongdiemtru/detail.html',
                       controller: 'DiemthuongdiemtruDetailController',
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
               $scope.Delete = function (Id) {
                   if (Id == "") {
                       alert("Bạn chưa chọn phần tử");
                       return;
                   }
                   var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                   if (!valid)
                       return;
                   diemthuongdiemtruService.getObj(Id).then(function (result) {
                       $scope.obj = result.data;
                       diemthuongdiemtruService.Delete($scope.obj).then(function () {
                           diemthuongdiemtruService.getList().then(function (result) {
                               $scope.grid1.dataSource.read();
                           });
                       });
                   });
               };
           }
    ]);

    app.controller('DiemthuongdiemtruDetailController', ['$scope', '$modalInstance', '$filter', 'id', 'staffId', 'diemthuongdiemtruService', 'staffService', 'departmentService',
        function ($scope, $modalInstance, $filter, id, staffId, diemthuongdiemtruService, staffService, departmentService) {
            $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
            $scope.title = "Chi tiết điểm thưởng điểm trừ";
            var moment = require('moment');
            $scope.obj = {};
            $scope.Id = id;
            $scope.SubStaffIds = [];;
            $scope.staffs = [];
            $scope.DepartmentList = [];
            $scope.obj.DepartmentId = "";
            $scope.DVCC = [];
            $scope.checked = false;
            $scope.options = {
                filter: "contains"
            }
            diemthuongdiemtruService.getListStudyYear().then(function (result) {
                $scope.studyYears = result.data;
            });
            diemthuongdiemtruService.getListProfessorCriterion().then(function (result) {
                $scope.professorCriterions = result.data;
                $scope.selectedChangeActivityGroup();
            });
           
            departmentService.getList().then(function (result) {
                $scope.DepartmentList = result.data;
                $scope.obj.DonViCungCapId = $scope.DepartmentList[0].Id;
                $scope.selectedChangeDepartment();
            });
            $scope.selectedChangeActivityGroup = function () {
                diemthuongdiemtruService.getListDictionaryByManageCode($scope.obj.MaNhomHoatDong).then(function (result) {
                    $scope.professorCriterionsDictionaries = result.data;
                });
            };
            $scope.selectedChangeDepartment = function () {
                $scope.staffResource = {};
                $scope.checked = false;
                staffService.getListByDepartmentId($scope.obj.DonViCungCapId).then(function (result) {
                    $scope.staffResource = result.data;
                });
            };
            $scope.numericOptions = {
                format: "n0",
                min: 0
            }

            $scope.uploadOptions = {
                async: {
                    saveUrl: "/DiemthuongdiemtruImportData/SaveFileToData",
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
                $scope.obj.StaffId = staffId;

                diemthuongdiemtruService.getUserNhap().then(function (result) {
                    $scope.UserNhaps = result.data;
                    $scope.obj.UserNhap = $scope.UserNhaps.HoVaTen;
                    $scope.obj.DonViCungCapName = $scope.UserNhaps.DepartmentName;
                });
                var currentDate = moment();
                var currentYear = currentDate.format('YYYY');
                var currentMonth = currentDate.format('MM');
                if (parseInt(currentMonth) <= 2 || parseInt(currentMonth) >= 9) {
                    $scope.obj.HocKy = "HK01";
                }
                else {
                    $scope.obj.HocKy = "HK02";
                }
                diemthuongdiemtruService.getListStudyYear().then(function (result) {
                    $.each(result.data, function (index, item) {
                        var itemYearBefore = item.StudyYear.substring(0, 4);
                        var itemYearAfter = item.StudyYear.slice(-4);
                        if (currentYear == itemYearAfter) {
                            if (parseInt(currentMonth) < 9)
                                $scope.obj.NamHoc = item.StudyYear;
                            else
                                $scope.obj.NamHoc = itemYearAfter + '-' + (parseInt(itemYearAfter) + 1);
                            return false;
                        }
                    });
                });
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return diemthuongdiemtruService.ListUserOtherActivity().then(function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    pageSize: 20
                });

            } else {
                diemthuongdiemtruService.getObj(id).then(function (result) {
                    $scope.obj = result.data;
                });
                $scope.dataSource = new kendo.data.DataSource({
                    dataType: 'json',
                    transport: {
                        read: function (options) {
                            return diemthuongdiemtruService.ListOtherActivityUserDetail($scope.Id).then(function (result) {
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
                    field: "NoiDungHoatDong",
                    title: "Nội dung"
                },
                {
                    field: "SoDiem",
                    title: "Số điểm",
                }
                ]
            };
            diemthuongdiemtruService.getDVCC($scope.obj.MaNhomHoatDong).then(function (result) {
                $scope.ProfessorCriterion = result.data;
                $scope.DVCC = $scope.ProfessorCriterion.DepartmentIds;
            });
            $scope.save = function () {
                $scope.obj.staffIds = [];
                $.each($scope.staffs, function (index, item) {
                    $scope.obj.staffIds.push(item.Id);
                });
                var total = $scope.obj.staffIds.length;
              
                if ($scope.isNew) {
                    if ($scope.DVCC.length > 0) {
                        $.each($scope.DVCC, function (ind, it) {
                            if ($scope.UserNhaps.DepartmentId != it) {
                                alert("Nhân viên này không có quyền đổ dữ liệu!");
                            }
                            else {
                                diemthuongdiemtruService.Save($scope.obj).then(function (result) {
                                    alert("Thành công!");
                                    $modalInstance.close(result);
                                });
                            }
                        });
                    }
                    else
                        alert("Hoạt động này chưa được phân quyền đơn vị cung cấp!");
                   
                } else {
                    diemthuongdiemtruService.Save($scope.obj).then(function () {
                        $modalInstance.close(result);
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.close();
            };
        }
    ]);
});