<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GiayXacNhanVayVonNH.aspx.cs" Inherits="HRMChamCong.Views.ThongTinNhanSu.GiayXacNhanVayVonNH" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/knockout-3.2.0.js"></script>
    <script type="text/javascript">
        function formatDate(date) {
            return date.getDate() + "/" + parseInt(date.getMonth() + 1) + "/" + date.getFullYear();
        }
        $(function () {
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    ko.applyBindings(data, $("#VayVon")[0]);
                }
            });
        })

    </script>
    <style type="text/css">
        .header {
            width: 100%;
            padding: 5px 0px 0px 0px;
            font-size: 14px;
            float: left;
        }

        .content {
            float: left;
            width: 80%;
            padding: 0 10%;
        }

        .twocolumn {
            float: left;
            width: 50%;
        }

        .boldText {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <div style="margin: 0 auto;" id="VayVon">
        <div class="header">
            <div style="float: left; width: 50%">
                <div style="text-align: center">
                    <div style="font-weight: bold">BỘ CÔNG THƯƠNG</div>
                    <div style="font-weight: bold">TRƯỜNG ĐẠI HỌC CÔNG NGHIỆP TP.HCM</div>
                    <div style="width: 100%; font-weight: bold">------------------</div>
                    <div style="width: 100%; font-weight: bold">Số: <span data-bind="text: SoChungTu"></span></div>
                    <div>
                        <img alt="Trường Đại Học Công Nghiệp TP.HCM" src="/Images/logo.png" align="middle">
                    </div>
                </div>
            </div>
            <div style="float: right; width: 50%">
                <div style="text-align: center">
                    <div style="font-weight: bold">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</div>
                    <div style="font-weight: bold">Độc Lập - Tự Do - Hạnh Phúc</div>
                    <div style="width: 100%; font-weight: bold">-----ooOoo-----</div>
                </div>
            </div>
        </div>
        <div class="content">
            <h2 style="text-align: center; font-weight: bold;">GIẤY XÁC NHẬN</h2>
            <div style="float: left; width: 100%; padding: 5px 0px 5px 0px">
                <div style="float: left; width: 60%;">
                    <span style="width: 140px; float: left">Anh (chị):</span>
                    <span style="text-transform: uppercase" data-bind="text: HoTen" class="boldText"></span>
                </div>
                <div style="float: left; width: 40%;">
                    <span style="width: 140px; float: left">Chức danh:</span>
                    <span data-bind="text: TenChucDanh" class="boldText"></span>
                </div>
            </div>
            <div style="float: left; width: 100%; padding: 10px 0px 5px 0px">
                <div style="float: left; width: 60%;">
                    <span style="width: 140px; float: left">Ngày sinh:</span>
                    <span data-bind="text: formatDate(new Date(NgaySinh))" class="boldText"></span>
                </div>
                <div style="float: left; width: 40%;">
                    <span style="width: 140px; float: left">Nơi sinh:</span>
                    <span data-bind="text: NoiSinh" class="boldText"></span>
                </div>
            </div>
            <div style="float: left; width: 100%; padding: 10px 0px 5px 0px">
                <div>
                    <span style="width: 140px; float: left">Địa chỉ thường trú:</span>
                    <span data-bind="text: DiaChiThuongTru" class="boldText"></span>
                </div>

            </div>
            <div style="float: left; width: 100%; padding: 10px 0px 5px 0px">
                <div style="float: left; width: 50%;">
                    <span style="width: 100px; float: left">Mã ngạch:</span>
                    <span data-bind="text: NgachLuong" class="boldText"></span>
                </div>
                <div style="float: left; width: 10%;">
                    <span>Bậc:</span>
                    <span data-bind="text: BacLuong" class="boldText"></span>
                </div>
                <div style="float: left; width: 30%;">
                    <span>Hệ số lương:</span>
                    <span data-bind="text: HeSoLuong" class="boldText"></span>
                </div>
            </div>
            <div style="float: left; width: 100%; padding: 10px 0px 5px 0px">
                <div>
                    <span style="float: left; width: 140px;">Bộ phận/Phòng ban:</span>
                    <span data-bind="text: TenBoPhan" class="boldText"></span>
                </div>

            </div>
            <div style="float: left; width: 100%; padding: 10px 0px 5px 0px">
                <div>
                    <span style="float: left; width: 140px;">Thu nhập hàng tháng:</span>
                    <span data-bind="text: TongThuNhap" class="boldText"></span>
                </div>

            </div>
            <div style="width:100%; float:left;padding: 10px 0px 0px 0px">Trường Đại học Công nghiệp thành phố Hồ Chí Minh cấp giấy chứng nhận này cho anh(chị) <span data-bind="text: HoTen" style="text-transform: uppercase" class="boldText"></span> để sử dụng vào việc bổ sung hồ sơ.</div>
            <div style="float:right; font-style: italic;padding: 10px 0px 0px 0px">TP.Hồ Chí Minh <span data-bind="text: 'ngày ' + new Date().getDate() + ' tháng ' + (new Date().getMonth() +1) + ' năm ' + new Date().getFullYear()"></span></div>
        </div>
    </div>
</body>
</html>

