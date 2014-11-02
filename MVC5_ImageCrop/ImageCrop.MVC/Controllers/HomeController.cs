using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageCrop.Common;
using ImageCrop.MVC.Models;

namespace ImageCrop.MVC.Controllers
{
    public class HomeController : Controller
    {
        #region -- Fields & Properties --

        private readonly UploadImageService _service = new UploadImageService();

        private string UploadFolder
        {
            get
            {
                return @"FileUpload/Temp";
            }
        }

        private string OriginalFolder
        {
            get
            {
                return @"FileUpload/Original";
            }
        }

        private string CropFolder
        {
            get
            {
                return @"FileUpload/Crop";
            }
        }

        private string UploadPath
        {
            get
            {
                return Server.MapPath("~/" + this.UploadFolder);
            }
        }

        private string OriginalPath
        {
            get
            {
                return Server.MapPath("~/" + this.OriginalFolder);
            }
        }

        private string CropPath
        {
            get
            {
                return Server.MapPath("~/" + this.CropFolder);
            }
        }

        #endregion

        //=========================================================================================

        public ActionResult Index()
        {
            var result = _service.FindAll();
            this.ViewData.Model = result;
            return View();
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            Dictionary<string, string> result = this.ProcessDelete(id);
            return Json(result);
        }

        #region -- ProcessDelete --
        /// <summary>
        /// Processes the delete.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        private Dictionary<string, string> ProcessDelete(string id)
        {
            var jo = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(id))
            {
                jo.Add("result", "error");
                jo.Add("msg", "無ID編號");
            }
            else
            {
                var imageId = new Guid(id.ToString());

                var item = _service.FindOne(imageId);

                if (item == null)
                {
                    jo.Add("result", "error");
                    jo.Add("msg", "資料不存在");
                }
                else
                {
                    try
                    {
                        _service.Delete(imageId);

                        if (!string.IsNullOrWhiteSpace(item.OriginalImage))
                        {
                            string fileName1 = Server.MapPath(string.Format(@"~/{0}/{1}",
                                OriginalFolder, 
                                item.OriginalImage));

                            if (System.IO.File.Exists(fileName1))
                            {
                                System.IO.File.Delete(fileName1);
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(item.CropImage))
                        {
                            string fileName2 = Server.MapPath(string.Format(@"~/{0}/{1}", 
                                CropFolder, 
                                item.CropImage));

                            if (System.IO.File.Exists(fileName2))
                            {
                                System.IO.File.Delete(fileName2);
                            }
                        }
                        jo.Add("result", "OK");
                        jo.Add("msg", "");
                    }
                    catch (Exception ex)
                    {
                        jo.Add("result", "exception");
                        jo.Add("msg", ex.Message);
                    }
                }
            }
            return jo;
        }
        #endregion

        //=========================================================================================

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase uploadFile)
        {
            if (uploadFile == null)
            {
                TempData["Upload_Result"] = "error";
                TempData["Upload_Msg"] = "沒有上傳檔案";
                return View();
            }

            try
            {
                var jo = new Dictionary<string, string>();

                var cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, "");

                jo = cropUtils.ProcessUploadImage(uploadFile);

                TempData["Upload_Result"] = jo["result"];
                TempData["Upload_Msg"] = jo["msg"];
            }
            catch (Exception ex)
            {
                TempData["Upload_Result"] = "Exception";
                TempData["Upload_Msg"] = ex.Message;
            }
            return View();
        }

        [HttpPost]
        public JsonResult Save(string fileName)
        {
            var jo = new Dictionary<string, string>();

            var cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, "");

            var result = cropUtils.SaveUploadImageToOriginalFolder(fileName);

            if (!result["result"].Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                jo.Add("result", result["result"]);
                jo.Add("msg", result["msg"]);
                return Json(jo);
            }

            try
            {
                var instance = new ImageCrop.Common.UploadImage
                {
                    ID = Guid.NewGuid(),
                    OriginalImage = fileName,
                    CreateDate = DateTime.Now
                };
                instance.UpdateDate = instance.CreateDate;

                _service.Add(instance);

                jo.Add("result", "Success");
                jo.Add("msg", string.Format(@"/{0}/{1}", OriginalFolder, fileName));
                jo.Add("id", instance.ID.ToString());
            }
            catch (Exception ex)
            {
                jo.Add("result", "Exception");
                jo.Add("msg", ex.Message);
            }
            return Json(jo);
        }

        public JsonResult Cancel(string fileName)
        {
            var cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, "");
            Dictionary<string, string> result = cropUtils.DeleteUploadImage(fileName);
            return Json(result);
        }

        //=========================================================================================

        public ActionResult Crop(string id)
        {
            ViewData["ErrorMessage"] = "";
            ViewData["UploadImage_ID"] = "";
            ViewData["OriginalImage"] = "";
            ViewData["CropImape"] = "";

            if (string.IsNullOrWhiteSpace(id))
            {
                ViewData["ErrorMessage"] = "沒有輸入資料編號";
                return View();
            }

            Guid imageID;
            if (!Guid.TryParse(id, out imageID))
            {
                ViewData["ErrorMessage"] = "資料編號錯誤";
                return View();
            }

            var instance = _service.FindOne(imageID);
            if (instance == null)
            {
                ViewData["ErrorMessage"] = "資料不存在";
                return View();
            }

            ViewData["UploadImage_ID"] = imageID;
            ViewData["OriginalImage"] = string.Format(@"/{0}/{1}",
                OriginalFolder, 
                instance.OriginalImage);

            if (!string.IsNullOrWhiteSpace(instance.CropImage))
            {
                ViewData["CropImape"] = string.Format(@"/{0}/{1}", CropFolder, instance.CropImage);
            }
            return View();
        }

        [HttpPost]
        public JsonResult CropImage(string id, int? x1, int? x2, int? y1, int? y2)
        {
            var result = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(id))
            {
                result.Add("result", "error");
                result.Add("msg", "沒有輸入資料編號");
                return Json(result);
            }

            if (!x1.HasValue || !x2.HasValue || !y1.HasValue || !y2.HasValue)
            {
                result.Add("result", "error");
                result.Add("msg", "裁剪圖片區域值有缺少");
                return Json(result);
            }

            Guid imageID;
            if (!Guid.TryParse(id, out imageID))
            {
                result.Add("result", "error");
                result.Add("msg", "資料編號錯誤");
                return Json(result);
            }

            var instance = _service.FindOne(imageID);
            if (instance == null)
            {
                result.Add("result", "error");
                result.Add("msg", "資料不存在");
                return Json(result);
            }

            var cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, this.CropPath);

            Dictionary<string, string> processResult = cropUtils.ProcessImageCrop
            (
                instance,
                new int[] { x1.Value, x2.Value, y1.Value, y2.Value }
            );

            if (processResult["result"].Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                //裁剪圖片檔名儲存到資料庫
                _service.Update(instance.ID, processResult["CropImage"]);

                //如果有之前的裁剪圖片，則刪除
                if (!string.IsNullOrWhiteSpace(processResult["OldCropImage"]))
                {
                    cropUtils.DeleteCropImage(processResult["OldCropImage"]);
                }

                result.Add("result", "OK");
                result.Add("msg", "");
                result.Add("OriginalImage", string.Format(@"/{0}/{1}", this.OriginalFolder, processResult["OriginalImage"]));
                result.Add("CropImage", string.Format(@"/{0}/{1}", this.CropFolder, processResult["CropImage"]));
            }
            else
            {
                result.Add("result", processResult["result"]);
                result.Add("msg", processResult["msg"]);
            }
            return Json(result);
        }

    }
}