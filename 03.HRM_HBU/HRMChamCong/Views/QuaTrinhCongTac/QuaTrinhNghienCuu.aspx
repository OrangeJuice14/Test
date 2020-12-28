<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="QuaTrinhNghienCuu.aspx.cs" Inherits="HRMChamCong.Views.QuaTrinhCongTac.QuaTrinhNghienCuu" %>
<%@ Import Namespace="HRMChamCong.Helper" %>
<html>
    <head></head>
    <body>
    <div>
        <div class="hrm_clear">
        </div>
        <div class="hrm_content" style="width: 900px !important">
            <table width="100%" border="1" style="border-collapse: collapse">
                <tr class="backGroundTitle" style="height: 25px;">
                    <th style="width: 30%" class="textToCenter">Tên đề tài</th>
                    <th style="width: 10%" class="textToCenter">Từ năm</th>
                    <th style="width: 10%" class="textToCenter">Đến năm</th>
                    <th style="width: 20%" class="textToCenter">Cơ quan chủ quản</th>
                    <th style="width: 20%" class="textToCenter">Chức danh tham gia</th>
                    <th style="width: 10%" class="textToCenter">Ngày nghiệm thu</th>
                </tr>
                <tbody data-bind="foreach: DanhSach_QuaTrinhNghienCuu">
                    <tr>
                        <td data-bind="text: TenDeTai" class="textToCenter"></td>
                        <td data-bind="text: TuNam" class="textToCenter"></td>
                        <td data-bind="text: DenNam" class="textToCenter"></td>
                        <td data-bind="text: CoQuanChuTri" class="textToCenter"></td>
                        <td data-bind="text: ChucDanhThamGia" class="textToCenter"></td>
                        <td data-bind="text: NgayNghiemThu == null ? '' : formatDate(new Date(NgayNghiemThu))" class="textToCenter"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    </body>
</html>