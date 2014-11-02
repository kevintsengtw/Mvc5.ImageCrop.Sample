using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageCrop.MVC.Models
{
    public class UploadImageService
    {
        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ImageCrop.Common.UploadImage FindOne(Guid id)
        {
            using (var db = new ImageCropDbContext())
            {
                var model = db.UploadImages.SingleOrDefault(x => x.ID == id);

                Mapper.CreateMap<UploadImage, ImageCrop.Common.UploadImage>();
                var result = Mapper.Map<ImageCrop.Common.UploadImage>(model);

                return result;
            }
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public List<ImageCrop.Common.UploadImage> FindAll()
        {
            using (var db = new ImageCropDbContext())
            {
                var query = db.UploadImages.OrderByDescending(x => x.CreateDate);

                Mapper.CreateMap<UploadImage, ImageCrop.Common.UploadImage>();
                var result = Mapper.Map<List<ImageCrop.Common.UploadImage>>(query.ToList());

                return result;
            }
        }

        /// <summary>
        /// Adds the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Add(ImageCrop.Common.UploadImage instance)
        {
            using (var db = new ImageCropDbContext())
            {
                Mapper.CreateMap<ImageCrop.Common.UploadImage, UploadImage>();
                var model = Mapper.Map<UploadImage>(instance);
                db.UploadImages.Add(model);
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
            using (var db = new ImageCropDbContext())
            {
                var target = db.UploadImages.SingleOrDefault(x => x.ID == id);
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
                using (var db = new ImageCropDbContext())
                {
                    var target = db.UploadImages.SingleOrDefault(x => x.ID == id);
                    db.UploadImages.Remove(target);
                    db.SaveChanges();
                }
            }
        }
    }
}