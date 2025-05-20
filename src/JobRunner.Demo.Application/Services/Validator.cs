using Microsoft.Extensions.DependencyInjection;
using FluentValidation.Results;
using FluentValidation;

namespace JobRunner.Demo.Application.Services;

public class Validator
{
    private readonly IServiceProvider _serviceProvider;
    public Validator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public bool Validate<TValidator, T>(T entity, out IReadOnlyCollection<string> exceptions)
        where TValidator : IValidator
    {
        var validator = GetValidator<TValidator>();
        var validationResult = ValidateInternal(validator, entity);

        return GetValidationExceptions(validationResult, out exceptions);
    }

    public void ValidateOrThrow<TValidator, T>(T entity)
        where TValidator : IValidator
    {
        var validator = GetValidator<TValidator>();
        var validationResult = ValidateInternal(validator, entity);
        var isValid = GetValidationExceptions(validationResult, out var exceptions);

        if (!isValid)
            throw new InvalidOperationException($"The entity '{typeof(T).Name}' did not pass validation. Errors: "
                + string.Join(", ", exceptions));
    }

    public async Task<(bool, IReadOnlyCollection<string>)> ValidateAsync<TValidator, T>(T entity)
        where TValidator : IValidator
    {
        var validator = GetValidator<TValidator>();
        var validationResult = await ValidateInternalAsync(validator, entity);
        var isValid = GetValidationExceptions(validationResult, out var exceptions);

        return (isValid, exceptions);
    }

    public async Task ValidateOrThrowAsync<TValidator, T>(T entity)
        where TValidator : IValidator
    {
        var validator = GetValidator<TValidator>();
        var validationResult = await ValidateInternalAsync(validator, entity);
        var isValid = GetValidationExceptions(validationResult, out var exceptions);

        if (!isValid)
            throw new InvalidOperationException($"The entity '{nameof(T)}' did not pass validation. Errors: "
                + string.Join(", ", exceptions));
    }

    public async Task<(bool, IReadOnlyCollection<string>)> ValidateAsync(object entity)
    {
        var validator = GetValidator(entity);
        var validationResult = await ValidateInternalAsync(validator, entity);

        var isValid = GetValidationExceptions(validationResult, out var exceptions);

        return (isValid, exceptions);
    }

    public async Task ValidateOrThrowAsync(object entity)
    {
        var validator = GetValidator(entity);
        var validationResult = await ValidateInternalAsync(validator, entity);
        var isValid = GetValidationExceptions(validationResult, out var exceptions);

        if (!isValid)
            throw new InvalidOperationException($"The entity '{entity.GetType().Name}' did not pass validation. Errors: "
                                                + string.Join(", ", exceptions));
    }

    private IValidator GetValidator<TValidator>()
        where TValidator : IValidator
    {
        var validator = _serviceProvider.GetService<TValidator>();

        if (validator == null)
            throw new InvalidOperationException($"Unable to resolve " +
                $"validator '{nameof(TValidator)}' in DI");

        return validator;
    }

    private IValidator GetValidator(object entity)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(entity.GetType());
        var validators = _serviceProvider.GetServices(validatorType).ToArray();

        if (validators.Length == 0)
            throw new InvalidOperationException($"Unable to resolve " +
                                                $"validator for type '{entity.GetType().Name}' in DI");

        if (validators.Length > 1)
            throw new InvalidOperationException($"More than one validator found " +
                                                $"for type '{entity.GetType().Name}' in DI");

        return (IValidator)validators[0]!;
    }

    private static ValidationResult ValidateInternal<T>(IValidator validator, T entity)
    {
        var validationContext = new ValidationContext<T>(entity);
        var validationResult = validator.Validate(validationContext);

        return validationResult;
    }
    private static async Task<ValidationResult> ValidateInternalAsync(IValidator validator, object entity)
    {
        var validationContext = new ValidationContext<object>(entity);
        var validationResult = await validator.ValidateAsync(validationContext);

        return validationResult;
    }

    private static async Task<ValidationResult> ValidateInternalAsync<T>(IValidator validator, T entity)
    {
        var validationContext = new ValidationContext<T>(entity);
        var validationResult = await validator.ValidateAsync(validationContext);

        return validationResult;
    }

    private static bool GetValidationExceptions(
        ValidationResult validationResult, out IReadOnlyCollection<string> exceptions)
    {
        exceptions = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToArray();

        return validationResult.IsValid;
    }
}
