using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using Library.eCommerce.Services;

namespace API.eCommerce.Database
{
    public static class FakeDatabase
    {
        private static List<Item?> inventory = new List<Item?>
            {
                new Item { Product = new ProductDTO{ Id = 1, Name = "Mango WEB", Quantity = 10, Price = 1.00 }, Id = 1, Quantity = 1 },
                new Item { Product = new ProductDTO{ Id = 2, Name = "Banana WEB", Quantity = 10, Price = 1.00 }, Id = 2, Quantity = 1 },
                new Item { Product = new ProductDTO{ Id = 3, Name = "Orange WEB", Quantity = 10, Price = 1.00 }, Id = 3, Quantity = 1 },
            };

        public static List<Item?> Inventory
        {
            get
            {
                return inventory;
            }
        }
        public static int LastKeyItem
        {
            get
            {
                if (!inventory.Any())
                {
                    return 0;
                }

                return inventory.Select(p => p?.Id ?? 0).Max();
            }
        }
    }
}
