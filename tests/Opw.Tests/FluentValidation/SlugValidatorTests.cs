using FluentAssertions;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Validators;
using System.Linq;
using Xunit;

namespace Opw.FluentValidation
{
    public class SlugValidatorTests
    {
        [Fact]
        public void Validate_Should_ReturnError()
        {
            var validator = new SlugValidator();
            var product = new Product { Slug = "this is not a valid slug" };

            var selector = ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory();
            var context = new PropertyValidatorContext(
                new ValidationContext(product, new PropertyChain(), selector)
                , PropertyRule.Create<Product, string>(p => p.Slug)
                , nameof(product.Slug)
                , product.Slug);

            var result = validator.Validate(context);

            result.Should().HaveCount(1);
            result.First().ErrorMessage.Should().StartWith("Invalid slug");
        }

        [Fact]
        public void Validate_Should_ReturnNoErrors()
        {
            var validator = new SlugValidator();
            var product = new Product { Slug = "this-is-a-valid-slug" };

            var selector = ValidatorOptions.ValidatorSelectors.DefaultValidatorSelectorFactory();
            var context = new PropertyValidatorContext(
                new ValidationContext(product, new PropertyChain(), selector)
                , PropertyRule.Create<Product, string>(p => p.Slug)
                , nameof(product.Slug)
                , product.Slug);

            var result = validator.Validate(context);

            result.Should().HaveCount(0);
        }

        private class Product
        {
            public string Slug { get; set; }
        }
    }
}
