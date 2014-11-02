using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using ImageCrop.Models;
using ImageCrop;

namespace ImageCrop.WebForm
{
	public partial class Crop : System.Web.UI.Page
	{
		#region -- Fields & Properties --

		private UploadImageService service = new UploadImageService();

		private Guid _ImageID;
		public Guid ImageID
		{
			get { return this._ImageID; }
			set { this._ImageID = value; }
		}

		public UploadImage CurrentImage
		{
			get
			{
				if (ImageID == null)
				{
					return null;
				}
				else
				{
					return service.FindOne(this.ImageID);
				}
			}
		}

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

		private string UploadPath
		{
			get
			{
				return Server.MapPath(@"~/" + this.UploadFolder);
			}
		}

		private string OriginalPath
		{
			get
			{
				return Server.MapPath(@"~/" + this.OriginalFolder);
			}
		}

		private string CropPath
		{
			get
			{
				return Server.MapPath(@"~/" + this.CropFolder);
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			Page_Init();

			if (!Page.IsPostBack)
			{
				SetDefault();
			}
			else
			{
				LoadCropImage();
			}
		}

		#region -- Page_Init --
		/// <summary>
		/// Page_s the init.
		/// </summary>
		private void Page_Init()
		{
			if (Request.QueryString["ID"] == null)
			{
				ClientScriptHelper.ShowMessage(this.Page, "未輸入ImageID", "Default.aspx", RegisterScriptType.Start);
			}
			else
			{
				Guid id;
				if (!Guid.TryParse(Request.QueryString["ID"], out id))
				{
					ClientScriptHelper.ShowMessage(this.Page, "ImageID錯誤", "Default.aspx", RegisterScriptType.Start);
				}
				else
				{
					this.ImageID = id;
				}
			}
		}
		#endregion

		#region -- SetDefault --

		/// <summary>
		/// Sets the default.
		/// </summary>
		private void SetDefault()
		{
			if (this.CurrentImage != null)
			{
				this.Image1.Src = string.Format("{0}/{1}/{2}",
					this.WebSiteRootPath,
					this.OriginalFolder.Replace("~", ""),
					this.CurrentImage.OriginalImage);

				this.Image2.Src = string.Format("{0}/{1}/{2}",
					this.WebSiteRootPath,
					this.OriginalFolder.Replace("~", ""),
					this.CurrentImage.OriginalImage);

				LoadCropImage();
			}
		}

		#endregion

		#region -- LoadCropImage -

		/// <summary>
		/// Loads the crop image.
		/// </summary>
		private void LoadCropImage()
		{
			if (this.ImageID != null)
			{
				var instance = service.FindOne(this.ImageID);

				if (instance != null && !string.IsNullOrWhiteSpace(instance.CropImage))
				{
					this.Image3.ImageUrl = string.Format("{0}/{1}/{2}",
						this.WebSiteRootPath,
						this.CropFolder.Replace("~", ""),
						instance.CropImage);

					this.Panel1.Visible = true;
				}
				else
				{
					this.Panel1.Visible = false;
				}
			}
		}
		#endregion

		protected void Button1_Click(object sender, EventArgs e)
		{
			Response.Redirect("Default.aspx");
		}

		protected void Button2_Click(object sender, EventArgs e)
		{
			SaveCropImage();
		}

		#region -- SaveCropImage --
		/// <summary>
		/// Saves the crop image.
		/// </summary>
		private void SaveCropImage()
		{
			bool isNullOfsectionValue = this.x1.Value == null
				&& this.x2.Value == null
				&& this.y1.Value == null
				&& this.y2.Value == null;

			if (isNullOfsectionValue)
			{
				ClientScriptHelper.ShowMessage(this.Page, "請選擇相片裁剪區域", RegisterScriptType.Start);
			}
			else
			{
				CropImageUtility cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, this.CropPath);
				Dictionary<string, string> result = cropUtils.ProcessImageCrop
				(
					this.CurrentImage,
					new int[]
			{ 
				this.x1.Value.ConvertToInt(),
				this.x2.Value.ConvertToInt(),
				this.y1.Value.ConvertToInt(),
				this.y2.Value.ConvertToInt()
			}
				);

				if (!result["result"].Equals("Success", StringComparison.OrdinalIgnoreCase))
				{
					ClientScriptHelper.ShowMessage(this.Page, result["msg"], RegisterScriptType.Start);
				}
				else
				{
					//裁剪圖片檔名儲存到資料庫
					service.Update(this.ImageID, result["CropImage"]);

					//如果有之前的裁剪圖片，則刪除
					if (!string.IsNullOrWhiteSpace(result["OldCropImage"]))
					{
						cropUtils.DeleteCropImage(result["OldCropImage"]);
					}

					//載入裁剪圖片檔
					LoadCropImage();

					ClientScriptHelper.ShowMessage(this.Page, "相片裁剪完成", RegisterScriptType.Start);
				}
			}
		}
		#endregion

	}
}