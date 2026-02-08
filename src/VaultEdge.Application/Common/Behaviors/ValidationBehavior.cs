using ErrorOr;
using MediatR;
using FluentValidation;

namespace VaultEdge.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : 
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(_validator is null)
            {
                return await next();
            }

            var valdiationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (valdiationResult.IsValid)
            {
                return await next();
            }

            var errors = valdiationResult.Errors
                .ConvertAll(validationFailure => Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage));

            return (dynamic)errors;
        }
    }
}
