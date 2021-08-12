using System.Data.SqlClient;

namespace WBEADMS.Models
{
    public partial class NoteAttachment
    {
        public static byte[] GetAttachment(string id)
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand locationsCount = new SqlCommand(@"select attachment FROM NoteAttachments WHERE note_attachment_id = @noteAttachmentID", connection))
                {
                    locationsCount.Parameters.AddWithValue("noteAttachmentID", id);
                    return (byte[])locationsCount.ExecuteScalar();
                }
            }
        }

        public static NoteAttachment GetAttachmentByNoteID(string parentID)
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand locationsCount = new SqlCommand(@"select note_attachment_id FROM NoteAttachments WHERE note_id = @parentID", connection))
                {
                    locationsCount.Parameters.AddWithValue("parentID", parentID);
                    var id = locationsCount.ExecuteScalar();
                    if (id != null)
                    {
                        return Load(id.ToString());
                    }
                    else return null;
                }
            }
        }

        public static byte[] GetThumbnail(string id)
        {
            using (SqlConnection connection = new SqlConnection(ModelsCommon.FetchConnectionString()))
            {
                connection.Open();

                using (SqlCommand locationsCount = new SqlCommand(@"select thumbnail FROM NoteAttachment WHERE note_attachment_id = @noteAttachmentID", connection))
                {
                    locationsCount.Parameters.AddWithValue("noteAttachmentID", id);
                    return (byte[])locationsCount.ExecuteScalar();
                }
            }
        }

        public static NoteAttachment Load(string id)
        {
            return Load<NoteAttachment>(id);
        }
    }
}