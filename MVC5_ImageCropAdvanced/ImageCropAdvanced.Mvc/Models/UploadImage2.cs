namespace ImageCropAdvanced.Mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UploadImage2
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string OriginalImage { get; set; }

        [StringLength(50)]
        public string CropImage { get; set; }

        public int? SelectionX1 { get; set; }

        public int? SelectionX2 { get; set; }

        public int? SelectionY1 { get; set; }

        public int? SelectionY2 { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
