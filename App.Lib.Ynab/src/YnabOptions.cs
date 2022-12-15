namespace App.Lib.Ynab;

public class YnabOptions
{
    public const string OptionsKey = "Ynab";
    
    public string BaseAddress { get; set; }
    public string AppAddress { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}