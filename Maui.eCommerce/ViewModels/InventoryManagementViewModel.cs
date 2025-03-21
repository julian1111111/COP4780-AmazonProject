using AmazonProject.Models;
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
        public Product? SelectedProduct { get; set; }
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

        public ObservableCollection<Product?> Products
        {
            get
            {
                return new ObservableCollection<Product?>(_svc.InventoryProducts);
            }
        }

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
        }

        public Product? Add()
        {
            var newProduct = new Product();
            var item = _svc.AddOrUpdateInventory(newProduct);
            NotifyPropertyChanged("Products");
            return item;
        }

        public Product? Delete()
        {
            var item = _svc.DeleteFromInventory(SelectedProduct?.Id ?? 0);
            NotifyPropertyChanged("Products");
            return item;
        }
    }
}
