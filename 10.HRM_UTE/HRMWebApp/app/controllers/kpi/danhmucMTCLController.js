
define(['app/app', 'app/services/kpi/danhmucMTCLService', 'app/services/kpi/criterionService'], function (app) {
    "use strict";
    app.controller('danhmucMTCLController', ['$scope', '$modal', '$rootScope', 'danhmucMTCLService',
           function ($scope, $modal, $rootScope, danhmucMTCLService, criterionService) {
               $scope.createWidget = false;
               $scope.grid = {};
               $scope.isEdit = false;
               $scope.resultList = [];
               $scope.managecode = ""
               $scope.dataSource = new kendo.data.DataSource({
                   transport: {
                       read: function (options) {
                           return danhmucMTCLService.getListDanhMuc($scope.managecode).then(function (result) {
                               options.success(result.data);
                           });
                       }
                   },
                   pageSize: 20,

               });

               $scope.obj = null;
               $scope.mainGridOptions = {
                   sortable: true,
                   pageable: true,
                   selectable: true,
                   change: function (e) {
                       var selectedRows = this.select();
                       $rootScope.$broadcast('MANAGESELECTION', this.dataItem(selectedRows[0]).Id);
                   },
                   columns: [{
                       field: "MaDanhMuc",
                       title: "Mã danh mục",
                       width: "100px"
                   }, {
                       field: "TenDanhMuc",
                       title: "Tên danh muc"
                   },
                   {
                       field: "CapDanhMuc",
                       title: "Cấp danh muc"
                   },
                   {
                       template: "<div style='width: 30px;'><button ng-click='Edit(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button></div>",
                       width: "50px"
                   },
                   {
                       template: "<div style='width: 30px;'><button ng-click='Delete(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                       width: "50px",
                   }],
               };

               var editRender = function (row, columnfield, value, defaulthtml, columnproperties) {
                   return "<button ng-click='Edit(value)' class='btn btn-block btn-success btn-xs'><i class='fa fa-pencil'></i>  </button>";
               };
               var deleteRender = function (row, columnfield, value, defaulthtml, columnproperties) {
                   return "<button ng-click='Delete(value)' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button>";
               };

               $scope.$on("MANAGESELECTION", function (event, args) {
                   $scope.managecode = args;
                   $scope.grid.dataSource.read();
               });

               $scope.New = function () {
                   var modalInstance = $modal.open({
                       animation: true,
                       templateUrl: '/app/views/kpi/staffRole/danhmucCondetail.html',
                       controller: 'danhmucMTCLdetailController',
                       resolve: {
                           id: function () {
                               return MANAGER.GUID_EMPTY;
                           }
                       }
                   }).result.finally(function (result) {
                       $scope.grid.dataSource.read();
                   });
               };

               $scope.Edit = function (Id) {
                   if (Id == "") {
                       alert("Bạn chưa chọn phần tử");
                       return;
                   }

                   var modalInstance = $modal.open({
                       animation: true,
                       templateUrl: '/app/views/kpi/staffRole/danhmucCondetail.html',
                       controller: 'danhmucMTCLdetailController',
                       resolve: {
                           id: function () {
                               return Id;
                           }
                       }
                   }).result.finally(function () {
                       $scope.grid.dataSource.read();
                   });
               }

               $scope.Delete = function (Id) {

                   if (Id == "") {
                       alert("Bạn chưa chọn phần tử");
                       return;
                   }

                   var valid = window.confirm("Bạn có thật sự muốn xóa không?");
                   if (!valid)
                       return;

                   danhmucMTCLService.getListByDanhMucId(Id).then(function (result) {
                       $scope.obj = result.data;
                       danhmucMTCLService.Delete($scope.obj).then(function (result) {
                           $scope.grid.dataSource.read();
                       });
                   });
               };
           }
    ]);
    app.controller('danhmucMTCLdetailController', ['$scope', '$modalInstance', 'id', 'danhmucMTCLService', 'criterionService',
      function ($scope, $modalInstance, id, danhmucMTCLService, criterionService) {
          $scope.isNew = id == MANAGER.GUID_EMPTY || '' ? true : false;
          $scope.title = "Chi tiết danh mục";
          $scope.obj = {};

          //staffService.getListByDepartmentId(id).then(function (result) {
          //    $scope.staffs = result.data;
          //});
          criterionService.getListManageCode().then(function (result) {
              $scope.DanhMucCha = result.data;
          });

          $scope.Namhoc = {
              placeholder: "Chọn năm học...",
              dataTextField: "Name",
              dataValueField: "Id",
              valuePrimitive: true,
              autoBind: false,
              dataSource: {
                  transport: {
                      read: function (options) {
                          return danhmucMTCLService.getListStudyYear().then(function (result) {
                              options.success(result.data);
                          });
                      }
                  }
              }
          };

          danhmucMTCLService.getBoPhan().then(function (result) {
              $scope.DonViPhuTrach = result.data;
          });

          danhmucMTCLService.getBGH().then(function (result) {
              $scope.BGHPhuTrach = result.data;
          });

          if ($scope.isNew) {

              $scope.obj = {
                  Id: MANAGER.GUID_EMPTY,
                  Name: "",
              };
          } else {
              danhmucMTCLService.getListByDanhMucId(id).then(function (result) {
                  $scope.obj = result.data;
              });
          }

          $scope.save = function () {
              if ($scope.isNew) {
                  danhmucMTCLService.Save($scope.obj).then(function () {
                      $modalInstance.close();
                  });
              } else {
                  danhmucMTCLService.Save($scope.obj).then(function () {
                      $modalInstance.close();
                  });
              }
          };

          $scope.cancel = function () {
              $modalInstance.dismiss('cancel');
          };
      }
    ]);

})