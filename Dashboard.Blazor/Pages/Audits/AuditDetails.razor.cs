using System.Text.Json;

namespace Dashboard.Blazor.Pages.Audits;

//Copied Code
public partial class AuditDetails
{
    [Parameter] public int Id { get; set; }

    public string? ErrorMessage;
    private AuditDto? Auditing;
    private Dictionary<string, string> OldValuesDict = new();


    protected override async Task OnParametersSetAsync()
    {
        StartProcessing();

        Auditing = await GetByIdAsync<AuditDto>($"SecuirtyLogs/Audit/{Id}");

        if (Auditing.OldValues is not null)
            OldValuesDict = ParseJson(Auditing.OldValues);

        StopProcessing();
    }

    private string ExtractPrimaryKeyValue(string primaryKey)
    {
        if (!string.IsNullOrEmpty(primaryKey))
        {
            var jsonDocument = JsonDocument.Parse(primaryKey);
            var properties = jsonDocument.RootElement.EnumerateObject()
                .Select(property => $"{property.Value}");
            return string.Join(", ", properties);
        }
        return string.Empty;
    }

    private string GetOldValue(string propertyName)
    {
        if (OldValuesDict.TryGetValue(propertyName, out var oldValue))
        {
            if (propertyName.Equals("PrimaryKey") && oldValue.StartsWith("{\"Id\":"))
            {
                using var doc = JsonDocument.Parse(oldValue);
                if (doc.RootElement.TryGetProperty("Id", out var idElement))
                {
                    return idElement.ToString();
                }
            }
            else
            {
                return oldValue;
            }
        }

        return string.Empty;
    }

    private Dictionary<string, string> ParseJson(string json)
    {
        var result = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(json))
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (root.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in root.EnumerateObject())
                {
                    result[property.Name] = property.Value.ToString();
                }
            }
        }
        return result;
    }
}
