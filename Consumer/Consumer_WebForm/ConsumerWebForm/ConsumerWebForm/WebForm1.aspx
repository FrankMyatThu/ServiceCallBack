<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ConsumerWebForm.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="Button1" runat="server" Text="Connect to Service" OnClick="Button1_Click" />
        <br/>
        <asp:Label ID="Label1" runat="server" Text="..."></asp:Label>
    
    </div>
    </form>
</body>
</html>
