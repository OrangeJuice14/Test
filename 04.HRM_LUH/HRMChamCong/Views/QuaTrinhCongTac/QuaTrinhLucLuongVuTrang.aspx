<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="QuaTrinhLucLuongVuTrang.aspx.cs" Inherits="HRMChamCong.Views.QuaTrinhCongTac.QuaTrinhLucLuongVuTrang" %>

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
                    <th style="width: 10%" class="textToCenter">Ngày nhập ngũ</th>
                    <th style="width: 10%" class="textToCenter">Ngày xuất ngũ</th>
                    <th style="width: 20%" class="textToCenter">Quân hàm</th>
                    <th style="width: 60%">Nội dung</th>
                </tr>
                <tbody data-bind="foreach: DanhSach_QuaTrinhThamGiaLucLuongVuTrang">
                    <tr>
                        <td data-bind="text: NgayNhapNgu" class="textToCenter"></td>
                        <td data-bind="text: NgayXuatNgu" class="textToCenter"></td>
                        <td data-bind="text: QuanHam" class="textToCenter"></td>
                        <td data-bind="text: NoiDung"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
