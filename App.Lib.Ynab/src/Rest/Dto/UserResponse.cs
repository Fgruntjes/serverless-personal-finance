namespace App.Lib.Ynab.Rest.Dto;

public class UserResponse
{
    public class UserData
    {
        public string Id { get; set; }
    }

    public UserData User { get; set; }
}