using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(ILogger<InventoryController> logger)
        {
            _logger = logger;
        }

        public int LastKey { get; private set; }

        [HttpGet]
        public IEnumerable<Item?> Get()
        {
            return new List<Item?>
            {
                new Item { Product = new ProductDTO{ Id = ++LastKey, Name = "Mango WEB", Quantity = 10, Price = 1.00 }, Id = LastKey, Quantity = 1 },
                new Item { Product = new ProductDTO{ Id = ++LastKey, Name = "Banana WEB", Quantity = 10, Price = 1.00 }, Id = LastKey, Quantity = 1 },
                new Item { Product = new ProductDTO{ Id = ++LastKey, Name = "Orange WEB", Quantity = 10, Price = 1.00 }, Id = LastKey, Quantity = 1 },
            };
        }
    }
}
