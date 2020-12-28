
define(['app/app', 'app/services/uis/scoreIndexService'], function (app) {
    "use strict";

    app.controller('scoreIndexController', ['$scope', 'scoreIndexService',
            function ($scope, scoreIndexService) {


               
                $scope.obj = {};
              

                $scope.myScoreIndex = scoreIndexService.getScore();
                $scope.myObj = {
                    TenHocPhan: "abc",
                    HocSinhList: [
                        {
                            HoTen: "NguyenVanA",
                            Score: 10
                        },
                        {
                            HoTen: "NguyenVanB",
                            Score: 9
                        },
                        {
                            HoTen: "NguyenVanC",
                            Score: 5
                        }
                    ]
                }

                //scoreIndexService.getScore().then(function (result) {
                   
                //    $scope.myScoreIndex = result;
                //});
            }
    ]);

    

});