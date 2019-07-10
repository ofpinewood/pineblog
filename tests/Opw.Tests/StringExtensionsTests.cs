using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace Opw
{
    public class StringExtensionsTests
    {
        [Fact]
        public void ToSlug_Should_ReturnSlug_WithoutAccents()
        {
            var result = "brûlée".ToSlug();

            result.Should().Be("brulee");
        }

        [Fact]
        public void ToSlug_Should_ReturnSlug_WithSpacesReplacedByHyphens()
        {
            var result = "this is a string".ToSlug();

            result.Should().Be("this-is-a-string");
        }

        [Fact]
        public void ToSlug_Should_ReturnSlug_WithCamelCaseReplacedByHyphens()
        {
            var result = "thisIsAString".ToSlug();

            result.Should().Be("this-is-a-string");
        }

        [Fact]
        public void ToSlug_Should_ReturnSlug_WithoutSpecialChars()
        {
            var result = "this@*_10".ToSlug();

            result.Should().Be("this10");
        }
    }
}
