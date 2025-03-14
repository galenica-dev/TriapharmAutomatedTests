using Hci.WcfHelper;
using PersonApi.ServiceReferencePerson;
using System;
//using System.Collections.Generic;
//using System.Linq;
using System.ServiceModel;
//using System.Text;
//using System.Threading.Tasks;
using CommonPatient = Hci.ActivePharmacy.Common.Contracts.Data.Persons.Patient;

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
        CommonPatient GetPatient(Guid patientId);

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
        CommonPatient SavePatient(CommonPatient patient);

    }
}
