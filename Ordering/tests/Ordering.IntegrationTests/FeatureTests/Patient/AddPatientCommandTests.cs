namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Ordering.WebApi.Features.Patients;
    using static TestFixture;
    using System;
    using Ordering.Core.Exceptions;
    using Ordering.Core.Interfaces.Patients;
    using NSubstitute;

    public class AddPatientCommandTests : TestBase
    {
        [Test]
        public async Task AddPatientCommand_Adds_New_Patient_To_Db()
        {
            // Arrange
            var fakePatientOne = new FakePatientForCreationDto { }.Generate();

            // Act
            var command = new AddPatient.AddPatientCommand(fakePatientOne);
            var patientReturned = await SendAsync(command);
            var patientCreated = await ExecuteDbContextAsync(db => db.Patients.SingleOrDefaultAsync());

            // Assert
            patientReturned.Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
            patientCreated.Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
        }

        [Test]
        public async Task AddPatientCommand_Throws_Conflict_When_Patient_Name_Dob_Combo_Exists()
        {
            // Arrange
            var patient = new FakePatient { }
                .Generate();

            var conflictRecord = new FakePatientForCreationDto { }.Generate();
            conflictRecord.FirstName = patient.FirstName;
            conflictRecord.LastName = patient.LastName;
            conflictRecord.Dob = patient.Dob;

            await InsertAsync(patient);

            // Act
            var command = new AddPatient.AddPatientCommand(conflictRecord);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<ConflictException>();
        }
    }
}