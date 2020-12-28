
define(['app/app', 'app/services/abc/ABC_KyDanhGiaService', 'app/services/abc/WebUserService', 'app/directives/treedirective'], function (app) {
    "use strict";
    //var HRMWebAppModule = angular.module('HRMWebApp');
    app.controller('ABC_KyDanhGiaController', ['$scope', '$rootScope', '$modal', '$state', 'ABC_KyDanhGiaService', 'WebUserService',
        function ($scope, $rootScope, $modal, $state, ABC_KyDanhGiaService, WebUserService) {
            $scope.grid = {};
            $scope.obj = {};
            $scope.isEdit = false;
            ABC_KyDanhGiaService.GetNam().then(function (result) {
                $scope.ListNam = result.data;
            });
            var currentTime = new Date();

            function listToTree(list) {
                var map = {}, node, roots = [], i;
                for (i = 0; i < list.length; i += 1) {
                    map[list[i].Id] = i; // initialize the map
                    list[i].children = []; // initialize the children
                }
                for (i = 0; i < list.length; i += 1) {
                    node = list[i];
                    if (node.ParentId !== null && node.ParentId != '00000000-0000-0000-0000-000000000000') {
                        // if you have dangling branches check that map[node.parentId] exists
                        list[map[node.ParentId]].children.push(node);
                    } else {
                        roots.push(node);
                    }
                }
                return roots;
            }

            $scope.Nam = currentTime.getFullYear();


            $scope.New = function () {
                //alert($scope.Nam);
                ABC_KyDanhGiaService.saveKyDanhGia($scope.Nam).then(function () {
                    Notify('Thành công!', 'top-right', '3000', 'success', 'fa-check', true);
                    $scope.UpdateYearValue();
                });
                
            }
            $scope.UpdateYearValue = function () {
                //alert($scope.Nam);
                ABC_KyDanhGiaService.getListKyDanhGia($scope.Nam).then(function (result) {
                    $scope.TreeKyDanhGia = listToTree(angular.copy(result.data));
                });
            }
            $scope.UpdateYearValue();
            $scope.KyDanhGiaClicked = function (KyDanhGiaId) {
                //WebUserService.getWebUserByUserId($rootScope.session.UserId).then(function (result) {
                //    if (result.data.StaffInfoId != "00000000-0000-0000-0000-000000000000")
                //        $state.go("ChiTietKetQua", { KyDanhGiaId: KyDanhGiaId });
                //});
            }
        }
    ]);
});