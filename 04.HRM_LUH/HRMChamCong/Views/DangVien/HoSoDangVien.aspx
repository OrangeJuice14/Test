<%@ Page Title="" Language="C#"  AutoEventWireup="true" CodeBehind="HoSoDangVien.aspx.cs" Inherits="HRMChamCong.Views.DangVien.HoSoDangVien" %>

<html>
<head></head>
<body>
    <div>
        <div class="hrm_clear">
        </div>
        <div class="hrm_content">
            <div class="hrm_box">
                <div class="hrm_left">
                    Đảng bộ:
                </div>
                <div class="hrm_right"
                    data-bind="text: DangBo">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Chi bộ Đảng:
                </div>
                <div class="hrm_right"
                    data-bind="text: ChiBoDang">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Số lý lịch:
                </div>
                <div class="hrm_right"
                    data-bind="text: SoLyLich">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Ngày vào Đảng:
                </div>
                <div class="hrm_right"
                    data-bind="text: NgayVaoDang == null ? '' : formatDate(new Date(NgayVaoDang))">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Ngày vào Đảng chính thức:
                </div>
                <div class="hrm_right"
                    data-bind="text: NgayVaoDangChinhThuc == null ? '' : formatDate(new Date(NgayVaoDangChinhThuc))">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Số thẻ Đảng:
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
                    Chức vụ Đảng:
                </div>
                <div class="hrm_right"
                    data-bind="text: ChucVu">
                </div>
            </div>
        </div>
    </div>
</body>
</html>
