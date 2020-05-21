using System;
using System.IO;
using HCenter.Encryption.Core.Internals.Extensions;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;

namespace HCenter.Encryption.SMCrypto
{

    /**
 * 国密算法的签名、验签
 */
    internal class SM2SignVerUtils
    {
        /// <summary>
        /// 默认USERID
        /// </summary>
        public static String USER_ID = "1234567812345678";
        /**
         * 私钥签名
         * 使用SM3进行对明文数据计算一个摘要值
         * @param privatekey 私钥
         * @param sourceData 明文数据
         * @return 签名后的值
         * @throws Exception
         */
        public static SM2SignVO Sign2SM2(byte[] privatekey, byte[] sourceData)
        {
            SM2SignVO sm2SignVO = new SM2SignVO();
            sm2SignVO.setSm2_type("sign");
            var factory = SM2CryptoServiceProvider.Instance;
            BigInteger userD = new BigInteger(privatekey);
            //System.out.println("userD:"+userD.toString(16));
            sm2SignVO.setSm2_userd(userD.ToByteArray().ToHexString());

            ECPoint userKey = factory.ecc_point_g.Multiply(userD);
            //System.out.println("椭圆曲线点X: "+ userKey.getXCoord().toBigInteger().toString(16));
            //System.out.println("椭圆曲线点Y: "+ userKey.getYCoord().toBigInteger().toString(16));

            SM3Digest sm3Digest = new SM3Digest();
            byte[] z = factory.Sm2GetZ(USER_ID.GetBytes(), userKey);
            //System.out.println("SM3摘要Z: " + Util.getHexString(z));
            //System.out.println("被加密数据的16进制: " + Util.getHexString(sourceData));
            sm2SignVO.setSm3_z(z.ToHexString());
            sm2SignVO.setSign_express(sourceData.ToHexString());

            sm3Digest.update(z, 0, z.Length);
            sm3Digest.update(sourceData, 0, sourceData.Length);
            byte[] md = new byte[32];
            sm3Digest.doFinal(md, 0);
            //System.out.println("SM3摘要值: " + Util.getHexString(md));
            sm2SignVO.setSm3_digest(md.ToHexString());

            SM2Result sm2Result = new SM2Result();
            factory.sm2Sign(md, userD, userKey, sm2Result);
            //System.out.println("r: " + sm2Result.r.toString(16));
            //System.out.println("s: " + sm2Result.s.toString(16));
            sm2SignVO.setSign_r(sm2Result.r.ToByteArray().ToHexString());
            sm2SignVO.setSign_s(sm2Result.s.ToByteArray().ToHexString());

            var d_r = new DerInteger(sm2Result.r);
            var d_s = new DerInteger(sm2Result.s);
            var v2 = new Asn1EncodableVector();
            v2.Add(d_r);
            v2.Add(d_s);
            var sign = new DerSequence(v2);
            String result = sign.GetEncoded().ByteArrayToHex();
            sm2SignVO.setSm2_sign(result);
            return sm2SignVO;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="publicKey">公钥信息</param>
        /// <param name="sourceData">密文信息</param>
        /// <param name="signData">签名信息</param>
        /// <returns>验签的对象 包含了相关参数和验签结果</returns>
        public static SM2SignVO VerifySignSM2(byte[] publicKey, byte[] sourceData, byte[] signData)
        {
            try
            {
                byte[] formatedPubKey;
                SM2SignVO verifyVo = new SM2SignVO();
                verifyVo.setSm2_type("verify");
                if (publicKey.Length == 64)
                {
                    // 添加一字节标识，用于ECPoint解析
                    formatedPubKey = new byte[65];
                    formatedPubKey[0] = 0x04;
                    System.Array.Copy(publicKey, 0, formatedPubKey, 1, publicKey.Length);
                }
                else
                {
                    formatedPubKey = publicKey;
                }
                var factory = SM2CryptoServiceProvider.Instance;
                ECPoint userKey = factory.ecc_curve.DecodePoint(formatedPubKey);

                SM3Digest sm3Digest = new SM3Digest();
                byte[] z = factory.Sm2GetZ(USER_ID.GetBytes(), userKey);
                //System.out.println("SM3摘要Z: " + Util.getHexString(z));
                verifyVo.setSm3_z(z.ToHexString());
                sm3Digest.update(z, 0, z.Length);
                sm3Digest.update(sourceData, 0, sourceData.Length);
                byte[] md = new byte[32];
                sm3Digest.doFinal(md, 0);

                //System.out.println("SM3摘要值: " + Util.getHexString(md));
                verifyVo.setSm3_digest(md.ToHexString());
                var bis = new MemoryStream(signData);
                var dis = new Asn1InputStream(bis);
                SM2Result sm2Result = null;
                var derObj = dis.ReadObject();
                var e = ((Asn1Sequence)derObj).GetEnumerator();
                e.MoveNext();
                BigInteger r = ((DerInteger)e.Current).Value;
                e.MoveNext();
                BigInteger s = ((DerInteger)e.Current).Value;

                sm2Result = new SM2Result();
                sm2Result.r = r;
                sm2Result.s = s;
                //System.out.println("vr: " + sm2Result.r.toString(16));
                //System.out.println("vs: " + sm2Result.s.toString(16));
                verifyVo.setVerify_r(sm2Result.r.ToByteArray().ToHexString());
                verifyVo.setVerify_s(sm2Result.s.ToByteArray().ToHexString());
                factory.sm2Verify(md, userKey, sm2Result.r, sm2Result.s, sm2Result);
                var verifyFlag = sm2Result.r.Equals(sm2Result.R);
                verifyVo.setVerify(verifyFlag);
                return verifyVo;
            }
            catch (ArgumentException e)
            {
                //throw e;
                return null;
            }
            catch (Exception e)
            {
                //throw e;
                //e.printStackTrace();
                return null;
            }
        }

        /// <summary>
        /// 私钥签名,参数二:原串必须是hex!!!!因为是直接用于计算签名的,可能是SM3串,也可能是普通串转Hex
        /// </summary>
        /// <param name="priKey"></param>
        /// <param name="sourceData"></param>
        /// <returns></returns>
        public static SM2SignVO genSM2Signature(String priKey, String sourceData)
        {
            SM2SignVO sign = SM2SignVerUtils.Sign2SM2(priKey.hexToByte(), sourceData.hexToByte());
            return sign;
        }

        /// <summary>
        /// 公钥验签,参数二:原串必须是hex!!!!因为是直接用于计算签名的,可能是SM3串,也可能是普通串转Hex
        /// </summary>
        /// <param name="pubKey"></param>
        /// <param name="sourceData"></param>
        /// <param name="hardSign"></param>
        /// <returns></returns>
        public static bool verifySM2Signature(String pubKey, String sourceData, String softsign)
        {
            var verify = SM2SignVerUtils.VerifySignSM2(pubKey.HexToByteArray(), sourceData.hexToByte(), softsign.hexToByte());
            return verify?.isVerify == true;
        }

        /// <summary>
        /// 公钥验签,硬件加密方式验签 参数二:原串必须是hex!!!!因为是直接用于计算签名的,可能是SM3串,也可能是普通串转Hex
        /// </summary>
        /// <param name="pubKey"></param>
        /// <param name="sourceData"></param>
        /// <param name="hardSign"></param>
        /// <returns></returns>
        public static bool verifySM2SignatureHard(String pubKey, String sourceData, String hardSign)
        {
            var softsign = SM2SignHardToSoft(hardSign);
            return verifySM2Signature(pubKey, sourceData, softsign);
        }

        /// <summary>
        /// SM2签名Hard转soft
        /// </summary>
        /// <param name="hardSign"></param>
        /// <returns></returns>
        public static String SM2SignHardToSoft(String hardSign)
        {
            byte[] bytes = hardSign.hexToByte();
            byte[] r = new byte[bytes.Length / 2];
            byte[] s = new byte[bytes.Length / 2];
            System.Array.Copy(bytes, 0, r, 0, bytes.Length / 2);
            System.Array.Copy(bytes, bytes.Length / 2, s, 0, bytes.Length / 2);
            var d_r = new DerInteger(SM2CryptoServiceProvider.byteConvertInteger(r));
            var d_s = new DerInteger(SM2CryptoServiceProvider.byteConvertInteger(s));
            var v2 = new Asn1EncodableVector();
            v2.Add(d_r);
            v2.Add(d_s);
            var sign = new DerSequence(v2);

            String result = null;
            try
            {
                result = sign.GetEncoded().ByteArrayToHex();
            }
            catch (IOException e)
            {
                //e.printStackTrace();
                throw (e);
            }
            //SM2加密机转软加密编码格式
            //return SM2SignHardKeyHead+hardSign.substring(0, hardSign.Length()/2)+SM2SignHardKeyMid+hardSign.substring(hardSign.Length()/2);
            return result;
        }

        ///// <summary>
        ///// 根据字节数组获得值(十六进制数字)
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <returns></returns>
        //public static String getHexString(byte[] bytes)
        //{
        //    return getHexString(bytes, true);
        //}

        ///// <summary>
        ///// 根据字节数组获得值(十六进制数字)
        ///// </summary>
        ///// <param name="bytes"></param>
        ///// <param name="upperCase"></param>
        ///// <returns></returns>
        //public static String getHexString(byte[] bytes, bool upperCase)
        //{
        //    String ret = "";
        //    for (int i = 0; i < bytes.Length; i++)
        //    {
        //        ret += Integer.toString((bytes[i] & 0xff) + 0x100, 16).substring(1);
        //    }
        //    return upperCase ? ret.ToUpper() : ret;
        //}

    }
    /// <summary>
    /// SM2Result
    /// </summary>
    internal class SM2Result
    {
        public SM2Result()
        {
        }
        // 签名r
        public BigInteger r;
        public BigInteger s;
        //验签R
        public BigInteger R;

        // 密钥交换
        public byte[] sa;
        public byte[] sb;
        public byte[] s1;
        public byte[] s2;

        public ECPoint keyra;
        public ECPoint keyrb;

    }

    /// <summary>
    /// SM2密钥对Bean
    /// </summary>
    internal class SM2KeyPair
    {

        private ECPoint publicKey;
        private BigInteger privateKey;

        public SM2KeyPair(ECPoint publicKey, BigInteger privateKey)
        {
            this.publicKey = publicKey;
            this.privateKey = privateKey;
        }

        public ECPoint getPublicKey()
        {
            return publicKey;
        }

        public BigInteger getPrivateKey()
        {
            return privateKey;
        }

    }
    /// <summary>
    /// SM2签名所计算的值 可以根据实际情况增加删除字段属性
    /// </summary>
    internal class SM2SignVO
    {
        //16进制的私钥
        public String sm2_userd;
        //椭圆曲线点X
        public String x_coord;
        //椭圆曲线点Y
        public String y_coord;
        //SM3摘要Z
        public String sm3_z;
        //明文数据16进制
        public String sign_express;
        //SM3摘要值
        public String sm3_digest;
        //R
        public String sign_r;
        //S
        public String sign_s;
        //R
        public String verify_r;
        //S
        public String verify_s;
        //签名值
        public String sm2_sign;
        //sign 签名  verfiy验签
        public String sm2_type;
        //是否验签成功  true false
        public bool isVerify;
        public String getX_coord()
        {
            return x_coord;
        }
        public void setX_coord(String x_coord)
        {
            this.x_coord = x_coord;
        }
        public String getY_coord()
        {
            return y_coord;
        }
        public void setY_coord(String y_coord)
        {
            this.y_coord = y_coord;
        }
        public String getSm3_z()
        {
            return sm3_z;
        }
        public void setSm3_z(String sm3_z)
        {
            this.sm3_z = sm3_z;
        }
        public String getSm3_digest()
        {
            return sm3_digest;
        }
        public void setSm3_digest(String sm3_digest)
        {
            this.sm3_digest = sm3_digest;
        }
        public String getSm2_signForSoft()
        {
            return sm2_sign;
        }
        public String getSm2_signForHard()
        {
            //System.out.println("R:"+getSign_r());
            //System.out.println("s:"+getSign_s());
            return getSign_r() + getSign_s();
        }
        public void setSm2_sign(String sm2_sign)
        {
            this.sm2_sign = sm2_sign;
        }
        public String getSign_express()
        {
            return sign_express;
        }
        public void setSign_express(String sign_express)
        {
            this.sign_express = sign_express;
        }
        public String getSm2_userd()
        {
            return sm2_userd;
        }
        public void setSm2_userd(String sm2_userd)
        {
            this.sm2_userd = sm2_userd;
        }
        public String getSm2_type()
        {
            return sm2_type;
        }
        public void setSm2_type(String sm2_type)
        {
            this.sm2_type = sm2_type;
        }
        public bool getVerify()
        {
            return isVerify;
        }
        public void setVerify(bool isVerify)
        {
            this.isVerify = isVerify;
        }
        public String getSign_r()
        {
            return sign_r;
        }
        public void setSign_r(String sign_r)
        {
            this.sign_r = sign_r;
        }
        public String getSign_s()
        {
            return sign_s;
        }
        public void setSign_s(String sign_s)
        {
            this.sign_s = sign_s;
        }
        public String getVerify_r()
        {
            return verify_r;
        }
        public void setVerify_r(String verify_r)
        {
            this.verify_r = verify_r;
        }
        public String getVerify_s()
        {
            return verify_s;
        }
        public void setVerify_s(String verify_s)
        {
            this.verify_s = verify_s;
        }
    }

}
