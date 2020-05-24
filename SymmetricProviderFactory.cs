using System;
using System.Collections.Generic;
using System.Text;
using Vive.Crypto.Core.Internals.Extensions;

namespace Vive.Crypto
{
    /// <summary>
    /// 对称加密类型
    /// </summary>
    public enum SymmetricProviderType
    {
        AES128 = 1,
        AES192 = 2,
        AES256 = 3,
        DES = 4,
        TripleDES128 = 5,
        TripleDES192 = 6,
        SM4 = 7,
        SM4JAVA = 8,
        SM4JS = 9
    }
    //=====================================================================================================================================================
    /// <summary>
    /// 对称加密工厂类
    /// </summary>
    internal sealed class SymmetricProviderFactory
    {
        public static ISymmetricProvider Create(string providerTypestr = "SM4")
        {
            var providerType = providerTypestr.ToEnum<SymmetricProviderType>();
            return Create(providerType);
        }
        public static ISymmetricProvider Create(SymmetricProviderType providerType = SymmetricProviderType.SM4)
        {
            switch (providerType)
            {
                case SymmetricProviderType.AES128:
                    return new AESEncryptionL128();
                case SymmetricProviderType.AES192:
                    return new AESEncryptionL192();
                case SymmetricProviderType.AES256:
                    return new AESEncryptionL256();
                case SymmetricProviderType.DES:
                    return new DESEncryption();
                case SymmetricProviderType.SM4JAVA:
                    return new SM4ForJavaEncryption();
                case SymmetricProviderType.SM4:
                    return new SM4Encryption();
                case SymmetricProviderType.SM4JS:
                    return new SM4ForJSEncryption();
                case SymmetricProviderType.TripleDES128:
                    return new TripleDESEncryptionL128();
                case SymmetricProviderType.TripleDES192:
                    return new TripleDESEncryptionL192();
                default:
                    return new SM4ForJSEncryption();
            }
        }
    }
}
