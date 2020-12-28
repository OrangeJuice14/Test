<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ThemGiangVienThinhGiang.aspx.cs" Inherits="HRMChamCong.Views.QuanLyThinhGiang.ThemGiangVienThinhGiang" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <script>
        var OidThinhGiang;
        ///////////////////////////////////////Sơ yếu lý lịch///////////////////////////////////////////
        //Khởi tạo model
        function ViewModel_SoYeuLyLich() {
            var self = this;
            self.Oid = ko.observable();
            self.MaQuanLy = ko.observable();
            self.Ho = ko.observable();
            self.Ten = ko.observable();
            self.GioiTinh = ko.observable();
            self.NgaySinh = ko.observable();
            self.CMND = ko.observable();
            self.NgayCap = ko.observable();
            self.NoiCap = ko.observable();
            self.NgayVaoCoQuan = ko.observable();
            self.DonViCongTac = ko.observable();
            self.HopDongHienTai = ko.observable();
            self.Email = ko.observable();
            self.DienThoaiDiDong = ko.observable();
            self.DienThoaiNhaRieng = ko.observable();
            self.TinhTrang = ko.observable();
            //
            self.BoPhan = ko.observable();
            self.QuocGia = ko.observable();
            self.TonGiao = ko.observable();
            self.DanToc = ko.observable();
            self.TinhTrangHonNhan = ko.observable();
            //Nơi sinh
            self.QuocGia_NoiSinh = ko.observable();
            self.TinhThanh_NoiSinh = ko.observable();
            self.QuanHuyen_NoiSinh = ko.observable();
            self.XaPhuong_NoiSinh = ko.observable();
            self.SoNha_NoiSinh = ko.observable();
            self.Oid_NoiSinh = ko.observable();
            //Đia chỉ thường trú
            self.QuocGia_DCTT = ko.observable();
            self.TinhThanh_DCTT = ko.observable();
            self.QuanHuyen_DCTT = ko.observable();
            self.XaPhuong_DCTT = ko.observable();
            self.SoNha_DCTT = ko.observable();
            self.Oid_DCTT = ko.observable();
            //Nơi ở hiện nay
            self.QuocGia_NOHN = ko.observable();
            self.TinhThanh_NOHN = ko.observable();
            self.QuanHuyen_NOHN = ko.observable();
            self.XaPhuong_NOHN = ko.observable();
            self.SoNha_NOHN = ko.observable();
            self.Oid_NOHN = ko.observable();
            //List
            self.data_tinhtrang = ko.observableArray();
            self.data_bophan = ko.observableArray();
            self.data_dantoc = ko.observableArray();
            self.data_tongiao = ko.observableArray();
            self.data_tinhtranghonnhan = ko.observableArray();
            self.data_quocgia = ko.observableArray();
            self.data_tinhthanh = ko.observableArray();
            self.data_quanhuyen = ko.observableArray();
            self.data_xaphuong = ko.observableArray();
            self.data_trinhdovanhoa = ko.observableArray();
            self.data_trinhdochuyenmon = ko.observableArray();
            self.data_chuyennganhdaotao = ko.observableArray();
            self.data_truongdaotao = ko.observableArray();
            self.data_ngoaingu = ko.observableArray();
            self.data_trinhdongoaingu = ko.observableArray();

            //Lấy Oid của thỉnh giảng  
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetOidGiangVienThinhGiang',
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //
                    var obj = result;
                    OidThinhGiang = obj.d;
                }
            });


            /////////////////////Khởi tạo các control radio
            if (self.GioiTinh() == 'Nam') {
                $("#gioitinhnam").prop("checked", true);
            }
            else {
                $("#gioitinhnu").prop("checked", true);
            }

            /////////////////////Khởi tạo các control datatime
            if (self.NgaySinh() != undefined) {
                $("#jqxNgaySinh").jqxDateTimeInput({ width: '100%', height: '25px', formatString: 'dd/MM/yyyy', value: new Date(self.NgaySinh()) });
            }
            else {
                $("#jqxNgaySinh").jqxDateTimeInput({ width: '100%', height: '25px', formatString: 'dd/MM/yyyy', value: new Date() });
            }
            if (self.NgayCap() != undefined) {
                $("#jqxNgayCap").jqxDateTimeInput({ width: '100%', height: '25px', formatString: 'dd/MM/yyyy', value: new Date(self.NgayCap()) });
            }
            else {
                $("#jqxNgayCap").jqxDateTimeInput({ width: '100%', height: '25px', formatString: 'dd/MM/yyyy', value: new Date() });
            }
            if (self.NgayVaoCoQuan() != undefined) {
                $("#jqxNgayVaoCoQuan").jqxDateTimeInput({ width: '100%', height: '25px', formatString: 'dd/MM/yyyy', value: new Date(self.NgayVaoCoQuan()) });
            }
            else {
                $("#jqxNgayVaoCoQuan").jqxDateTimeInput({ width: '100%', height: '25px', formatString: 'dd/MM/yyyy', value: new Date() });
            }
            /////////////////////Khởi tạo các control combo
            //Lấy tất cả danh mục
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetDanhMucGiangVienThingGiangAll',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    //
                    var mydata = JSON.stringify(result);
                    var dataList = jQuery.parseJSON(mydata);
                    //
                    for (var i in dataList) {
                        var objList = dataList[i];
                        var obj = jQuery.parseJSON(objList);//
                        //
                        if (obj.BoPhan != null) {
                            self.data_bophan(obj.BoPhan);
                        }
                        if (obj.DanToc != null) {
                            self.data_dantoc(obj.DanToc);
                        }
                        if (obj.TonGiao != null) {
                            self.data_tongiao(obj.TonGiao);
                        }
                        if (obj.TinhTrangHonNhan != null) {
                            self.data_tinhtranghonnhan(obj.TinhTrangHonNhan);
                        }
                        if (obj.QuocGia != null) {
                            self.data_quocgia(obj.QuocGia);
                        }
                        if (obj.TinhThanh != null) {
                            self.data_tinhthanh(obj.TinhThanh);
                        }
                        if (obj.QuanHuyen != null) {
                            self.data_quanhuyen(obj.QuanHuyen);
                        }
                        if (obj.XaPhuong != null) {
                            self.data_xaphuong(obj.XaPhuong);
                        }
                        if (obj.TinhTrang != null) {
                            self.data_tinhtrang(obj.TinhTrang);
                        }
                    }
                }
            });

            //Đơn vị
            var serializedArr = JSON.stringify(eval("(" + self.data_bophan() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbBoPhan").jqxComboBox({ source: dataSource, displayMember: "TenBoPhan", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbBoPhan").jqxComboBox('selectItem', self.BoPhan());
            $('#jqxCbBoPhan').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.BoPhan(item.value);
                    }
                }
            });

            //Quốc gia 
            serializedArr = JSON.stringify(eval("(" + self.data_quocgia() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbQuocGia").jqxComboBox({ source: dataSource, displayMember: "TenQuocGia", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbQuocGia").jqxComboBox('selectItem', self.QuocGia());
            $('#jqxCbQuocGia').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.QuocGia(item.value);
                    }
                }
            });

            ///////////////Nơi sinh/////////////////////////
            //1. Quốc gia
            serializedArr = JSON.stringify(eval("(" + self.data_quocgia() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbQuocGia_NoiSinh").jqxComboBox({ source: dataSource, displayMember: "TenQuocGia", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbQuocGia_NoiSinh").jqxComboBox('selectItem', self.QuocGia_NoiSinh());
            $('#jqxCbQuocGia_NoiSinh').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.QuocGia_NoiSinh(item.value);
                    }
                }
            });

            //2. Tỉnh thành
            serializedArr = JSON.stringify(eval("(" + self.data_tinhthanh() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbTinhThanh_NoiSinh").jqxComboBox({ source: dataSource, displayMember: "TenTinhThanh", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTinhThanh_NoiSinh").jqxComboBox('selectItem', self.TinhThanh_NoiSinh());
            $('#jqxCbTinhThanh_NoiSinh').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TinhThanh_NoiSinh(item.value);
                    }
                }
            });
            //3. Quận huyện
            serializedArr = JSON.stringify(eval("(" + self.data_quanhuyen() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbQuanHuyen_NoiSinh").jqxComboBox({ source: dataSource, displayMember: "TenQuanHuyen", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbQuanHuyen_NoiSinh").jqxComboBox('selectItem', self.QuanHuyen_NoiSinh());
            $('#jqxCbQuanHuyen_NoiSinh').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.QuanHuyen_NoiSinh(item.value);
                    }
                }
            });
            //4. Xã phường
            serializedArr = JSON.stringify(eval("(" + self.data_xaphuong() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbXaPhuong_NoiSinh").jqxComboBox({ source: dataSource, displayMember: "TenXaPhuong", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbXaPhuong_NoiSinh").jqxComboBox('selectItem', self.XaPhuong_NoiSinh());
            $('#jqxCbXaPhuong_NoiSinh').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.XaPhuong_NoiSinh(item.value);
                    }
                }
            });

            ///////////////Nơi cấp/////////////////////////
            //2. Tỉnh thành
            serializedArr = JSON.stringify(eval("(" + self.data_tinhthanh() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbTinhThanh_NoiCap").jqxComboBox({ source: dataSource, displayMember: "TenTinhThanh", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTinhThanh_NoiCap").jqxComboBox('selectItem', self.TinhThanh_NoiSinh());
            $('#jqxCbTinhThanh_NoiCap').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TinhThanh_NoiCap(item.value);
                    }
                }
            });
            //Tình trạng
            serializedArr = JSON.stringify(eval("(" + self.data_tinhtrang() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbTinhTrang").jqxComboBox({ source: dataSource, displayMember: "TenTinhTrang", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTinhTrang").jqxComboBox('selectItem', self.TinhTrang());
            $('#jqxCbTinhTrang').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TinhTrang(item.value);
                    }
                }
            });

            ///////////////Địa chỉ thường trú/////////////////////////
            //1. Quốc gia
            serializedArr = JSON.stringify(eval("(" + self.data_quocgia() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbQuocGia_DCTT").jqxComboBox({ source: dataSource, displayMember: "TenQuocGia", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbQuocGia_DCTT").jqxComboBox('selectItem', self.QuocGia_DCTT());
            $('#jqxCbQuocGia_DCTT').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.QuocGia_DCTT(item.value);
                    }
                }
            });
            //2. Tỉnh thành
            serializedArr = JSON.stringify(eval("(" + self.data_tinhthanh() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbTinhThanh_DCTT").jqxComboBox({ source: dataSource, displayMember: "TenTinhThanh", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTinhThanh_DCTT").jqxComboBox('selectItem', self.TinhThanh_DCTT());
            $('#jqxCbTinhThanh_DCTT').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TinhThanh_DCTT(item.value);
                    }
                }
            });
            //3. Quận huyện
            serializedArr = JSON.stringify(eval("(" + self.data_quanhuyen() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbQuanHuyen_DCTT").jqxComboBox({ source: dataSource, displayMember: "TenQuanHuyen", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbQuanHuyen_DCTT").jqxComboBox('selectItem', self.QuanHuyen_DCTT());
            $('#jqxCbQuanHuyen_DCTT').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.QuanHuyen_DCTT(item.value);
                    }
                }
            });
            //4. Xã phường
            serializedArr = JSON.stringify(eval("(" + self.data_xaphuong() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbXaPhuong_DCTT").jqxComboBox({ source: dataSource, displayMember: "TenXaPhuong", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbXaPhuong_DCTT").jqxComboBox('selectItem', self.XaPhuong_DCTT());
            $('#jqxCbXaPhuong_DCTT').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.XaPhuong_DCTT(item.value);
                    }
                }
            });

            ///////////////Nơi ở hiện tại/////////////////////////
            //1. Quốc gia
            serializedArr = JSON.stringify(eval("(" + self.data_quocgia() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbQuocGia_NOHN").jqxComboBox({ source: dataSource, displayMember: "TenQuocGia", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbQuocGia_NOHN").jqxComboBox('selectItem', self.QuocGia_NOHN());
            $('#jqxCbQuocGia_NOHN').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.QuocGia_NOHN(item.value);
                    }
                }
            });
            //2. Tỉnh thành
            serializedArr = JSON.stringify(eval("(" + self.data_tinhthanh() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbTinhThanh_NOHN").jqxComboBox({ source: dataSource, displayMember: "TenTinhThanh", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTinhThanh_NOHN").jqxComboBox('selectItem', self.TinhThanh_NOHN());
            $('#jqxCbTinhThanh_NOHN').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TinhThanh_NOHN(item.value);
                    }
                }
            });
            //3. Quận huyện
            serializedArr = JSON.stringify(eval("(" + self.data_quanhuyen() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbQuanHuyen_NOHN").jqxComboBox({ source: dataSource, displayMember: "TenQuanHuyen", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbQuanHuyen_NOHN").jqxComboBox('selectItem', self.QuanHuyen_NOHN());
            $('#jqxCbQuanHuyen_NOHN').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.QuanHuyen_NOHN(item.value);
                    }
                }
            });
            //4. Xã phường
            serializedArr = JSON.stringify(eval("(" + self.data_xaphuong() + ")"));
            dataSource = JSON.parse(serializedArr);
            $("#jqxCbXaPhuong_NOHN").jqxComboBox({ source: dataSource, displayMember: "TenXaPhuong", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbXaPhuong_NOHN").jqxComboBox('selectItem', self.XaPhuong_NOHN());
            $('#jqxCbXaPhuong_NOHN').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.XaPhuong_NOHN(item.value);
                    }
                }
            });
        }
        //Các sự kiện
        ViewModel_SoYeuLyLich.prototype = {
            //
            SaveSoYeuLyLich: function () {
                var self = this;
                //
                if (self.MaQuanLy() == null || self.MaQuanLy() == '' || self.MaQuanLy() == undefined) {
                    alert('Mã quản lý không được trống.');
                    $('#maquanly').focus();
                    return;
                }
                //
                if (self.Ho() == null || self.Ho() == '' || self.Ho() == undefined) {
                    alert('Họ không được trống.');
                    $('#ho').focus();
                    return;
                }
                //
                if (self.Ten() == null || self.Ten() == '' || self.Ten() == undefined) {
                    alert('Tên không được trống.');
                    $('#ten').focus();
                    return;
                }
                //
                if (self.BoPhan() == null || self.BoPhan() == undefined) {
                    alert('Khoa | bộ môn không được trống.');
                    $('#jqxCbBoPhan').focus();
                    return;
                }//
                if (self.DonViCongTac() == null || self.DonViCongTac() == undefined) {
                    alert('Đơn vị công tác không được trống.');
                    $('#donvicongtac').focus();
                    return;
                }
                //
                if (self.TinhTrang() == null || self.TinhTrang() == undefined) {
                    alert('Tình trạng được trống.');
                    $('#jqxCbTinhTrang').focus();
                    return;
                }
                //Tiến hành lưu dữ liệu
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/SaveGiangVienThinhGiang',
                    async: false,
                    data: ko.toJSON({
                        obj:
                        {
                            Oid: OidThinhGiang,
                            MaQuanLy: self.MaQuanLy(),
                            Ho: self.Ho(),
                            Ten: self.Ten(),
                            BoPhan: self.BoPhan(),
                            NgaySinh: self.NgaySinh(),
                            GioiTinh: $("#gioitinhnam").val() == '0' ? 'Nam' : 'Nữ',
                            CMND: self.CMND(),
                            NgayCap: self.NgayCap(),
                            NoiCap: self.NoiCap(),
                            QuocGia: self.QuocGia(),
                            TinhTrangHonNhan: self.TinhTrangHonNhan(),
                            DanToc: self.DanToc(),
                            TonGiao: self.TonGiao(),
                            NgayVaoCoQuan: self.NgayVaoCoQuan(),
                            //
                            QuocGia_NoiSinh: self.QuocGia_NoiSinh(),
                            TinhThanh_NoiSinh: self.TinhThanh_NoiSinh(),
                            QuanHuyen_NoiSinh: self.QuanHuyen_NoiSinh(),
                            XaPhuong_NoiSinh: self.XaPhuong_NoiSinh(),
                            SoNha_NoiSinh: self.SoNha_NoiSinh(),
                            Oid_NoiSinh: self.Oid_NoiSinh(),
                            NgayVaoCoQuan: self.NgayVaoCoQuan(),
                            DonViCongTac: self.DonViCongTac(),
                            Email: self.Email(),
                            DienThoaiDiDong: self.DienThoaiDiDong(),
                            DienThoaiNhaRieng: self.DienThoaiNhaRieng(),
                            TinhTrang: self.TinhTrang(),
                            QuocGia_DCTT: self.QuocGia_DCTT(),
                            TinhThanh_DCTT: self.TinhThanh_DCTT(),
                            QuanHuyen_DCTT: self.QuanHuyen_DCTT(),
                            XaPhuong_DCTT: self.XaPhuong_DCTT(),
                            SoNha_DCTT: self.SoNha_DCTT(),
                            Oid_DCTT: self.Oid_DCTT(),
                            QuocGia_NOHN: self.QuocGia_NOHN(),
                            TinhThanh_NOHN: self.TinhThanh_NOHN(),
                            QuanHuyen_NOHN: self.QuanHuyen_NOHN(),
                            XaPhuong_NOHN: self.XaPhuong_NOHN(),
                            SoNha_NOHN: self.SoNha_NOHN(),
                            Oid_NOHN: self.Oid_NOHN(),
                        },
                        type: "SoYeuLyLich"
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var obj = result;
                        //
                        if (obj.d == 'success') {
                            alert('Lưu dữ liệu thành công.');
                        }
                        else {
                            alert('Lưu dữ liệu thất bại.');
                        }
                    }
                });
            }
        }

        ///////////////////////////////////////Trình độ chuyên môn///////////////////////////////////////////
        //Khởi tạo model 
        function ViewModel_TrinhDoChuyenMon() {
            var self = this;
            self.Oid = ko.observable();
            self.Oid_NVTD = ko.observable();
            self.TrinhDoVanHoa = ko.observable();
            self.TrinhDoTinHoc = ko.observable();
            self.HocHam = ko.observable();
            self.TrinhDoChuyenMon = ko.observable();
            self.TruongDaoTao = ko.observable();
            self.ChuyenNganhDaoTao = ko.observable();
            self.HinhThucDaoTao = ko.observable();
            self.NamTotNghiep = ko.observable();
            self.NgoaiNgu = ko.observable();
            self.TrinhDoNgoaiNgu = ko.observable();
            //
            self.data_trinhdovanhoa = ko.observableArray();
            self.data_trinhdotinhoc = ko.observableArray();
            self.data_hocham = ko.observableArray();
            self.data_trinhdochuyenmon = ko.observableArray();
            self.data_truongdaotao = ko.observableArray();
            self.data_chuyennganhdaotao = ko.observableArray();
            self.data_hinhthucdaotao = ko.observableArray();
            self.data_ngoaingu = ko.observableArray();
            self.data_trinhdongoaingu = ko.observableArray();


            //Lấy tất cả danh mục
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetDanhMucGiangVienThingGiangAll',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    //
                    var mydata = JSON.stringify(result);
                    var dataList = jQuery.parseJSON(mydata);
                    //
                    for (var i in dataList) {
                        var objList = dataList[i];
                        var obj = jQuery.parseJSON(objList);//
                        //
                        if (obj.TrinhDoVanHoa != null) {
                            self.data_trinhdovanhoa(obj.TrinhDoVanHoa);
                        }
                        if (obj.TrinhDoTinHoc != null) {
                            self.data_trinhdotinhoc(obj.TrinhDoTinHoc);
                        }
                        if (obj.HocHam != null) {
                            self.data_hocham(obj.HocHam);
                        }
                        if (obj.TrinhDoChuyenMon != null) {
                            self.data_trinhdochuyenmon(obj.TrinhDoChuyenMon);
                        }
                        if (obj.TruongDaoTao != null) {
                            self.data_truongdaotao(obj.TruongDaoTao);
                        }
                        if (obj.ChuyenNganhDaoTao != null) {
                            self.data_chuyennganhdaotao(obj.ChuyenNganhDaoTao);
                        }
                        if (obj.HinhThucDaoTao != null) {
                            self.data_hinhthucdaotao(obj.HinhThucDaoTao);
                        }
                        if (obj.NgoaiNgu != null) {
                            self.data_ngoaingu(obj.NgoaiNgu);
                        }
                        if (obj.TrinhDoNgoaiNgu != null) {
                            self.data_trinhdongoaingu(obj.TrinhDoNgoaiNgu);
                        }
                    }
                }
            });


            //////////////////////Khởi tạo các combobox/////////
            //Học hàm
            var serializedArr = JSON.stringify(eval("(" + self.data_hocham() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbHocHam").jqxComboBox({ source: dataSource, displayMember: "TenHocHam", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbHocHam").jqxComboBox('selectItem', self.HocHam());
            $('#jqxCbHocHam').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.HocHam(item.value);
                    }
                }
            });
            //Trình độ văn hóa
            var serializedArr = JSON.stringify(eval("(" + self.data_trinhdovanhoa() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbTrinhDoVanHoa").jqxComboBox({ source: dataSource, displayMember: "TenTrinhDoVanHoa", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTrinhDoVanHoa").jqxComboBox('selectItem', self.TrinhDoVanHoa());
            $('#jqxCbTrinhDoVanHoa').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TrinhDoVanHoa(item.value);
                    }
                }
            });
            //Trình độ tin học
            var serializedArr = JSON.stringify(eval("(" + self.data_trinhdotinhoc() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbTrinhDoTinHoc").jqxComboBox({ source: dataSource, displayMember: "TenTrinhDoTinHoc", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTrinhDoTinHoc").jqxComboBox('selectItem', self.TrinhDoTinHoc());
            $('#jqxCbTrinhDoTinHoc').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TrinhDoTinHoc(item.value);
                    }
                }
            });
            //Trình độ chuyên môn
            var serializedArr = JSON.stringify(eval("(" + self.data_trinhdochuyenmon() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbTrinhDoChuyenMon").jqxComboBox({ source: dataSource, displayMember: "TenTrinhDoChuyenMon", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTrinhDoChuyenMon").jqxComboBox('selectItem', self.TrinhDoChuyenMon());
            $('#jqxCbTrinhDoChuyenMon').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TrinhDoChuyenMon(item.value);
                    }
                }
            });
            //Chuyên ngành đào tạo
            var serializedArr = JSON.stringify(eval("(" + self.data_chuyennganhdaotao() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbChuyenNganhDaoTao").jqxComboBox({ source: dataSource, displayMember: "TenChuyenNganhDaoTao", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbChuyenNganhDaoTao").jqxComboBox('selectItem', self.ChuyenNganhDaoTao());
            $('#jqxCbChuyenNganhDaoTao').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.ChuyenNganhDaoTao(item.value);
                    }
                }
            });
            //Trường đào tạo
            var serializedArr = JSON.stringify(eval("(" + self.data_truongdaotao() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbTruongDaoTao").jqxComboBox({ source: dataSource, displayMember: "TenTruongDaoTao", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTruongDaoTao").jqxComboBox('selectItem', self.TruongDaoTao());
            $('#jqxCbTruongDaoTao').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TruongDaoTao(item.value);
                    }
                }
            });
            //Hình thức đào tạo
            var serializedArr = JSON.stringify(eval("(" + self.data_hinhthucdaotao() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbHinhThucDaoTao").jqxComboBox({ source: dataSource, displayMember: "TenHinhThucDaoTao", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbHinhThucDaoTao").jqxComboBox('selectItem', self.HinhThucDaoTao());
            $('#jqxCbHinhThucDaoTao').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.HinhThucDaoTao(item.value);
                    }
                }
            });
            //Ngoại ngữ
            var serializedArr = JSON.stringify(eval("(" + self.data_ngoaingu() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbNgoaiNgu").jqxComboBox({ source: dataSource, displayMember: "TenNgoaiNgu", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbNgoaiNgu").jqxComboBox('selectItem', self.NgoaiNgu());
            $('#jqxCbNgoaiNgu').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.NgoaiNgu(item.value);
                    }
                }
            });
            //Trình độ Ngoại ngữ
            var serializedArr = JSON.stringify(eval("(" + self.data_trinhdongoaingu() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbTrinhDoNgoaiNgu").jqxComboBox({ source: dataSource, displayMember: "TenTrinhDoNgoaiNgu", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbTrinhDoNgoaiNgu").jqxComboBox('selectItem', self.TrinhDoNgoaiNgu());
            $('#jqxCbTrinhDoNgoaiNgu').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.TrinhDoNgoaiNgu(item.value);
                    }
                }
            });
        }
        //Các sự kiện
        ViewModel_TrinhDoChuyenMon.prototype = {
            //
            SaveTrinhDoChuyenMon: function () {
                var self = this;

                //
                if (OidThinhGiang == null || OidThinhGiang == undefined) {
                    alert('Lưu thông tin sơ yếu lý lịch trước.');
                    return;
                }

                //Lấy giảng viên thỉnh giảng    
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/GetGiangVienThinhGiang_ByOid',
                    async: false,
                    data: ko.toJSON({ id: OidThinhGiang }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        //
                        var mydata = JSON.stringify(result);
                        var dataList = jQuery.parseJSON(mydata);
                        for (var i in dataList) {
                            var objList = dataList[i];
                            var obj = jQuery.parseJSON(objList);//
                            if (obj != null) {
                                var data = obj;
                                self.Oid(data.Oid);
                                self.MaQuanLy = ko.observable(data.MaQuanLy);
                            }
                        }
                    }
                });

                //Kiểm tra lần nữa
                if (self.Oid() == null || self.Oid() == undefined) {
                    alert('Lưu thông tin sơ yếu lý lịch trước.');
                    return;
                }

                //Tiến hành lưu dữ liệu
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/SaveGiangVienThinhGiang',
                    async: false,
                    data: ko.toJSON({
                        obj:
                        {
                            Oid: self.Oid(),
                            Oid_NVTD: self.Oid_NVTD(),
                            NamTotNghiep: self.NamTotNghiep(),
                            //
                            TrinhDoVanHoa: self.TrinhDoVanHoa(),
                            TrinhDoTinHoc: self.TrinhDoTinHoc(),
                            HocHam: self.HocHam(),
                            TrinhDoChuyenMon: self.TrinhDoChuyenMon(),
                            ChuyenNganhDaoTao: self.ChuyenNganhDaoTao(),
                            TruongDaoTao: self.TruongDaoTao(),
                            HinhThucDaoTao: self.HinhThucDaoTao(),
                            NamTotNghiep: self.NamTotNghiep(),
                            NgoaiNgu: self.NgoaiNgu(),
                            TrinhDoNgoaiNgu: self.TrinhDoNgoaiNgu(),
                        },
                        type: "TrinhDoChuyenMon"
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var obj = result;
                        //
                        if (obj.d == 'success') {
                            alert('Lưu dữ liệu thành công.');
                        }
                        else if (obj.d == 'erroradd') {
                            alert('Lưu sơ yếu lý lịch trước.');
                        }
                        else {
                            alert('Lưu dữ liệu thất bại.');
                        }
                    }
                });
            }
        }

        ///////////////////////////////////////Trình độ chuyên môn///////////////////////////////////////////
        //Khởi tạo model 
        function ViewModel_ThongTinLuong() {
            var self = this;
            self.Oid = ko.observable();
            self.Oid_NVTTL = ko.observable();
            self.Oid_TKNH = ko.observable();
            self.MaSoThue = ko.observable();
            self.CoQuanThue = ko.observable();
            self.SoTaiKhoan = ko.observable();
            self.NganHang = ko.observable();
            //
            self.data_coquanthue = ko.observableArray();
            self.data_nganhang = ko.observableArray();

            //Lấy tất cả danh mục
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetDanhMucGiangVienThingGiangAll',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    //
                    var mydata = JSON.stringify(result);
                    var dataList = jQuery.parseJSON(mydata);
                    //
                    for (var i in dataList) {
                        var objList = dataList[i];
                        var obj = jQuery.parseJSON(objList);//
                        //
                        if (obj.CoQuanThue != null) {
                            self.data_coquanthue(obj.CoQuanThue);
                        }
                        if (obj.NganHang != null) {
                            self.data_nganhang(obj.NganHang);
                        }
                    }
                }
            });


            //////////////////////Khởi tạo các combobox/////////
            //Cơ quan thuế
            var serializedArr = JSON.stringify(eval("(" + self.data_coquanthue() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbCoQuanThue").jqxComboBox({ source: dataSource, displayMember: "TenCoQuanThue", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbCoQuanThue").jqxComboBox('selectItem', self.CoQuanThue());
            $('#jqxCbCoQuanThue').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.CoQuanThue(item.value);
                    }
                }
            });
            //Ngân hàng
            var serializedArr = JSON.stringify(eval("(" + self.data_nganhang() + ")"));
            var dataSource = JSON.parse(serializedArr);
            $("#jqxCbNganHang").jqxComboBox({ source: dataSource, displayMember: "TenNganHang", valueMember: "Oid", width: '100%', height: '25px' });
            $("#jqxCbNganHang").jqxComboBox('selectItem', self.NganHang());
            $('#jqxCbNganHang').on('select', function (event) {
                var args = event.args;
                if (args != undefined) {
                    var item = event.args.item;
                    if (item != null) {
                        //
                        self.NganHang(item.value);
                    }
                }
            });
        }
        ViewModel_ThongTinLuong.prototype = {
            //
            SaveThongTinLuong: function () {
                var self = this;

                //
                if (OidThinhGiang == null || OidThinhGiang == undefined) {
                    alert('Lưu thông tin sơ yếu lý lịch trước.');
                    return;
                }

                //Lấy giảng viên thỉnh giảng    
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/GetGiangVienThinhGiang_ByOid',
                    async: false,
                    data: ko.toJSON({ id: OidThinhGiang }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        //
                        var mydata = JSON.stringify(result);
                        var dataList = jQuery.parseJSON(mydata);
                        for (var i in dataList) {
                            var objList = dataList[i];
                            var obj = jQuery.parseJSON(objList);//
                            if (obj != null) {
                                var data = obj;
                                self.Oid(data.Oid);
                                self.MaQuanLy = ko.observable(data.MaQuanLy);
                            }
                        }
                    }
                });

                //Kiểm tra lần nữa
                if (self.Oid() == null || self.Oid() == undefined) {
                    alert('Lưu thông tin sơ yếu lý lịch trước.');
                    return;
                }

                //Tiến hành lưu dữ liệu
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/SaveGiangVienThinhGiang',
                    async: false,
                    data: ko.toJSON({
                        obj:
                        {
                            Oid: self.Oid(),
                            Oid_NVTTL: self.Oid_NVTTL(),
                            Oid_TKNH: self.Oid_TKNH(),
                            //
                            SoTaiKhoan: self.SoTaiKhoan(),
                            NganHang: self.NganHang(),
                            MaSoThue: self.MaSoThue(),
                            CoQuanThue: self.CoQuanThue()
                        },
                        type: "ThongTinLuong"
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var obj = result;
                        //
                        if (obj.d == 'success') {
                            alert('Lưu dữ liệu thành công.');
                        }
                        else {
                            alert('Lưu dữ liệu thất bại.');
                        }
                    }
                });
            }
        }

        //Kiểm tra mả quản lý
        function CheckExistsMaQuanLy() {
            var maquanly = $('#maquanly').val();

            //Lấy Oid của thỉnh giảng  
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/CheckExistsMaQuanLyOfGiangVienThinhGiang',
                async: false,
                data: ko.toJSON({ Oid: OidThinhGiang, MaQuanLy: maquanly }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //
                    var obj = result;
                    if (obj.d == "exists") {
                        alert("Mã quản lý đã tồn tại.");
                        $('#maquanly').val('');
                        $('#maquanly').focus();
                    }
                }
            });
        }

        //Kiểm tra email
        function CheckEmail() {
            var email = $('#email').val();
            var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            //
            if (!regex.test(email)) {
                alert('Email [' + email + '] không hợp lệ.');
                $('#email').val('');
                $('#email').focus();
            }
        }

        //Hàm kiểm tra số
        function CheckNumberWhenKeypress(e) {
            //Hàm dùng để ngăn người dùng nhập các ký tự khác ký tự số vào TextBox
            var keypressed = null;

            if (window.event) {
                keypressed = window.event.keyCode; //IE
            }
            else {
                keypressed = e.which; //NON-IE, Standard
            }
            if (keypressed < 48 || keypressed > 57) { //CharCode của 0 là 48 (Theo bảng mã ASCII)
                //CharCode của 9 là 57 (Theo bảng mã ASCII)
                if (keypressed == 8 || keypressed == 127) {//Phím Delete và Phím Back
                    return;
                }
                alert('Vui lòng nhập số');
                return false;
            }
        }

        //Hàm tạo load tab
        function LoadMenuTab() {
            var self = this;
            self.menuList = ko.observableArray();
            var obj = [{ "Name": "Sơ yếu lý lịch", "ID": "SoYeuLyLich", "Url": "/Views/QuanLyThinhGiang/SoYeuLyLich.aspx" },
                       { "Name": "Thông tin lương", "ID": "ThongTinLuong", "Url": "/Views/QuanLyThinhGiang/ThongTinLuong.aspx" },
                       { "Name": "Trình độ chuyên môn", "ID": "TrinhDoChuyenMon", "Url": "/Views/QuanLyThinhGiang/TrinhDoChuyenMon.aspx" }]
            self.menuList(obj);
        }
        //Tạo tab menu
        $(document).ready(function () {
            var menuTab = new LoadMenuTab();
            ko.applyBindings(menuTab, $('#jqxTabs_MenuList_ThemGiangVienThinhGiang')[0]);
            $('#jqxTabs_MenuList_ThemGiangVienThinhGiang').jqxTabs({ width: '100%', theme: 'darkBlue', scrollStep: 500 });

            var loadPage = function (url, tabIndex, value) {
                $.get(url, function (data) {
                    //Improt trang chi tiết
                    $('#' + value).html(data);
                    ko.cleanNode($('#' + value)[0]);
                    ko.applyBindings(new window["ViewModel_" + value](), document.getElementById(value));
                });
            };
            loadPage(menuTab.menuList()[0].Url, 1, menuTab.menuList()[0].Url.split('/')[3].split('.')[0]);

            $('#jqxTabs_MenuList_ThemGiangVienThinhGiang').on('selected', function (event) {
                var pageIndex = event.args.item;
                var contentDiv = $("#jqxTabs_MenuList_ThemGiangVienThinhGiang .jqx-tabs-content-element")[pageIndex];
                if (contentDiv.id == '')
                    return;
                loadPage('/Views/QuanLyThinhGiang/' + contentDiv.id + '.aspx', pageIndex, contentDiv.id);
            });
        });
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">  
     <div id='jqxTabs_MenuList_ThemGiangVienThinhGiang' >
        <ul >
             <!-- ko foreach:menuList -->
             <li data-bind="text: Name" style="margin-left: 5px;"></li>
             <!-- /ko -->
   
        </ul>
        <!-- ko foreach:menuList -->
            <div data-bind="attr: { id: $data.ID }" style="padding: 10px 10px;">     
            </div>
        <!-- /ko -->
     
    </div>
</asp:content>
