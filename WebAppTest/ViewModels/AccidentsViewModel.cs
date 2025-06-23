using WebAppDataBaseCars.Models;

namespace WebAppDataBaseCars.ViewModels;
public class AccidentsViewModel
{
    public IEnumerable<Accident> Accidents { get; set; } = [];

    public string? Error { get; set; } = null!;
}

