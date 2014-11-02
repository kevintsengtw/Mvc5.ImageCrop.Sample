using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using ImageCrop.Models;

namespace ImageCrop.WebForm
{
	public partial class Upload : System.Web.UI.Page
	{
		#region -- Fields & Properties --

		private UploadImageService service = new UploadImageService();

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

		public string UploadPath
		{
			get
			{
				return Server.MapPath(@"~/" + this.UploadFolder);
			}
		}

		public string OriginalPath
		{
			get
			{
				return Server.MapPath(@"~/" + this.OriginalFolder);
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				Panel1.Visible = false;
			}
		}

		protected void Button_Upload_Click(object sender, EventArgs e)
		{
			ProcessUpload();

			if (Session["Upload_File"] == null || string.IsNullOrWhiteSpace(Session["Upload_File"].ToString()))
			{
				Panel1.Visible = false;
			}
			else
			{
				Panel1.Visible = true;

				this.Image_Upload.ImageUrl = string.Format("{0}/{1}/{2}",
					this.WebSiteRootPath,
					this.UploadFolder.Replace("~", ""),
					Session["Upload_File"].ToString());
			}
		}

		#region -- ProcessUpload --
		/// <summary>
		/// Processes the upload.
		/// </summary>
		private void ProcessUpload()
		{
			string fileName = string.Empty;

			if (FileUpload1.HasFile)
			{
				HttpPostedFileBase uploadFile = new HttpPostedFileWrapper(FileUpload1.PostedFile);

				CropImageUtility cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, "");
				Dictionary<string, string> result = cropUtils.ProcessUploadImage(uploadFile);

				if (!result["result"].Equals("Success", StringComparison.OrdinalIgnoreCase))
				{
					Session["Upload_File"] = null;
					ClientScriptHelper.ShowMessage(this.Page, result["msg"], RegisterScriptType.Start);
				}
				else
				{
					Session["Upload_File"] = result["msg"];
				}
			}
		}

		#endregion

		protected void Button_Cancel_Click(object sender, EventArgs e)
		{
			CropImageUtility cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, "");
			cropUtils.DeleteUploadImage(Session["Upload_File"].ToString());

			Session["Upload_File"] = null;
			Server.Transfer("Upload.aspx");
		}

		protected void Button_Save_Click(object sender, EventArgs e)
		{
			ProcessSave();
		}

		#region -- ProcessSave --
		/// <summary>
		/// Processes the save.
		/// </summary>
		private void ProcessSave()
		{
			if (Session["Upload_File"] != null && !string.IsNullOrWhiteSpace(Session["Upload_File"].ToString()))
			{
				string fileName = Session["Upload_File"].ToString();

				CropImageUtility cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, "");
				Dictionary<string, string> result = cropUtils.SaveUploadImageToOriginalFolder(fileName);

				if (!result["result"].Equals("Success", StringComparison.OrdinalIgnoreCase))
				{
					ClientScriptHelper.ShowMessage(this.Page, result["msg"], RegisterScriptType.Start);
				}
				else
				{
					UploadImage instance = new UploadImage();
					instance.ID = Guid.NewGuid();
					instance.OriginalImage = fileName;
					instance.CreateDate = DateTime.Now;
					instance.UpdateDate = instance.CreateDate;

					service.Add(instance);

					this.HiddenField_ID.Value = instance.ID.ToString();

					this.Image_Upload.ImageUrl = string.Format("{0}/{1}/{2}",
						this.WebSiteRootPath,
						this.OriginalFolder.Replace("~", ""),
						fileName);

					this.Button_Save.Visible = false;
					this.Button_Cancel.Visible = false;

					if (!string.IsNullOrWhiteSpace(this.HiddenField_ID.Value))
					{
						this.Button_Crop.Visible = true;
					}
				}
			}
		}
		#endregion

		protected void Button_Crop_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.HiddenField_ID.Value))
			{
				Response.Redirect(string.Concat("Crop.aspx?ID=", this.HiddenField_ID.Value));
			}
		}

		protected void Button_Default_Click(object sender, EventArgs e)
		{
			Response.Redirect("Default.aspx");
		}
	}
}