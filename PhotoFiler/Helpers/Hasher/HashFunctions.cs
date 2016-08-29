using System.Security.Cryptography;

namespace PhotoFiler.Helpers.Hasher
{
    public class MD5 : Base62HasherBase<MD5Cng>
    {
    }

    public class SHA512 : Base62HasherBase<SHA512Managed>
    {
    }

    public class RIPEMD160 : Base62HasherBase<RIPEMD160Managed>
    {
    }
}