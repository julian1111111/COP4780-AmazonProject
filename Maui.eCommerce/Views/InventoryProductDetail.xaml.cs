using AmazonProject.Models;
using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

[QueryProperty(nameof(ProductId), "productId")]
public partial class InventoryProductDetail : ContentPage
{
    private ProductServiceProxy _svc = ProductServiceProxy.Current;
    public int ProductId { get; set; }

    public InventoryProductDetail()
	{
		InitializeComponent();
        BindingContext = new ProductViewModel();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
        (BindingContext as ProductViewModel).Undo();
        Shell.Current.GoToAsync("//InventoryManagement");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        (BindingContext as ProductViewModel).AddOrUpdateInventory();
        Shell.Current.GoToAsync("//InventoryManagement");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (ProductId == 0)
        {
            BindingContext = new ProductViewModel();
        }
        else
        {
            BindingContext = new ProductViewModel(_svc.GetById(ProductId));
        }
    }
}