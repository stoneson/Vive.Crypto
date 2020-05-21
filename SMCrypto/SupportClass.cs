namespace Vive.Crypto.SMCrypto
{
    internal class SupportClass
    {
        /// <summary>  
        /// Performs an unsigned bitwise right shift with the specified number  
        /// </summary>  
        /// <param name="number">Number to operate on</param>  
        /// <param name="bits">Ammount of bits to shift</param>  
        /// <returns>The resulting number from the shift operation</returns>  
        public static int URShift(int number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2 << ~bits);
        }

        /// <summary>  
        /// Performs an unsigned bitwise right shift with the specified number  
        /// </summary>  
        /// <param name="number">Number to operate on</param>  
        /// <param name="bits">Ammount of bits to shift</param>  
        /// <returns>The resulting number from the shift operation</returns>  
        public static int URShift(int number, long bits)
        {
            return URShift(number, (int)bits);
        }

        /// <summary>  
        /// Performs an unsigned bitwise right shift with the specified number  
        /// </summary>  
        /// <param name="number">Number to operate on</param>  
        /// <param name="bits">Ammount of bits to shift</param>  
        /// <returns>The resulting number from the shift operation</returns>  
        public static long URShift(long number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2L << ~bits);
        }

        /// <summary>  
        /// Performs an unsigned bitwise right shift with the specified number  
        /// </summary>  
        /// <param name="number">Number to operate on</param>  
        /// <param name="bits">Ammount of bits to shift</param>  
        /// <returns>The resulting number from the shift operation</returns>  
        public static long URShift(long number, long bits)
        {
            return URShift(number, (int)bits);
        }


    }
}
