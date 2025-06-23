using WebAppDataBaseCars.Models;

namespace WebAppDataBaseCars.ViewModels;
public class OwnersViewModel
{
    public IEnumerable<Owner> Owners { get; set; } = [];

    public string? Error { get; set; } = null!;
}
