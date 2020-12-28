<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CongTacCaNhan.aspx.cs" Inherits="HRMChamCong.Views.QuanLyCongTac.CongTacCaNhan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
         function ViewModel(datagrid) {
             var trangthaiArr = [
                 { Id: -1, Name: "Chờ xét" },
                 { Id: 0, Name: "Không chấp nhận" },
                 { Id: 1, Name: "Chấp nhận" }
             ];
             var self = this;
             self.returnData = [];
             self.datagrid = datagrid;
             self.month = ko.observable(new Date().getMonth() + 1);
             self.year = ko.observable(new Date().getFullYear());
             self.trangthaiList = ko.observableArray(trangthaiArr);
             self.trangthaiSelected = ko.observable(-1);         
            self.source =
            {
                datatype: "json",
                datafields: [
                   { name: 'Oid', type: 'string' },
                   { name: 'TuNgay', type: 'date' },
                   { name: 'DenNgay', type: 'date' },
                   { name: 'NoiDung', type: 'int' },
                   { name: 'NgayTao', type: 'date' },
                   { name: 'TrangThai', type: 'int' }
                ],
                id: 'Id',
                //async:false,
                //pagesize: 10,
                sortcolumn: 'Name',
                sortdirection: 'asc',
                url: "/Services/ChamCongService.asmx/CaNhanKhaiBaoCongTac_Find",
                //sort: function (value, row) {
                //    self.datagrid.jqxGrid('updatebounddata');
                //},
                formatdata: function (data) {
                    return {
                        thang: self.month(),
                        nam: self.year(),                        
                        webUserId: "'" + '<%#HttpContext.Current.Session[HRMChamCong.Helper.SessionKey.UserId.ToString()]%>' + "'"
                        //startIndex: (data.pagenum || 0) * (data.pagesize || 10),
                        //pageSize: data.pagesize,
                        //sortorder: "'" + data.sortorder + "'",
                        //sortdatafield: "'" + data.sortdatafield + "'"
                    };
                },
                beforeprocessing: function (result) {
                    self.returnData = $.parseJSON(result.d);
                    return self.returnData;
                }
            };
            self.dataAdapter = new $.jqx.dataAdapter(self.source, { contentType: 'application/json; charset=utf-8' });
            self.datagrid.jqxGrid(
            {
                source: self.dataAdapter,
                width: '100%',
                //editable: true,
                selectionmode: 'checkbox',
                //virtualmode: true,
                pageable: true,
                pagesize: 10,
                sortable: true,
                filterable: true,
                //rowsheight: 100,
                autoheight: true,
                autorowheight: true,
                theme: "darkBlue",
                //rendergridrows: function (args) {
                //    return args.data;
                //},
                columns: [
                  {
                      text: 'STT', columntype: 'number', width: 35, editable: false, cellsrenderer: function (row, column, value) {
                          return "<div style='text-align:center;margin-top:5px;'>" + (value + 1) + "</div>";
                      }
                  },

                  {
                      text: 'Từ ngày', datafield: 'TuNgay', width: 130, align: 'center', cellsalign: "middle", columnGroup: 'NgayCongTac', cellsformat: 'd/M/yyyy'
                  },
                  {
                      text: 'Đến ngày', datafield: 'DenNgay', width: 130, align: 'center', cellsalign: "middle", columnGroup: 'NgayCongTac', cellsformat: 'd/M/yyyy'
                  },
                  {
                      text: 'Nội dung', datafield: 'NoiDung', width: 415, align: 'center', cellsalign: "middle",
                  },
                  {
                      text: 'Ngày tạo', datafield: 'NgayTao', width: 150, align: 'center', cellsalign: "middle",cellsformat : 'd/M/yyyy'
                  },
                  {
                      text: 'Trạng thái', datafield: 'TrangThai',  align: 'center', cellsalign: "middle",
                      cellsrenderer: function (row, columnfield, cellvalue, defaulthtml, columnproperties) {
                          var str = "";
                          if (cellvalue == -1) {
                              str = "<img src='/Images/InfoSmall.jpg' style='padding:2px 0px 0px 30px;' />";
                          }
                          if (cellvalue == 1) {
                              str = "<img src='/Images/TT_yes.png' style='padding:2px 0px 0px 30px;'/>";
                          }
                          if (cellvalue == 0) {
                              str = "<img src='/Images/TT_no.png' style='padding:2px 0px 0px 30px;'/>";
                          }
                          return str;
                      }
                  }                  
                ],
                columnGroups: [
                   { text: 'Ngày công tác', cellsAlign: 'center', align: 'center', name: 'NgayCongTac' }
                ]
            });

        }

         ViewModel.prototype = {
             validate: function() {
                 var self = this;
                 if (isNaN(self.month()) || self.month() < 0 || self.month() > 12) {
                     alert("Tháng không hợp lệ");
                     return true;
                 } else if (isNaN(self.year()) || self.year() < 0) {
                     alert("Năm không hợp lệ");
                     return true;
                 }
                 return false;
             },
             validateFromDateToDate: function (khaiBaoCongTacOid, tuNgay, denNgay, webUserId) {
                 var check;
                 $.ajax({
                     type: 'POST',
                     url: '/Services/ChamCongService.asmx/CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe',
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,
                     data: ko.toJSON({
                         khaiBaoCongTacOid: khaiBaoCongTacOid,
                         tuNgay: tuNgay,
                         denNgay: denNgay,
                         webUserId: webUserId
                     }),
                     success: function (result) {
                         var obj = $.parseJSON(result.d);
                         check = obj;
                     }
                 });
                 return check;
             },
             search: function() {
                 var self = this;
                 if (self.validate())
                     return;
                 self.datagrid.jqxGrid('updatebounddata');
             },
             add: function() {
                 $("#popupWindow").on('open', function() {
                     $("#txtContent").jqxInput('selectAll');
                 });
                 $("#Cancel").jqxButton({ theme: "darkBlue" });
                 $("#Save").jqxButton({ theme: "darkBlue" });
                 var offset = $("#jqxgrid").offset();
                 $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 120, y: parseInt(offset.top) - 120 } });
                 $("#jqxFromDate").jqxDateTimeInput({ width: '250px', height: '25px' });
                 $("#jqxToDate").jqxDateTimeInput({ width: '250px', height: '25px' });
                 $("#popupWindow").jqxWindow('open');
             },
             remove:function() {
                 var self = this;
                 var rows = self.datagrid.jqxGrid('selectedrowindexes');
                 if (rows.length == 0) {
                     alert("Chưa có dòng nào được chọn !!");
                     return;
                 }
                 var r = confirm("Bạn có chắc xóa hay không ?");
                 if (r == true) {
                     var selectedRecords = new Array();
                     for (var i = 0, l = rows.length; i < l ; i++) {
                         var row = self.datagrid.jqxGrid('getrowdata', rows[i]);
                         selectedRecords.push({
                             Oid: row.Oid
                         });
                     }
                     if (selectedRecords.length > 0) {

                         $.ajax({
                             type: 'POST',
                             url: '/Services/ChamCongService.asmx/CaNhanKhaiBaoCongTac_DeleteList',
                             contentType: "application/json; charset=utf-8",
                             dataType: "json",
                             async: false,
                             data: ko.toJSON({
                                 list: selectedRecords
                             }),
                             success: function (result) {
                                 alert("Xóa thành công !!");
                                 self.datagrid.jqxGrid('updatebounddata');
                                 self.datagrid.jqxGrid('clearselection');
                             }
                         });

                     }
                 } else {
                     return;
                 }
             }
        };
        $(function () {
            
            var model = new ViewModel($("#jqxgrid"));
          
            $("#popupWindow").jqxWindow({
                width: 450, theme: "darkBlue", height: 350, resizable: true, isModal: false, autoOpen: false, cancelButton: $("#Cancel")
            });
            $("#Save").click(function () {
                if (!model.validateFromDateToDate(null, $('#jqxFromDate').jqxDateTimeInput('getDate'), $('#jqxToDate').jqxDateTimeInput('getDate'), '<%#HttpContext.Current.Session[HRMChamCong.Helper.SessionKey.UserId.ToString()]%>')) {
                    alert("Không được trùng hoặc giao ngày với dữ liệu trước !!");
                    return;
                }
                    $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/CaNhanKhaiBaoCongTac_KhaiBaoMoi',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        noidung: $("#txtContent").val(),
                        tungay: $('#jqxFromDate').jqxDateTimeInput('getDate'),
                        denngay: $('#jqxToDate').jqxDateTimeInput('getDate'),
                        webUserId: '<%#HttpContext.Current.Session[HRMChamCong.Helper.SessionKey.UserId.ToString()]%>'
                    }),
                    dataType: "json",
                    async: false,
                    success: function(result) {

                    }
                });
                $("#jqxgrid").jqxGrid('updatebounddata');
                $("#popupWindow").jqxWindow('hide');
            });    
            ko.applyBindings(model, $("#quanlycongtac")[0]);
          
        });
    </script>
    <style type="text/css">
        .chitiet {
            width: 120px !important;
       
        }
    </style>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div id="quanlycongtac">

          <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left:15px;">
                    <div class="row">
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: add">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Tạo mới
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
        <div style="margin: 10px 0px 10px 0px; text-align: center">

            <input type="text" placeholder="tháng" data-bind="value: month" style="width: 50px;height:32px; text-align: center" maxlength="2" />
            -
            <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px;height:32px; text-align: center" maxlength="4" />            
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px;height:32px;" />
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
          <div id="popupWindow">
            <div>Khai báo công tác</div>
            <div style="overflow: hidden;">
                <table >
                    <tr>
                        <td align="right">Nội dung:</td>
                        <td align="left"><textarea id="txtContent" style="width: 300px;height: 180px;"></textarea></td>
                    </tr>
                    <tr>
                        <td align="right">Từ ngày:</td>
                        <td align="left"><div id='jqxFromDate'></div></td>
                    </tr>
                    <tr>
                        <td align="right">Đến ngày:</td>
                        <td align="left"><div id='jqxToDate'></div></td>
                    </tr>
                     <tr>
                        <td align="right"></td>
                        <td style="padding-top: 10px;" align="right">
                            <input style="margin-right: 5px;" type="button" id="Save" value="Save" data-bind="click:save"/>
                            <input id="Cancel" type="button" value="Cancel"/>
                        </td>
                    </tr>
                </table>
            </div>
       </div>
    </div>
</asp:Content>
