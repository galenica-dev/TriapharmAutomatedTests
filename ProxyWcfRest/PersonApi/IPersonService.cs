using Hci.WcfHelper;
using PersonApi.ArizonaPersonServiceProxy;
using System;
using System.ServiceModel;
using Patient = Hci.ActivePharmacy.Common.Contracts.Data.Persons.Patient;

namespace PersonApi
{
    [ServiceContract(
        Name = "PersonService",
        Namespace = "http://triamun.ch/connectic/service/PersonService/")
    ]
    public interface IPersonService
    {
        /// <summary>
        /// Checks the patient exist.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>true if patient id is found in arizona</returns>
        [OperationContract]
        [NetDataContract]
        //[ServiceInspector]
        [FaultContract(typeof(ExceptionDetail))]
        bool CheckPatientExist(Guid patientId);

        /// <summary>
        /// Get the addressVguid by id
        /// </summary>
        /// <param name="addressId">integer to identify address.</param>
        /// <returns>
        /// String that represent address.
        /// </returns>
        [OperationContract]
        [NetDataContract]
        //[ServiceInspector]
        [FaultContract(typeof(ExceptionDetail))]
        Guid? GetAddressVguidById(int addressId);

        /// <summary>
        /// Gets the patient.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>The patient loaded from the server.</returns>
        [OperationContract]
        [NetDataContract]
        //[ServiceInspector]
        [FaultContract(typeof(ExceptionDetail))]
        Patient GetPatient(Guid patientId);

        /// <summary>
        /// Gets the name of the physician.
        /// </summary>
        /// <param name="documentHeaderId">The document header identifier.</param>
        /// <returns>The full name of the physician</returns>
        [OperationContract]
        [NetDataContract]
        //[ServiceInspector]
        [FaultContract(typeof(ExceptionDetail))]
        string GetPhysicianName(int documentHeaderId);

        /// <summary>
        /// Save the patient.
        /// </summary>
        /// <param name="patient">The patient to be saved.</param>
        /// <returns>
        /// The new <see cref="Patient" />
        /// </returns>
        [OperationContract]
        [NetDataContract]
        //[ServiceInspector]
        [FaultContract(typeof(ServiceExceptionDetail))]
        Patient SavePatient(Patient patient);

    }
}

