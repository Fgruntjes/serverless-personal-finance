namespace App.Lib.Ynab.Rest.Dto;

public class ApiResponse<TData>
{
	public TData Data { get; set; }
}