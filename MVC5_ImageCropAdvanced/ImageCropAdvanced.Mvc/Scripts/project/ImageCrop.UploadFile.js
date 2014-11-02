;
(function(app) {
    var current = project.UploadFile = {};

    jQuery.extend(app.UploadFile, {
        Init: function() {
            $('#ButtonClose').click(function () { window.top.location.reload(); });
            $('#ButtonUpload').click(function () { current.UploadEventHandler(); });
            $('#ButtonCrop').hide();
            $('#ButtonCrop').click(function () { current.CropEnevtHandler(); });
            $('#ButtonSave').click(function () { current.SaveEventHandler(); });
            $('#ButtonCancel').click(function () { current.CancelEventHandler(); });
        },

        UploadEventHandler: function() {
            var uploadFile = $('#uploadFile').val();
            if (uploadFile.length == 0) {
                alert('請選擇上傳的檔案');
                return false;
            }
            else {
                $('#FormUpload').submit();
            }
        },

        SaveEventHandler: function() {
            $.ajax(
            {
                url: Router.action("Home", "Save"),
                type: 'post',
                data: { fileName: $('#HiddenFileName').val() },
                cache: false,
                async: false,
                dataType: 'json',
                success: function (result) {
                    if (result) {
                        if (result.result == "Success") {
                            $('#ButtonSave').hide();
                            $('#ButtonCancel').hide();

                            $('#ButtonCrop').show();
                            $('#UploadImage').attr('src', result.msg);
                            $('#Hidden_UploadImageID').val(result.id);
                        }
                        else {
                            $('#ButtonCrop').hide();
                            $('#PanelUploadImage').hide();
                            $('#HiddenFileName').empty();
                            $('#UploadImage').attr('src', '');
                            alert(result.msg);
                        }
                    }
                    return false;
                }
            });
        },

        CancelEventHandler: function() {
            $.ajax(
            {
                url: Router.action("Home", "Cancel"),
                type: 'post',
                data: { fileName: $('#HiddenFileName').val() },
                cache: false,
                async: false,
                dataType: 'json',
                success: function (data) {
                    if (data) {
                        if (data.result != 'Success') {
                            alert(data.msg);
                        }
                        $('#PanelUploadImage').hide();
                        $('#ButtonCrop').hide();
                        $('#HiddenFileName').empty();
                        $('#UploadImage').attr('src', '');
                        $('#UploadImage').attr('width', '');
                        $('#UploadImage').attr('height', '');
                    }
                    return false;
                }
            });
        },

        CropEnevtHandler: function() {
            var imageId = $.trim($('#Hidden_UploadImageID').val());

            if (imageId.length == 0) {
                alert('沒有資料ID編號');
            }
            else {
                window.location.href = Router.action("Home", "Crop") + '?id=' + imageId;
            }
            return false;
        }
    });
})
(project);