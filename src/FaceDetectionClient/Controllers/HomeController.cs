using FaceDetectionClient.Helpers;
using FaceDetectionClient.Services;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FaceDetectionClient.Controllers
{
    public class HomeController : Controller
    {
        [Route ("")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("about")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Route("detect-url")]
        public async Task<ActionResult> DetectExternalImage(string RemoteURL)
        {
            if (!Uri.IsWellFormedUriString(RemoteURL, UriKind.Absolute))
            {
                ViewBag.ErrorMessage = "Invalid Image Url: " + RemoteURL;
                return View("Index");
            }

            IList <DetectedFace> faceList = await FaceClientService.Instance.GetDetectFaceWitUrl(RemoteURL);
            ViewBag.CountPeople = (faceList.Count > 1) ? faceList.Count + " people detected" : "Only 1 person detected";
            ViewBag.Message = Helper.GetFaceAttributes(faceList);
            return View("Index", (object) RemoteURL);
        }


        [HttpPost]
        [Route("detect-localimage")]
        public async Task<ActionResult> DetectLocalImage(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string base64String = string.Empty;
                    string path = Path.Combine(Server.MapPath("~/LocalImages"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    IList<DetectedFace> faceList = await FaceClientService.Instance.GetDetectFaceWitLocalImage(fs);
                    ViewBag.CountPeople = (faceList.Count > 1) ? faceList.Count + " people detected" : "Only 1 person detected";
                    ViewBag.Message = Helper.GetFaceAttributes(faceList);
                    using (Image image = Image.FromFile(path))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                             base64String = "data:image/jpeg;base64," + Convert.ToBase64String(imageBytes);
                        }
                    }
                    return View("Index", (object)base64String);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }
    }
}