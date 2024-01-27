namespace Dashboard.Blazor.Models.Dtos;

public class DashboardDto
{
    public int ServicesCount { get; set; }
    public int ClientsCount { get; set; }
    public int ContractClientsCount { get; set; }
    public int SupervisorsCount { get; set; }
    public int ManagersCount { get; set; }
    public List<Earning> Earnings { get; set; } = new();
}

public class Earning
{
    public double Amount { get; set; }
    public string CurrencyCode { get; set; } = null!;
    public string CurrencySymbol { get; set; } = null!;
}
