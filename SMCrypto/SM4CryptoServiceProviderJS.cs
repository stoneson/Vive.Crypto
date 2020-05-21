using System;
using System.Collections.Generic;

namespace HCenter.Encryption.SMCrypto
{
    internal class SM4CryptoServiceProviderJS: SM4CryptoServiceProvider
    {
        private int GET_ULONG_BE(byte[] b, int i)
        {
            int n = (int)((b[i] & 0xff) << 24 | (int)((b[i + 1] & 0xff) << 16) | (int)((b[i + 2] & 0xff) << 8) | (int)(b[i + 3] & 0xff) & 0xffffffffL);
            return n;
        }

        private void PUT_ULONG_BE(int n, byte[] b, int i)
        {
            b[i] = (byte)(int)(0xFF & n >> 24);
            b[i + 1] = (byte)(int)(0xFF & n >> 16);
            b[i + 2] = (byte)(int)(0xFF & n >> 8);
            b[i + 3] = (byte)(int)(0xFF & n);
        }

        private int SHL(int x, int n)
        {
            return (int)(x & 0xFFFFFFFF) << n;
        }

        private int ROTL(int x, int n)
        {
            return SHL(x, n) | x >> (32 - n);
        }

        private void SWAP(int[] sk, int i)
        {
            int t = sk[i];
            sk[i] = sk[(31 - i)];
            sk[(31 - i)] = t;
        }

        private byte sm4Sbox(byte inch)
        {
            int i = inch & 0xFF;
            byte retVal = SboxTable[i];
            return retVal;
        }

        private int sm4Lt(int ka)
        {
            int bb = 0;
            int c = 0;
            byte[] a = new byte[4];
            byte[] b = new byte[4];
            PUT_ULONG_BE(ka, a, 0);
            b[0] = sm4Sbox(a[0]);
            b[1] = sm4Sbox(a[1]);
            b[2] = sm4Sbox(a[2]);
            b[3] = sm4Sbox(a[3]);
            bb = GET_ULONG_BE(b, 0);
            c = bb ^ ROTL(bb, 2) ^ ROTL(bb, 10) ^ ROTL(bb, 18) ^ ROTL(bb, 24);
            return c;
        }

        private int sm4F(int x0, int x1, int x2, int x3, int rk)
        {
            return x0 ^ sm4Lt(x1 ^ x2 ^ x3 ^ rk);
        }

        private int sm4CalciRK(int ka)
        {
            int bb = 0;
            int rk = 0;
            byte[] a = new byte[4];
            byte[] b = new byte[4];
            PUT_ULONG_BE(ka, a, 0);
            b[0] = sm4Sbox(a[0]);
            b[1] = sm4Sbox(a[1]);
            b[2] = sm4Sbox(a[2]);
            b[3] = sm4Sbox(a[3]);
            bb = GET_ULONG_BE(b, 0);
            rk = bb ^ ROTL(bb, 13) ^ ROTL(bb, 23);
            return rk;
        }

        private void sm4_setkey(int[] SK, byte[] key)
        {
            int[] MK = new int[4];
            int[] k = new int[36];
            int i = 0;
            MK[0] = GET_ULONG_BE(key, 0);
            MK[1] = GET_ULONG_BE(key, 4);
            MK[2] = GET_ULONG_BE(key, 8);
            MK[3] = GET_ULONG_BE(key, 12);
            k[0] = MK[0] ^ (int)FK[0];
            k[1] = MK[1] ^ (int)FK[1];
            k[2] = MK[2] ^ (int)FK[2];
            k[3] = MK[3] ^ (int)FK[3];
            for (; i < 32; i++)
            {
                k[(i + 4)] = (k[i] ^ sm4CalciRK(k[(i + 1)] ^ k[(i + 2)] ^ k[(i + 3)] ^ (int)CK[i]));
                SK[i] = k[(i + 4)];
            }
        }

        private void sm4_one_round(int[] sk, byte[] input, byte[] output)
        {
            int i = 0;
            int[] ulbuf = new int[36];
            ulbuf[0] = GET_ULONG_BE(input, 0);
            ulbuf[1] = GET_ULONG_BE(input, 4);
            ulbuf[2] = GET_ULONG_BE(input, 8);
            ulbuf[3] = GET_ULONG_BE(input, 12);
            while (i < 32)
            {
                ulbuf[(i + 4)] = sm4F(ulbuf[i], ulbuf[(i + 1)], ulbuf[(i + 2)], ulbuf[(i + 3)], sk[i]);
                i++;
            }
            PUT_ULONG_BE(ulbuf[35], output, 0);
            PUT_ULONG_BE(ulbuf[34], output, 4);
            PUT_ULONG_BE(ulbuf[33], output, 8);
            PUT_ULONG_BE(ulbuf[32], output, 12);
        }

        private byte[] padding(byte[] input, int mode)
        {
            if (input == null)
            {
                return null;
            }

            byte[] ret = (byte[])null;
            if (mode == SM4_ENCRYPT)
            {
                int p = 16 - input.Length % 16;
                ret = new byte[input.Length + p];
                Array.Copy(input, 0, ret, 0, input.Length);
                for (int i = 0; i < p; i++)
                {
                    ret[input.Length + i] = (byte)p;
                }
            }
            else
            {
                int p = input[input.Length - 1];
                ret = new byte[input.Length - p];
                Array.Copy(input, 0, ret, 0, input.Length - p);
            }
            return ret;
        }

        public void sm4_setkey_enc(SM4ContextJS ctx, byte[] key)
        {
            ctx.mode = SM4_ENCRYPT;
            sm4_setkey(ctx.sk, key);
        }

        public void sm4_setkey_dec(SM4ContextJS ctx, byte[] key)
        {
            int i = 0;
            ctx.mode = SM4_DECRYPT;
            sm4_setkey(ctx.sk, key);
            for (i = 0; i < 16; i++)
            {
                SWAP(ctx.sk, i);
            }
        }

        public byte[] sm4_crypt_ecb(SM4ContextJS ctx, byte[] input)
        {
            if ((ctx.isPadding) && (ctx.mode == SM4_ENCRYPT))
            {
                input = padding(input, SM4_ENCRYPT);
            }

            int length = input.Length;
            byte[] bins = new byte[length];
            Array.Copy(input, 0, bins, 0, length);
            byte[] bous = new byte[length];
            for (int i = 0; length > 0; length -= 16, i++)
            {
                byte[] inBytes = new byte[16];
                byte[] outBytes = new byte[16];
                Array.Copy(bins, i * 16, inBytes, 0, length > 16 ? 16 : length);
                sm4_one_round(ctx.sk, inBytes, outBytes);
                Array.Copy(outBytes, 0, bous, i * 16, length > 16 ? 16 : length);
            }

            if (ctx.isPadding && ctx.mode == SM4_DECRYPT)
            {
                bous = padding(bous, SM4_DECRYPT);
            }
            return bous;
        }

        public byte[] sm4_crypt_cbc(SM4ContextJS ctx, byte[] iv, byte[] input)
        {
            if (ctx.isPadding && ctx.mode == SM4_ENCRYPT)
            {
                input = padding(input, SM4_ENCRYPT);
            }

            int i = 0;
            int length = input.Length;
            byte[] bins = new byte[length];
            Array.Copy(input, 0, bins, 0, length);
            byte[] bous = null;
            List<byte> bousList = new List<byte>();
            if (ctx.mode == SM4_ENCRYPT)
            {
                for (int j = 0; length > 0; length -= 16, j++)
                {
                    byte[] inBytes = new byte[16];
                    byte[] outBytes = new byte[16];
                    byte[] out1 = new byte[16];

                    Array.Copy(bins, j * 16, inBytes, 0, length > 16 ? 16 : length);
                    for (i = 0; i < 16; i++)
                    {
                        outBytes[i] = ((byte)(inBytes[i] ^ iv[i]));
                    }
                    sm4_one_round(ctx.sk, outBytes, out1);
                    Array.Copy(out1, 0, iv, 0, 16);
                    for (int k = 0; k < 16; k++)
                    {
                        bousList.Add(out1[k]);
                    }
                }
            }
            else
            {
                byte[] temp = new byte[16];
                for (int j = 0; length > 0; length -= 16, j++)
                {
                    byte[] inBytes = new byte[16];
                    byte[] outBytes = new byte[16];
                    byte[] out1 = new byte[16];

                    Array.Copy(bins, j * 16, inBytes, 0, length > 16 ? 16 : length);
                    Array.Copy(inBytes, 0, temp, 0, 16);
                    sm4_one_round(ctx.sk, inBytes, outBytes);
                    for (i = 0; i < 16; i++)
                    {
                        out1[i] = ((byte)(outBytes[i] ^ iv[i]));
                    }
                    Array.Copy(temp, 0, iv, 0, 16);
                    for (int k = 0; k < 16; k++)
                    {
                        bousList.Add(out1[k]);
                    }
                }

            }

            if (ctx.isPadding && ctx.mode == SM4_DECRYPT)
            {
                bous = padding(bousList.ToArray(), SM4_DECRYPT);
                return bous;
            }
            else
            {
                return bousList.ToArray();
            }
        }
    }
}
