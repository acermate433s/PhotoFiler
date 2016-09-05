using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoFiler.Models
{
    public interface IPreviewablePhotos
    {
        List<IPreviewablePhoto> Retrieve();
    }
}
