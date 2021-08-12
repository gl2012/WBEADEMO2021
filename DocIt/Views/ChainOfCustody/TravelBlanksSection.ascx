<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
	<%
        var travelViewData = new ViewDataDictionary(Model.TravelBlanks);
        travelViewData["coc"] = Model;
        travelViewData["legend"] = "Travel Blank(s)";
        travelViewData["fieldSetClass"] = "";
        travelViewData["formId"] = "travel_sample_id";
        travelViewData["sample_id"] = ViewData["travel_sample_id"];
        travelViewData["maxSampleCount"] = 3;   
        Html.RenderPartial("SamplesSection", travelViewData);
	%>