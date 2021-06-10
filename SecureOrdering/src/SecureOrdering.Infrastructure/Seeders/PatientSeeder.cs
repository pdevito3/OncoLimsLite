namespace SecureOrdering.Infrastructure.Seeders
{

    using AutoBogus;
    using SecureOrdering.Core.Entities;
    using SecureOrdering.Infrastructure.Contexts;
    using System.Linq;

    public static class PatientSeeder
    {
        public static void SeedSamplePatientData(OrderingDbContext context)
        {
            if (!context.Patients.Any())
            {
                context.Patients.Add(new AutoFaker<Patient>());
                context.Patients.Add(new AutoFaker<Patient>());
                context.Patients.Add(new AutoFaker<Patient>());

                context.SaveChanges();
            }
        }
    }
}