using System.Web.Services;

namespace DocIt
{
    /// <summary>
    /// Summary description for ActiveReportsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ActiveReportsService : GrapeCity.ActiveReports.Web.ReportService
    {
        /*
        protected override object OnCreateReportHandler(string reportPath)
        {
            var data = reportPath.Split('-');
            if (data.Length != 2)
                return base.OnCreateReportHandler(reportPath);
            switch (data[0])
            {
                case "Details":
                    var sectionReport = new SectionReport();
                    sectionReport.LoadLayout(XmlReader.Create(Server.MapPath("~/Reports/COC_Pass.rpx")));
                    // sectionReport.DataSource = Repository.GetCOC(data[1]);
                    sectionReport.DataSource = Repository.GetData(data[1]);
                   
                    GrapeCity.ActiveReports.SectionReportModel.Parameter myParam1 = new GrapeCity.ActiveReports.SectionReportModel.Parameter();
                    myParam1.Key = "myParam1";
                    myParam1.Type = GrapeCity.ActiveReports.SectionReportModel.Parameter.DataType.String;
                    //Set to False if you do not want input from user.
                    myParam1.PromptUser = false;
                    myParam1.Prompt = "Enter Data:";
                    myParam1.DefaultValue = "Default Value";

                    sectionReport.Parameters.Add(myParam1);

                    ((TextBox)sectionReport.Sections["PageHeader"].Controls["txtRefCOC"]).Text = data[1];

                    using (DataTable dt = Repository.GetData(data[1]))
                    {
                        ((TextBox)sectionReport.Sections["PageFooter"].Controls["txtReceivedDate"]).Text = dt.Rows[0].Field<string>(3);
                        ((TextBox)sectionReport.Sections["PageFooter"].Controls["txtDeployment"]).Text = dt.Rows[0].Field<string>(4);
                        ((TextBox)sectionReport.Sections["PageFooter"].Controls["txtRetrieval"]).Text = dt.Rows[0].Field<string>(5);
                        ((TextBox)sectionReport.Sections["PageFooter"].Controls["txtShippedToLab"]).Text = dt.Rows[0].Field<string>(2);
                        ((TextBox)sectionReport.Sections["PageFooter"].Controls["txtCreateBy"]).Text = dt.Rows[0].Field<string>(6);
                        ((TextBox)sectionReport.Sections["PageFooter"].Controls["txtDeployedby"]).Text = dt.Rows[0].Field<string>(7);
                        ((TextBox)sectionReport.Sections["PageFooter"].Controls["txtRetrievedby"]).Text = dt.Rows[0].Field<string>(8);
                        ((TextBox)sectionReport.Sections["PageFooter"].Controls["txtShippedby"]).Text = dt.Rows[0].Field<string>(9);
                        ((TextBox)sectionReport.Sections["PageHeader"].Controls["txtSiteID"]).Text = dt.Rows[0].Field<string>(10);
                    }
                    // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", data[1]);
                    var xvalues = new List<string>();
                    var yvalues = new List<double>();
                    //  foreach (Order order in Repository.GetOrders(data[1]))
                    // {
                    //      xvalues.Add(order.ShippedDate);
                    //      yvalues.Add((double)order.Freight);
                    //  }
                    //  ((SectionReportModel.ChartControl)sectionReport.Sections["reportFooter1"].Controls[0]).Series[0].Points.DataBindXY(xvalues.ToArray(), yvalues.ToArray());
                    return sectionReport;
               
                case "Passive_COC":
                    var sectionReport1 = new SectionReport();
                    sectionReport1.LoadLayout(XmlReader.Create(Server.MapPath("~/Reports/COC_Pass.rpx")));
                    // sectionReport.DataSource = Repository.GetCOC(data[1]);
                    sectionReport1.DataSource = Repository.GetData(data[1]);
                    sectionReport1.PageSettings.Margins.Top = 0.3f;
                    sectionReport1.PageSettings.Margins.Bottom = 0.75f;

                    ((TextBox)sectionReport1.Sections["PageHeader"].Controls["txtRefCOC"]).Text = data[1];
                   
                    using (DataTable dt = Repository.GetData(data[1]))
                    {
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtReceivedDate"]).Text =   dt.Rows[0].Field<string>(3);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtDeployment"]).Text = dt.Rows[0].Field<string>(4);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtRetrieval"]).Text = dt.Rows[0].Field<string>(5);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtShippedToLab"]).Text = dt.Rows[0].Field<string>(2);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtCreateBy"]).Text = dt.Rows[0].Field<string>(6);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtDeployedby"]).Text = dt.Rows[0].Field<string>(7);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtRetrievedby"]).Text = dt.Rows[0].Field<string>(8);
                        ((TextBox)sectionReport1.Sections["PageFooter"].Controls["txtShippedby"]).Text = dt.Rows[0].Field<string>(9);
                        ((TextBox)sectionReport1.Sections["PageHeader"].Controls["txtSiteID"]).Text = dt.Rows[0].Field<string>(10);
                    }
                    // System.IO.File.WriteAllText(@"C:\temp\strcocid.txt", data[1]);
                
                    return sectionReport1;
                case "Page":
                    var pageReport = new PageReport(new FileInfo(Server.MapPath("~/Reports/OrderDetailsReport.rdlx")));
                  //  pageReport.Document.LocateDataSource += delegate (object sender, LocateDataSourceEventArgs args)
                    {
                      //  args.Data = Repository.GetDetails(data[1]);
                    };
                    return pageReport;
            }
            return base.OnCreateReportHandler(reportPath);
        }*/
    }
}
