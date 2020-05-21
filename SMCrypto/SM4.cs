using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Text;

namespace HCenter.Encryption.SMCrypto
{
    internal class SM4
    {
        public String secretKey = "";
        public String iv = "";
        public bool hexString = false;

        public String EncryptECB(String plainText)
        {
            var ctx = new SM4Context();
            ctx.isPadding = true;
            ctx.mode = SM4CryptoServiceProvider.SM4_ENCRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.UTF8.GetBytes(secretKey);
            }

            var sm4 = new SM4CryptoServiceProvider();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Encoding.UTF8.GetBytes(plainText));

            String cipherText = Encoding.UTF8.GetString(Hex.Encode(encrypted));
            return cipherText;
        }

        public String DecryptECB(String cipherText)
        {
            var ctx = new SM4Context();
            ctx.isPadding = true;
            ctx.mode = SM4CryptoServiceProvider.SM4_DECRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.UTF8.GetBytes(secretKey);
            }

            var sm4 = new SM4CryptoServiceProvider();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
            return Encoding.UTF8.GetString(decrypted);
        }
        public String EncryptCBC(String plainText)
        {
            var ctx = new SM4Context();
            ctx.isPadding = true;
            ctx.mode = SM4CryptoServiceProvider.SM4_ENCRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            }
            else
            {
                keyBytes = Encoding.UTF8.GetBytes(secretKey);
                ivBytes = Encoding.UTF8.GetBytes(iv);
            }

            var sm4 = new SM4CryptoServiceProvider();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Encoding.UTF8.GetBytes(plainText));

            String cipherText = Encoding.UTF8.GetString(Hex.Encode(encrypted));
            return cipherText;
        }

        public String DecryptCBC(String cipherText)
        {
            var ctx = new SM4Context();
            ctx.isPadding = true;
            ctx.mode = SM4CryptoServiceProvider.SM4_DECRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            }
            else
            {
                keyBytes = Encoding.UTF8.GetBytes(secretKey);
                ivBytes = Encoding.UTF8.GetBytes(iv);
            }

            var sm4 = new SM4CryptoServiceProvider();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
            return Encoding.UTF8.GetString(decrypted);
        }
        //=====================FOR JavaScript==============================================================================
        public String EncryptECB4JS(String plainText)
        {
            var ctx = new SM4ContextJS();
            ctx.isPadding = true;
            ctx.mode = SM4CryptoServiceProviderJS.SM4_ENCRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.UTF8.GetBytes(secretKey);
            }

            var sm4 = new SM4CryptoServiceProviderJS();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Encoding.UTF8.GetBytes(plainText));

            String cipherText = Encoding.UTF8.GetString(Hex.Encode(encrypted));
            return cipherText;
        }

        public String DecryptECB4JS(String cipherText)
        {
            var ctx = new SM4ContextJS();
            ctx.isPadding = true;
            ctx.mode = SM4CryptoServiceProviderJS.SM4_DECRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            }
            else
            {
                keyBytes = Encoding.UTF8.GetBytes(secretKey);
            }

            var sm4 = new SM4CryptoServiceProviderJS();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
            return Encoding.UTF8.GetString(decrypted);
        }
        public String EncryptCBC4JS(String plainText)
        {
            var ctx = new SM4ContextJS();
            ctx.isPadding = true;
            ctx.mode = SM4CryptoServiceProviderJS.SM4_ENCRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            }
            else
            {
                keyBytes = Encoding.UTF8.GetBytes(secretKey);
                ivBytes = Encoding.UTF8.GetBytes(iv);
            }

            SM4CryptoServiceProviderJS sm4 = new SM4CryptoServiceProviderJS();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Encoding.UTF8.GetBytes(plainText));

            String cipherText = Encoding.UTF8.GetString(Hex.Encode(encrypted));
            return cipherText;
        }

        public String DecryptCBC4JS(String cipherText)
        {
            var ctx = new SM4ContextJS();
            ctx.isPadding = true;
            ctx.mode = SM4CryptoServiceProviderJS.SM4_DECRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            }
            else
            {
                keyBytes = Encoding.UTF8.GetBytes(secretKey);
                ivBytes = Encoding.UTF8.GetBytes(iv);
            }

            var sm4 = new SM4CryptoServiceProviderJS();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
