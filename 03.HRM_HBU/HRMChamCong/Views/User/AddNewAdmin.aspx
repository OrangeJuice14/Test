<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewAdmin.aspx.cs" Inherits="HRMChamCong.Views.User.AddNewAdmin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<%--    <script src="/Components/jqwidgets/jqx-all.js" type="text/javascript"></script>
    <script src="/Scripts/knockout-3.2.0.js"></script>       
    <script type="text/javascript" src="/Components/jqwidgets/jqxcore.js"></script>
    <script type="text/javascript" src="/Components/jqwidgets/jqxdata.js"></script>
    <script type="text/javascript" src="/Components/jqwidgets/jqxmenu.js"></script>--%>

</head>
<body>
        <div>Tạo mới User</div>
        <div style="overflow: hidden;">
            <table>
                <tr>
                    <td align="right">Phòng ban:</td>
                    <td align="left">
                        <select style="width: 150px" data-bind="options: departments, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected_nhansu, optionsCaption: 'Không chọn'"></select></td>
                </tr>
                <tr data-bind="visible: departmentSelected_nhansu() !== undefined">
                    <td align="right">Nhân viên:</td>
                    <td align="left">
                        <div id="jqxdropdownbutton">
                            <div style="border-color: transparent;" id="jqxgrid_hosonhanvien">
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right">Tên đăng nhập:</td>
                    <td align="left">
                        <input type="text" data-bind="value: userName" id="txtUserName" />
                        <span class="validate" data-bind="visible: $.trim(userName()).length == 0">(*)</span>
                </tr>
                <tr>
                    <td align="right">Mật khẩu:</td>
                    <td align="left">
                        <div id="jqxdropdownbutton">
                            <input type="password" data-bind="value: passWord" />
                            <span class="validate" data-bind="visible: $.trim(passWord()).length == 0">(*)</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right">Trạng Thái:</td>
                    <td align="left">
                        <input type="radio" data-bind="checked: status" value="true" name="rStatus" checked /><span>Hoạt động</span>
                        <input type="radio" data-bind="checked: status" value="false" name="rStatus" /><span>Khóa lại</span>
                </tr>
                <tr>
                    <td align="right">Loại tài khoản:</td>
                    <td align="left">
                        <select data-bind="options: accountList, optionsText: 'Name', optionsValue: 'ID', value: accountSelected"></select>
                    </td>
                </tr>
                <tr>
                    <td align="right">Bộ phận:</td>
                    <td align="left">
                        <div class="container">
                            <div class="form_checkbox">
                                <input type="checkbox" data-bind="click: selectAll, checked: allSelected" />
                                <span>Chọn tất cả</span>
                            </div>
                            <!-- ko foreach: departments -->
                            <!-- ko if:Oid != null -->
                            <div class="form_checkbox">
                                <input type="checkbox" data-bind="checked: Chon" />
                                <span data-bind="text: STT + '. ' + TenBoPhan"></span>
                            </div>
                            <!-- /ko -->
                            <!-- /ko -->
                            <br />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td align="right"></td>
                    <td style="padding-top: 10px;" align="right">
                        <input style="margin-right: 5px;" type="button" id="Save" value="Save" data-bind="click: save" />
                        <input id="Cancel" type="button" value="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="padding-bottom: 0px;"><span>Lưu ý : <span style="color: red">(*)</span> bắt buộc nhập</span></td>
                </tr>
            </table>
        </div>
</body>
</html>
