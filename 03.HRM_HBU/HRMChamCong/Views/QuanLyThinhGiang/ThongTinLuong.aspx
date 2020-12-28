<html>
<head></head>
<body>
    <div class="col-lg-12 col-xs-12 col-sm-12">
        <div class="form-horizontal form-bordered">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="form-horizontal form-bordered">
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Cơ quan thuế:</label>
                        <div class="col-sm-9">
                            <div id="jqxCbCoQuanThue"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Mã số thuế:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: MaSoThue" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Số tài khoản:</label>
                        <div class="col-sm-9">
                            <input class="form-control" type="text" data-bind="value: SoTaiKhoan" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Ngân hàng:</label>
                        <div class="col-sm-9">
                            <div id="jqxCbNganHang"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label no-padding-right"></label>
                        <a href="#" class="btn btn-labeled btn-blue" style="width: 150px;" data-bind="click: SaveThongTinLuong">
                            <i class="btn-label glyphicon glyphicon-ok"></i>Lưu
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>

</body>
</html>
