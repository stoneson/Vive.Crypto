
using Vive.Crypto.Core.Internals.Extensions;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Vive.Crypto.SMCrypto
{
    internal class SM2
    {
        public static Dictionary<string, string> GetKeyPair()
        {
            SM2CryptoServiceProvider sm2 = SM2CryptoServiceProvider.Instance;
            AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
            ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
            ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
            BigInteger privateKey = ecpriv.D;
            ECPoint publicKey = ecpub.Q;

            var result = new Dictionary<string, string>();
            result.Add("公钥", Encoding.UTF8.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper());
            result.Add("私钥", Encoding.UTF8.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper());
            return result;
        }
        /// <summary>
        /// 获取生成的公钥私钥
        /// </summary>
        /// <param name="prik">out prik 私钥</param>
        /// <param name="pubk">out pubk 公钥</param>
        /// <returns></returns>
        public static void GenerateKeyPair(out string prik, out string pubk)
        {
            var dicKeyPair = GetKeyPair();
            prik = dicKeyPair["私钥"];
            pubk = dicKeyPair["公钥"];
        }
        public static Tuple<string, string> GetTupleKeyPair()
        {
            var dicKeyPair = GetKeyPair();
            var prik = dicKeyPair["私钥"];
            var pubk = dicKeyPair["公钥"];

            return new Tuple<string, string>(pubk, prik);
        }

        //public static X509Certificate MakeRootCert(string filePath, IDictionary subjectNames)
        //{
        //    AsymmetricCipherKeyPair keypair = SM2CryptoServiceProvider.Instance.ecc_key_pair_generator.GenerateKeyPair();
        //    ECPublicKeyParameters pubKey = (ECPublicKeyParameters)keypair.Public; //CA公钥     
        //    ECPrivateKeyParameters priKey = (ECPrivateKeyParameters)keypair.Private;    //CA私钥     



        //    X509Name issuerDN = new X509Name(GetDictionaryKeys(subjectNames), subjectNames);
        //    X509Name subjectDN = issuerDN;  //自签证书，两者一样  

        //    SM2X509V3CertificateGenerator sm2CertGen = new SM2X509V3CertificateGenerator();
        //    //X509V3CertificateGenerator sm2CertGen = new X509V3CertificateGenerator();  
        //    sm2CertGen.SetSerialNumber(new BigInteger(128, new Random()));   //128位     
        //    sm2CertGen.SetIssuerDN(issuerDN);
        //    sm2CertGen.SetNotBefore(DateTime.UtcNow.AddDays(-1));
        //    sm2CertGen.SetNotAfter(DateTime.UtcNow.AddDays(365 * 10));
        //    sm2CertGen.SetSubjectDN(subjectDN);
        //    sm2CertGen.SetPublicKey(pubKey); //公钥  


        //    sm2CertGen.SetSignatureAlgorithm("SM3WITHSM2");

        //    sm2CertGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(true));
        //    sm2CertGen.AddExtension(X509Extensions.SubjectKeyIdentifier, false, new SubjectKeyIdentifierStructure(pubKey));
        //    sm2CertGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, new AuthorityKeyIdentifierStructure(pubKey));
        //    sm2CertGen.AddExtension(X509Extensions.KeyUsage, true, new KeyUsage(6));


        //    Org.BouncyCastle.X509.X509Certificate sm2Cert = sm2CertGen.Generate(keypair);

        //    sm2Cert.CheckValidity();
        //    sm2Cert.Verify(pubKey);

        //    return sm2Cert;
        //}

        public static String Encrypt(string publicKey, string data)
        {
            return Encrypt(publicKey.hexToByte(), Encoding.UTF8.GetBytes(data));
        }

        public static String Encrypt(byte[] publicKey, byte[] data)
        {
            if (null == publicKey || publicKey.Length == 0)
            {
                return null;
            }
            if (data == null || data.Length == 0)
            {
                return null;
            }

            byte[] source = new byte[data.Length];
            Array.Copy(data, 0, source, 0, data.Length);

            SM2Cipher cipher = new SM2Cipher();
            var sm2 = SM2CryptoServiceProvider.Instance;

            ECPoint userKey = sm2.ecc_curve.DecodePoint(publicKey);
            ECPoint c1 = cipher.Init_enc(sm2, userKey);
            cipher.Encrypt(source);
            byte[] c3 = new byte[32];
            cipher.Dofinal(c3);

            //String sc1 = Encoding.UTF8.GetString(Hex.Encode(c1.GetEncoded()));
            //String sc2 = Encoding.UTF8.GetString(Hex.Encode(source));
            //String sc3 = Encoding.UTF8.GetString(Hex.Encode(c3));

            String sc1 = c1.GetEncoded().byteToHex();
            String sc2 = source.byteToHex();
            String sc3 = c3.byteToHex();

            return (sc1 + sc2 + sc3).ToUpper();
        }

        public static byte[] Decrypt(string privateKey, string encryptedData)
        {
            return Decrypt(privateKey.hexToByte(), encryptedData.hexToByte());
        }

        public static byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
        {
            if (null == privateKey || privateKey.Length == 0)
            {
                return null;
            }
            if (encryptedData == null || encryptedData.Length == 0)
            {
                return null;
            }
            //加密字节数组转换为十六进制的字符串 长度变为encryptedData.length * 2
            var data = encryptedData.byteToHex();// Encoding.UTF8.GetString(Hex.Encode(encryptedData));

            byte[] c1Bytes = data.Substring(0, 130).hexToByte();
            int c2Len = encryptedData.Length - 97;
            byte[] c2 = data.Substring(130, 2 * c2Len).hexToByte();
            byte[] c3 = data.Substring(130 + 2 * c2Len, 64).hexToByte();

            //byte[] c1Bytes = Hex.Decode(Encoding.UTF8.GetBytes(data.Substring(0, 130)));
            //int c2Len = encryptedData.Length - 97;
            //byte[] c2 = Hex.Decode(Encoding.UTF8.GetBytes(data.Substring(130, 2 * c2Len)));
            //byte[] c3 = Hex.Decode(Encoding.UTF8.GetBytes(data.Substring(130 + 2 * c2Len, 64)));

            SM2CryptoServiceProvider sm2 = SM2CryptoServiceProvider.Instance;
            BigInteger userD = new BigInteger(1, privateKey);

            ECPoint c1 = sm2.ecc_curve.DecodePoint(c1Bytes);
            SM2Cipher cipher = new SM2Cipher();
            cipher.Init_dec(userD, c1);
            cipher.Decrypt(c2);
            cipher.Dofinal(c3);

            return (c2);
        }
        //========================================================================================================================
        /// <summary>
        /// 使用指定密钥对明文进行签名，返回明文签名的字符串
        /// </summary>
        /// <param name="source">要签名的明文字符串</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="isSignForSoft">true=软签,false=硬签</param>
        /// <returns></returns>
        public static string Sm2Sign(string source, string privateKey, bool isSignForSoft = true)
        {
            source = BytesAndStringExtensions.byteToHex(Encoding.UTF8.GetBytes(source));
            var sign = SMCrypto.SM2SignVerUtils.genSM2Signature(privateKey, source);
            if (isSignForSoft)
                return sign.getSm2_signForSoft();
            else
                return sign.getSm2_signForHard();
        }
        /// <summary>
        /// 使用指定公钥验证解密得到的明文是否符合签名
        /// </summary>
        /// <param name="source">解密得到的明文</param>
        /// <param name="signData">明文签名字符串</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="isSignForSoft">true=软签,false=硬签</param>
        /// <returns></returns>
        public static bool Verify(string source, string signData, string publicKey, bool isSignForSoft = true)
        {
            source = BytesAndStringExtensions.byteToHex(Encoding.UTF8.GetBytes(source));
            if (isSignForSoft)
                return SMCrypto.SM2SignVerUtils.verifySM2Signature(publicKey, source, signData);
            else
                return SMCrypto.SM2SignVerUtils.verifySM2SignatureHard(publicKey, source, signData);
        }
    }

}
