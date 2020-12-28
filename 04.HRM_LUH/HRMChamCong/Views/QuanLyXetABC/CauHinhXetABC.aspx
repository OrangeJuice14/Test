<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CauHinhXetABC.aspx.cs" Inherits="HRMChamCong.Views.QuanLyXetABC.CauHinhXetABC" %>
<%@ Import Namespace="HRMChamCong.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var source;
            var pathname = window.location.pathname;
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/WebMenu_GetURLListBy_WebUserId',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    source = data;
                }
            });
            var check = $.inArray(pathname, source);
            if (check < 0) {
                window.location.href = "../../Default.aspx";
            }
        });
                    </script>
    <style type="text/css">
        .formGroup {
            padding: 10px 0px 0px 0px;
            margin: 0 auto;
        }

            .formGroup label {
                float: left;
                width: 120px;
            }

            .formGroup span {
                padding: 0px 10px;
            }

        .container {
            border: solid 1px #7F9DB9;
            width: 400px;
            height: 500px;
            overflow-y: scroll;
        }

        .form_checkbox {
            padding: 0 5px;
        }

        h3 {
            color: #3B6097;
        }

        .formEvent {
            float: right;
        }

            .formEvent a {
                color: #3B6097;
                width: 50px;
                float: left;
            }

        .validate {
            color: red;
        }
    </style>
    <script type="text/javascript">
        function ViewModel(datagrid) {           
            var self = this;
            var thoigian;
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/CauHinhXetABC_GetThoiGian',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    thoigian = $.parseJSON(result.d);
                }
            });
            $("#ddlday").val(thoigian);
        }
        ViewModel.prototype = {           
            save: function () {
                var self = this;
                var day = $("#ddlday").val();
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/CauHinhXetABC_Update',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({ day: day }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Cập nhật thành công");
                    }
                });
            }
            

        };
        $(function () {
            var model = new ViewModel();
            ko.applyBindings(model, $("#quanlyxetabc")[0]);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="quanlyxetabc">
        
        <div>
            
            <div style="font-family:sans-serif; font-size:24px;color:cadetblue">Cấu hình xét ABC</div>
            <div class="formGroup">
                <span>Thời gian xét ABC đến hết ngày:</span>
                <select id="ddlday" style="width: 110px;">                  
                    <option value="1">Ngày 1</option>
                    <option value="2">Ngày 2</option>
                    <option value="3">Ngày 3</option>
                    <option value="4">Ngày 4</option>
                    <option value="5">Ngày 5</option>
                    <option value="6">Ngày 6</option>
                    <option value="7">Ngày 7</option>
                    <option value="8">Ngày 8</option>
                    <option value="9">Ngày 9</option>
                    <option value="10">Ngày 10</option>
                    <option value="11">Ngày 11</option>
                    <option value="12">Ngày 12</option>
                    <option value="13">Ngày 13</option>
                    <option value="14">Ngày 14</option>
                    <option value="15">Ngày 15</option>
                    <option value="16">Ngày 16</option>
                    <option value="17">Ngày 17</option>
                    <option value="18">Ngày 18</option>
                    <option value="19">Ngày 19</option>
                    <option value="20">Ngày 20</option>
                    <option value="21">Ngày 21</option>
                    <option value="22">Ngày 22</option>
                    <option value="23">Ngày 23</option>
                    <option value="24">Ngày 24</option>
                    <option value="25">Ngày 25</option>
                    <option value="26">Ngày 26</option>
                    <option value="27">Ngày 27</option>
                    <option value="28">Ngày 28</option>
                    <option value="29">Ngày 29</option>
                    <option value="30">Ngày 30</option>
                    <option value="31">Ngày 31</option>
                </select>
            </div>
        </div>

        <div class="formGroup">
            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: save">
                <i class="btn-label glyphicon glyphicon-ok"></i>Lưu
            </a>
            <a href="javascript:history.back()" class="btn btn-labeled btn-blue" style="width: 158px;">
                <i class="btn-label glyphicon glyphicon-chevron-left"></i>Trở về
            </a>

        </div>


    </div>
</asp:Content>
