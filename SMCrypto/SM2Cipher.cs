using HCenter.Encryption.Core.Internals.Extensions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace HCenter.Encryption.SMCrypto
{
    internal class SM2Cipher
    {
        private int ct;

        private ECPoint p2;
        private SM3Digest sm3keybase;
        private SM3Digest sm3c3;

        private byte[] key;
        private byte keyOff;

        public SM2Cipher()
        {
            this.ct = 1;
            this.key = new byte[32];
            this.keyOff = 0;
        }


        private void Reset()
        {
            sm3keybase = new SM3Digest();
            sm3c3 = new SM3Digest();

            byte[] p = p2.Normalize().XCoord.ToBigInteger().ToByteArray();
            sm3keybase.update(p, 0, p.Length);
            sm3c3.update(p, 0, p.Length);

            p = p2.Normalize().YCoord.ToBigInteger().ToByteArray();
            sm3keybase.update(p, 0, p.Length);

            ct = 1;
            NextKey();
        }


        private void NextKey()
        {
            SM3Digest sm3keycur = new SM3Digest(sm3keybase);
            sm3keycur.update((byte)(ct >> 24 & 0xff));
            sm3keycur.update((byte)(ct >> 16 & 0xff));
            sm3keycur.update((byte)(ct >> 8 & 0xff));
            sm3keycur.update((byte)(ct & 0xff));
            sm3keycur.doFinal(key, 0);
            keyOff = 0;
            ct++;
        }


        public virtual ECPoint Init_enc(SM2CryptoServiceProvider sm2, ECPoint userKey)
        {
            //BigInteger k = null;
            //ECPoint c1 = null;

            //AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
            //ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
            //ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
            //k = ecpriv.D;
            //c1 = ecpub.Q;

            //p2 = userKey.Multiply(k);
            //Reset();

            //return c1;

            var keySTR = userKey.GetEncoded().byteToHex();
            if (keySTR.Length > 64)
                keySTR = keySTR.Substring(0, 64);
            BigInteger k = new BigInteger(keySTR, 16);//ecpriv.getD();
            ECPoint c1 = sm2.ecc_point_g.Multiply(k);//ecpub.getQ();
            this.p2 = userKey.Multiply(k);
            Reset();
            return c1;
        }


        public virtual void Encrypt(byte[] data)
        {
            sm3c3.update(data, 0, data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                if (keyOff == key.Length)
                    NextKey();

                data[i] ^= key[keyOff++];
            }
        }


        public virtual void Init_dec(BigInteger userD, ECPoint c1)
        {
            p2 = c1.Multiply(userD);
            Reset();
        }


        public virtual void Decrypt(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (keyOff == key.Length)
                    NextKey();

                data[i] ^= key[keyOff++];
            }
            sm3c3.update(data, 0, data.Length);
        }


        public virtual void Dofinal(byte[] c3)
        {
            byte[] p = p2.Normalize().YCoord.ToBigInteger().ToByteArray();
            sm3c3.update(p, 0, p.Length);
            sm3c3.doFinal(c3, 0);
            Reset();
        }

    }
}
