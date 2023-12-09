using ECommerceAPI.Application.ViewModels.Products;
using FluentValidation;


namespace ECommerceAPI.Application.Validators.Products
{
    public class CreateProductValidator:AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("Product name cannot be empty")
                .MaximumLength(150).MinimumLength(3).WithMessage("Product name lenght has to be between 3 and 150 letters");

            RuleFor(x => x.Stock).NotEmpty().NotNull().WithMessage("Stock cannot be empty")
                .Must(s => s >= 0).WithMessage("Stock cannot be negative");

            RuleFor(x => x.Price).NotEmpty().NotNull().WithMessage("Price connot be empty")
                .Must(p => p > 0).WithMessage("Price connat be negative or zero");
        }
    }
}
