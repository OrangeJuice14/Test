/// <reference path="D:\Projects\HRMWebApp\HRMWebApp\HRMWebApp\Scripts/ui-bootstrap-tpls-1.1.0.js" />
require.config({
    baseUrl: "",
    waitSeconds: 0,

    // alias libraries paths.  Must set 'angular'
    paths: {
        'angular': '/scripts/angular',
        'angular-route': '/scripts/angular-route',
        'ui-route': '/scripts/AngularUI/ui-router.min',
        'ui-validate': '/scripts/AngularUI/ui-validate',
        'angularAMD': '/scripts/angularAMD',
        'ui-bootstrap': '/scripts/ui-bootstrap-0.10.0',
        'ui-bootstrap-tpls': '/Scripts/ui-bootstrap-tpls-0.13.3.min',
        'kendo': '/Components/kendo/kendo.all.min',        
        'moment': '/scripts/moment.min',
    },

    // Add angular modules that does not support AMD out of the box, put it in a shim
    shim: {
        'angularAMD': ['angular'],
        'ui-route': ['angular'],
        'ui-validate': ['angular'],
        'angular-route': ['angular'],
        'ui-bootstrap': ['angular'],
        'kendo': ['angular'],       
        'ui-bootstrap-tpls': ['angular', 'ui-bootstrap'],
    },

    // kick start application
    deps: ['./app/app']
});
