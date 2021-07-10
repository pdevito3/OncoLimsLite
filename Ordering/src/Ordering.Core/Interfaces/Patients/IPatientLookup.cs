using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Interfaces.Patients
{
    public interface IPatientLookup
    {
        Task<bool> PatientIdExists(Guid patientId);

        Task<bool> PatientNameDobExists(Patient patient);
    }
}