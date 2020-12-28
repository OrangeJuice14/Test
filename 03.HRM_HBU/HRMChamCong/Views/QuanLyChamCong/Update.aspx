<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.Update" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Components/jqwidgets/jqx.base.css" rel="stylesheet" />
    <link href="/Components/jqwidgets/jqx.darkBlue.css" rel="stylesheet" />
    <script src="/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/knockout-3.2.0.js"></script>
    <script src="/Scripts/jquery.linq.min.js"></script>
    <script type="text/javascript">

        function getDaysInMonth(m, y) {
            var daysArray = [];
            daysInWeek = ['<span style="color:red">CN</span>', 'T2', 'T3', 'T4', 'T5', 'T6', '<span style="color:red">T7</span>'];
            daysIndex = { 'Sun': 0, 'Mon': 1, 'Tue': 2, 'Wed': 3, 'Thu': 4, 'Fri': 5, 'Sat': 6 };
            index = daysIndex[(new Date(y, m - 1, 1)).toString().split(' ')[0]];
            var numDaysInMonth = /8|3|5|10/.test(--m) ? 30 : m == 1 ? (!(y % 4) && y % 100) || !(y % 400) ? 29 : 28 : 31;
            for (i = 0, l = numDaysInMonth  ; i < l  ; i++) {
                daysArray.push((i + 1) + '<br/>' + daysInWeek[index++]);
                if (index == 7) index = 0;
            }
            return daysArray;
        }

        function viewModel(item) {
            var self = this;
            self.items = ko.observableArray(item);
            console.log(self.items());
            self.days = ko.observableArray([]);
            self.CheckChot = ko.observable();
            self.CheckChotDonVi = ko.observable();
            self.thang = ko.observable();
            self.nam = ko.observable();
            self.tuNgay = ko.observable();
            self.denNgay = ko.observable();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/KyChamCong_FindByDate',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    ngay: '<%#Request.QueryString["Ngay"] %>',
                    thang: '<%#Request.QueryString["Thang"] %>',
                    nam: '<%#Request.QueryString["Nam"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    data = $.parseJSON(result.d);
                    self.thang(data.Thang);
                    self.nam(data.Nam);
                    self.tuNgay(data.TuNgayString);
                    self.denNgay(data.DenNgayString);
                }
            });
            self.WebGroupID = ko.observable();
            self.WebGroup = ko.observable(0);
            self.WebGroup('<%#HttpContext.Current.Session[SessionKey.WebGroupId.ToString()]%>');
            switch (self.WebGroup()) {
                case "05a1bf24-bd1c-455f-96f6-7c4237f4659e":
                    self.WebGroup(1);
                    break;
                case "9290b6f5-a08f-4d5e-9e73-a20cff4cb825":
                    self.WebGroup(2);
                    break;
                case "00000000-0000-0000-0000-000000000001":
                    self.WebGroup(3);
                    break;
                case "00000000-0000-0000-0000-000000000002":
                    self.WebGroup(2);
                    break;
            }
            self.dayInMonth = getDaysInMonth(self.thang(), self.nam());
            self.HinhThucNghiList = ko.observableArray([]);
            self.HinhThucNghiListForUpdate = ko.observableArray([]);
            self.TenPhongBan = ko.observable();
            self.daylength = ko.observable();
            self.spanlength = ko.observable();
            self.checkExits = function () {
                var check;

                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChotChamCongThang_CheckExists',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        boPhanId: '<%#Request.QueryString["PhongBan"] %>',
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            };
            self.checkExitsDept = function () {
                var check;

                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChotChamCongThangDonVi_CheckExists',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        boPhanId: '<%#Request.QueryString["PhongBan"] %>',
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            };
            self.checkKyChamCong = function () {
                var check;

                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KyChamCong_Check',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            };
            self.checkKyTinhLuong = function () {
                var check;

                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KyTinhLuong_Check',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            };
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetPhongBan_ById',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON({ id: '<%#Request.QueryString["PhongBan"] %>' }),
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.TenPhongBan(obj.TenBoPhan);
                    self.CheckChot = result; (obj.TenBoPhan);
                    self.CheckChotDonVi = result; (obj.TenBoPhan);
                }
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_GetListHinhThucNghiKyHieu',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.HinhThucNghiList(obj);
                }
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_CheckChot',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    thang: self.thang(),
                    nam: self.nam(),
                    boPhanId: '<%#Request.QueryString["PhongBan"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    self.CheckChot = result.d;
                }
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_CheckChotDonVi',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    thang: self.thang(),
                    nam: self.nam(),
                    boPhanId: '<%#Request.QueryString["PhongBan"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    self.CheckChotDonVi = result.d;
                }
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_GetListHinhThucNghiForUpdate',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.HinhThucNghiListForUpdate(obj);
                }
            });
            self.save = function (val) {
                var self = this;
                self.CheckChotDonVi = ko.observable();
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCong_CheckChotDonVi',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        boPhanId: '<%#Request.QueryString["PhongBan"] %>'
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        self.CheckChotDonVi(result.d);
                    }
                });
                self.WebGroupID = ko.observable();
                self.WebGroup = ko.observable(0);
                self.WebGroup('<%#HttpContext.Current.Session[SessionKey.WebGroupId.ToString()]%>');
                switch (self.WebGroup()) {
                    case "05a1bf24-bd1c-455f-96f6-7c4237f4659e":
                        self.WebGroup(1);
                        break;
                    case "9290b6f5-a08f-4d5e-9e73-a20cff4cb825":
                        self.WebGroup(2);
                        break;
                    case "00000000-0000-0000-0000-000000000001":
                        self.WebGroup(3);
                        break;
                    case "00000000-0000-0000-0000-000000000002":
                        self.WebGroup(2);
                        break;
                }
                var item = new Array();
                $(val.items()).each(function (index, value) {
                    value.ChamCongNgay = new Array();
                    $(value.ChiTietChamCong).each(function (index1, value1) {
                        if (value1.OldValue != value1.MaHinhThucNghi)
                            value.ChamCongNgay.push({ CC_ChamCongTheoNgayOid: value1.CC_ChamCongTheoNgayOid, MaHinhThucNghi: value1.MaHinhThucNghi });
                    });
                    item.push({ ChiTietChamCong: value.ChamCongNgay });
                });
                var obj = $.Enumerable.From(item).Where(function (x) {
                    return x.ChiTietChamCong.length > 0;
                }).ToArray();
                if (obj.length == 0)
                    return;
                if (self.WebGroup() == 3) {
                    if (self.CheckChotDonVi()) {
                        alert("Tháng này đã chốt chấm công!!");
                    }
                    else {
                        $.ajax({
                            type: 'POST',
                            url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThang_Save',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: ko.toJSON({
                                chamcongthang: obj,
                                idUser: '<%#Request.QueryString["UserId"] %>'
                            }),
                            async: false,
                            success: function (result) {
                                alert("Lưu thành công !!");
                                location.reload();
                            },
                            error: function (result) {
                                alert("Lỗi !!");
                                location.reload();
                            },
                            failure: function (result) {
                                alert("Lỗi !!");
                                location.reload();
                            }

                        });
                    }

                }
                else {
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThang_Save',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: ko.toJSON({
                            chamcongthang: obj,
                            idUser: '<%#Request.QueryString["UserId"] %>'
                        }),
                        async: false,
                        success: function (result) {
                            alert("Lưu thành công !!");
                            location.reload();
                        },
                        error: function (result) {
                            alert("Lỗi !!");
                            location.reload();
                        },
                        failure: function (result) {
                            alert("Lỗi !!");
                            location.reload();
                        }

                    });
                }

            };
            self.chot = function (val) {
                var self = this;
                if (self.checkExits()) {
                    alert('Tháng này đã chốt chấm công rồi !!');
                    return;
                }
                if (self.checkKyChamCong() == false) {
                    alert('Chưa có kỳ chấm công !!');
                    return;
                }
                if (self.checkKyTinhLuong() == false) {
                    alert('Chưa có kỳ tính lương !!');
                    return;
                }
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChotChamCongThang_Create',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        boPhanId: '<%#Request.QueryString["PhongBan"] %>',
                        userId: '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>'
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Chốt chấm công thành công !!");
                        location.reload();
                    }
                });
            };
            self.huychot = function (val) {
                var self = this;
                if (self.checkExits()) {
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/ChotChamCongThang_Delete',
                        contentType: "application/json; charset=utf-8",
                        data: ko.toJSON({
                            thang: self.thang(),
                            nam: self.nam(),
                            boPhanId: '<%#Request.QueryString["PhongBan"] %>',
                        }),
                        dataType: "json",
                        async: false,
                        success: function (result) {
                            alert("Hủy chốt chấm công thành công !!");
                            location.reload();
                        }
                    });
                } else {
                    alert("Chưa có dữ liệu để hủy chấm công !!");
                    return;
                }
            };
        }
        self.chotdonvi = function (val) {
            var self = this;
            if (self.checkExitsDept()) {
                alert('Tháng này đã chốt chấm công rồi !!');
                return;
            }
            if (self.checkKyChamCong() == false) {
                alert('Chưa có kỳ chấm công!!');
                return;
            }
            if (self.checkKyTinhLuong() == false) {
                alert('Chưa có kỳ tính lương!!');
                return;
            }
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ChotChamCongThangDonVi_Create',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    thang: self.thang(),
                    nam: self.nam(),
                    boPhanId: '<%#Request.QueryString["PhongBan"] %>',
                    userId: '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    alert("Chốt chấm công thành công !!");
                    location.reload();
                }
            });
        };
        self.huychotdonvi = function (val) {
            var self = this;
            if (self.checkExits()) {
                alert('Không thể hủy chốt vì Quản trị đã chốt chấm công !!');
                return;
            }
            if (self.checkExitsDept()) {
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChotChamCongThangDonVi_Delete',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        boPhanId: '<%#Request.QueryString["PhongBan"] %>',
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Hủy chốt chấm công thành công !!");
                        location.reload();
                    }
                });
            } else {
                alert("Chưa có dữ liệu để hủy chấm công !!");
                return;
            }
        };
        $(function () {
            var self = this;
            self.thang = ko.observable();
            self.nam = ko.observable();
            self.WebGroupID = ko.observable();
            self.WebGroup = ko.observable(0);
            self.WebGroup('<%#HttpContext.Current.Session[SessionKey.WebGroupId.ToString()]%>');
            switch (self.WebGroup()) {
                case "05a1bf24-bd1c-455f-96f6-7c4237f4659e":
                    self.WebGroup(1);
                    break;
                case "9290b6f5-a08f-4d5e-9e73-a20cff4cb825":
                    self.WebGroup(2);
                    break;
                case "00000000-0000-0000-0000-000000000001":
                    self.WebGroup(3);
                    break;
                case "00000000-0000-0000-0000-000000000002":
                    self.WebGroup(2);
                    break;
            }
            self.checkExitsDept = function () {
                var check;

                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChotChamCongThangDonVi_CheckExists',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        boPhanId: '<%#Request.QueryString["PhongBan"] %>',
                        }),
                        dataType: "json",
                        async: false,
                        success: function (result) {
                            check = result.d;
                        }
                    });
                    return check;
                };
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/KyChamCong_FindByDate',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    ngay: '<%#Request.QueryString["Ngay"] %>',
                        thang: '<%#Request.QueryString["Thang"] %>',
                        nam: '<%#Request.QueryString["Nam"] %>'
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        data = $.parseJSON(result.d);
                        self.thang(data.Thang);
                        self.nam(data.Nam);

                    }
                });
           <%-- if (self.WebGroup() == 2) {
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThangDonVi',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        bophanId: '<%#Request.QueryString["PhongBan"] %>',
                    maNhanSu: '<%#Request.QueryString["Value"] %>',
                    idLoaiNhanSu: '<%#Request.QueryString["IdLoaiNhanSu"] %>'
                }),
                                dataType: "json",
                                async: false,
                                success: function (result) {
                                    data = $.parseJSON(result.d);
                                },
                                error: function () {
                                    alert("Chưa có dữ liệu chấm công!");
                                    window.close();
                                }
                            });
        }--%>
            //else {
            var days = [];
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetList_NgayTrongKyChamCong',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    thang: self.thang(),
                    nam: self.nam(),
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    data = $.parseJSON(result.d);
                    days = data;
                }
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThang',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    thang: self.thang(),
                    nam: self.nam(),
                    bophanId: '<%#Request.QueryString["PhongBan"] %>',
                        maNhanSu: '<%#Request.QueryString["Value"] %>',
                        idLoaiNhanSu: '<%#Request.QueryString["IdLoaiNhanSu"] %>'
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        data = $.parseJSON(result.d);
                        for (var i = 0, l = data.length; i < l; i++) {
                            for (var j = 0, ll = days.length; j < ll; j++) {
                                if (data[i].ChiTietChamCong.findIndex(q => q.Ngay == days[j].NgayChamCong) === -1) {
                                    data[i].ChiTietChamCong.push({
                                        MaHinhThucNghi: '',
                                        KyHieu: '',
                                        Ngay: days[j].NgayChamCong
                                    });
                                }
                            }
                            data[i].ChiTietChamCong.sort(function (a, b) { return (a.Ngay > b.Ngay) ? 1 : ((b.Ngay > a.Ngay) ? -1 : 0); });
                        }
                    },
                    error: function () {
                        alert("Chưa có dữ liệu chấm công!");
                        window.close();
                    }
                });
            //}

            var view = new viewModel(data);
            view.days(days);
            view.daylength(days.length);
            view.spanlength(days.length + 8);
            ko.bindingHandlers.rename = {
                update: function (element, valueAccessor, AllBindings, data) {
                    data["OldValue"] = valueAccessor();
                    var value = ko.observable(valueAccessor());
                    var interceptor = ko.computed({
                        read: function () {
                            return value();
                        },
                        write: function (newValue) {
                            var validate = $.Enumerable.From(view.HinhThucNghiList()).Count(function (x) {
                                return x == newValue;
                            });
                            if (validate == 0) {
                                alert("Ký hiệu không hợp lệ !!");
                                $(element).focus();
                                value(null);
                            } else {
                                value(newValue);
                            }
                            value.valueHasMutated();
                        }
                    }).extend({ notify: 'always' });
                    ko.applyBindingsToNode(element, {
                        value: interceptor
                    });
                }
            };
            ko.applyBindings(view, document.getElementById("chamcongupdate"));
        });
    </script>
</head>
<body>

    <div id="chamcongupdate">
        <table border="0" cellpadding="0" cellspacing="0" width="1400px">
            <tbody>
                <tr>
                    <td>
                        <img alt="TRƯỜNG ĐẠI HỌC QUỐC TẾ HỒNG BÀNG" src="/Images/logo_HBU-log-in.png" align="middle" /></td>
                    <td style="font-family: Arial,Tahoma; font-size: 20pt; font-weight: bold; padding-left: 150px">TRƯỜNG ĐẠI HỌC QUỐC TẾ HỒNG BÀNG</td>
                </tr>
            </tbody>
        </table>
        <div align="center" style="font-family: Arial, Tahoma; font-size: 14pt; font-weight: bold; padding-bottom: 5px; width: 1400px;">BẢNG CHI TIẾT CHẤM CÔNG - <span data-bind="text: thang"></span>/ <span data-bind="    text: nam"></span></div>
        <div align="center" style="font-family: Arial, Tahoma; font-size: 12pt; padding-bottom: 5px; width: 1400px;">(<span data-bind="text: tuNgay"></span> - <span data-bind="    text: denNgay"></span>)</div>
        <table border="0" cellpadding="1" cellspacing="0" style="font-family: Arial, Tahoma; font-size: 10pt; border: solid 1px #CCCCCC; width: 1500px">
            <tr>
                <td data-bind="attr: { colspan: $root.spanlength }, text: TenPhongBan" style="background-color: #888888; color: White; font-weight: bold; font-size: 14pt;"></td>
            </tr>
            <tr style="height: 30px; border-top: solid 1px #CCCCCC;">
                <th style="width: 25px; border-right: solid 1px #CCCCCC;" rowspan="2">STT</th>
                <th style="width: 110px; border-right: solid 1px #CCCCCC;" rowspan="2">Mã quản lý</th>
                <th style="width: 200px; border-right: solid 1px #CCCCCC;" rowspan="2">Họ tên</th>
                <th style="border-bottom: solid 1px #CCCCCC;" data-bind="attr: { colspan: $root.daylength }">Ngày trong tháng</th>
                <th style="border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" colspan="4">Quy ra công</th>
                <th style="border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC; border-right: solid 1px #CCCCCC; width: 80px;" rowspan="2">Tổng cộng ngày công tính lương</th>
            </tr>
            <tr>
                <!-- ko foreach: $root.days -->
                <!-- ko if:T7CN -->
                <td style="border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC; width: 25px; background-color: lightgray" data-bind="html: $data.Ngay + '<br>' + $data.Thu" align="center"></td>
                <!-- /ko -->
                <!-- ko if:!T7CN -->
                <td style="border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC; width: 25px;" data-bind="html: $data.Ngay + '<br>' + $data.Thu" align="center"></td>
                <!-- /ko -->
                <!-- /ko -->
                <td style="border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC; width: 30px;" align="center">NC</td>
                <td style="border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC; width: 30px;" align="center">Phép</td>
                <td style="border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC; width: 30px;" align="center">KL</td>
                <td style="border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC; width: 30px;" align="center">BHXH</td>
            </tr>

            <tbody data-bind="foreach: items">
                <tr>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-right: solid 1px #CCCCCC;" align="center" data-bind="text: $index() + 1"></td>
                    <td style="border-top: solid 1px #CCCCCC; border-right: solid 1px #CCCCCC; width: 100px; text-align: center; white-space: nowrap;" data-bind="text: MaNhanSu"></td>
                    <td style="border-top: solid 1px #CCCCCC; border-right: solid 1px #CCCCCC; width: 140px; white-space: nowrap;" data-bind="text: HoTen"></td>

                    <!-- ko foreach: ChiTietChamCong -->
                    <td data-bind="visible: $data.CC_ChamCongTheoNgayOid != null">
                        <input data-bind="rename: $data.MaHinhThucNghi, value: $data.MaHinhThucNghi" style="width: 27px; text-align: center;" />
                    </td>
                    <td data-bind="visible: $data.CC_ChamCongTheoNgayOid == null"></td>
                    <!-- /ko -->

                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: HuongLuong"></td>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: Phep"></td>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: KhongLuong"></td>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: BHXH"></td>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: TongCong"></td>
                    <%--style: { backgroundColor: LaNhanVienToChucHanhChinh == false ? '#f99191' : '' }--%>
                </tr>
            </tbody>
        </table>
        <div style="padding-top: 10px; text-align: center">
            <%--Nếu là admin--%>
            <!-- ko if: WebGroup()==1 -->
            <!-- ko if:!CheckChot -->
            <input type="button" name="btnSave" value="Lưu" data-bind="click: save" style="font-family: Times New Roman; font-size: 18pt; width: 150px; font-weight: bold;" />
            <!-- /ko -->
            <!-- ko if:CheckChot -->
            <span style="color: red">(*)Tháng này đã chốt chấm công!</span>
            <!-- /ko -->
            <input type="button" name="btnSave" value="Chốt" data-bind="click: chot" style="font-family: Times New Roman; font-size: 18pt; width: 150px; font-weight: bold;" />
            <input type="button" name="btnSave" value="Hủy chốt" data-bind="click: huychot" style="font-family: Times New Roman; font-size: 18pt; width: 150px; font-weight: bold;" />
            <!-- /ko -->

            <%--Nếu là admin đơn vị--%>
            <!-- ko if: WebGroup()==2 -->
            <!-- ko if:CheckChotDonVi -->
            <span style="color: red">(*)Tháng này đã chốt chấm công!!</span>
            <!-- /ko -->
            <input type="button" name="btnSave" value="Chốt" data-bind="click: chotdonvi" style="font-family: Times New Roman; font-size: 18pt; width: 150px; font-weight: bold;" />
            <input type="button" name="btnSave" value="Hủy chốt" data-bind="click: huychotdonvi" style="font-family: Times New Roman; font-size: 18pt; width: 150px; font-weight: bold;" />
            <!-- /ko -->

            <%--Nếu là thư ký--%>
            <!-- ko if: WebGroup()==3 -->
            <!-- ko if:!CheckChotDonVi -->
            <input type="button" name="btnSave" value="Lưu" data-bind="click: save" style="font-family: Times New Roman; font-size: 18pt; width: 150px; font-weight: bold;" />
            <!-- /ko -->
            <!-- ko if:CheckChotDonVi -->
            <span style="color: red">(*)Tháng này đã chốt chấm công!!!</span>
            <!-- /ko -->
            <!-- /ko -->
        </div>
        <div style="width: 100%;" align="left">
            <div style="font-family: Tahoma,Arial; font-size: 10pt; padding-top: 10px;" align="left">
                <span style="font-weight: bold;">Ghi chú:</span> <span style="color: red">(*) Ký hiệu Làm nửa ngày (nửa ngày phép) không thể chấm vào thứ 7</span><br />
                <table style="float: left; width: 33%">
                    <thead>
                        <tr>
                            <td>- +:</td>
                            <td>Làm cả ngày</td>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: HinhThucNghiListForUpdate">
                        <!-- ko if: $index() < 11 -->
                        <tr>
                            <td data-bind="html: '- ' + $data.KyHieu + ':'"></td>
                            <td style="width: 80%" data-bind="html: $data.TenHinhThucNghi"></td>
                        </tr>
                        <!-- /ko -->
                    </tbody>
                </table>
                <table style="float: left; width: 33%">
                    <tbody data-bind="foreach: HinhThucNghiListForUpdate">
                        <!-- ko if: $index() >=11 -->
                        <tr>
                            <td data-bind="html: '- ' + $data.KyHieu + ':'"></td>
                            <td style="width: 80%" data-bind="html: $data.TenHinhThucNghi"></td>
                        </tr>
                        <!-- /ko -->
                    </tbody>
                </table>
            </div>
            <table style="float: left; width: 33%">
                <tr>
                    <td>- NC:</td>
                    <td>Số công hưởng lương</td>
                </tr>
                <tr>
                    <td>- Phép:</td>
                    <td>Số ngày nghỉ phép</td>
                </tr>
                <tr>
                    <td>- KL:</td>
                    <td>Nghỉ không lương</td>
                </tr>
                <tr>
                    <td>- BHXH:</td>
                    <td>Số công hưởng BHXH</td>
                </tr>
            </table>
        </div>
    </div>
</body>

</html>
