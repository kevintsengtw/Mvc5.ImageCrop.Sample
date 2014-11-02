<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Crop.aspx.cs" Inherits="ImageCrop.WebForm.Crop" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Image Crop</title>
    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <link href="Content/ImageAreaSelect/imgareaselect-default.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="row">
            <div class="col-md-12">
                <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="回到首頁" OnClick="Button1_Click" />
                <asp:Button ID="Button2" class="btn btn-success" runat="server" Text="裁剪相片" OnClick="Button2_Click" />
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-md-12">
                <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <asp:Image ID="Image3" runat="server" />
                    <hr />
                </asp:Panel>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
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
        </div>
    </div>
    </form>
    <script src="Scripts/jquery-2.1.1.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.imgareaselect.pack.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('img#Image1').imgAreaSelect(
            {
                handles: 'corners',
                aspectRatio: '1:1',
                x1: 0, y1: 0, x2: 100, y2: 100,
                onSelectChange: preview
            });
        });

        function preview(img, selection) {
            var scaleX = 100 / selection.width;
            var scaleY = 100 / selection.height;
            img = new Image();
            img.src = $('#Image1').attr('src');
            var picRealWidth = img.width;
            var picRealHeight = img.height;

            $('#Image1 + div > img').css({
                width: Math.round(scaleX * picRealWidth) + 'px',
                height: Math.round(scaleY * picRealHeight) + 'px',
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
