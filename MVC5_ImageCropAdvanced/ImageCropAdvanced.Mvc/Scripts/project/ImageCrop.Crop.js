;
(function(app) {
    var current = app.Crop = {};

    jQuery.extend(app.Crop, 
    {
        CropWidth: 0,
        CropHeight: 0,

        Init: function (cropWidth, cropHeight) {
            current.CropWidth = cropWidth;
            current.CropHeight = cropHeight;

            $('#ButtonClose').click(function () { window.top.location.reload(); });
            $('#ButtonCrop').click(function () { current.SaveCropEventHandler(); });

            var errorMessage = $.trim($('#ErrorMessage').val());

            if (errorMessage.length > 0)
            {
                alertify.alert(errorMessage, function() {
                    window.location.href = Router.action("Home", "Index");
                });
            }
            else
            {
                var imageId = $.trim($('#UploadImage_ID').val());
                var originalImage = $.trim($('#OriginalImage').val());
                var cropImape = $.trim($('#CropImape').val());

                if (imageId.length > 0 && originalImage.length > 0)
                {
                    $('img#Image1').attr('src', originalImage);
                    $('img#Image2').attr('src', originalImage);
                }
            }

            $('img#Image1').imgAreaSelect(
            {
                handles: 'corners',
                aspectRatio: '1:1',
                minHeight: current.CropHeight,
                minWidth: current.CropWidth,
                x1: parseInt($.trim($('#x1').val()), 10),
                y1: parseInt($.trim($('#y1').val()), 10),
                x2: parseInt($.trim($('#x2').val()), 10),
                y2: parseInt($.trim($('#y2').val()), 10),
                onInit: current.Preview,
                onSelectChange: current.Preview
            });
        },

        Preview: function (img, selection) {
            var scaleX = current.CropWidth / selection.width;
            var scaleY = current.CropHeight / selection.height;
            img = new Image();
            img.src = $('#Image1').attr('src');
            var picRealWidth = img.width;
            var picRealHeight = img.height;

            $('#Image1 + div > img').css(
            {
                width: Math.round(scaleX * picRealWidth) + 'px',
                height: Math.round(scaleY * picRealHeight) + 'px',
                marginLeft: '-' + Math.round(scaleX * selection.x1) + 'px',
                marginTop: '-' + Math.round(scaleY * selection.y1) + 'px'
            });

            $('input[name="x1"]').val(selection.x1);
            $('input[name="y1"]').val(selection.y1);
            $('input[name="x2"]').val(selection.x2);
            $('input[name="y2"]').val(selection.y2);
        },

        SaveCropEventHandler: function() {
            var x1 = $('input[name="x1"]').val();
            var x2 = $('input[name="x2"]').val();
            var y1 = $('input[name="y1"]').val();
            var y2 = $('input[name="y2"]').val();

            if (x1.length == 0 && x2.length == 0 && y1.length == 0 && y2.length == 0) {
                alertify.alert('請選擇裁剪區域');
                return false;
            }
            else {
                $.ajax({
                    type: 'post',
                    url: Router.action("Home", "CropImage"),
                    data: { id: $('#UploadImage_ID').val(), x1: x1, x2: x2, y1: y1, y2: y2 },
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (result) {
                        if (result) {
                            if (result.result != 'OK') {
                                alertify.alert(result.msg, function() {
                                    window.location.href = Router.action("Home", "Crop") + '?id=' + $('#UploadImage_ID').val();
                                });                                
                            }
                            else {
                                alertify.confirm('Confirm', '裁剪完成! 是否關閉裁剪功能視窗?', function() {
                                    $('#ButtonClose').trigger('click');
                                },
                                null);
                            }
                            return false;
                        }
                    }
                });
            }
        }

    });
})
(project);