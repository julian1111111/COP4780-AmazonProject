using API.eCommerce.EC;
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
            return new InventoryEC().Get();
        }

        [HttpGet("{id}")]
        public Item? GetById(int id)
        {
            return new InventoryEC().Get().FirstOrDefault(i => i?.Id == id);
        }

        [HttpDelete("{id}")]
        public Item? Delete(int id)
        {
            return new InventoryEC().Delete(id);
        }

        [HttpPost]
        public Item? AddOrUpdateInventory([FromBody]Item item)
        {
            var newItem = new InventoryEC().AddOrUpdateInventory(item);
            return item;
        }
    }
}
