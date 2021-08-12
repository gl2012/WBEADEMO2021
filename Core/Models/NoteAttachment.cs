using System;
using System.Data.SqlClient;

namespace WBEADMS.Models
{
    public partial class NoteAttachment : BaseModel
    {

        byte[] _attachment;
        bool _attachmentIsset;
        byte[] _thumbnail;
        bool _thumbnailIsset;
        string _fileName;
        User _uploadedBy;
        User _modifiedBy;
        DateTime? _dateUploaded;
        DateTime? _dateModified;

        #region Properties
        public string note_attachment_id
        {
            get
            {
                return _data["note_attachment_id"];
            }

            private set
            {
                _data["note_attachment_id"] = value;
            }
        }

        public string note_id
        {
            get
            {
                return _data["note_id"];
            }

            set
            {
                _data["note_id"] = value;
            }
        }

        public String FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }

        public User UploadedBy
        {
            get
            {
                if (_uploadedBy == null && !String.IsNullOrEmpty(uploaded_by))
                {
                    _uploadedBy = new User(uploaded_by);
                }

                return _uploadedBy;
            }
        }

        public string uploaded_by
        {
            get
            {
                return _data["uploaded_by"];
            }

            set
            {
                _data["uploaded_by"] = value;
            }
        }

        public DateTime? DateUploaded
        {
            get
            {
                if (!_dateUploaded.HasValue && !date_uploaded.IsBlank())
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(date_uploaded, out parsedDate))
                    {
                        _dateUploaded = parsedDate;
                    }
                }

                return _dateUploaded;
            }
        }

        public string date_uploaded
        {
            get
            {
                return _data["date_uploaded"].ToDateTimeFormat();
            }

            set
            {
                _data["date_uploaded"] = value.ToDateTimeFormat();
            }
        }

        public User ModifiedBy
        {
            get
            {
                if (_modifiedBy == null && !String.IsNullOrEmpty(modified_by))
                {
                    _modifiedBy = new User(modified_by);
                }

                return _modifiedBy;
            }
        }

        public string modified_by
        {
            get
            {
                return _data["modified_by"];
            }

            set
            {
                _data["modified_by"] = value;
            }
        }

        public DateTime? DateModified
        {
            get
            {
                if (!_dateModified.HasValue && !date_modified.IsBlank())
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(date_modified, out parsedDate))
                    {
                        _dateModified = parsedDate;
                    }
                }

                return _dateModified;
            }
        }

        public string date_modified
        {
            get
            {
                return _data["date_modified"].ToDateTimeFormat();
            }

            set
            {
                _data["date_modified"] = value.ToDateTimeFormat();
            }
        }

        public string filename
        {
            get
            {
                return _data["filename"];
            }
            set
            {
                _data["filename"] = value;
            }
        }

        public string mime_type
        {
            get { return _data["mime_type"]; }
            set { _data["mime_type"] = value; }
        }

        public byte[] GetAttachment()
        {
            try
            {
                return System.Text.Encoding.Default.GetBytes(_data["attachment"]);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public byte[] attachment
        {
            private get
            {
                return (_attachmentIsset) ? _attachment : null;
            }

            set
            {
                _attachmentIsset = true;
                _attachment = value;
            }
        }

        public byte[] thumbnail
        {
            private get
            {
                return (_thumbnailIsset) ? _thumbnail : null;
            }

            set
            {
                _thumbnailIsset = true;
                _thumbnail = value;
            }
        }

        public Note Note
        {
            get
            {
                return Note.Load(note_id);
            }
        }

        #endregion

        public NoteAttachment() : base() { }

        public NoteAttachment(string noteAttachmentID)
            : this()
        {
            note_attachment_id = noteAttachmentID;
            LoadData();
        }

        public override void Validate()
        {
            ////throw new NotImplementedException();
        }

        public override void Save()
        {

            if (attachment == null)
            {
                base.Save();
                return;
            }

            ProcessThumbnail();

            base.Save();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE NoteAttachments SET attachment=@attachment WHERE note_attachment_id=@note_attachment_id", connection))
                {
                    cmd.Parameters.AddWithValue("note_attachment_id", note_attachment_id);
                    cmd.Parameters.AddWithValue("@attachment", attachment);
                    cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE NoteAttachments SET thumbnail=@thumbnail WHERE note_attachment_id=@note_attachment_id", connection))
                {
                    cmd.Parameters.AddWithValue("note_attachment_id", note_attachment_id);
                    cmd.Parameters.AddWithValue("@thumbnail", thumbnail);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public void ProcessThumbnail()
        {
            /*
            const int THUMB_DIMENSION = 100;
            Image img;
            Image thumb;
            using (var byteStream = new System.IO.MemoryStream(attachment)) {
                img = Image.FromStream(byteStream);
                string width = img.Width.ToString();
                string height = img.Height.ToString();
                int thumbWidth = img.Width;
                int thumbHeight = img.Height;
                int maxDimension = Math.Max(thumbWidth, thumbHeight);


                if (thumbWidth > THUMB_DIMENSION || thumbHeight > THUMB_DIMENSION) {
                    thumbWidth = (int)Math.Round((float)thumbWidth / maxDimension * THUMB_DIMENSION);
                    thumbHeight = (int)Math.Round((float)thumbHeight / maxDimension * THUMB_DIMENSION);
                }
                    
                thumb = img.GetThumbnailImage(thumbWidth, thumbHeight, null, IntPtr.Zero); // NOTE: img.GetThumbnailImage requires byteStream to be open or it will throw Out of Memory exception.
            }

            using (var byteStream = new System.IO.MemoryStream()) {
                thumb.Save(byteStream, System.Drawing.Imaging.ImageFormat.Png);
                thumbnail = byteStream.ToArray();
            }
             */
            _thumbnailIsset = true;
            _thumbnail = new byte[] { 0 };
        }
    }
}