namespace Dashboard.Blazor.Pages.Authentication;

public partial class TwoFactorAuthentication
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private string TwoFactorAuth = string.Empty;
    private bool IsUnlock;

    private void TrackTwoFactorAuthCodeChanges()
    {
        var TwoFactorAuthNum = TwoFactorAuth.Replace(" ", "");

        if (TwoFactorAuthNum.Length == 6)
            IsUnlock = true;
        else
            IsUnlock = false;
    }

    private void Submit()
    {
        TwoFactorAuth = TwoFactorAuth.Replace(" ", "");
        MudDialog.Close(DialogResult.Ok(TwoFactorAuth));
    }
}
