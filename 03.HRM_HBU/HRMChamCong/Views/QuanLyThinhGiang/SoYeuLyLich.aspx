<html>
<head></head>
<body>
    <div class="col-lg-12 col-xs-12 col-sm-12">
        <div class="form-horizontal form-bordered">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="form-horizontal form-bordered">
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Mã nhân sự <span style="color: red">(*)</span>:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: MaQuanLy" id="maquanly" onchange="CheckExistsMaQuanLy()"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Họ <span style="color: red">(*)</span>:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: Ho" id="ho" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Tên <span style="color: red">(*)</span>:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: Ten" id="ten"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Ngày sinh:</label>
                        <div class="col-sm-6">
                            <div data-bind="value: NgaySinh" id="jqxNgaySinh"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-12" style="font-weight: bold; text-align: center">Nơi sinh</label>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Quốc gia:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbQuocGia_NoiSinh"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Tỉnh thành:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbTinhThanh_NoiSinh"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Quận huyện:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbQuanHuyen_NoiSinh"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Xã phường:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbXaPhuong_NoiSinh"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Số nhà:</label>
                            <div class="col-sm-6">
                                <input class="form-control" type="text" data-bind="value: SoNha_NoiSinh" style="width: 100%;" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Giới tính:</label>
                        <div class="col-sm-6">
                            <span>
                                <input type="radio" name="radiogioitinh" value="0" id="gioitinhnam">Nam</span>
                            <span>
                                <input type="radio" name="radiogioitinh" value="1" id="gioitinhnu">Nữ</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">CMND:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: CMND" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Ngày cấp:</label>
                        <div class="col-sm-6">
                            <div data-bind="value: NgayCap" id="jqxNgayCap"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Nơi cấp:</label>
                        <div class="col-sm-9">
                            <div id="jqxCbTinhThanh_NoiCap"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Quốc tịch:</label>
                        <div class="col-sm-9">
                            <div id="jqxCbQuocGia"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Khoa | Bộ môn <span style="color: red">(*)</span>:</label>
                        <div class="col-sm-9">
                            <div id="jqxCbBoPhan" ></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Ngày vào cơ quan:</label>
                        <div class="col-sm-6">
                            <div data-bind="value: NgayVaoCoQuan" id="jqxNgayVaoCoQuan"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Đơn vị công tác <span style="color: red">(*)</span>:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: DonViCongTac" id="donvicongtac"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Hợp đồng hiện tại:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: HopDongHienTai" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Email:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: Email" id="email" onchange="CheckEmail()" placeholder="abc@gmail.com"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">ĐT di động:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: DienThoaiDiDong" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">ĐT nhà riêng:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: DienThoaiNhaRieng" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-12" style="font-weight: bold; text-align: center">Địa chỉ thường trú</label>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Quốc gia:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbQuocGia_DCTT"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Tỉnh thành:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbTinhThanh_DCTT"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Quận huyện:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbQuanHuyen_DCTT"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Xã phường:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbXaPhuong_DCTT"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Số nhà:</label>
                            <div class="col-sm-6">
                                <input class="form-control" type="text" data-bind="value: SoNha_DCTT" style="width: 100%;" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-12" style="font-weight: bold; text-align: center">Nơi ở hiện nay</label>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Quốc gia:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbQuocGia_NOHN"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Tỉnh thành:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbTinhThanh_NOHN"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Quận huyện:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbQuanHuyen_NOHN"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Xã phường:</label>
                            <div class="col-sm-6">
                                <div id="jqxCbXaPhuong_NOHN"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-4 control-label no-padding-right">Số nhà:</label>
                            <div class="col-sm-6">
                                <input class="form-control" type="text" data-bind="value: SoNha_NOHN" style="width: 100%;" />
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Tình trạng <span style="color: red">(*)</span>:</label>
                        <div class="col-sm-9">
                            <div id="jqxCbTinhTrang"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label no-padding-right"></label>
                        <a href="#" class="btn btn-labeled btn-blue" style="width: 150px;" data-bind="click: SaveSoYeuLyLich">
                            <i class="btn-label glyphicon glyphicon-ok"></i>Lưu
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>


