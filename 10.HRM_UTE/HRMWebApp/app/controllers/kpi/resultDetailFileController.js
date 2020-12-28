define(['app/app','app/services/kpi/fileAttachmentService', 'moment'], function (app) {
    app.controller('resultDetailFileController', ['$scope', '$modalInstance','resultDetailId', 'targetId','fileAttachmentService',
                function ($scope, $modalInstance, resultDetailId,targetId, fileAttachmentService) {
                    $scope.isNew = targetId == MANAGER.GUID_EMPTY || '' ? true : false;
                    //if ($scope.isNew) {
                    //    $scope.obj = {
                    //        Id: MANAGER.GUID_EMPTY,
                    //        Name: "",
                    //        PlanKPIDetail: { Id: planKPIDetailId, PlanStaff: { Id: planStaffId }, TargetGroupDetail: { Id: targetGroupDetailId } }
                    //    };
                    //} else {
                    //    fileAttachmentService.getObj(id).then(function (result) {
                    //        $scope.obj = result.data;
                    //        $scope.obj.PlanKPIDetail = { Id: planKPIDetailId };
                    //    });
                    //}



                    $scope.uploadOptions = {
                        async: {
                            saveUrl: "/FileUpload/SaveResultDetailFile?resultDetailId=" + resultDetailId,
                            autoUpload: true
                        },
                        validation: {
                            allowedExtensions: [".txt", ".gif", ".jpg", ".png", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pps", ".zip", ".rar", ".7z", ".gz", ".gzip"],
                            maxFileSize: 1048576
                        },
                        success: onUploadSuccess,
                    }

                    function onUploadSuccess(e) {
                        $scope.fileGrid.dataSource.read();
                        $scope.fileGrid.refresh();
                    }
                    $scope.fileDataSource = new kendo.data.DataSource({
                        transport: {
                            read: function (options) {
                                return fileAttachmentService.getListByResultDetail(resultDetailId).then(function (result) {
                                    options.success(result.data);
                                });
                            }
                        },
                        pageSize: 20
                    });
                    $scope.fileMainGridOptions = {
                        sortable: true,
                        pageable: false,
                        columns: [{
                            field: "FileName",
                            title: "Tên file"
                        },
                        {
                            template: "<div style='width: 30px;'><button ng-click='downdLoadFile(\"#:data.Id #\")' class='btn btn-block btn-success btn-xs'><i class='fa fa-download'></i>  </button></div>",
                            width: "50px"
                        },
                        {
                            template: "<div style='width: 30px;'><button ng-click='deletelFile(\"#:data.Id #\")' class='btn btn-block btn-danger btn-xs'><i class='fa fa-times'></i>  </button></div>",
                            width: "50px",
                        }],
                    };
                    $scope.downdLoadFile = function (Id) {
                        fileAttachmentService.downloadFile(Id);
                    }

                    $scope.deletelFile = function (Id) {
                        if (Id == "") {
                            alert("Bạn chưa chọn phần tử");
                            return;
                        }
                        var isValid = window.confirm("Bạn có muốn xóa phần tử này không?");
                        if (!isValid)
                            return;
                        fileAttachmentService.getObj(Id).then(function (result) {
                            result.data.PlanDetailFile = { Id: result.data.planDetailFileId }
                            fileAttachmentService.Delete(result.data).then(function () {

                                if (result.data == 1) {
                                    alert("Xóa thất bại!");
                                }
                                $scope.fileGrid.dataSource.read();
                                $scope.fileGrid.refresh();
                            });
                        });
                    }

                    $scope.save = function () {
                        fileAttachmentService.Save($scope.obj).then(function (result) {
                            if (result != MANAGER.GUID_EMPTY) {
                                alert("Lưu thành công");
                                $modalInstance.close(result);
                            }
                            else {
                                alert("Lưu thất bại");
                            }
                        });
                    };

                    $scope.ok = function () {
                        var result = { resultDetailId: resultDetailId, targetId: targetId }
                        $modalInstance.close(result);
                    };
                }
    ]);
});