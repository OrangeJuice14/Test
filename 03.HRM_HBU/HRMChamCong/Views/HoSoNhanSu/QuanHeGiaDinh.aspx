<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="QuanHeGiaDinh.aspx.cs" Inherits="HRMChamCong.Views.HoSoNhanSu.QuanHeGiaDinh" %>

<html>
<head></head>
<body>
    <div>
        <div class="hrm_clear">
        </div>
        <div class="hrm_content" style="width: 900px !important">
            <table width="100%" border="1" style="border-collapse: collapse">
                <tr class="backGroundTitle" style="height: 40px;">
                    <th style="width: 100px" >Họ tên</th>
                    <th class="textToCenter">Quan hệ</th>
                    <th class="textToCenter">Giới tính</th>
                    <th class="textToCenter">Năm sinh</th>
                    <th class="textToCenter">Quốc tịch</th>
                    <th class="textToCenter">Tình trạng</th>
                    <th class="textToCenter">Dân tộc</th>
                    <th class="textToCenter">Nghề nghiệp</th>
                    <th class="textToCenter">Nơi cư trú</th>
                    <th class="textToCenter">Nước cư trú</th>
                    <th class="textToCenter">Tôn giáo</th>
                    <th class="textToCenter">Quê quán</th>
                    <th class="textToCenter">Nơi làm việc</th>
                </tr>
                <tbody data-bind="foreach: DanhSach_QuanHeGiaDinh">
                    <tr>
                        <td data-bind="text: HoTen"></td>
                        <td data-bind="text: QuanHe" class="textToCenter"></td>
                        <td data-bind="text: GioiTinh" class="textToCenter"></td>
                        <td data-bind="text: NamSinh" class="textToCenter"></td>
                        <td data-bind="text: QuocTich" class="textToCenter"></td>
                        <td data-bind="text: TinhTrang" class="textToCenter"></td>
                        <td data-bind="text: DanToc" class="textToCenter"></td>
                        <td data-bind="text: NgheNghiep" class="textToCenter"></td>
                        <td data-bind="text: NoiCuTru" class="textToCenter"></td>
                        <td data-bind="text: NuocCuTru" class="textToCenter"></td>
                        <td data-bind="text: TonGiao" class="textToCenter"></td>
                        <td data-bind="text: QueQuan" class="textToCenter"></td>
                        <td data-bind="text: NoiLamViec" class="textToCenter"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
