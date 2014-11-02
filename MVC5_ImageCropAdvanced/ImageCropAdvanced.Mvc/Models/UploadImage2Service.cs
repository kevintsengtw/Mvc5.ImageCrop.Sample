using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageCropAdvanced.Mvc.Models
{
    public class UploadImage2Service
    {
        /// <summary>
        /// Finds the one.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public UploadImage2 FindOne(Guid id)
        {
            using (var db = new DemoDbContext())
            {
                var query = db.UploadImage2.SingleOrDefault(x => x.ID == id);
                return query;
            }
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <returns></returns>
        public List<UploadImage2> FindAll()
        {
            using (var db = new DemoDbContext())
            {
                var query = db.UploadImage2.OrderByDescending(x => x.CreateDate);
                return query.ToList();
            }
        }

        /// <summary>
        /// Adds the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Add(UploadImage2 instance)
        {
            using (var db = new DemoDbContext())
            {
                db.UploadImage2.Add(instance);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Update(UploadImage2 instance)
        {
            using (var db = new DemoDbContext())
            {
                var target = db.UploadImage2.SingleOrDefault(x => x.ID == instance.ID);
                if (target == null) return;

                target.CropImage = instance.CropImage;
                target.SelectionX1 = instance.SelectionX1;
                target.SelectionX2 = instance.SelectionX2;
                target.SelectionY1 = instance.SelectionY1;
                target.SelectionY2 = instance.SelectionY2;
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
                using (var db = new DemoDbContext())
                {
                    var target = db.UploadImage2.SingleOrDefault(x => x.ID == id);
                    db.UploadImage2.Remove(target);
                    db.SaveChanges();
                }
            }
        }

    }
}