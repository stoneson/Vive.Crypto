# Vive.Crypto（https://github.com/stoneson/Vive.Crypto）
Vive.Crypto对各种常用的加密算法进行封装，有 Base64、对称加密（DES、3DES、AES、SM4）、非对称加密（RSA、SM2）、Hash(MD4、MD5、HMAC、HMAC-MD5、HMAC-SHA1、HMAC-SHA256、HMAC-SHA384、HMAC-SHA512、SHA、SHA1、SHA256、SHA384、SHA512、SM3)等实现。

内含Java（https://github.com/stoneson/Vive.Crypto/tree/master/SMCrypto/SMJAVA） 和 js（https://github.com/stoneson/Vive.Crypto/tree/master/SMCrypto/SMJS） 的SM2,SM3,SM4 密码类；
实现了C#、Java 和 JS 的SM3,SM4的相互加密解密


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
    
    所有加密方式创建都通过下面的类来处理，只要传相应的加密方式类型名称进去就行：
    
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

国密算法参考：
java:https://github.com/hyfree/SM2_SM3_SM4Encrypt/tree/5e7ec1b2604ae9471dc0baaafb45d07563576e9d
JS:https://github.com/yazhouZhang/SM2-SM3-SM4-SM9
C#:https://www.cnblogs.com/shenblogs/p/10346009.html
其他加密参考：
https://github.com/bing-framework/Bing.Encryption
