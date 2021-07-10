namespace Ordering.WebApi.Features.Patients.Services
{
    using Microsoft.EntityFrameworkCore;
    using Ordering.Core.Entities;
    using Ordering.Core.Interfaces.Patients;
    using Ordering.Infrastructure.Contexts;
    using System;
    using System.Threading.Tasks;

    public class PatientLookup : IPatientLookup
    {
        private readonly OrderingDbContext _db;

        public PatientLookup(OrderingDbContext db)
        {
            _db = db;
        }

        public async Task<bool> PatientIdExists(Guid patientId)
        {
            return await _db.Patients.AnyAsync(p => p.PatientId == patientId);
        }

        public async Task<bool> PatientNameDobExists(Patient patient)
        {
            // Dates need to be able to handle null
            return await _db.Patients.AnyAsync(p =>
                (p.FirstName == patient.FirstName
                && p.LastName == patient.LastName
                && p.Dob.Value.Date == patient.Dob.Value.Date) // Change to DateOnly
            );
        }
    }
}