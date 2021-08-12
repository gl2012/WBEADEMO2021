
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using WBEADMS.Models;

namespace WBEADMS.AdminPanel.Controllers
{
    [Authorize]
    public class LocationImageController : Controller
    {
        public ActionResult ListByLocation(string id, int? page, int? page_size)
        {
            if (!SetLocationInViewData(id))
            {
                return RedirectToAction("Index", "Location");
            }

            string whereClause = "location_id = " + id;
            var urlParameters = new { action = "ListByLocation", controller = "LocationImage", id = id };
            Paginator paginator = this.AddDefaultPaginator<LocationImage>(urlParameters, page, new { where = whereClause, order = "date_uploaded DESC" });
            this.SetPageSize(page_size, paginator);
            List<LocationImage> locationImages = BaseModel.FetchPage<LocationImage>(paginator, new { where = whereClause, order = "date_uploaded DESC" });

            // add notice if there is no records
            if (locationImages.Count == 0)
            {
                this.AddViewNotice("There are no Images for this location.");
            }

            return View(locationImages);
        }

        public ActionResult Details(string id)
        {
            LocationImage selectedLocationImage = new LocationImage(id);
            if (selectedLocationImage == null)
            {
                this.AddTempNotice("The location image " + id + " could not be found.");
                return RedirectToAction("Index", "Location");
            }

            return View(selectedLocationImage);
        }

        public FileContentResult GetThumbnail(string id)
        {
            return new FileContentResult(LocationImage.GetThumbnail(id), "image/jpeg");
        }

        public FileContentResult GetImage(string id)
        {
            return new FileContentResult(LocationImage.GetImage(id), "image/jpeg");
        }

        public ActionResult Upload(string id)
        {
            if (!SetLocationInViewData(id)) { return RedirectToAction("Index", "Location"); }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(string id, FormCollection collection)
        {
            if (!SetLocationInViewData(id)) { return RedirectToAction("Index", "Location"); }

            var item = new LocationImage();
            try
            {
                byte[] image;
                string mimeType;
                bool fileExists = GetImageFromRequest(out image, out mimeType);
                if (!fileExists)
                {
                    throw new ModelException(new Exception("Uploaded image was empty."));
                }

                UpdateModel(item);
                item.location_id = id;
                item.mime_type = mimeType;
                item.image = image;

                item.uploaded_by = ((WBEADMS.Models.User)Session["user"]).user_id;
                item.date_uploaded = System.DateTime.Now.ToString();

                item.Save();
                this.AddTempNotice("Successfully created item [" + item.location_image_id + "]");
                return RedirectToRoute(new { action = "Details", id = item.location_image_id });
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                return View(item);
            }
        }

        // GET: /LocationImage/Edit/5
        public ActionResult Edit(string id)
        {
            var item = LocationImage.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The image " + id + " could not be found.");
                return RedirectToAction("Index", "Location");
            }

            return View(item);
        }

        // POST: /LocationImage/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(string id, FormCollection collection)
        {
            var item = LocationImage.Load(id);
            if (item == null)
            {
                this.AddTempNotice("The item " + id + " could not be found.");
                return RedirectToAction("Index", "Location");
            }

            try
            {
                byte[] image;
                string mimeType;
                bool fileExists = GetImageFromRequest(out image, out mimeType);

                UpdateModel(item);
                if (fileExists)
                {
                    item.image = image;
                    item.mime_type = mimeType;
                }

                item.modified_by = ((WBEADMS.Models.User)Session["user"]).user_id;
                item.date_modified = System.DateTime.Now.ToString();

                item.Save();
                this.AddTempNotice("Successfully updated image [" + item.location_image_id + "]");
                return RedirectToRoute(new { action = "Details", id = item.location_image_id });
            }
            catch (ModelException e)
            {
                this.PopulateViewWithErrorMessages(e);
                return View(item);
            }
        }

        private bool SetLocationInViewData(string id)
        {
            var location = Location.Load(id);
            if (location == null)
            {
                this.AddTempNotice("Unable to load location " + id);
                return false;
            }

            ViewData["location_id"] = id;
            ViewData["location_name"] = location.name;
            return true;
        }

        private bool GetImageFromRequest(out byte[] image, out string mimeType)
        {
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                image = null;
                mimeType = null;
                return false;
            }

            // get file from request
            HttpPostedFileBase file = Request.Files[0];

            // get image from file
            image = new byte[file.ContentLength];
            file.InputStream.Read(image, 0, file.ContentLength);

            // determine MIME content type
            switch (System.IO.Path.GetExtension(file.FileName).ToLower())
            {
                case ".jpeg":
                case ".jpe":
                case ".jpg": mimeType = "image/jpeg"; break;
                case ".gif": mimeType = "image/gif"; break;
                case ".png": mimeType = "image/png"; break;
                default: mimeType = file.ContentType; break;
            }

            if (!mimeType.ToLower().Contains("image"))
            {
                throw new ModelException(new ArgumentException("Uploaded file type is not supported."));
            }

            return true;
        }
    }
}