using System;
using System.Security.Cryptography;
using System.Text;
using HCenter.Encryption.Core;
using HCenter.Encryption.Core.Internals;
using HCenter.Encryption.Core.Internals.Extensions;

// ReSharper disable once CheckNamespace
namespace HCenter.Encryption
{
    #region sealed class DESEncryptionProvider
    /// <summary>
    /// DES 加密提供程序
    /// </summary>
    internal sealed class DESEncryptionProvider : SymmetricEncryptionBase
    {
        /// <summary>
        /// 初始化一个<see cref="DESEncryptionProvider"/>类型的实例
        /// </summary>
        private DESEncryptionProvider()
        {
        }

        /// <summary>
        /// 创建 DES 密钥
        /// </summary>
        /// <returns></returns>
        public static DESKey CreateKey()
        {
            return new DESKey()
            {
                Key = RandomStringGenerator.Generate(),
                IV = RandomStringGenerator.Generate(),
            };
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
        /// <returns></returns>
        public static string Encrypt(string value, string key, string iv, string salt = null,
            OutType outType = OutType.Base64,
            Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrEmpty(iv))
            {
                throw new ArgumentNullException(nameof(iv));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var result = EncryptCore<DESCryptoServiceProvider>(encoding.GetBytes(value),
                ComputeRealValueFunc()(key)(salt)(encoding)(64),
                ComputeRealValueFunc()(iv)(salt)(encoding)(64));

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
        /// <param name="key">DES 密钥对象</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string Encrypt(string value, DESKey key, OutType outType = OutType.Base64,
            Encoding encoding = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Encrypt(value, key.Key, key.IV, outType: outType, encoding: encoding);
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
        /// <returns></returns>
        public static string Decrypt(string value, string key, string iv = null, string salt = null,
            OutType outType = OutType.Base64,
            Encoding encoding = null)
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

            var result = DecryptCore<DESCryptoServiceProvider>(value.GetEncryptBytes(outType),
                ComputeRealValueFunc()(key)(salt)(encoding)(64),
                ComputeRealValueFunc()(iv)(salt)(encoding)(64));

            return encoding.GetString(result);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">DES 密钥对象</param>
        /// <param name="outType">输出类型，默认为<see cref="OutType.Base64"/></param>
        /// <param name="encoding">编码类型，默认为<see cref="Encoding.UTF8"/></param>
        /// <returns></returns>
        public static string Decrypt(string value, DESKey key, OutType outType = OutType.Base64,
            Encoding encoding = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Decrypt(value, key.Key, key.IV, outType: outType, encoding: encoding);
        }
    }
    #endregion

    #region DESEncryption

    /// <summary>
    /// DES  加密提供程序
    /// </summary>
    internal class DESEncryption : AbsSymmetricProvider
    {
        /// <summary>
        /// 初始化一个<see cref="DESEncryption"/>类型的实例
        /// </summary>
        public DESEncryption() : this(OutType.Hex, Encoding.UTF8) { }
        /// <summary>
        /// 初始化一个<see cref="DESEncryption"/>类型的实例
        /// </summary>
        /// <param name="outType"></param>
        /// <param name="encoding"></param>
        public DESEncryption(OutType outType = OutType.Hex, Encoding encoding = null) : base(outType, encoding)
        {
        }

        /// <summary>
        /// 创建密钥
        /// </summary>
        /// <returns></returns>
        public override SymmetricKey CreateKey()
        {
            var key = DESEncryptionProvider.CreateKey();
            return new SymmetricKey() { Key = key.Key, IV = key.IV };
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
            return DESEncryptionProvider.Encrypt(value, key, iv, null, OutType, Encoding);
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
            return DESEncryptionProvider.Decrypt(value, key, iv, null, OutType, Encoding);
        }

    }
    #endregion
}
