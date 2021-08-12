/*****************************************************************************************
    Copyright 2013-2014 Wood Buffalo Environmental Asssociation
    
    http://wbea.org
******************************************************************************************/
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