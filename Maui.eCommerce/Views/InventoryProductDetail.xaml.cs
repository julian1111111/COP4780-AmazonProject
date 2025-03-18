using AmazonProject.Models;
using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class InventoryProductDetail : ContentPage
{
    private ProductServiceProxy _svc = ProductServiceProxy.Current;

    public InventoryProductDetail()
	{
		InitializeComponent();
        BindingContext = new ProductViewModel();
	}
    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//InventoryManagement");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        var name = (BindingContext as ProductViewModel).Name;
        _svc.AddOrUpdateInventory(new Product { Name = name });
        Shell.Current.GoToAsync("//InventoryManagement");
    }
}