require.config({
    baseUrl: "",
    waitSeconds: 0,

    // alias libraries paths.  Must set 'angular'
    paths: {
        'angular': '/Components/angularjs/angular.min',
        'angular-route': '/Components/angularjs/angular-route',
        'ui-route': '/Components/angularjs/angular-ui-router.min',
        'ui-validate': '/scripts/AngularUI/ui-validate',
        'angularAMD': '/scripts/angularAMD',
        'ui-bootstrap': '/scripts/ui-bootstrap-0.10.0',
        'ui-bootstrap-tpls': '/Scripts/ui-bootstrap-tpls-0.13.3.min',
        'kendo': '/Components/kendo/kendo.all.min',        
        'moment': '/scripts/moment.min',
        'angular-animate': '/Components/angularjs/material-angularjs/angular-animate.min',
        'angular-aria': '/Components/angularjs/material-angularjs/angular-aria.min',
        'angular-material': '/Components/angularjs/material-angularjs/angular-material.min',
        'angular-messages': '/Components/angularjs/material-angularjs/angular-messages.min',
        'ag-grid-community': '/Components/angularjs/ag-gird/ag-grid-community.min',
        'ag-grid-community-noStyle': '/Components/angularjs/ag-gird/ag-grid-community.min.noStyle',
        'ckeditor': '/Components/angularjs/ckeditor/ckeditor',
        'angular-sanitize': '/Scripts/angular-sanitize',
        'angular.treeview': '/Components/angularjs/angular.treeview.min',
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
        'angular-animate': ['angular'],
        'angular-aria': ['angular'],
        'angular-material': ['angular', 'angular-aria', 'angular-animate', 'angular-messages'],
        'angular-messages': ['angular'],
        'ag-grid-community': ['angular'],
        'ag-grid-community-noStyle': ['angular'],
        'angular-sanitize': ['angular'],
        'angular.treeview': ['angular']
    },

    // kick start application
    deps: ['./app/app']
});
