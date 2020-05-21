using Vive.Crypto.Core.Internals.Extensions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace Vive.Crypto.SMCrypto
{
    internal class SM2CryptoServiceProvider
    {
        public static SM2CryptoServiceProvider Instance
        {
            get
            {
                return new SM2CryptoServiceProvider();
            }

        }

        public static readonly string[] sm2_param = {
            "fffffffeffffffffffffffffffffffffffffffff00000000ffffffffffffffff",// p,0  
            "fffffffeffffffffffffffffffffffffffffffff00000000fffffffffffffffc",// a,1  
            "28e9fa9e9d9f5e344d5a9e4bcf6509a7f39789f515ab8f92ddbcbd414d940e93",// b,2  
            "fffffffeffffffffffffffffffffffff7203df6b21c6052b53bbf40939d54123",// n,3  
            "32c4ae2c1f1981195f9904466a39c9948fe30bbff2660be1715a4589334c74c7",// gx,4  
            "bc3736a2f4f6779c59bdcee36b692153d0a9877cc62a474002df32e52139f0a0" // gy,5  
        };

        public string[] ecc_param = sm2_param;
        //public readonly string userId = "1234567812345678";
        public readonly BigInteger ecc_p;
        public readonly BigInteger ecc_a;
        public readonly BigInteger ecc_b;
        public readonly BigInteger ecc_n;
        public readonly BigInteger ecc_gx;
        public readonly BigInteger ecc_gy;

        public readonly ECCurve ecc_curve;
        public readonly ECPoint ecc_point_g;

        public readonly ECDomainParameters ecc_bc_spec;

        public readonly ECKeyPairGenerator ecc_key_pair_generator;

        public SM2CryptoServiceProvider()
        {
            ecc_param = sm2_param;

            ecc_p = new BigInteger(sm2_param[0], 16);
            ecc_a = new BigInteger(sm2_param[1], 16);
            ecc_b = new BigInteger(sm2_param[2], 16);
            ecc_n = new BigInteger(sm2_param[3], 16);
            ecc_gx = new BigInteger(sm2_param[4], 16);
            ecc_gy = new BigInteger(sm2_param[5], 16);

           //ecc_curve = new FpCurve(ecc_p, ecc_a, ecc_b, ecc_gx, ecc_gy);

            var ecc_gx_fieldelement = new FpFieldElement(ecc_p, ecc_gx);//ecc_curve.FromBigInteger(ecc_gx);// 
            var ecc_gy_fieldelement = new FpFieldElement(ecc_p, ecc_gy);// ecc_curve.FromBigInteger(ecc_gy);// 

            ecc_curve = new FpCurve(ecc_p, ecc_a, ecc_b);// ecc_curve.CreatePoint(ecc_gx, ecc_gy);//
            ecc_point_g = new FpPoint(ecc_curve, ecc_gx_fieldelement, ecc_gy_fieldelement);

            ecc_bc_spec = new ECDomainParameters(ecc_curve, ecc_point_g, ecc_n);

            var ecc_ecgenparam = new ECKeyGenerationParameters(ecc_bc_spec, new SecureRandom());

            ecc_key_pair_generator = new ECKeyPairGenerator();
            ecc_key_pair_generator.Init(ecc_ecgenparam);
        }

        public virtual byte[] Sm2GetZ(byte[] userId, ECPoint userKey)
        {
            //SM3Digest sm3 = new SM3Digest();
            //byte[] p;
            //// userId Length  
            //int len = userId.Length * 8;
            //sm3.Update((byte)(len >> 8 & 0x00ff));
            //sm3.Update((byte)(len & 0x00ff));

            //// userId  
            //sm3.BlockUpdate(userId, 0, userId.Length);

            //// a,b  
            //p = ecc_a.ToByteArray();
            //sm3.BlockUpdate(p, 0, p.Length);
            //p = ecc_b.ToByteArray();
            //sm3.BlockUpdate(p, 0, p.Length);
            //// gx,gy  
            //p = ecc_gx.ToByteArray();
            //sm3.BlockUpdate(p, 0, p.Length);
            //p = ecc_gy.ToByteArray();
            //sm3.BlockUpdate(p, 0, p.Length);

            //// x,y  
            //p = userKey.XCoord.ToBigInteger().ToByteArray();
            //sm3.BlockUpdate(p, 0, p.Length);
            //p = userKey.YCoord.ToBigInteger().ToByteArray();
            //sm3.BlockUpdate(p, 0, p.Length);

            //// Z  
            //byte[] md = new byte[sm3.GetDigestSize()];
            //sm3.DoFinal(md, 0);

          //  return md;

            SM3Digest sm3 = new SM3Digest();

            int len = userId.Length * 8;
            sm3.update((byte)(len >> 8 & 0xFF));
            sm3.update((byte)(len & 0xFF));
            sm3.update(userId, 0, userId.Length);

            byte[] p = byteConvert32Bytes(this.ecc_a);
            sm3.update(p, 0, p.Length);

            p = byteConvert32Bytes(this.ecc_b);
            sm3.update(p, 0, p.Length);

            p = byteConvert32Bytes(this.ecc_gx);
            sm3.update(p, 0, p.Length);

            p = byteConvert32Bytes(this.ecc_gy);
            sm3.update(p, 0, p.Length);

            p = byteConvert32Bytes(userKey.Normalize().XCoord.ToBigInteger());
            sm3.update(p, 0, p.Length);

            p = byteConvert32Bytes(userKey.Normalize().YCoord.ToBigInteger());
            sm3.update(p, 0, p.Length);

            byte[] md = new byte[sm3.getDigestSize()];
            sm3.doFinal(md, 0);
            return md;
        }

        /// <summary>
        /// 大数字转换字节流（字节数组）型数据
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static byte[] byteConvert32Bytes(BigInteger n)
        {
            byte[] tmpd = new byte[0];
            if (n == null)
            {
                return null;
            }

            if (n.ToByteArray().Length == 33)
            {
                tmpd = new byte[32];
                System.Array.Copy(n.ToByteArray(), 1, tmpd, 0, 32);
            }
            else if (n.ToByteArray().Length == 32)
            {
                tmpd = n.ToByteArray();
            }
            else
            {
                tmpd = new byte[32];
                for (int i = 0; i < 32 - n.ToByteArray().Length; i++)
                {
                    tmpd[i] = 0;
                }
                System.Array.Copy(n.ToByteArray(), 0, tmpd, 32 - n.ToByteArray().Length, n.ToByteArray().Length);
            }
            return tmpd;
        }
        /// <summary>
        /// 换字节流（字节数组）型数据转大数字
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger byteConvertInteger(byte[] b)
        {
            if (b[0] < 0)
            {
                byte[] temp = new byte[b.Length + 1];
                temp[0] = 0;
                System.Array.Copy(b, 0, temp, 1, b.Length);
                return new BigInteger(temp);
            }
            return new BigInteger(b);
        }
        //=========================================================================================================
        /// <summary>
        /// 签名相关值计算
        /// </summary>
        /// <param name="md"></param>
        /// <param name="userD"></param>
        /// <param name="userKey"></param>
        /// <param name="sm2Result"></param>
        public void sm2Sign(byte[] md, BigInteger userD, ECPoint userKey, SM2Result sm2Result)
        {
            BigInteger e = new BigInteger(1, md);
            BigInteger k = null;
            ECPoint kp = null;
            BigInteger r = null;
            BigInteger s = null;
            do
            {
                do
                {
                    // 正式环境
                    AsymmetricCipherKeyPair keypair = ecc_key_pair_generator.GenerateKeyPair();
                    ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)keypair.Private;
                    ECPublicKeyParameters ecpub = (ECPublicKeyParameters)keypair.Public;
                    k = ecpriv.D;
                    kp = ecpub.Q;
                    //System.out.println("BigInteger:" + k + "\nECPoint:" + kp);

                    //System.out.println("计算曲线点X1: "+ kp.getXCoord().toBigInteger().toString(16));
                    //System.out.println("计算曲线点Y1: "+ kp.getYCoord().toBigInteger().toString(16));
                    //System.out.println("");
                    // r
                    r = e.Add(kp.XCoord.ToBigInteger());
                    r = r.Mod(this.ecc_n);
                } while (r.Equals(BigInteger.Zero) || r.Add(k).Equals(this.ecc_n) || r.ToByteArray().ToHexString().Length != 64);

                // (1 + dA)~-1
                BigInteger da_1 = userD.Add(BigInteger.One);
                da_1 = da_1.ModInverse(this.ecc_n);
                // s
                s = r.Multiply(userD);
                s = k.Subtract(s).Mod(this.ecc_n);
                s = da_1.Multiply(s).Mod(this.ecc_n);
            } while (s.Equals(BigInteger.Zero) || (s.ToByteArray().ToHexString().Length != 64));

            sm2Result.r = r;
            sm2Result.s = s;
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="md">sm3摘要</param>
        /// <param name="userKey">根据公钥decode一个ecpoint对象</param>
        /// <param name="r">没有特殊含义</param>
        /// <param name="s">没有特殊含义</param>
        /// <param name="sm2Result">sm2Result 接收参数的对象</param>
        public void sm2Verify(byte[] md, ECPoint userKey, BigInteger r, BigInteger s, SM2Result sm2Result)
        {
            sm2Result.R = null;
            BigInteger e = new BigInteger(1, md);
            BigInteger t = r.Add(s).Mod(this.ecc_n);
            if (t.Equals(BigInteger.Zero))
            {
                return;
            }
            else
            {
                ECPoint x1y1 = ecc_point_g.Multiply(sm2Result.s);
                //System.out.println("计算曲线点X0: "+ x1y1.normalize().getXCoord().toBigInteger().toString(16));
                //System.out.println("计算曲线点Y0: "+ x1y1.normalize().getYCoord().toBigInteger().toString(16));
                //System.out.println("");

                x1y1 = x1y1.Add(userKey.Multiply(t));
                //System.out.println("计算曲线点X1: "+ x1y1.normalize().getXCoord().toBigInteger().toString(16));
                //System.out.println("计算曲线点Y1: "+ x1y1.normalize().getYCoord().toBigInteger().toString(16));
                //System.out.println("");
                sm2Result.R = e.Add(x1y1.Normalize().XCoord.ToBigInteger()).Mod(this.ecc_n);
                //System.out.println("R: " + sm2Result.R.toString(16));
                return;
            }
        }
    }
}
