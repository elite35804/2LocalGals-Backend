function JsFormValueChanged(field) {
    //field.style.backgroundColor = "#FFFFE6";
    field.style.color = "Blue";
}

function JsStringToTime(value) {
    var ret = new Date("1/1/1900 " + value);
    return ret;
}

function JsTimeToString(value) {
    var hrs = value.getHours();
    var min = value.getMinutes();
    var ap = hrs > 11 ? "PM" : "AM";
    if (hrs > 12) hrs = hrs - 12;
    if (hrs == 0) hrs = 12;
    if (min < 10) min = "0" + min;
    return hrs + ":" + min + " " + ap;
}

function JsFormatMoney(value) {
    if (isNaN(value)) value = 0;
    return value < 0 ? "-$" + (-value).toFixed(2) : "$" + value.toFixed(2);
}

function JsFormMoneyChanged(field) {
    var money = parseFloat(field.value.replace("$", ""));
    field.value = JsFormatMoney(money);
    JsFormValueChanged(field);
}

function JsFormHoursChanged(field) {
    var hours = parseFloat(field.value);
    if (isNaN(hours)) hours = 0;
    field.value = hours.toFixed(2);
    JsFormValueChanged(field);
}

function JsFormPercentChanged(field) {
    var percent = parseFloat(field.value.replace("%", ""));
    if (isNaN(percent)) percent = 0;
    field.value = percent.toFixed(0) + ' %';
    JsFormValueChanged(field);
}

function JsFormPercentNoRoundChanged(field) {
    var percent = parseFloat(field.value.replace("%", ""));
    if (isNaN(percent)) percent = 0;
    field.value = percent.toFixed(3) + ' %';
    JsFormValueChanged(field);
}

function JsSetScrollPos(field) {
    document.getElementById('hiddenScrollPos').value = window.pageYOffset || document.documentElement.scrollTop;
}

function JsSetCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function JsGetCookie(c_name) {
    var c_value = document.cookie;
    var c_start = c_value.indexOf(" " + c_name + "=");
    if (c_start == -1) {
        c_start = c_value.indexOf(c_name + "=");
    }
    if (c_start == -1) {
        c_value = null;
    }
    else {
        c_start = c_value.indexOf("=", c_start) + 1;
        var c_end = c_value.indexOf(";", c_start);
        if (c_end == -1) {
            c_end = c_value.length;
        }
        c_value = unescape(c_value.substring(c_start, c_end));
    }
    return c_value;
}

function JsGetParameter(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
