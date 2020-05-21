namespace HCenter.Encryption.SMCrypto
{
    internal class SM4ContextJS
    {
        public int mode;

        public int[] sk;

        public bool isPadding;

        public SM4ContextJS()
        {
            this.mode = 1;
            this.isPadding = true;
            this.sk = new int[32];
        }
    }
}
