define(['app/app', 'app/services/kpi/ratingKPIService', 'app/services/kpi/professorCriterionService', 'app/services/kpi/professorOtherActivityService', 'app/services/kpi/scienceResearchService', 'app/services/kpi/otherActivityDataService', 'moment'], function (app) {
    app.controller('ratingKPIOtherdetailController', ['$scope', '$modal', '$modalInstance', 'ManageCode', 'ratingKPIService', 'professorCriterionService', 'professorOtherActivityService', 'scienceResearchService', 'otherActivityDataService',
        function ($scope, $modal, $modalInstance, ManageCode, ratingKPIService, professorCriterionService, professorOtherActivityService, scienceResearchService, otherActivityDataService) {
            var counter = 0;
            $scope.options = {
                filter: "contains"
            }
            $scope.createWidget = false;
            $scope.grid = {};
            $scope.resultList = [];

            ///////////////////////// chi tiết đánh giá hoạt động khác ///////

            $scope.dataSource = new kendo.data.DataSource({
                dataType: 'json',
                transport: {
                    read: function (options) {
                        return ratingKPIService.getListOtherDetail(ManageCode).then(function (result) {
                            options.success(result.data);
                        });
                    }
                },
                pageSize: 20
            });
            
            function renderCounter() {
                var result = 0;
                var page = parseInt($scope.dataSource.page()) - 1;
                var pageSize = parseInt($scope.dataSource.pageSize());
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
                    title: "STT",
                    template: function (dataItem) {
                        var index = $scope.dataSource.indexOf(dataItem) + 1;
                        return "<p style='text-align:center; margin: 0'>" + index + "</p>";
                    },
                    width: "50px"
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
                ],
            };
            $scope.cancel = function () {
                $modalInstance.close();
            }
            ///////////////////////////////
        }
    ]);
});