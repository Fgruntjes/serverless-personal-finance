using Rebus.Bus;

namespace App.Job.Import.Ynab.Service;

public class YnabImportService
{
    private readonly IBus _bus;

    public YnabImportService(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task Import()
    {
        throw new NotImplementedException();
    }
}