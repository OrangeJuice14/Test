<html>
<head></head>
<body>
    <div class="col-lg-12 col-xs-12 col-sm-12">
        <div class="form-horizontal form-bordered">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="form-horizontal form-bordered">                
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Trình độ văn hóa:</label>
                        <div class="col-sm-9">
                            <div id="jqxCbTrinhDoVanHoa"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Trình độ tin học:</label>
                        <div class="col-sm-9">
                            <div id="jqxCbTrinhDoTinHoc"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-3 control-label no-padding-right">Học hàm:</label>
                        <div class="col-sm-9">
                            <div id="jqxCbHocHam"></div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-12" style="font-weight: bold; text-align: center">Trình độ cao nhất hiện tại</label>
                        <div class="row">
                            <label class="col-sm-3 control-label no-padding-right">Trình độ chuyên môn:</label>
                            <div class="col-sm-9">
                                <div id="jqxCbTrinhDoChuyenMon"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-3 control-label no-padding-right">Trường đào tạo:</label>
                            <div class="col-sm-9">
                                <div id="jqxCbTruongDaoTao"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-3 control-label no-padding-right">Chuyên ngành đào tạo:</label>
                            <div class="col-sm-9">
                                <div id="jqxCbChuyenNganhDaoTao"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-3 control-label no-padding-right">Hình thức đào tạo:</label>
                            <div class="col-sm-9">
                                <div id="jqxCbHinhThucDaoTao"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-3 control-label no-padding-right">Năm tốt nghiệp:</label>
                            <div class="col-sm-9">
                                <input class="form-control" type="text" data-bind="value: NamTotNghiep" onkeypress="return CheckNumberWhenKeypress(event);"/>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-12" style="font-weight: bold; text-align: center">Trình độ ngoại ngữ chính</label>
                        <div class="row">
                            <label class="col-sm-3 control-label no-padding-right">Ngoại ngữ:</label>
                            <div class="col-sm-9">
                                <div id="jqxCbNgoaiNgu"></div>
                            </div>
                        </div>
                        <div class="row">
                            <label class="col-sm-3 control-label no-padding-right">Trình độ:</label>
                            <div class="col-sm-9">
                                <div id="jqxCbTrinhDoNgoaiNgu"></div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label no-padding-right"></label>
                        <a href="#" class="btn btn-labeled btn-blue" style="width: 150px;" data-bind="click: SaveTrinhDoChuyenMon">
                            <i class="btn-label glyphicon glyphicon-ok"></i>Lưu
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>

</body>
</html>
