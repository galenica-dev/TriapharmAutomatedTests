using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFromDb
{
    public class Customer
    {
        // WARNING when add or updating properties, update the view model in WebApiClient/Models/CustomerViewModel.cs
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

    }
}
