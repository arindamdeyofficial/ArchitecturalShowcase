using BusinessModel.Interface.Common;
using FluentValidation;

namespace BusinessModel.Common
{
    public abstract class BaseRequest<T, U> : IBaseRequest<T, U>
    {
        private readonly IValidator<T> _customValidator;

        public BaseRequest(IValidator<T> validator)
        {
            _customValidator = validator;
        }

        // ExecuteQuery from IQuery<T, U>
        public async Task<U> Execute(T request, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await _customValidator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                // Handle validation failure, e.g., throw an exception or return a default value
                throw new ValidationException(validationResult.Errors);
            }

            // Proceed with the actual query handling
            return await Handle(request, cancellationToken);
        }

        // Abstract method that should be implemented in derived classes
        protected abstract Task<U> Handle(T request, CancellationToken cancellationToken);
    }
}
