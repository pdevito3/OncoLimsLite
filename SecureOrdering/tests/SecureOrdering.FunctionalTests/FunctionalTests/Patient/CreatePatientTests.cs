namespace SecureOrdering.FunctionalTests.FunctionalTests.Patient
{
    using SecureOrdering.SharedTestHelpers.Fakes.Patient;
    using SecureOrdering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CreatePatientTests : TestBase
    {
        [Test]
        public async Task Create_Patient_Returns_Created_WithAuth()
        {
            // Arrange
            var fakePatient = new FakePatientForCreationDto { }.Generate();

            _client.AddAuth(new[] {"patients.add"});

            // Act
            var route = ApiRoutes.Patients.Create;
            var result = await _client.PostJsonRequestAsync(route, fakePatient);

            // Assert
            result.StatusCode.Should().Be(201);
        }
            
        [Test]
        public async Task Create_Patient_Returns_Unauthorized_Without_Valid_Token()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Create;
            var result = await _client.PostJsonRequestAsync(route, fakePatient);

            // Assert
            result.StatusCode.Should().Be(401);
        }
            
        [Test]
        public async Task Create_Patient_Returns_Forbidden_Without_Proper_Scope()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();
            _client.AddAuth();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Create;
            var result = await _client.PostJsonRequestAsync(route, fakePatient);

            // Assert
            result.StatusCode.Should().Be(403);
        }
    }
}