using AmazonProject.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class InventoryManagementViewModel
    {
        public Product? SelectedProduct { get; set; }
        private ProductServiceProxy _svc = ProductServiceProxy.Current;
        public List<Product?> Products
        {
            get
            {
                return _svc.InventoryProducts;
            }
        }

        public Product? Delete()
        {
            return _svc.DeleteFromInventory(SelectedProduct?.Id ?? 0);
        }
    }
}
