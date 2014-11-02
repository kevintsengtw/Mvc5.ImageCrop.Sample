using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCropAdvanced.Core
{
    public class UploadImage
    {
        public Guid ID { get; set; }

        public string OriginalImage { get; set; }

        public string CropImage { get; set; }

        public int SelectionX1 { get; set; }

        public int SelectionX2 { get; set; }

        public int SelectionY1 { get; set; }

        public int SelectionY2 { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}
