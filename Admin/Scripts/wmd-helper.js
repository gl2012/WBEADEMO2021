/* Copyright © [AUTO:CURRENT_YEAR] Air Shed Systems Inc. All rights reserved.
* www.airshedsystems.com <http://www.airshedsystems.com>
* Notice of License
* Provided to WBEA under license agreement dated 22 December 2010
*/
// WBEADMS-helper for datetime pickers

$(document).ready(function() {
    $('.inputHours, .inputMins').change(function() {
        AddLeadingZero(this);
    });

    $('.inputHours').keyup(function() {
        RemoveSpaceColon(this);
        ValidateHr(this);
        UpdateHiddenDateTime($(this).parent());
        if (!IsErrorClass(this) && $(this).val().length == 2) {
            $(this).parent().find('.inputMins').select();
        }
    });

    $('.inputMins').keyup(function() {
        RemoveSpaceColon(this);
        ValidateMin(this);
        UpdateHiddenDateTime($(this).parent());
    });

    $('span.datetimepicker input.datepicker').change(function() {
        var parent = $(this).parent();
        var hours = parent.find('.inputHours');
        var minutes = parent.find('.inputMins');

        if ($(this).val() == '') {
            hours.val('');
            minutes.val('');
        } else {
            if (hours.val() == '') { hours.val('00'); }
            if (minutes.val() == '') { minutes.val('00'); }

            if ($(this).val().match(/\d{4}-\d{1,2}-\d{1,2}/)) {
                RemoveErrorClass(this);
            } else {
                AddErrorClass(this);
            }
        }

        hours.focus();
        hours.select();

        UpdateHiddenDateTime(parent);
    });

    $('span.datetimepicker').each(function() {
        ValidateBlanks(this); // on load, highlight missing fields
        if ($(this).find('.inputHidden').hasClass('input-validation-error')) {
            $(this).addClass('input-validation-error'); // on load, highlight span if the field has error
        }
    });
});

function ValidateHr(obj) {
    var input = $(obj);
    var val = input.val();
    var relatedMin = input.parent().find('.inputMins').val();

    if (val == '') { return; }

    var hour = parseInt(val);
    if (val != hour || hour > 23 || hour < 0) {
        input.addClass('input-validation-error');
    } else {
        input.removeClass('input-validation-error');
    }
}

function ValidateMin(obj) {
    var input = $(obj);
    var val = input.val();
    var relatedHr = input.parent().find('.inputHours').val();

    if (val == '') { return; }

    var mins = parseInt(val);
    if (val != mins || mins > 59 || mins < 0) {
        input.addClass('input-validation-error');
    } else {
        input.removeClass('input-validation-error');
    }
}

function ValidateBlanks(span) {
    var date = $(span).find('input.datepicker');
    var hr = $(span).find('input.inputHours');
    var min = $(span).find('input.inputMins');

    if (date.val() + hr.val() + min.val() == '') {
        RemoveErrorClass(date);
        RemoveErrorClass(hr);
        RemoveErrorClass(min);
    } else {
        if (date.val() == '') { AddErrorClass(date); }
        if (hr.val() == '') { AddErrorClass(hr); }
        if (min.val() == '') { AddErrorClass(min); }
    }
}

function UpdateHiddenDateTime(span) {
    ValidateBlanks(span);

    var dateVal = $(span).find('input.hasDatepicker').val();
    var hrVal = $(span).find('input.inputHours').val();
    var minVal = $(span).find('input.inputMins').val();

    var hidden = $(span).find('input.inputHidden');
    if (dateVal + hrVal + minVal == '') {
        hidden.val('');
    } else {
        hidden.val(dateVal + ' ' + hrVal + ':' + minVal);
    }
}

function AddLeadingZero(obj) {
    var val = $(obj).val();
    if (val.length == 1) {
        $(obj).val('0' + val);
    }
}

function RemoveSpaceColon(obj) {
    var val = $(obj).val();
    val = val.replace(/[: a-zA-Z]/, '');
    $(obj).val(val);
}

function RemoveErrorClass(obj) {
    $(obj).removeClass('input-validation-error');
}

function AddErrorClass(obj) {
    $(obj).addClass('input-validation-error');
}

function IsErrorClass(obj) {
    return $(obj).hasClass('input-validation-error');
}