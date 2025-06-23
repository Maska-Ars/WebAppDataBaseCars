using WebAppDataBaseCars.Models;

namespace WebAppDataBaseCars.ViewModels;

public class MileagesViewModel
    {
        public IEnumerable<Mileage> Mileages { get; set; } = [];

        public string? Error { get; set; } = null!;
    }
