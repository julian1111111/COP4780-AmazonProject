using AmazonProject.Models;
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
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        public Item? SelectedProduct { get; set; }
        public string? Query { get; set; }
        public int? AddQuantity { get; set; }
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

        public void ApplySorting()
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
        }

        public InventoryManagementViewModel()
        {
            SortOptions = new ObservableCollection<string> { "ID", "Name", "Price: Low to High", "Price: High to Low" };
            SelectedSortOption = SortOptions[0];
        }

        public Item? Add()
        {
            var newProduct = new Item();
            var item = _svc.AddOrUpdateInventory(newProduct);
            ApplySorting();
            NotifyPropertyChanged("Products");
            return item;
        }

        public Item? Delete()
        {
            var item = _svc.DeleteFromInventory(SelectedProduct?.Id ?? 0);
            NotifyPropertyChanged("Products");
            return item;
        }
    }
}
