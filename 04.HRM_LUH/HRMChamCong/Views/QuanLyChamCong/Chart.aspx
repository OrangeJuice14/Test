<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chart.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.Chart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Components/jqwidgets/jqx.base.css" rel="stylesheet" />
    <link href="/Components/jqwidgets/jqx.energyblue.css" rel="stylesheet" />
    <script src="/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/Components/jqwidgets/jqx-all.js" type="text/javascript"></script>
    <script src="/Components/jqwidgets/jqxcore.js"></script>
    <%--<script src="/Components/jqwidgets/jqxdata.js"></script>--%>
    <script src="/Components/jqwidgets/jqxdraw.js"></script>
    <script src="/Components/jqwidgets/jqxchart.core.js"></script>
    <script src="/Scripts/knockout-3.2.0.js"></script>
    <script type="text/javascript">
        var TenPhongBan = "", data = []; 
        var formatHour = function (dd) {
            var dtstr = dd.replace(/\D/g, " ");
            var dtcomps = dtstr.split(" ");
            var hours = parseInt(dtcomps[3]);
            var minutes = dtcomps[4]/60;
            var seconds = dtcomps[5] / 3600;
            return hours + minutes + seconds;
        };
        $(document).ready(function () {
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetPhongBan_ById',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON({ id: '<%#Request.QueryString["PhongBan"] %>' }),
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    TenPhongBan = obj.TenBoPhan;
                }
            });

            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_BieuDo',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON({
                    ngay: '<%#Request.QueryString["Ngay"] %>',
                    thang: '<%#Request.QueryString["Thang"] %>',
                    nam: '<%#Request.QueryString["Nam"] %>',
                    bophanId: '<%#Request.QueryString["PhongBan"] %>'
                }),
                async: false,
                success: function (result) {
                    data = $.parseJSON(result.d);
                    $(data).each(function (index, value) {
                        value.HoTen = value.HoTen + " (" + value.MaNhanSu + ") ";
                        if (value.GioVao == null && value.GioRa == null)
                            return;
                        value.GioVao = value.GioVao == null ? null : formatHour(value.GioVao);
                        value.GioRa = value.GioVao != null && value.GioRa == null ? value.GioVao : formatHour(value.GioRa);
                    });
                }
            });
            var toolTipCustomFormatFn = function (value, itemIndex, serie, group, categoryValue, categoryAxis) {
                var from = Math.round(value.from * 3600);
                var to = Math.round(value.to * 3600);
                var hours_from = Math.floor(from / 3600);
                var hours_to = Math.floor(to / 3600);
                var minutes_from = Math.floor((from - (hours_from * 3600)) / 60);
                var minutes_to = Math.floor((to - (hours_to * 3600)) / 60);
                var seconds_from = from - (hours_from * 3600) - (minutes_from * 60);
                var seconds_to = to - (hours_to * 3600) - (minutes_to * 60);

                if (hours_from < 10) {
                    hours_from = "0" + hours_from;
                }
                if (hours_to < 10) {
                    hours_to = "0" + hours_to;
                }
                if (minutes_from < 10) {
                    minutes_from = "0" + minutes_from;
                }
                if (minutes_to < 10) {
                    minutes_to = "0" + minutes_to;
                }
                if (seconds_from < 10) {
                    seconds_from = "0" + seconds_from;
                }
                if (seconds_to < 10) {
                    seconds_to = "0" + seconds_to;
                }
                var time_from = hours_from + ':' + minutes_from + ':' + seconds_from;
                var time_to = hours_to + ':' + minutes_to + ':' + seconds_to;
                var str = '<DIV style="text-align:left"><b>Nhân viên: ' + categoryValue + '</b><br />Thời gian vào: <span style="color:red">' + time_from + '</span><br />';
                str += time_from == time_to ? '' : 'Thời gian ra: <span style="color:red">' + time_to + '</span><br /></DIV>';
                return str;
            };
            var settings = {
                title: "TRƯỜNG ĐẠI HỌC CÔNG NGHIỆP TP.HCM",
                description: "Thống kê giờ đi làm - " + '<%#Request.QueryString["Ngay"] %>' + '/' + '<%#Request.QueryString["Thang"] %>' + '/' + '<%#Request.QueryString["Nam"] %>' + ' (' + TenPhongBan + ')',
                enableAnimations: true,
                showLegend: true,
                padding: { left: 15, top: 5, right: 15, bottom: 5 },
                titlePadding: { left: 90, top: 0, right: 0, bottom: 10 },
                enableCrosshairs: true,
                source: data,
                xAxis:
                {
                    textRotationAngle: 90,
                    dataField: "HoTen",
                    showTickMarks: true,
                    tickMarksInterval: 1,
                    tickMarksColor: '#888888',
                    unitInterval: 1,
                    showGridLines: true,
                    //gridLinesInterval: 1,
                    //gridLinesColor: 'red'
                },
                colorScheme: 'scheme05',
                seriesGroups:
                [
                    {
                        type: 'rangecolumn',
                        columnsGapPercent: 100,
                        //orientation: 'vertical',
                        orientation: 'horizontal',
                        toolTipFormatFunction: toolTipCustomFormatFn,
                        valueAxis:
                        {
                            unitInterval: 1,
                            displayValueAxis: true,
                            position: 'top',
                            flip: true,
                            description: 'Giờ [h]',
                            formatSettings: { sufix: 'h' },
                            axisSize: 'auto',
                            tickMarksColor: '#888888',
                            minValue: 0,
                            maxValue: 24,
                            //showGridLines: false,
                            //gridLinesColor: 'brown',
                            //gridLinesInterval: 0
                        },
                        series: [
                            {
                                dataFieldTo: 'GioRa', displayText: 'Thời gian chấm công', dataFieldFrom: 'GioVao', opacity: 1,
                                showLabels: true,
                                labelsHorizontalAlignment: 'top',
                                formatFunction: function (val) {
                                    if (val.from == null && val.to == null)
                                        return " Chưa quét vân tay";
                                    return "";
                                }
                            }
                        ],
                        bands: [{ minValue: 0, maxValue: 24, color: '#E5E5E5', opacity: 0.5 }]
                    }
                ]
            };
            $('#chartContainer').jqxChart(settings);
        });
    </script>
    <style>
        .over {
            position: relative;
        }

            .over span {
                position: absolute;
                top: -0.5em;
                left: 0;
            }

        .jqx-chart-label-text {
            fill: red !important;
        }

        body, html {
            height: 100%;
        }
    </style>
</head>
<body>
    <div id='chartContainer' style="width: 100%; height: 100%"></div>
</body>
</html>
