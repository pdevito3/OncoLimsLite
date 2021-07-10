namespace Ordering.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Ordering.Core.Interfaces.Patients;
    using Sieve.Attributes;

    [Table("Patient")]
    public class Patient
    {
        private string _sex;

        [Key]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public Guid PatientId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ExternalId { get; set; }

        //[Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public string InternalId { get; private set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string FirstName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string LastName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTimeOffset? Dob { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Sex
        {
            get => _sex;
            set
            {
                if (value == null)
                    _sex = "Unknown";
                else if (value.Equals("Male", StringComparison.InvariantCultureIgnoreCase) || value.Equals("M", StringComparison.InvariantCultureIgnoreCase))
                    _sex = "Male";
                else if (value.Equals("Female", StringComparison.InvariantCultureIgnoreCase) || value.Equals("F", StringComparison.InvariantCultureIgnoreCase))
                    _sex = "Female";
                else
                    _sex = "Unknown";
            }

            //init c#10, can change to this since i'm using the same data type
            //get;
            //set
            //{
            //    if (value.Equals("Male", StringComparison.InvariantCultureIgnoreCase) || value.Equals("M", StringComparison.InvariantCultureIgnoreCase))
            //        field = "Male";
            //    else if (value.Equals("Female", StringComparison.InvariantCultureIgnoreCase) || value.Equals("F", StringComparison.InvariantCultureIgnoreCase))
            //        field = "Female";
            //    else
            //        field = "Unknown";
            //}
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Gender { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Race { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Ethnicity { get; set; }

        // add-on property marker - Do Not Delete This Comment

        public Patient(string firstName,
            string lastName,
            string externalId,
            DateTimeOffset? dob,
            string gender,
            string sex,
            string race,
            string ethnicity,
            IInternalIdGenerator internalIdGenerator
        )
        {
            PatientId = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            ExternalId = externalId;
            Dob = dob;
            Gender = gender;
            Sex = sex;
            Race = race;
            Ethnicity = ethnicity;
            InternalId = internalIdGenerator.NewInternalId();
        }

        // for EF
        private Patient() { }
    }
}