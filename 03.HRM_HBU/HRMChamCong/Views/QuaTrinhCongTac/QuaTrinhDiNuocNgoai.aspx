<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="QuaTrinhDiNuocNgoai.aspx.cs" Inherits="HRMChamCong.Views.QuaTrinhCongTac.QuaTrinhDiNuocNgoai" %>

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
                    <th style="width: 15%;" class="textToCenter">Từ ngày</th>
                    <th style="width: 15%;" class="textToCenter">Đến ngày</th>
                    <th style="width: 15%;" class="textToCenter">Quốc gia</th>
                    <th style="width: 55%;" class="textToCenter">Lý do</th>
                </tr>
                <tbody data-bind="foreach: DanhSach_QuaTrinhDiNuocNgoai">
                    <tr>
                        <td data-bind="text: TuNgay == null ? '' : formatDate(new Date(TuNgay))" class="textToCenter"></td>
                        <td data-bind="text: DenNgay == null ? '' : formatDate(new Date(DenNgay))" class="textToCenter"></td>
                        <td data-bind="text: QuocGia" class="textToCenter"></td>
                        <td data-bind="text: LyDo" class="textToCenter"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
