/* Copyright © [AUTO:CURRENT_YEAR] Air Shed Systems Inc. All rights reserved.
* www.airshedsystems.com <http://www.airshedsystems.com>
* Notice of License
* Provided to WBEA under license agreement dated 22 December 2010
*/
/*  
<form>
<input id="sort" />
</form>
    
<table id="sortable-index">
<tr>
<th>Location</th>
<th class="no-sort">Parameter</th>
<th>Names</th>
</tr>
</table>
*/

var icon_class_none = 'ui-icon-circle-minus';
//var icon_class_none = 'ui-icon-carat-2-e-w';
var icon_class_asc = 'ui-icon-circle-arrow-n';
var icon_class_desc = 'ui-icon-circle-arrow-s';

$(document).ready(function() {
    var form = $('input#sort').parent('form');

    // reset button
    if (form.find('input[value=Reset]').length == 0 && form.find('input.btnReset').length == 0) {
        form.append('<input class="btnReset" type="button" value="Reset"/>');
    }
    form.find('input[value=Reset],input.btnReset').click(function() {
        /* doesn't work for yes/no dropdowns in IE
        $(this).parent('form').find('input[type!=submit][type!=button],select').val('');
        */
        /* The following works in IE, but not used since it still leaves the form elements as parameters in the URL
        form.find('input[type!=submit][type!=button]').val('');
        form.find('select option:selected').removeAttr('selected');
        form.find('select option:first').attr('selected', 'selected');
        form.submit();
        */
        if (form.attr('action')) {
            document.location = form.attr('action');
        } else {
            document.location = document.location.pathname
        }
    });

    form.find('[type=submit]').click(function() {
        $(this).parent('form').find('input,select').each(function() {
            if ($(this).val() == '') {
                $(this).attr('disabled', 'disabled');
            }
        });
    });

    if (!$('table#sortable-index').length || !$('form input#sort').length) { return; } //do not execute sort script if missing table or sort fields

    var $headings = $('table#sortable-index th[class!=no-sort]');
    $headings.each(function(i, heading) {
        $heading = $(heading);
        if ($heading.hasClass('no-sort')) { return; }

        $heading.disableTextSelect();
        if ($.trim($heading.text()).length) {
            if (!$heading.attr('sort_name')) {
                $heading.attr('sort_name', $.trim($heading.text()).toLowerCase().replace(/\s+/g, '-')); // add a default sort_name
            }
            $heading.wrapInner('<span class="text"/>');
            $heading.append('<a class="ui-icon ui-icon-sort-dir ' + icon_class_none + '" />');
        }
    });

    var sort_list = $('input#sort').val().split('_');
    $.each(sort_list, function(index, sort_item) {
        if (sort_item.indexOf('desc-') == 0) {
            applySortOrder(sort_item.substr(5), 1, index + 1);
        } else {
            applySortOrder(sort_item, 0, index + 1);
        }
    });

    $headings.click(function(ev) {
        var $target = $(ev.target);
        var $this = $(this);
        var sort_name = $this.attr('sort_name');

        if ($target.is('a.ui-icon-sort-dir') || $target.is('a.ui-sort-order')) {
            // clicked on icon / order
            if (!$this.hasClass('sorted')) {
                applySortOrder(sort_name, 0, $('.sorted').length + 1); // add new sort item
            }

            var sort_dir = parseInt($this.attr('sort_dir'));
            if ($target.is('a.ui-icon-sort-dir')) {
                applySortOrder($this.attr('sort_name'), (sort_dir == 1) ? 0 : 1, $this.attr('sort_order'));
            } else {
                applySortOrder($this.attr('sort_name'), sort_dir, 1);
            }
        } else {
            // clicked on text
            if (ev.shiftKey) {
                if ($this.hasClass('sorted')) {
                    applySortOrder(sort_name, null, null); // remove 
                } else {
                    applySortOrder(sort_name, 0, $('.sorted').length + 1); // add new sort item
                }
            } else {
                if ($this.hasClass('sorted') && $('.sorted').length == 1) {
                    var sort_dir = parseInt($this.attr('sort_dir'));
                    if (sort_dir == 0) { // if clicking on an existing sort; go sort desc, asc, no-sort
                        applySortOrder(sort_name, 1, 1);
                    } else {
                        applySortOrder(sort_name, null, null);
                    }
                } else {
                    removeAllSort();
                    applySortOrder(sort_name, 0, 1); // reset; and put this as the only sort item
                }
            }
        }

        var form = $('input#sort').parent('form');
        form.find('input,select').each(function() {
            if ($(this).val() == '') {
                $(this).removeAttr('name'); // remove empty input boxes with no value
            }
        });
        form.submit(); //resubmit search
    });
});

function removeAllSort() {
    $sort = $('.sorted');
    $sort.removeClass('sorted').removeAttr('sort_dir').removeAttr('sort_order');
    $sort.find('a.ui-icon-sort-dir').removeClass(icon_class_asc).removeClass(icon_class_desc).addClass(icon_class_none);
    $sort.find('a.ui-sort-order').remove();
    $('form input#sort').val('');
}

function applySortOrder(sort_item, sort_dir, sort_order) {
    $heading = $('[sort_name=' + sort_item + ']');

    // add sort attributes
    if (sort_dir == 0 || sort_dir == 1) {
        $heading.addClass('sorted');
        $heading.attr('sort_dir', sort_dir);
        var this_order = $heading.attr('sort_order');
        if (sort_order != this_order) {
            $('.sorted').each(function(i, otherheading) {
                var other_order = parseInt($(otherheading).attr('sort_order'));
                if (other_order <= this_order) {
                    $(otherheading).attr('sort_order', other_order + 1);
                }
            });
        }
        $heading.attr('sort_order', sort_order);
    } else {
        var removed_order = parseInt($heading.attr('sort_order'));
        $('.sorted').each(function(i, otherheading) {
            var other_order = parseInt($(otherheading).attr('sort_order'));
            if (other_order > removed_order) {
                $(otherheading).attr('sort_order', other_order - 1);
            }
        });
        $heading.removeClass('sorted').removeAttr('sort_dir').removeAttr('sort_order');
    }

    // add sort asc/desc icon
    var $sort_dir_icon = $heading.find('a.ui-icon-sort-dir');
    $sort_dir_icon.removeClass(icon_class_none).removeClass(icon_class_asc).removeClass(icon_class_desc);
    switch (sort_dir) {
        case 0: $sort_dir_icon.addClass(icon_class_asc); break;
        case 1: $sort_dir_icon.addClass(icon_class_desc); break;
        default: $sort_dir_icon.addClass(icon_class_none); break;
    }

    // update all displayed sort numbers
    $('a.ui-sort-order').remove();
    if ($('th.sorted').length > 1) {
        $('th.sorted').each(function(i, heading) {
            $(heading).append('<a class="ui-sort-order">' + $(heading).attr('sort_order') + '</a>');
        });
    }

    // update sort form field
    var sort_count = $('.sorted').length;
    var sort_list = new Array(sort_count);
    for (var i = 1; i <= sort_count; i++) {
        $sort_item = $('[sort_order=' + i + ']')
        if ($sort_item.attr('sort_dir') == 0) {
            sort_list[i - 1] = $sort_item.attr('sort_name');
        } else {
            sort_list[i - 1] = 'desc-' + $sort_item.attr('sort_name');
        }
    }
    $('input#sort').val(sort_list.join('_'));

}
