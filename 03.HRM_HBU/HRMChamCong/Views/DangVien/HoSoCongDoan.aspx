<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="HoSoCongDoan.aspx.cs" Inherits="HRMChamCong.Views.DangVien.HoSoCongDoan" %>

<html>
<head></head>
<body>
    <div>
        <div class="hrm_clear">
        </div>
        <div class="hrm_content">
            <div class="hrm_box">
                <div class="hrm_left">
                    Tổ chức Công đoàn:
                </div>
                <div class="hrm_right"
                    data-bind="text: ToChucCongDoan">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Số quyết định:
                </div>
                <div class="hrm_right"
                    data-bind="text: SoQuyetDinh">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Số thẻ:
                </div>
                <div class="hrm_right"
                    data-bind="text: SoThe">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Chức vụ Công đoàn:
                </div>
                <div class="hrm_right"
                    data-bind="text: ChucVu">
                </div>
            </div>
        </div>
    </div>
</body>
</html>
