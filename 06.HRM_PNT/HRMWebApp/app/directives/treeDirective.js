define(['app/app'], function (app) {
    "use strict";

    app.directive('tree', function () {
        return {
            restrict: 'E', // tells Angular to apply this to only html tag that is <tree>
            replace: true, // tells Angular to replace <tree> by the whole template
            scope: {
                t: '=src' // create an isolated scope variable 't' and pass 'src' to it.  
            },
            template: '<ul><branch ng-repeat="c in t" src="c"></branch></ul>'
        };
    })

    app.directive('branch', function ($compile) {
        return {
            restrict: 'E', // tells Angular to apply this to only html tag that is <branch>
            replace: true, // tells Angular to replace <branch> by the whole template
            scope: {
                b: '=src' // create an isolated scope variable 'b' and pass 'src' to it.  
            },
            template: '<li><a ng-click="KyDanhGiaClicked()">{{b.Id}}{{ b.Ten }}</a></li>',
            //link: function (scope, element, attrs) {
            //    //// Check if there are any children, otherwise we'll have infinite execution

            //    var has_children = angular.isArray(scope.b.children);

            //    //// Manipulate HTML in DOM
            //    if (has_children) {
            //        element.append('<tree src="b"></tree>');

            //        // recompile Angular because of manual appending
            //        $compile(element.contents())(scope);
            //    }

            //    //// Bind events
            //    element.on('click', function (event) {
            //        event.stopPropagation();

            //        if (has_children) {
            //            element.toggleClass('collapsed');
            //        }
            //    });
            //}
        };
    })


});