using AmazonProject.Models;
using Library.eCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Services
{
    public class ShoppingCartService
    {
        private ProductServiceProxy _prodSvc;
        private static ShoppingCartService? instance;
        private List<Item> items;
        public List<Item> CartItems { get { return items; } }

        public static ShoppingCartService Current 
        { 
            get
            {
                if (instance == null)
                {
                    instance = new ShoppingCartService();
                }
                return instance;
            } 
        }

        private ShoppingCartService()
        {
            items = new List<Item>();
        }
    }
}
