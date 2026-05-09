namespace Dashboard.Blazor.Models.Consts;

public enum VisitDays
{
    None = 0,
    Saturday = 1 << 0,
    Sunday = 1 << 1,
    Monday = 1 << 2,
    Tuesday = 1 << 3,
    Wednesday = 1 << 4,
    Thursday = 1 << 5,
    Friday = 1 << 6,
}

