using DataFromDb;
using System;

namespace WebApiClient.Models
{
    public class CustomerViewModel
    {
        // Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AddressId { get; set; }
        public Guid? AddressGuid { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
        public int Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Language { get; set; }
        public bool Dead { get; set; }
        public string AHVNumber { get; set; }
        public string CustomerCard { get; set; }
        public bool Active { get; set; }
        public string OmnichannelUUID { get; set; }
        public string AddressSupplement { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string EmployeeCardNumber { get; set; }
        public string Institution { get; set; }
        public Guid RepetitionID { get; set; }
        public string CoverCardNumber { get; set; }
        public DateTime InsuranceValidTo { get; set; }
        public string SqlError { get; set; }
        public string InsuranceName { get; set; }
        public Guid? PatientId { get; set; }

        // Parameterless constructor
        public CustomerViewModel() { }

        // Constructor to initialize from Customer object (optional)
        public CustomerViewModel(Customer customer)
        {
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            AddressId = customer.AddressId;
            AddressGuid = customer.AddressGuid;
            Address1 = customer.Address1;
            Address2 = customer.Address2;
            ZipCode = customer.ZipCode;
            City = customer.City;
            CountryId = customer.CountryId;
            Sex = customer.Sex;
            DateOfBirth = customer.DateOfBirth;
            Language = customer.Language;
            Dead = customer.Dead;
            AHVNumber = customer.AHVNumber;
            CustomerCard = customer.CustomerCard;
            Active = customer.Active;
            OmnichannelUUID = customer.OmnichannelUUID;
            AddressSupplement = customer.AddressSupplement;
            Height = customer.Height;
            Weight = customer.Weight;
            EmployeeCardNumber = customer.EmployeeCardNumber;
            Institution = customer.Institution;
            RepetitionID = customer.RepetitionID;
            CoverCardNumber = customer.CoverCardNumber;
            InsuranceValidTo = customer.InsuranceValidTo;
            SqlError = customer.SqlError;
            InsuranceName = customer.InsuranceName;
            PatientId = customer.PatientId;
        }
    }
}
