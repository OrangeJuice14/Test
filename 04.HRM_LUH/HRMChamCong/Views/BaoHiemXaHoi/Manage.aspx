<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="HRMChamCong.Views.BaoHiemXaHoi.Manage" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/CSS/hrmmain.css" rel="stylesheet" />
    <script type="text/javascript">
        function formatDate(date) {
            return date.getDate() + "/" + parseInt(date.getMonth() + 1) + "/" + date.getFullYear();
        }
        function ViewModel() {
            var self = this;
            self.DenNgay = "";
            self.NgayThamGiaBHXH = "";
            self.NoiDangKyKCB = "";
            self.QuyenLoiHuongBHYT = "";
            self.SoSoBHXH = "";
            self.SoTheBHYT = "";
            self.ThamGiaBHTN = "";
            self.TuNgay = "";
            self.DanhSach_QuaTrinhThamGiaBHXH = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_BAOHIEMXAHOI',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function(result) {
                    var data = $.parseJSON(result.d);
                    if (data.HoSoBaoHiem != null)
                        {
                        self.DenNgay = data.HoSoBaoHiem.DenNgay;
                    self.NgayThamGiaBHXH = data.HoSoBaoHiem.NgayThamGiaBHXH;
                    self.NoiDangKyKCB = data.HoSoBaoHiem.NoiDangKyKCB;
                    self.QuyenLoiHuongBHYT = data.HoSoBaoHiem.QuyenLoiHuongBHYT;
                    self.SoSoBHXH = data.HoSoBaoHiem.SoSoBHXH;
                    self.SoTheBHYT = data.HoSoBaoHiem.SoTheBHYT;
                    self.ThamGiaBHTN = data.HoSoBaoHiem.ThamGiaBHTN;
                    self.TuNgay = data.HoSoBaoHiem.TuNgay;
                }
                self.DanhSach_QuaTrinhThamGiaBHXH=data.DanhSach_QuaTrinhThamGiaBHXH;
                }
            });
        }
        $(function () {
            var model = new ViewModel();
            ko.applyBindings(model, $("#BaoHiemXaHoi")[0]);
        });
    </script>
    <style type="text/css">
        .boldText {
            font-weight: bold;
        }

        .textToCenter {
            text-align: center;
        }

        .textToRight {
            text-align: right;
        }

        .backGroundTitle {
            background: #DCDCDC;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="BaoHiemXaHoi">
        <h3 style="font-weight: bold;color: #333">Bảo hiểm xã hội</h3>
        <div class="hrm_clear">
        </div>
        <div class="hrm_content">
            <div class="hrm_box">
                <div class="hrm_left">
                    Số sổ BHXH:
                </div>
                <div class="hrm_right"
                    data-bind="text: SoSoBHXH">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Ngày tham gia BHXH:
                </div>
                <div class="hrm_right"
                    data-bind="text: NgayThamGiaBHXH == null ? '' :formatDate(new Date(NgayThamGiaBHXH))">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Số thẻ BHYT:
                </div>
                <div class="hrm_right"
                    data-bind="text: SoTheBHYT">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Từ ngày:
                </div>
                <div class="hrm_right"
                    data-bind="text:TuNgay == null? '' : formatDate(new Date(TuNgay))">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Đến ngày:
                </div>
                <div class="hrm_right"
                    data-bind="text:DenNgay == null?'': formatDate(new Date(DenNgay))">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Nơi đăng ký KCB:
                </div>
                <div class="hrm_right"
                    data-bind="text: NoiDangKyKCB">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Quyền lợi hưởng BHYT:
                </div>
                <div class="hrm_right"
                    data-bind="text: QuyenLoiHuongBHYT">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Tham gia BHTN:
                </div>
                <div class="hrm_right"
                    data-bind="text: ThamGiaBHTN">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_subtitle">
                    Quá trình tham gia BHXH
                </div>
                <div class="hrm_line">
                </div>
                <div>
                    <table width="100%" border="1" style="border-collapse: collapse;width: 900px !important" >
                        <tr class="backGroundTitle" style="height: 25px;">
                            <th>Từ năm</th>
                            <th>Đến năm</th>
                            <th>Nơi làm việc</th>
                            <th>Hệ số lương</th>
                            <th>HSPC Chức vụ</th>
                            <th>HSPC Vượt khung</th>
                            <th>HSPC Thâm niên</th>
                        </tr>
                        <tbody data-bind="foreach: DanhSach_QuaTrinhThamGiaBHXH" >
                            <tr>
                                <td data-bind="text: TuNam" class="textToCenter"></td>
                                <td data-bind="text: DenNam" class="textToCenter"></td>
                                <td data-bind="text: NoiLamViec" class="textToCenter"></td>
                                <td data-bind="text: HeSoLuong" class="textToCenter"></td>
                                <td data-bind="text: HSPCChucVu"></td>
                                <td data-bind="text: VuotKhung"></td>
                                <td data-bind="text: ThamNien"></td>
                            </tr>
                        </tbody>
                    </table>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
