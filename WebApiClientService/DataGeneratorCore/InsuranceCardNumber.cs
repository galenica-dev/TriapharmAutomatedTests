using System;

namespace DataGenerator
{
    public class InsuranceCardNumberGenerator
    {
        private static readonly Random _random = new Random();

        public string GenerateCardNumber()
        {
            string prefix = "80756";  // Fixed starting value
            string randomPart = GenerateRandomDigits(15);  // Generate 15 random digits
            return prefix + randomPart;
        }

        private string GenerateRandomDigits(int length)
        {
            string digits = "";
            for (int i = 0; i < length; i++)
            {
                digits += _random.Next(0, 10);  // Generate a random digit between 0 and 9
            }
            return digits;
        }
    }
}