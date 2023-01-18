namespace App.Lib.Dto.Frontend;

public class IntegrationStatus
{
    public bool Connected { get; init; }
    public string? AccountName { get; init; }

    public IntegrationStatus() : this(false, null)
    {
    }

    public IntegrationStatus(bool connected = false, string? accountName = null)
    {
        Connected = connected;
        AccountName = accountName;
    }
}