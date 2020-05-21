using System;

namespace Vive.Crypto.SMCrypto
{
    internal class SM3Digest
    {
        /** SM3值的长度 */
        private static int BYTE_LENGTH = 32;

        /** SM3分组长度 */
        private static int BLOCK_LENGTH = 64;

        /** 缓冲区长度 */
        private static int BUFFER_LENGTH = BLOCK_LENGTH * 1;

        /** 缓冲区 */
        private byte[] xBuf = new byte[BUFFER_LENGTH];

        /** 缓冲区偏移量 */
        private int xBufOff;

        /** 初始向量 */
        public static byte[] iv = { 0x73, (byte) 0x80, 0x16, 0x6f, 0x49,
            0x14, (byte) 0xb2, (byte) 0xb9, 0x17, 0x24, 0x42, (byte) 0xd7,
            (byte) 0xda, (byte) 0x8a, 0x06, 0x00, (byte) 0xa9, 0x6f, 0x30,
            (byte) 0xbc, (byte) 0x16, 0x31, 0x38, (byte) 0xaa, (byte) 0xe3,
            (byte) 0x8d, (byte) 0xee, 0x4d, (byte) 0xb0, (byte) 0xfb, 0x0e,
            0x4e };
        private byte[] V = (byte[])iv.Clone();

        private int cntBlock = 0;

        public SM3Digest()
        {
            //V = (byte[])iv.Clone();
            //System.Array.Copy(iv, 0, V, 0, iv.Length);
        }

        public SM3Digest(SM3Digest t)
        {
            System.Array.Copy(t.xBuf, 0, this.xBuf, 0, t.xBuf.Length);
            this.xBufOff = t.xBufOff;
            System.Array.Copy(t.V, 0, this.V, 0, t.V.Length);
        }

        /**
         * SM3结果输出
         *
         * @param out 保存SM3结构的缓冲区
         * @param outOff 缓冲区偏移量
         * @return
         */
        public int doFinal(byte[] out1, int outOff)
        {
            byte[] tmp = doFinal();
            System.Array.Copy(tmp, 0, out1, 0, tmp.Length);
            return BYTE_LENGTH;
        }

        public void reset()
        {
            xBufOff = 0;
            cntBlock = 0;
            V = (byte[])iv.Clone();
        }

        /**
         * 明文输入
         *
         * @param in
         *            明文输入缓冲区
         * @param inOff
         *            缓冲区偏移量
         * @param len
         *            明文长度
         */
        public void update(byte[] in1, int inOff, int len)
        {
            int partLen = BUFFER_LENGTH - xBufOff;
            int inputLen = len;
            int dPos = inOff;
            if (partLen < inputLen)
            {
                System.Array.Copy(in1, dPos, xBuf, xBufOff, partLen);
                inputLen -= partLen;
                dPos += partLen;
                doUpdate();
                while (inputLen > BUFFER_LENGTH)
                {
                    System.Array.Copy(in1, dPos, xBuf, 0, BUFFER_LENGTH);
                    inputLen -= BUFFER_LENGTH;
                    dPos += BUFFER_LENGTH;
                    doUpdate();
                }
            }

            System.Array.Copy(in1, dPos, xBuf, xBufOff, inputLen);
            xBufOff += inputLen;
        }

        private void doUpdate()
        {
            byte[] B = new byte[BLOCK_LENGTH];
            for (int i = 0; i < BUFFER_LENGTH; i += BLOCK_LENGTH)
            {
                System.Array.Copy(xBuf, i, B, 0, B.Length);
                doHash(B);
            }
            xBufOff = 0;
        }

        private void doHash(byte[] B)
        {
            byte[] tmp = SM3.CF(V, B);
            System.Array.Copy(tmp, 0, V, 0, V.Length);
            cntBlock++;
        }

        private byte[] doFinal()
        {
            byte[] B = new byte[BLOCK_LENGTH];
            byte[] buffer = new byte[xBufOff];
            System.Array.Copy(xBuf, 0, buffer, 0, buffer.Length);
            byte[] tmp = SM3.padding(buffer, cntBlock);
            for (int i = 0; i < tmp.Length; i += BLOCK_LENGTH)
            {
                System.Array.Copy(tmp, i, B, 0, B.Length);
                doHash(B);
            }
            return V;
        }

        public void update(byte in1)
        {
            byte[] buffer = new byte[] { in1 };
            update(buffer, 0, 1);
        }

        public int getDigestSize()
        {
            return BYTE_LENGTH;
        }


        //public static void main(String[] args)
        //{
        //    byte[] md = new byte[32];
        //    byte[] msg1 = "加密和解密都是用C#就可以完美的解决了。".getBytes();
        //    SM3Digest sm3 = new SM3Digest();
        //    sm3.update(msg1, 0, msg1.Length);
        //    sm3.doFinal(md, 0);
        //    String s = new String(Hex.encode(md));
        //    System.out.println(s);
        //}
    }
}
