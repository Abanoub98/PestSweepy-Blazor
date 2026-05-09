namespace Dashboard.Blazor.Models.Consts;
public enum VisitReportStatus
{
    CheckedIn = 1,              // عمل check-in وبدأ الزيارة (ممكن قبل ما يكتب كل حاجة)
    InProgress = 2,             // بيكمل شغل/بيسجل تفاصيل
    Completed = 3,              // خلّص الزيارة وسجل check-out
    Cancelled = 4               // الزيارة اتلغت/الريبورت اتقفل بدون تنفيذ
}
