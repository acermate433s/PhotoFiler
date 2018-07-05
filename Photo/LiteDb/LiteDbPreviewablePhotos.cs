using Photo.Helpers;
using Photo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.LiteDb
{
    public class LiteDbPreviewablePhotos : IPreviewablePhotos
    {
        public List<IPreviewablePhoto> Retrieve(Helpers.Helpers.ErrorGeneratingPreviewEventHandler errorGeneratingPreviewHandler = null)
        {
            throw new NotImplementedException();
        }
    }
}
