namespace Dashboard.Blazor.Models.Dtos;

public class PaymentBaseDto
{
    public string Id { get; set; } = null!;
    public string? Status { get; set; }
    public decimal Amount { get; set; }
    public string? AmountFormat { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public string? Company { get; set; }
    public int OrderId { get; set; }
}

public class PaymentDto
{
    public string Id { get; set; } = null!;
    public int OrderIndex { get; set; }
    public string? Status { get; set; }
    public decimal Fee { get; set; }
    public string? Currency { get; set; }
    public decimal Refunded { get; set; }
    public DateTime? Refunded_at { private get; set; }
    public DateTime? Refunded_atLocal { get => Refunded_at?.ToLocalTime(); }
    public decimal Captured { get; set; }
    public DateTime? Captured_at { private get; set; }
    public DateTime? Captured_atLocal { get => Captured_at?.ToLocalTime(); }
    public DateTime? Voided_at { private get; set; }
    public DateTime? Voided_atLocal { get => Voided_at?.ToLocalTime(); }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string? Amount_format { get; set; }
    public string? Fee_format { get; set; }
    public string? Refunded_format { get; set; }
    public string? Captured_format { get; set; }
    public string? Invoice_id { get; set; }
    public string? IP { get; set; }
    public string? Callback_url { get; set; }
    public DateTime Created_at { private get; set; }
    public DateTime Created_atLocal { get => Created_at.ToLocalTime(); }
    public DateTime Updated_at { private get; set; }
    public DateTime Updated_atLocal { get => Updated_at.ToLocalTime(); }
    public Dictionary<string, string>? Metadata { get; set; }
    public virtual PaymentSource Source { get; set; } = null!;
}

public class PaymentSource
{
    public string? Type { get; set; }
    public string? Company { get; set; }
    public string? Name { get; set; }
    public string? Number { get; set; }
    public string? Gateway_id { get; set; }
    public string? Reference_number { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
    public string? Transaction_url { get; set; }
}