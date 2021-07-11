namespace Ordering.Infrastructure.Seeders
{
    using AutoBogus;
    using AutoBogus.NSubstitute;
    using Ordering.Core.Entities;
    using Ordering.Infrastructure.Contexts;
    using System;
    using System.Linq;

    public static class PatientSeeder
    {
        public static void SeedSamplePatientData(OrderingDbContext context)
        {
            if (!context.Patients.Any())
            {
                foreach (int value in Enumerable.Range(1, 10))
                {
                    context.Patients.Add(new AutoFaker<Patient>()
                        .Configure(c => { c.WithBinder<NSubstituteBinder>(); })
                        .RuleFor(fake => fake.Sex, fake => fake.Person.Gender.ToString())
                        .RuleFor(fake => fake.FirstName, fake => fake.Person.FirstName)
                        .RuleFor(fake => fake.LastName, fake => fake.Person.LastName)
                        .RuleFor(fake => fake.Race, fake => null)
                        .RuleFor(fake => fake.Ethnicity, fake => null)
                        .RuleFor(fake => fake.InternalId, fake => fake.Random.Int(55748, 83927).ToString())
                        .RuleFor(fake => fake.ExternalId, fake => fake.Random.Int(55748, 83927).ToString())
                        .RuleFor(fake => fake.Dob, fake => fake.Date.PastOffset(new Random().Next(10, 50)))
                    );
                }

                context.SaveChanges();
            }
        }
    }
}