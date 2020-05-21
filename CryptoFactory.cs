using System;
using System.Collections.Generic;
using System.Text;

namespace Vive.Crypto
{
    /// <summary>
    /// 所有加密创建工厂
    /// </summary>
    public sealed class CryptoFactory
    {
        /// <summary>
        /// 创建哈希加密提供程序
        /// </summary>
        /// <param name="providerTypestr"></param>
        /// <returns></returns>
        public static IHashingProvider CreateHashing(string providerTypestr = "SHA256")
        {
            return HashingProviderFactory.Create(providerTypestr);
        }
        /// <summary>
        /// 创建哈希加密提供程序
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static IHashingProvider CreateHashing(HashingProviderType providerType = HashingProviderType.SHA256)
        {
            return HashingProviderFactory.Create(providerType);
        }
        //---------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 创建非对称加密提供程序
        /// </summary>
        /// <param name="providerTypestr"></param>
        /// <returns></returns>
        public static IAsymmetricProvider CreateAsymmetric(string providerTypestr = "RSA")
        {
            return AsymmetricProviderFactory.Create(providerTypestr);
        }
        /// <summary>
        /// 创建非对称加密提供程序
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static IAsymmetricProvider CreateAsymmetric(AsymmetricProviderType providerType = AsymmetricProviderType.RSA)
        {
            return AsymmetricProviderFactory.Create(providerType);
        }
        //---------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 创建对称加密提供程序
        /// </summary>
        /// <param name="providerTypestr"></param>
        /// <returns></returns>
        public static ISymmetricProvider CreateSymmetric(string providerTypestr = "SM4")
        {
            return SymmetricProviderFactory.Create(providerTypestr);
        }
        /// <summary>
        /// 创建对称加密提供程序
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static ISymmetricProvider CreateSymmetric(SymmetricProviderType providerType = SymmetricProviderType.SM4)
        {
            return SymmetricProviderFactory.Create(providerType);
        }
    }
}
