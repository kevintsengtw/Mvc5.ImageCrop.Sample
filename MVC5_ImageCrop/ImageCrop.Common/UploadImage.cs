using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCrop.Common
{
    public class UploadImage
    {
        public Guid ID { get; set; }

        public string OriginalImage { get; set; }

        public string CropImage { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
