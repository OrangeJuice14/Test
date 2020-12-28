define(['angularAMD', 'ui-route', 'ui-validate', 'ui-bootstrap', 'kendo', 'ui-bootstrap-tpls', 'angular-sanitize'], function (angularAMD) {
    "use strict";
    var HRMWebApp = angular.module('HRMWebApp', ['ui.router', 'ui.validate', 'ui.bootstrap', 'kendo.directives', 'ngSanitize']);
    HRMWebApp.factory('principal', ['$q', '$http', '$timeout', '$rootScope',
         function ($q, $http, $timeout, $rootScope) {
             var _identity = undefined,
               _authenticated = false;

             return {
                 isIdentityResolved: function () {
                     return angular.isDefined(_identity);
                 },
                 isAuthenticated: function () {
                     return _authenticated;
                 },
                 isInRole: function (role) {
                     if (!_authenticated || !_identity.roles) return false;
                   
                     return _identity.roles.indexOf(role) != -1;
                 },
                 isInAnyRole: function (roles) {
                     if (!_authenticated || !_identity.roles) return false;

                     for (var i = 0; i < roles.length; i++) {
                         if (this.isInRole(roles[i])) return true;
                     }
                     return false;
                 },
                 authenticate: function (identity) {
                     _identity = identity;
                     _authenticated = identity != null;

                     // for this demo, we'll store the identity in localStorage. For you, it could be a cookie, sessionStorage, whatever
                     if (identity) localStorage.setItem("demo.identity", angular.toJson(identity));
                     else localStorage.removeItem("demo.identity");
                 },
                 identity: function (force) {
                     var deferred = $q.defer();

                     if (force === true) _identity = undefined;

                     // check and see if we have retrieved the identity data from the server. if we have, reuse it by immediately resolving
                     if (angular.isDefined(_identity)) {
                         deferred.resolve(_identity);

                         return deferred.promise;
                     }
                     //
                     var session = null;
                     $.ajax({
                         type: 'POST',
                         url: '/Authentication/GetUserSessionInfo',
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         async: false,
                         success: function (result) {
                             session = result;
                             _authenticated = true;
                             $rootScope.session = session;
                         }
                     });
                     var source;
                     var pathname = window.location.href;
                     $.ajax({
                         type: 'POST',
                         url: '/WebServices/WebMenu_GetURLListBy_WebUserId',
                         contentType: "application/json; charset=utf-8",
                         data: ko.toJSON({
                             webUserId: session.UserId
                         }),
                         dataType: "json",
                         async: true,
                         success: function (result) {
                             _identity = {};
                             _identity["roles"] = [];
                             _identity["roles"]=result;
                             deferred.resolve(_identity);
                         }
                     });

                     return deferred.promise;
                 }
             };
         }
    ])
    HRMWebApp.factory('authorization', ['$rootScope', '$state', 'principal',
         function ($rootScope, $state, principal) {
             return {
                 authorize: function () {
                     return principal.identity()
                       .then(function () {
                           var isAuthenticated = principal.isAuthenticated();

                           //Kiểm tra nếu cookie bị mất thì thoát ra trang login
                           $.ajax({
                               type: 'POST',
                               url: '/Authentication/GetUserCookieInfo',
                               contentType: "application/json; charset=utf-8",
                               dataType: "json",
                               async: false,
                               success: function (result) {
                                   var obj = result;
                                   //
                                   if (obj.message != "exists")
                                   {
                                       //Đăng xuất
                                       $.ajax({
                                           type: 'POST',
                                           url: '/Authentication/LogOff',
                                           contentType: "application/json; charset=utf-8",
                                           dataType: "json",
                                           async: false,
                                           success: function (result) {

                                               //Gọi trang login
                                               window.location = "/login.html";
                                           }
                                       });
                                   }
                               }
                           });

                           //
                           if ($rootScope.toState.data == null) {
                               //if (isAuthenticated) {
                               //    window.location = "/";
                               //}
                               //else {
                               //    // user is not authenticated. stow the state they wanted before you
                               //    // send them to the signin state, so you can return them when you're done
                               //    $rootScope.returnToState = $rootScope.toState;
                               //    $rootScope.returnToStateParams = $rootScope.toStateParams;

                               //    // now, send them to the signin state so they can log in
                               //    window.location = "/login.html";
                               //}
                           }
                           else {
                               if ($rootScope.toState.data.roles && $rootScope.toState.data.roles.length > 0 && !principal.isInAnyRole($rootScope.toState.data.roles)) {
                                   if (isAuthenticated) {
                                       window.location = "/";
                                   }// user is signed in but not authorized for desired state
                                   else {
                                       // user is not authenticated. stow the state they wanted before you
                                       // send them to the signin state, so you can return them when you're done
                                       $rootScope.returnToState = $rootScope.toState;
                                       $rootScope.returnToStateParams = $rootScope.toStateParams;

                                       // now, send them to the signin state so they can log in
                                       window.location = "/login.html";
                                   }
                               }
                           }
                       });
                 }
             };
         }
    ]);
    HRMWebApp.config(['$stateProvider', '$urlRouterProvider','$locationProvider', function ($stateProvider, $urlRouterProvider, $locationProvider,ngSanitize) {
      
        //var baseSiteUrlPath = "/";

        //var baseTemplateUrl = baseSiteUrlPath + "app/templates/";

        //$urlRouterProvider.otherwise({
        //    redirectTo: function () {

        //        if (window.location.pathname == baseSiteUrlPath || window.location.pathname == baseSiteUrlPath + "angular") {
        //            window.location = baseSiteUrlPath + "angular/index";
        //        } else {
        //            window.location = baseSiteUrl + "angular/page-not-found";
        //        }
        //    },
        //});
        $locationProvider.html5Mode(true);
        $stateProvider.state('site', {
            'abstract': true,
          
        })
            .state('default', angularAMD.route({
                url: '/'
            }))
        ////////////////////***********  CHAM CONG ********************///////////////////////////
              .state('user', angularAMD.route({
                  data: {
                      roles: ['kpi/user']
                  },
                  resolve: {
                      authorize: ['authorization',
                        function (authorization) {
                            return authorization.authorize();
                        }
                      ]
                  },
                  url: '/kpi/user',
                  templateUrl: '/app/views/chamcong/user/manage.html',
              }))
             .state('AccountInfo', angularAMD.route({
                 data: {
                     roles: ['kpi/AccountInfo']
                 },
                 resolve: {
                     authorize: ['authorization',
                       function (authorization) {
                           return authorization.authorize();
                       }
                     ]
                 },
                 url: '/kpi/AccountInfo',
                 templateUrl: '/app/views/chamcong/user/AccountInfo.html',
                 
             }))
             .state('adminUser', angularAMD.route({
                 data: {
                     roles: ['kpi/adminUser']
                 },
                 resolve: {
                     authorize: ['authorization',
                       function (authorization) {
                           return authorization.authorize();
                       }
                     ]
                 },
                 url: '/kpi/adminUser',
                 templateUrl: '/app/views/chamcong/user/Manage_Admin.html'
             }))
            .state('userDetail', angularAMD.route({
                url: '/kpi/userDetail',
                templateUrl: '/app/views/chamcong/user/Detail.html'
            }))
            //Ho so nhan su
             .state('HoSoNhanSu', angularAMD.route({
                 data: {
                     roles: ['kpi/HoSoNhanSu']
                 },
                 resolve: {
                     authorize: ['authorization',
                       function (authorization) {
                           return authorization.authorize();
                       }
                     ]
                 },
                 url: '/kpi/HoSoNhanSu',
                 templateUrl: '/app/views/chamcong/HoSoNhanSu/Manage.html'
             }))

             //Qua trinh cong tac
             .state('QuaTrinhCongTac', angularAMD.route({
                 data: {
                     roles: ['kpi/QuaTrinhCongTac']
                 },
                 resolve: {
                     authorize: ['authorization',
                       function (authorization) {
                           return authorization.authorize();
                       }
                     ]
                 },
                 url: '/kpi/QuaTrinhCongTac',
                 templateUrl: '/app/views/chamcong/QuaTrinhCongTac/Manage.html'
             }))
            .state('khaibaoccngaynghi', angularAMD.route({
                data: {
                    roles: ['kpi/khaibaoccngaynghi']
                },
                resolve: {
                    authorize: ['authorization',
                        function (authorization) {
                            return authorization.authorize();
                        }
                    ]
                },
                url: '/kpi/khaibaoccngaynghi',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/DangKyNghiPhep.html'
            }))
            .state('quanlyguimail', angularAMD.route({
                data: {
                    roles: ['kpi/quanlyguimail']
                },
                resolve: {
                    authorize: ['authorization',
                        function (authorization) {
                            return authorization.authorize();
                        }
                    ]
                },
                url: '/kpi/quanlyguimail',
                templateUrl: '/app/views/chamcong/MailManager/Manager.html'
            }))
            .state('guimail', angularAMD.route({
                data: {
                    roles: ['kpi/guimail']
                },
                resolve: {
                    authorize: ['authorization',
                        function (authorization) {
                            return authorization.authorize();
                        }
                    ]
                },
                url: '/kpi/guimail',
                templateUrl: '/app/views/chamcong/MailManager/SendMail.html'
            }))
            .state('cauhinhguimail', angularAMD.route({
                resolve: {
                    authorize: ['authorization',
                        function (authorization) {
                            return authorization.authorize();
                        }
                    ]
                },
                url: '/kpi/cauhinhguimail',
                templateUrl: '/app/views/chamcong/MailManager/MailTemplate.html'
            }))
            .state('importexcel', angularAMD.route({
                data: {
                    roles: ['kpi/importexcel']
                },
                url: '/kpi/importexcel',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/ImportThuNhapKhac.html',
                controller: 'ImportThuNhapKhacController',
                controllerUrl: 'app/controllers/chamcong/ImportThuNhapKhacController'
            }))
            //quan ly cham cong
             .state('quanlychamcong', angularAMD.route({
                 data: {
                     roles: ['kpi/quanlychamcong']
                 },
                 resolve: {
                     authorize: ['authorization',
                       function (authorization) {
                           return authorization.authorize();
                       }
                     ]
                 },
                 url: '/kpi/quanlychamcong',
                 templateUrl: '/app/views/chamcong/QuanLyChamCong/Manage.html'
             }))
            .state('quanlynghiphep', angularAMD.route({
                data: {
                    roles: ['kpi/quanlynghiphep']
                },
                resolve: {
                    authorize: ['authorization',
                    function (authorization) {
                        return authorization.authorize();
                    }
                    ]
                },
                url: '/kpi/quanlynghiphep',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/QuanLyNghiPhep.html'
            }))
            .state('quanlynghiphepnam', angularAMD.route({
                data: {
                    roles: ['kpi/quanlynghiphepnam']
                },
                resolve: {
                    authorize: ['authorization',
                    function (authorization) {
                        return authorization.authorize();
                    }
                    ]
                },
                url: '/kpi/quanlynghiphepnam',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/QuanLyNghiPhepNam.html'
            }))
            .state('quanlyphephe', angularAMD.route({
                data: {
                    roles: ['kpi/quanlyphephe']
                },
                resolve: {
                    authorize: ['authorization',
                    function (authorization) {
                        return authorization.authorize();
                    }
                    ]
                },
                url: '/kpi/quanlyphephe',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/QuanLyPhepHe.html'
            }))
            .state('xembangchamcong', angularAMD.route({
                data: {
                    roles: ['kpi/xembangchamcong']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/xembangchamcong',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/XemBangChamCong.html'
            }))
            .state('khaibaochamconggv', angularAMD.route({
                data: {
                    roles: ['kpi/khaibaochamconggv']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/khaibaochamconggv',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/KhaiBaoChamCongGiangVien.html'
            }))
            .state('dangkyngoaigio', angularAMD.route({
                data: {
                    roles: ['kpi/dangkyngoaigio']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/dangkyngoaigio',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/DangKyChamCongNgoaiGio.html'
            }))
            .state('quanlyngoaigio', angularAMD.route({
                data: {
                    roles: ['kpi/quanlyngoaigio']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/quanlyngoaigio',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/ChamCongNgoaiGio.html'
            }))
            .state('quanlyngoaigio_nhacviec', angularAMD.route({
                data: {
                    roles: ['kpi/quanlyngoaigio']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/quanlyngoaigio_nhacviec',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/ChamCongNgoaiGio.html'
            }))
            .state('dangkykhunggio', angularAMD.route({
                data: {
                    roles: ['kpi/dangkykhunggio']
                },
                resolve: {
                    authorize: ['authorization',
                    function (authorization) {
                        return authorization.authorize();
                    }
                    ]
                },
                url: '/kpi/dangkykhunggio',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/DangKyKhungGioLamViec.html'
            }))
             .state('kydangkykhunggio', angularAMD.route({
                 data: {
                     roles: ['kpi/kydangkykhunggio']
                 },
                 resolve: {
                     authorize: ['authorization',
                         function (authorization) {
                             return authorization.authorize();
                         }
                     ]
                 },
                 url: '/kpi/kydangkykhunggio',
                 templateUrl: '/app/views/chamcong/QuanLyChamCong/KyDangKyKhungGio.html'
             }))
            .state('doica', angularAMD.route({
                data: {
                    roles: ['kpi/doica']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/doica',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/DoiCa.html'
            }))
            .state('cachamcong', angularAMD.route({
                data: {
                    roles: ['kpi/cachamcong']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/cachamcong',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/CaChamCong.html'
            }))
            .state('cauhinhchamcong', angularAMD.route({
                data: {
                    roles: ['kpi/cauhinhchamcong']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/cauhinhchamcong',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/CauHinhChamCong.html'
            }))
            .state('dodulieuchamcong', angularAMD.route({
                data: {
                    roles: ['kpi/dodulieuchamcong']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/dodulieuchamcong',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/DoDuLieuChamCong.html'
            }))
            .state('chotchamcongthang', angularAMD.route({
                data: {
                    roles: ['kpi/chotchamcongthang']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/chotchamcongthang',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/ChotChamCongThang.html'
            }))
            .state('chamcongnhanh', angularAMD.route({
                data: {
                    roles: ['kpi/chamcongnhanh']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/chamcongnhanh',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/ChamCongNhanh.html'
            }))
            .state('dangkyngaynghi', angularAMD.route({
                data: {
                    roles: ['kpi/dangkyngaynghi']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/dangkyngaynghi',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/DangKyChamCongNgayNghi.html'
            }))
            .state('chamcongngaynghi', angularAMD.route({
                data: {
                    roles: ['kpi/chamcongngaynghi']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/chamcongngaynghi',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/ChamCongNgayNghi.html'
            }))
            .state('chamcongngaynghi_nhacviec', angularAMD.route({
                data: {
                    roles: ['kpi/chamcongngaynghi']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/chamcongngaynghi_nhacviec',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/ChamCongNgayNghi.html'
            }))
            .state('quanlyvipham', angularAMD.route({
                data: {
                    roles: ['kpi/quanlyvipham']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/quanlyvipham',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/QuanLyViPham.html'
            }))
            // Bang luong
            .state('bangluong', angularAMD.route({
                data: {
                    roles: ['kpi/bangluong']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/bangluong',
                templateUrl: '/app/views/chamcong/BangLuong/Manage.html'
            }))

            //Quan ly cong tac
             .state('quanlycongtac', angularAMD.route({
                 data: {
                     roles: ['kpi/quanlycongtac']
                 },
                 resolve: {
                     authorize: ['authorization',
                       function (authorization) {
                           return authorization.authorize();
                       }
                     ]
                 },
                 url: '/kpi/quanlycongtac',
                 templateUrl: '/app/views/chamcong/QuanLyCongTac/Manage.html'
             }))
             .state('quanlycongtac_nhacviec', angularAMD.route({
                 data: {
                     roles: ['kpi/quanlycongtac']
                 },
                 resolve: {
                     authorize: ['authorization',
                       function (authorization) {
                           return authorization.authorize();
                       }
                     ]
                 },
                 url: '/kpi/quanlycongtac_nhacviec',
                 templateUrl: '/app/views/chamcong/QuanLyCongTac/Manage.html'
             }))
             .state('khaibaocongtac', angularAMD.route({
                 data: {
                     roles: ['kpi/khaibaocongtac']
                 },
                     resolve: {
                 authorize: ['authorization',
                       function (authorization) {
                           return authorization.authorize();
                       }
                           ]
                           },
                 url: '/kpi/khaibaocongtac',
                 templateUrl: '/app/views/chamcong/QuanLyCongTac/CongTacCaNhan.html'
             }))

             .state('quanlynhacviec', angularAMD.route({
                 data: {
                     roles: ['kpi/quanlynhacviec']
                 },
                 resolve: {
                     authorize: ['authorization',
                           function (authorization) {
                               return authorization.authorize();
                           }
                     ]
                 },
                 url: '/kpi/quanlynhacviec',
                 templateUrl: '/app/views/chamcong/NhacViec/QuanLyNhacViec.html'
             }))

             //quan ly xet ABC
             .state('quanlyxetABC', angularAMD.route({
                 data: {
                     roles: ['kpi/quanlyxetABC']
                 },
                 resolve: {
                     authorize: ['authorization',
                       function (authorization) {
                           return authorization.authorize();
                       }
                     ]
                 },
                 url: '/kpi/quanlyxetABC',
                 templateUrl: '/app/views/chamcong/QuanLyXetABC/Manage.html'
             }))
            .state('kiemtraxetABC', angularAMD.route({
                data: {
                    roles: ['kpi/kiemtraxetABC']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                 url: '/kpi/kiemtraxetABC',
                 templateUrl: '/app/views/chamcong/QuanLyXetABC/KiemTraXetABC.html'
             }))
            .state('abcthang', angularAMD.route({
                data: {
                    roles: ['kpi/abcthang']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                 url: '/kpi/abcthang',
                 templateUrl: '/app/views/chamcong/QuanLyXetABC/ThongKeXetAbcTheoThang.html'
             }))
            .state('abcnam', angularAMD.route({
                data: {
                    roles: ['kpi/abcnam']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/abcnam',
                 templateUrl: '/app/views/chamcong/QuanLyXetABC/ThongKeXetAbcTheoNam.html'
             }))

        ////////////////////*********** KPI ********************////////////////////////////////

        .state('agentTargetManage', {
            url: '/kpi/agentTargetManage',
            views: {
                // the main template will be placed here (relatively named)
                '': { templateUrl: '/app/views/kpi/agent_TargetManage/agent_TargetManage.html' },
                // the child views will be defined here (absolutely named)
                'agentObject@agentTargetManage': angularAMD.route({
                    templateUrl: '/app/views/kpi/AgentObject/manage.html',
                    controller: 'agentObjectController',
                    controllerUrl: 'app/controllers/kpi/agentObjectController'
                }),
                'targetGroup@agentTargetManage': angularAMD.route({
                    templateUrl: '/app/views/kpi/targetGroup/manage.html',
                    controller: 'targetGroupController',
                    controllerUrl: 'app/controllers/kpi/targetGroupController'
                }),
                'targetGroupDetail@agentTargetManage': angularAMD.route({
                    templateUrl: '/app/views/kpi/targetGroupDetail/manage.html',
                    controller: 'targetGroupDetail_ManageController',
                    controllerUrl: 'app/controllers/kpi/targetGroupDetailController'
                }),
            }
        })
        .state('professorActivityManage', {
            url: '/kpi/professorActivityManage',
            views: {
                // the main template will be placed here (relatively named)
                '': { templateUrl: '/app/views/kpi/professorActivityManage/professorActivityManage.html' },
                // the child views will be defined here (absolutely named)
                'staffByDepartment@professorActivityManage': angularAMD.route({
                    templateUrl: '/app/views/kpi/staffByDepartment/manage.html',
                    controller: 'staffByDepartmentController',
                    controllerUrl: 'app/controllers/kpi/staffByDepartmentController'
                }),
                'otherActivityData@professorActivityManage': angularAMD.route({
                    templateUrl: '/app/views/kpi/otherActivityData/manage.html',
                    controller: 'otherActivityDataController',
                    controllerUrl: 'app/controllers/kpi/otherActivityDataController'
                })
            }
        })
        .state('planKPI', angularAMD.route({
            url: '/kpi/planKPI',
            templateUrl: '/app/views/kpi/planKPI/manage.html',
            controller: 'planKPIController',
            controllerUrl: 'app/controllers/kpi/planKPIController'
        }))
        .state('editRecord', angularAMD.route({
            url: '/kpi/editRecord',
            templateUrl: '/app/views/kpi/editRecord/manage.html',
            controller: 'staffController',
            controllerUrl: 'app/controllers/kpi/staffController'
        }))
        .state('userPlanKPI', angularAMD.route({
            url: '/kpi/userPlanKPI',
            templateUrl: '/app/views/kpi/planKPIDetail/planManage.html',
            controller: 'planStaffDetailController',
            controllerUrl: 'app/controllers/kpi/planStaffDetailController'
        }))
        .state('professorPlankpidetail', angularAMD.route({
            url: '/kpi/professorPlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor',
            templateUrl: '/app/views/kpi/planKPIDetail/professorManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('schoolManagePlankpidetail', angularAMD.route({
            data: {
                roles: []
            },
            //params:[
            //    'planId',
            //    'agentObjectId',
            //    'normalStaffId',
            //    'isSupervisor',                       
            //],

            //params:  {firstName:"John", lastName:"Doe", age:46},
            resolve: {
                authorize: ['authorization',
                  function (authorization) {
                      return authorization.authorize();
                  }
                ]
            },
            url: '/kpi/schoolManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor',
            templateUrl: '/app/views/kpi/planKPIDetail/schoolManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        //.state('schoolManagePlankpidetail', angularAMD.route({
        //    data: {
        //        roles: []
        //    },           
        //    resolve: {
        //        authorize: ['authorization',
        //          function (authorization) {
        //              return authorization.authorize();
        //          }
        //        ]
        //    },
        //    url: '/kpi/schoolManagePlankpidetail',
        //    //params: { 'planId': MANAGER.GUID_EMPTY, 'agentObjectId': MANAGER.GUID_EMPTY, 'normalStaffId': MANAGER.GUID_EMPTY, 'isSupervisor': 0 },            
        //    templateUrl: '/app/views/kpi/planKPIDetail/schoolManage.html',
        //    controller: 'planKPIDetailController',
        //    controllerUrl: 'app/controllers/kpi/planKPIDetailController',
        //    params:{
        //        planId: null,
        //        agentObjectId: null,
        //        normalStaffId: null,
        //        isSupervisor: 0,                       
        //    },
        //}))
        .state('facultyManagePlankpidetail', angularAMD.route({
            url: '/kpi/facultyManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/facultyManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController',          
        }))
        .state('subFacultyManagePlankpidetail', angularAMD.route({
            url: '/kpi/subFacultyManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/subFacultyManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('principalPlankpidetail', angularAMD.route({
            url: '/kpi/principalPlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/principalManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('vicePrincipalPlankpidetail', angularAMD.route({
            url: '/kpi/vicePrincipalPlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/vicePrincipalManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
       .state('subDepartmentPlankpiDetail', angularAMD.route({
           url: '/kpi/subDepartmentPlankpiDetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
           templateUrl: '/app/views/kpi/planKPIDetail/subDepartmentManage.html',
                 controller: 'planKPIDetailController',
                 controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('subjectManagePlankpidetail', angularAMD.route({
            url: '/kpi/subjectManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor',
            templateUrl: '/app/views/kpi/planKPIDetail/subjectManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('subSubjectManagePlankpidetail', angularAMD.route({
            url: '/kpi/subjectManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor',
            templateUrl: '/app/views/kpi/planKPIDetail/subSubjectManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('plankpidetail', angularAMD.route({            
            url: '/kpi/plankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/staffManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('userPlan', angularAMD.route({
            url: '/kpi/userPlan',
            templateUrl: '/app/views/kpi/planKPI/userPlan.html',
            controller: 'userPlanKPIController',
            controllerUrl: 'app/controllers/kpi/userPlanKPIController'
        }))
        .state('menuRole', {
            url: '/kpi/menuRole',
            views: {
                '': { templateUrl: '/app/views/kpi/menuRole/display.html' },
                'menuRole@menuRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/menuRole/manage.html',
                    controller: 'menuRoleController',
                    controllerUrl: 'app/controllers/kpi/menuRoleController'
                }),
                'webGroup@menuRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/menuRole/manage_webGroup.html',
                    controller: 'webGroupController',
                    controllerUrl: 'app/controllers/kpi/webGroupController'
                })
            }
        })
        .state('professorRatingKPI', angularAMD.route({
            url: '/kpi/professorRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/professorManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('ratingKPI', angularAMD.route({
            url: '/kpi/ratingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/manage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('departmentRatingKPI', angularAMD.route({
            url: '/kpi/departmentRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/departmentManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
         .state('subDepartmentRatingKPI', angularAMD.route({
             url: '/kpi/subDepartmentRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
             templateUrl: '/app/views/kpi/ratingKPI/subDepartmentManage.html',
             controller: 'ratingKPIController',
             controllerUrl: 'app/controllers/kpi/ratingKPIController'
         }))
        .state('subjectRatingKPI', angularAMD.route({
            url: '/kpi/subjectRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/subjectManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('subSubjectRatingKPI', angularAMD.route({
            url: '/kpi/subSubjectRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/subSubjectManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('facultyRatingKPI', angularAMD.route({
            url: '/kpi/facultyRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/facultyManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('subFacultyRatingKPI', angularAMD.route({
            url: '/kpi/subFacultyRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/subFacultyManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('vicePrincipalRatingKPI', angularAMD.route({
            url: '/kpi/vicePrincipalRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/vicePrincipalManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
            .state('principalRatingKPI', angularAMD.route({
                url: '/kpi/principalRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
                templateUrl: '/app/views/kpi/ratingKPI/principalManage.html',
                controller: 'ratingKPIController',
                controllerUrl: 'app/controllers/kpi/ratingKPIController'
            }))
        .state('staffRole', {
            url: '/kpi/staffRole',
            views: {
                // the main template will be placed here (relatively named)
                '': { templateUrl: '/app/views/kpi/staffRole/display.html' },
                // the child views will be defined here (absolutely named)
                //'staffRole@staffRole': angularAMD.route({
                //    templateUrl: '/app/views/kpi/staffRole/manage.html',
                //    controller: 'staffRoleController',
                //    controllerUrl: 'app/controllers/kpi/staffRoleController'
                //}),
                'position@staffRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/position/manage.html',
                    controller: 'positionController',
                    controllerUrl: 'app/controllers/kpi/positionController'
                }),
                'manageCode@staffRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/manageCode/manage.html',
                    controller: 'manageCodeController',
                    controllerUrl: 'app/controllers/kpi/manageCodeController'
                }),
                'measureUnit@staffRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/measureUnit/manage.html',
                    controller: 'measureUnitController',
                    controllerUrl: 'app/controllers/kpi/measureUnitController'
                }),
                'targetGroupDictionary@staffRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/targetGroupDictionary/manage.html',
                    controller: 'targetGroupDictionaryController',
                    controllerUrl: 'app/controllers/kpi/targetGroupDictionaryController'
                }),
                'configuration@staffRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/configuration/manage.html',
                    controller: 'configurationController',
                    controllerUrl: 'app/controllers/kpi/configurationController'
                }),
                'agentObjectTypeRate@staffRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/agentObjectTypeRate/manage.html',
                    controller: 'agentObjectTypeRateController',
                    controllerUrl: 'app/controllers/kpi/agentObjectTypeRateController'
                })
            }
        })
        .state('departmentdisplay', {
            url: '/kpi/departmentdisplay',
            views: {
                // the main template will be placed here (relatively named)
                '': { templateUrl: '/app/views/kpi/department/display.html' },
                // the child views will be defined here (absolutely named)
                'department@departmentdisplay': angularAMD.route({
                    templateUrl: '/app/views/kpi/department/manage.html',
                    controller: 'departmentController',
                    controllerUrl: 'app/controllers/kpi/departmentController'
                }),
                'staff@departmentdisplay': angularAMD.route({
                    templateUrl: '/app/views/kpi/staff/manage.html',
                    controller: 'staffController',
                    controllerUrl: 'app/controllers/kpi/staffController'
                })
            }
        })
        .state('administratorDepartment', {
            url: '/kpi/administratorDepartment',
            views: {
                // the main template will be placed here (relatively named)
                '': { templateUrl: '/app/views/kpi/administratorDepartment/display.html' },
                // the child views will be defined here (absolutely named)
                'department@administratorDepartment': angularAMD.route({
                    templateUrl: '/app/views/kpi/department/departmentByAdmin.html',
                    controller: 'departmentController',
                    controllerUrl: 'app/controllers/kpi/departmentController'
                }),
                'staff@administratorDepartment': angularAMD.route({
                    templateUrl: '/app/views/kpi/staff/departmentByAdmin.html',
                    controller: 'staffController',
                    controllerUrl: 'app/controllers/kpi/staffController'
                })
            }
        })
        .state('unlockRating', angularAMD.route({
            url: '/kpi/unlockRating',
            templateUrl: '/app/views/kpi/unlockRating/manage.html',
            controller: 'unlockRatingController',
            controllerUrl: 'app/controllers/kpi/unlockRatingController'
        }))
        .state('unlockRatingManage', {
            url: '/kpi/unlockRatingManage/:planId',
            views: {
                '': angularAMD.route({
                    templateUrl: '/app/views/kpi/unlockRating/display.html', 
                    controller: 'unlockRatingController',
                    controllerUrl: 'app/controllers/kpi/unlockRatingController'
                }),
                'unlockRatingDepartment@unlockRatingManage': angularAMD.route({
                    templateUrl: '/app/views/kpi/unlockRatingDepartment/manage.html',
                    controller: 'departmentController',
                    controllerUrl: 'app/controllers/kpi/departmentController'
                }),
                'unlockRatingStaff@unlockRatingManage': angularAMD.route({
                    templateUrl: '/app/views/kpi/unlockRatingStaff/manage.html',
                    controller: 'staffController',
                    controllerUrl: 'app/controllers/kpi/staffController'
                })
            }
        })
        .state('sitecriterion', angularAMD.route({
            url: '/kpi/criterion',
            templateUrl: '/app/views/kpi/criterion/manage.html',
            controller: 'criterionController',
            controllerUrl: 'app/controllers/kpi/criterionController'
        }))
        .state('scienceResearchData', angularAMD.route({
            url: '/kpi/scienceResearchData',
            templateUrl: '/app/views/kpi/scienceResearchData/manage.html',
            controller: 'scienceResearchDataController',
            controllerUrl: 'app/controllers/kpi/scienceResearchDataController'
        }))
        .state('otherActivityData', angularAMD.route({
            url: '/kpi/otherActivityData',
            templateUrl: '/app/views/kpi/otherActivityData/manage.html',
            controller: 'otherActivityDataController',
            controllerUrl: 'app/controllers/kpi/otherActivityDataController'
        }))
        .state('departmentResult', angularAMD.route({
            url: '/kpi/departmentResult/:planId/:agentObjectId',
            templateUrl: '/app/views/kpi/result/departmentResult.html',
            controller: 'resultController',
            controllerUrl: 'app/controllers/kpi/resultController'
        }))
            ////////////////////*********** ABC ********************////////////////////////////////
        .state('evaluationBoard', angularAMD.route({
             data: {
                 roles: ['kpi/evaluationBoard']
             },
             url: '/kpi/evaluationBoard',
             templateUrl: '/app/views/abc/evaluationBoard/manage.html',
             controller: 'evaluationBoardController',
             controllerUrl: 'app/controllers/abc/evaluationBoardController'
         }))
        .state('userEvaluationBoard', angularAMD.route({
            data: {
                roles: ['kpi/userEvaluationBoard']
            },
            url: '/kpi/userEvaluationBoard',
            templateUrl: '/app/views/abc/evaluationBoard/userEvaluationBoard.html',
            controller: 'evaluationBoardController',
            controllerUrl: 'app/controllers/abc/evaluationBoardController'
        }))
        .state('ABC_Criterion', angularAMD.route({
            data: {
                roles: ['kpi/ABC_Criterion']
            },
            url: '/kpi/ABC_Criterion',
            views: {
                // the main template will be placed here (relatively named)
                '': { templateUrl: '/app/views/abc/ABC_Criterion/display.html' },
                // the child views will be defined here (absolutely named)
                'ratingType@ABC_Criterion': angularAMD.route({
                    templateUrl: '/app/views/abc/ABC_Criterion/ratingTypeManage.html',
                    controller: 'ABC_RatingTypeController',
                    controllerUrl: 'app/controllers/abc/ABC_RatingTypeController'
                }),
                'criterion@ABC_Criterion': angularAMD.route({
                    templateUrl: '/app/views/abc/ABC_Criterion/manage.html',
                    controller: 'ABC_CriterionController',
                    controllerUrl: 'app/controllers/abc/ABC_CriterionController'
                })
            }
        }))
        .state('ABC_RatingDetail', angularAMD.route({
             data: {
                 roles: ['kpi/ABC_RatingDetail']
             },
             url: '/kpi/ABC_RatingDetail/:evaluationId/:staffId/:supervisorId/:departmentId/:isAdminRating',
             templateUrl: '/app/views/abc/ABC_RatingDetail/manage.html',
             controller: 'ABC_RatingDetailController',
             controllerUrl: 'app/controllers/abc/ABC_RatingDetailController'
        }))
        .state('ABC_RatingDetailProfessor', angularAMD.route({
            data: {
                roles: ['kpi/ABC_RatingDetailProfessor']
            },
            url: '/kpi/ABC_RatingDetailProfessor/:evaluationId/:staffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/abc/ABC_RatingDetail/professorManage.html',
            controller: 'ABC_RatingDetailController',
            controllerUrl: 'app/controllers/abc/ABC_RatingDetailController'
        }))
        .state('StaffSyntheticEvaluation', angularAMD.route({
            data: {
                roles: ['kpi/StaffSyntheticEvaluation']
            },
            url: '/kpi/StaffSyntheticEvaluation/:evaluationId',
            templateUrl: '/app/views/abc/ABC_RatingDetail/staffSyntheticEvaluation.html',
            controller: 'SyntheticEvaluationController',
            controllerUrl: 'app/controllers/abc/SyntheticEvaluationController'
        }))
        .state('DepartmentSyntheticEvaluation', angularAMD.route({
            data: {
                roles: ['kpi/DepartmentSyntheticEvaluation']
            },
            url: '/kpi/DepartmentSyntheticEvaluation/:evaluationId',
            templateUrl: '/app/views/abc/ABC_RatingDetail/departmentSyntheticEvaluation.html',
            controller: 'SyntheticEvaluationController',
            controllerUrl: 'app/controllers/abc/SyntheticEvaluationController'
        }))
        .state('EvaluationManage', angularAMD.route({
            data: {
                roles: ['kpi/evaluationBoard']
            },
            url: '/kpi/EvaluationManage/:evaluationId',
            templateUrl: '/app/views/abc/evaluationBoard/evaluationManage.html',
            controller: 'EvaluationManageController',
            controllerUrl: 'app/controllers/abc/EvaluationManageController'
        }))
        .state('syntheticEvaluationBoard', angularAMD.route({
            data: {
                roles: ['kpi/syntheticEvaluationBoard']
            },
            url: '/kpi/syntheticEvaluationBoard',
            templateUrl: '/app/views/abc/SyntheticEvaluationBoard/syntheticEvaluationBoard.html',
            controller: 'SyntheticEvaluationBoardController',
            controllerUrl: 'app/controllers/abc/SyntheticEvaluationBoardController'
        }))
        .state('departmentSyntheticEvaluationBoard', angularAMD.route({
            data: {
                roles: ['kpi/departmentSyntheticEvaluationBoard']
            },
            url: '/kpi/departmentSyntheticEvaluationBoard',
            templateUrl: '/app/views/abc/SyntheticEvaluationBoard/departmentSyntheticEvaluationBoard.html',
            controller: 'SyntheticEvaluationBoardController',
            controllerUrl: 'app/controllers/abc/SyntheticEvaluationBoardController'
        }))
        .state('staffDepartment', {
            data: {
                roles: ['kpi/staffDepartment']
            },
            url: '/kpi/staffDepartment',
            views: {
                // the main template will be placed here (relatively named)
                '': { templateUrl: '/app/views/abc/staffDepartment/display.html' },
                // the child views will be defined here (absolutely named)
                'department@staffDepartment': angularAMD.route({
                    templateUrl: '/app/views/abc/staffDepartment/department.html',
                    controller: 'departmentController',
                    controllerUrl: 'app/controllers/abc/departmentController'
                }),
                'staff@staffDepartment': angularAMD.route({
                    templateUrl: '/app/views/abc/staffDepartment/staff.html',
                    controller: 'staffController',
                    controllerUrl: 'app/controllers/abc/staffController'
                })
            }
        })
        .state('classifications', {
            data: {
                roles: ['kpi/classifications']
            },
            url: '/kpi/classifications',
            views: {
                // the main template will be placed here (relatively named)
                '': { templateUrl: '/app/views/abc/ABC_CauHinh/display.html' },
                // the child views will be defined here (absolutely named)
                'classificationSet@classifications': angularAMD.route({
                    templateUrl: '/app/views/abc/ABC_CauHinh/ClassificationSet.html',
                    controller: 'classificationSetController',
                    controllerUrl: 'app/controllers/abc/classificationsController'
                }),
                'classifications@classifications': angularAMD.route({
                    templateUrl: '/app/views/abc/ABC_CauHinh/Classifications.html',
                    controller: 'classificationsController',
                    controllerUrl: 'app/controllers/abc/classificationsController'
                })
            }
        })
         //-------------------------Uis---------------------------------------------
        .state('professorInfo', angularAMD.route({
            url: '/kpi/professorInfo',
            templateUrl: '/app/views/uis/professorInfo.html',
            controller: 'professorInfoController',
            controllerUrl: 'app/controllers/uis/professorInfoController'
        }))
        .state('professorRegists', angularAMD.route({
            url: '/kpi/professorRegists',
            templateUrl: '/app/views/uis/professorRegists.html',
            controller: 'professorRegistsController',
            controllerUrl: 'app/controllers/uis/professorRegistsController.js'
        }))
        .state('professorSchedule', angularAMD.route({
            url: '/kpi/professorSchedule',
            templateUrl: '/app/views/uis/professorSchedule.html',
            controller: 'professorScheduleController',
            controllerUrl: 'app/controllers/uis/professorScheduleController'
        }))
          .state('scoreIndex', angularAMD.route({
              url: '/kpi/scoreIndex',
              templateUrl: '/app/views/uis/scoreIndex.html',
              controller: 'scoreIndexController',
              controllerUrl: 'app/controllers/uis/scoreIndexController'
          }))
         .state('diemQuaTrinh', angularAMD.route({
             url: '/kpi/diemquatrinh',
             templateUrl: '/app/views/uis/diemQuaTrinh.html',
             controller: 'diemQuaTrinhController',
             controllerUrl: 'app/controllers/uis/diemQuaTrinhController'
         }))
       .state('diemThiTheoLopHocPhan', angularAMD.route({
           url: '/kpi/diemthitheolophocphan',
           templateUrl: '/app/views/uis/diemThiTheoLopHocPhan.html',
           controller: 'diemThiTheoLopHocPhanController',
           controllerUrl: 'app/controllers/uis/diemThiTheoLopHocPhanController'
       }))
         .state('diemThiTheoNhom', angularAMD.route({
             url: '/kpi/diemthitheonhom',
             templateUrl: '/app/views/uis/diemThiTheoNhom.html',
             controller: 'diemThiTheoNhomController',
             controllerUrl: 'app/controllers/uis/diemThiTheoNhomController'
         }))
        .state('giaHanNhapDiem', angularAMD.route({
            url: '/kpi/giahannhapdiem',
            templateUrl: '/app/views/uis/giaHanNhapDiem.html',
            controller: 'giaHanNhapDiemController',
            controllerUrl: 'app/controllers/uis/giaHanNhapDiemController'
        }))
         .state('thuLaoGiangVien', angularAMD.route({
             url: '/kpi/thulaogiangvien',
             templateUrl: '/app/views/uis/thuLaoGiangVien.html',
             controller: 'thuLaoGiangVienController',
             controllerUrl: 'app/controllers/uis/thuLaoGiangVienController'
         }))
    }]);

    HRMWebApp.run(['$rootScope', '$state', '$stateParams', '$rootElement','authorization', 'principal',
        function ($rootScope, $state, $stateParams, $rootElement, authorization, principal) {
            $rootScope.$on('$stateChangeStart', function (event, toState, toStateParams) {

                $(".k-widget.k-window").remove();
                $(".jqx-window.jqx-popup").remove();

                $rootScope.toState = toState;
                $rootScope.toStateParams = toStateParams;

                //chỉ các trang chấm công dùng jqx datetimepicker mới load script này => để gõ ngày tháng năm trực tiếp vào input
                //load script này ở trang có kendo datetimepicker sẽ lỗi không lấy được date
                if (!$rootScope.toState.controller && !$rootScope.toState.views) {
                    var script = document.createElement('script');
                    script.src = '/Components/jqwidgets/jqxglobalize.js';

                    document.head.appendChild(script);
                }

                if (!principal.isIdentityResolved())
                    authorization.authorize();

                //$rootElement.on('click', function (e) { e.stopPropagation(); });
            });
        }
    ]);
    HRMWebApp.directive('ngAutoExpand', function () {
        return {
            restrict: 'A',
            link: function ($scope, elem, attrs) {
                elem.bind('keyup', function ($event) {
                    var element = $event.target;
                    $(element).height(0);
                    var height = $(element)[0].scrollHeight;

                    //8 is for the padding
                    if (height < 20) {
                        height = 28;
                    }
                    $(element).height(height - 8);
                });

                //Expand the textarea as soon as it is added to the DOM
                setTimeout(function () {
                    var element = elem;

                    $(element).height(0);
                    var height = $(element)[0].scrollHeight;

                    //8 is for the padding
                    if (height < 20) {
                        height = 28;
                    }
                    $(element).height(height - 8);
                }, 0)
            }
        }
    });
    kendo.culture("vi-VN");
    return angularAMD.bootstrap(HRMWebApp);
});