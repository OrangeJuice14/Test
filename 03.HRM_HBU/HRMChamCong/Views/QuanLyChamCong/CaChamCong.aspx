<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CaChamCong.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.CaChamCong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        th, td {
            padding: 5px;
        }
    </style>
    <script type="text/javascript">
        function ViewModel(datagrid) {
            var self = this;
            self.returnData = [];
            self.datagrid = datagrid;
            self.isEdit = ko.observable(false);
            var loaiCaArr = [
        { Id: 0, Name: "Không nghỉ giữa giờ" },
        { Id: 1, Name: "Nghỉ giữa giờ" },
            ];
            self.loaiCa = ko.observableArray(loaiCaArr);
            self.visible = ko.observable();
            //self.loaiCaSelected = ko.observable();
            //self.loaiCaSelected.subscribe(function (newValue) {
            //    if (newValue == 0) {
            //        self.visible(false);
            //    }
            //    else {
            //        self.visible(true);
            //    }
            //});
            //self.loaiCaSelected(loaiCaArr[1].Id);
            self.source =
          {
              datatype: "json",
              datafields: [
                { name: 'Oid', type: 'string' },
                { name: 'TenCa', type: 'string' },
                { name: 'LoaiCa', type: 'string' },
                { name: 'ThoiGianVaoSang', type: 'string' },
                { name: 'ThoiGianRaSang', type: 'string' },
                { name: 'ThoiGianBatDauNghiGiuaCa', type: 'string' },
                { name: 'ThoiGianKetThucNghiGiuaCa', type: 'string' },
                { name: 'ThoiGianVaoChieu', type: 'string' },
                { name: 'ThoiGianRaChieu', type: 'string' },
                { name: 'ThoiGianBDQuetBuoiSang', type: 'string' },
                { name: 'ThoiGianKTQuetBuoiChieu', type: 'string' },
                { name: 'SoPhutCong', type: 'string' },
                { name: 'SoPhutTru', type: 'string' },
                { name: 'TongSoGioNghiGiuaCa', type: 'string' },
                { name: 'TongSoGioLamViecBuoiSang', type: 'string' },
                { name: 'TongSoGioLamViecBuoiChieu', type: 'string' },
                { name: 'TongSoGioLamViecCaNgay', type: 'string' },
                { name: 'ThoiGianRaThu7', type: 'string' }
              ],
              id: 'Id',
              url: "/Services/ChamCongService.asmx/GetList_CaChamCong",
              beforeprocessing: function (result) {
                  self.returnData = $.parseJSON(result.d);
                  return self.returnData;
              }
          };
            self.checkDangSuDung = function (Oid) {
                var check;

                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/CaChamCong_CheckDangSuDung',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        Oid: Oid
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            }
            var cellsrenderer = function (row, column, value) {
                return '<div style="text-align: center; margin-top: 15px;">' + value + '</div>';
            }
            var leftcellsrenderer = function (row, column, value) {
                return '<div style="text-align: left; margin-top: 15px;margin-left:5px">' + value + '</div>';
            }
            self.dataAdapter = new $.jqx.dataAdapter(self.source, { contentType: 'application/json; charset=utf-8' });
            self.datagrid.jqxGrid(
                {
                    source: self.dataAdapter,
                    width: '100%',
                    selectionmode: 'checkbox',
                    pageable: true,
                    pagesize: 10,
                    sortable: true,
                    rowsheight: 50,
                    filterable: true,
                    autorowheight: true,
                    autoheight: true,
                    theme: "darkBlue",
                    columns: [
                        {
                            text: 'STT', columntype: 'number', width: 35, editable: false, cellsrenderer: function (row, column, value) {
                                return "<div style='text-align:center;margin-top:15px;'>" + (value + 1) + "</div>";
                            }
                        },
                        {
                            text: 'Tên khung giờ', datafield: 'TenCa', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: leftcellsrenderer
                        },
                        {
                            text: 'Loại khung', datafield: 'LoaiCa', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: leftcellsrenderer
                        },
                        {
                            text: 'Vào sáng', datafield: 'ThoiGianVaoSang', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        },
                        //{
                        //    text: 'Ra sáng', datafield: 'ThoiGianRaSang', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        //},
                        {
                            text: 'Bắt đầu nghỉ', datafield: 'ThoiGianBatDauNghiGiuaCa', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        },
                        {
                            text: 'Kết thúc nghỉ', datafield: 'ThoiGianKetThucNghiGiuaCa', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        },
                        //{
                        //    text: 'Vào chiều', datafield: 'ThoiGianVaoChieu', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        //},
                        {
                            text: 'Ra chiều', datafield: 'ThoiGianRaChieu', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        },
                        //{
                        //    text: 'Bắt đầu quét', datafield: 'ThoiGianBDQuetBuoiSang', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        //},
                        //{
                        //    text: 'Kết thúc quét', datafield: 'ThoiGianKTQuetBuoiChieu', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        //},
                        {
                            text: 'Số phút cộng', datafield: 'SoPhutCong', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        },
                        {
                            text: 'Số phút trừ', datafield: 'SoPhutTru', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        },
                        //{
                        //    text: 'Số giờ nghỉ', datafield: 'TongSoGioNghiGiuaCa', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        //},
                        //{
                        //    text: 'Số giờ ca sáng', datafield: 'TongSoGioLamViecBuoiSang', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        //},
                        //{
                        //    text: 'Số giờ ca chiều', datafield: 'TongSoGioLamViecBuoiChieu', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        //},
                        //{
                        //    text: 'Cả ngày', datafield: 'TongSoGioLamViecCaNgay', width: 120, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        //}
                        {
                            text: 'Thời gian ra thứ 7', datafield: 'ThoiGianRaThu7', width: 130, align: 'center', cellsalign: "middle", cellsrenderer: cellsrenderer
                        },
                    ]
                });

        }

        ViewModel.prototype = {
            search: function () {
                var self = this;
                if (self.validate())
                    return;
                self.datagrid.jqxGrid('updatebounddata');
            },
            New: function () {
                var self = this;
                $("#Cancel").jqxButton({ theme: "darkBlue" });
                $("#Save").jqxButton({ theme: "darkBlue" });
                var offset = $("#jqxgrid").offset();
                $("#popupWindowCCCNew").jqxWindow({ position: { x: parseInt(offset.left) + 120, y: parseInt(offset.top) - 120 } });
                $("#popupWindowCCCNew").jqxWindow('open');
                ko.applyBindings(self, $("#popupWindowCCCNew")[0]);
            },
            Edit: function () {
                var self = this;
                var getselectedrowindexes = $('#jqxgrid').jqxGrid('getselectedrowindexes');
                var selectedrow = $('#jqxgrid').jqxGrid('getrowdata', getselectedrowindexes[0]);
                if (getselectedrowindexes.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                if (getselectedrowindexes.length > 1) {
                    alert("Chỉ được chọn 1 dòng !!");
                    return;
                }
                $("#Cancel1").jqxButton({ theme: "darkBlue" });
                $("#Save1").jqxButton({ theme: "darkBlue" });
                var offset = $("#jqxgrid").offset();
                $("#popupWindowCCCEdit").jqxWindow({ position: { x: parseInt(offset.left) + 120, y: parseInt(offset.top) - 120 } });
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/CaChamCong_GetByID',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({ id: selectedrow.Oid }),
                    async: false,
                    success: function (result) {
                        var obj = $.parseJSON(result.d);
                        $("#tbTenCa1").val(obj.TenCa);
                        //self.loaiCaSelected(obj.LoaiCa);

                        $("#GioVaoSang1").val(obj.GioVaoSang);
                        $("#PhutVaoSang1").val(obj.PhutVaoSang);

                        //$("#GioRaSang1").val(obj.GioRaSang);
                        //$("#PhutRaSang1").val(obj.PhutRaSang);

                        $("#GioBatDauNghi1").val(obj.GioBatDauNghi);
                        $("#PhutBatDauNghi1").val(obj.PhutBatDauNghi);

                        $("#GioKetThucNghi1").val(obj.GioKetThucNghi);
                        $("#PhutKetThucNghi1").val(obj.PhutKetThucNghi);

                        //$("#GioVaoChieu1").val(obj.GioVaoChieu);
                        //$("#PhutVaoChieu1").val(obj.PhutVaoChieu);

                        $("#GioRaChieu1").val(obj.GioRaChieu);
                        $("#PhutRaChieu1").val(obj.PhutRaChieu);

                        $("#GioBatDauQuet1").val(obj.GioBatDauQuet);
                        $("#PhutBatDauQuet1").val(obj.PhutBatDauQuet);

                        $("#GioKetThucQuet1").val(obj.GioKetThucQuet);
                        $("#PhutKetThucQuet1").val(obj.PhutKetThucQuet);

                        $("#SoPhutCong1").val(obj.SoPhutCong);
                        $("#SoPhutTru1").val(obj.SoPhutTru);

                        $("#GioRaThu71").val(obj.GioRaThu7);
                        $("#PhutRaThu71").val(obj.PhutRaThu7);
                    }
                });
                $("#popupWindowCCCEdit").jqxWindow('open');
                ko.applyBindings(self, $("#popupWindowCCCEdit")[0]);
            },
            Update: function () {
                var self = this;
                var getselectedrow_edit = $('#jqxgrid').jqxGrid('getselectedrowindexes');
                var selectedrow = $('#jqxgrid').jqxGrid('getrowdata', getselectedrow_edit[0]);

                //if (self.checkDangSuDung(selectedrow.Oid)) {
                //    alert('Ca chấm công đang được sử dụng !!');
                //    return;
                //} else {
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/CaChamCong_Save',
                        contentType: "application/json; charset=utf-8",
                        data: ko.toJSON({
                            Oid: selectedrow.Oid,
                            TenCa: $("#tbTenCa1").val(),
                            LoaiCa: 1,
                            GioVaoSang: $("#GioVaoSang1").val(),
                            PhutVaoSang: $("#PhutVaoSang1").val(),

                            //GioRaSang: $("#GioRaSang").val(),
                            //PhutRaSang: $("#PhutRaSang").val(),

                            GioBatDauNghi: $("#GioBatDauNghi1").val(),
                            PhutBatDauNghi: $("#PhutBatDauNghi1").val(),

                            GioKetThucNghi: $("#GioKetThucNghi1").val(),
                            PhutKetThucNghi: $("#PhutKetThucNghi1").val(),

                            //GioVaoChieu: $("#GioVaoChieu").val(),
                            //PhutVaoChieu: $("#PhutVaoChieu").val(),

                            GioRaChieu: $("#GioRaChieu1").val(),
                            PhutRaChieu: $("#PhutRaChieu1").val(),

                            GioBatDauQuet: 0,
                            PhutBatDauQuet: 0,

                            GioKetThucQuet:0,
                            PhutKetThucQuet:0,

                            SoPhutCong: $("#SoPhutCong1").val(),
                            SoPhutTru: $("#SoPhutTru1").val(),

                            GioRaThu7: $("#GioRaThu71").val(),
                            PhutRaThu7: $("#PhutRaThu71").val(),
                        }),
                        dataType: "json",
                        async: false,
                        success: function (result) {
                            alert("Lưu thành công !!");
                            $("#jqxgrid").jqxGrid('updatebounddata');
                            $("#popupWindowCCCEdit").jqxWindow('hide');
                        }
                    });
                //}
            },
            Create: function () {
                var self = this;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/CaChamCong_Save',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        Oid: "",
                        TenCa: $("#tbTenCa").val(),
                        LoaiCa: 1,
                        GioVaoSang: $("#GioVaoSang").val(),
                        PhutVaoSang: $("#PhutVaoSang").val(),

                        //GioRaSang: $("#GioRaSang").val(),
                        //PhutRaSang: $("#PhutRaSang").val(),

                        GioBatDauNghi: $("#GioBatDauNghi").val(),
                        PhutBatDauNghi: $("#PhutBatDauNghi").val(),

                        GioKetThucNghi: $("#GioKetThucNghi").val(),
                        PhutKetThucNghi: $("#PhutKetThucNghi").val(),

                        //GioVaoChieu: $("#GioVaoChieu").val(),
                        //PhutVaoChieu: $("#PhutVaoChieu").val(),

                        GioRaChieu: $("#GioRaChieu").val(),
                        PhutRaChieu: $("#PhutRaChieu").val(),

                        GioBatDauQuet: $("#GioBatDauQuet").val(),
                        PhutBatDauQuet: $("#PhutBatDauQuet").val(),

                        GioKetThucQuet: $("#GioKetThucQuet").val(),
                        PhutKetThucQuet: $("#PhutKetThucQuet").val(),

                        SoPhutCong: $("#SoPhutCong").val(),
                        SoPhutTru: $("#SoPhutTru").val(),

                        GioRaThu7: $("#GioRaThu7").val(),
                        PhutRaThu7: $("#PhutRaThu7").val(),
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Lưu thành công !!");
                        $("#jqxgrid").jqxGrid('updatebounddata');
                        $("#popupWindowCCCNew").jqxWindow('hide');
                    }
                });
            },
            remove: function () {
                var self = this;
                var getselectedrowindexes = $('#jqxgrid').jqxGrid('getselectedrowindexes');
                var selectedrow = $('#jqxgrid').jqxGrid('getrowdata', getselectedrowindexes[0]);
                if (getselectedrowindexes.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                if (getselectedrowindexes.length > 1) {
                    alert("Chỉ được chọn 1 dòng !!");
                    return;
                }
                var r = confirm("Bạn có chắc xóa hay không ?");
                if (r == true) {
                    if (self.checkDangSuDung(selectedrow.Oid)) {
                        alert('Ca chấm công đang được sử dụng !!');
                        return;
                    } else {
                        $.ajax({
                            type: 'POST',
                            url: '/Services/ChamCongService.asmx/CaChamCong_Delete',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: ko.toJSON({
                                Oid: selectedrow.Oid,
                            }),
                            success: function (result) {
                                alert("Xóa thành công !!");
                                self.datagrid.jqxGrid('updatebounddata');
                                self.datagrid.jqxGrid('clearselection');
                            }
                        });
                    }
                } else {
                    return;
                }
            },
        };
        $(function () {

            var model = new ViewModel($("#jqxgrid"));

            $("#popupWindowCCCNew").jqxWindow({
                width: 500, theme: "darkBlue", height: 300, resizable: true, isModal: false, autoOpen: false, cancelButton: $("#Cancel")
            });
            $("#popupWindowCCCNew").jqxWindow('hide');
            $("#popupWindowCCCEdit").jqxWindow({
                width: 500, theme: "darkBlue", height: 300, resizable: true, isModal: false, autoOpen: false, cancelButton: $("#Cancel")
            });
            $("#popupWindowCCCEdit").jqxWindow('hide');

            ko.applyBindings(model, $("#cachamcong")[0]);

        });
    </script>
    <style type="text/css">
        .chitiet {
            width: 120px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="cachamcong">

        <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left: 15px;">
                    <div class="row">
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: New">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Tạo mới
                            </a>
                        </div>
                                                <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: Edit">
                                <i class="btn-label fa fa-pencil"></i>Chỉnh sửa
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: remove">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Xóa
                            </a>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
        <div id="popupWindowCCCNew">
            <div>Khung giờ làm việc</div>
            <div style="overflow: hidden;">
                <table>
                    <tr>
                        <td align="right">Tên khung giờ:</td>
                        <td align="left">
                            <input type="text" class="form-control" style="height: 20px" id="tbTenCa" />
                        </td>
                    </tr>

                    <tr>
                        <td align="right">Thời gian vào sáng:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioVaoSang" value="00" />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutVaoSang"   value="00" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Thời gian ra chiều:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioRaChieu"  value="00" />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutRaChieu" value="00"  />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Thời gian bắt đầu nghỉ giữa ca:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioBatDauNghi"  value="00" />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutBatDauNghi" value="00"  />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Thời gian kết thúc nghỉ giữa ca:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioKetThucNghi" value="00"  />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutKetThucNghi" value="00"  />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td align="right">Thời gian bắt đầu quét buổi sáng :</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioBatDauQuet" value="00"  />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutBatDauQuet" value="00"  />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td align="right">Thời gian kết thúc quét buổi chiều:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioKetThucQuet"  value="00" />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutKetThucQuet" value="00"  />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Số phút cộng:</td>
                        <td align="left">
                            <input type="number" style="height: 20px; width: 40px; text-align: center" id="SoPhutCong" value="00"  />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Số phút trừ:</td>
                        <td align="left">
                            <input type="number" style="height: 20px; width: 40px; text-align: center" id="SoPhutTru" value="00"  />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Thời gian ra thứ 7:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioRaThu7" value="00"  />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutRaThu7"  value="00" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td style="padding-top: 10px;" align="right">
                            <input style="margin-right: 5px;" type="button" id="Save" value="Save" data-bind="click: Create" />
                            <input id="Cancel" type="button" value="Cancel" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="popupWindowCCCEdit">
            <div>Khung giờ làm việc</div>
            <div style="overflow: hidden;">
                <table>
                    <tr>
                        <td align="right">Tên khung giờ:</td>
                        <td align="left">
                            <input type="text" class="form-control" style="height: 20px" id="tbTenCa1" />
                        </td>
                    </tr>
                    <%--                    <tr>
                        <td align="right">Loại khung:</td>
                        <td align="left">
                            <select data-bind="options: loaiCa, optionsText: 'Name', optionsValue: 'Id', value: loaiCaSelected"></select>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="right">Thời gian vào sáng:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioVaoSang1"  value="00" />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutVaoSang1"  value="00" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Thời gian ra chiều:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioRaChieu1" value="00"  />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutRaChieu1"  value="00" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Thời gian bắt đầu nghỉ giữa ca:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioBatDauNghi1" value="00"  />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutBatDauNghi1"  value="00" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Thời gian kết thúc nghỉ giữa ca:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioKetThucNghi1" value="00"  />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutKetThucNghi1"  value="00" />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td align="right">Thời gian bắt đầu quét buổi sáng :</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioBatDauQuet1" value="00"  />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutBatDauQuet1" value="00"  />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td align="right">Thời gian kết thúc quét buổi chiều:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioKetThucQuet1" value="00"  />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutKetThucQuet1"  value="00" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Số phút cộng:</td>
                        <td align="left">
                            <input type="number" style="height: 20px; width: 40px; text-align: center" id="SoPhutCong1"  value="00" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Số phút trừ:</td>
                        <td align="left">
                            <input type="number" style="height: 20px; width: 40px; text-align: center" id="SoPhutTru1" value="00"  />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Thời gian ra thứ 7:</td>
                        <td align="left">
                            <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioRaThu71" value="00"  />
                            <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutRaThu71"  value="00" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td style="padding-top: 10px;" align="right">
                            <input style="margin-right: 5px;" type="button" id="Save1" value="Lưu" data-bind="click: Update" />
                            <input id="Cancel1" type="button" value="Đóng" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
