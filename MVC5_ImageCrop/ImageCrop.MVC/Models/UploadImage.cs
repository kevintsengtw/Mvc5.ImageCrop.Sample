namespace ImageCrop.MVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UploadImage")]
    public partial class UploadImage
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string OriginalImage { get; set; }

        [StringLength(50)]
        public string CropImage { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
