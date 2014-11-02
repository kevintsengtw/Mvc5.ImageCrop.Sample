<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Crop.aspx.cs" Inherits="ImageCrop.WebForm.Crop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head id="Head1" runat="server">
		<title>Image Crop</title>
		<link rel="stylesheet" type="text/css" href="css/imgareaselect-default.css" />
	</head>
	<body>
		<form id="form1" runat="server">
			<div>
				<asp:Button ID="Button1" runat="server" Text="回到首頁" onclick="Button1_Click" />
				<asp:Button ID="Button2" runat="server" Text="裁剪相片" onclick="Button2_Click" />
				<asp:Panel ID="Panel1" runat="server" Visible="false">
					<asp:Image ID="Image3" runat="server" />
					<hr />
				</asp:Panel>
				<div align="center">
					<img id="Image1" runat="server" style="float: left; margin-right: 10px;" alt="Create Thumbnail" />
					<div style="float: left; position: relative; overflow: hidden; width: 100px; height: 100px;">
						<img id="Image2" runat="server" style="position: relative;" alt="Thumbnail Preview" />
					</div>
				</div>
				<input type="hidden" id="x1" name="x1" value="" runat="server" />
				<input type="hidden" id="y1" name="y1" value="" runat="server" />
				<input type="Hidden" id="x2" name="x2" value="" runat="server" />
				<input type="hidden" id="y2" name="y2" value="" runat="server" />
			</div>
		</form>

		<script type="text/javascript" src="Scripts/jquery-1.7.1.min.js"></script>
		<script type="text/javascript" src="Scripts/jquery.imgareaselect.pack.js"></script>
		<script type="text/javascript">

			$(document).ready(function ()
			{
				$('img#Image1').imgAreaSelect(
				{
					handles: 'corners',
					aspectRatio: '1:1',
					x1: 0, y1: 0, x2: 100, y2: 100,
					onSelectChange: preview
				});
			});

			function preview(img, selection)
			{
				var scaleX = 100 / selection.width;
				var scaleY = 100 / selection.height;

				var img = new Image();
				img.src = $('#Image1').attr('src');
				var pic_real_width = img.width;
				var pic_real_height = img.height;

				$('#Image1 + div > img').css({
					width: Math.round(scaleX * pic_real_width) + 'px',
					height: Math.round(scaleY * pic_real_height) + 'px',
					marginLeft: '-' + Math.round(scaleX * selection.x1) + 'px',
					marginTop: '-' + Math.round(scaleY * selection.y1) + 'px'
				});
				$('input[name="x1"]').val(selection.x1);
				$('input[name="y1"]').val(selection.y1);
				$('input[name="x2"]').val(selection.x2);
				$('input[name="y2"]').val(selection.y2);
			} 
		</script>
	</body>
</html>