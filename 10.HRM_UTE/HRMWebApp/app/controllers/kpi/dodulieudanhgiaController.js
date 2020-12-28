define(['app/app', 'app/services/kpi/dodulieudanhgiaService', 'app/services/kpi/departmentService', 'app/services/kpi/agentObjectService', 'app/services/kpi/staffService', 'moment'], function (app) {
    "use strict";

    app.controller('dodulieudanhgiaController', ['$scope', '$modal', '$rootScope', 'dodulieudanhgiaService', 'departmentService',
           function ($scope, $modal, $rootScope, dodulieudanhgiaService, departmentService) {
               var counter = 0;
               $scope.options = {
                   filter: "contains"
               }

               $scope.createWidget = false;
               $scope.grid = {};
               $scope.dataSource = new kendo.data.DataSource({
                   dataType: 'json',
                   transport: {
                       read: function (options) {
                           return dodulieudanhgiaService.getListData().then(function (result) {
                               options.success(result.data);
                           });
                       }
                   },
                   pageSize: 20
               });

               $scope.obj = null;
               function renderCounter() {
                   var result = 0;
                   var page = parseInt($scope.dataSource.page()) - 1;
                   var pageSize = parseInt($scope.dataSource.pageSize());
                   result = page * pageSize;
                   return result;
               }

               $scope.fileMainGridOptions = {
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
                       field: "TenCanBo",
                       title: "Họ tên"
                   },
                    {
                        field: "DepartmentName",
                        title: "Đơn vị"
                    },
                   {
                       field: "HocKy",
                       title: "Học kỳ",
                       width: "100px"
                   },
                   {
                       field: "NamHoc",
                       title: "Năm học",
                       width: "100px"
                   },
                   {
                       field: "GhiChu",
                       title: "Ghi chú"
                   },
                   {
                       template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                       width: "65px",
                   }
                   ],
               };

               $scope.uploadOptions = {
                   async: {
                       saveUrl: "/dodulieudanhgiaImportData/SaveFileToData",
                       autoUpload: true
                   },
                   success: function (result) {
                       //if (result.response == "2") {
                       //    alert("Có lỗi xảy ra, vui lòng thử lại!");
                       //}
                       //else
                       alert("Thành công!");
                       onUploadSuccess();
                   }
               }

               function onUploadSuccess() {
                   $scope.grid.dataSource.read();
                   $scope.grid.refresh();
               }

               $scope.numericOptions = {
                   format: "n0",
                   min: 0
               }

               $scope.Delete = function (Id) {
                   if (Id == "") {
                       alert("Bạn chưa chọn phần tử");
                       return;
                   }
                   var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                   if (!valid)
                       return;
                   dodulieudanhgiaService.getObj(Id).then(function (result) {
                       $scope.obj = result.data;
                       dodulieudanhgiaService.Delete($scope.obj).then(function () {
                           dodulieudanhgiaService.getList().then(function (result) {
                               $scope.grid.dataSource.read();
                           });
                       });
                   });
               };
           }
    ]);

});