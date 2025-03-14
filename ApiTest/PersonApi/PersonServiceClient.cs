using System;
//using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;

//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Hci.Logging;
using Hci.WcfHelper;
using PersonApi.ServiceReferencePerson;
//using PatientServiceReference = PersonService2.ServiceReferencePerson.Patient;
using CommonPatient = Hci.ActivePharmacy.Common.Contracts.Data.Persons.Patient;

namespace PersonApi
{
    public class PersonServiceClient : WcfSimplexClientBase<IPersonService>, IPersonService
    {
        #region Private Fields

        private static readonly ILogger _log = LogManager.GetLoggerForCallingClass();

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonServiceClient" /> class.
        /// </summary>
        public PersonServiceClient()
            : base(_log, null)
        {
            Start();
        }

        #endregion

        /// <summary>
        /// Checks the patient exist.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>
        /// true if patient id is found in Arizona
        /// </returns>
        public bool CheckPatientExist(Guid patientId)
        {
            _log.Info($"Call of CheckPatientExist {string.Join("-", patientId)}");
            return Invoke(c => c.CheckPatientExist(patientId));
        }

        /// <summary>
        /// Get the addressVguid by Id
        /// </summary>
        /// <param name="addressId">The address ID</param>
        /// <returns>The address string.</returns>
        [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP0100:AdvancedNamingRules", Justification = "VGuid is an abbreviation.")]
        public Guid? GetAddressVguidById(int addressId)
        {
            _log.Info($"Call of GetAddressVguidById search VGuid for addressID : {addressId}");
            return Invoke(c => c.GetAddressVguidById(addressId));
        }

        /// <summary>
        /// Gets the patient.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>
        /// The patient loaded from the server.
        /// </returns>
        public CommonPatient GetPatient(Guid patientId)
        {
            _log.Info($"Call of GetPatient {string.Join("-", patientId)}");
            return Invoke(c => c.GetPatient(patientId));
        }

        /// <summary>
        /// Gets the name of the physician.
        /// </summary>
        /// <param name="documentHeaderId">The document header identifier.</param>
        /// <returns>
        /// The full name of the physician
        /// </returns>
        public string GetPhysicianName(int documentHeaderId)
        {
            _log.Info($"Call of GetPhysicianName for document header id {documentHeaderId}.");
            return Invoke(c => c.GetPhysicianName(documentHeaderId));
        }

        /// <summary>
        /// Save the patient.
        /// </summary>
        /// <param name="patient">The patient to be saved.</param>
        /// <returns>
        /// The new <see cref="T:Hci.ActivePharmacy.Common.Contracts.Data.Persons.Patient" />
        /// </returns>
        public CommonPatient SavePatient(CommonPatient patient)
        {
            _log.Info(
                $"Call of SavePatient for '{patient.LastName} {patient.FirstName}', AddressId = {patient.AddressId}, PatientId = '{patient.PatientId}'{Environment.NewLine}{Environment.StackTrace}");
            return Invoke(c => c.SavePatient(patient));
        }



        #region Protected Methods

        protected override void AfterConnect()
        {
        }

        protected override void BeforeDisconnect()
        {
        }

        #endregion
    }
}
