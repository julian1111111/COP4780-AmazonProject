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
    public class CheckoutViewModel : INotifyPropertyChanged
    {
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
                //var filteredList = _svc.CartProducts.Where(p => p?.Product?.Name?.ToLower().Contains(Query?.ToLower() ?? string.Empty) ?? false);
                return new ObservableCollection<Item?>(_svc.CartProducts);
            }
        }

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Tax));
            NotifyPropertyChanged(nameof(Products));
            NotifyPropertyChanged(nameof(OrderPrice));
        }

        public Item? Model { get; set; }

        public CheckoutViewModel()
        {
            Model = new Item();
        }

        public CheckoutViewModel(Item? model)
        {
            Model = model;
        }

        public string? Tax
        {
            get
            {
                
                //double tax = _svc.CartProducts.Sum(p => p.Product.Price * p.Product.Quantity) * 0.07;
                var rate = AppSettings.SalesTaxRate;
                double tax = _svc.CartProducts
                    .Sum(p => p.Product.Price * p.Product.Quantity * (rate));
                return $"{tax.ToString("C2")}";
            }
        }

        public double? Total
        {
            get
            {
                var rate = AppSettings.SalesTaxRate;
                double total = _svc.CartProducts
                    .Sum(p => p.Product.Price * p.Product.Quantity * (1 + rate));
                return total;
            }
        }

        public string? OrderPrice
        {
            get
            {
                return $"Complete Order: {Total?.ToString("C2")}";
            }
        }

        public void ClearCart()
        {
            _svc.ClearCart();
            NotifyPropertyChanged("Products");
        }
    }
}
