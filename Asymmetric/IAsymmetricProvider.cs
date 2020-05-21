using Vive.Crypto.Core;
using Vive.Crypto.Core.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vive.Crypto
{
    /// <summary>
    /// 非对称密钥
    /// </summary>
    public class AsymmetricKey
    {
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublickKey { get; set; }

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }
    }

    /// <summary>
    /// 非对称加密提供程序
    /// </summary>
    public interface IAsymmetricProvider
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
        /// 密钥类型
        /// </summary>
        RSAKeyType KeyType { get; set; }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="size">RSA 密钥长度类型</param>
        /// <returns></returns>
        AsymmetricKey CreateKey(RSAKeySizeType size = RSAKeySizeType.L2048);

        /// <summary>
        /// 使用指定公钥加密字符串
        /// </summary>
        /// <param name="value">要加密的明文字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        string Encrypt(string value, string publicKey);


        /// <summary>
        /// 使用指定私钥解密字符串
        /// </summary>
        /// <param name="value">要解密的密文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        string Decrypt(string value, string privateKey);

        /// <summary>
        /// 使用指定密钥对明文进行签名，返回明文签名的字符串
        /// </summary>
        /// <param name="source">要签名的明文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        string SignData(string source, string privateKey);

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密得到的明文</param>
        /// <param name="signData">明文签名字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        bool VerifyData(string source, string signData, string publicKey);
    }
    /// <summary>
    /// 一个抽象非对称加密提供程序
    /// </summary>
    internal abstract class AbsAsymmetricProvider : IAsymmetricProvider
    {
        public OutType OutType { get; set; }
        public Encoding Encoding { get; set; }
        public RSAKeyType KeyType { get; set; }

        /// <summary>
        /// 初始化一个<see cref="AbsAsymmetricProvider"/>类型的实例
        /// </summary>
        public AbsAsymmetricProvider(OutType outType = OutType.Hex, RSAKeyType keyType = RSAKeyType.Xml, Encoding encoding = null)
        {
            OutType = outType;
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            Encoding = encoding;
            KeyType = keyType;
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <returns></returns>
        public abstract AsymmetricKey CreateKey(RSAKeySizeType size = RSAKeySizeType.L2048);
        /// <summary>
        /// 使用指定公钥加密字符串
        /// </summary>
        /// <param name="value">要加密的明文字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public abstract string Encrypt(string value, string publicKey);

        /// <summary>
        /// 使用指定私钥解密字符串
        /// </summary>
        /// <param name="value">要解密的密文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public abstract string Decrypt(string value, string privateKey);

        /// <summary>
        /// 使用指定密钥对明文进行签名，返回明文签名的字符串
        /// </summary>
        /// <param name="source">要签名的明文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns> $"{r}|{s}"</returns>
        public abstract string SignData(string source, string privateKey);

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密得到的明文</param>
        /// <param name="signData">明文签名字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public abstract bool VerifyData(string source, string signData, string publicKey);
    }
    /// <summary>
    /// 非对称加密类型
    /// </summary>
    public enum AsymmetricProviderType
    {
        RSA = 1,
        RSA2 = 2,
        SM2 = 3,
    }
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

    #region SM2Encryption

    /// <summary>
    /// SM2 加密提供程序
    /// </summary>
    internal class SM2Encryption : AbsAsymmetricProvider
    {
        public SM2Encryption() : this(OutType.Hex, RSAKeyType.Xml, Encoding.UTF8)
        {
        }
        /// <summary>
        /// 初始化一个<see cref="SM2Encryption"/>类型的实例
        /// </summary>
        public SM2Encryption(OutType outType = OutType.Hex, RSAKeyType keyType = RSAKeyType.Xml, Encoding encoding = null):base(outType, keyType, encoding)
        {
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <returns></returns>
        public override AsymmetricKey CreateKey(RSAKeySizeType size = RSAKeySizeType.L2048)
        {
            // 私钥  
            string prik = "";
            // 公钥  
            string pubk = "";
            SMCrypto.SM2.GenerateKeyPair(out prik, out pubk); //获取公钥 私钥
            return new AsymmetricKey() { PrivateKey = prik, PublickKey = pubk };
        }
        /// <summary>
        /// 使用指定公钥加密字符串
        /// </summary>
        /// <param name="value">要加密的明文字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public override string Encrypt(string value, string publicKey)
        {
            return SMCrypto.SM2.Encrypt(publicKey, value);
        }

        /// <summary>
        /// 使用指定私钥解密字符串
        /// </summary>
        /// <param name="value">要解密的密文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns></returns>
        public override string Decrypt(string value, string privateKey)
        {
            return Encoding.UTF8.GetString(SMCrypto.SM2.Decrypt(privateKey, value));
        }

        /// <summary>
        /// 使用指定密钥对明文进行签名，返回明文签名的字符串
        /// </summary>
        /// <param name="source">要签名的明文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <returns> $"{r}|{s}"</returns>
        public override string SignData(string source, string privateKey)
        {
            return SMCrypto.SM2.Sm2Sign(source, privateKey, KeyType == RSAKeyType.Xml);
        }

        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密得到的明文</param>
        /// <param name="signData">明文签名字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <returns></returns>
        public override bool VerifyData(string source, string signData, string publicKey)
        {
            return SMCrypto.SM2.Verify(source, signData, publicKey, KeyType == RSAKeyType.Xml);
        }
    }
    #endregion
}
