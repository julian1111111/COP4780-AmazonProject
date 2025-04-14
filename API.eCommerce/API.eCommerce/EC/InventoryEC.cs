using API.eCommerce.Database;
using Library.eCommerce.DTO;
using Library.eCommerce.Models;

namespace API.eCommerce.EC
{
    public class InventoryEC
    {
        public int LastKey { get; private set; }


        public List<Item?> Get()
        {
            return FakeDatabase.Inventory;
        }

        public Item? Delete(int id)
        {
            var itemToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i?.Id == id);
            if (itemToDelete != null)
            {
                FakeDatabase.Inventory.Remove(itemToDelete);
            }

            return itemToDelete;
        }
    }
}
