
define(['app/app'], function (app) {
    "use strict";

    app.controller('ChatLuongCaoController', [ 
        function ($scope, $modal, $rootScope) {
            window.open("http://www.myaep.neu.edu.vn/Login/LoginADFS")
        }
    ]);
    app.controller('SauDaiHocController', [
        function () {
            window.open("http://thacsi.neu.edu.vn/Login/LoginADFS")

        }
    ]);
    app.controller('ChinhQuyController', [ 
        function () {
            window.open("http://daihocchinhquy.neu.edu.vn/Login/LoginADFS")

        }
    ]);
    app.controller('TuXaController', [ 
        function () {
            window.open("http://tuxa.neu.edu.vn/Login/LoginADFS")
        }
    ]);
    app.controller('VuaHocVuaLamController', [ 
        function () {
            window.open("http://vlvh.neu.edu.vn/Login/LoginADFS")
        }
    ]);
});