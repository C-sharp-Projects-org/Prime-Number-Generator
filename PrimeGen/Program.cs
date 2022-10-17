using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Security.Cryptography;


namespace PrimeGen
{
    //Todo: each class/interface needs appropriete description
    public static class PrimeGen
    {
        static BigInteger GenerateRandomNumber(String bit)
        {
            var toByte = Int16.Parse(bit) / 8;
            var random = RandomNumberGenerator.Create();
            Byte[] bytes = new byte[(int)toByte];
            random.GetBytes(bytes);
            BigInteger number = new BigInteger(bytes);
            return number;
        }

        //needs to be cleaned
        //Todo: (CONTINUE HERE!) Working on this method now, check again
        static bool miillerTest(BigInteger d, BigInteger n)
        {
            // n is random Big integer to be tested for primeness
            // d?

            // Pick a random number in [2..n-2]
            // Corner cases make sure that n > 4
            //TODO: needs to be changed
            Random r = new Random();
            int a = 2 + (int)(r.Next() % (n - 4));

            // Compute a^d % n
            //Implemented ModPow
            BigInteger x = BigInteger.ModPow(a, d, n);
            if (x == 1 || x == n - 1)
                return true;

            // Keep squaring x while one of the
            // following doesn't happen
            // (i) d does not reach n-1
            // (ii) (x^2) % n is not 1
            // (iii) (x^2) % n is not n-1
            while (d != n - 1)
            {
                x = (x * x) % n;
                d *= 2;

                if (x == 1)
                    return false;
                if (x == n - 1)
                    return true;
            }

            // Return composite
            return false;
        }

        
        static String throwError(Boolean bitInputDivisionerror = false, Boolean error2 = false)
        {
            //Todo: handle various error messages here
            if (bitInputDivisionerror)
            {
                return "Error info: Please enter bit size that is divisible by 8.";
            }
            return "Error";
        }

        static Boolean IsProbablyPrime( this BigInteger value, int k = 10)
        {
            //Todo: implement this method
            BigInteger d = value - 1;
            while (d % 2 == 0)
                d /= 2;

            // iterate k times
            for (int i = 0; i < k; i++)
            {
                if (miillerTest(d, value) == false)
                    return false;
            }
            return true;
        }

        static void GenPrimes(int countToInt, String bit)
        {
            int currentCount = 1;
            while (currentCount <= countToInt)
            {
                // Generate random number
                BigInteger randomBitNumber = GenerateRandomNumber(bit);
                //Skip corner cases:
                //Corner cases:
                // 1) if even, 2) if less than or equal to 1
                if ((randomBitNumber <= 1) || (randomBitNumber % 2) == 0)
                {
                    //skip this iteration
                    continue;
                }

                // for {1,2}
                else if (randomBitNumber < 3)
                {
                    Console.WriteLine(currentCount + ": " + randomBitNumber);
                    currentCount++;
                }

                else if (randomBitNumber.IsProbablyPrime())
                {
                    Console.WriteLine(currentCount + ": " + randomBitNumber);
                    currentCount++;
                }

            }
        }


        static void Main()
        {
            //check if bit number, and check if it is multiple of 8 (mod)
            //Todo: prime number is natural number
            String bit = "1024"; // arg[0]
            int bitToInt = Int16.Parse(bit);
            if (bitToInt % 8 != 0)
            {
                Console.WriteLine(throwError(bitInputDivisionerror: true));
                return;

            }
            String countRequested = "2"; //arg[1]
            int countToInt = Int16.Parse(countRequested);

            //Todo: Continue from here!
            // call IsProbablyPrime.
            
            Stopwatch clock = new Stopwatch();
            Console.WriteLine("Bit length: " + bit + " bits");
            clock.Start();
            // Todo: put this while look in a void func (GetPrimes) and thread it
            GenPrimes(countToInt, bit);
            
            clock.Stop();
            //Todo: Check time format correctnetss
            TimeSpan ts = clock.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("Time to Generate: " + elapsedTime);


            Console.WriteLine("Run Sequencially");
        }
    }
}