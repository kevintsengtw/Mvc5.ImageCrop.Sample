using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ImageCrop.WebForm.Models;

namespace ImageCrop.WebForm
{
    public partial class _Default : Page
    {
        #region -- Fields & Properties --

        private readonly UploadImageService _service = new UploadImageService();

        private string WebSiteRootPath
        {
            get
            {
                return string.Format(@"http://{0}", HttpContext.Current.Request.Url.Authority);
            }
        }

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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ImageDataBound();
            }
        }

        private void ImageDataBound()
        {
            var source = _service.FindAll();

            this.ListView1.DataSource = source;
            this.ListView1.DataKeyNames = new string[] { "ID" };
            this.ListView1.DataBind();
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var dataItem = e.Item as ListViewDataItem;
                object primaryKey = this.ListView1.DataKeys[dataItem.DisplayIndex]["ID"];
                if (primaryKey != null)
                {
                    Guid imageId = new Guid(primaryKey.ToString());
                    var item = _service.FindOne(imageId);

                    Image originalImage = dataItem.FindControl("Image1") as Image;
                    Image cropImage = dataItem.FindControl("Image2") as Image;
                    HyperLink hyperLink1 = dataItem.FindControl("HyperLink1") as HyperLink;

                    if (item != null)
                    {
                        if (originalImage != null)
                        {
                            if (string.IsNullOrWhiteSpace(item.OriginalImage))
                            {
                                originalImage.Visible = false;
                            }
                            else
                            {
                                originalImage.ImageUrl = string.Format("{0}/{1}/{2}",
                                    this.WebSiteRootPath,
                                    this.OriginalFolder.Replace("~", ""),
                                    item.OriginalImage);

                                originalImage.Height = new Unit(200);
                            }
                        }

                        if (cropImage != null)
                        {
                            if (string.IsNullOrWhiteSpace(item.CropImage))
                            {
                                cropImage.Visible = false;
                            }
                            else
                            {
                                cropImage.ImageUrl = string.Format("{0}/{1}/{2}",
                                    this.WebSiteRootPath,
                                    this.CropFolder.Replace("~", ""),
                                    item.CropImage);
                            }
                        }

                        if (hyperLink1 != null)
                        {
                            hyperLink1.NavigateUrl = string.Concat("Crop.aspx?ID=", item.ID.ToString());
                        }
                    }
                }
            }
        }

        protected void ListView1_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteItem")
            {
                object primaryKey = e.CommandArgument;
                Guid imageId = new Guid(primaryKey.ToString());

                var item = _service.FindOne(imageId);

                if (item != null)
                {
                    _service.Delete(imageId);

                    if (!string.IsNullOrWhiteSpace(item.OriginalImage))
                    {
                        string fileName1 = Server.MapPath(string.Format(@"~/{0}/{1}", OriginalFolder, item.OriginalImage));
                        if (System.IO.File.Exists(fileName1))
                        {
                            System.IO.File.Delete(fileName1);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(item.CropImage))
                    {
                        string fileName2 = Server.MapPath(string.Format(@"~/{0}/{1}", CropFolder, item.CropImage));
                        if (System.IO.File.Exists(fileName2))
                        {
                            System.IO.File.Delete(fileName2);
                        }
                    }

                    ImageDataBound();
                }
            }
        }

    }
}