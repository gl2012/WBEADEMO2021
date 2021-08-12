/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Data.SqlClient;
using System.Drawing;

namespace WBEADMS.Models
{
    public partial class LocationImage : BaseModel
    {

        byte[] _image;
        bool _imageIsset;
        byte[] _thumbnail;
        bool _thumbnailIsset;
        User _uploadedBy;
        User _modifiedBy;
        DateTime? _dateUploaded;
        DateTime? _dateModified;

        #region Properties
        public string location_image_id
        {
            get
            {
                return _data["location_image_id"];
            }

            private set
            {
                _data["location_image_id"] = value;
            }
        }

        public string location_id
        {
            get
            {
                return _data["location_id"];
            }

            set
            {
                _data["location_id"] = value;
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

        public string comments
        {
            get
            {
                return _data["comments"];
            }

            set
            {
                _data["comments"] = value;
            }
        }


        public string width
        {
            get
            {
                return _data["width"];
            }

            set
            {
                _data["width"] = value;
            }
        }

        public string height
        {
            get
            {
                return _data["height"];
            }

            set
            {
                _data["height"] = value;
            }
        }

        public string mime_type
        {
            get { return _data["mime_type"]; }
            set { _data["mime_type"] = value; }
        }

        public byte[] image
        {
            private get
            {
                return (_imageIsset) ? _image : null;
            }

            set
            {
                _imageIsset = true;
                _image = value;
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

        public Location Location
        {
            get
            {
                return Location.Load(location_id);
            }
        }
        #endregion

        public LocationImage() : base() { }

        public LocationImage(string locationImageID) : this()
        {
            location_image_id = locationImageID;
            LoadData();
        }

        public override void Validate()
        {
            ////throw new NotImplementedException();
        }

        public override void Save()
        {

            if (image == null)
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
                    "UPDATE LocationImages SET image=@image WHERE location_image_id=@location_image_id", connection))
                {
                    cmd.Parameters.AddWithValue("location_image_id", location_image_id);
                    cmd.Parameters.AddWithValue("@image", image);
                    cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE LocationImages SET thumbnail=@thumbnail WHERE location_image_id=@location_image_id", connection))
                {
                    cmd.Parameters.AddWithValue("location_image_id", location_image_id);
                    cmd.Parameters.AddWithValue("@thumbnail", thumbnail);
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public void ProcessThumbnail()
        {
            const int THUMB_DIMENSION = 100;
            Image img;
            Image thumb;
            using (var byteStream = new System.IO.MemoryStream(image))
            {
                img = Image.FromStream(byteStream);
                width = img.Width.ToString();
                height = img.Height.ToString();
                int thumbWidth = img.Width;
                int thumbHeight = img.Height;
                int maxDimension = Math.Max(thumbWidth, thumbHeight);


                if (thumbWidth > THUMB_DIMENSION || thumbHeight > THUMB_DIMENSION)
                {
                    thumbWidth = (int)Math.Round((float)thumbWidth / maxDimension * THUMB_DIMENSION);
                    thumbHeight = (int)Math.Round((float)thumbHeight / maxDimension * THUMB_DIMENSION);
                }

                thumb = img.GetThumbnailImage(thumbWidth, thumbHeight, null, IntPtr.Zero); // NOTE: img.GetThumbnailImage requires byteStream to be open or it will throw Out of Memory exception.
            }

            using (var byteStream = new System.IO.MemoryStream())
            {
                thumb.Save(byteStream, System.Drawing.Imaging.ImageFormat.Png);
                thumbnail = byteStream.ToArray();
            }
        }
    }
}