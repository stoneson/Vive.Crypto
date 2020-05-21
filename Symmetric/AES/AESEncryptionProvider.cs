using System;
using System.Security.Cryptography;
using System.Text;
using HCenter.Encryption.Core;
using HCenter.Encryption.Core.Internals.Extensions;

// ReSharper disable once CheckNamespace
namespace HCenter.Encryption
{
    #region sealed class AESEncryptionProvider
    /// <summary>
    /// AES 加密提供程序
    /// </summary>
    internal sealed class AESEncryptionProvider : SymmetricEncryptionBase
    {
        /// <summary>
        /// 初始化一个<see cref="AESEncryptionProvider"/>类型的实例
        /// </summary>
        private AESEncryptionProvider() { }

        /// <summary>
        /// 创建 AES 密钥
        /// </summary>
        /// <param name="size">密钥长度类型，默认为<see cref="AESKeySizeType.L256"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static AESKey CreateKey(AESKeySizeType size = AESKeySizeType.L256, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            using (var provider = new AesCryptoServiceProvider() { KeySize = (int)size })
            {
                return new AESKey()
                {
                    Key = encoding.GetString(provider.Key),
                    IV = encoding.GetString(provider.IV),
                    Size = size
                };
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="salt">加盐</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <param name="keySize">密钥长度类型，默认为<see cref="AESKeySizeType.L256"/></param>
        /// <returns></returns>
        public static string Encrypt(string value, string key, string iv = null, string salt = null,
            OutType outType = OutType.Base64,
            Encoding encoding = null, AESKeySizeType keySize = AESKeySizeType.L256)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var result = EncryptCore<AesCryptoServiceProvider>(encoding.GetBytes(value),
                ComputeRealValueFunc()(key)(salt)(encoding)((int)keySize),
                ComputeRealValueFunc()(iv)(salt)(encoding)(128));

            if (outType == OutType.Base64)
            {
                return Convert.ToBase64String(result);
            }

            return result.ToHexString();
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">AES 密钥对象</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string Encrypt(string value, AESKey key, OutType outType = OutType.Base64,
            Encoding encoding = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Encrypt(value, key.Key, key.IV, outType: outType, encoding: encoding, keySize: key.Size);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="salt">加盐</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <param name="keySize">密钥长度类型，默认为<see cref="AESKeySizeType.L256"/></param>
        /// <returns></returns>
        public static string Decrypt(string value, string key, string iv = null, string salt = null,
            OutType outType = OutType.Base64,
            Encoding encoding = null, AESKeySizeType keySize = AESKeySizeType.L256)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var result = DecryptCore<AesCryptoServiceProvider>(value.GetEncryptBytes(outType),
                ComputeRealValueFunc()(key)(salt)(encoding)((int)keySize),
                ComputeRealValueFunc()(iv)(salt)(encoding)(128));

            return encoding.GetString(result);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">AES 密钥对象</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string Decrypt(string value, AESKey key, OutType outType = OutType.Base64,
            Encoding encoding = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Decrypt(value, key.Key, key.IV, outType: outType, encoding: encoding, keySize: key.Size);
        }
    }
    #endregion

    #region AESEncryptionL256
    /// <summary>
    /// AES L256 加密提供程序
    /// </summary>
    internal class AESEncryptionL256 : AbsSymmetricProvider
    {
        /// <summary>
        /// 初始化一个<see cref="AESEncryptionL256"/>类型的实例
        /// </summary>
        public AESEncryptionL256() : this(OutType.Hex, Encoding.UTF8) { }
        /// <summary>
        /// 初始化一个<see cref="AESEncryptionL256"/>类型的实例
        /// </summary>
        /// <param name="outType"></param>
        /// <param name="encoding"></param>
        public AESEncryptionL256(OutType outType = OutType.Hex, Encoding encoding = null) : base(outType, encoding)
        {
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override SymmetricKey CreateKey()
        {
            var key = AESEncryptionProvider.CreateKey(AESKeySizeType.L256, Encoding);
            return new SymmetricKey() { Key = key.Key, IV = key.IV };
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Encrypt(string value, string key, string iv = null)
        {
            return AESEncryptionProvider.Encrypt(value, key, iv, null, OutType, Encoding, AESKeySizeType.L256);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Decrypt(string value, string key, string iv = null)
        {
            return AESEncryptionProvider.Decrypt(value, key, iv, null,OutType, Encoding, AESKeySizeType.L256);
        }

    }
    #endregion

    #region AESEncryptionL128
    /// <summary>
    /// AES L128 加密提供程序
    /// </summary>
    internal class AESEncryptionL128 : AbsSymmetricProvider
    {
        /// <summary>
        /// 初始化一个<see cref="AESEncryptionL128"/>类型的实例
        /// </summary>
        public AESEncryptionL128() : this(OutType.Hex, Encoding.UTF8) { }
        /// <summary>
        /// 初始化一个<see cref="AESEncryptionL128"/>类型的实例
        /// </summary>
        /// <param name="outType"></param>
        /// <param name="encoding"></param>
        public AESEncryptionL128(OutType outType = OutType.Hex, Encoding encoding = null) : base(outType, encoding)
        {
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override SymmetricKey CreateKey()
        {
            var key = AESEncryptionProvider.CreateKey(AESKeySizeType.L128, Encoding);
            return new SymmetricKey() { Key = key.Key, IV = key.IV };
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Encrypt(string value, string key, string iv = null)
        {
            return AESEncryptionProvider.Encrypt(value, key, iv, null, OutType, Encoding, AESKeySizeType.L128);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Decrypt(string value, string key, string iv = null)
        {
            return AESEncryptionProvider.Decrypt(value, key, iv, null, OutType, Encoding, AESKeySizeType.L128);
        }

    }
    #endregion

    #region AESEncryptionL192

    /// <summary>
    /// AES L192 加密提供程序
    /// </summary>
    internal class AESEncryptionL192 : AbsSymmetricProvider
    {
        /// <summary>
        /// 初始化一个<see cref="AESEncryptionL192"/>类型的实例
        /// </summary>
        public AESEncryptionL192() : this(OutType.Hex, Encoding.UTF8) { }
        /// <summary>
        /// 初始化一个<see cref="AESEncryptionL192"/>类型的实例
        /// </summary>
        /// <param name="outType"></param>
        /// <param name="encoding"></param>
        public AESEncryptionL192(OutType outType = OutType.Hex, Encoding encoding = null) : base(outType, encoding)
        {
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override SymmetricKey CreateKey()
        {
            var key = AESEncryptionProvider.CreateKey(AESKeySizeType.L192, Encoding);
            return new SymmetricKey() { Key = key.Key, IV = key.IV };
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Encrypt(string value, string key, string iv = null)
        {
            return AESEncryptionProvider.Encrypt(value, key, iv, null, OutType, Encoding, AESKeySizeType.L192);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">加密偏移量</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public override string Decrypt(string value, string key, string iv = null)
        {
            return AESEncryptionProvider.Decrypt(value, key, iv, null, OutType, Encoding, AESKeySizeType.L192);
        }

    }
    #endregion
}
