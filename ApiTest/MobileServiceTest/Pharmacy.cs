using System;
using MobileServiceTest.MobileServiceReference;

namespace MobileServiceTest
{
    public class Pharmacy
    {
        public string OuCode { get; set; }
        public int OuId { get; set; }
        public string PharmacyName { get; set; }
        public System.Version PharmacyVersion { get; set; }
        public Language Language { get; set; }
        public string PharmacyGln { get; set; }

        // Constructor
        public Pharmacy(string ouCode, int ouId, string pharmacyName, System.Version pharmacyVersion, Language language)
        {
            OuCode = ouCode;
            OuId = ouId;
            PharmacyName = pharmacyName;
            PharmacyVersion = pharmacyVersion;
            Language = language;
        }

        // ToString method to display Pharmacy information
        public override string ToString()
        {
            return $"OuCode: {OuCode}, OuId: {OuId}, PharmacyName: {PharmacyName}, PharmacyVersion: {PharmacyVersion}, Language: {Language}";
        }
    }
}
