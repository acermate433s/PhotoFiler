using System.Security.Cryptography;

namespace PhotoFiler.Hasher
{
    public class MD5 : Base62HasherBase<MD5Cng>
    {
        public MD5(int hashLength)
        {
            HashLength = hashLength;
        }
    }

    public class SHA512 : Base62HasherBase<SHA512Managed>
    {
        public SHA512(int hashLength)
        {
            HashLength = hashLength;
        }
    }

    public class RIPEMD160 : Base62HasherBase<RIPEMD160Managed>
    {
        public RIPEMD160(int hashLength)
        {
            HashLength = hashLength;
        }
    }
}