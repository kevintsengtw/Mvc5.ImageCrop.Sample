;
(function (app) {
    var current = app.Index = {};

    jQuery.extend(app.Index,
    {
        Init: function () {
            $('#HyperLink_Reload').click(function () { window.location.reload(); });

            $('.DeleteLink').click(function () {
                current.ShowDeleteConfirm($(this).attr('id'));
            });

            $(".default").fancybox({
                autoSize: true,
                openEffect: 'elastic',
                closeEffect: 'elastic'
            });
            $(".various").fancybox({
                maxWidth: 1600,
                maxHeight: 1000,
                fitToView: false,
                width: '95%',
                height: '95%',
                autoSize: false,
                closeClick: false,
                openEffect: 'none',
                closeEffect: 'none'
            });
        },

        ShowDeleteConfirm: function (idValue) {
            alertify.confirm('Confirm', '你確定要刪除這筆資料嗎？', function () {
                $.ajax(
                {
                    type: 'post',
                    url: Router.action('Home', 'Delete'),
                    data: { id: idValue },
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (data) {
                        if (data) {
                            if (data.result != 'OK') {
                                alertify.alert(data.result, function () {
                                    window.location.href = Router.action('Home', 'Index');
                                });
                            }
                            else {
                                $('#tr-' + idValue).remove();
                            }
                        }
                        return false;
                    }
                });
            },
            null);
        }
    });
})
(project);
