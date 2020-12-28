<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="MyUIS_MVC.Pages.Report" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="margin:0 auto; padding:10px">
    
        <asp:Panel ID="Panel1" runat="server">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="700px"  SizeToReportContent="True"
                Width="100%">
            </rsweb:ReportViewer>
        </asp:Panel>
    
       
    
    </div>
    </form>
</body>
</html>