using System;
using System.Web.Http;
using Hci.ActivePharmacy.Common.Contracts.Data.Persons;
using Language = Hci.ActivePharmacy.Common.Contracts.Data.Language;

namespace PersonApi
{
    [RoutePrefix("api/person")]
    public class PersonController : ApiController
    {
        private readonly PersonServiceClient _personServiceClient;

        public PersonController()
        {
            _personServiceClient = new PersonServiceClient(); // Move initialization inside the constructor
        }

        /// <summary>
        /// Checks if a patient exists.
        /// </summary>
        [HttpGet]
        [Route("is-patient-exist/{patientId}")]
        public IHttpActionResult IsPatientExist(string patientId)
        {
            try
            {
                if (!Guid.TryParse(patientId, out Guid patientGuid))
                {
                    return BadRequest("Invalid GUID format.");
                }

                bool exists = _personServiceClient.CheckPatientExist(patientGuid);
                return Ok(new { isPatientExist = exists });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Retrieves a patient by ID.
        /// </summary>
        [HttpGet]
        [Route("get-patient/{patientId}")]
        public IHttpActionResult GetPatient(string patientId)
        {
            try
            {
                if (!Guid.TryParse(patientId, out Guid patientGuid))
                {
                    return BadRequest("Invalid GUID format.");
                }

                Patient patient = _personServiceClient.GetPatient(patientGuid);

                if (patient == null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    patientId = patient.PatientId,
                    firstName = patient.FirstName,
                    lastName = patient.LastName,
                    countryIso = patient.CountryIso,
                    street1 = patient.Street1,
                    city = patient.City,
                    postalCode = patient.PostalCode,
                    customerId = patient.CustomerId
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Creates a new patient from a JSON request body.
        /// </summary>
        [HttpPost]
        [Route("create-patient")]
        public IHttpActionResult CreatePatient([FromBody] Patient newPatient)
        {
            try
            {
                if (newPatient == null)
                {
                    return BadRequest("Invalid patient data.");
                }

                // Ensure a unique last name for differentiation
                newPatient.LastName = $"{newPatient.LastName} {DateTime.Now:yyyyMMddHHmmss}";

                // Ensure mandatory fields are set
                if (newPatient.Language == Language.French)
                {
                    return BadRequest("Language is mandatory.");
                }

                newPatient = _personServiceClient.SavePatient(newPatient);

                return Ok(new
                {
                    patientId = newPatient.PatientId,
                    firstName = newPatient.FirstName,
                    lastName = newPatient.LastName,
                    addressId = newPatient.AddressId
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
