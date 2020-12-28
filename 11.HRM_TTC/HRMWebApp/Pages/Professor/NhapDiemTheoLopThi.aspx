<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NhapDiemTheoLopThi.aspx.cs" Inherits="MyUIS_MVC.Pages.Professor.NhapDiemTheoLopThi" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nhập điểm thi theo lớp thi</title>
    <style>
        .textbox{
    text-align:center;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 900px; margin: auto; overflow: auto">
            <table cellpadding="0" cellspacing="5" border="0" width="100%">
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="Label1" runat="server" Text="NHẬP ĐIỂM THI THEO LỚP THI" CssClass="title" Font-Bold="True" Font-Size="20pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="100px">Năm học:</td>
                    <td>
                        <asp:Label ID="lblYearStudy" runat="server" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblExam" runat="server" Visible="False"></asp:Label>
                    </td>
                    <td width="100px">Học kỳ:</td>
                    <td>
                        <asp:Label ID="lblTermID" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Học phần:</td>
                    <td>
                        <asp:Label ID="lblCurriculumName" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                    <td>Mã học phần:</td>
                    <td>
                        <asp:Label ID="lblCurriculumID" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Ngày thi:</td>
                    <td colspan="3"><asp:Label ID="lblNgayThiPhongThiGioThi" runat="server" Font-Bold="true"></asp:Label></td>
                </tr>
                <tr>
                    <td>Thời gian nhập điểm:</td>
                    <td>
                        <asp:Label ID="lblFromdate" runat="server" Font-Bold="true"></asp:Label>

                    </td>
                    <td></td>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="right" style="text-align: right" class="auto-style2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align:center">
                        <asp:Label ID="lblThongbao" runat="server" Font-Bold="True" Font-Italic="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: right">
                     <asp:Button ID="btnDongform" runat="server" OnClientClick="window.close(); return false;" Text="Đóng"  />
                        <asp:Button ID="btnKhoaDiem" runat="server" OnClientClick="return confirm('Xác nhận khóa điểm ?')" Text="Khóa điểm" OnClick="btnKhoaDiem_Click" />
                        &nbsp;<asp:Button ID="btnLuuDiem" runat="server" Text="Lưu điểm" OnClick="btnLuuDiem_Click" />          
                    </td>
                </tr>
            </table>
            <div>
                <asp:GridView ID="gvDanhSachSinhVien" runat="server" AutoGenerateColumns="False" Width="100%">
                    <AlternatingRowStyle BackColor="#EEEEEE" />
                    <Columns>
                       <asp:TemplateField HeaderText="STT" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>

                        <asp:BoundField DataField="StudentID" HeaderText="Mã sinh viên">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="LastMiddleName" HeaderText="Họ lót" />
                        <asp:BoundField DataField="FirstName" HeaderText="Tên">
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ClassStudentID" ItemStyle-HorizontalAlign="Center" HeaderText="Lớp" >
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Birthday" HeaderText="Ngày sinh">
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Điểm thi">
                            <ItemTemplate>

                                <asp:TextBox ID="txtMark" CssClass="textbox"  Enabled='<%#Eval("Status").ToString()=="1"?false:true %>' Width="50" Text='<%#Eval("Mark") %>' MaxLength="3" runat="server"></asp:TextBox>
                         <asp:TextBox ID="txtGhiChu" CssClass="textbox"  Width="50" Visible="false" MaxLength="3" runat="server"></asp:TextBox>

                                <asp:Label ID="lblStudentID" Visible="false" runat="server" Text='<%#Eval("StudentID") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="Điểm thi TH">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMarkTH" CssClass="textbox"  Enabled='<%#Eval("Status_TH").ToString()=="1"?false:true %>' Width="50" Text='<%#Eval("Mark_TH") %>' MaxLength="3" runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                    </Columns>
                    <HeaderStyle BackColor="#CCCCCC" />
                </asp:GridView>
            </div>
        </div>
    </form>
<script>
    function heartBeat() {
        $.get("/KeepAlive.ashx", function (data) { });
    }

    $(function () {
        setInterval("heartBeat()", 10000 * 10); // 10p gửi request một lần
    });
    ///////////////////////////////////////////
    function stopRKey(evt) {
        var evt = (evt) ? evt : ((event) ? event : null);
        var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
        if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
    }

    document.onkeypress = stopRKey;

    ///////////////////////////////////////
    function clickEnter(cuobj, obj, event) {
        var keyCode;
        if (event.keyCode > 0) {
            keyCode = event.keyCode;
        }
        else if (event.which > 0) {
            keyCode = event.which;
        }
        else {
            keycode = event.charCode;
        }
        if (keyCode == 13 || keyCode == 40 || keyCode == 9) {
            var valueee = document.getElementById(cuobj).value;
            if (isNaN(valueee) == false && (valueee > 10 || valueee < 0)) {
                alert('Điểm không hợp lệ');
                document.getElementById(cuobj).style.border = "thin solid red";
                document.getElementById(cuobj).focus();
            }
            else if (isNaN(valueee) == true && valueee.toUpperCase() != "VT") {
                alert('Điểm không hợp lệ');
                document.getElementById(cuobj).style.border = "thin solid red";
                document.getElementById(cuobj).focus();
            }
            else {
                document.getElementById(obj).focus();
                document.getElementById(cuobj).style.border = "thin solid blue";;
            }
            return false;
        }
        else {

            return true;
        }
    }
    // 
    function Onblur(cuobj, obj) {
        var valueee = document.getElementById(cuobj).value;
        if (isNaN(valueee) == true || valueee > 10 || valueee < 0) {
            alert('Điểm không hợp lệ');
            document.getElementById(cuobj).style.border = "thin solid red";
            document.getElementById(cuobj).focus();
            return false;

        }
        else { }
    }
</script>
</body>
</html>
