using System;
using System.Collections.Generic;
using System.Text;
using Vive.Crypto.Core.Internals.Extensions;

namespace Vive.Crypto
{

    /// <summary>
    /// 非对称加密类型
    /// </summary>
    public enum AsymmetricProviderType
    {
        RSA = 1,
        RSA2 = 2,
        SM2 = 3,
    }
    //=====================================================================================================================================================
    /// <summary>
    /// 非对称加密工厂类
    /// </summary>
    internal sealed class AsymmetricProviderFactory
    {
        public static IAsymmetricProvider Create(string providerTypestr = "RSA")
        {
            var providerType = providerTypestr.ToEnum<AsymmetricProviderType>();
            return Create(providerType);
        }
        public static IAsymmetricProvider Create(AsymmetricProviderType providerType = AsymmetricProviderType.RSA)
        {
            switch (providerType)
            {
                case AsymmetricProviderType.RSA:
                    return new RSAEncryption();
                case AsymmetricProviderType.RSA2:
                    return new RSA2Encryption();
                case AsymmetricProviderType.SM2:
                    return new SM2Encryption();
                default:
                    return new RSAEncryption();
            }
        }
    }
}
