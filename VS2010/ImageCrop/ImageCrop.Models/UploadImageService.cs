using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageCrop.Models
{
	public class UploadImageService
	{
		/// <summary>
		/// Finds the one.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public UploadImage FindOne(Guid id)
		{
			using (ImageCropEntities db = new ImageCropEntities())
			{
				var query = db.UploadImage.SingleOrDefault(x => x.ID == id);
				return query;
			}
		}

		/// <summary>
		/// Finds all.
		/// </summary>
		/// <returns></returns>
		public List<UploadImage> FindAll()
		{
			using (ImageCropEntities db = new ImageCropEntities())
			{
				var query = db.UploadImage.OrderByDescending(x => x.CreateDate);
				return query.ToList();
			}
		}

		/// <summary>
		/// Adds the specified instance.
		/// </summary>
		/// <param name="instance">The instance.</param>
		public void Add(UploadImage instance)
		{
			using (ImageCropEntities db = new ImageCropEntities())
			{
				db.UploadImage.AddObject(instance);
				db.SaveChanges();
			}
		}

		/// <summary>
		/// Updates the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="cropImage">The crop image.</param>
		public void Update(Guid id, string cropImage)
		{
			using (ImageCropEntities db = new ImageCropEntities())
			{
				var target = db.UploadImage.SingleOrDefault(x => x.ID == id);
				target.CropImage = cropImage;
				target.UpdateDate = DateTime.Now;
				db.SaveChanges();
			}
		}

		/// <summary>
		/// Deletes the specified id.
		/// </summary>
		/// <param name="id">The id.</param>
		public void Delete(Guid id)
		{
			var item = this.FindOne(id);
			if (item != null)
			{
				using (ImageCropEntities db = new ImageCropEntities())
				{
					var target = db.UploadImage.SingleOrDefault(x => x.ID == id);
					db.DeleteObject(target);
					db.SaveChanges();
				}
			}
		}

	}
}