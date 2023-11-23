(function () {
    $(function () {
        var $form = $('.pass-reset-form');
        $form.validate();

        $form.submit(function (e) {
            e.preventDefault();

            if (!$form.valid()) {
                return;
            }

            console.log($form.serialize());

            abp.ui.setBusy(
                null,
                abp.ajax({
                    contentType: 'application/x-www-form-urlencoded',
                    url: $form.attr('action'),
                    data: $form.serialize()
                }).done(function (res) {
                    if (res == "fail") {
                        abp.message.error("Password not match", "Error")

                    }
                    else {
                        abp.message.success("Password reset successfully...!", "Mail sent")
                            .done(function () {
                                location.href = abp.appPath + 'Account/Login';
                            });
                    }
                })
            );
        });
    });

})();