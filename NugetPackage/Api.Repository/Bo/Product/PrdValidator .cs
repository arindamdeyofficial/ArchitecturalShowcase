using BusinessModel.Product;
using FluentValidation;

public class PrdValidator : AbstractValidator<ProductBo>
{
    public PrdValidator()
    {
        RuleFor(x => x.Version).NotEmpty().WithMessage("Version is required.");
    }
}
