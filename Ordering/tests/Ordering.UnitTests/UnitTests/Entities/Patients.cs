using FluentAssertions;
using NUnit.Framework;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.UnitTests.UnitTests.Entities
{
    public class Patients
    {
        [TestCase("Male", "Male")]
        [TestCase("MALE", "Male")]
        [TestCase("M", "Male")]
        [TestCase("FEMALE", "Female")]
        [TestCase("F", "Female")]
        [TestCase("female", "Female")]
        [TestCase("Gibberish", "Unknown")]
        public void Patient_Sets_Sex_Accurately(string originalValue, string cleanValue)
        {
            var patient = new Patient() { Sex = originalValue };
            patient.Sex.Should().Be(cleanValue);
        }
    }
}