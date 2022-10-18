/*
 * Tesfatsion Shiferaw
 * tms4150
 */
using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Security.Cryptography;


namespace PrimeGen
{
    public static class PrimeGen
    {
        static BigInteger GenerateRandomNumber(String bit)
        {
            // Generates random numer of size: bit
            var toByte = Int16.Parse(bit) / 8;
            var random = RandomNumberGenerator.Create();
            Byte[] bytes = new byte[(int)toByte];
            random.GetBytes(bytes);
            BigInteger number = new BigInteger(bytes);
            return number;
        }

        static bool miillerTest(BigInteger d, BigInteger n)
        {
            // does miller test given the random integer and n-value
            Random r = new Random();
            int a = 2 + (int)(r.Next() % (n - 4));
            BigInteger x = BigInteger.ModPow(a, d, n);
            if (x == 1 || x == n - 1)
                return true;

            while (d != n - 1)
            {
                x = (x * x) % n;
                d *= 2;

                if (x == 1)
                    return false;
                if (x == n - 1)
                    return true;
            }

            return false;
        }

        static Boolean IsProbablyPrime( this BigInteger value, int k = 10)
        {
            //C alls millerTest method checks if a number is probably prime using
            // k value
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
            // runs to generate a random number and check for primality
            // until the requested number of primes are generated.
            int currentCount = 1;
            while (currentCount <= countToInt)
            {
                // Generate random number
                BigInteger randomBitNumber = GenerateRandomNumber(bit);
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

        static void throwError(Boolean bitInputDivisionerror = false, Boolean help = false, Boolean bitNotFound = false)
        {
            // Dedicated to error handling and help message
            if (bitInputDivisionerror)
            {
                Console.WriteLine("Error info: Please enter bit size that is divisible by 8.");
                //return;
            }
            else if (bitNotFound)
            {
                Console.WriteLine("Error info: Please enter bit size");
            }

            else if (help)
            {
                Console.WriteLine("dotnet run <bits> <count=1>\n     - bits - the number of bits of the prime number, this must be a\n     multiple of 8, and at least 32 bits.\n     - count - the number of prime numbers to generate, defaults to 1");
            }
            
            return;
        }


        static void Main(String[] args)
        {
            String countRequested = "1";
            if (args.Length == 2)
            {
                countRequested = args[1];
            }
            else if (args.Length < 1)
            {
                throwError(bitNotFound: true);
                //return;
            }
            else if ((args.Length == 1) && ((args[0] == "-h") || (args[0] == "--help")))
            {
                
            }
            try
            {
                String bit = args[0];
                int bitToInt = Int16.Parse(bit);
                if (bitToInt % 8 != 0)
                {
                    throwError(bitInputDivisionerror: true);
                    return;

                }
                
                int countToInt = Int16.Parse(countRequested);
                Stopwatch clock = new Stopwatch();
                Console.WriteLine("Bit length: " + bit + " bits");
                clock.Start();
                GenPrimes(countToInt, bit);
                clock.Stop();
                TimeSpan ts = clock.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                Console.WriteLine("Time to Generate: " + elapsedTime);
            }
            catch
            {
                throwError(help: true);
            }
            
        }
    }
}