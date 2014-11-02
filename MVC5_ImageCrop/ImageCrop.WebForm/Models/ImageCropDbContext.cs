namespace ImageCrop.WebForm.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ImageCropDbContext : DbContext
    {
        public ImageCropDbContext()
            : base("name=ImageCropDbContext")
        {
        }

        public virtual DbSet<UploadImage> UploadImages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
