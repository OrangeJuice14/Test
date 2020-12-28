(function() {
    "use strict";

    var HRMWebAppModule = angular.module('HRMWebApp');

    HRMWebAppModule.controller('accountController', ['$scope', 'accountService','authService',
        function($scope, accountService, authService) {
            $scope.title = "Login";
            $scope.loginFailed = false;
            $scope.obj = { UserName: "", Password: "" };
            $scope.login = function() {
                accountService.login($scope.obj).then(function (result) {
                    if (result != null) {
                        $scope.loginFailed = false;
                        window.location = "/#/departmentdisplay";
                        authService.setStaff(result);
                    } else
                        $scope.loginFailed = true;
                });
            };
        }
    ]);
})();sd