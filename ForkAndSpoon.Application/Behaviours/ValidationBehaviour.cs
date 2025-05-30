using FluentValidation;
using MediatR;

namespace ForkAndSpoon.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(validate => validate.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                var errorMessages = failures.Select(failure => failure.ErrorMessage).ToArray();

                // If you use OperationResult<T>
                var resultType = typeof(TResponse);
                var failureMethod = resultType.GetMethod("Failure", new[] { typeof(string[]) });

                if (failureMethod != null)
                {
                    return (TResponse)failureMethod.Invoke(null, new object[] { errorMessages })!;
                }

                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}