using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace MvcWebRole1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        BlobStorageServices _blobStorageServices = new BlobStorageServices();

        public ActionResult Upload()
        {
            CloudBlobContainer blobContainer = _blobStorageServices.GetCloudBlobContainer();
            Dictionary<string, string> blobs = new Dictionary<string, string>();
            foreach (var blobItem in blobContainer.ListBlobs())
            {
                char[] delimiterChars = { '/' };
                string[] words = blobItem.Uri.ToString().Split(delimiterChars);
                blobs.Add(blobItem.Uri.ToString(), words[words.Length - 1]);
            }
            return View(blobs);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    CloudBlobContainer blobContainer = _blobStorageServices.GetCloudBlobContainer();
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(file.FileName);
                    blob.UploadFromStream(file.InputStream);
                }
            }
            return RedirectToAction("Upload");
        }

        public string DeleteFile(string Name)
        {
            Uri uri = new Uri(Name);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            CloudBlobContainer blobContainer = _blobStorageServices.GetCloudBlobContainer();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(filename);

            blob.Delete();
            return "File Deleted";
        }

    }
}
