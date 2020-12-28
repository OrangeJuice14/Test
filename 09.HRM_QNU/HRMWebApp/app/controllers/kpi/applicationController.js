(function () {
    "use strict";

    var HRMWebAppModule = angular.module('HRMWebApp');

    HRMWebAppModule.controller('ApplicationController', ['$scope',
            function ($scope) {
                $scope.isMainMenuCollapsed = false;
            }
    ]);
})();