using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PhotoFiler.Helpers.Helpers;

namespace PhotoFiler.Models
{
    public interface IPreviewablePhotos
    {
        List<IPreviewablePhoto> Retrieve(ErrorGeneratingPreview errorGeneratingPreviewHandler = null);
    }
}
