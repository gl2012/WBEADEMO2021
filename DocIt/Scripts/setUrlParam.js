/* Copyright © [AUTO:CURRENT_YEAR] Air Shed Systems Inc. All rights reserved.
* www.airshedsystems.com <http://www.airshedsystems.com>
* Notice of License
* Provided to WBEA under license agreement dated 22 December 2010
*/
// takes an array of name:value objects that specify params
function setUrlParam(paramSet) {
    var paramList = top.location.search;

    for (var i = 0; i < paramSet.length; i++) {
        var name = paramSet[i].name;
        var value = paramSet[i].value;
        if (paramList.indexOf(name) >= 0) {
            if (value == '') {
                paramList = paramList.replace(new RegExp('&?' + name + '=' + '.*?(?=$|&)'), '');
            } else {
                paramList = paramList.replace(new RegExp(name + '=' + '.*?(?=$|&)'), name + '=' + value);
            }
        } else {
            paramList += (paramList.length > 1) ? '&' : '';
            paramList += name + '=' + value;
        }
    }

    top.location.search = paramList;
}