using WebAppDataBaseCars.Models;

namespace WebAppDataBaseCars.ViewModels;

public class BrandsViewModel
{
    public IEnumerable<Brand> Brands { get; set; } = [];

    public string? Error { get; set; } = null!;
}

