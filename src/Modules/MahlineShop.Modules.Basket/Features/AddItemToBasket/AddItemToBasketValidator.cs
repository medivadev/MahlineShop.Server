using FluentValidation;

namespace MahlineShop.Modules.Basket.Features.AddItemToBasket;

public class AddItemToBasketValidator : AbstractValidator<AddItemToBasketCommand>
{
    public AddItemToBasketValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ProductName).NotEmpty();
        RuleFor(x => x.UnitPrice).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}

