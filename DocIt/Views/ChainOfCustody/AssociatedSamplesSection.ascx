<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.ChainOfCustody>" %>
	<%
        var associatedViewData = new ViewDataDictionary(Model.Samples);
        associatedViewData["coc"] = Model;
        associatedViewData["legend"] = "Associated Sample(s)";
        associatedViewData["fieldSetClass"] = "first";
        associatedViewData["formId"] = "sample_id";
        associatedViewData["sample_id"] = ViewData["sample_id"];
        
	    if (Model.SampleType.name == "PASS") {
            associatedViewData["maxSampleCount"] = 20;
        }
        else {
            associatedViewData["maxSampleCount"] = 1;
        }   
     
	    Html.RenderPartial("NewSamplesSection", associatedViewData);
        
	%>