<%--
Copyright © 2010 Air Shed Systems Inc. All rights reserved.
www.airshedsystems.com <http://www.airshedsystems.com>
Notice of License
Provided to WBEA under license agreement dated 22 December 2010
--%>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<WBEADMS.Models.SampleCompound[]>" %>

<fieldset>
    <legend>Compound Results</legend>
    <table class="generalTable" id="compoundTable">
        <tr>
            <th>Name</th>
            <th>Number</th>
            <th>CAS</th>
            <th>Molecular<br />Formula</th>
            <th>Molecular<br />Weight</th>
            <th>Group<br />Name</th>
            <th>Retention<br />Time</th>
            <th>Match<br />Quality</th>
            <th>MDL</th>
            <th>RDL</th>
            <th>Raw<br />Measurement</th>
            <th>Final<br />Concentration</th>
            <th>QAQC</th>
        </tr>
        <% foreach (SampleCompound compound in Model) { %>
        <tr>
            <td><%= compound.compound_name%></td>
            <td><%= compound.compound_number%></td>
            <td><%= compound.cas_number%></td>
            <td><%= compound.molecular_formula%></td>
            <td><%= compound.molecular_weight%></td>
            <td><%= compound.compound_group_name%></td>
            <td><%= compound.retention_time%></td>
            <td><%= compound.match_quality%></td>
            <td><%= compound.method_detection_limit%> <%= compound.method_detection_unit%></td>
            <td><%= compound.reporting_detection_limit%> <%= compound.reporting_detection_unit%></td>
            <td><%= compound.raw_measurement%> <%= compound.raw_measurement_unit%></td>
            <td><%= compound.final_concentration%> <%= compound.final_concentration_unit%></td>
            <td><%= compound.qaqc_flag%></td>
        </tr>
        <% } %>
    </table>
</fieldset>