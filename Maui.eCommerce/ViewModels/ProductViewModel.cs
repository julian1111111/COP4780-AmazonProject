﻿using AmazonProject.Models;
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
        private Item? cachedModel { get; set; }

        public string? Name 
        {
            get { return Model?.Product?.Name ?? string.Empty; }

            set
            {
                if (Model != null && Model.Product.Name != value)
                {
                    Model.Product.Name = value;
                }
            }
        }

        public int? Quantity
        { 
            get { return Model?.Product?.Quantity ?? 0; }

            set
            {
                if (Model != null && Model.Product.Quantity != value)
                {
                    Model.Product.Quantity = value ?? 0;
                }
            }
        }

        public double? Price
        { 
            get { return Model?.Product?.Price ?? 0.0; }

            set
            {
                if (Model != null && Model.Product.Price != value)
                {
                    Model.Product.Price = value ?? 0.0;
                }
            }
        }

        public Item? Model { get; set; }

        public ProductViewModel()
        {
            Model = new Item();
            cachedModel = null;
        }

        public ProductViewModel(Item? model)
        {
            Model = model;
            if (model != null)
            {
                cachedModel = new Item(model);
            }
        }

        public void AddOrUpdateInventory()
        {
            _svc.AddOrUpdateInventory(Model);
        }

        public void Undo()
        {
            ProductServiceProxy.Current.AddOrUpdateInventory(cachedModel);
        }
    }
}
