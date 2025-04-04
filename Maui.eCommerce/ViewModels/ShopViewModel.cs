using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        public Item? SelectedProduct { get; set; }
        public string? Query { get; set; }
        private ProductServiceProxy _svc = ProductServiceProxy.Current;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Item?> Products
        {
            get
            {
                var filteredList = _svc.InventoryProducts.Where(p => p?.Product?.Name?.ToLower().Contains(Query?.ToLower() ?? string.Empty) ?? false);
                return new ObservableCollection<Item?>(filteredList);
            }
        }

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
            NotifyPropertyChanged(nameof(Total));
        }

        public Item? Model { get; set; }

        public ShopViewModel()
        {
            Model = new Item();
        }

        public ShopViewModel(Item? model)
        {
            Model = model;
        }

        public string? Total
        {
            get
            {
                double total = _svc.CartProducts.Sum(p => p.Product.Price * p.Product.Quantity) * 1.07;
                return $"Checkout: {total.ToString("C2")}";
            }
        }

        public void AddToCart(int productId, int quantity)
        {
            if (SelectedProduct != null)
            {
                _svc.AddToCart(productId, quantity);
            }
            NotifyPropertyChanged("Products");
            NotifyPropertyChanged("Total");
        }
    }
}
