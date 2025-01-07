namespace WebAppTest.ViewModels
{
    public class MileagesViewModel
    {
        public IEnumerable<Mileage> Mileages { get; set; } = Enumerable.Empty<Mileage>();
        public string? Error { get; set; } = null!;
    }
}
