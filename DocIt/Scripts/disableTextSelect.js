/* 
* Property of WBEA 2013 
*/
$.extend($.fn.disableTextSelect = function() {
    return this.each(function () {
        if ($.browser.mozilla) {//Firefox
            $(this).css('MozUserSelect', 'none');
        } else if ($.browser.msie) {//IE
            $(this).bind('selectstart', function () { return false; });
        } else {//Opera, etc.
            $(this).mousedown(function () { return false; });
        }
    });
});