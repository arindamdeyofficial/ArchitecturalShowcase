using BusinessModel.Interface.Common;
using FluentValidation;

namespace BusinessModel.Common
{
    public abstract class BaseStreamQuery<T, U> : BaseStreamRequest<T, U>, IBaseStreamQuery<T, U>
    {
        private readonly IValidator<T> _customValidator;

        public BaseStreamQuery(IValidator<T> validator) : base(validator)
        {
            _customValidator = validator;
        }
    }
}
