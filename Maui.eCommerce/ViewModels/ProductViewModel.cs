using AmazonProject.Models;
using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class ProductViewModel
    {
        ProductServiceProxy _svc = ProductServiceProxy.Current;
        public string? Name {
            get
            {
                return Model?.Product?.Name ?? string.Empty;
            }

            set
            {
                if (Model != null && Model.Product.Name != value)
                {
                    Model.Product.Name = value;
                }
            }
        }

        public Item? Model { get; set; }

        public void AddOrUpdateInventory()
        {
            _svc.AddOrUpdateInventory(Model);
        }

        public ProductViewModel()
        {
            Model = new Item();
        }

        public ProductViewModel(Item? model)
        {
            Model = model;
        }
    }
}
