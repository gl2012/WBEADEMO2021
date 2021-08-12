using System.Data;
using System.Data.SqlClient;

namespace WBEADMS.Models
{
    public static class Repository
    {
        public static ChainOfCustody GetCOC(string id)
        {
            ChainOfCustody selectedCoC = ChainOfCustody.Load(id);
            return selectedCoC;

        }

        public static DataTable GetData(string Id)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("chain_of_custody_id");
            dt.Columns.Add("MediaNo");
            dt.Columns.Add("date_shipped_to_lab");
            dt.Columns.Add("date_recieved_from_lab");
            dt.Columns.Add("date_deployed");
            dt.Columns.Add("date_sample_retrieved");
            dt.Columns.Add("Create_by");
            dt.Columns.Add("Deployed_by");
            dt.Columns.Add("Retrieved_by");
            dt.Columns.Add("Shipped_by");
            dt.Columns.Add("location_name");
            DataRow row;
            int intcount = 0;
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {

                connection.Open();
                using (SqlCommand loadSampleTypes = new SqlCommand(@"SELECT * FROM PassAirSample_Views where chain_of_custody_id=" + Id, connection))
                {
                    using (SqlDataReader dataReader = loadSampleTypes.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            row = dt.NewRow();
                            if (string.IsNullOrWhiteSpace(dataReader["chain_of_custody_id"].ToString()))
                                row[0] = "";
                            else
                                row[0] = dataReader["chain_of_custody_id"].ToString();

                            if (string.IsNullOrWhiteSpace(dataReader["media_serial_number"].ToString()))
                                row[1] = "";
                            else
                                row[1] = dataReader["media_serial_number"].ToString();

                            if (string.IsNullOrWhiteSpace(dataReader["date_shipped_to_lab"].ToString()))
                                row[2] = "";
                            else
                                row[2] = dataReader["date_shipped_to_lab"].ToString().ToDateTimeFormat();

                            if (string.IsNullOrWhiteSpace(dataReader["date_recieved_from_lab"].ToString()))
                                row[3] = "";
                            else
                                row[3] = dataReader["date_recieved_from_lab"].ToString().ToDateTimeFormat();

                            if (string.IsNullOrWhiteSpace(dataReader["date_deployed"].ToString()))
                                row[4] = "";
                            else
                                row[4] = dataReader["date_deployed"].ToString().ToDateTimeFormat();

                            if (string.IsNullOrWhiteSpace(dataReader["date_sample_retrieved"].ToString()))
                                row[5] = "";
                            else
                                row[5] = dataReader["date_sample_retrieved"].ToString().ToDateTimeFormat();

                            if (string.IsNullOrWhiteSpace(dataReader["Create_by"].ToString()))
                                row[6] = "";
                            else
                                row[6] = dataReader["Create_by"].ToString();

                            if (string.IsNullOrWhiteSpace(dataReader["Deployed_by"].ToString()))
                                row[7] = "";
                            else
                                row[7] = dataReader["Deployed_by"].ToString();
                            if (string.IsNullOrWhiteSpace(dataReader["Retrieved_by"].ToString()))
                                row[8] = "";
                            else
                                row[8] = dataReader["Retrieved_by"].ToString();

                            if (string.IsNullOrWhiteSpace(dataReader["Shipped_by"].ToString()))
                                row[9] = "";
                            else
                                row[9] = dataReader["Shipped_by"].ToString();
                            if (string.IsNullOrWhiteSpace(dataReader["location_name"].ToString()))
                                row[10] = "";
                            else
                                row[10] = dataReader["location_name"].ToString();


                            dt.Rows.Add(row);
                            intcount = intcount + 1;
                        }
                    }
                }
                connection.Close();
            }


            for (int i = 0; i < (15 - intcount % 15); i++)
            {
                row = dt.NewRow();
                row[0] = "";
                row[1] = "";
                row[2] = "";
                row[3] = "";
                row[4] = "";
                row[5] = "";
                row[6] = "";
                row[7] = "";
                row[8] = "";
                row[9] = "";
                row[10] = "";
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
