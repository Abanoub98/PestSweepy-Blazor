namespace Dashboard.Blazor.Pages.Authentication;

public partial class TwoFactorAuthentication
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = default!;

    private string TwoFactorAuth = string.Empty;
    private bool IsUnlock;

    private void TrackTwoFactorAuthCodeChanges()
    {
        if (TwoFactorAuth.Length == 11)
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
