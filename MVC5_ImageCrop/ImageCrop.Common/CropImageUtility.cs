﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ImageCrop.Common
{
    public class CropImageUtility
    {
        public string UploadPath
        {
            get;
            set;
        }

        public string OriginalPath
        {
            get;
            set;
        }

        public string CropPath
        {
            get;
            set;
        }

        public CropImageUtility(string uploadPath, string originalPath, string cropPath)
        {
            this.UploadPath = uploadPath;
            this.OriginalPath = originalPath;
            this.CropPath = cropPath;
        }

        //=========================================================================================
        // Upload Image

        #region -- ProcessUploadImage --
        /// <summary>
        /// Processes the upload image.
        /// </summary>
        /// <param name="uploadFile">The upload file.</param>
        /// <returns></returns>
        public Dictionary<string, string> ProcessUploadImage(HttpPostedFileBase uploadFile)
        {
            var jo = new Dictionary<string, string>();

            if (uploadFile.ContentLength <= 0)
            {
                jo.Add("result", "error");
                jo.Add("msg", "無內容檔案！請重新選擇檔案！");
                return jo;
            }
            else if (uploadFile.ContentLength >= 10240000)
            {
                //容量超過 10M (10240KB)
                jo.Add("result", "error");
                jo.Add("msg", "上傳檔案不可超過10M！請重新選擇檔案！");
                return jo;
            }
            else
            {
                if (!IsImage(uploadFile))
                {
                    jo.Add("result", "error");
                    jo.Add("msg", "上傳檔案並不是圖片檔！請重新選擇檔案！");
                    return jo;
                }

                // Picture Image_Type
                var imageTypes = new string[] { "jpg", "jpeg", "png", "gif" };
                var extension = Path.GetExtension(uploadFile.FileName);
                if (extension != null && !imageTypes.Contains(extension.Substring(1, 3).ToLower()))
                {
                    jo.Add("result", "error");
                    jo.Add("msg", "上傳檔案的圖片檔只能接受 jpg, jpeg, png, gif！請重新選擇檔案！");
                    return jo;
                }

                try
                {
                    //存檔
                    var fileName = String.Concat(
                        MiscUtility.MakeGuid().Replace("-", string.Empty).Substring(0, 20), 
                        Path.GetExtension(uploadFile.FileName).ToLower());

                    uploadFile.SaveAs(string.Format(@"{0}\{1}", this.UploadPath, fileName));

                    jo.Add("result", "Success");
                    jo.Add("msg", fileName);
                }
                catch (Exception ex)
                {
                    jo.Add("result", "Failure");
                    jo.Add("msg", ex.Message);
                }
                return jo;
            }
        }

        /// <summary>
        /// Determines whether the specified file is image.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        ///   <c>true</c> if the specified file is image; otherwise, <c>false</c>.
        /// </returns>
        private bool IsImage(HttpPostedFileBase file)
        {
            if (file != null
                && 
                System.Text.RegularExpressions.Regex.IsMatch(file.ContentType, "image/\\S+")
                && 
                file.ContentLength > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region -- SaveUploadImageToOriginalFolder --
        /// <summary>
        /// Saves the upload image to original folder.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public Dictionary<string, string> SaveUploadImageToOriginalFolder(string fileName)
        {
            var jo = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                jo.Add("result", "error");
                jo.Add("msg", "無檔案名稱");
            }
            else
            {
                try
                {
                    var sourceFile = string.Format(@"{0}\{1}", this.UploadPath, fileName);
                    var destFile = string.Format(@"{0}\{1}", this.OriginalPath, fileName);

                    if (!System.IO.Directory.Exists(this.OriginalPath))
                    {
                        System.IO.Directory.CreateDirectory(this.OriginalPath);
                    }

                    System.IO.File.Copy(sourceFile, destFile, true);

                    var deleteFileName = string.Format(@"{0}\{1}", this.UploadPath, fileName);
                    if (System.IO.File.Exists(deleteFileName))
                    {
                        System.IO.File.Delete(deleteFileName);
                    }

                    jo.Add("result", "Success");
                    jo.Add("msg", "");
                }
                catch (Exception ex)
                {
                    jo.Add("result", "Exception");
                    jo.Add("msg", ex.Message);
                }
            }
            return jo;
        }
        #endregion

        //=========================================================================================
        // Crop Image

        #region -- ProcessImageCrop --
        /// <summary>
        /// Processes the image crop.
        /// </summary>
        public Dictionary<string, string> ProcessImageCrop(
            UploadImage currentImage, 
            int[] sectionValue)
        {
            var result = new Dictionary<string, string>();

            try
            {
                //取得裁剪的區域座標
                var sectionX1 = sectionValue[0];
                var sectionX2 = sectionValue[1];
                var sectionY1 = sectionValue[2];
                var sectionY2 = sectionValue[3];

                //取得裁剪的圖片寬高
                int width = sectionX2 - sectionX1;
                int height = sectionY2 - sectionY1;

                //讀取原圖片
                System.Drawing.Image sourceImage = System.Drawing.Image.FromFile
                (
                    string.Format(@"{0}\{1}", this.OriginalPath, currentImage.OriginalImage)
                );

                //從原檔案取得裁剪圖片
                System.Drawing.Image cropImage = this.CropImage(
                    sourceImage,
                    new Rectangle(sectionX1, sectionY1, width, height)
                );

                //將採剪下來的圖片做縮圖處理
                Bitmap resizeImage = this.ResizeImage(cropImage, new Size(100, 100));

                //將縮圖處理完成的圖檔儲存為JPG格式
                var fileName = String.Concat(MiscUtility.MakeGuid().Replace("-", string.Empty).Substring(0, 20), ".jpg");
                var savePath = string.Format(@"{0}\{1}", this.CropPath, fileName);
                SaveJpeg(savePath, resizeImage, 100L);

                //釋放檔案資源
                resizeImage.Dispose();
                cropImage.Dispose();
                sourceImage.Dispose();

                //如果有之前的裁剪圖片，暫存既有的裁剪圖片檔名
                string oldCropImageFileName = string.Empty;
                if (!string.IsNullOrWhiteSpace(currentImage.CropImage))
                {
                    oldCropImageFileName = currentImage.CropImage;
                }

                //JSON
                result.Add("result", "Success");
                result.Add("OriginalImage", currentImage.OriginalImage);
                result.Add("CropImage", fileName);
                result.Add("OldCropImage", oldCropImageFileName);
            }
            catch (Exception ex)
            {
                result.Add("result", "Exception");
                result.Add("msg", ex.Message);
            }
            return result;
        }

        #endregion

        #region -- CropImage --
        /// <summary>
        /// Crops the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="cropArea">The crop area.</param>
        /// <returns></returns>
        private System.Drawing.Image CropImage(System.Drawing.Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop as System.Drawing.Image;
        }

        #endregion

        #region -- ResizeImage --
        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="imgToResize">The img to resize.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        private Bitmap ResizeImage(System.Drawing.Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return bmp;
        }
        #endregion

        #region -- SaveJpeg --
        /// <summary>
        /// Saves the JPEG.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="img">The img.</param>
        /// <param name="quality">The quality.</param>
        private void SaveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality
            var qualityParam = 
                new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            // Jpeg image codec
            var jpegCodec = this.getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
            {
                return;
            }

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
            img.Dispose();
        }

        /// <summary>
        /// Gets the encoder info.
        /// </summary>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <returns></returns>
        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            var codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (var i = 0; i < codecs.Length; i++)
            {
                if (codecs[i].MimeType == mimeType)
                {
                    return codecs[i];
                }
            }
            return null;
        }
        #endregion

        //=========================================================================================

        #region -- DeleteUploadImage --
        /// <summary>
        /// Deletes the upload image.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public Dictionary<string, string> DeleteUploadImage(string fileName)
        {
            var result = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                result.Add("result", "error");
                result.Add("msg", "無檔案名稱");
                return result;
            }

            try
            {
                var deletefileName = string.Format(@"{0}\{1}", this.UploadPath, fileName);
                if (System.IO.File.Exists(deletefileName))
                {
                    System.IO.File.Delete(deletefileName);
                    result.Add("result", "Success");
                    result.Add("msg", "");
                }
                else
                {
                    result.Add("result", "Failure");
                    result.Add("msg", "檔案不存在");
                }
            }
            catch (Exception ex)
            {
                result.Add("result", "Exception");
                result.Add("msg", ex.Message);
            }
            return result;
        }
        #endregion

        #region -- DeleteOriginalImage --
        /// <summary>
        /// Deletes the original image.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public Dictionary<string, string> DeleteOriginalImage(string fileName)
        {
            var result = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                result.Add("result", "Failure");
                result.Add("msg", "沒有輸入OrinigalImage檔名");
                return result;
            }

            try
            {
                string file = string.Format(@"{0}\{1}", this.OriginalPath, fileName);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                    result.Add("result", "Success");
                    result.Add("msg", "");
                }
                else
                {
                    result.Add("result", "Failure");
                    result.Add("msg", "檔案不存在");
                }
            }
            catch (Exception ex)
            {
                result.Add("result", "Exception");
                result.Add("msg", ex.Message);
            }
            return result;
        }
        #endregion

        #region -- DeleteCropImage --
        /// <summary>
        /// Deletes the crop image.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public Dictionary<string, string> DeleteCropImage(string fileName)
        {
            var result = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                result.Add("result", "Failure");
                result.Add("msg", "沒有輸入CropImage檔名");
                return result;
            }

            try
            {
                var file = string.Format(@"{0}\{1}", this.CropPath, fileName);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                    result.Add("result", "Success");
                    result.Add("msg", "");
                }
                else
                {
                    result.Add("result", "Failure");
                    result.Add("msg", "檔案不存在");
                }
            }
            catch (Exception ex)
            {
                result.Add("result", "Exception");
                result.Add("msg", ex.Message);
            }
            return result;
        }
        #endregion

    }
}
