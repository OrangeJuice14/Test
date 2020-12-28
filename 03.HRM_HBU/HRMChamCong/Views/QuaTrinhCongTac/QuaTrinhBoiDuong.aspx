<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="QuaTrinhBoiDuong.aspx.cs" Inherits="HRMChamCong.Views.QuaTrinhCongTac.QuaTrinhBoiDuong" %>

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
                    <th class="textToCenter">Từ ngày</th>
                    <th class="textToCenter">Đến ngày</th>
                    <th>Nội dung bồi dưỡng</th>
                    <th class="textToCenter">Nơi bồi dưỡng</th>
                    <th class="textToCenter">Chứng chỉ được cấp</th>
                </tr>
                <tbody data-bind="foreach: DanhSach_QuaTrinhBoiDuong">
                    <tr>
                        <td data-bind="text: TuNgay" class="textToCenter"></td>
                        <td data-bind="text: DenNgay" class="textToCenter"></td>
                        <td data-bind="text: NoiDungBoiDuong"></td>
                        <td data-bind="text: NoiBoiDuong" class="textToCenter"></td>
                        <td data-bind="text: ChungChiDuocCap" class="textToCenter"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
