<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestWebForm.aspx.cs" Inherits="TestApplication.TestWebForm" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form" runat="server">
        <asp:Chart ID="BirdPopulation" runat="server" EnableViewState="true" Height="382px" Width="320px">
            <ChartAreas>
                <asp:ChartArea Name="ChartArea"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>

        Choose year<asp:DropDownList ID="yearsDropdown" runat="server" OnSelectedIndexChanged="yearsDropdown_SelectedIndexChanged" AutoPostBack="true" Height="18px" style="margin-left: 30px" Width="121px"></asp:DropDownList>
    </form>
</body>
</html>
