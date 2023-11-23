(function () {

    $(function () {
        var $form = $('.forget-form');

        $form.validate();

        $form.submit(function (e) {
            e.preventDefault();

            if (!$form.valid()) {
                return;
            }

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: 'application/x-www-form-urlencoded',
                    url: $form.attr('action'),
                    data: $form.serialize()
                }).done(function () {
                    abp.message.success("A password reset link sent to your e-mail address. Please check your emails.", "Mail sent")
                        .done(function () {
                            location.href = abp.appPath + 'Account/Login';
                        });
                })
            );
        });
    });

})();