using WebAppDataBaseCars.Models;

namespace WebAppDataBaseCars.ViewModels;

public class CarsViewModel
{
    public IEnumerable<Car> Cars { get; set; } = [];

    public IEnumerable<Brand> Brands { get; set; } = [];

    public string? Error { get; set; } = null!;
}