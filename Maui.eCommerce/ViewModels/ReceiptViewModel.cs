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
    public class ReceiptViewModel
    {
        ProductServiceProxy _svc = ProductServiceProxy.Current;
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
                return new ObservableCollection<Item?>(_svc.CartProducts);
            }
        }

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
            NotifyPropertyChanged(nameof(Total));
            NotifyPropertyChanged(nameof(Tax));
        }

        public Item? Model { get; set; }

        public ReceiptViewModel()
        {
            Model = new Item();
        }

        public ReceiptViewModel(Item? model)
        {
            Model = model;
        }

        public string? Tax
        {
            get
            {
                
                double tax = _svc.CartProducts.Sum(p => p.Product.Price * p.Product.Quantity) * 0.07;
                return $"{tax.ToString("C2")}";
            }
        }

        public string? Total
        {
            get 
            {
                double total = _svc.CartProducts.Sum(p => p.Product.Price * p.Product.Quantity) * 1.07;
                return $"{total.ToString("C2")}"; 
            }
        }

        public void ClearCart()
        {
            _svc.ClearCart();
        }
    }
}
