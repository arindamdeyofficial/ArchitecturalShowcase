using BusinessModel.Interface.Common;
using FluentValidation;

namespace BusinessModel.Common
{
    public abstract class BaseCommand<T, U> : BaseRequest<T, U>, IBaseCommand<T, U>
    {
        private readonly IValidator<T> _customValidator;

        public BaseCommand(IValidator<T> validator) : base(validator)
        {
            _customValidator = validator;
        }
    }
}
