using System;
using CommonPatient = Hci.ActivePharmacy.Common.Contracts.Data.Persons.Patient;
using Person = Hci.ActivePharmacy.Common.Contracts.Data.Persons.Person;
using Language = Hci.ActivePharmacy.Common.Contracts.Data.Language;
using PersonApi.ServiceReferencePerson;
using Hci.ActivePharmacy.Common.Contracts.Data.Persons;
using Hci.ActivePharmacy.Common.Contracts.Data.LoyaltyCardMicroService;

namespace PersonApi
{
    class Program
    {
        
        static void Main(string[] args)
        {
            PersonServiceClient personServiceClient = new PersonServiceClient();
            CommonPatient patient = new CommonPatient();

            bool back = false;

            while (!back)
            {
                // Main Menu
                Console.Clear();
                Console.WriteLine("0. Exit");
                Console.WriteLine("******* Test Person Service *******");
                Console.WriteLine("1. Check customer exists");
                Console.WriteLine("2. Get address Vguid by addressId");
                Console.WriteLine("3. Get Physician Name");
                Console.WriteLine("4. Get Patient");
                Console.WriteLine("5. Create Patient");
                Console.Write("Select an option: ");
                string mainChoice = Console.ReadLine();

                switch (mainChoice)
                {

                    case "1":
                        CheckCustomerExists();
                        break;

                    case "2":
                        GetAddressVguidByAddressId();
                        break;

                    case "3":
                        GetPhysicianName();
                        break;

                    case "4":
                        GetPatient();
                        break;

                    case "5":
                        CreatePatient();
                        break;

                    case "0":
                        back = true;
                        Console.WriteLine("Exiting... Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

                if (!back)
                {
                    Console.WriteLine("\nType 'ok' to return to the menu...");

                    string userInput;
                    do
                    {
                        userInput = Console.ReadLine()?.Trim(); // Read user input and trim whitespace
                    } while (!string.Equals(userInput, "ok", StringComparison.OrdinalIgnoreCase));

                    Console.WriteLine("Returning to the menu...");
                }
            }
        }

        static public void CheckCustomerExists() 
        {
            PersonServiceClient personServiceClient = new PersonServiceClient();

            Console.WriteLine("Check if customer exist?");
            Guid patientGuidFalse = Guid.NewGuid();
            Guid patientGuidTrue = Guid.Parse("9AFC6B29-E369-49B7-B9F8-4689F542EE45");
            Console.WriteLine($"Should be false : {personServiceClient.CheckPatientExist(patientGuidFalse).ToString()}");
            Console.WriteLine($"Should be true : {personServiceClient.CheckPatientExist(patientGuidTrue).ToString()}");
        }

        static public void GetAddressVguidByAddressId() 
        {
            PersonServiceClient personServiceClient = new PersonServiceClient();

            Console.WriteLine("Get address Vguid by addressId");
            int addressId = 1500042285;
            Guid addressVguid = Guid.Parse(personServiceClient.GetAddressVguidById(addressId).ToString());
            Console.WriteLine($"Get the addressVguid by Id : {addressVguid}");
        }

        static public void GetPhysicianName() 
        {
            PersonServiceClient personServiceClient = new PersonServiceClient();

            Console.WriteLine("Get Physician Name");
            int documentHeaderId = 10000600;
            string physicianName = personServiceClient.GetPhysicianName(documentHeaderId);
            Console.WriteLine($"Get the physician name : {physicianName}");
        }

        static public void GetPatient()
        {
            PersonServiceClient personServiceClient = new PersonServiceClient();
            CommonPatient patient = new CommonPatient();

            Console.WriteLine("Get Patient");
            Guid patientGuid = Guid.Parse("47B790C9-38E9-4F4F-A5FA-54A2B7309FF9");
            patient = personServiceClient.GetPatient(patientGuid);
            Console.WriteLine($"Get PatientId : {patient.PatientId}");
            Console.WriteLine($"Get FirstName : {patient.FirstName}");
            Console.WriteLine($"Get LastName : {patient.LastName}");
            Console.WriteLine($"Get CountryIso : {patient.CountryIso}");
            Console.WriteLine($"Get Street1 : {patient.Street1}");
            Console.WriteLine($"Get City : {patient.City}");
            Console.WriteLine($"Get PostalCode : {patient.PostalCode}");
            Console.WriteLine($"Get CustomerId : {patient.CustomerId}");
            //Console.WriteLine($"Get the patient FN LN : {personServiceClient.GetFirstAndLastName(patientGuidTrue)}");
        }

        static public void CreatePatient()
        {
            PersonServiceClient personServiceClient = new PersonServiceClient();

            Console.WriteLine("Create Patient");

            CommonPatient newPatient = new CommonPatient
            {
                //AddressId = 123456, // Dont use it for create patient
                FirstName = "Toto Test",
                LastName = $"Tata {DateTime.Now.ToString("yyyyMMddHHmmss")}", // have a way to differentiate patient created
                //PatientId = Guid.NewGuid(), // Dont use it for create patient
                Street1 = "Rue Ruchonet 12",
                City = "Lausanne",
                PostalCode = "1000",
                CountryIso = "CH",
                Language = Language.French, // Madatory
                DateOfBirth = DateTime.Now.AddYears(-46),
                //Gender = Gender.Female,
                //InstitutionId = institutionId,
                //SocialSecurityNumber = cardNumber,

            };

            newPatient = personServiceClient.SavePatient(newPatient);
            Console.WriteLine($"Get PatientId of the new patient: {newPatient.PatientId}");
            Console.WriteLine($"Get AddressID of the new patient : {newPatient.AddressId}");
            Console.WriteLine($"Get FirstName of the new patient : {newPatient.FirstName}");
            Console.WriteLine($"Get LastNam of the new patiente : {newPatient.LastName}");
        }
    }
}
