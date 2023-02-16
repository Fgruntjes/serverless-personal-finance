namespace App.Lib.Configuration;

public class AppOptions
{
    public const string OptionsKey = "App";

    public string Frontend { get; set; } = "http://localhost:3000";

    public string Environment { get; set; } = "main";
}