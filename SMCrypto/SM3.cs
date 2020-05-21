using HCenter.Encryption.Core.Internals.Extensions;
using Org.BouncyCastle.Utilities.Encoders;
using System.Text;

namespace HCenter.Encryption.SMCrypto
{
    internal class SM3
    {
        public static string Hash(string str)
        {
            SM3Digest sm3 = new SM3Digest();
            byte[] md = new byte[sm3.getDigestSize()];
            byte[] msg1 = Encoding.UTF8.GetBytes(str);
            sm3.update(msg1, 0, msg1.Length);
            sm3.doFinal(md, 0);
            string s = Encoding.UTF8.GetString(Hex.Encode(md));
            return s.ToUpper();
        }
        //==================================================================================================================================
        public static int[] Tj = new int[64];
        static SM3()
        {
            for (int i = 0; i < 16; i++)
            {
                Tj[i] = 0x79cc4519;
            }

            for (int i = 16; i < 64; i++)
            {
                Tj[i] = 0x7a879d8a;
            }
        }

        public static byte[] CF(byte[] V, byte[] B)
        {
            int[] v, b;
            v = convert(V);
            b = convert(B);
            return convert(CF(v, b));
        }

        private static int[] convert(byte[] arr)
        {
            int[] out1 = new int[arr.Length / 4];
            byte[] tmp = new byte[4];
            for (int i = 0; i < arr.Length; i += 4)
            {
                System.Array.Copy(arr, i, tmp, 0, 4);
                out1[i / 4] = bigEndianByteToInt(tmp);
            }
            return out1;
        }

        private static byte[] convert(int[] arr)
        {
            byte[] out1 = new byte[arr.Length * 4];
            byte[] tmp = null;
            for (int i = 0; i < arr.Length; i++)
            {
                tmp = bigEndianIntToByte(arr[i]);
                System.Array.Copy(tmp, 0, out1, i * 4, 4);
            }
            return out1;
        }

        public static int[] CF(int[] V, int[] B)
        {
            int a, b, c, d, e, f, g, h;
            int ss1, ss2, tt1, tt2;
            a = V[0];
            b = V[1];
            c = V[2];
            d = V[3];
            e = V[4];
            f = V[5];
            g = V[6];
            h = V[7];

            int[][] arr = expand(B);
            int[] w = arr[0];
            int[] w1 = arr[1];

            for (int j = 0; j < 64; j++)
            {
                ss1 = (bitCycleLeft(a, 12) + e + bitCycleLeft(Tj[j], j));
                ss1 = bitCycleLeft(ss1, 7);
                ss2 = ss1 ^ bitCycleLeft(a, 12);
                tt1 = FFj(a, b, c, j) + d + ss2 + w1[j];
                tt2 = GGj(e, f, g, j) + h + ss1 + w[j];
                d = c;
                c = bitCycleLeft(b, 9);
                b = a;
                a = tt1;
                h = g;
                g = bitCycleLeft(f, 19);
                f = e;
                e = P0(tt2);

                /*System.out.print(j+" ");
                System.out.print(Integer.toHexString(a)+" ");
                System.out.print(Integer.toHexString(b)+" ");
                System.out.print(Integer.toHexString(c)+" ");
                System.out.print(Integer.toHexString(d)+" ");
                System.out.print(Integer.toHexString(e)+" ");
                System.out.print(Integer.toHexString(f)+" ");
                System.out.print(Integer.toHexString(g)+" ");
                System.out.print(Integer.toHexString(h)+" ");
                System.out.println("");*/
            }
            //System.out.println("");

            int[] out1 = new int[8];
            out1[0] = a ^ V[0];
            out1[1] = b ^ V[1];
            out1[2] = c ^ V[2];
            out1[3] = d ^ V[3];
            out1[4] = e ^ V[4];
            out1[5] = f ^ V[5];
            out1[6] = g ^ V[6];
            out1[7] = h ^ V[7];

            return out1;
        }

        private static int[][] expand(int[] B)
        {
            int[] W = new int[68];
            int[] W1 = new int[64];
            for (int i = 0; i < B.Length; i++)
            {
                W[i] = B[i];
            }

            for (int i = 16; i < 68; i++)
            {
                W[i] = P1(W[i - 16] ^ W[i - 9] ^ bitCycleLeft(W[i - 3], 15))
                        ^ bitCycleLeft(W[i - 13], 7) ^ W[i - 6];
            }

            for (int i = 0; i < 64; i++)
            {
                W1[i] = W[i] ^ W[i + 4];
            }

            int[][] arr = new int[][] { W, W1 };
            return arr;
        }

        private static byte[] bigEndianIntToByte(int num)
        {
            return back(num.intToBytes());
        }

        private static int bigEndianByteToInt(byte[] bytes)
        {
            return back(bytes).byteToInt();
        }

        private static int FFj(int X, int Y, int Z, int j)
        {
            if (j >= 0 && j <= 15)
            {
                return FF1j(X, Y, Z);
            }
            else
            {
                return FF2j(X, Y, Z);
            }
        }

        private static int GGj(int X, int Y, int Z, int j)
        {
            if (j >= 0 && j <= 15)
            {
                return GG1j(X, Y, Z);
            }
            else
            {
                return GG2j(X, Y, Z);
            }
        }

        // 逻辑位运算函数
        private static int FF1j(int X, int Y, int Z)
        {
            int tmp = X ^ Y ^ Z;
            return tmp;
        }

        private static int FF2j(int X, int Y, int Z)
        {
            int tmp = ((X & Y) | (X & Z) | (Y & Z));
            return tmp;
        }

        private static int GG1j(int X, int Y, int Z)
        {
            int tmp = X ^ Y ^ Z;
            return tmp;
        }

        private static int GG2j(int X, int Y, int Z)
        {
            int tmp = (X & Y) | (~X & Z);
            return tmp;
        }

        private static int P0(int X)
        {
            int y = rotateLeft(X, 9);
            y = bitCycleLeft(X, 9);
            int z = rotateLeft(X, 17);
            z = bitCycleLeft(X, 17);
            int t = X ^ y ^ z;
            return t;
        }

        private static int P1(int X)
        {
            int t = X ^ bitCycleLeft(X, 15) ^ bitCycleLeft(X, 23);
            return t;
        }

        /**
         * 对最后一个分组字节数据padding
         *
         * @param in
         * @param bLen
         *            分组个数
         * @return
         */
        public static byte[] padding(byte[] in1, int bLen)
        {
            int k = 448 - (8 * in1.Length + 1) % 512;
            if (k < 0)
            {
                k = 960 - (8 * in1.Length + 1) % 512;
            }
            k += 1;
            byte[] padd = new byte[k / 8];
            padd[0] = (byte)0x80;
            long n = in1.Length * 8 + bLen * 512;
            byte[] out1 = new byte[in1.Length + k / 8 + 64 / 8];
            int pos = 0;
            System.Array.Copy(in1, 0, out1, 0, in1.Length);
            pos += in1.Length;
            System.Array.Copy(padd, 0, out1, pos, padd.Length);
            pos += padd.Length;
            byte[] tmp = back(n.longToBytes());
            System.Array.Copy(tmp, 0, out1, pos, tmp.Length);
            return out1;
        }

        /**
         * 字节数组逆序
         *
         * @param in
         * @return
         */
        private static byte[] back(byte[] in1)
        {
            byte[] out1 = new byte[in1.Length];
            for (int i = 0; i < out1.Length; i++)
            {
                out1[i] = in1[out1.Length - i - 1];
            }

            return out1;
        }

        public static int rotateLeft(int x, int n)
        {
            return (x << n) | (x >> (32 - n));
        }

        private static int bitCycleLeft(int n, int bitLen)
        {
            bitLen %= 32;
            byte[] tmp = bigEndianIntToByte(n);
            int byteLen = bitLen / 8;
            int len = bitLen % 8;
            if (byteLen > 0)
            {
                tmp = byteCycleLeft(tmp, byteLen);
            }

            if (len > 0)
            {
                tmp = bitSmall8CycleLeft(tmp, len);
            }

            return bigEndianByteToInt(tmp);
        }

        private static byte[] bitSmall8CycleLeft(byte[] in1, int len)
        {
            byte[] tmp = new byte[in1.Length];
            int t1, t2, t3;
            for (int i = 0; i < tmp.Length; i++)
            {
                t1 = (byte)((in1[i] & 0x000000ff) << len);
                t2 = (byte)((in1[(i + 1) % tmp.Length] & 0x000000ff) >> (8 - len));
                t3 = (byte)(t1 | t2);
                tmp[i] = (byte)t3;
            }

            return tmp;
        }

        private static byte[] byteCycleLeft(byte[] in1, int byteLen)
        {
            byte[] tmp = new byte[in1.Length];
            System.Array.Copy(in1, byteLen, tmp, 0, in1.Length - byteLen);
            System.Array.Copy(in1, 0, tmp, in1.Length - byteLen, byteLen);
            return tmp;
        }

    }
}
