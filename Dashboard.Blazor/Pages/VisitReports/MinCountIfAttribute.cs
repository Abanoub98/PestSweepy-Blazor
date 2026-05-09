using System.Collections;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class MinCountIfAttribute : ValidationAttribute
{
    private readonly string _dependentPropertyPath;
    private readonly int _min;
    private readonly object[] _targetValues;

    public MinCountIfAttribute(string dependentPropertyPath, int min, params object[] targetValues)
    {
        _dependentPropertyPath = dependentPropertyPath;
        _min = min;
        _targetValues = targetValues ?? Array.Empty<object>();

        // default message (can be overridden by ErrorMessage / ErrorMessageResource*)
        ErrorMessage = "{0} is required (at least 1).";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var dependentValue = RequiredIfAttributeAccessor.Get(validationContext.ObjectInstance, _dependentPropertyPath);
        var shouldValidate = _targetValues.Any(v => Equals(dependentValue, v));

        if (!shouldValidate) return ValidationResult.Success;

        // count items
        var count = 0;
        if (value is IEnumerable enumerable)
            count = enumerable.Cast<object>().Count();

        if (count >= _min)
            return ValidationResult.Success;

        // Use DisplayName (Providers, Pests Types, etc.)
        var message = FormatErrorMessage(validationContext.DisplayName);

        return new ValidationResult(message, new[] { validationContext.MemberName! });
    }

    public override string FormatErrorMessage(string name)
        => string.Format(ErrorMessageString, name, _min);

    private static class RequiredIfAttributeAccessor
    {
        public static object? Get(object obj, string path)
        {
            object? current = obj;
            foreach (var part in path.Split('.'))
            {
                if (current == null) return null;
                var prop = current.GetType().GetProperty(part);
                if (prop == null) return null;
                current = prop.GetValue(current);
            }
            return current;
        }
    }
}
