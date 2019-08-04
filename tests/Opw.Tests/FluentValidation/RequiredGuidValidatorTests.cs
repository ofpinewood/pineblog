using FluentAssertions;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using System;
using System.Linq;
using Xunit;

namespace Opw.FluentValidation
{
    public class RequiredGuidValidatorTests
    {
        [Fact]
        public void Validate_Should_ReturnError()
        {
            var validator = new RequiredGuidValidator();
            var product = new Product();

            var selector = ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory();
            var context = new PropertyValidatorContext(
                new ValidationContext(product, new PropertyChain(), selector)
                , PropertyRule.Create<Product, Guid>(p => p.Id)
                , nameof(product.Id)
                , product.Id);

            var result = validator.Validate(context);

            result.Should().HaveCount(1);
            result.First().ErrorMessage.Should().Contain("required");
        }

        [Fact]
        public void Validate_Should_ReturnNoErrors()
        {
            var validator = new RequiredGuidValidator();
            var product = new Product { Id = Guid.NewGuid() };

            var selector = ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory();
            var context = new PropertyValidatorContext(
                new ValidationContext(product, new PropertyChain(), selector)
                , PropertyRule.Create<Product, Guid>(p => p.Id)
                , nameof(product.Id)
                , product.Id);

            var result = validator.Validate(context);

            result.Should().HaveCount(0);
        }

        private class Product
        {
            public Guid Id { get; set; }
        }
    }
}
