/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Data.SqlClient;

namespace WBEADMS.Models
{
    public partial class LocationImage
    {
        public static byte[] GetThumbnail(string id)
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand locationsCount = new SqlCommand(@"select thumbnail FROM LocationImages WHERE location_image_id = @locationImageID", connection))
                {
                    locationsCount.Parameters.AddWithValue("locationImageID", id);
                    return (byte[])locationsCount.ExecuteScalar();
                }
            }
        }

        public static byte[] GetImage(string id)
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand locationsCount = new SqlCommand(@"select image FROM LocationImages WHERE location_image_id = @locationImageID", connection))
                {
                    locationsCount.Parameters.AddWithValue("locationImageID", id);
                    return (byte[])locationsCount.ExecuteScalar();

                    /*
                    dbLocationsCount.Parameters.AddWithValue("locationImageID", id);
                    MemoryStream imageStream= new MemoryStream((byte[])dbLocationsCount.ExecuteScalar());
                    return Image.FromStream(imageStream);
                     * */
                }
            }
        }

        public static LocationImage Load(string id)
        {
            return Load<LocationImage>(id);
        }
    }
}