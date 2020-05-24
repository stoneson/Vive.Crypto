using System;
using System.Collections.Generic;
using System.Text;
using Vive.Crypto.Core.Internals.Extensions;

namespace Vive.Crypto
{
    /// <summary>
    /// 哈希加密类型
    /// </summary>
    public enum HashingProviderType
    {
        HMACMD5 = 1,
        HMACSHA1 = 2,
        HMACSHA256 = 3,
        HMACSHA384 = 4,
        HMACSHA512 = 5,
        MD4 = 6,
        MD5 = 7,
        SHA1 = 8,
        SHA256 = 9,
        SHA384 = 10,
        SHA512 = 11,
        SM3 = 12,
    }
    //=====================================================================================================================================================
    /// <summary>
    /// 哈希加密工厂类
    /// </summary>
    internal sealed class HashingProviderFactory
    {
        public static IHashingProvider Create(string providerTypestr = "SHA256")
        {
            var providerType = providerTypestr.ToEnum<HashingProviderType>();
            return Create(providerType);
        }
        public static IHashingProvider Create(HashingProviderType providerType = HashingProviderType.SHA256)
        {
            switch (providerType)
            {
                case HashingProviderType.HMACMD5:
                    return new HMACMD5Hashing();
                case HashingProviderType.HMACSHA1:
                    return new HMACSHA1Hashing();
                case HashingProviderType.HMACSHA256:
                    return new HMACSHA256Hashing();
                case HashingProviderType.HMACSHA384:
                    return new HMACSHA384Hashing();
                case HashingProviderType.HMACSHA512:
                    return new HMACSHA512Hashing();
                case HashingProviderType.MD4:
                    return new MD4Hashing();
                case HashingProviderType.MD5:
                    return new MD5Hasing();
                case HashingProviderType.SHA1:
                    return new SHA1Hashing();
                case HashingProviderType.SHA256:
                    return new SHA256Hashing();
                case HashingProviderType.SHA384:
                    return new SHA384Hashing();
                case HashingProviderType.SHA512:
                    return new SHA512Hashing();
                case HashingProviderType.SM3:
                    return new SM3Hashing();
                default:
                    return new SHA256Hashing();
            }
        }
    }
}
