/* Copyright © [AUTO:CURRENT_YEAR] Air Shed Systems Inc. All rights reserved.
* www.airshedsystems.com <http://www.airshedsystems.com>
* Notice of License
* Provided to WBEA under license agreement dated 22 December 2010
*/
// initialize: global variables
var autoSaveTimeout = 0;
var changeFlag = false;
var changedFields = new Object(); // associative array
var AUTOSAVE_WAIT_SECS = 180;
var AUTOSAVE_DISPLAY_SECS = 60;

$(document).ready(function() {
    InitialzeFormForAutoSave();

    // check if anything changed on key press
    $('input[type=text], input[type=checkbox], input[type=radio], select').keyup(function() {
        CheckFormForChange(this);
    });

    // check if anything changed on click
    $('input[type=text], input[type=checkbox], input[type=radio], select').click(function() {
        changeFlag = true;
        ResetAutoSaveTimeout();
        setTimeout("CheckFormForChange($('#" + $(this).attr('id') + "').get(0))", 15000);
    });
});

// initialize: save old values
function InitialzeFormForAutoSave() {
    $('input[type=text], input[type=checkbox], input[type=radio], select').each(function() {
            $(this).attr('auto_save_old_value', $(this).val());
    });
}

// check if anything changed; also reset autosave timeout due to action
function CheckFormForChange(obj) {
    if ($(obj).attr('no_autosave')) { return false; } // ignore fields flagged with no_autosave
    
    var objName = $(obj).attr('name');
    if ($(obj).val() != $(obj).attr('auto_save_old_value')) {
        // set flag to true; and autosave if field changes
        changeFlag = true;
        changedFields[objName] = obj;
    } else {
        // set flag to false; and do not autosave if fields are no longer different
        changeFlag = false;
        changedFields[objName] = null;
        for (key in changedFields) {
            if (changedFields[key] != null) {
                changeFlag = true;
                break;
            }
        }
    }
    //SetFloatingFixedNotice('debug: flag set to ' + changeFlag + ' at [' + new Date() + ']');

    ResetAutoSaveTimeout();
}

// hide floating notice; reset timeout duration
function ResetAutoSaveTimeout() {
    $('#floating-fixed-notice').hide('slow');
    clearTimeout(autoSaveTimeout);

    if (!changeFlag) { return; }

    autoSaveTimeout = setTimeout("AutoSaveNotice(" + AUTOSAVE_DISPLAY_SECS + ");", AUTOSAVE_WAIT_SECS * 1000);
}

// show floating notice; perform countdown
function AutoSaveNotice(secondsLeft) {
    if (secondsLeft > 2) {
        SetFloatingFixedNotice('Notice: Autosaving due to inactivity in ' + secondsLeft + ' seconds.');
        if (secondsLeft % 4 == 0) { $('#floating-fixed-notice').effect('highlight', {}, 3000); }
        autoSaveTimeout = setTimeout('AutoSaveNotice(' + (secondsLeft - 1) + ');', 1000);
    } else if (secondsLeft > 0) {
        SetFloatingFixedNotice('Warning: Autosaving due to inactivity.');
        autoSaveTimeout = setTimeout('AutoSaveNotice(0);', 2000);
    } else {
        SetFloatingFixedNotice('Please wait while your form is being saved...');
        $('#floating-fixed-notice').effect('highlight', {}, 1000);
        clearTimeout(autoSaveTimeout);

        // perform autosave
        $('#auto_save').val('1');
        $('input[value=Save]').click(); // TODO: autosave via ajax.

        // successful autosave via ajax
        /*
        InitialzeFormForAutoSave();
        SuccessfulAutoSaveNotice();
        */
    }
}

// show successful autosave message
function SuccessfulAutoSaveNotice() {
    var currentTime = new Date();
    var year = currentTime.getFullYear(); var month = AddLeadingZeroInt(currentTime.getMonth() + 1); var day = AddLeadingZeroInt(currentTime.getDate());
    var hour = AddLeadingZeroInt(currentTime.getHours()); var minute = AddLeadingZeroInt(currentTime.getMinutes());
    SetFloatingFixedNotice('Autosave complete at ' + year + '-' + month + '-' + day + ' ' + hour + ':' + minute);
    clearTimeout(autoSaveTimeout);
    changeFlag = false;
}

// set a message to floating notice
function SetFloatingFixedNotice(html) {
    if (!$('#floating-fixed-notice').length) {
        // TODO: move the styles to site.css
        var floatingFixedNoticeHtml = '<div style="position:fixed; width:100%; z-index:10001;"><div id="floating-fixed-notice" class="ui-state-default" style="float:right; background:white; text-align:right; width:40%;">NOTICE</div></div>';
        $('body').eq(0).prepend(floatingFixedNoticeHtml);
    }
    $('#floating-fixed-notice').html(html).show('slow');
}


function AddLeadingZeroInt(val) {
    if (val < 10) {
        return '0' + val;
    }
    return val;
}