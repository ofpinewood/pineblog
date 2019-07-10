using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;

namespace Opw.ComponentModel.DataAnnotations
{
    public class RequiredEnumAttributeTests
    {
        [Fact]
        public void GetValidationResult_Should_ReturnValid()
        {
            var attribute = new RequiredEnumAttribute();

            var result = attribute.GetValidationResult(TestEnum.A, new ValidationContext(this));

            result.Should().BeNull();
        }

        [Fact]
        public void GetValidationResult_Should_ReturnInvalid_ForNotSet()
        {
            var attribute = new RequiredEnumAttribute();

            var result = attribute.GetValidationResult(TestEnum.NotSet, new ValidationContext(this));

            result.ErrorMessage.Should().NotBeNull();
        }

        [Fact]
        public void GetValidationResult_Should_ReturnInvalid_ForNull()
        {
            var attribute = new RequiredEnumAttribute();

            var result = attribute.GetValidationResult(null, new ValidationContext(this));

            result.ErrorMessage.Should().NotBeNull();
        }

        private enum TestEnum
        {
            NotSet = 0,
            A = 1,
            B = 2
        }
    }
}
