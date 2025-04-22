using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce
{
    public static class AppSettings
    {
        const string SalesTaxKey = "SalesTaxRate";
        const double DefaultTax = 0.07;

        public static double SalesTaxRate
        {
            get
            {
                return Preferences.Get(SalesTaxKey, DefaultTax);
            }
            set
            {
                Preferences.Set(SalesTaxKey, value);
            }
        }
    }
}
