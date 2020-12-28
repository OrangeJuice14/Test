define(['app/app'], function (app) {
    "use strict";



    app.factory('dataShareService', ['$http', '$q', function ($http, $q) {

        var dataShareService = this;

        
        dataShareService.normalStaffId = MANAGER.GUID_EMPTY;
        dataShareService.isSupervisor = false;
        dataShareService.departmentId = MANAGER.GUID_EMPTY;
        dataShareService.supervisorId = MANAGER.GUID_EMPTY;
        dataShareService.planId = MANAGER.GUID_EMPTY;
        dataShareService.agentObjectId = MANAGER.GUID_EMPTY;
        dataShareService.isAdminRating = false;
       
    }]);
});