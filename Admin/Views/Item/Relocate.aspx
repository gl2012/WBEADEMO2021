<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="WBEADMS" %>
<%@ Import Namespace="WBEADMS.Views.ItemHelpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Relocate
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="title-icon icon-items-move">Relocate Items</h2>
    
    <% using (Html.BeginForm()) {%>
    
    <fieldset id="items_draggable_frame"><legend>Dispose / Relocate Items</legend>
        <table><tr>
            <td><label for="date_move">Date Moved:</label></td>
            <td><%= Html.DatePicker("date_moved") %></td>
            <td><span id="date_moved_validation" class="field-validation-error"></span></td>
        </tr></table>
        <div id="left_items" class="drag_list ui-widget-content">
            <%= Html.DropDownList("left_location")%>
            <ul>
            <% foreach(var item in (WBEADMS.Models.Item[])(ViewData["left_items"])) { %>
                <li item_id="<%= item.id %>" item_name="<%= item.name %>" item_date="<%= item.DateInstalled %>"><%= Html.ItemListedName(item) %></li>
            <% } %>
            </ul>
        </div>
        
        <div id="right_items" class="drag_list ui-widget-content">
            <%= Html.DropDownList("right_location", "--Disposal--") %>
        </div>
        
        <div id="item_info">
        <% var items = new List<WBEADMS.Models.Item>();
           items.AddRange((WBEADMS.Models.Item[])ViewData["left_items"]);
           items.AddRange((WBEADMS.Models.Item[])ViewData["right_items"]);
           foreach (var item in items) {
               var hists = WBEADMS.Models.ItemHistory.GetItemHistorys(item.id);
        %>
            <div item_id="<%= item.id %>">
                <p><%= item.name %></p>
                <ul>
                <% foreach (var hist in hists) { %>
                <li>
                    <%= hist.Location.name %> : <%= hist.DateInstalled%> ~ <%= hist.DateRemoved%>
                </li>
                <% } %>
                </ul>
           </div>
        <% } %>
        </div>
    </fieldset>

    <input id="left_ids" name="left_ids" type="hidden" />
    <input id="right_ids" name="right_ids" type="hidden" />
    
    <input id="submit" type="submit" value="Save Changes" disabled="disabled" onclick="$('select').removeAttr('disabled');"/>
    <% } %>
    
    <script type="text/javascript">
<% 
    string item_id = Page.Request.QueryString["item_id"]; 
    if (!String.IsNullOrEmpty(item_id)) { %>
    $('[item_id=<%= item_id %>]').addClass('selected');
<% } %>
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AboveContent" runat="server">
<style type="text/css">
    #items_draggable_frame { padding: 2em; }
    
    #left_items { float: left; width: 370px; min-height: 22em; margin-right: 2em;} * html #left_items { height: 18em; } /* IE6 */
	#left_items.custom-state-active { background: #eee; }

	#right_items { float: left; width: 370px; min-height: 22em; margin-right: 2em;} * html #right_items { height: 18em; } /* IE6 */
	
	.drag_list ul { margin: 10px; padding: 0px; }
	.drag_list li a { float: left; margin-top: 1px;}
	.drag_list li { list-style-type:none; padding: 5px 5px; cursor: default; border: solid 1px white; }
	.drag_list li.selected { border: solid 1px #5C87B2; background-color: #A6C9E2; }
	.drag_list li.changed { /* background-color: #B7DAF3; */  font-weight: bold; }
	.drag_list li.hover { border: solid 1px black; background-color: #F0F0F0; }
	a.ui-icon { cursor: pointer; }
	
	#item_info { float: left; max-width: 225px;}
	#item_info div { border: solid 1px #5C87B2; padding: 1em;}
	#item_info ul { padding-left: 1em; }
	
</style>
<script type="text/javascript">
    // icon links
    var left_icon = '<a title="Move item left" class="ui-icon ui-icon-arrowthick-1-w">Move Item</a>';
    var right_icon = '<a title="Move item right" class="ui-icon ui-icon-arrowthick-1-e">Move Item</a>';
    var return_left_icon = '<a title="Undo" class="ui-icon ui-icon-arrowreturnthick-1-w">Undo</a>';
    var return_right_icon = '<a title="Undo" class="ui-icon ui-icon-arrowreturnthick-1-e">Undo</a>';
    var trash_icon = '<a title="Consume Item" class="ui-icon ui-icon-trash">Consume Item</a>';

    $(function() {
        $('#left_location').val('<%= (string)((SelectList)ViewData["left_location"]).SelectedValue %>');
        $('#right_location').val('<%= (string)((SelectList)ViewData["right_location"]).SelectedValue %>');

        // item history stuff
        $('#item_info div[item_id]').hide();

        // dropable areas
        makeDropable();
        function makeDropable() {
            $('#right_items').droppable({
                accept: '#left_items li',
                activeClass: 'ui-state-highlight',
                drop: function(ev, ui) {
                    dropRight(ui.draggable);
                }
            });
            $('#left_items').droppable({
                accept: '#right_items li',
                activeClass: 'custom-state-active',
                drop: function(ev, ui) {
                    dropLeft(ui.draggable);
                }
            });
        }

        // create right list
        if (!$('ul', '#right_items').length) { $('<ul/>').appendTo('#right_items'); }

        // list items
        setupListItems('left');
        function setupListItems(side) {
            var $list = $('li', '#' + side + '_items');

            // draggable items
            $list.draggable({
                cancel: 'a.ui-icon', // clicking will not initiate dragging
                revert: 'invalid', // when not dropped, the item will revert back to its initial state
                containment: $('#items_draggable_frame').length ? '#items_draggable_frame' : 'document',
                helper: 'clone',
                cursor: 'move'
            });

            // assign icons and statuses to existing items
            if (side == 'left') {
                if ($('#right_location').val()) {
                    $list.prepend(right_icon);
                } else {
                    $list.prepend(trash_icon);
                }
            } else {
                $list.prepend(left_icon);
            }
            $list.addClass(side + '_item').disableTextSelect();

            // resolve clicks on item
            $list.click(function(ev) {
                $('#item_info div').hide();
                $('.drag_list li').removeClass('hover');

                var $item = $(this);
                var $target = $(ev.target);

                if ($target.is('a.ui-icon')) {
                    if ($item.parents('#left_items').length) {
                        dropRight($item);
                    } else {
                        dropLeft($item);
                    }
                    $('#item_info div[item_id]').slideUp(); // this is a hack to resolve bug: on clicked drop action; the hover-off doesn't occur
                }

                return false;
            });

            // add history
            $('#item_info div[item_id]').hide();
            $list.each(function(i, listitem) {
                var item_id = $(listitem).attr('item_id');
                var item_name = $(listitem).attr('item_name');
                if ($('#item_info div[item_id=' + item_id + ']').length) { return; } // don't add hist if already exists

                $.ajax({
                    type: 'GET',
                    url: '<%= Url.RouteUrl(new {action ="GetItemHistory"}) %>',
                    data: { item_id: item_id },
                    item_id: item_id,
                    item_name: item_name,
                    success: function(hists) {
                        $('<div item_id="' + item_id + '"><p>' + item_name + '</p><ul/>').hide().appendTo('#item_info');
                        $.each(eval(hists), function(i, v) {
                            $('<li>' + v.location + ' : ' + v.install_date + ' ~ ' + v.remove_date + '</li>').appendTo('#item_info div[item_id=' + item_id + '] ul');
                        });
                    }
                });
            });

            // add hover
            $list.hover(
            function() {
                $(this).addClass('hover');
                var item_id = $(this).attr('item_id');
                $('#item_info div[item_id=' + item_id + ']').show();
            },
            function() {
                $(this).removeClass('hover');
                var item_id = $(this).attr('item_id');
                $('#item_info div[item_id=' + item_id + ']').hide();
            });
        }

        // dropping item on right area
        function dropRight($item) {
            $item.fadeOut(function() {
                $item.find('a.ui-icon').remove();
                if ($item.hasClass('left_item')) {
                    $item.prepend(return_left_icon);
                } else {
                    $item.prepend(left_icon);
                }
                var $list = $('ul', '#right_items'); // use class? gallery ui-helper-reset
                $item.appendTo($list).fadeIn();
                updateLists();
                checkChanges();
            });
        }

        // dropping item on left area
        function dropLeft($item) {
            $item.fadeOut(function() {
                $item.find('a.ui-icon').remove();
                if (!$('#right_location').val()) {
                    $item.prepend(trash_icon);
                } else if ($item.hasClass('right_item')) {
                    $item.prepend(return_right_icon);
                } else {
                    $item.prepend(right_icon);
                }
                var $list = $('ul', '#left_items');
                $item.appendTo($list).fadeIn();
                updateLists();
                checkChanges();
            });
        }

        // update form fields for left & right submission
        updateLists();
        function updateLists() {
            $left_ids = $('#left_ids');
            $right_ids = $('#right_ids');

            $left_ids.val('');
            $('#left_items li').each(function(i, item) {
                $left_ids.val($left_ids.val() + ' ' + $(item).attr('item_id'));
            });

            $right_ids.val('');
            $('#right_items li').each(function(i, item) {
                $right_ids.val($right_ids.val() + ' ' + $(item).attr('item_id'));
            });
        }

        // check date_moved
        $('#date_moved').change(function() {
            checkChanges();
        });

        // disable on any changes
        function checkChanges() {
            // find items that were moved
            var $changed = $('#left_items').find('.right_item');
            $changed = $changed.add($('#right_items').find('.left_item'));

            $('.drag_list li').removeClass('changed');
            $changed.addClass('changed');

            // disable location dropdowns if items were moved
            if ($changed.length) {
                $('#left_location').attr('disabled', 'disabled');
                $('#right_location').attr('disabled', 'disabled');
            } else {
                $('#left_location').removeAttr('disabled');
                $('#right_location').removeAttr('disabled');
            }

            // find out if date_moved is valid date and later than install dates
            var valid = true;
            var date_moved = getDate($('#date_moved').val());
            $('#date_moved').removeClass('input-validation-error');
            $('#date_moved_validation').html('');
            if (date_moved == null) {
                valid = false;
                $('#date_moved').addClass('input-validation-error');
                $('#date_moved_validation').html('Invalid date.');
            } else
                $changed.each(function() {
                    var install_date = getDate($(this).attr('item_date'));
                    if (install_date != null && date_moved < install_date) {
                        valid = false;
                        $('#date_moved').addClass('input-validation-error');
                        var item_name = $(this).attr('item_name');
                        var item_date = $(this).attr('item_date');
                        $('#date_moved_validation').append('Date cannot be before installation date of ' + item_date + ' on ' + item_name + '. ');
                    }
                });

            // enable/disable save button
            if ($changed.length && valid) {
                $('#submit').removeAttr('disabled');
            } else {
                $('#submit').attr('disabled', 'disabled');
            }
        }

        function getDate(string) {
            if (!string) { return null; }

            var s = string.split('-');
            if (s.length < 3) { return null; }

            var date = new Date();
            date.setFullYear(s[0], s[1]-1, s[2]);
            if (date == 'Invalid Date') { return null; }

            var epoch = new Date();
            epoch.setFullYear(1900, 1, 1);
            if (date < epoch) { return null; }

            return date;
        }

        // repopulate items on location dropdown changes
        $('#left_location').change(function() {
            validateLocationChange(this);
            $.get('<%= Url.RouteUrl(new {action ="GetItems"}) %>', { location_id: $(this).val() }, function(data) {
                $('#left_items ul').empty();
                $.each(eval(data), function(i, item) {
                    $('#left_items ul').append('<li class="left_item" item_id="' + item.id + '" item_name="' + item.name + '" item_date="' + item.date + '">' + item.listname + '</li>');
                });
                setupListItems('left');
            });
        });
        $('#right_location').change(function() {
            validateLocationChange(this);
            $('#right_items ul').empty();
            if ($(this).val() != "") {
                var $items = $('#left_items li:has(a.ui-icon-trash)');
                $items.find('a.ui-icon').remove();
                $items.prepend(right_icon);
                $.get('<%= Url.RouteUrl(new {action ="GetItems"}) %>', { location_id: $(this).val() }, function(data) {
                    $.each(eval(data), function(i, item) {
                        $('#right_items ul').append('<li class="right_item" item_id="' + item.id + '" item_name="' + item.name + '" item_date="' + item.date + '">' + item.listname + '</li>');
                    });
                    setupListItems('right');
                });
            } else {
                var $items = $('#left_items li:has(a.ui-icon-arrowthick-1-e)');
                $items.find('a.ui-icon').remove();
                $items.prepend(trash_icon);
            }
        });

        function validateLocationChange(dropdown) {
            var $left_loc = $('#left_location');
            var $right_loc = $('#right_location');
            if ($left_loc.val() == $right_loc.val()) {
                alert('Cannot relocate items from one location to itself.');
                $right_loc.val('');
                $right_loc.change();
            }
        }

    });
</script>

</asp:Content>