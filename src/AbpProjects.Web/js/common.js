/* ========== Other All Common function on Ajax Success Start========== */

function funCustomCommon() {

    funAllow_Alpha("allow_alpha");
    funallow_Alpha_Number("allow_alpha_number");
    funallow_numeric("allow_numeric");
    funallow_decimal("allow_decimal");
    funallow_decimal_Minus("allow_decimal_Minus");
   
}
function funallow_numeric(clsallow_numeric) {
    $("." + clsallow_numeric).bind("keypress", function (evt) {
        if (event.charCode != 0) {
            if (event.charCode == 13) { return true; }
            else {
                var regex = new RegExp("^[0-9]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            }
        }
    });

    $("." + clsallow_numeric).bind('paste', function (e) { //Attach paste event handler for all inputs
        var pastedText = e.originalEvent.clipboardData.getData('text');
        if (/^[0-9]*$/.test(pastedText) == false) { return false; }
    });
}


function funallow_decimal(clsallow_decimal) {
    $("." + clsallow_decimal).bind("keypress", function (evt) {
        if (event.charCode != 0) {
            if (event.charCode == 13) { return true; }
            else {
                var regex = new RegExp("^[0-9.]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                var str = $(this).val();
                if ((str.indexOf('.') !== -1) && key == '.') {
                    event.preventDefault();
                    return false;
                }
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            }
        }
    });
    $("." + clsallow_decimal).bind('paste', function (e) { //Attach paste event handler for all inputs
        var pastedText = e.originalEvent.clipboardData.getData('text');
        if (/^[0-9.]*$/.test(pastedText) == false) { return false; }
    });
}

//Allow Numeric, dot and Minus 
function funallow_decimal_Minus(clsallow_decimal) {
    $("." + clsallow_decimal).bind("keypress", function (evt) {
        if (event.charCode != 0) {
            if (event.charCode == 13) { return true; }
            else {
                var regex = new RegExp("^[0-9.-]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            }
        }
    });
    $("." + clsallow_decimal).bind('paste', function (e) { //Attach paste event handler for all inputs
        var pastedText = e.originalEvent.clipboardData.getData('text');
        if (/^[0-9.]*$/.test(pastedText) == false) { return false; }
    });
}

//Allow only Alpha, Space, Back Space, F5, Shift, Enter
function funAllow_Alpha(clsallow_alpha) {
    $('.' + clsallow_alpha).bind('keypress', function (e) {

        if (event.charCode != 0) {
            if (event.charCode == 13) { return true; }
            else {
                var regex = new RegExp("^[a-zA-Z ]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            }
        }
    });

    $("." + clsallow_alpha).bind('paste', function (e) { //Attach paste event handler for all inputs
        var pastedText = e.originalEvent.clipboardData.getData('text');
        if (/^[a-zA-Z ]*$/.test(pastedText) == false) { return false; }
    });
}
//Allow only Alpha,Number, Space, Back Space, F5,  Enter
function funallow_Alpha_Number(clsallow_Alpna_number) {

    $('.' + clsallow_Alpna_number).bind('keypress', function (e) {

        if (event.charCode != 0) {
            if (event.charCode == 13) { return true; }
            else {
                var regex = new RegExp("^\d*[a-zA-Z\-\s]{1,}\d*");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            }
        }
    });
    $("." + clsallow_Alpna_number).bind('paste', function (e) { //Attach paste event handler for all inputs
        var pastedText = e.originalEvent.clipboardData.getData('text');
        if (/^[a-zA-Z0-9\n ]*$/.test(pastedText) == false) { return false; }
    });
}
function funallow_numeric_Phone(clsallow_numeric) {
    $("." + clsallow_numeric).bind("keypress", function (evt) {
        if (event.charCode != 0) {
            if (event.charCode == 13) { return true; }
            else {
                var regex = new RegExp("^[+0-9 ]+$");
                var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
                if (!regex.test(key)) {
                    event.preventDefault();
                    return false;
                }
            }
        }
    });
    $("." + clsallow_numeric).bind('paste', function (e) { //Attach paste event handler for all inputs
        var pastedText = e.originalEvent.clipboardData.getData('text');
        if (/^[+0-9 ]*$/.test(pastedText) == false) { return false; }
    });
}


function funallow_verfiyPanNo(val) {
    var regpan = /^[A-Za-z]{3}[cphfatbljCPHFATBLJ]{1}[A-Za-z]{1}[0-9]{4}[A-Za-z]$/;
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regpan.test(val)) {
        //event.preventDefault();
        return false;
    }
    else {
        //abp.notify.info("valid panno!!!");
        return true;
    }
}



function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
    if (pattern.test(emailAddress)) {
        return true;
    }
    else { return false; }
}

function isValidURL(string) {
    var res = string.match(/(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/g);
    return (res !== null)
};





