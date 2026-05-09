using System.Reflection;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _dependentPropertyPath;
    private readonly object[] _targetValues;

    public RequiredIfAttribute(string dependentPropertyPath, params object[] targetValues)
    {
        _dependentPropertyPath = dependentPropertyPath;
        _targetValues = targetValues ?? Array.Empty<object>();
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var dependentValue = GetPropertyValue(instance, _dependentPropertyPath);

        var shouldRequire = _targetValues.Any(v => Equals(dependentValue, v));
        if (!shouldRequire)
            return ValidationResult.Success;

        var isEmpty =
            value == null ||
            (value is string s && string.IsNullOrWhiteSpace(s));

        if (isEmpty)
        {
            // return a key + field name so you can map "Required" and still display the name
            var fieldName = validationContext.DisplayName; // uses [Display(Name="...")] if set
            var message = $"The {fieldName} is required";

            return new ValidationResult(message, new[] { validationContext.MemberName! });
        }

        return ValidationResult.Success;
    }

    private static object? GetPropertyValue(object obj, string path)
    {
        object? current = obj;
        foreach (var part in path.Split('.'))
        {
            if (current == null) return null;

            var prop = current.GetType().GetProperty(part, BindingFlags.Public | BindingFlags.Instance);
            if (prop == null) return null;

            current = prop.GetValue(current);
        }
        return current;
    }
}
