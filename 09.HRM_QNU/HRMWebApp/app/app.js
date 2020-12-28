define(['angularAMD', 'ui-route', 'ui-validate', 'ui-bootstrap', 'kendo', 'ui-bootstrap-tpls'], function (angularAMD) {
    "use strict";
    var HRMWebApp = angular.module('HRMWebApp', ['ui.router','ui.validate', 'ui.bootstrap', 'kendo.directives']);
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
                     var session = null;
                     $.ajax({
                         type: 'POST',
                         url: '/Authentication/GetUserSessionInfo',
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         async: false,
                         success: function (result) {
                             session = result;
                             $rootScope.session = session;
                             $rootScope.MANAGER = MANAGER;
                             _authenticated = true;
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
                             //deferred.resolve(_identity);
                              $.ajax({
                                 type: 'GET',
                                 url: '/Api/SecurityApi/GetUserViewRoleIds?userId=' + session.UserId,
                                 contentType: "application/json; charset=utf-8",                                
                                 dataType: "json",
                                 async: true,
                                 success: function (result) {                                   
                                     _identity["roles"]=_identity["roles"].concat(result);
                                     deferred.resolve(_identity);
                                 },
                                 error: function (result) {
                                     deferred.resolve(_identity);
                                 }
                             });
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
                               $rootScope.isSupervisor = $rootScope.isSupervisor ? $rootScope.isSupervisor : 0;
                               if ( !$rootScope.isSupervisor==1 && $rootScope.toState.data.roles && $rootScope.toState.data.roles.length > 0 && !principal.isInAnyRole($rootScope.toState.data.roles)) {
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
    HRMWebApp.config(['$stateProvider', '$urlRouterProvider','$locationProvider', function ($stateProvider, $urlRouterProvider, $locationProvider) {
      
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
                url: '/',
                templateUrl: '/default.html'
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
            .state('chitietchamcong', angularAMD.route({
                data: {
                    roles: ['kpi/chitietchamcong']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/chitietchamcong',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/ChiTietChamCong.html'
            }))
            .state('ngaynghitrongnam', angularAMD.route({
                data: {
                    roles: ['kpi/ngaynghitrongnam']
                },
                resolve: {
                    authorize: ['authorization',
                    function (authorization) {
                        return authorization.authorize();
                    }
                    ]
                },
                url: '/kpi/ngaynghitrongnam',
                templateUrl: '/app/views/chamcong/NgayNghiTrongNam/Manage.html'
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
            .state('dangkychamcongngoaigio', angularAMD.route({
                data: {
                    roles: ['kpi/dangkychamcongngoaigio']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
                url: '/kpi/dangkychamcongngoaigio',
                templateUrl: '/app/views/chamcong/QuanLyChamCong/DangKyChamCongNgoaiGio.html'
            }))
            .state('quanlydangkyccng', angularAMD.route({
                data: {
                    roles: ['kpi/quanlydangkyccng']
                },
                resolve: {
                    authorize: ['authorization',
                      function (authorization) {
                          return authorization.authorize();
                      }
                    ]
                },
            url: '/kpi/quanlydangkyccng',
            templateUrl: '/app/views/chamcong/QuanLyChamCong/DangKyChamCongNgoaiGio_Manage.html'
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
            data: {
                roles: ['kpi/planKPI']
            },
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
        .state('role', angularAMD.route({
            url: '/kpi/role',
            templateUrl: '/app/views/kpi/role/manage.html',
            controller: 'roleController',
            controllerUrl: 'app/controllers/kpi/roleController'
        }))
        .state('userPlanKPI', angularAMD.route({
            url: '/kpi/userPlanKPI',
            templateUrl: '/app/views/kpi/planKPIDetail/planManage.html',
            controller: 'planStaffDetailController',
            controllerUrl: 'app/controllers/kpi/planStaffDetailController'
        }))
        .state('professorPlankpidetail', angularAMD.route({
            data: {
                roles: ['kpi/professorPlankpidetail']
            },
            url: '/kpi/professorPlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor',
            templateUrl: '/app/views/kpi/planKPIDetail/professorManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('schoolManagePlankpidetail', angularAMD.route({
            data: {
                roles: ['kpi/schoolManagePlankpidetail']
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
        .state('facultyManagePlankpidetail', angularAMD.route({
            data: {
                roles: ['kpi/facultyManagePlankpidetail']
            },
            url: '/kpi/facultyManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/facultyManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController',          
        }))
        .state('subFacultyManagePlankpidetail', angularAMD.route({
            data: {
                roles: ['kpi/subFacultyManagePlankpidetail']
            },
            url: '/kpi/subFacultyManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/subFacultyManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('principalPlankpidetail', angularAMD.route({
            data: {
                roles: ['kpi/principalPlankpidetail']
            },
            url: '/kpi/principalPlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/principalManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('vicePrincipalPlankpidetail', angularAMD.route({
            data: {
                roles: ['kpi/vicePrincipalPlankpidetail']
            },
            url: '/kpi/vicePrincipalPlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/vicePrincipalManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
       .state('subDepartmentPlankpiDetail', angularAMD.route({
           data: {
               roles: ['kpi/subDepartmentPlankpiDetail']
           },
           url: '/kpi/subDepartmentPlankpiDetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
           templateUrl: '/app/views/kpi/planKPIDetail/subDepartmentManage.html',
                 controller: 'planKPIDetailController',
                 controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('subjectManagePlankpidetail', angularAMD.route({
            data: {
                roles: ['kpi/subjectManagePlankpidetail']
            },
            url: '/kpi/subjectManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor',
            templateUrl: '/app/views/kpi/planKPIDetail/subjectManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('subSubjectManagePlankpidetail', angularAMD.route({
            data: {
                roles: ['kpi/subjectManagePlankpidetail']
            },
            url: '/kpi/subjectManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor',
            templateUrl: '/app/views/kpi/planKPIDetail/subSubjectManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        
         .state('departmentManagePlankpidetail', angularAMD.route({
             data: {
                 roles: ['kpi/departmentManagePlankpidetail']
             },
             url: '/kpi/departmentManagePlankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
             templateUrl: '/app/views/kpi/planKPIDetail/departmentManage.html',
             controller: 'planKPIDetailController',
             controllerUrl: 'app/controllers/kpi/planKPIDetailController'
         }))
        .state('plankpidetail', angularAMD.route({            
            data: {
                roles: ['kpi/plankpidetail']
            },
            url: '/kpi/plankpidetail/:planId/:agentObjectId/:normalStaffId/:isSupervisor/:isConfig',
            templateUrl: '/app/views/kpi/planKPIDetail/staffManage.html',
            controller: 'planKPIDetailController',
            controllerUrl: 'app/controllers/kpi/planKPIDetailController'
        }))
        .state('userPlan', angularAMD.route({
            data: {
                roles: ['kpi/userPlan']
            },
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
            data: {
                roles: ['kpi/professorRatingKPI']
            },
            url: '/kpi/professorRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/professorManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('ratingKPI', angularAMD.route({
            data: {
                roles: ['kpi/ratingKPI']
            },
            url: '/kpi/ratingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/manage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('departmentRatingKPI', angularAMD.route({
            data: {
                roles: ['kpi/departmentRatingKPI']
            },
            url: '/kpi/departmentRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/departmentManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
         .state('subDepartmentRatingKPI', angularAMD.route({
             data: {
                 roles: ['kpi/subDepartmentRatingKPI']
             },
             url: '/kpi/subDepartmentRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
             templateUrl: '/app/views/kpi/ratingKPI/subDepartmentManage.html',
             controller: 'ratingKPIController',
             controllerUrl: 'app/controllers/kpi/ratingKPIController'
         }))
        .state('subjectRatingKPI', angularAMD.route({
            data: {
                roles: ['kpi/subjectRatingKPI']
            },
            url: '/kpi/subjectRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/subjectManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('subSubjectRatingKPI', angularAMD.route({
            data: {
                roles: ['kpi/subSubjectRatingKPI']
            },
            url: '/kpi/subSubjectRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/subSubjectManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('facultyRatingKPI', angularAMD.route({
            data: {
                roles: ['kpi/facultyRatingKPI']
            },
            url: '/kpi/facultyRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/facultyManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('subFacultyRatingKPI', angularAMD.route({
            data: {
                roles: ['kpi/subFacultyRatingKPI']
            },
            url: '/kpi/subFacultyRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/subFacultyManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
        .state('vicePrincipalRatingKPI', angularAMD.route({
            data: {
                roles: ['kpi/vicePrincipalRatingKPI']
            },
            url: '/kpi/vicePrincipalRatingKPI/:planId/:agentObjectId/:planStaffId/:supervisorId/:departmentId/:isAdminRating',
            templateUrl: '/app/views/kpi/ratingKPI/vicePrincipalManage.html',
            controller: 'ratingKPIController',
            controllerUrl: 'app/controllers/kpi/ratingKPIController'
        }))
            .state('principalRatingKPI', angularAMD.route({
                data: {
                    roles: ['kpi/principalRatingKPI']
                },
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
        //.state('administratorDepartment', {
        //    url: '/kpi/administratorDepartment',
        //    views: {
        //        // the main template will be placed here (relatively named)
        //        '': { templateUrl: '/app/views/kpi/administratorDepartment/display.html' },
        //        // the child views will be defined here (absolutely named)
        //        'department@administratorDepartment': angularAMD.route({
        //            templateUrl: '/app/views/kpi/department/departmentByAdmin.html',
        //            controller: 'departmentController',
        //            controllerUrl: 'app/controllers/kpi/departmentController'
        //        }),
        //        'staff@administratorDepartment': angularAMD.route({
        //            templateUrl: '/app/views/kpi/staff/departmentByAdmin.html',
        //            controller: 'staffController',
        //            controllerUrl: 'app/controllers/kpi/staffController'
        //        })
        //    }
        //})
        .state('agentObjectTypeRole', {
            url: '/kpi/agentObjectTypeRole',
            views: {
                // the main template will be placed here (relatively named)
                '': { templateUrl: '/app/views/kpi/agentObjectTypeRole/display.html' },
                // the child views will be defined here (absolutely named)
                'agentObjectType@agentObjectTypeRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/agentObjectTypeRole/agentObjectType.html',
                    controller: 'agentObjectTypeController',
                    controllerUrl: 'app/controllers/kpi/agentObjectTypeController'
                }),
                'role@agentObjectTypeRole': angularAMD.route({
                    templateUrl: '/app/views/kpi/role/manage.html',
                    controller: 'roleController',
                    controllerUrl: 'app/controllers/kpi/roleController'
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
            templateUrl: '/app/views/abc/ABC_Criterion/manage.html',
            controller: 'ABC_CriterionController',
            controllerUrl: 'app/controllers/abc/ABC_CriterionController'
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
                roles: ['kpi/EvaluationManage']
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
    }]);

    HRMWebApp.run(['$rootScope', '$state', '$stateParams', '$rootElement','authorization', 'principal',
       function ($rootScope, $state, $stateParams, $rootElement, authorization, principal) {
           $rootScope.$on('$stateChangeStart', function (event, toState, toStateParams) {
               $rootScope.toState = toState;
               $rootScope.toStateParams = toStateParams;

               if (!principal.isIdentityResolved())
                   authorization.authorize();

               //$rootElement.on('click', function (e) { e.stopPropagation(); });
           });
       }]);
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
    //kendo.culture("vi-VN");
    return angularAMD.bootstrap(HRMWebApp);
});