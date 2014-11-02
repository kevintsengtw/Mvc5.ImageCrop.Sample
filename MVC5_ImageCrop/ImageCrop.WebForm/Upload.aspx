<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="ImageCrop.WebForm.Upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" /> 
    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <title>Upload File</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="row">
            <div class="col-md-2">
                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="filestyle" data-buttontext="選擇圖檔" />
            </div>
            <div class="col-md-4">
                <asp:Button ID="Button_Upload" runat="server" OnClick="Button_Upload_Click" Text="上傳圖片" CssClass="btn btn-success" />
                <asp:Button ID="Button_Default" runat="server" OnClick="Button_Default_Click" Text="回到首頁" CssClass="btn btn-primary" />
            </div>
        </div>
        <br />
        <hr />
        <asp:Panel ID="Panel1" runat="server">
            <asp:Button ID="Button_Save" runat="server" OnClick="Button_Save_Click" Text="儲存圖片" CssClass="btn btn-info" />
            <asp:Button ID="Button_Cancel" runat="server" OnClick="Button_Cancel_Click" Text="取消儲存" CssClass="btn btn-default" />
            <asp:Button ID="Button_Crop" runat="server" OnClick="Button_Crop_Click" Text="裁剪圖片" Visible="False" CssClass="btn btn-warning" />
            <asp:HiddenField ID="HiddenField_ID" runat="server" />
            <p></p>
            <asp:Image ID="Image_Upload" runat="server" />
        </asp:Panel>
    </div>
    </form>  
    <script src="Scripts/jquery-2.1.1.min.js"></script>
    <script src="Scripts/bootstrap-filestyle.js"></script>
</body>
</html>
