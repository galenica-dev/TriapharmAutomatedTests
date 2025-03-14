using System;

namespace DataGenerator
{
    public class AHVAVSNumberGenerator
    {
        private static readonly Random _random = new Random();

        public string GenerateAHVAVSNumber()
        {
            string countryPrefix = "756";  // Fixed country code for Switzerland
            string randomPart = GenerateRandomDigits(9);  // Generate 9 random digits (123456789)
            string checksum = GenerateChecksum(countryPrefix + randomPart);  // Calculate the checksum
            return FormatAHVAVSNumber(countryPrefix + randomPart + checksum);  // Format the full AHV/AVS number
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

        private string GenerateChecksum(string numberWithoutChecksum)
        {
            // Split the number into individual digits
            int[] digits = new int[numberWithoutChecksum.Length];
            for (int i = 0; i < numberWithoutChecksum.Length; i++)
            {
                digits[i] = int.Parse(numberWithoutChecksum[i].ToString());
            }

            // Calculate the sum of every other digit starting from the first
            int sumOddPositions = digits[0] + digits[2] + digits[4] + digits[6] + digits[8] + digits[10];

            // Calculate the sum of every other digit starting from the second
            int sumEvenPositions = digits[1] + digits[3] + digits[5] + digits[7] + digits[9] + digits[11];

            // Multiply the second sum by 3
            int totalSum = sumOddPositions + (sumEvenPositions * 3);

            // Calculate the checksum: 10 minus (totalSum % 10)
            int modulo = totalSum % 10;
            int checksum = (modulo == 0) ? 0 : 10 - modulo;

            return checksum.ToString();
        }

        private string FormatAHVAVSNumber(string number)
        {
            // Format the number as 756.XXXX.XXXX.XX
            return $"{number.Substring(0, 3)}.{number.Substring(3, 4)}.{number.Substring(7, 4)}.{number.Substring(11, 2)}";
        }
    }
}