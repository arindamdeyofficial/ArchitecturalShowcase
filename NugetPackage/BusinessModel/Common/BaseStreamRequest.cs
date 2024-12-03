using BusinessModel.Interface.Common;
using FluentValidation;

namespace BusinessModel.Common
{
    public abstract class BaseStreamRequest<T, U> : IBaseStreamRequest<T, U>
    {
        private readonly IValidator<T> _customValidator;

        public BaseStreamRequest(IValidator<T> validator)
        {
            _customValidator = validator;  // Proper initialization of the validator field
        }

        // ExecuteQuery from IQuery<T, U>
        public async IAsyncEnumerable<U> ExecuteStreamQueryAsync(T request, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResult = await _customValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Execute the query and return an IAsyncEnumerable of results
            await foreach (var result in HandleStreamQuery(request, cancellationToken))
            {
                yield return result;
            }
        }

        // Abstract method that should be implemented in derived classes
        protected abstract IAsyncEnumerable<U> HandleStreamQuery(T request, CancellationToken cancellationToken);
    }
}
