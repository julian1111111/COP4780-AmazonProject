using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        void Notify([CallerMemberName] string n = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private double _salesTax = AppSettings.SalesTaxRate;
        public double SalesTax
        {
            get
            {
                return _salesTax;
            }
            private set
            {
                if (_salesTax == value) return;
                _salesTax = value;
                AppSettings.SalesTaxRate = value;
                Notify();
            }
        }

        public void OnSalesTaxCompleted(object sender)
        {
            var entry = (Entry)sender;
            if (double.TryParse(entry.Text, out var val))
            {
                SalesTax = val;
                entry.Text = val.ToString("F2");
            }
            else
            {
                entry.Text = SalesTax.ToString("F2");
            }
        }
        public string Display
        {
            get
            {
                return "Hello, world";
            }
        }
    }
}
