using System;

namespace WebAppTest.ViewModels
{
    public class ViewModel
    {
        public IEnumerable<Car> Cars { get; set; } = Enumerable.Empty<Car>();
        public IEnumerable<Brand> Brands { get; set; } = Enumerable.Empty<Brand>();
    }
}