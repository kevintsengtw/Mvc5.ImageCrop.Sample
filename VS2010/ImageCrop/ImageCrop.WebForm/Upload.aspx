<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="ImageCrop.WebForm.Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
	
			<asp:Button ID="Button_Default" runat="server" onclick="Button_Default_Click" Text="回到首頁" />
			<br />
		<hr />
	
		圖片檔案：<asp:FileUpload ID="FileUpload1" runat="server" />
		<asp:Button ID="Button_Upload" runat="server" onclick="Button_Upload_Click" Text="上傳圖片" />
		<br />
		<hr />
		<asp:Panel ID="Panel1" runat="server">
			<asp:Button ID="Button_Save" runat="server" onclick="Button_Save_Click" Text="儲存圖片" />
			<asp:Button ID="Button_Cancel" runat="server" onclick="Button_Cancel_Click" Text="取消儲存" />
			<asp:Button ID="Button_Crop" runat="server" onclick="Button_Crop_Click" Text="裁剪圖片" Visible="False" />
			<asp:HiddenField ID="HiddenField_ID" runat="server" />
			<br />
			<asp:Image ID="Image_Upload" runat="server" />
		</asp:Panel>
	
	</div>
	</form>
</body>
</html>