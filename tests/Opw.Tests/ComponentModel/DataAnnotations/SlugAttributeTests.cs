using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;

namespace Opw.ComponentModel.DataAnnotations
{
    public class SlugAttributeTests
    {
        [Fact]
        public void GetValidationResult_Should_ReturnValid()
        {
            var attribute = new SlugAttribute();

            var result = attribute.GetValidationResult("test-slug-10", new ValidationContext(this));

            result.Should().BeNull();
        }

        [Fact]
        public void GetValidationResult_Should_ReturnInvalid_ForEmptyString()
        {
            var attribute = new SlugAttribute();

            var result = attribute.GetValidationResult("", new ValidationContext(this));

            result.ErrorMessage.Should().NotBeNull();
        }

        [Fact]
        public void GetValidationResult_Should_ReturnInvalid_ForStringWithAccents()
        {
            var attribute = new SlugAttribute();

            var result = attribute.GetValidationResult("brûlée", new ValidationContext(this));

            result.ErrorMessage.Should().NotBeNull();
        }

        [Fact]
        public void GetValidationResult_Should_ReturnInvalid_ForStringWithSpaces()
        {
            var attribute = new SlugAttribute();

            var result = attribute.GetValidationResult("test slug", new ValidationContext(this));

            result.ErrorMessage.Should().NotBeNull();
        }

        [Fact]
        public void GetValidationResult_Should_ReturnInvalid_ForStringWithInvalidChar()
        {
            var attribute = new SlugAttribute();

            var result = attribute.GetValidationResult("test:slug", new ValidationContext(this));

            result.ErrorMessage.Should().NotBeNull();
        }
    }
}
