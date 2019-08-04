using FluentAssertions;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using System;
using System.Linq;
using Xunit;

namespace Opw.FluentValidation
{
    public class RequiredEnumValidatorTests
    {
        [Fact]
        public void Validate_Should_ReturnError()
        {
            var validator = new RequiredEnumValidator();
            var product = new Product { ProductType = ProductType.NotSet };

            var selector = ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory();
            var context = new PropertyValidatorContext(
                new ValidationContext(product, new PropertyChain(), selector)
                , PropertyRule.Create<Product, ProductType>(p => p.ProductType)
                , nameof(product.ProductType)
                , product.ProductType);

            var result = validator.Validate(context);

            result.Should().HaveCount(1);
            result.First().ErrorMessage.Should().Contain("required");
        }

        [Fact]
        public void Validate_Should_ReturnNoErrors()
        {
            var validator = new RequiredGuidValidator();
            var product = new Product { ProductType = ProductType.Hardware };

            var selector = ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory();
            var context = new PropertyValidatorContext(
                new ValidationContext(product, new PropertyChain(), selector)
                , PropertyRule.Create<Product, ProductType>(p => p.ProductType)
                , nameof(product.ProductType)
                , product.ProductType);

            var result = validator.Validate(context);

            result.Should().HaveCount(0);
        }

        private class Product
        {
            public ProductType ProductType { get; set; }
        }

        private enum ProductType
        {
            NotSet = 0,
            Hardware = 1,
            Software = 2
        }
    }
}
