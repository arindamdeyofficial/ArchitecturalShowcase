using BusinessModel.Interface.Common;
using FluentValidation;

namespace BusinessModel.Common
{
    public abstract class BaseQuery<T, U> : BaseRequest<T, U>, IBaseQuery<T, U>
    {
        private readonly IValidator<T> _customValidator;

        public BaseQuery(IValidator<T> validator) : base(validator)
        {
            _customValidator = validator;
        }
    }
}
