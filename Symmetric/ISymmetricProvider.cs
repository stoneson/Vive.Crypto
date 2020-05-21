using Vive.Crypto.Core;
using Vive.Crypto.Core.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vive.Crypto
{
    /// <summary>
    /// 对称密钥
    /// </summary>
    public class SymmetricKey
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 偏移量
        /// </summary>
        public string IV { get; set; }
    }

    /// <summary>
    /// 对称加密提供程序
    /// </summary>
    public interface ISymmetricProvider
    {
        /// <summary>
      /// 输出类型，默认为<see cref="OutType.Hex"/>
      /// </summary>
        OutType OutType { get; set; }
        /// <summary>
        /// 编码类型，默认为<see cref="Encoding.UTF8"/>
        /// </summary>
        Encoding Encoding { get; set; }
        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        SymmetricKey CreateKey();

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        string Encrypt(string value, string key, string iv = null);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        string Decrypt(string value, string key, string iv = null);
    }
    /// <summary>
    /// 一个抽象对称加密提供程序
    /// </summary>
    internal abstract class AbsSymmetricProvider : ISymmetricProvider
    {
        public OutType OutType { get; set; }
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 初始化一个<see cref="AbsSymmetricProvider"/>类型的实例
        /// </summary>
        public AbsSymmetricProvider(OutType outType = OutType.Hex, Encoding encoding = null)
        {
            OutType = outType;
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            Encoding = encoding;
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public abstract SymmetricKey CreateKey();
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <returns></returns>
        public abstract string Encrypt(string value, string key, string iv = null);

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <returns></returns>
        public abstract string Decrypt(string value, string key, string iv = null);
    }


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

    #region SM4ForJavaEncryption

    /// <summary>
    /// SM4 加密提供程序
    /// </summary>
    internal class SM4ForJavaEncryption : SM4Encryption
    {
        /// <summary>
        /// 初始化一个<see cref="SM4ForJavaEncryption"/>类型的实例
        /// </summary>
        public SM4ForJavaEncryption() : base() { }
        /// <summary>
        /// 初始化一个<see cref="SM4ForJavaEncryption"/>类型的实例
        /// </summary>
        /// <param name="outType"></param>
        /// <param name="encoding"></param>
        public SM4ForJavaEncryption(OutType outType = OutType.Hex, Encoding encoding = null) : base(OutType.Hex, Encoding.UTF8) { }
    }
    #endregion

    #region SM4ForJSEncryption

    /// <summary>
    /// SM4 加密提供程序
    /// </summary>
    internal class SM4ForJSEncryption : SM4Encryption
    {
        /// <summary>
        /// 初始化一个<see cref="SM4Encryption"/>类型的实例
        /// </summary>
        public SM4ForJSEncryption():base() {  }
        /// <summary>
        /// 初始化一个<see cref="SM4ForJSEncryption"/>类型的实例
        /// </summary>
        /// <param name="outType"></param>
        /// <param name="encoding"></param>
        public SM4ForJSEncryption(OutType outType = OutType.Hex, Encoding encoding = null) : base(OutType.Hex, Encoding.UTF8) { }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <returns></returns>
        public override string Encrypt(string value, string key, string iv = null)
        {
            sm4.secretKey = key;
            sm4.hexString = false;
            if (!string.IsNullOrWhiteSpace(iv) && iv != null && iv != "")
            {
                sm4.iv = iv;
                return sm4.EncryptCBC4JS(value);
            }
            return sm4.EncryptECB4JS(value);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <returns></returns>
        public override string Decrypt(string value, string key, string iv = null)
        {
            sm4.secretKey = key;
            sm4.hexString = false;
            if (!string.IsNullOrWhiteSpace(iv) && iv != null && iv != "")
            {
                sm4.iv = iv;
                return sm4.DecryptCBC4JS(value);
            }
            return sm4.DecryptECB4JS(value);
        }
    }
    #endregion

    #region SM4Encryption
    /// <summary>
    /// SM4 加密提供程序
    /// </summary>
    internal class SM4Encryption : AbsSymmetricProvider
    {
        protected SMCrypto.SM4 sm4 = null;
        /// <summary>
        /// 初始化一个<see cref="SM4Encryption"/>类型的实例
        /// </summary>
        public SM4Encryption() : this(OutType.Hex, Encoding.UTF8) { }
        /// <summary>
        /// 初始化一个<see cref="SM4Encryption"/>类型的实例
        /// </summary>
        /// <param name="outType"></param>
        /// <param name="encoding"></param>
        public SM4Encryption(OutType outType = OutType.Hex, Encoding encoding = null):base(outType, encoding)
        {
            sm4 = new SMCrypto.SM4();
        }
        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override SymmetricKey CreateKey()
        {
            var Key = Core.Internals.RandomStringGenerator.Generate(16);
            var IV = Core.Internals.RandomStringGenerator.Generate(16);
            return new SymmetricKey() { Key = Key, IV = IV };
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <returns></returns>
        public override string Encrypt(string value, string key, string iv = null)
        {
            sm4.secretKey = key; 
            sm4.hexString = false;
            if (!string.IsNullOrWhiteSpace(iv) && iv != null && iv != "")
            {
                sm4.iv = iv;
                return sm4.EncryptCBC( value);
            }
            return sm4.EncryptECB(value);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <returns></returns>
        public override string Decrypt(string value, string key, string iv = null)
        {
            sm4.secretKey = key;
            sm4.hexString = false;
            if (!string.IsNullOrWhiteSpace(iv) && iv != null && iv != "")
            {
                sm4.iv = iv;
                return sm4.DecryptCBC(value);
            }
            return sm4.DecryptECB(value);
        }
    }
    #endregion
}
