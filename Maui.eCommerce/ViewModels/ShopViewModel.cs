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

        public ObservableCollection<string> SortOptions { get; }

        string _selectedSortOption;
        public string SelectedSortOption
        {
            get
            {
                return _selectedSortOption;
            }
            set
            {
                if (_selectedSortOption == value) return;
                _selectedSortOption = value;
                ApplySorting();
            }
        }

        void ApplySorting()
        {
            var buffer = SelectedSortOption.ToLower() switch
            {
                "id" => _svc.InventoryProducts.OrderBy(p => p.Product.Id).ToList(),
                "name" => _svc.InventoryProducts.OrderBy(p => p.Product.Name).ToList(),
                "price: low to high" => _svc.InventoryProducts.OrderBy(p => p.Product.Price).ToList(),
                "price: high to low" => _svc.InventoryProducts.OrderByDescending(p => p.Product.Price).ToList(),
                _ => _svc.InventoryProducts.ToList()
            };
            _svc.InventoryProducts.Clear();
            foreach (var itm in buffer)
                _svc.InventoryProducts.Add(itm);
            RefreshProductList();
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
            SortOptions = new ObservableCollection<string> { "ID", "Name", "Price: Low to High", "Price: High to Low" };
            SelectedSortOption = SortOptions[0];
        }

        public ShopViewModel(Item? model)
        {
            Model = model;
            SortOptions = new ObservableCollection<string> { "ID", "Name", "Price: Low to High", "Price: High to Low" };
            SelectedSortOption = SortOptions[0];
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
