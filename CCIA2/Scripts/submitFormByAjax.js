function submitFormByAjax(formObject) {
    formObject.submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: $(this).attr("action"),
            type: $(this).attr("method"),
            processData: false,
            contentType: false,
            data: new FormData(this),
            success: function (res) {
                if (res.success) {
                    alert(res.message);
                    //$('#div-bPopupWin').bPopup().close(); //不要用這個, 會無法觸發原本的onClose事件
                    $("#btn-close-bPopupWin").trigger("click");
                } else {
                    if (typeof (res.errorMessage) != 'undefined') {
                        alert(res.errorMessage);
                    }
                    if (typeof (res.ModelStateErrors) != 'undefined') {
                        $.each(res.ModelStateErrors, function (key, value) {
                            //alert(key + ":" + value[0]);
                            if (value != null) {
                                var container = $('span[data-valmsg-for="' + key + '"]');
                                container.removeClass("field-validation-valid").addClass("field-validation-error");
                                container.html(value[0]);
                            }
                        });
                    }
                }
                return false
            }
        });
    });
}