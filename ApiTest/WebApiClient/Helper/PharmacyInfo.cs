using MobileServiceTest;
using MobileServiceTest.MobileServiceReference;
using System;
using WebApiClient.Models;

namespace WebApiClient.Helper
{
    public class PharmacyInfo
    {
        public static PharmacyInfoViewModel GetPharmacyInfo()
        {
            // Get the pharmacy information
            PharmacyService pharmacyService = new PharmacyService();
            Pharmacy pharmacy = pharmacyService.GetPharmacyInfo();

            if(pharmacy != null) { 
                // Create a new instance of PharmacyInfoViewModel
                var pharmacyInfo = new PharmacyInfoViewModel
                {
                    OuCode = pharmacy.OuCode,
                    OuId = pharmacy.OuId,
                    PharmacyName = pharmacy.PharmacyName,
                    PharmacyVersion = pharmacy.PharmacyVersion.ToString(),
                    Language = pharmacy.Language.ToString(),
                    PharmacyGln = pharmacy.PharmacyGln
                };

                // Pass the list to the view as the model
                return pharmacyInfo;
            }
            else
            {
                return null;
            }

        }

    }
}