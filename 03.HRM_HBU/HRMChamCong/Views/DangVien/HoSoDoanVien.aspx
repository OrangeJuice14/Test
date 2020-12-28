<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="HoSoDoanVien.aspx.cs" Inherits="HRMChamCong.Views.DangVien.HoSoDoanVien" %>

<html>
<head></head>
<body>
    <div>
        <h3 style="font-weight: bold; color: #333">Hồ sơ đoàn viên</h3>
        <div class="hrm_clear">
        </div>
        <div class="hrm_content">
            <div class="hrm_box">
                <div class="hrm_left">
                    Tổ chức Đoàn:
                </div>
                <div class="hrm_right"
                    data-bind="text: ToChucDoan">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Số thẻ Đoàn:
                </div>
                <div class="hrm_right"
                    data-bind="text: SoThe">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Ngày cấp:
                </div>
                <div class="hrm_right"
                    data-bind="text: NgayCap == null ? '' : formatDate(new Date(NgayCap))">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Nơi cấp:
                </div>
                <div class="hrm_right"
                    data-bind="text: NoiKetNap">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Ngày kết nạp:
                </div>
                <div class="hrm_right"
                    data-bind="text: NgayKetNap == null ? '' : formatDate(new Date(NgayKetNap))">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Nơi kết nạp:
                </div>
                <div class="hrm_right"
                    data-bind="text: NoiKetNap">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Chức vụ Đoàn:
                </div>
                <div class="hrm_right"
                    data-bind="text: ChucVu">
                </div>
            </div>
        </div>
    </div>
</body>
</html>
