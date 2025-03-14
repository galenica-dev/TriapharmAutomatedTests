using System;
using System.ServiceModel;
using Hci.WcfToolkit.Client;
using MobileServiceTest.MobileServiceReference;
using DataFromDb;
using System.Collections.Generic;

namespace MobileServiceTest
{
    public class PharmacyService
    {
        private string _urlService = "https://localhost:33365/ActivePharmacy/MobileService/MobileService.svc";
        private string _connectionStringActivePosRead = "Server=localhost;Database=ActivePos_read;Integrated Security=true;";

        // Method to retrieve pharmacy information from the WCF service
        public Pharmacy GetPharmacyInfo()
        {
            
            try
            {
                Uri uri = new Uri(_urlService);
                ChannelFactory<MobileServiceChannel> factory = new ChannelFactory<MobileServiceChannel>();
                factory = CustomUriClientProxy<MobileServiceChannel>.CreateChannelFactory(uri);
                factory.Open();

                var channel = factory.CreateChannel();

                    var pharmacyInfo = channel.GetPharmacyServerInfo();
           


                // Create and return a Pharmacy object with the retrieved data
                var pharmacy = new Pharmacy(
                    pharmacyInfo.OuCode,
                    pharmacyInfo.OuId,
                    pharmacyInfo.PharmacyName,
                    pharmacyInfo.PharmacyVersion,
                    pharmacyInfo.Language
                );

                //Get GLN
                if (pharmacy.OuCode != null)
                {
                    pharmacy.PharmacyGln = PharmacyInfo.GetPharmacyGLN(pharmacy.OuCode);
                }

                factory.Close();

                return pharmacy;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }
        }
    }
}
