﻿using System.IO;

namespace PhotoFiler.Models
{
    public interface IHashedPhotoPreviewer<THashedPhoto> where THashedPhoto : IHashedPhoto
    {
        THashedPhoto Photo { get; set; }
        DirectoryInfo PreviewLocation { get; set; }

        void Generate();

        byte[] Preview();

        byte[] View();
    }
}