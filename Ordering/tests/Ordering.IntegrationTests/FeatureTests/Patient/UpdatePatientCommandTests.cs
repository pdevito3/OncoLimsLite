namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.IntegrationTests.TestUtilities;
    using Ordering.Core.Dtos.Patient;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using Ordering.WebApi.Features.Patients;
    using static TestFixture;
    using System;
    using Ordering.Core.Exceptions;
    using System.Collections.Generic;

    public class UpdatePatientCommandTests : TestBase
    {
        [Test]
        public async Task UpdatePatientCommand_Updates_Existing_Patient_In_Db()
        {
            // Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var updatedPatientDto = new FakePatientForUpdateDto { }.Generate();
            await InsertAsync(fakePatientOne);

            var patient = await ExecuteDbContextAsync(db => db.Patients.SingleOrDefaultAsync());
            var patientId = patient.PatientId;

            // Act
            var command = new UpdatePatient.UpdatePatientCommand(patientId, updatedPatientDto);
            await SendAsync(command);
            var updatedPatient = await ExecuteDbContextAsync(db => db.Patients.Where(p => p.PatientId == patientId).SingleOrDefaultAsync());

            // Assert
            updatedPatient.Should().BeEquivalentTo(updatedPatientDto, options =>
                options.ExcludingMissingMembers());
        }

        [Test]
        public async Task UpdatePatientCommand_Throws_Error_If_Patient_Exists()
        {
            // Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            var updatedPatientDto = new FakePatientForUpdateDto { }.Generate();
            await InsertAsync(fakePatientOne, fakePatientTwo);

            var patient = await ExecuteDbContextAsync(db => db.Patients.FirstOrDefaultAsync());
            var patientId = patient.PatientId;

            updatedPatientDto.FirstName = fakePatientTwo.FirstName;
            updatedPatientDto.LastName = fakePatientTwo.LastName;
            updatedPatientDto.Dob = fakePatientTwo.Dob;

            // Act
            var command = new UpdatePatient.UpdatePatientCommand(patientId, updatedPatientDto);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<ConflictException>();
        }

        [Test]
        public async Task UpdatePatientCommand_Throws_Error_If_Patient_Null()
        {
            // Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            await InsertAsync(fakePatientOne);

            var patient = await ExecuteDbContextAsync(db => db.Patients.FirstOrDefaultAsync());
            var patientId = patient.PatientId;

            // Act
            var command = new UpdatePatient.UpdatePatientCommand(patientId, null);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}