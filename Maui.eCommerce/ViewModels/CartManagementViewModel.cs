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
    public class CartManagementViewModel : INotifyPropertyChanged
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
                var filteredList = _svc.CartProducts.Where(p => p?.Product?.Name?.ToLower().Contains(Query?.ToLower() ?? string.Empty) ?? false);
                return new ObservableCollection<Item?>(filteredList);
            }
        }

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
            NotifyPropertyChanged(nameof(Total));
        }

        public string? Total
        {
            get
            {
                //double total = _svc.CartProducts.Sum(p => p.Product.Price * p.Product.Quantity) * 1.07;
                var rate = AppSettings.SalesTaxRate;
                double total = _svc.CartProducts
                    .Sum(p => p.Product.Price * p.Product.Quantity * (1 + rate));
                return $"Checkout: {total.ToString("C2")}";
            }
        }

        public void RemoveFromCart(int productId, int quantity)
        {
            if (SelectedProduct != null)
            {
                _svc.RemoveFromCart(productId, quantity);
            }
            NotifyPropertyChanged("Products");
            NotifyPropertyChanged("Total");
        }
    }
}
