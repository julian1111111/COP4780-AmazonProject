using API.eCommerce.Database;
using Library.eCommerce.DTO;
using Library.eCommerce.Models;

namespace API.eCommerce.EC
{
    public class InventoryEC
    {


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

        public Item? AddOrUpdateInventory(Item item)
        {
            if (item.Id == 0) {
                item.Id = FakeDatabase.LastKeyItem + 1;
                item.Product.Id = item.Id;
                FakeDatabase.Inventory.Add(item);
            }
            else
            {
                var existingItem = FakeDatabase.Inventory.FirstOrDefault(p => p?.Id == item.Id);
                existingItem.Product.Name = item.Product.Name;
                existingItem.Product.Quantity = item.Product.Quantity;
                existingItem.Product.Price = item.Product.Price;
            }
            return item;
        }
    }
}
