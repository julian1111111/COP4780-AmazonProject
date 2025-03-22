using AmazonProject.Models;
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
                return Model?.Name ?? string.Empty;
            }

            set
            {
                if (Model != null && Model.Name != value)
                {
                    Model.Name = value;
                }
            }
        }

        public Product? Model { get; set; }

        public void AddOrUpdateInventory()
        {
            _svc.AddOrUpdateInventory(Model);
        }

        public ProductViewModel()
        {
            Model = new Product();
        }

        public ProductViewModel(Product? model)
        {
            Model = model;
        }
    }
}
