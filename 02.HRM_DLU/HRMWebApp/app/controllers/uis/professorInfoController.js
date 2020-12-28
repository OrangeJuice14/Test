
define(['app/app', 'app/services/uis/professorService',], function (app) {
    "use strict";

    app.controller('professorInfoController', ['$scope', 'professorService',
            function ($scope, professorService) {
            $scope.obj={};
                professorService.getObj().then(function (result) {
                   
                    $scope.obj = result.data;
                });
            }
    ]);

    

});