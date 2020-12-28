
define(['app/app', 'app/services/kpi/ratingKPIService', 'app/services/kpi/targetGroupDetailService', 'app/services/kpi/criterionService', 'app/services/kpi/staffService', 'app/services/kpi/planKPIService', 'app/services/kpi/authService', 'app/services/kpi/fileAttachmentService', 'app/services/kpi/scienceResearchService', 'app/services/kpi/professorOtherActivityService', 'app/controllers/kpi/ratingKPIInformationController', 'app/controllers/kpi/ratingKPIOtherdetailController', 'app/controllers/kpi/resultDetailFileController', 'app/directives/directives', 'app/services/kpi/professorCriterionService', 'moment'], function (app) {
    "use strict";

    app.controller('ratingKPIController', ['$scope', '$document', '$modal', '$rootScope', '$state', '$stateParams', 'ratingKPIService', 'targetGroupDetailService', 'criterionService', 'staffService', 'planKPIService', 'authService', 'fileAttachmentService', 'scienceResearchService', 'professorOtherActivityService', 'professorCriterionService',
            function ($scope, $document, $modal, $rootScope, $state, $stateParams, ratingKPIService, targetGroupDetailService, criterionService, staffService, planKPIService, authService, fileAttachmentService, scienceResearchService, professorOtherActivityService, professorCriterionService) {
                $("#sidebar").addClass("menu-compact");
                var moment = require('moment');
                $scope.TABLECOLORS = MANAGER.TABLECOLORS;
                $scope.recordTemp = 0;
                $scope.departmentLeaderId = "";
                $scope.departmentLeaderName = "";
                $scope.IsLoading = false;
                $scope.planId = $stateParams.planId;
                $scope.planName = "";
                $scope.planType = 0;
                $scope.isExpired = false;
                $scope.isFreeRating = true;
                //$scope.isFreeRating = false; //không được tự do đánh giá nữa (trường hợp đã ngoài thời hạn đánh giá)      --sau này phải đổi thành false, tạm thời cho đánh giá tự do
                $scope.agentObjectId = $stateParams.agentObjectId;
                $scope.isAdminRating = $rootScope.isAdminRating ? $rootScope.isAdminRating : 0;// $stateParams.isAdminRating;
                if ($stateParams.isAdminRating != null && $stateParams.isAdminRating != undefined) { //sửa ngày 16/11/2016 lấy $scope.isAdminRating từ url
                    $scope.isAdminRating = $stateParams.isAdminRating;
                }
                $scope.isRate = $stateParams.isRate;
                $scope.planStaffId = $stateParams.planStaffId != "" ? $stateParams.planStaffId : MANAGER.GUID_EMPTY;
                $scope.supervisorId = $stateParams.supervisorId != "" ? $stateParams.supervisorId : MANAGER.GUID_EMPTY;
                $scope.departmentId = $stateParams.departmentId != "" ? $stateParams.departmentId : MANAGER.GUID_EMPTY;
                $scope.staffResultDetaitObject = [];
                $scope.criterionDictionaries = [];
                $scope.isRecordValid = true;
                $scope.numericOptions = {
                    format: "n0",
                    min: 0,
                    max: 100,
                    ForceMinValueOnEmpty: true
                }

                $scope.RankingRecordList = {
                    A: [90, 100],
                    B: [73, 89],
                    C: [53, 72],
                    D: [38, 52],
                    E: [1, 37],
                    F: [0, 0]
                }

                $scope.rank = function () {
                    var result = "";
                    var totalSumRecord = $scope.totalsumProfessor();

                    for (var key in $scope.RankingRecordList) {
                        if (parseInt(totalSumRecord) >= parseInt($scope.RankingRecordList[key][0]) && parseInt(totalSumRecord) <= parseInt($scope.RankingRecordList[key][1])) {
                            result = key;
                            break;
                        }
                    }
                    return result;
                }

                $scope.rankProfessor = function () {
                    var result = "";
                    var totalSumRecord = $scope.totalsumProfessor2();

                    for (var key in $scope.RankingRecordList) {
                        if (parseInt(totalSumRecord) >= parseInt($scope.RankingRecordList[key][0]) && parseInt(totalSumRecord) <= parseInt($scope.RankingRecordList[key][1])) {
                            result = key;
                            break;
                        }
                    }
                    return result;
                }

                $scope.rankSecondRecord = function (secondRecord) {
                    var result = "";
                    var totalSumRecord = secondRecord;

                    for (var key in $scope.RankingRecordList) {
                        if (parseInt(totalSumRecord) >= parseInt($scope.RankingRecordList[key][0]) && parseInt(totalSumRecord) <= parseInt($scope.RankingRecordList[key][1])) {
                            result = key;
                            break;
                        }
                    }
                    return result;
                }

                //--------------------------------------------------Sum functions--------------------------------//
                $scope.sum = function (items, prop) {
                    return items.reduce(function (a, b) {
                        return (parseFloat(a) + (parseFloat(b[prop]) * (parseFloat(b['Density']))));
                    }, 0).toFixed(1);
                };

                // dùng trong bảng đánh giá
                $scope.sumResult = function (items, prop) {
                    return items.reduce(function (a, b) {
                        return (parseFloat(a) + (parseFloat(b[prop]) * (parseFloat(b['MaxRecord']) / 100)));
                    }, 0).toFixed(1);
                };

                $scope.sumTotalResult = function (items, prop) {
                    return items.reduce(function (a, b) {
                        return (parseFloat(a) + (parseFloat(b[prop] + b['Record']) / 2 * (parseFloat(b['MaxRecord']) / 100)));
                    }, 0).toFixed(1);
                };

                $scope.sumProfessors = function (items, prop) {
                    return items.reduce(function (a, b) {
                        return (parseFloat(a) + (parseFloat(b[prop])) / items.length);
                    }, 0).toFixed(1);
                };
                $scope.sumtotalProfessors = function (items, prop) {
                    return items.reduce(function (a, b) {
                        return (parseFloat(a));
                    }, 0).toFixed(1);
                };
                //----

                //$scope.sumProfessor = function (items, prop)
                //{
                //    var result = 0;        
                //    var obj = $scope.staffResultDetaitObject.ProfessorAdditionalResultDetailDTOs;
                //     var a = obj.length;
                //    for (i = 0; i < a; i++) 
                //    {
                //        if ((obj[i].TargetGroupDetailString == items[1].TargetGroupDetail.Id) && (prop=='Record'))
                //        {
                //            result += parseFloat($scope.staffResultDetaitObject.ProfessorAdditionalResultDetailDTOs[i].Record);
                //        }
                //        else if ((obj[i].TargetGroupDetailString == items[1].TargetGroupDetail.Id) && (prop=='SupervisorRecord'))
                //        {
                //            result += parseFloat($scope.staffResultDetaitObject.ProfessorAdditionalResultDetailDTOs[i].SupervisorRecord);
                //        }          
                //    }
                //    var result1= items.reduce(function (a, b) {
                //            return (parseFloat(a) + (parseFloat(b[prop]) * (parseFloat(b['Density']))));
                //        }, 0);
                //    return (parseFloat(result) + parseFloat(result1)).toFixed(1);
                //};
                $scope.sumProfessor = function (items, prop) {
                    return items.reduce(function (a, b) {
                        return (parseFloat(a) + parseFloat(b[prop]));
                    }, 0).toFixed(1);
                };
                $scope.totalSum = function () {
                    var result = 0;
                    $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                        result += $scope.sumProfessor(item.ResultDetailDTOs, 'Record') * (item.Density / 100) / 2 + $scope.sumProfessor(item.ResultDetailDTOs, 'Density') * (item.Density / 100) / 2;
                    });
                    var totalBonusRecord = $scope.staffResultDetaitObject.BonusRecordList.reduce(function (a, b) {
                        return (parseFloat(a) + (parseFloat(b["Record"]))).toFixed(1);
                    }, 0);

                    result += parseFloat(totalBonusRecord);
                    $scope.staffResultDetaitObject.TotalSumRecord = result;
                    if ($scope.isAdminRating == 1) {
                        $scope.recordTemp = result;
                    }
                    return result.toFixed(1);
                };
                $scope.totalSupervisorRecord = function () {
                    var result = 0;
                    $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                        result += $scope.sum(item.ResultDetailDTOs, 'SupervisorRecord') * (item.Density / 100);
                    });
                    return result.toFixed(1);
                };
                $scope.totalStaffRecord = function () {
                    var result = 0;
                    $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                        result += $scope.sum(item.ResultDetailDTOs, 'Record') * (item.Density / 100);
                    });
                    $scope.staffResultDetaitObject.TempRecord = result;
                    return result.toFixed(1);
                };

                $scope.totalsumProfessor = function () {
                    var result = 0;
                    $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                        if (item.TargetGroupDetailTypeId != 0){
                            result += $scope.sumProfessor(item.ResultDetailDTOs, 'Record') * (item.Density / 100) / 2 + $scope.sumProfessor(item.ResultDetailDTOs, 'SupervisorRecord') * (item.Density / 100) / 2;
                        }
                        else if (item.ResultDetailDTOs.length > 0)
                        {
                            result += ($scope.sumProfessor(item.ResultDetailDTOs, 'Record') / item.ResultDetailDTOs.length * (item.Density / 100) / 2) + ($scope.sumProfessor(item.ResultDetailDTOs, 'SupervisorRecord') / item.ResultDetailDTOs.length * (item.Density / 100) / 2);
                        }
                    });
                    $scope.staffResultDetaitObject.TotalSumRecord = result;
                    return result.toFixed(1);
                };

                $scope.sumRecord = function (a,b) {
                    var result = 0;
                    result += (a + b) / 2;
                    return result.toFixed(1);
                }
                //Không sử dụng nữa
                //$scope.totalsumProfessor = function () {
                //    var result = 0;
                //    $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                //        result += $scope.sumProfessor(item.ResultDetailDTOs, 'SupervisorRecord') * (item.Density / 100);
                //    });
                //    var totalBonusRecord = $scope.staffResultDetaitObject.BonusRecordList.reduce(function (a, b) {
                //        return (parseFloat(a) + (parseFloat(b["Record"]))).toFixed(1);
                //    }, 0);
                //    result += parseFloat(totalBonusRecord);
                //    $scope.staffResultDetaitObject.TotalSumRecord = result;
                //    return result.toFixed(1);
                //};

                $scope.totalsumProfessor2 = function () {
                    var result = 0;
                    $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                        if (item.TargetGroupDetailTypeId == 0) {
                            result += $scope.sumProfessor(item.ResultDetailDTOs, 'SupervisorRecord') * (item.Density / 100);
                        }
                        else if (item.TargetGroupDetailTypeId == 5) {
                            result += $scope.sumProfessor(item.ResultDetailDTOs, 'SupervisorRecord') * (item.Density / 100);
                        }
                        else if (item.TargetGroupDetailTypeId == 4) {
                            result += $scope.sumProfessor(item.ResultDetailDTOs, 'SupervisorRecord') * (item.Density / 100);
                        }
                    });
                    var totalBonusRecord = $scope.staffResultDetaitObject.BonusRecordList.reduce(function (a, b) {
                        return (parseFloat(a) + (parseFloat(b["Record"]))).toFixed(1);
                    }, 0);
                    result += parseFloat(totalBonusRecord);
                    result += parseFloat($scope.staffResultDetaitObject.diemthuongdiemtruRecord)
                    $scope.staffResultDetaitObject.TotalSumRecord = result;
                    return result.toFixed(1);
                }
                //----------------------------------------------------------------------------------------------------------//

                $scope.criterionDictionaryUpdate = function (selectedValue, TargetGroupId, PlanKPIDetailId) {
                    $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                        if (item.Id == TargetGroupId) {
                            var selectedCount = 0;
                            $.each(item.ResultDetailDTOs, function (idx, item2) {
                                if (item2.PlanKPIDetailId == PlanKPIDetailId) {
                                    var dictionaryRecord = 0;
                                    $.each(item2.CriterionDictionaries, function (idx, item3) {
                                        if (item3.Id == selectedValue)
                                            dictionaryRecord = item3.Record;
                                    });
                                    item2.Record = dictionaryRecord;
                                    item2.SupervisorRecord = dictionaryRecord;
                                }
                            });
                        }
                    });
                }

                //authService.getCurrentUser().then(function (result) {
                //    ratingKPIService.getRatingResultDetail($scope.planId,$scope.agentObjectId,$scope.supervisorId).then(function (result) {
                //        $scope.staffResultDetaitObject = result.data;               
                //    });

                //});
                $scope.dateCompare = function (date1, date2) {
                    var result = -1;
                    var isSame = moment(date1).isSame(date2, 'day');
                    if (!isSame) {
                        var isAfter = moment(date1).isAfter(date2, 'day');
                        if (isAfter) {
                            result = 1;
                        }
                        else
                            result = -1;
                    }
                    else
                        result = 0;
                    return result;
                }

                function getRatingDetailList() {
                    $scope.IsLoading = true;
                    ratingKPIService.getRatingResultDetail($scope.planId, $scope.agentObjectId, $scope.planStaffId, $scope.supervisorId, $scope.departmentId, $scope.isAdminRating, $scope.isRate).then(function (result) {
                        if (result.Id == MANAGER.GUID_EMPTY) {
                            //alert("Chưa soạn kế hoạch");
                            if ($scope.isAdminRating)
                                window.location = "/#/kpi/planKPI";
                            else
                                window.location = "/#/kpi/userPlanKPI";
                        }
                        else {
                            if (result.data.PlanStaffId == MANAGER.GUID_EMPTY) {
                               // alert("Chưa soạn kế hoạch");
                                if ($scope.isAdminRating)
                                    window.location = "/#/kpi/planKPI";
                                else
                                    window.location = "/#/kpi/userPlanKPI";
                            }

                            $scope.staffResultDetaitObject = result.data;
                            setTimeout(function () {
                                $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                                    if (item.TargetGroupDetailTypeId == 4) {
                                        $.each(item.ResultDetailDTOs, function (idx2, item2) {
                                            $.each(item2.ProfessorOtherActivities, function (idx3, item3) {
                                                fileAttachmentService.getListByActivityDetail(item3.Id).then(function (result) {
                                                    item3.FileAttachments = result.data;
                                                });
                                            });
                                        });
                                    }
                                });
                            }, 500);

                            $scope.planType = result.data.PlanTypeId;

                            $scope.startRatingTime = $scope.staffResultDetaitObject.StartRatingTime;
                            $scope.endRatingTime = $scope.staffResultDetaitObject.EndRatingTime;
                            var currentDate = new Date();
                            $scope.isExpired = !(($scope.dateCompare($scope.startRatingTime, currentDate) <= 0 && $scope.dateCompare($scope.endRatingTime, currentDate) >= 0));

                            //canh đều chiều cao các dòng
                            setTimeout(function () {
                                $scope.refreshEqualHeight();
                            }, 70);
                        }
                        $scope.IsLoading = false;
                    });
                }
                getRatingDetailList();

                $scope.refreshEqualHeight = function () {
                    var iiparent = 1;
                    $('.table-equal_height').each(function () {
                        var parentOfParentSelector = ".table-equal_height.table-" + iiparent;
                        var iparent = 1;
                        $(this).find($('.tr-equal_height')).each(function () {
                            var parentSelector = ".tr-equal_height.tr-" + iparent;
                            var $el = $(this);
                            var child = $(".equal_height.one");
                            var child2 = $('.row-height');

                            $el.find(child).each(function () {
                                var $el2 = $(this);

                                var index = 1;
                                $el2.find(child2).each(function () {
                                    var $el3 = $(this);
                                    var childSelector = parentOfParentSelector + ' ' + parentSelector + ' .equal_height:not(.one) .row-height.row-' + index;

                                    $(childSelector).css('height', $el3.height());
                                    index++;
                                });

                            });
                            iparent++;
                        });
                        iiparent++;
                    });
                }

                $scope.staffResultDetaitObject = {
                    Id: $scope.staffId,
                    targetGroupList: []
                };


                $scope.save = function () {
                    if (!$scope.formValid()) {
                        alert("Dữ liệu chưa hợp lệ");
                        return;
                    }


                    var valid = window.confirm("Bạn muốn lưu thay đổi không?");
                    if (!valid)
                        return;
                    $scope.staffResultDetaitObject.TotalRecordSecond = $scope.recordTemp;
                    $scope.IsLoading = true;
                    ratingKPIService.Save($scope.staffResultDetaitObject).then(function () {
                        //alert("Thành công!");
                        Notify("Thành công", 'top-right', '2000', 'success', 'fa-check-square-o', true);
                        $state.go(
                            $state.current,
                            {},
                            { reload: true });
                        $scope.IsLoading = false;
                    });
                };
                $scope.lock = function () {
                    var valid = window.confirm("Bạn sẽ không thể chỉnh sửa điểm sau khi khóa, bạn có muốn tiếp tục không?");
                    if (!valid)
                        return;
                    ratingKPIService.Lock($scope.staffResultDetaitObject).then(function () {
                        alert("Thành công!");
                        $state.go($state.current, {}, { reload: true });
                    });
                };
                $scope.unlock = function () {
                    ratingKPIService.getUnlock($scope.staffResultDetaitObject.RatingResultId).then(function () {
                        alert("Thành công!");
                        $state.go($state.current, {}, { reload: true });
                    });
                };

                $scope.cancel = function () {
                    ratingKPIService.Save($scope.staffResultDetaitObject).then(function () {
                        alert("Thành công!");
                    });
                };

                $scope.downloadFile = function (Id) {
                    fileAttachmentService.downloadFile(Id);
                }

                //var queryResult = $document[0].getElementByClassName("ng-hide");
                //var wrappedID = angular.element(queryResult);


                $scope.$watch(
                  function () { return angular.element('.error').attr("class"); },
                  function (newVal, oldVal) {
                      /* execute onShow here if newVal === false */
                  }
                );

                $scope.formChange = function () {
                    var elements = $(".error");
                    var isvalid = true;
                    $.each(elements, function (idx, item) {
                        if (!$(item).hasClass("ng-hide")) {
                            $scope.formValid = false;
                            return;
                        }
                    });
                }

                //staffService.getDepartmentLeader($scope.departmentId).then(function (result) {
                //    $scope.departmentLeaderId = result.data.Id;
                //    $scope.departmentLeaderName = result.data.Name;
                //    //Nếu null thì gán bằng ''
                //    if (!$scope.departmentLeaderName) {
                //        $scope.departmentLeaderName = "";
                //    }
                //});

                staffService.getDirectLeader($scope.departmentId, $scope.agentObjectId, $scope.planStaffId).then(function (result) {
                    $scope.departmentLeaderId = result.data.Id;
                    $scope.departmentLeaderName = result.data.Name;
                    //Nếu null thì gán bằng ''
                    if (!$scope.departmentLeaderName) {
                        $scope.departmentLeaderName = "";
                    }
                });


                planKPIService.getObj($scope.planId).then(function (result) {
                    $scope.planName = result.data.Name;
                });


                $scope.validateRecord = function (value, note, fileCount) {
                    return !(value > 60 && (note == null || note == '') && fileCount <= 0);

                }

                $scope.formValid = function () {
                    var result = true;
                    $.each($(".error"), function (idx, item) {
                        if (!$(item).hasClass("ng-hide")) {
                            result = false;
                        }
                    });
                    return result;
                }

                $scope.newResultDetaillFile = function (id, targetId) {
                    $scope.isEdit = false;
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: '/app/views/kpi/ratingKPI/resultDetailFileDetail.html',
                        controller: 'resultDetailFileController',
                        backdrop: 'static',
                        keyboard: false,
                        resolve: {
                            resultDetailId: function () {
                                return id;
                            },
                            targetId: function () {
                                return targetId;
                            }
                        }
                    }).result.then(function (result) {
                        setTimeout(function () {
                            $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                                if (item.Id == result.targetId) {
                                    $.each(item.ResultDetailDTOs, function (idx, item2) {
                                        if (item2.Id == result.resultDetailId) {

                                            fileAttachmentService.getListByResultDetail(item2.Id).then(function (item2Result) {
                                                item2.FileAttachments = item2Result.data;

                                                return;
                                            });
                                        }
                                    });
                                }
                            });
                            $.each($scope.staffResultDetaitObject.ProfessorAdditionalResultDetailDTOs, function (idx, item) {
                                if (item.Id == result.resultDetailId) {
                                    fileAttachmentService.getListByResultDetail(item.Id).then(function (itemResult) {
                                        item.FileAttachments = itemResult.data;
                                        return;
                                    });
                                }
                            });
                            $.each($scope.staffResultDetaitObject.BonusRecordList, function (idx, item) {
                                if (item.Id == result.resultDetailId) {
                                    fileAttachmentService.getListByResultDetail(item.Id).then(function (itemResult) {
                                        item.FileAttachments = itemResult.data;
                                        return;
                                    });
                                }
                            });
                            $scope.$apply();
                        }, 500);

                        //$scope.fileGrid.dataSource.read();
                        //$scope.fileGrid.refresh();

                    });
                }

                $scope.editPlan = function (planDetailId, criterionTypeId, targetGroupDetailTypeId, criterionId) {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: "/app/views/kpi/ratingKPI/detail.html",
                        controller: 'ratingKPIInformationController',
                        size: 'lg',
                        resolve: {
                            planDetailId: function () {
                                return planDetailId;
                            },
                            criterionTypeId: function () {
                                return criterionTypeId;
                            },
                            targetGroupDetailTypeId: function () {
                                return targetGroupDetailTypeId;
                            },
                            criterionId: function () {
                                return criterionId;
                            }
                        }
                    }).result.finally(function () {
                        setTimeout(function () {
                            $scope.refreshResultDetailList(planDetailId, targetGroupDetailTypeId);
                            $scope.$apply();
                        }, 100);
                    });
                }

                $scope.refreshResultDetailList = function (id, targetGroupDetailTypeId) {
                    if (targetGroupDetailTypeId == 4) {
                        $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                            $.each(item.ResultDetailDTOs, function (idx2, item2) {
                                if (item2.PlanKPIDetailId == id) {
                                    professorOtherActivityService.getListByPlanDetail(id).then(function (result) {
                                        item2.ProfessorOtherActivities = result.data;

                                    });
                                    setTimeout(function () {
                                        $.each(item2.ProfessorOtherActivities, function (idx3, item3) {
                                            fileAttachmentService.getListByActivityDetail(item3.Id).then(function (result) {
                                                item3.FileAttachments = result.data;
                                            });
                                            //  $scope.ProfessorOtherActivitiesId = item3.Id;     
                                        });
                                    }, 100);
                                }
                            });
                        });
                    }
                    if (targetGroupDetailTypeId == 5) {
                        $.each($scope.staffResultDetaitObject.TargetGroupRatingDTOs, function (idx, item) {
                            $.each(item.ResultDetailDTOs, function (idx2, item2) {
                                if (item2.PlanKPIDetailId == id) {
                                    scienceResearchService.getListByPlanDetail(id).then(function (result) {
                                        item2.ScienceResearches = result.data;
                                    });
                                }
                            });
                        });
                    }
                }

                $scope.viewdetail = function (ManageCode) {
                    var modalInstance = $modal.open({
                        animation: true,
                        templateUrl: "/app/views/kpi/ratingKPI/OtherDetail.html",
                        controller: 'ratingKPIOtherdetailController',
                        size: 'lg',              
                        resolve: {
                            ManageCode: function () {
                                return ManageCode;
                            }
                        }
                    }).result.then(function (result) {

                    });
                }
            }
    ]);
});