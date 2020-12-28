<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="QuaTrinhHoiThao.aspx.cs" Inherits="HRMChamCong.Views.QuaTrinhCongTac.QuaTrinhHoiThao" %>

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
                    <th style="width: 30%" class="textToCenter">Tên hội thảo</th>
                    <th style="width: 10%" class="textToCenter">Từ ngày</th>
                    <th style="width: 10%" class="textToCenter">Đến ngày</th>
                    <th style="width: 25%" class="textToCenter">Địa điểm</th>
                    <th style="width: 15%" class="textToCenter">Quốc gia</th>
                    <th style="width: 10%" class="textToCenter">Có bài tham luận</th>
                </tr>
                <tbody data-bind="foreach: DanhSach_QuaTrinhThamGiaHoiThao">
                    <tr>
                        <td data-bind="text: TenHoiThao" class="textToCenter"></td>
                        <td data-bind="text: TuNgay == null ? '' : formatDate(new Date(TuNgay))" class="textToCenter"></td>
                        <td data-bind="text: DenNgay == null ? '' : formatDate(new Date(DenNgay))" class="textToCenter"></td>
                        <td data-bind="text: DiaDiem" class="textToCenter"></td>
                        <td data-bind="text: QuocGia" class="textToCenter"></td>
                        <td class="textToCenter">
                            <input type="checkbox" data-bind="checked: CoBaiThamLuan" /></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
